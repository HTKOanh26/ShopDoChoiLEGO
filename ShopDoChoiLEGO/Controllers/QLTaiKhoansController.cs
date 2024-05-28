using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using ShopDoChoiLEGO.Models;

namespace ShopDoChoiLEGO.Controllers
{
    public class QLTaiKhoansController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLTaiKhoans
        public ActionResult TrangChu()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            var taiKhoan = db.TaiKhoan.Include(t => t.NguoiDung);
            return View(taiKhoan.ToList());
        }


        // GET: QLTaiKhoans/Create
        public ActionResult TaoMoi()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap");
            return View();
        }

        // POST: QLTaiKhoans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaTaiKhoan,MaNguoiDung,LoaiTaiKhoan,NgayTao,TrangThaiTaiKhoan")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có người dùng nào khác đã sử dụng tên đăng nhập này chưa
                if (!db.TaiKhoan.Any(tk => tk.MaNguoiDung == taiKhoan.MaNguoiDung))
                {
                    // Tạo số ngẫu nhiên
                    Random random = new Random();
                    int randomNumber = random.Next(1000, 9999);
                    //Tạo mã sản phẩm 
                    taiKhoan.MaTaiKhoan = $"MTK_{randomNumber}";
                    db.TaiKhoan.Add(taiKhoan);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Thêm mới tài khoản thành công.";

                    return RedirectToAction("TrangChu");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã được sử dụng cho một tài khoản khác.");
                }
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", taiKhoan.MaNguoiDung);
            return View(taiKhoan);
        }


        // GET: QLTaiKhoans/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", taiKhoan.MaNguoiDung);
            return View(taiKhoan);
        }

        // POST: QLTaiKhoans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaTaiKhoan,MaNguoiDung,LoaiTaiKhoan,NgayTao,TrangThaiTaiKhoan")] TaiKhoan taiKhoan)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có tài khoản khác nào sử dụng tên đăng nhập này không
                if (!db.TaiKhoan.Any(tk => tk.MaNguoiDung == taiKhoan.MaNguoiDung && tk.MaTaiKhoan != taiKhoan.MaTaiKhoan))
                {
                    db.Entry(taiKhoan).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Cập nhật tài khoản thành công.";

                    return RedirectToAction("TrangChu");
                }
                else
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã được sử dụng cho một tài khoản khác.");
                }
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", taiKhoan.MaNguoiDung);
            return View(taiKhoan);
        }

        // GET: QLTaiKhoans/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            if (taiKhoan == null)
            {
                return HttpNotFound();
            }
            return View(taiKhoan);
        }

        // POST: QLTaiKhoans/Delete/5
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            TaiKhoan taiKhoan = db.TaiKhoan.Find(id);
            db.TaiKhoan.Remove(taiKhoan);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Xóa tài khoản thành công.";

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
