using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    public class BaoCaoX
    {
        public int MaMatHang { get; set; }
        public string TenMatHang { get; set; }
        public string DonViTinh { get; set; }
        public decimal Gia { get; set; }
        public string TenKho { get; set; }
        public int SLHangTamX { get; set; }
        public Nullable<System.DateTime> NgayXuat { get; set; }
        public string TenDonVi { get; set; }
        public int SoLuongX { get; set; }
        public decimal ThanhTien { get; set; }
        public string HTTT { get; set; }
    }
}