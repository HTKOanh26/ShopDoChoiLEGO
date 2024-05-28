using ShopDoChoiLEGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopDoChoiLEGO.Controllers
{
    public class QLDoanhSosController : Controller
    {

        private Model1 db = new Model1();
        // GET: QLDoanhSos
        public ActionResult DoanhSo()
        {

            if (Session["MaNguoiDung"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhaps");
            }
            decimal? TongDoanhThu = db.DonHang.Sum(o => o.TongTien);
            int TongSoHangDaBan = db.DonHang.Count();
            var donHang = db.DonHang.ToList();

            ViewBag.TongDoanhThu = TongDoanhThu ?? 0;
            ViewBag.TongSoHangDaBan = TongSoHangDaBan != null ? TongSoHangDaBan : 0;

            return View(donHang);
        }

        public ActionResult XoaDonHang()
        {
            return View();
        }
        [HttpPost]
        public ActionResult XoaDonHang(string MaDonHang)
        {
            if (MaDonHang == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }

            // Tìm hóa đơn cần xóa trong cơ sở dữ liệu
            var donHang = db.DonHang.Find(MaDonHang);

            if (donHang == null)
            {
                return HttpNotFound();
            }

            // Xóa hóa đơn khỏi cơ sở dữ liệu
            db.DonHang.Remove(donHang);
            db.SaveChanges();



            // Chuyển hướng về trang danh sách hóa đơn (hoặc trang tương tự)
            return RedirectToAction("DoanhSo");
        }
    }
}