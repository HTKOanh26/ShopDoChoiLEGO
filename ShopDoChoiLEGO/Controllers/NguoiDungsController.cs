using ShopDoChoiLEGO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopDoChoiLEGO.Controllers
{
    public class NguoiDungsController : Controller
    {
        Model1 db =new Model1();
        // GET: NguoiDungs
        public ActionResult ThongTinNguoiDung(string id)
        {
            var maNguoiDung = Session["MaNguoiDung"] as string;

            if (string.IsNullOrEmpty(maNguoiDung))
            {
                // Xử lý trường hợp không có mã người dùng trong session
                return RedirectToAction("DangNhap", "DangNhaps"); // Giả sử bạn có trang đăng nhập
            }

            var nguoiDung = db.NguoiDung.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);

            if (nguoiDung == null)
            {
                // Xử lý trường hợp không tìm thấy người dùng
                return HttpNotFound();
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];


            return View(nguoiDung);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThongTinNguoiDung(NguoiDung nguoiDung)
        {
            if (ModelState.IsValid)
            {
                var maNguoiDung = Session["MaNguoiDung"] as string;

                if (string.IsNullOrEmpty(maNguoiDung))
                {
                    // Xử lý trường hợp không có mã người dùng trong session
                    return RedirectToAction("DangNhap", "DangNhaps"); // Chuyển hướng đến trang đăng nhập
                }

                var nguoiDungDB = db.NguoiDung.FirstOrDefault(nd => nd.MaNguoiDung == maNguoiDung);

                if (nguoiDungDB == null)
                {
                    // Xử lý trường hợp không tìm thấy người dùng
                    return HttpNotFound();
                }

                // Cập nhật thông tin người dùng từ form chỉnh sửa
                nguoiDungDB.HoTen = nguoiDung.HoTen;
                nguoiDungDB.SoDienThoai = nguoiDung.SoDienThoai;
                nguoiDungDB.Email = nguoiDung.Email;
                nguoiDungDB.DiaChiGiaoHang = nguoiDung.DiaChiGiaoHang;

                // Lưu thay đổi vào CSDL
                db.SaveChanges();

                TempData["SuccessMessage"] = "Cập nhật thành công.";


                // Chuyển hướng sau khi chỉnh sửa thành công
                return RedirectToAction("ThongTinNguoiDung", "NguoiDungs");
            }

            // Nếu dữ liệu không hợp lệ, hiển thị lại form chỉnh sửa
            return View(nguoiDung);
        }

    }
}