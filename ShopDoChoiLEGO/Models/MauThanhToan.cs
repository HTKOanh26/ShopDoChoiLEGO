using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopDoChoiLEGO.Models
{
    public class MauThanhToan
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal? Gia { get; set; }
        public decimal? TongTien { get; set; }
        public int? SoLuong { get; set; }
        public string MaPhuongThucThanhToan { get; set; }
        public string TenPhuongThucThanhToan { get; set; }
        public string MaKhuyenMai { get; set; }
        public string TenKhuyenMai { get; set; }

    }
}