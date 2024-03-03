//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Hasaki.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class SanPham
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public SanPham()
        {
            this.ChiTietDonHangs = new HashSet<ChiTietDonHang>();
            this.DanhMucSanPhams = new HashSet<DanhMucSanPham>();
        }
    
        public int SanPhamID { get; set; }
        public string TenSanPham { get; set; }
        public string HinhAnh { get; set; }
        public string MoTa { get; set; }
        public Nullable<double> Gia { get; set; }
        public string DungTich { get; set; }
        public string ThuongHieu { get; set; }
        public string XuatXu { get; set; }
        public string LoaiDaPhuHop { get; set; }
        public Nullable<double> DoPH { get; set; }
        public string CongDung { get; set; }
        public string ThanhPhanChinh { get; set; }
        public string CachSuDung { get; set; }
        public string LuuY { get; set; }
        public Nullable<int> ThuongHieuID { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<ChiTietDonHang> ChiTietDonHangs { get; set; }
        public virtual ThuongHieu ThuongHieu1 { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<DanhMucSanPham> DanhMucSanPhams { get; set; }
    }
}