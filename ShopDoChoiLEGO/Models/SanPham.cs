namespace ShopDoChoiLEGO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("SanPham")]
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            ChiTietDonHang = new HashSet<ChiTietDonHang>();
            DanhGia = new HashSet<DanhGia>();
        }

        [Key]
        [StringLength(50)]
        public string MaSanPham { get; set; }

        [StringLength(255)]
        public string TenSanPham { get; set; }

        public string MoTa { get; set; }

        public decimal? Gia { get; set; }

        [StringLength(255)]
        public string URLHinhAnh { get; set; }

        public int? SoLuongTrongKho { get; set; }

        public DateTime? NgayThemVao { get; set; }

        [StringLength(20)]
        public string TrangThai { get; set; }
        [StringLength(50)]
        public string MaLoaiSanPham { get; set; }
        [StringLength(50)]
        public string MaNhaSanXuat { get; set; }
        [StringLength(50)]
        public string MaKhuyenMai { get; set; }
        [StringLength(50)]
        public string MaKho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHang { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhGia> DanhGia { get; set; }

        public virtual Kho Kho { get; set; }

        public virtual KhuyenMai KhuyenMai { get; set; }

        public virtual LoaiSanPham LoaiSanPham { get; set; }

        public virtual NhaSanXuat NhaSanXuat { get; set; }
    }
}
