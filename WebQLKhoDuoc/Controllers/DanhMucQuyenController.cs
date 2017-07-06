using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQLKhoDuoc.Context;
using PagedList;
using WebQLKhoDuoc.Models;
using System.Data.Entity;

namespace WebQLKhoDuoc.Controllers
{
     [WebQLKhoDuoc.Models.Authorization]
    public class DanhMucQuyenController : Controller
    {
         QLKhoDuocContext db = new QLKhoDuocContext();
        //
        // GET: /DanhMucQuyen/
        public ActionResult Index()
        {
            return View();
        }


        public ActionResult UpdateNV()
        {
            ReflectController rc = new ReflectController();
            List<Type> listControllerType = rc.GetControllers("WebQLKhoDuoc.Controllers");
            //   
            List<string> listControllerOld = db.DanhMucQuyens.Select(c => c.MaDMQuyen).ToList();
            List<string> listPermissionOld = db.PhanQuyens.Select(p => p.TenQuyen).ToList();
            foreach (var c in listControllerType)
            {
                if (!listControllerOld.Contains(c.Name))
                {
                    DanhMucQuyen b = new DanhMucQuyen() { MaDMQuyen = c.Name, TenDMQuyen = "Chưa có mô tả" };
                    db.DanhMucQuyens.Add(b);
                }
                List<string> listPermission = rc.GetActions(c);
                foreach (var p in listPermission)
                {
                    if (!listPermissionOld.Contains(c.Name + "-" + p))
                    {
                        PhanQuyen permission = new PhanQuyen()
                        {
                            TenQuyen = c.Name + "-" + p,
                            Mota = "Chưa có mô tả",
                            MaDMQuyen = c.Name
                        };
                        db.PhanQuyens.Add(permission);
                    }
                }
            }
            db.SaveChanges();
            TempData["err"] = "<div class='alert alert-info' role='alert'><span class='glypgicon glyphicon-exdamation-sign' aria-hidden='true'></span><span class='sr-only'></span>Cập nhật thành công";
            return RedirectToAction("XemDMQuyen");

        }

        public ActionResult XemDMQuyen(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            return View(db.DanhMucQuyens.ToList().OrderBy(n => n.MaDMQuyen).ToPagedList(pageNumber, pageSize));

        }

        [HttpGet]
        public ActionResult SuaDMQuyen(string MaDMQuyen)
        {
            DanhMucQuyen dv = db.DanhMucQuyens.Find(MaDMQuyen);
            if (dv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDMQuyen([Bind(Include = "MaDMQuyen,TenDMQuyen")]DanhMucQuyen dmq)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.Entry(dmq).State = EntityState.Modified;
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(dmq.TenDMQuyen + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                return RedirectToAction("XemDMQuyen", "DanhMucQuyen");
            }


            return View(dmq);

        }


	}
}