using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    [Table("PhieuXuat")]
    public class PhieuXuat
    {
        [Key]
        public int MaPhieuX { get; set; }
        public string TenPhieuX { get; set; }
        public Nullable<System.DateTime> NgayXuat { get; set; }
        public int SoLuongX { get; set; }
        public decimal TongGiaTriX { get; set; }
        public string HTTT { get; set; }
        public Nullable<bool> isThanhToan { get; set; }
        public Nullable<bool> isDuyet { get; set; }
        public Nullable<int> MaThanhVien { get; set; }
        public Nullable<int> MaKho { get; set; }
        public Nullable<int> MaDonVi { get; set; }
        public virtual ThanhVien ThanhVien { get; set; }
        public virtual Kho Kho { get; set; }
        public virtual DonViGN DonViGN { get; set; }
        public virtual ICollection<ChiTietPhieuXuat> ChiTietPhieuXuats { get; set; }      

    }
}