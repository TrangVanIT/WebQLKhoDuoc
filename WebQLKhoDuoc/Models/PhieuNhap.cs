using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    [Table("PhieuNhap")]
    public class PhieuNhap
    {
        [Key]
        public int MaPhieuN { get; set; }
        public string TenPhieuN { get; set; }
        public Nullable<System.DateTime> NgayNhap { get; set; }
        public int SoLuongN { get; set; }        
        public decimal TongGiaTriN { get; set; }
        public string HTTT { get; set; }
        public string DotGiaoHang { get; set; }
        public Nullable<bool> isThanhToan { get; set; }
        public Nullable<bool> isDuyet { get; set; }
        public Nullable<int> MaThanhVien { get; set; }
        public Nullable<int> MaKho { get; set; }
        public Nullable<int> MaHopDong { get; set; }  
        public virtual ThanhVien ThanhVien { get; set; }
        public virtual Kho Kho { get; set; }
        public virtual HopDong HopDong { get; set; }
        public virtual ICollection<ChiTietPhieuNhap> ChiTietPhieuNhaps { get; set; }
       
        
    }
}