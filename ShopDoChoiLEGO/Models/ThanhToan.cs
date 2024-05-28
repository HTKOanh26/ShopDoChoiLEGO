namespace ShopDoChoiLEGO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("ThanhToan")]
    public partial class ThanhToan
    {
        [Key]
        [StringLength(50)]
        public string MaThanhToan { get; set; }

        [StringLength(50)]
        public string MaPhuongThucThanhToan { get; set; }

        [StringLength(50)]
        public string MaSanPham { get; set; }

        public int SoLuong { get; set; }

        public decimal? TongTien { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayThanhToan { get; set; }
        [StringLength(50)]
        public string MaKhuyenMai { get; set; }

        //public virtual DonHang DonHang { get; set; }

        public virtual PhuongThucThanhToan PhuongThucThanhToan { get; set; }
    }
}
