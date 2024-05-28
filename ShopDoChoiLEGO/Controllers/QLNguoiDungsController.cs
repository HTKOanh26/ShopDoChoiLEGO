using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ShopDoChoiLEGO.Models;

namespace ShopDoChoiLEGO.Controllers
{
    public class QLNguoiDungsController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLNguoiDungs
        public ActionResult TrangChu()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(db.NguoiDung.ToList());
        }

        // GET: QLNguoiDungs/Create
        public ActionResult TaoMoi()
        {
            return View();
        }

        // POST: QLNguoiDungs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaNguoiDung,TenDangNhap,MatKhau,HoTen,Email,DiaChiGiaoHang,SoDienThoai,LoaiTaiKhoan, NgayDangKy")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem đã tồn tại người dùng với email đã nhập chưa
                var existingNguoiDungByEmail = db.NguoiDung.FirstOrDefault(p => p.Email.Trim().ToLower() == nguoiDung.Email.Trim().ToLower());
                if (existingNguoiDungByEmail != null)
                {
                    ModelState.AddModelError("", "Email đã được sử dụng bởi người dùng khác. Vui lòng chọn email khác.");
                    return View(nguoiDung);
                }

                // Kiểm tra xem đã tồn tại người dùng với tên đăng nhập đã nhập chưa
                var existingNguoiDungByTenDangNhap = db.NguoiDung.FirstOrDefault(p => p.TenDangNhap.Trim().ToLower() == nguoiDung.TenDangNhap.Trim().ToLower());
                if (existingNguoiDungByTenDangNhap != null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                    return View(nguoiDung);
                }
                // Tạo số ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(1000, 9999);
                //Tạo mã sản phẩm 
                nguoiDung.MaNguoiDung = $"MND_{randomNumber}";
                // Nếu không có trùng tên đăng nhập hoặc email, tiến hành thêm người dùng vào cơ sở dữ liệu
                db.NguoiDung.Add(nguoiDung);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Thêm mới người dùng thành công.";


                return RedirectToAction("TrangChu");
            }

            return View(nguoiDung);
        }


        // GET: QLNguoiDungs/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDung.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }

        // POST: QLNguoiDungs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaNguoiDung,TenDangNhap,MatKhau,HoTen,Email,DiaChiGiaoHang,SoDienThoai, LoaiTaiKhoan, NgayDangKy")] NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem đã tồn tại người dùng với email đã nhập chưa
                var existingNguoiDungByEmail = db.NguoiDung.FirstOrDefault(p => p.Email.Trim().ToLower() == nguoiDung.Email.Trim().ToLower() && p.MaNguoiDung != nguoiDung.MaNguoiDung);
                if (existingNguoiDungByEmail != null)
                {
                    ModelState.AddModelError("", "Email đã được sử dụng bởi người dùng khác. Vui lòng chọn email khác.");
                    return View(nguoiDung);
                }

                // Kiểm tra xem đã tồn tại người dùng với tên đăng nhập đã nhập chưa
                var existingNguoiDungByTenDangNhap = db.NguoiDung.FirstOrDefault(p => p.TenDangNhap.Trim().ToLower() == nguoiDung.TenDangNhap.Trim().ToLower() && p.MaNguoiDung != nguoiDung.MaNguoiDung);
                if (existingNguoiDungByTenDangNhap != null)
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại. Vui lòng chọn tên khác.");
                    return View(nguoiDung);
                }

                db.Entry(nguoiDung).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật người dùng thành công.";

                return RedirectToAction("TrangChu");
            }
            return View(nguoiDung);
        }


        // GET: QLNguoiDungs/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NguoiDung nguoiDung = db.NguoiDung.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }
            return View(nguoiDung);
        }
        // POST: QLNguoiDungs/Delete/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            // Tìm người dùng cần xóa
            NguoiDung nguoiDung = db.NguoiDung.Find(id);
            if (nguoiDung == null)
            {
                return HttpNotFound();
            }

            string maNguoiDung = id.ToString();
            // Tìm và xóa tất cả các bản ghi trong bảng GioHang liên quan đến người dùng
            var gioHangs = db.GioHang.Where(gh => gh.MaNguoiDung == maNguoiDung).ToList();
            db.GioHang.RemoveRange(gioHangs);

            // Tìm và xóa tất cả các tài khoản liên quan đến người dùng
            var taiKhoans = db.TaiKhoan.Where(tk => tk.MaNguoiDung == maNguoiDung).ToList();
            db.TaiKhoan.RemoveRange(taiKhoans);

            // Tìm và xóa tất cả các đơn hàng liên quan đến người dùng
            var donHangs = db.DonHang.Where(dh => dh.MaNguoiDung == maNguoiDung).ToList();
            db.DonHang.RemoveRange(donHangs);



            // Tìm và xóa tất cả các đơn hàng liên quan đến người dùng
            var danhGias = db.DanhGia.Where(dg => dg.MaNguoiDung == maNguoiDung).ToList();
            db.DanhGia.RemoveRange(danhGias);


            // Xóa người dùng
            db.NguoiDung.Remove(nguoiDung);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa người dùng thành công.";

            return RedirectToAction("TrangChu");
        }




        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
