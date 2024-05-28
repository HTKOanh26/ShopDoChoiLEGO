using ShopDoChoiLEGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;


namespace ShopDoChoiLEGO.Controllers
{
    public class TrangChinhsController : Controller
    {
        Model1 db = new Model1();
        // GET: TrangChinhs
        public ActionResult TrangChinh()
        {
            if (Session["MaNguoiDung"] != null)
            {
                var maNguoiDung = Session["MaNguoiDung"] as string;
                var nguoiDung = db.NguoiDung.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);
                ViewBag.TenNguoiDung = nguoiDung.HoTen;
            }
            var sanpham = db.SanPham.Include(s => s.LoaiSanPham).ToList();
            return View(sanpham);
        }

    }
}