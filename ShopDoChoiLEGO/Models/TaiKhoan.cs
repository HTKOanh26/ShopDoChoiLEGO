namespace ShopDoChoiLEGO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TaiKhoan")]
    public partial class TaiKhoan
    {
        [Key]
        [StringLength(50)]
        public string MaTaiKhoan { get; set; }
        [StringLength(50)]
        public string MaNguoiDung { get; set; }

        [StringLength(50)]
        public string LoaiTaiKhoan { get; set; }

        [Column(TypeName = "date")]
        public DateTime? NgayTao { get; set; }

        [StringLength(20)]
        public string TrangThaiTaiKhoan { get; set; }

        public virtual NguoiDung NguoiDung { get; set; }
    }
}
