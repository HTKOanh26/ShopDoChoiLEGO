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
    public class QLPhuongThucThanhToansController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLPhuongThucThanhToans
        public ActionResult TrangChu()
        {
            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(db.PhuongThucThanhToan.ToList());
        }


        // GET: QLPhuongThucThanhToans/Create
        public ActionResult TaoMoi()
        {
            return View();
        }

        // POST: QLPhuongThucThanhToans/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaPhuongThucThanhToan,TenPhuongThucThanhToan")] PhuongThucThanhToan phuongThucThanhToan)
        {
            if (ModelState.IsValid)
            {
                var existingPTTT = db.PhuongThucThanhToan.FirstOrDefault(p => p.TenPhuongThucThanhToan.Trim().ToLower() == phuongThucThanhToan.TenPhuongThucThanhToan.Trim().ToLower());
                if (existingPTTT != null)
                {
                    ModelState.AddModelError("", "Tên PTTT đã tồn tại. Vui lòng chọn tên khác.");
                    return View(phuongThucThanhToan);
                }
                // Tạo số ngẫu nhiên
                Random random = new Random();
                int randomNumber = random.Next(1000, 9999);
                //Tạo mã sản phẩm 
                phuongThucThanhToan.MaPhuongThucThanhToan = $"MPTTT_{randomNumber}";
                db.PhuongThucThanhToan.Add(phuongThucThanhToan);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Thêm mới PTTT thành công.";

                return RedirectToAction("TrangChu");
            }

            return View(phuongThucThanhToan);
        }

        // GET: QLPhuongThucThanhToans/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuongThucThanhToan phuongThucThanhToan = db.PhuongThucThanhToan.Find(id);
            if (phuongThucThanhToan == null)
            {
                return HttpNotFound();
            }
            return View(phuongThucThanhToan);
        }

        // POST: QLPhuongThucThanhToans/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaPhuongThucThanhToan,TenPhuongThucThanhToan")] PhuongThucThanhToan phuongThucThanhToan)
        {
            if (ModelState.IsValid)
            {
                var existingPTTT = db.PhuongThucThanhToan.FirstOrDefault(c => c.TenPhuongThucThanhToan == phuongThucThanhToan.TenPhuongThucThanhToan);
                if (existingPTTT != null)
                {
                    ModelState.AddModelError("", "Tên PTTT đã tồn tại. Vui lòng chọn tên khác.");
                    return View(phuongThucThanhToan);
                }
                db.Entry(phuongThucThanhToan).State = EntityState.Modified;
                db.SaveChanges();
                TempData["SuccessMessage"] = "Cập nhật PTTT thành công.";

                return RedirectToAction("TrangChu");
            }
            return View(phuongThucThanhToan);
        }

        // GET: QLPhuongThucThanhToans/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            PhuongThucThanhToan phuongThucThanhToan = db.PhuongThucThanhToan.Find(id);
            if (phuongThucThanhToan == null)
            {
                return HttpNotFound();
            }
            return View(phuongThucThanhToan);
        }

        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            // Tìm bản ghi từ bảng PhuongThucThanhToan cần xóa
            PhuongThucThanhToan phuongThucThanhToan = db.PhuongThucThanhToan.Find(id);
            if (phuongThucThanhToan == null)
            {
                return HttpNotFound();
            }
            string maPTTT = id.ToString();
            // Tìm tất cả các thanh toán liên quan đến PhuongThucThanhToan cần xóa
            var thanhToans = db.ThanhToan.Where(tt => tt.MaPhuongThucThanhToan == maPTTT).ToList();

            // Xóa tất cả các thanh toán liên quan
            db.ThanhToan.RemoveRange(thanhToans);

            // Xóa bản ghi từ bảng PhuongThucThanhToan
            db.PhuongThucThanhToan.Remove(phuongThucThanhToan);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Xóa PTTT thành công.";

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
