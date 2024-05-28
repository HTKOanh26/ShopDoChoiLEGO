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
    public class QLLoaiSanPhamsController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLLoaiSanPhams
        public ActionResult TrangChu()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(db.LoaiSanPham.ToList());
        }
       

        // GET: QLLoaiSanPhams/Create
        public ActionResult TaoMoi()
        {
            return View();
        }

        // POST: QLLoaiSanPhams/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaLoaiSanPham,TenLoaiSanPham")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                var existingDanhMuc = db.LoaiSanPham.FirstOrDefault(p => p.TenLoaiSanPham.Trim().ToLower() == loaiSanPham.TenLoaiSanPham.Trim().ToLower());
                if (existingDanhMuc != null)
                {
                    ModelState.AddModelError("", "Tên danh mục đã tồn tại. Vui lòng chọn tên khác.");
                    return View(loaiSanPham);
                }
                // Tạo số ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(1000, 9999);
                //Tạo mã sản phẩm 
                loaiSanPham.MaLoaiSanPham = $"MLSP_{randomNumber}";
                db.LoaiSanPham.Add(loaiSanPham);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Thêm mới danh mục thành công.";


                return RedirectToAction("TrangChu");
            }

            return View(loaiSanPham);
        }


        // GET: QLLoaiSanPhams/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = db.LoaiSanPham.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // POST: QLLoaiSanPhams/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaLoaiSanPham,TenLoaiSanPham")] LoaiSanPham loaiSanPham)
        {
            if (ModelState.IsValid)
            {
                var existingDanhMuc = db.LoaiSanPham.FirstOrDefault(c => c.TenLoaiSanPham == loaiSanPham.TenLoaiSanPham);
                if (existingDanhMuc != null)
                {
                    ModelState.AddModelError("", "Tên NSX đã tồn tại. Vui lòng chọn tên khác.");
                    return View(loaiSanPham);
                }
                db.Entry(loaiSanPham).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật danh mục thành công.";

                return RedirectToAction("TrangChu");
            }
            return View(loaiSanPham);
        }

        // GET: QLLoaiSanPhams/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LoaiSanPham loaiSanPham = db.LoaiSanPham.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            return View(loaiSanPham);
        }

        // POST: QLLoaiSanPhams/Delete/5
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            LoaiSanPham loaiSanPham = db.LoaiSanPham.Find(id);
            if (loaiSanPham == null)
            {
                return HttpNotFound();
            }
            string maLSP = id.ToString();
            // Xóa các sản phẩm liên quan
            var xoaSanPham = db.SanPham.Where(c => c.MaLoaiSanPham == maLSP).ToList();
            db.SanPham.RemoveRange(xoaSanPham);

            // Xóa các đánh giá liên quan
            var xoaDanhGia = db.DanhGia.Where(i => i.MaSanPham == maLSP).ToList();
            db.DanhGia.RemoveRange(xoaDanhGia);

            // Xóa các chi tiết đơn hàng liên quan
            var xoaCTDH = db.ChiTietDonHang.Where(i => i.MaSanPham == maLSP).ToList();
            db.ChiTietDonHang.RemoveRange(xoaCTDH);

            db.LoaiSanPham.Remove(loaiSanPham);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Xóa danh mục thành công.";

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
