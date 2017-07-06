using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
     [Table("HisLog")]
    public class HisLog
    {
        [Key]
        public int MaHis { get; set; }
        public string NoiDung { get; set; }
        public Nullable<System.DateTime> NgayLuu { get; set; }
        public int MaAction { get; set; }
        public int MaThanhVien { get; set; }
        public virtual HanhDong HanhDongs { get; set; }
        public virtual ThanhVien ThanhViens { get; set; }
    }
}