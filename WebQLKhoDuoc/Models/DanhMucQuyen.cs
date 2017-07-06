using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
     [Table("DanhMucQuyen")]
    public class DanhMucQuyen
    {
        [Key]       
        public string MaDMQuyen { get; set; }
        public string TenDMQuyen { get; set; }
        public virtual ICollection<PhanQuyen> PhanQuyens { get; set; }
    }
}