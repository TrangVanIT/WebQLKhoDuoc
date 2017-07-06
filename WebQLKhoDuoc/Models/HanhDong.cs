using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
     [Table("HanhDong")]
    public class HanhDong
    {
        [Key]
        public int MaAction { get; set; }
        public string TenAction { get; set; }
        public virtual ICollection<HisLog> HisLogs { get; set; }
    }
}