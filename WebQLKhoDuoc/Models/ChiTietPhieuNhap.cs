using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
     [Table("ChiTietPhieuNhap")]
    public class ChiTietPhieuNhap
    {
        [Key]
        public int MaHangNhap { get; set; }
        public Nullable<int> MaMatHang { get; set; }
        public int SLHangTamN { get; set; }
        public decimal GiaHangN { get; set; }
        public Nullable<DateTime> NgaySX { get; set; }
        public Nullable<DateTime> HanSD { get; set; }
        public Nullable<int> MaKho { get; set; }
        public Nullable<int> MaPhieuN { get; set; }        
        public virtual MatHang MatHang { get; set; }
        public virtual PhieuNhap PhieuNhap { get; set; }
    }
}