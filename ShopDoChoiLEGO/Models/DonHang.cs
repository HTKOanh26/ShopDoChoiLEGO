namespace ShopDoChoiLEGO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("DonHang")]
    public partial class DonHang
    {

        [Key]
        [StringLength(50)]
        public string MaDonHang { get; set; }
        [StringLength(50)]
        public string MaNguoiDung { get; set; }
        [StringLength(50)]
        public string MaSanPham { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayDatHang { get; set; }

        [StringLength(50)]
        public string TrangThaiDonHang { get; set; }

        public int? SoLuong { get; set; }

        public decimal? TongTien { get; set; }

        [StringLength(50)]
        public string MaThanhToan { get; set; }

        
        public virtual NguoiDung NguoiDung { get; set; }

    }
}
