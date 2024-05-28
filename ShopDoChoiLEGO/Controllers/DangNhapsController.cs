using ShopDoChoiLEGO.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;

namespace ShopDoChoiLEGO.Controllers
{
    public class DangNhapsController : Controller
    {
        Model1 db = new Model1();
        // GET: DangNhaps
        public ActionResult DangNhap()
        {
            return View();
        }

        [HttpPost]
        public ActionResult DangNhap(NguoiDung nguoiDung)
        {
            var nd = db.NguoiDung.FirstOrDefault(u => u.TenDangNhap == nguoiDung.TenDangNhap && u.MatKhau == nguoiDung.MatKhau);
            if (nd != null)
            {
                Session["MaNguoiDung"] = nd.MaNguoiDung;
                Session["HoTen"] = nd.HoTen;
                if (nd.LoaiTaiKhoan == "Quản trị viên")
                {
                    
                    return RedirectToAction("DoanhSo", "QLDoanhSos");
                }
                else if (nd.LoaiTaiKhoan == "Khách hàng")
                {
                    return RedirectToAction("TrangChinh", "TrangChinhs");
                }
                else
                {
                    ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu chưa đúng";
                    return View();
                }
            }
            else
            {
                ViewBag.ThongBao = "Tên đăng nhập hoặc mật khẩu chưa đúng";
                return View();
            }
        }

        [HttpGet]
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DangKy(NguoiDung nguoiDung, string xacNhanMatKhau)
        {
            if (nguoiDung.MatKhau != xacNhanMatKhau)
            {
                ViewBag.ThongBao = "Mật khẩu và xác nhận mật khẩu không khớp.";
                return View(nguoiDung);
            }

            var KiemTraNguoiDung = db.NguoiDung.FirstOrDefault(u => u.TenDangNhap == nguoiDung.TenDangNhap);
            if (KiemTraNguoiDung != null)
            {
                ViewBag.ThongBao = "Tên đăng nhập đã tồn tại. Vui lòng chọn tên đăng nhập khác.";
                return View(nguoiDung);
            }

            KiemTraNguoiDung = db.NguoiDung.FirstOrDefault(u => u.Email == nguoiDung.Email);
            if (KiemTraNguoiDung != null)
            {
                ViewBag.ThongBao = "Email đã tồn tại. Vui lòng sử dụng một Email khác.";
                return View(nguoiDung);
            }
            nguoiDung.MaNguoiDung = TaoMaNguoiDung();
            nguoiDung.LoaiTaiKhoan = "Khách hàng";
            nguoiDung.NgayDangKy = DateTime.Now;
            db.NguoiDung.Add(nguoiDung);
            db.SaveChanges();
            return RedirectToAction("DangNhap");
        }

        public ActionResult QuenMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult QuenMatKhau(string email, string tenNguoiDung, string soDienThoai)
        {
            var user = db.NguoiDung.FirstOrDefault(n => n.Email == email);
            if (user != null)
            {
                if (user.TenDangNhap == tenNguoiDung && user.SoDienThoai == soDienThoai)
                {
                    ViewBag.TenNguoiDung = user.TenDangNhap;
                    ViewBag.SoDienThoai = user.SoDienThoai;
                    ViewBag.MatKhau = user.MatKhau;
                    return View();
                }
                else
                {
                    ViewBag.Email = email;
                    ViewBag.ThongBao = "Thông tin xác nhận không đúng";
                    return View();
                }
            }
            else
            {
                ViewBag.ThongBao = "Email không tồn tại trong hệ thống";
                return View();
            }
        }




        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(DoiMatKhau doimatkhau)
        {
            if (ModelState.IsValid)
            {

                var maND = Session["MaNguoiDung"] as string;


                if (KiemTraMatKhauCu(doimatkhau.MatKhauCu))
                {

                    if (doimatkhau.MatKhauMoi != doimatkhau.XacNhanMatKhau)
                    {

                        ModelState.AddModelError("XacNhanMatKhau", "Xác nhận mật khẩu không khớp với mật khẩu mới.");
                    }
                    else
                    {

                        var nd = db.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == maND);

                        if (nd != null)
                        {
                            doimatkhau.MatKhauMoi = doimatkhau.MatKhauMoi;

                            db.SaveChanges();


                            ViewBag.ThongBao = "Đổi mật khẩu thành công!";
                        }
                        else
                        {
                            ViewBag.ThongBao = "Không tìm thấy người dùng.";
                        }
                    }
                }
                else
                {
                    ModelState.AddModelError("MatKhauCu", "Mật khẩu cũ không đúng");
                }
            }
            return View(doimatkhau);
        }
        [HttpPost]
        public ActionResult DangXuat()
        {
            var maNguoiDung = Session["MaNguoiDung"] as string;

            // Truy vấn người dùng từ CSDL dựa trên mã người dùng
            var nguoiDung = db.NguoiDung.FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);

            if (nguoiDung != null)
            {
                if (nguoiDung.LoaiTaiKhoan == "Quản trị viên")
                {
                    return RedirectToAction("DangNhap", "DangNhaps");
                }
                else if (nguoiDung.LoaiTaiKhoan == "Khách hàng")
                {

                    return RedirectToAction("TrangChinh", "TrangChinhs");
                }
            }

            return RedirectToAction("Index", "Home");
        }





        public string TaoMaNguoiDung()
        {
            const string tienTo = "MND";
            var ngauNhien = new Random();
            var soNgauNhien = ngauNhien.Next(1000, 10000);

            var maTuyChinh = $"{tienTo}{soNgauNhien}";

            return maTuyChinh;
        }


        public string RandomMK()
        {
            int Numrd;
            string Numrd_str;
            Random rd = new Random();
            Numrd_str = rd.Next(100000, 1000000).ToString();
            return Numrd_str;
        }

        public string chuyenMD5(string matkhau)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(matkhau);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new StringBuilder();
            for(int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }    
            return sb.ToString();
        }

        private bool KiemTraMatKhauCu(string MatKhauCu)
        {
            var maND = Session["MaNguoiDung"] as string;
            if (maND != null)
            {
                var nd = db.NguoiDung.FirstOrDefault(u => u.MaNguoiDung == maND);

                if (nd != null && MatKhauCu == nd.MatKhau)
                {
                    return true;
                }
            }

            return false;
        }

    }
}