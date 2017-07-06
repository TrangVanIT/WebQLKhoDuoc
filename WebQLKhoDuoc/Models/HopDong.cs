using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WebQLKhoDuoc.Models
{
     [Table("HopDong")]
    public class HopDong
    {
        [Key]
        public int MaHopDong { get; set; }
        public string SoHopDong { get; set; }        
        public Nullable<System.DateTime> NgayKy { get; set; }
        public Nullable<System.DateTime> NgayKT { get; set; }
        public decimal TongGiaHD { get; set; }   
        public Nullable<int> MaThanhVien { get; set; }
        public Nullable<int> MaNhaCC { get; set; }
        public Nullable<bool> isThanhToan { get; set; }
        public virtual NhaCungCap NhaCungCap { get; set; }
        public virtual ThanhVien ThanhVien { get; set; }
        public virtual ICollection<ChiTietHopDong> ChiTietHopDongs { get; set; }
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }
         
    }
}
