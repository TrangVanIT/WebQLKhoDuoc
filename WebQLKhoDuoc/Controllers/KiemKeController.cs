using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DevExpress.Web.Mvc;

namespace WebQLKhoDuoc.Controllers
{
      [WebQLKhoDuoc.Models.Authorization]
    public class KiemKeController : Controller
    {
        //
        // GET: /KiemKe/
        public ActionResult KKTheoLoaiMH()
        {         
            return View();
        }
       
	}
}




