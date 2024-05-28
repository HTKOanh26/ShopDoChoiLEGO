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
    public class QLNhaSanXuatsController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLNhaSanXuats
        public ActionResult TrangChu()
        {

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(db.NhaSanXuat.ToList());
        }


        // GET: QLNhaSanXuats/Create
        public ActionResult TaoMoi()
        {
            return View();
        }

        // POST: QLNhaSanXuats/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaNhaSanXuat,TenNhaSanXuat,QuocGia,DiaChi")] NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                var existingNSX = db.NhaSanXuat.FirstOrDefault(p => p.TenNhaSanXuat.Trim().ToLower() == nhaSanXuat.TenNhaSanXuat.Trim().ToLower());
                if (existingNSX != null)
                {
                    ModelState.AddModelError("", "Tên NSX đã tồn tại. Vui lòng chọn tên khác.");
                    return View(nhaSanXuat);
                }
                // Tạo số ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(1000, 9999);
                //Tạo mã sản phẩm 
                nhaSanXuat.MaNhaSanXuat = $"MNSX_{randomNumber}";
                db.NhaSanXuat.Add(nhaSanXuat);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Thêm mới nhà sản xuất thành công.";

                return RedirectToAction("TrangChu");
            }

            return View(nhaSanXuat);
        }

        // GET: QLNhaSanXuats/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuat.Find(id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            return View(nhaSanXuat);
        }

        // POST: QLNhaSanXuats/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaNhaSanXuat,TenNhaSanXuat,QuocGia,DiaChi")] NhaSanXuat nhaSanXuat)
        {
            if (ModelState.IsValid)
            {
                var existingNSX = db.NhaSanXuat.FirstOrDefault(p => p.TenNhaSanXuat.Trim().ToLower() == nhaSanXuat.TenNhaSanXuat.Trim().ToLower() && p.MaNhaSanXuat != nhaSanXuat.MaNhaSanXuat);
                if (existingNSX != null)
                {
                    ModelState.AddModelError("", "Tên NSX đã tồn tại. Vui lòng chọn tên khác.");
                    return View(nhaSanXuat);
                }
                db.Entry(nhaSanXuat).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật nhà sản xuất thành công.";

                return RedirectToAction("TrangChu");
            }
            return View(nhaSanXuat);
        }


        // GET: QLNhaSanXuats/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NhaSanXuat nhaSanXuat = db.NhaSanXuat.Find(id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            return View(nhaSanXuat);
        }

        // POST: QLNhaSanXuats/Delete/5
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            NhaSanXuat nhaSanXuat = db.NhaSanXuat.Find(id);
            if (nhaSanXuat == null)
            {
                return HttpNotFound();
            }
            string maNSX = id.ToString();
            // Xóa các sản phẩm liên quan
            var xoaSanPham = db.SanPham.Where(c => c.MaNhaSanXuat == maNSX).ToList();
            db.SanPham.RemoveRange(xoaSanPham);

            // Xóa các đánh giá liên quan
            var xoaDanhGia = db.DanhGia.Where(i => i.MaSanPham == maNSX).ToList();
            db.DanhGia.RemoveRange(xoaDanhGia);

            // Xóa các chi tiết đơn hàng liên quan
            var xoaCTDH = db.ChiTietDonHang.Where(o => o.MaSanPham == maNSX).ToList();
            db.ChiTietDonHang.RemoveRange(xoaCTDH);
            db.NhaSanXuat.Remove(nhaSanXuat);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Xóa nhà sản xuất thành công.";

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
