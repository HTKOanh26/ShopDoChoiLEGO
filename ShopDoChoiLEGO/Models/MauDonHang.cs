using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopDoChoiLEGO.Models
{
    public class MauDonHang
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public string URLHinhAnh { get; set; }
        public decimal? Gia { get; set; }
        public int? SoLuong { get; set; }
        public decimal? TongTien { get; set; }
        public string TrangThaiDonHang { get; set; }
        public string MaThanhToan { get; set; }
        public DateTime? NgayDatHang { get; set; }

    }
}