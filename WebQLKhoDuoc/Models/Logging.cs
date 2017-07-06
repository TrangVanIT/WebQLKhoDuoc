using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebQLKhoDuoc.Context;

namespace WebQLKhoDuoc.Models
{
    public class Logging
    {

        public void SaveLog(string NoiDung, int MaAction, int MaThanhVien)
        {
            QLKhoDuocContext db = new QLKhoDuocContext();
            HisLog h = new HisLog();
            h.MaAction = MaAction;
            h.MaThanhVien = MaThanhVien;
            h.NgayLuu = DateTime.Now;
            h.NoiDung = NoiDung;
            db.HisLogs.Add(h);
            db.SaveChanges();

        }
    }
}