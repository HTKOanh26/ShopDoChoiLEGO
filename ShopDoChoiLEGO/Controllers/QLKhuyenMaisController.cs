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
    public class QLKhuyenMaisController : Controller
    {
        private Model1 db = new Model1();

        // GET: QLKhuyenMais
        public ActionResult TrangChu()
        {

            ViewBag.SuccessMessage = TempData["SuccessMessage"];

            return View(db.KhuyenMai.ToList());
        }

      

        // GET: QLKhuyenMais/Create
        public ActionResult TaoMoi()
        {
            return View();
        }

        // POST: QLKhuyenMais/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TaoMoi([Bind(Include = "MaKhuyenMai,TenKhuyenMai,MoTa,NgayBatDau,NgayKetThuc,GiamGia")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có khuyến mãi nào khác có cùng tên không
                if (!db.KhuyenMai.Any(km => km.TenKhuyenMai == khuyenMai.TenKhuyenMai))
                {
                    // Kiểm tra ngày bắt đầu và ngày kết thúc
                    if (khuyenMai.NgayBatDau > khuyenMai.NgayKetThuc)
                    {
                        ModelState.AddModelError("", "Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
                        return View(khuyenMai);
                    }
                    // Tạo số ngẫu nhiên
                    Random random = new Random();
                    int randomNumber = random.Next(1000, 9999);
                    //Tạo mã sản phẩm 
                    khuyenMai.MaKhuyenMai = $"MKM_{randomNumber}";
                    db.KhuyenMai.Add(khuyenMai);
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Thêm mới khuyến mãi thành công.";

                    return RedirectToAction("TrangChu");
                }
                else
                {
                    ModelState.AddModelError("", "Tên khuyến mãi đã tồn tại. Vui lòng chọn tên khác.");
                }
            }

            return View(khuyenMai);
        }


        // GET: QLKhuyenMais/Edit/5
        public ActionResult ChinhSua(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMai.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // POST: QLKhuyenMais/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChinhSua([Bind(Include = "MaKhuyenMai,TenKhuyenMai,MoTa,NgayBatDau,NgayKetThuc,GiamGia")] KhuyenMai khuyenMai)
        {
            if (ModelState.IsValid)
            {
                // Kiểm tra xem có khuyến mãi nào khác có cùng tên không, nhưng không tính khuyến mãi hiện tại
                if (!db.KhuyenMai.Any(km => km.TenKhuyenMai == khuyenMai.TenKhuyenMai && km.MaKhuyenMai != khuyenMai.MaKhuyenMai))
                {
                    // Kiểm tra ngày bắt đầu và ngày kết thúc
                    if (khuyenMai.NgayBatDau > khuyenMai.NgayKetThuc)
                    {
                        ModelState.AddModelError("", "Ngày bắt đầu không thể lớn hơn ngày kết thúc.");
                        return View(khuyenMai);
                    }

                    db.Entry(khuyenMai).State = EntityState.Modified;
                    db.SaveChanges();
                    TempData["SuccessMessage"] = "Cập nhật khuyến mãi thành công.";

                    return RedirectToAction("TrangChu");
                }
                else
                {
                    ModelState.AddModelError("", "Tên khuyến mãi đã tồn tại. Vui lòng chọn tên khác.");
                }
            }
            return View(khuyenMai);
        }


        // GET: QLKhuyenMais/Delete/5
        public ActionResult Xoa(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            KhuyenMai khuyenMai = db.KhuyenMai.Find(id);
            if (khuyenMai == null)
            {
                return HttpNotFound();
            }
            return View(khuyenMai);
        }

        // POST: QLKhuyenMais/Delete/5
        [HttpPost, ActionName("Xoa")]
        [ValidateAntiForgeryToken]
        public ActionResult XacNhanXoa(string id)
        {
            KhuyenMai khuyenMai = db.KhuyenMai.Find(id);
            db.KhuyenMai.Remove(khuyenMai);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Xóa khuyến mãi thành công.";

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
