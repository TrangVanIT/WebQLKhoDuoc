using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQLKhoDuoc.Context;

namespace WebQLKhoDuoc.Models
{
    public class Authorization: ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            if(HttpContext.Current.Session["MaThanhVien"]==null)
            {
                filterContext.Result = new RedirectResult("/Home/Login");
                return;

            }
            int maloai = int.Parse(HttpContext.Current.Session["MaLoaiThanhVien"].ToString());
            int userId = int.Parse(HttpContext.Current.Session["MaThanhVien"].ToString());
            string actionName = filterContext.ActionDescriptor.ControllerDescriptor.ControllerName + "Controller-" + filterContext.ActionDescriptor.ActionName;
            QLKhoDuocContext db = new QLKhoDuocContext();

            var admin = db.ThanhViens.Where(a => a.MaThanhVien==userId && a.isadmin.Value == true && a.MaLoaiThanhVien == 1).FirstOrDefault();
            if (admin != null)
                return;


            var listpermission = from p in db.PhanQuyens
                                 join g in db.PhanQuyenTVs on p.MaQuyen equals g.MaQuyen                              
                                 where g.MaLoaiThanhVien == maloai 
                                 select p.TenQuyen;


            if(!listpermission.Contains(actionName))
            {
                filterContext.Result = new RedirectResult("/Home/Notification");
                return;
            }
            
        }
    }
}