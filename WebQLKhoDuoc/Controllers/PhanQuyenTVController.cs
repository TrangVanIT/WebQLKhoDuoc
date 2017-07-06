using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQLKhoDuoc.Context;
using WebQLKhoDuoc.Models;

namespace WebQLKhoDuoc.Controllers
{
     [WebQLKhoDuoc.Models.Authorization]
    public class PhanQuyenTVController : Controller
    {
        //
        // GET: /PhanQuyenTV/
        QLKhoDuocContext db = new QLKhoDuocContext();
        public ActionResult Index(int id)
        {
            var listcontrol = db.DanhMucQuyens.AsEnumerable();
            List<SelectListItem> items = new List<SelectListItem>();
            foreach(var item in listcontrol)
            {
                items.Add(new SelectListItem()
                {
                    Text = item.TenDMQuyen,
                    Value = item.MaDMQuyen
                });
               
            }
            ViewBag.items = items;
            var listgranted = from g in db.PhanQuyenTVs
                              join p in db.PhanQuyens on g.MaQuyen equals p.MaQuyen
                              where g.MaLoaiThanhVien == id
                              select new SelectListItem() { Value = p.MaQuyen.ToString(), Text = p.Mota };
            ViewBag.listgranted = listgranted;
            Session["usergrant"] = id;
            var usergrant = db.LoaiThanhViens.Find(id);
            ViewBag.usergrant = usergrant.TenLoaiThanhVien;

            return View();
        }
        public JsonResult getPhanQuyenTV(string id, int usertemp)
        {
            //lay all quyen user va nghiep vu
            var listgranted = (from g in db.PhanQuyenTVs
                              join p in db.PhanQuyens on g.MaQuyen equals p.MaQuyen
                              where g.MaLoaiThanhVien == usertemp && p.MaDMQuyen == id
                              select new PermissionAction { MaQuyen = p.MaQuyen, TenQuyen = p.TenQuyen, MoTa = p.Mota, IsGranted=true }).ToList();
         // lay all per nv hien tai
            var listpermission = from p in db.PhanQuyens
                                 where p.MaDMQuyen == id
                                 select new PermissionAction { MaQuyen = p.MaQuyen, TenQuyen = p.TenQuyen, MoTa = p.Mota, IsGranted = false };
           //lay cac id cua per da gan cho ng dung
            var listpermissionid = listgranted.Select(p => p.MaQuyen);
            foreach(var item in listpermission)
            {
                if (!listpermissionid.Contains(item.MaQuyen))
                    listgranted.Add(item);
            }
            return Json(listgranted.OrderBy(x => x.MoTa), JsonRequestBehavior.AllowGet);
        }

        public string updatePhanQuyen(int id, int usertemp)
        {
            string msg = "";
            var grant = db.PhanQuyenTVs.Find(id, usertemp);
            if(grant==null)
            {
                PhanQuyenTV g = new PhanQuyenTV() { MaQuyen = id, MaLoaiThanhVien = usertemp };
                db.PhanQuyenTVs.Add(g);
                msg = "<div class='alert alert-success'>Quyền cấp thành công</div>";
            }
            else
            {
                db.PhanQuyenTVs.Remove(grant);
                msg = "<div class='alert alert-danger'>Quyền hủy thành công</div>";

            }
            db.SaveChanges();
            return msg;
        }
	}
}

