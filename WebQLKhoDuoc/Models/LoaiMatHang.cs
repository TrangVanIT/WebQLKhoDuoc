using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WebQLKhoDuoc.Models
{
     [Table("LoaiMatHang")]
  public  class LoaiMatHang
    {
        [Key]
        public int MaLoaiMH { get; set; }
        public string TenLoaiMH { get; set; }
        public virtual ICollection<MatHang> MatHangs { get; set; }
    }
}
