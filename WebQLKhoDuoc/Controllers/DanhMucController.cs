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
    public class DanhMucController : Controller
    {
        
        //
        // GET: /DanhMuc/
        QLKhoDuocContext db = new QLKhoDuocContext();
        public ActionResult Index()
        {
            return View();
      
        }
        //========donvi===========
        public ActionResult XemDMDonVi(int? page, String search = "")
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.DonViGNs.Where(p => p.TenDonVi.Contains(search) || search == "").ToList().OrderBy(n => n.TenDonVi).ToPagedList(pageNumber, pageSize));

        }


        [HttpGet]
        public ActionResult SuaDMDonVi(int? MaDonVi)
        {
            DonViGN dv = db.DonViGNs.Find(MaDonVi);
            if (dv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(dv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDMDonVi([Bind(Include = "MaDonVi,TenDonVi")]DonViGN donvi)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.Entry(donvi).State = EntityState.Modified;
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(donvi.TenDonVi + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                return RedirectToAction("XemDMDonVi", "DanhMuc");
            }
            

            return View(donvi);

        }

        public ActionResult ThemDMDonVi()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDMDonVi(DonViGN donvi)
        {

            if (ModelState.IsValid)
            {
                var count = db.DonViGNs.Count(n => n.TenDonVi == donvi.TenDonVi);
                if (count == 0)
                {
                    db.DonViGNs.Add(donvi);
                    db.SaveChanges();
                    Logging log = new Logging();
                    log.SaveLog(donvi.TenDonVi + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
                    return Redirect("../DanhMuc/XemDMDonVi");
                }
                else
                {
                    ViewBag.Message = 0;
                }
            }
            return View(donvi);
        }






        //----------done--------
        public ActionResult XemDMKho(int? page, String search="")
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;          
            return View(db.Khos.Where(p => p.TenKho.Contains(search) || search=="").ToList().OrderBy(n => n.TenKho).ToPagedList(pageNumber, pageSize));
            
        }
        [HttpGet]
        public ActionResult SuaDMKho(int? MaKho)
        {
            Kho kho = db.Khos.Find(MaKho);
            if(kho == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(kho);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDMKho([Bind(Include ="MaKho,TenKho")]Kho kho)
        {
         
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
               
                db.Entry(kho).State = EntityState.Modified;                         
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(kho.TenKho+ "-" + db.HanhDongs.Where(n=>n.MaAction==2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                    
               
                return RedirectToAction("XemDMKho", "DanhMuc");        
            }

            return View(kho);
                 
        }
        public ActionResult XoaDMKho(int MaKho)
        {
            Kho kho = db.Khos.Find(MaKho);
            if (kho == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Logging log = new Logging();
            log.SaveLog(kho.TenKho + "-" + db.HanhDongs.Where(n => n.MaAction == 3).FirstOrDefault().TenAction, 3, int.Parse(Session["MaThanhVien"].ToString()));
            db.Khos.Remove(kho);
            db.SaveChanges();
       
            return RedirectToAction("XemDMKho", "DanhMuc");  
        }
        public ActionResult ThemDMKho()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDMKho(Kho kho)
        {

            if (ModelState.IsValid)
            {
                var count = db.Khos.Count(n => n.TenKho == kho.TenKho);
                if (count == 0)
                {
                    db.Khos.Add(kho);
                    db.SaveChanges();
                    Logging log = new Logging();
                    log.SaveLog(kho.TenKho + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
                    
                    return Redirect("../DanhMuc/XemDMKho");
                }
                else
                {
                    ViewBag.Message = 0;
                }
            }
            return View(kho);
        }


        //=============================done==========================
        public ActionResult XemDMHopDong(int? page, String search = "")
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            return View(db.HopDongs.Where(p => p.SoHopDong.Contains(search) || search == "").ToList().OrderBy(n => n.MaHopDong).ToPagedList(pageNumber, pageSize));

        }
        [HttpGet]
        public ActionResult SuaDMHopDong(int? MaHopDong)
        {
            //ViewBag.HD = db.ThanhViens;
            //  ViewBag.NCC = db.NhaCungCaps;

            ViewBag.HD = db.ThanhViens.ToList().OrderBy(n => n.MaThanhVien);
            ViewBag.NCC = db.NhaCungCaps.ToList().OrderBy(n => n.MaNhaCC);
            HopDong HD = db.HopDongs.Find(MaHopDong);
            if (HD == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(HD);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDMHopDong(HopDong hd)
        {            //Thêm vào cơ sở dữ liệu         
            ViewBag.HD = db.ThanhViens.ToList().OrderBy(n => n.MaThanhVien);
            ViewBag.NCC = db.NhaCungCaps.ToList().OrderBy(n => n.MaNhaCC);
            //ViewBag.HD = db.ThanhViens;
            //   ViewBag.NCC = db.NhaCungCaps;
            if (ModelState.IsValid)
            {

                db.Entry(hd).State = EntityState.Modified;
                if (hd.NgayKy < hd.NgayKT)
                {

                    db.SaveChanges();
                    Logging log = new Logging();
                    log.SaveLog(hd.SoHopDong + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                    return RedirectToAction("XemDMHopDong", "DanhMuc");
                }
                else ViewBag.Message = 0;

            }
            return View(hd);
        }
        public ActionResult ThemDMHopDong()
        {
            ViewBag.HD = db.ThanhViens.ToList().OrderBy(n => n.MaThanhVien);
            ViewBag.NCC = db.NhaCungCaps;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDMHopDong(HopDong hd)
        {
            ViewBag.HD = db.ThanhViens.ToList().OrderBy(n => n.MaThanhVien);
            ViewBag.NCC = db.NhaCungCaps;
            var count = db.HopDongs.Count(n => n.SoHopDong == hd.SoHopDong);
            if (count == 0)
            {
                if (hd.NgayKy < hd.NgayKT)
                {
                    db.HopDongs.Add(hd);
                    db.SaveChanges();
                    Logging log = new Logging();
                    log.SaveLog(hd.SoHopDong + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
                    return Redirect("../DanhMuc/XemDMHopDong");
                }
                else ViewBag.Message = 0;

            }
            else ViewBag.Message = 1;
            return View(hd);

        }
        public ActionResult XoaDMHopDong(int MaHopDong)
        {
            HopDong px2 = db.HopDongs.Find(MaHopDong);

            var ctx2 = (from ctx in db.ChiTietHopDongs
                        where MaHopDong == ctx.MaHopDong
                        select new
                        {
                            ctx.MaCTHD,
                            ctx.MaMatHang,
                         //   ctx.isThanhToan,
                            ctx.MaHopDong,
                            ctx.SoLuong,
                          
                            ctx.Gia
                        }).AsEnumerable();

            if (px2 == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            foreach (var i in ctx2)
            {
                var chitiet = db.ChiTietHopDongs.FirstOrDefault(s => s.MaHopDong == MaHopDong);
                if (chitiet != null)
                {
                  
                    db.ChiTietHopDongs.Remove(chitiet);
                    db.SaveChanges();
                  
                }

            }
            Logging log2 = new Logging();
            log2.SaveLog(px2.SoHopDong + "-" + db.HanhDongs.Where(n => n.MaAction == 3).FirstOrDefault().TenAction, 3, int.Parse(Session["MaThanhVien"].ToString()));

            db.HopDongs.Remove(px2);
            db.SaveChanges();          
            
            return RedirectToAction("XemDMHopDong", "DanhMuc"); 
        }
        //============done===
         
        //===CTHD
        public ActionResult XemCTHopDong(int? page, int MaHopDong)
        {
            
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            ViewBag.MaHopDong = MaHopDong;
                return View(db.ChiTietHopDongs.Where(p =>p.MaHopDong==MaHopDong).ToList().OrderBy(n => n.MaHopDong).ToPagedList(pageNumber, pageSize));
           
        }
        public ActionResult XoaCTHopDong(int MaCTHD)
        {
            ChiTietHopDong CTHD = db.ChiTietHopDongs.Find(MaCTHD);

            var hd = db.HopDongs.Find(CTHD.MaHopDong);
            
            var ctx2 = (from ctx in db.ChiTietHopDongs
                       where hd.MaHopDong == ctx.MaHopDong
                        select new
                        {
                            ctx.MaCTHD,
                            ctx.MaMatHang,
                            ctx.MaHopDong,
                            ctx.SoLuong,
                           
                            ctx.Gia,
                            thanhtien = ctx.SoLuong * ctx.Gia
                        }).AsEnumerable();

            if (CTHD == null)
            {
                Response.StatusCode = 404;
                return null;
            }


            Logging log = new Logging();
            log.SaveLog(CTHD.HopDong.SoHopDong + "-" + CTHD.MatHang.TenMatHang + "-" + db.HanhDongs.Where(n => n.MaAction == 3).FirstOrDefault().TenAction, 3, int.Parse(Session["MaThanhVien"].ToString()));
         
            
            db.ChiTietHopDongs.Remove(CTHD);
            db.SaveChanges();
            hd.TongGiaHD = ctx2.Sum(n => n.thanhtien);
            db.SaveChanges();
            return RedirectToAction("XemDMHopDong", "DanhMuc");
        }
        [HttpGet]
        public ActionResult SuaCTHopDong(int? MaCTHD)
        {
            ChiTietHopDong CTHD = db.ChiTietHopDongs.Find(MaCTHD);
            ViewBag.HD = db.HopDongs.ToList().OrderBy(n => n.SoHopDong);
            ViewBag.MH = db.MatHangs.ToList().OrderBy(n => n.TenMatHang);
            
            if (CTHD == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(CTHD);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaCTHopDong(ChiTietHopDong CTHD)
        {
            ViewBag.HD = db.HopDongs.ToList().OrderBy(n => n.SoHopDong);
            ViewBag.MH = db.MatHangs.ToList().OrderBy(n => n.TenMatHang);

            var hd = db.HopDongs.Find(CTHD.MaHopDong);

            var ctx2 = (from ctx in db.ChiTietHopDongs
                        where hd.MaHopDong == ctx.MaHopDong
                        select new
                        {
                            ctx.MaCTHD,
                            ctx.MaMatHang,
                            ctx.MaHopDong,
                            ctx.SoLuong,                          
                            ctx.Gia,
                            thanhtien = ctx.SoLuong * ctx.Gia
                        }).AsEnumerable();


            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                ViewBag.Message = 1;
                db.Entry(CTHD).State = EntityState.Modified;
                db.SaveChanges();


                Logging log = new Logging();
                log.SaveLog(CTHD.HopDong.SoHopDong + "-" + CTHD.MatHang.TenMatHang + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
         
                hd.TongGiaHD = ctx2.Sum(n => n.thanhtien);
                db.SaveChanges();

                return RedirectToAction("XemDMHopDong", "DanhMuc");
            }
            return View(CTHD);
        }


        public ActionResult ThemCTHD(int? MaCTHD)
        {
            ChiTietHopDong model = db.ChiTietHopDongs.Find(MaCTHD);
            ViewBag.HD = db.HopDongs.ToList().OrderBy(n => n.SoHopDong);
            ViewBag.MH = db.MatHangs.ToList().OrderBy(n => n.TenMatHang);
            //ViewBag.MaHD = db.ChiTietHopDongs.Find(CTHD.MaHopDong);  
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemCTHD(ChiTietHopDong CTHD)
        {
            ViewBag.HD = db.HopDongs.ToList().OrderBy(n => n.SoHopDong);
            ViewBag.MH = db.MatHangs.ToList().OrderBy(n => n.TenMatHang);
            ViewBag.MaHD = db.ChiTietHopDongs.Find(CTHD.MaHopDong);
            var count = db.ChiTietHopDongs.Count(n => n.MaHopDong == CTHD.MaHopDong && n.MaMatHang==CTHD.MaMatHang);
            var hd = db.HopDongs.Find(CTHD.MaHopDong);
            var ctx2 = (from ctx in db.ChiTietHopDongs
                        where CTHD.MaHopDong == ctx.MaHopDong
                        select new
                        {
                            ctx.MaCTHD,
                            ctx.MaMatHang,                           
                            ctx.MaHopDong,
                            ctx.SoLuong,                   
                            ctx.Gia,
                            thanhtien=ctx.SoLuong*ctx.Gia
                        }).AsEnumerable();

        //    ViewBag.a = ctx2.Sum(n => n.thanhtien);

            if (ModelState.IsValid)
            {
                if (count == 0)
                {
                    ViewBag.Message = 1;
                    db.ChiTietHopDongs.Add(CTHD);                    
                    db.SaveChanges();
                    Logging log = new Logging();
                    log.SaveLog(CTHD.HopDong.SoHopDong + "-" + CTHD.MatHang.TenMatHang + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
         
                    hd.TongGiaHD = ctx2.Sum(n => n.thanhtien);
                    db.SaveChanges();
                }
                else ViewBag.Message = 0;
            }      
        
            return RedirectToAction("XemDMHopDong", "DanhMuc");
        }


        //==done========
        public ActionResult XemDMLoaiTV(int? page, String search="")
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;                     
                return View(db.LoaiThanhViens.Where(p => p.TenLoaiThanhVien.Contains(search) || search == null).ToList().OrderBy(n => n.MaLoaiThanhVien).ToPagedList(pageNumber, pageSize));
          
        }
        public ActionResult ThemDMLoaiTV()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDMLoaiTV(LoaiThanhVien ltv)
        {
            if (ModelState.IsValid)
            {
                var count = db.LoaiThanhViens.Count(n => n.TenLoaiThanhVien == ltv.TenLoaiThanhVien);
                if (count == 0)
                {
                    db.LoaiThanhViens.Add(ltv);
                    db.SaveChanges();
                    Logging log = new Logging();
                    log.SaveLog(ltv.TenLoaiThanhVien + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
         
                    return Redirect("../DanhMuc/XemDMLoaiTV");
                }
                else
                {
                    ViewBag.Message = 0;
                }
            }
            return View(ltv);
        }
        [HttpGet]
        public ActionResult SuaDMLoaiTV(int? MaLoaiThanhVien)
        {
            //   ViewBag.MaLoaiThanhVien = new SelectList(db.LoaiThanhViens.ToList().OrderBy(n => n.TenLoaiThanhVien), "MaLoaiThanhVien", "TenLoaiThanhVien");
            LoaiThanhVien tv = db.LoaiThanhViens.Find(MaLoaiThanhVien);

            if (tv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tv);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDMLoaiTV(LoaiThanhVien ltv)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.Entry(ltv).State = EntityState.Modified;
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(ltv.TenLoaiThanhVien + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                return RedirectToAction("XemDMLoaiTV", "DanhMuc");
            }
            return View(ltv);
        }
        public ActionResult XoaDMLoaiTV(int MaLoaiThanhVien)
        {
            LoaiThanhVien ltv1 = db.LoaiThanhViens.Find(MaLoaiThanhVien);
            if (ltv1 == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Logging log = new Logging();
            log.SaveLog(ltv1.TenLoaiThanhVien + "-" + db.HanhDongs.Where(n => n.MaAction == 3).FirstOrDefault().TenAction, 3, int.Parse(Session["MaThanhVien"].ToString()));
            
            
            db.LoaiThanhViens.Remove(ltv1);
            db.SaveChanges();
            return RedirectToAction("XemDMLoaiTV", "DanhMuc");  
        }

        //=====done======
        public ActionResult XemDMThanhVien(int? page, String searchBy="", String search="")
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;
            var tv=db.ThanhViens.ToList().OrderBy(n => n.MaThanhVien).ToPagedList(pageNumber, pageSize);
            if (searchBy == "TenDangNhap")
            {
                return View(db.ThanhViens.Where(p => p.TenDangNhap.Contains(search) || search == null).ToList().OrderBy(n => n.MaThanhVien).ToPagedList(pageNumber, pageSize));
            }
            if (searchBy == "FullName")
            {
                return View(db.ThanhViens.Where(p => p.FullName.Contains(search) || search == null).ToList().OrderBy(n => n.MaThanhVien).ToPagedList(pageNumber, pageSize));
            }
            return View(tv);
        }
        public ActionResult SuaDMThanhVien(int? MaThanhVien)
        {
            ThanhVien tv = db.ThanhViens.Find(MaThanhVien);
            ViewBag.LoaiTV = db.LoaiThanhViens;
            if (tv == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(tv);
        }
        //[Bind(Include = "MaThanhVien,FullName,TenDangNhap,Pass,GioiTinh,isadmin,MaLoaiThanhVien")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDMThanhVien(ThanhVien tv)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.Entry(tv).State = EntityState.Modified;
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(tv.TenDangNhap + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                return RedirectToAction("XemDMThanhvien", "DanhMuc");
            }
            return View(tv);
        }


        public ActionResult XoaDMTV(int MaThanhVien)
        {
            ThanhVien ltv1 = db.ThanhViens.Find(MaThanhVien);
            if (ltv1 == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Logging log = new Logging();
            log.SaveLog(ltv1.TenDangNhap + "-" + db.HanhDongs.Where(n => n.MaAction == 3).FirstOrDefault().TenAction, 3, int.Parse(Session["MaThanhVien"].ToString()));
            db.ThanhViens.Remove(ltv1);
            db.SaveChanges();
            return RedirectToAction("XemDMThanhVien", "DanhMuc");
        }


        //them moi thanh vien
        public ActionResult ThemMoiTV()
        {
            ViewBag.LoaiTV = db.LoaiThanhViens;
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemMoiTV(ThanhVien tv)
        {
            ViewBag.LoaiTV = db.LoaiThanhViens;
            if (ModelState.IsValid)
            {
                var count = db.ThanhViens.Count(n => n.TenDangNhap == tv.TenDangNhap || n.Email==tv.Email);
                if (count == 0)
                {
                    db.ThanhViens.Add(tv);
                    db.SaveChanges();
                    Logging log = new Logging();
                    log.SaveLog(tv.TenDangNhap + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
                    return Redirect("../DanhMuc/XemDMThanhVien");
                }
                else
                {
                    ViewBag.Message = 0;
                }
            }
            return View(tv);
        }

        //===done===

        [HttpGet]
        public ActionResult XemDMNhaCC(int? page, String searchBy="", String search="")
        {
            int pageNumber = (page ?? 1);
            int pageSize = 4;  
            var nhacc=db.NhaCungCaps.ToList().OrderBy(n => n.MaNhaCC).ToPagedList(pageNumber, pageSize);
          
                if (searchBy == "DiaChi")
                {
                  
                    return View(db.NhaCungCaps.Where(p => p.DiaChi.Contains(search)||search == "").ToList().OrderBy(n => n.MaNhaCC).ToPagedList(pageNumber, pageSize));
                }
                if (searchBy == "TenNhaCC")
                {
                    return View(db.NhaCungCaps.Where(p => p.TenNhaCC.Contains(search) || search == "").ToList().OrderBy(n => n.MaNhaCC).ToPagedList(pageNumber, pageSize));

                }
                return View(nhacc);

        }
    
        public ActionResult ThemDMNhaCC()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemDMNhaCC(NhaCungCap ncc)
        {
            if (ModelState.IsValid)
            {
                var count = db.NhaCungCaps.Count(n => n.TenNhaCC == ncc.TenNhaCC);
                if (count == 0)
                {
                    db.NhaCungCaps.Add(ncc);
                    db.SaveChanges();
                    Logging log = new Logging();
                    log.SaveLog(ncc.TenNhaCC + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
                 
                    
                    return Redirect("../DanhMuc/XemDMNhaCC");
                }
                else
                {
                    ViewBag.Message = 0;
                }
            }
            return View(ncc);
        }
        public ActionResult XoaDMNhaCC(int MaNhaCC)
        {
            NhaCungCap ncc = db.NhaCungCaps.Find(MaNhaCC);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.NhaCungCaps.Remove(ncc);
            db.SaveChanges();
            return RedirectToAction("XemDMNhaCC", "DanhMuc");  
        }
        public ActionResult SuaDMNhaCC(int? MaNhaCC)
        {

            NhaCungCap ncc = db.NhaCungCaps.Find(MaNhaCC);
            if (ncc == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(ncc); ;
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaDMNhaCC([Bind(Include = "MaNhaCC,TenNhaCC,DiaChi,SDT")]NhaCungCap ncc)
        {
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                db.Entry(ncc).State = EntityState.Modified;
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(ncc.TenNhaCC + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                return RedirectToAction("XemDMNhaCC", "DanhMuc");
            }
            return View(ncc);
        }

        //==================done========================
        public ActionResult XemDMLoaiMH(int? page, String search="")
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;          
                return View(db.LoaiMatHangs.Where(p => p.TenLoaiMH.Contains(search) || search==null).ToList().OrderBy(n => n.TenLoaiMH).ToPagedList(pageNumber, pageSize));
          
        }
        public ActionResult XoaLoaiMH(int MaLMH)
        {
            LoaiMatHang LMH = db.LoaiMatHangs.SingleOrDefault(n => n.MaLoaiMH == MaLMH);
            if (LMH == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            db.LoaiMatHangs.Remove(LMH);
            db.SaveChanges();
            return RedirectToAction("XemDMLoaiMH");
        }
        [HttpGet]
        public ActionResult SuaLoaiMH(int MaLMH)
        {

            //  ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH");
            LoaiMatHang LMH = db.LoaiMatHangs.SingleOrDefault(n => n.MaLoaiMH == MaLMH);

            if (LMH == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(LMH);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaLoaiMH([Bind(Include = "MaLoaiMH,TenLoaiMH")]LoaiMatHang LMH)
        {

            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(LMH).State = EntityState.Modified;
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(LMH.TenLoaiMH + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                return RedirectToAction("XemDMLoaiMH", "DanhMuc");
            }

            return View(LMH);

        }
        [HttpGet]
        public ActionResult ThemMoiLMH()
        {
            //ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH");

            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiLMH(LoaiMatHang LMH)
        {
            if (ModelState.IsValid)
            {
                var count = db.LoaiMatHangs.Count(n => n.TenLoaiMH == LMH.TenLoaiMH);
                if (count == 0)
                {
                    db.LoaiMatHangs.Add(LMH);
                    db.SaveChanges();
                    Logging log = new Logging();
                    log.SaveLog(LMH.TenLoaiMH + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
                    return RedirectToAction("XemDMLoaiMH", "DanhMuc");
                }
                else
                {
                    ViewBag.Message = 0;
                }
            }
            return View(LMH);
           
        }
     
        //=========================done==============================
        public ActionResult XemMH(int? page, String searchBy = "", String search = "")
        {
           
            int pageNumber = (page ?? 1);
            int pageSize = 10;
            var mh = db.MatHangs.ToList().OrderBy(n => n.TenMatHang).ToPagedList(pageNumber, pageSize);

            if (searchBy == "TenMatHang")
            {

                return View(db.MatHangs.Where(p => p.TenMatHang.Contains(search) || search == "").ToList().OrderBy(n => n.TenMatHang).ToPagedList(pageNumber, pageSize));
            }
            if (searchBy == "MaLoaiMH")
            {
                return View(db.MatHangs.Where(p => p.LoaiMatHang.TenLoaiMH.Contains(search) || search == "").ToList().OrderBy(n => n.TenMatHang).ToPagedList(pageNumber, pageSize));

            }
            return View(mh);
         
           
        }

        public ActionResult XoaMH(int MaMH)
        {
            MatHang MH = db.MatHangs.SingleOrDefault(n => n.MaMatHang == MaMH);
            if (MH == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            Logging log = new Logging();
            log.SaveLog(MH.TenMatHang + "-" + db.HanhDongs.Where(n => n.MaAction == 3).FirstOrDefault().TenAction, 3, int.Parse(Session["MaThanhVien"].ToString()));
            db.MatHangs.Remove(MH);
            db.SaveChanges();
            return RedirectToAction("XemMH");
        }

        [HttpGet]
        public ActionResult SuaMatHang(int MaMH)
        {
            
            //MatHang MH = db.MatHangs.SingleOrDefault(n => n.MaMatHang == MaMH);
            MatHang MH = db.MatHangs.Find(MaMH);
            ViewBag.LoaiMH = db.LoaiMatHangs;

            if (MH == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(MH);

        }
        //[Bind(Include = "MaMatHang,TenMatHang,DonViTinh,SoLuong,Gia,isTonKho,MaLoaiMH")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaMatHang(MatHang MH)
        {
            if (ModelState.IsValid)
            {

                db.Entry(MH).State = EntityState.Modified;
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(MH.TenMatHang + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                return RedirectToAction("XemMH", "DanhMuc");

            }
            //  ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH", MH.MaLoaiMH);

            return View(MH);
        }

        [HttpGet]
        public ActionResult ThemMoiMH()
        {
            ViewBag.LoaiMH = db.LoaiMatHangs;
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ThemMoiMH(MatHang MH)
        {
            ViewBag.LoaiMH = db.LoaiMatHangs;
       
            if (ModelState.IsValid)
            {
                var count = db.MatHangs.Count(n => n.TenMatHang == MH.TenMatHang);
                if (count == 0)
                {
                    db.MatHangs.Add(MH);
                    db.SaveChanges();

                    Logging log = new Logging();
                    log.SaveLog(MH.TenMatHang + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
                    return RedirectToAction("XemMH", "DanhMuc");
                }
                else
                {
                    ViewBag.Message = 0;
                }
                //====================================================

            }
            return View(MH);
        }
        //==========================done=============================================
        //====================================
          [HttpGet]
        public JsonResult AjaxThemLMH(string TenLoaiMatHang)
        {
            var count1 = db.LoaiMatHangs.Count(n => n.TenLoaiMH == TenLoaiMatHang);
            if (count1 == 0)
            {
                LoaiMatHang lmh = new LoaiMatHang();
                lmh.TenLoaiMH = TenLoaiMatHang;
                db.LoaiMatHangs.Add(lmh);
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(lmh.TenLoaiMH + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
                   
            }                   
            else
            {
                ViewBag.Message = 0;
            }
            ViewBag.LoaiMH = db.LoaiMatHangs;
            return Json(count1, JsonRequestBehavior.AllowGet);
           
        }
	}
}
