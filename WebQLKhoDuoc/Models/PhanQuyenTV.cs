using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
     [Table("PhanQuyenTV")]
    public class PhanQuyenTV
    {
         [Key, Column(Order = 1)]
        public int MaQuyen { get; set; }
         [Key, Column(Order = 2)]
        public int MaLoaiThanhVien { get; set; }
         public virtual LoaiThanhVien LoaiThanhVien { get; set; }
         public virtual PhanQuyen PhanQuyen { get; set; }
    }
}