using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
     [Table("PhanQuyen")]
    public class PhanQuyen
    {
        [Key]
        public int MaQuyen { get; set; }
        public string TenQuyen { get; set; }
        public string  Mota { get; set; }
        public string MaDMQuyen { get; set; }
        public virtual DanhMucQuyen DanhMucQuyen { get; set; }
        public virtual ICollection<PhanQuyenTV> PhanQuyenTVs { get; set; }

    }
}