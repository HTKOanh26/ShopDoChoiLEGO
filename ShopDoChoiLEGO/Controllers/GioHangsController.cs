using ShopDoChoiLEGO.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ShopDoChoiLEGO.Controllers
{
    public class GioHangsController : Controller
    {
        Model1 db = new Model1();
        // GET: GioHangs
        public ActionResult GioHang()
        {
         
            if (Session["MaNguoiDung"] == null)
            {
    
                return RedirectToAction("DangNhap", "DangNhaps");
            }

           
            var maNguoiDung = Session["MaNguoiDung"] as string;
            var gioHang = db.GioHang.FirstOrDefault(g => g.MaNguoiDung == maNguoiDung);




            var MatHangGioHang = (from giohang in db.GioHang
                                  where giohang.MaNguoiDung == maNguoiDung 
                                  join sanpham in db.SanPham on giohang.MaSanPham equals sanpham.MaSanPham
                                  select new XemMauGioHang
                                  {
                                      MaSanPham = giohang.MaSanPham,
                                      TenSanPham = sanpham.TenSanPham,
                                      Gia = sanpham.Gia,
                                      SoLuong = giohang.SoLuong,
                                      URLHinhAnh = sanpham.URLHinhAnh,
                                  }).ToList();


            return View(MatHangGioHang);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemVaoGio(string maSanPham, int? soluong)
        {

            if (Session["MaNguoiDung"] == null)
            {
                return RedirectToAction("DangNhap", "DangNhaps");
            }

            var maND = Session["MaNguoiDung"] as string;

            var KiemTraSanPham = db.GioHang.FirstOrDefault(c => c.MaNguoiDung == maND && c.MaSanPham == maSanPham);

            if (KiemTraSanPham != null)
            {
                KiemTraSanPham.SoLuong++;
            }
            else
            {
                var ThemVaoGio = new GioHang
                {
                    MaGioHang = TaoMaGioHang(),
                    MaNguoiDung = maND,
                    MaSanPham = maSanPham,
                    SoLuong = 1,
                    TinhTrang = "InCart"
                };

                db.GioHang.Add(ThemVaoGio);
            }    

            db.SaveChanges();

            return RedirectToAction("GioHang");
        }

        [HttpPost]
        public ActionResult CapNhatSoLuong(string maSanPham, int thayDoi)
        {
          
            var maNguoiDung = Session["MaNguoiDung"] as string;

          
            var matHang = db.GioHang.FirstOrDefault(c => c.MaNguoiDung == maNguoiDung && c.MaSanPham == maSanPham && c.TinhTrang == "InCart");

            if (matHang != null)
            {
           
                matHang.SoLuong += thayDoi;

                
                if (matHang.SoLuong == 0)
                {
                    db.GioHang.Remove(matHang);
                }

                db.SaveChanges();
            }

            
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        [HttpPost]
        public ActionResult XoaSanPham(string maSanPham)
        {
       
            var maNguoiDung = Session["MaNguoiDung"] as string;

      
            var matHang = db.GioHang.FirstOrDefault(c => c.MaNguoiDung == maNguoiDung && c.MaSanPham == maSanPham && c.TinhTrang == "InCart");

            if (matHang != null)
            {
          
                db.GioHang.Remove(matHang);
                db.SaveChanges();
            }

      
            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public string TaoMaGioHang()
        {
            const string tienTo = "MGH";
            var ngauNhien = new Random();
            var soNgauNhien = ngauNhien.Next(1000, 10000); 

            var maTuyChinh = $"{tienTo}{soNgauNhien}";

            return maTuyChinh;
        }
        public ActionResult MuaNgay()
        {
            ViewBag.MaPhuongThucThanhToan = new SelectList(db.PhuongThucThanhToan, "MaPhuongThucThanhToan", "TenPhuongThucThanhToan");
            ViewBag.MaKhuyenMai = new SelectList(db.KhuyenMai, "MaKhuyenMai", "TenKhuyenMai");
            return View();
        }
        [HttpPost]
        public ActionResult MuaNgay(string maSanPham, string maPhuongThucThanhToan, string maKhuyenMai)
        {   
            var maNguoiDung = Session["MaNguoiDung"] as string;
            var kiemTraGioHang = db.GioHang.FirstOrDefault(c => c.MaNguoiDung == maNguoiDung && c.MaSanPham == maSanPham);
            int soLuong = LaySoLuongTuGioHang(maSanPham);
            ViewBag.MaPhuongThucThanhToan = new SelectList(db.PhuongThucThanhToan, "MaPhuongThucThanhToan", "TenPhuongThucThanhToan");
            ViewBag.MaKhuyenMai = new SelectList(db.KhuyenMai, "MaKhuyenMai", "TenKhuyenMai");
/*
            var khuyenMai = db.KhuyenMai.FirstOrDefault(km => km.MaKhuyenMai == maKhuyenMai);
            decimal? giamGiaPhanTram = khuyenMai != null ? khuyenMai.GiamGia : 0; // Lấy phần trăm giảm giá, nếu không có khuyến mãi thì gán mặc định là 0

            // Lấy tổng giá của sản phẩm từ giỏ hàng
            decimal? tongGia = TinhTongGia(maSanPham);

            // Tính giá sau khi giảm giá
            decimal? tongTien = tongGia - (tongGia * giamGiaPhanTram);*/

            var thanhtoan = new ThanhToan
            {
                MaThanhToan = TaoMaThanhToan(),
                MaPhuongThucThanhToan = maPhuongThucThanhToan,
                TongTien = (decimal?)TinhTongGia(maSanPham),
                MaSanPham = maSanPham,
                NgayThanhToan = DateTime.Now,
                MaKhuyenMai = maKhuyenMai,
                SoLuong = soLuong,
            };
           

            db.ThanhToan.Add(thanhtoan);
            db.SaveChanges();
 
            var taoMoiDonHang = new DonHang
            {
                MaDonHang = TaoMaDonHang(), 
                MaNguoiDung = maNguoiDung,
                MaSanPham = maSanPham,
                TongTien = (decimal?)TinhTongGia(maSanPham), 
                NgayDatHang = DateTime.Now,             
                TrangThaiDonHang = "Chờ xử lý",
                MaThanhToan = maPhuongThucThanhToan,
                SoLuong = soLuong
            };
            db.DonHang.Add(taoMoiDonHang);
            db.SaveChanges();

            var sanPhamTrongGioHang = db.GioHang.FirstOrDefault(c => c.MaNguoiDung == maNguoiDung && c.MaSanPham == maSanPham);
            if (sanPhamTrongGioHang != null)
            {
                db.GioHang.Remove(sanPhamTrongGioHang);
                db.SaveChanges();
            }


            return RedirectToAction("GioHang", "GioHangs");
        }

        //Tạo mã giỏ hàng
        public string TaoMaDonHang()
        {
            const string tienTo = "MDH";
            var ngauNhien = new Random();
            var soNgauNhien = ngauNhien.Next(1000, 10000); 

            var maTuyChinh = $"{tienTo}{soNgauNhien}";

            return maTuyChinh;
        }

        //Tạo mã thanh toán
        public string TaoMaThanhToan()
        {
            const string tienTo = "MTT";
            var ngauNhien = new Random();
            var soNgauNhien = ngauNhien.Next(1000, 10000);

            var maTuyChinh = $"{tienTo}{soNgauNhien}";

            return maTuyChinh;
        }
        private decimal TinhTongGia(string maSanPham)
        {

            var sanpham = db.SanPham.FirstOrDefault(p => p.MaSanPham == maSanPham);

            if (sanpham != null)
            {
                decimal tongGia = (decimal)(sanpham.Gia * sanpham.SoLuongTrongKho);

                return tongGia;
            }

            return 0; 
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