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
    public class QLKhosController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLKhos
        public ActionResult TrangChu()
        {
            var kho  = db.Kho.ToList();
            return View(kho);
        }


        // GET: QLKhos/Create
        public ActionResult TaoMoi()
        {
            return View();
        }

        // POST: QLKhos/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaKho,TenKho,DiaChi,SoLuongSanPhamTrongKho")] Kho kho)
        {
            if (ModelState.IsValid)
            {
                var existingKho = db.Kho.FirstOrDefault(p => p.TenKho.Trim().ToLower() == kho.TenKho.Trim().ToLower());
                if (existingKho != null)
                {
                    ModelState.AddModelError("", "Tên kho đã tồn tại. Vui lòng chọn tên khác.");
                    return View(kho);
                }
                // Tạo số ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(1000, 9999);
                //Tạo mã sản phẩm 
                kho.MaKho = $"MK_{randomNumber}";
                db.Kho.Add(kho);
                db.SaveChanges();
                return RedirectToAction("TrangChu");
            }

            return View(kho);
        }

        // GET: QLKhos/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kho kho = db.Kho.Find(id);
            if (kho == null)
            {
                return HttpNotFound();
            }
            return View(kho);
        }

        // POST: QLKhos/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaKho,TenKho,DiaChi,SoLuongSanPhamTrongKho")] Kho kho)
        {
            if (ModelState.IsValid)
            {
                var existingKho = db.Kho.FirstOrDefault(c => c.TenKho == kho.TenKho && c.MaKho != kho.MaKho);
                if (existingKho != null)
                {
                    ModelState.AddModelError("", "Tên kho đã tồn tại. Vui lòng chọn tên khác.");
                    return View(kho);
                }
                db.Entry(kho).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("TrangChu");
            }
            return View(kho);
        }


        // GET: QLKhos/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Kho kho = db.Kho.Find(id);
            if (kho == null)
            {
                return HttpNotFound();
            }
            return View(kho);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            // Tìm bản ghi từ bảng Kho cần xóa
            Kho kho = db.Kho.Find(id);
            if (kho == null)
            {
                return HttpNotFound();
            }
            string maKho = id.ToString();
            // Tìm tất cả các sản phẩm liên quan đến Kho cần xóa
            var sanPhamIds = db.SanPham.Where(sp => sp.MaKho == maKho).Select(sp => sp.MaSanPham).ToList();

            // Tìm tất cả các chi tiết đơn hàng liên quan đến các sản phẩm thuộc Kho cần xóa
            var chiTietDonHangs = db.ChiTietDonHang.Where(ctdh => sanPhamIds.Contains(ctdh.MaSanPham)).ToList();

            // Xóa tất cả các chi tiết đơn hàng liên quan
            db.ChiTietDonHang.RemoveRange(chiTietDonHangs);

            // Xóa tất cả các sản phẩm liên quan
            db.SanPham.RemoveRange(db.SanPham.Where(sp => sp.MaKho == maKho));

            // Xóa bản ghi từ bảng Kho
            db.Kho.Remove(kho);
            db.SaveChanges();

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
