
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebQLKhoDuoc.Context;
using WebQLKhoDuoc.Models;
using PagedList;

namespace WebQLKhoDuoc.Controllers
{
    public class HomeController : Controller
    {
        QLKhoDuocContext db = new QLKhoDuocContext();
        public ActionResult Index()
        {
            var model = db.LoaiMatHangs.ToList();
         
            return View(model);
        }

        public ActionResult home()
        {
            var model = db.Khos.ToList();

            return View(model);
        }
        public ActionResult Login()
        {
            ThanhVien _objuserloginmodel = new ThanhVien();
            return View(_objuserloginmodel);
        }
        
        

        [HttpPost]
        public ActionResult Login(ThanhVien thanhvien)
        {
            if (ModelState.IsValid)
            {
                using (QLKhoDuocContext db = new QLKhoDuocContext())
                {
                    var obj = db.ThanhViens.Where(a => a.TenDangNhap.Equals(thanhvien.TenDangNhap) && a.Pass.Equals(thanhvien.Pass)).FirstOrDefault();
                    if (obj != null)
                    {
                        if (obj.LoaiThanhVien.MaLoaiThanhVien == 1)
                        {
                            ViewBag.Message = 1;
                            Session["TenDangNhap"] = obj.TenDangNhap;
                            Session["MaThanhVien"] = obj.MaThanhVien;
                            Session["MaLoaiThanhVien"] = obj.MaLoaiThanhVien;
                            return Redirect("Home/AfterLoginAdmin");
                        }
                             
                        else
                        {
                            Session["TenDangNhap"] = obj.TenDangNhap;
                            Session["FullName"] = obj.FullName;
                            Session["MaLoaiThanhVien"] = obj.MaLoaiThanhVien;
                            Session["MaThanhVien"] = obj.MaThanhVien;                   
                            ViewBag.Message = 2;
                            return Redirect("Home/AfterLogin");
                        }
                    }
                    else
                    {
                        ViewBag.Message = 0;

                    }
                }
            }
            return View(thanhvien);
        }

        public ActionResult AfterLogin()
        {
            if (Session["TenDangNhap"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AfterLogout");
            }
        }
        public ActionResult AfterLoginAdmin()
        {
            if (Session["TenDangNhap"] != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("AfterLogoutAdmin");
            }
        }
        public ActionResult AfterLogouyAdmin()
        {
            Session["TenDangNhap"] = null;
            Session["MaThanhVien"] = null;

            return View();
        }
        public ActionResult AfterLogout()
        {
            Session["TenDangNhap"] = null;
            Session["MaThanhVien"] = null;
          
            return View();
        }
        public ActionResult DangKy()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult DangKy(ThanhVien nd)
        {
            if (ModelState.IsValid)
            {
                var count = db.ThanhViens.Count(u => u.TenDangNhap == nd.TenDangNhap);
               
                if (count == 0)
                {
                    nd.MaLoaiThanhVien = 5;
                    db.ThanhViens.Add(nd);                   
                    //lưu vào CSDL
                    db.SaveChanges();
                    ModelState.Clear();
                    nd = null;
                    return Redirect("../Home/Login");
                }
                else
                {
                    ViewBag.Message1 = "Tên đăng nhập đã tồn tại vui lòng nhập lại!!!";
                }
            }
            return View();
        }
        public ActionResult DoiMatKhau()
        {
            return View();
        }
        [HttpPost]
        public ActionResult DoiMatKhau(ManageUserViewModel tv)
        {
            if (ModelState.IsValid)
            {
                var name = Session["TenDangNhap"].ToString();
                var model = db.ThanhViens.Where(n => n.TenDangNhap == name).FirstOrDefault();
                var user = db.ThanhViens.Where(u => u.MaThanhVien == model.MaThanhVien).FirstOrDefault();
                if (user.Pass == tv.OldPassword)
                {
                    user.Pass = tv.NewPassword;
                    user.ConfirmPassword = tv.NewPassword;
                    db.SaveChanges();
                    return Redirect("../Home/Login");
                }
                else
                {
                    ViewBag.Message = "Password hiện tại không đúng vui lòng nhập lại";
                }
            }
            return View();
        }
        public ActionResult XemThongTin(int MaThanhVien)
        {
        //   matv = Int32.Parse(Session["MaThanhVien"].ToString());
            var model = db.ThanhViens.Find(MaThanhVien);
            return View(model);

        }

        public ActionResult Notification()
        {
            return View();
        }

        public ActionResult LienHe()
        {
            return View();
        }
        //Xem lich su ng dung
        public ActionResult ViewHis(int? page, String searchBy = "", String search = "")
        {

            int pageNumber = (page ?? 1);
            int pageSize = 10;
            var his = db.HisLogs.ToList().OrderBy(n => n.NgayLuu).ToPagedList(pageNumber, pageSize);

            if (searchBy == "MaThanhVien")
            {

                return View(db.HisLogs.Where(p => p.ThanhViens.FullName.Contains(search) || search == "").ToList().OrderBy(n => n.NgayLuu).ToPagedList(pageNumber, pageSize));
            }
            if (searchBy == "MaAction")
            {
                return View(db.HisLogs.Where(p => p.HanhDongs.TenAction.Contains(search) || search == "").ToList().OrderBy(n => n.NgayLuu).ToPagedList(pageNumber, pageSize));

            }
            return View(his);
        }

    }
}