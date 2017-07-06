using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    [Table("ChiTietHopDong")]
    public class ChiTietHopDong
    {
        [Key]
        public int MaCTHD { get; set; }
        public int SoLuong { get; set; }
        public decimal Gia { get; set; }       
        public Nullable<int> MaMatHang { get; set; }
        public Nullable<int> MaHopDong { get; set; }
        public virtual MatHang MatHang { get; set; }
        public virtual HopDong HopDong { get; set; }

    }
}