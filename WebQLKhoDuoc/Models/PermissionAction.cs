using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebQLKhoDuoc.Models
{
    public class PermissionAction
    {
        public int MaQuyen { get; set; }
        public string TenQuyen { get; set; }
        public string MoTa { get; set; }
        public bool IsGranted { get; set; }
    }
}