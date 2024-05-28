using ShopDoChoiLEGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopDoChoiLEGO.Controllers
{
    public class DonHangsController : Controller
    {
        Model1 db = new Model1();
        // GET: DonHangs
        public ActionResult DonHang(string maSanPham)
        {
            if (Session["MaNguoiDung"] == null)
            {

                return RedirectToAction("DangNhap", "DangNhaps");
            }


            var maNguoiDung = Session["MaNguoiDung"] as string;
            var donHang = db.DonHang.FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);



            var MatHangDonHang = (from donhang in db.DonHang
                                  where donhang.MaNguoiDung == maNguoiDung
                                  join sanpham in db.SanPham on donhang.MaSanPham equals sanpham.MaSanPham
                                  select new MauDonHang
                                  {
                                      MaSanPham = donhang.MaSanPham,
                                      TenSanPham = sanpham.TenSanPham,
                                      Gia = sanpham.Gia,
                                      SoLuong = donhang.SoLuong,
                                      TongTien = donhang.TongTien,
                                      URLHinhAnh = sanpham.URLHinhAnh,
                                      TrangThaiDonHang = donhang.TrangThaiDonHang,
                                      MaThanhToan = donhang.MaThanhToan,
                                      NgayDatHang = donhang.NgayDatHang,
                                  }).ToList();


            return View(MatHangDonHang);
        }

    }
}