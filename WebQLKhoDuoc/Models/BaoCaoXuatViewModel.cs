using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    public class BaoCaoXuatViewModel
    {
        [Key]
        public int MaHangKho { get; set; }
      
        public int MaMatHang { get; set; }
        public int SoLuong { get; set; } //tung loai mat hang xuat mhk
        public decimal Gia { get; set; }
        public int MaKho { get; set; }
        public int MaDonVi { get; set; }
        public Nullable<System.DateTime> NgayXuat { get; set; }
        public decimal TongGiaTriX { get; set; }
        public string HTTT { get; set; }        
        public int MaThanhVien { get; set; }
        public int MaPhieuX { get; set; }
        public virtual Kho Kho { get; set; }
        public virtual DonViGN DonViGN { get; set; }
        public virtual MatHang MatHang { get; set; }
        public virtual ChiTietPhieuXuat MatHang_Kho { get; set; }
        public virtual PhieuXuat PhieuXuat { get; set; }
        public virtual ThanhVien ThanhVien { get; set; }
        
    }
}

 