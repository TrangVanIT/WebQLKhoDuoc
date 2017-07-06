using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    [Table("NhaCungCap")]
    public class NhaCungCap
    {
        [Key]
        public int MaNhaCC { get; set; }
        public string TenNhaCC { get; set; }
        public string DiaChi { get; set; }
        public string SDT { get; set; }
        public virtual ICollection<HopDong> HopDongs { get; set; }
    }
}