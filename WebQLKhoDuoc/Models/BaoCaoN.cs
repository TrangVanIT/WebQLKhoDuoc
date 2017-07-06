using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    public class BaoCaoN
    {
        public int MaPhieuN { get; set; }
        public int MaKho { get; set; }
        public int MaNhaCC { get; set; }
        public int MaHopDong { get; set; }
        public string TenMatHang { get; set; }
        public string DonViTinh { get; set; }
        public Nullable<System.DateTime> NgayNhap { get; set; }
        public int SoLuongN { get; set; }
        public int SLHangTamN { get; set; }
        public Nullable<System.DateTime> NgayKy { get; set; }
        public Nullable<System.DateTime> NgayKT { get; set; }    
        public string SoHopDong { get; set; }   
        public decimal Gia { get; set; }
        public decimal ThanhTien { get; set; }
        public string TenNhaCC { get; set; }
        public string TenKho { get; set; }

    }
}