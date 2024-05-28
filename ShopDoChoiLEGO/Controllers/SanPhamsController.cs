using ShopDoChoiLEGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ShopDoChoiLEGO.Controllers
{
    public class SanPhamsController : Controller
    {
        Model1 db = new Model1();

        // GET: Product
        public ActionResult TrangChu()
        {
            var taiKhoan = db.SanPham.ToList();
            return View(taiKhoan);
        }
        public ActionResult ChiTietSanPham(string maSanPham)
        {
            var sanpham = db.SanPham.FirstOrDefault(o => o.MaSanPham == maSanPham);

            if(sanpham == null)
            {
                return HttpNotFound();
            }

            return View(sanpham);
        }
        [HttpGet]
        public ActionResult TimKiem(string timkiemTen)
        {
            if(string.IsNullOrEmpty(timkiemTen))
            {
                return RedirectToAction("TrangChinh", "TrangChinhs");
            }    

            var KetQuaTimKiem = db.SanPham.Where(p => p.TenSanPham.ToLower().Contains(timkiemTen.ToLower())).ToList();

            return View(KetQuaTimKiem);
        }
    }
}