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
    public class QLDanhGiasController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLDanhGias
        public ActionResult TrangChu()
        {

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            var danhGia = db.DanhGia.Include(d => d.NguoiDung).Include(d => d.SanPham);
            return View(danhGia.ToList());
        }

    

        // GET: QLDanhGias/Create
        public ActionResult TaoMoi()
        {
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap");
            ViewBag.MaSanPham = new SelectList(db.SanPham, "MaSanPham", "TenSanPham");
            return View();
        }

        // POST: QLDanhGias/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaDanhGia,MaSanPham,MaNguoiDung,DiemDanhGia,NoiDungDanhGia,NgayDanhGia")] DanhGia danhGia)
        {
            if (ModelState.IsValid)
            {
                // Tạo số ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(1000, 9999);
                //Tạo mã sản phẩm 
                danhGia.MaDanhGia = $"MĐG_{randomNumber}";
                db.DanhGia.Add(danhGia);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Thêm mới đánh giá thành công.";

                return RedirectToAction("TrangChu");
            }

            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", danhGia.MaNguoiDung);
            ViewBag.MaSanPham = new SelectList(db.SanPham, "MaSanPham", "TenSanPham", danhGia.MaSanPham);
            return View(danhGia);
        }

        // GET: QLDanhGias/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhGia danhGia = db.DanhGia.Find(id);
            if (danhGia == null)
            {
                return HttpNotFound();
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", danhGia.MaNguoiDung);
            ViewBag.MaSanPham = new SelectList(db.SanPham, "MaSanPham", "TenSanPham", danhGia.MaSanPham);
            return View(danhGia);
        }

        // POST: QLDanhGias/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaDanhGia,MaSanPham,MaNguoiDung,DiemDanhGia,NoiDungDanhGia,NgayDanhGia")] DanhGia danhGia)
        {
            if (ModelState.IsValid)
            {
                db.Entry(danhGia).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật đánh giá thành công.";

                return RedirectToAction("TrangChu");
            }
            ViewBag.MaNguoiDung = new SelectList(db.NguoiDung, "MaNguoiDung", "TenDangNhap", danhGia.MaNguoiDung);
            ViewBag.MaSanPham = new SelectList(db.SanPham, "MaSanPham", "TenSanPham", danhGia.MaSanPham);
            return View(danhGia);
        }

        // GET: QLDanhGias/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            DanhGia danhGia = db.DanhGia.Find(id);
            if (danhGia == null)
            {
                return HttpNotFound();
            }
            return View(danhGia);
        }

        // POST: QLDanhGias/Delete/5
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            DanhGia danhGia = db.DanhGia.Find(id);
            db.DanhGia.Remove(danhGia);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Xóa đánh giá thành công.";

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
