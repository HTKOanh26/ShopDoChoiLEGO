namespace ShopDoChoiLEGO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("GioHang")]
    public partial class GioHang
    {
        [Key]
        [StringLength(50)]
        public string MaGioHang { get; set; }
        [StringLength(50)]
        public string MaNguoiDung { get; set; }

        [StringLength(50)]
        public string MaSanPham { get; set; }


        [StringLength(50)]
        public string TinhTrang { get; set; }

        public int? SoLuong { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
