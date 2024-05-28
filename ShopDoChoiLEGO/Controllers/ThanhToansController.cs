using ShopDoChoiLEGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopDoChoiLEGO.Controllers
{
    public class ThanhToansController : Controller
    {
        Model1 db = new Model1();
        public ActionResult ThanhToan()
        {
            ViewBag.MaKhuyenMai = new SelectList(db.KhuyenMai, "MaKhuyenMai", "TenKhuyenMai");

            ViewBag.MaPhuongThucThanhToan = new SelectList(db.PhuongThucThanhToan, "MaPhuongThucThanhToan", "TenPhuongThucThanhToan");
            return View();
        }
        [HttpPost]
        public ActionResult ThanhToan(string maSanPham)
        {
            var sanPham = db.SanPham.FirstOrDefault(sp => sp.MaSanPham == maSanPham);
            
            if (sanPham == null)
            {
                return View("ProductNotFound");
            }
            int soLuong = LaySoLuongTuGioHang(maSanPham);
            decimal? gia = sanPham.Gia;
            var thanhToan = new MauThanhToan
            {
                MaSanPham = maSanPham,
                TenSanPham = sanPham.TenSanPham,
                SoLuong = soLuong,  
                Gia = gia
            };
            ViewBag.MaKhuyenMai = new SelectList(db.KhuyenMai, "MaKhuyenMai", "TenKhuyenMai");
            ViewBag.MaPhuongThucThanhToan = new SelectList(db.PhuongThucThanhToan, "MaPhuongThucThanhToan", "TenPhuongThucThanhToan");
            return View(thanhToan);


        }

        private int LaySoLuongTuGioHang(string maSanPham)
        {
            var maNguoiDung = Session["MaNguoiDung"] as string;
            var gioHang = db.GioHang.FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (gioHang == null)
            {
                return 0;
            }

            var item = db.GioHang.FirstOrDefault(x => x.MaSanPham == maSanPham && x.MaNguoiDung == maNguoiDung);

            if (item != null)
            {
                return (int)item.SoLuong;
            }

            return 0;
        }

    }
}