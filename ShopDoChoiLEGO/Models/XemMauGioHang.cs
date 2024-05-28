using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ShopDoChoiLEGO.Models
{
    public class XemMauGioHang
    {
        public string MaSanPham { get; set; }
        public string TenSanPham { get; set; }
        public decimal? Gia { get; set; }
        public int? SoLuong { get; set; }
        public string URLHinhAnh { get; set; }
        public string CoGiaTri { get; set; }
    }
}