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
    public class QLDonHangsController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLDonHangs
        public ActionResult TrangChu()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            var donHang = db.DonHang.Include(d => d.NguoiDung);
            return View(donHang.ToList());
        }

        // GET: QLDonHangs/Create
        public ActionResult TaoMoi()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap");
            return View();
        }

        // POST: QLDonHangs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaDonHang,MaNguoiDung,NgayDatHang,TrangThaiDonHang,TongTien")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra nếu ngày đặt hàng lớn hơn hoặc bằng ngày hiện tại
                if (donHang.NgayDatHang >= DateTime.Today)
                {
                    // Tạo số ngẫu nhiên
                    Random random = new Random();
                    int randomNumber = random.Next(1000, 9999);
                    //Tạo mã sản phẩm 
                    donHang.MaDonHang = $"MĐH_{randomNumber}";
                    db.DonHang.Add(donHang);
                    db.SaveChanges();

                    TempData["SuccessMessage"] = "Thêm mới đơn hàng thành công.";

                    return RedirectToAction("TrangChu");
                }
                else
                {
                    ModelState.AddModelError("", "Ngày đặt hàng không thể nhỏ hơn ngày hiện tại.");
                }
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", donHang.MaNguoiDung);
            return View(donHang);
        }


        // GET: QLDonHangs/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", donHang.MaNguoiDung);
            return View(donHang);
        }

        // POST: QLDonHangs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaDonHang,MaNguoiDung, MaSanPham, NgayDatHang,TrangThaiDonHang,TongTien, MaThanhToan, SoLuong")] DonHang donHang)
        {
            if (ModelState.IsValid)
            {
                db.Entry(donHang).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật đơn hàng thành công.";

                return RedirectToAction("TrangChu");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", donHang.MaNguoiDung);
            return View(donHang);
        }

        // GET: QLDonHangs/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            return View(donHang);
        }

        // POST: QLDonHangs/Delete/5
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            DonHang donHang = db.DonHang.Find(id);
            if (donHang == null)
            {
                return HttpNotFound();
            }
            string maDH = id.ToString();
            // Xóa các tahnh toán liên quan
           /* var xoaThanhToan = db.ThanhToan.Where(c => c.MaDonHang == maDH).ToList();
            db.ThanhToan.RemoveRange(xoaThanhToan);*/
            db.DonHang.Remove(donHang);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa đơn hàng thành công.";

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
