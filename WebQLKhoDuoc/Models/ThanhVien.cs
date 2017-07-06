using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WebQLKhoDuoc.Models
{
      [Table("ThanhVien")]
   public class ThanhVien
    {
        [Key]
        public int MaThanhVien { get; set; }
        public string FullName { get; set; }
        public string TenDangNhap { get; set; }
        public string Email { get; set; }
        public string Pass { get; set; }
        public string ConfirmPassword { get; set; }
        public Nullable<bool> GioiTinh { get; set; }
        public Nullable<bool> isadmin { get; set; }
        public Nullable<int> MaLoaiThanhVien { get; set; }
        public virtual LoaiThanhVien LoaiThanhVien { get; set; }
        public virtual ICollection<PhieuXuat> PhieuXuats { get; set; }
        public virtual ICollection<PhieuNhap> PhieuNhaps { get; set; }
        public virtual ICollection<HopDong> HopDongs { get; set; }
        public virtual ICollection<HisLog> HisLogs { get; set; }





      
    }
}
