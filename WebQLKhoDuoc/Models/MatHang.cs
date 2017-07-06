using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WebQLKhoDuoc.Models
{
    [Table("MatHang")]
    public class MatHang
    {
        [Key]
        public int MaMatHang { get; set; }
        public string TenMatHang { get; set; }
        public string DonViTinh { get; set; }
        public int TongSoLuong { get; set; }
        public decimal Gia { get; set; }
        public Nullable<DateTime> NgaySX { get; set; }
        public Nullable<DateTime> HanSD { get; set; }
        public Nullable<int> MaLoaiMH { get; set; }
        public virtual LoaiMatHang LoaiMatHang { get; set; }      
        public virtual ICollection<ChiTietHopDong> ChiTietHopDongs { get; set; }
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
    
      
        
    }
    
}
