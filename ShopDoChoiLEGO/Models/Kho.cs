namespace ShopDoChoiLEGO.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Kho")]
    public partial class Kho
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public Kho()
        {
            SanPham = new HashSet<SanPham>();
        }

        [Key]
        [StringLength(50)]
        public string MaKho { get; set; }

        [StringLength(100)]
        public string TenKho { get; set; }

        [StringLength(255)]
        public string DiaChi { get; set; }

        public int? SoLuongSanPhamTrongKho { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SanPham> SanPham { get; set; }
    }
}
