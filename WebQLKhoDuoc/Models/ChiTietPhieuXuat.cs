using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    [Table("ChiTietPhieuXuat")]
    public class ChiTietPhieuXuat
    {
        [Key]
        public int MaHangXuat { get; set; }   
        public Nullable<int> MaMatHang { get; set; }
        public int SLHangTamX { get; set; }  
        public Nullable<int> MaKho { get; set; }
        public Nullable<int> MaPhieuX { get; set; }
        public virtual MatHang MatHang { get; set; }
        public virtual PhieuXuat PhieuXuat { get; set; }
       
   
    }
}