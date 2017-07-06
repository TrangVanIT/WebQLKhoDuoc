using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;

namespace WebQLKhoDuoc.Models
{
    [Table("Kho")]
   public class Kho
    {
        [Key]
        public int MaKho { get; set; }
        public string TenKho { get; set; }
        public virtual ICollection<PhieuNhap> NhapPhieus { get; set; }
        public virtual ICollection<PhieuXuat> XuatPhieus { get; set; }


    }
}
