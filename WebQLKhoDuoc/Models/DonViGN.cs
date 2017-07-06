using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    [Table("DonViGN")]
    public class DonViGN
    {
        [Key]
        public int MaDonVi { get; set; }
        public string TenDonVi { get; set; }
        public virtual ICollection<PhieuXuat> PhieuXuats { get; set; }

    }
}