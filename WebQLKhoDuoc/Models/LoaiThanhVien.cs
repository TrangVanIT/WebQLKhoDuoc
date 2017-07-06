using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
     [Table(" LoaiThanhVien")]
    public class LoaiThanhVien
    {
        [Key]
        public int MaLoaiThanhVien { get; set; }
        public string TenLoaiThanhVien { get; set; }
        public virtual ICollection<ThanhVien> ThanhViens { get; set; }
        public virtual ICollection<PhanQuyenTV> PhanQuyenTVs { get; set; }
    }
}