using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using WebQLKhoDuoc.Models;
using WebQLKhoDuoc.Context;
using System.Data.Entity;
using Rotativa;
using Excel = Microsoft.Office.Interop.Excel;
using System.Runtime.InteropServices;


namespace WebQLKhoDuoc.Controllers
{
    [WebQLKhoDuoc.Models.Authorization]
    public class PhieuNhapXuatController : Controller
    {
        //
        // GET: /PhieuNhapXuat/
        //public ActionResult Index()
        //{
        //    return View();
        //}
        QLKhoDuocContext db = new QLKhoDuocContext();
        public ActionResult DMXNP()
        {
            return View();
        }
        public ActionResult XemPhieuXuat(int? page, String NgayNhapS, String NgayNhapF)
        {
            DateTime tungay;
            DateTime denngay;
            tungay = Convert.ToDateTime(NgayNhapS);
            denngay = Convert.ToDateTime(NgayNhapF);
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.PhieuXuats.Where(r => r.NgayXuat >= tungay && r.NgayXuat <= denngay || NgayNhapS == null && NgayNhapF == null).ToList().OrderBy(n => n.MaPhieuX).ToPagedList(pageNumber, pageSize));

        }



        [HttpGet]
        public ActionResult SuaPX(int? MaPX)
        {
            PhieuXuat px = db.PhieuXuats.SingleOrDefault(n => n.MaPhieuX == MaPX);
            ViewBag.Kho = db.Khos.ToList().OrderBy(n => n.TenKho);
            // ViewBag.MaMatHang = new SelectList(db.MatHangs.ToList().OrderBy(n => n.TenMatHang), "MaMatHang", "TenMatHang", PN.);
            ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
            ViewBag.DonVi = db.DonViGNs.ToList().OrderBy(n => n.TenDonVi);
            if (px == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(px);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SuaPX(PhieuXuat PX)
        {
            ViewBag.Kho = db.Khos.ToList().OrderBy(n => n.TenKho);
            // ViewBag.MaMatHang = new SelectList(db.MatHangs.ToList().OrderBy(n => n.TenMatHang), "MaMatHang", "TenMatHang", PN.);
            ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
            ViewBag.DonVi = db.DonViGNs.ToList().OrderBy(n => n.TenDonVi);
            //Thêm vào cơ sở dữ liệu
            if (ModelState.IsValid)
            {
                //Thực hiện cập nhận trong model
                db.Entry(PX).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(PX.TenPhieuX + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                return RedirectToAction("XemPhieuXuat", "PhieuNhapXuat");
            }
            return View(PX);
        }


        public ActionResult XoaPX(int MaPX)
        {

            PhieuXuat px2 = db.PhieuXuats.Find(MaPX);

            var ctx2 = (from ctx in db.ChiTietPhieuXuats
                        where MaPX == ctx.MaPhieuX
                        select new
                        {
                            ctx.MaHangXuat,
                            ctx.MaMatHang,
                            ctx.SLHangTamX,
                            ctx.MaKho,
                            ctx.MaPhieuX

                        }).AsEnumerable();

            if (px2 == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            foreach (var i in ctx2)
            {
                var chitiet = db.ChiTietPhieuXuats.FirstOrDefault(s => s.MaPhieuX == MaPX);
                if (chitiet != null)
                {

                    db.ChiTietPhieuXuats.Remove(chitiet);
                    db.SaveChanges();
                }

            }

            Logging log = new Logging();
            log.SaveLog(px2.TenPhieuX + "-" + db.HanhDongs.Where(n => n.MaAction == 3).FirstOrDefault().TenAction, 3, int.Parse(Session["MaThanhVien"].ToString()));

            db.PhieuXuats.Remove(px2);
            db.SaveChanges();

            return RedirectToAction("XemPhieuXuat", "PhieuNhapXuat");

        }


        public ActionResult ThemPX(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
            //ViewBag.MaKho = new SelectList(db.Khos.ToList().OrderBy(n => n.MaKho), "MaKho", "TenKho");
            ViewBag.MaKho = db.Khos.ToList().OrderBy(n => n.TenKho);
            ViewBag.DonVi = db.DonViGNs.ToList().OrderBy(n => n.TenDonVi);
            ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH");
            //    ViewBag.StateList = db.LoaiMatHangs;
            //   ViewBag.Kho = db.Khos;
            var model = new ChiTietPhieuNhap();
            return View(model);

            //  return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemPX(PhieuXuat px, FormCollection f)
        {
            ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
            //    ViewBag.MaKho = new SelectList(db.Khos.ToList().OrderBy(n => n.MaKho), "MaKho", "TenKho");
            ViewBag.MaKho = db.Khos.ToList().OrderBy(n => n.TenKho);
            ViewBag.DonVi = db.DonViGNs.ToList().OrderBy(n => n.TenDonVi);
            ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH");
            ViewBag.StateList = db.LoaiMatHangs;
            var count = db.PhieuXuats.Count(n => n.TenPhieuX == px.TenPhieuX);
            if (ModelState.IsValid)
            {
                if (count == 0)
                {
                    db.PhieuXuats.Add(px);
                    db.SaveChanges();



                    ViewBag.MaPhieuX = db.PhieuXuats.SingleOrDefault(n => n.TenPhieuX == px.TenPhieuX).MaPhieuX;
                    string ids = f["dataput"];
                    char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                    string[] words = ids.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                    foreach (string word in words)
                    {
                        if (word != " " || word != ",")
                        {
                            int a = Int32.Parse(word);
                            ChiTietPhieuXuat mhk = db.ChiTietPhieuXuats.SingleOrDefault(n => n.MaHangXuat == a);
                            mhk.MaPhieuX = px.MaPhieuX;
                            mhk.MaKho = px.MaKho;
                            db.Entry(mhk).State = EntityState.Modified;

                            MatHang mh = db.MatHangs.Where(s => s.MaMatHang == mhk.MaMatHang && mhk.MaPhieuX==px.MaPhieuX).FirstOrDefault();
                            mh.TongSoLuong = mh.TongSoLuong - mhk.SLHangTamX;
                            db.Entry(mh).State = EntityState.Modified;

                            db.SaveChanges();
                        }

                    }
                    Logging log = new Logging();
                    log.SaveLog(px.TenPhieuX + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));

                }
                else
                {
                    ViewBag.Message = "Tên phiếu đã tồn tại!!!";
                    ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
                    ViewBag.Kho = db.Khos.ToList().OrderBy(n => n.TenKho);
                    ViewBag.DonVi = db.DonViGNs.ToList().OrderBy(n => n.TenDonVi);
                    ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH");
                    ViewBag.StateList = db.LoaiMatHangs;
                    var model = new MatHang();
                    return View(model);
                }
            }
            return RedirectToAction("XemPhieuXuat", "PhieuNhapXuat");

        }
        
      

        [HttpGet]
        public JsonResult CustomerList(int MaKho)
        {

            var result = from r in db.ChiTietPhieuNhaps
                         join mh in db.MatHangs on r.MaMatHang equals mh.MaMatHang
                         where r.MaKho == MaKho
                         //   group by r.MaMatHang
                         select new { r.MaMatHang, r.SLHangTamN, r.GiaHangN, mh.DonViTinh, mh.TenMatHang };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

       
        [HttpPost]
        public JsonResult LuuChon(int MaMH, int sl, int ma_kho, string tenpn)
        {

            var k = db.ChiTietPhieuNhaps.Where(a => a.MaMatHang == MaMH && a.SLHangTamN > 0 && a.SLHangTamN >= sl).FirstOrDefault();
            if (k != null)
            {
                k.SLHangTamN = k.SLHangTamN - sl;
                db.Entry(k).State = EntityState.Modified;
            }

            db.SaveChanges();
            var k1 = db.PhieuNhaps.Where(a => a.TenPhieuN == tenpn || k.MaPhieuN == a.MaPhieuN).FirstOrDefault();
            if (k1 != null)
            {
                k1.SoLuongN = k1.SoLuongN - sl;
                db.Entry(k1).State = EntityState.Modified;
            }

            db.SaveChanges();
            ChiTietPhieuXuat mhk = new ChiTietPhieuXuat();
            mhk.MaKho = ma_kho;
            mhk.MaMatHang = MaMH;
            if (sl > 0)
            {
                mhk.SLHangTamX = sl;
            }
            db.ChiTietPhieuXuats.Add(mhk);

            db.SaveChanges();
            int id = db.ChiTietPhieuXuats.OrderByDescending(i => i.MaHangXuat).FirstOrDefault().MaHangXuat;

            var result = new { Success = "True", Message = id };
            return Json(result, JsonRequestBehavior.AllowGet);
        }



        public ActionResult ExportPXtoPDF()
        {
            return new ActionAsPdf("XemPhieuXuat")
            {
                FileName = Server.MapPath("~/Content/PhieuXuat.pdf")
            };

        }

        //Xem chi tiet Xuat
        public ActionResult XemCTPX(int? page, int MaPX)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            ViewBag.CTPX = MaPX;
            return View(db.ChiTietPhieuXuats.Where(p => p.MaPhieuX == MaPX && p.SLHangTamX>0).ToList().OrderBy(n => n.MaPhieuX).ToPagedList(pageNumber, pageSize));
        }



        //============================       
        //PHIẾU NHẬP
        public ActionResult XemPhieuNhap(int? page, String NgayNhapS, String NgayNhapF)
        {
            DateTime tungay;
            DateTime denngay;
            tungay = Convert.ToDateTime(NgayNhapS);
            denngay = Convert.ToDateTime(NgayNhapF);
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            return View(db.PhieuNhaps.Where(r => r.NgayNhap >= tungay && r.NgayNhap <= denngay || NgayNhapS == null && NgayNhapF == null).ToList().OrderBy(n => n.MaPhieuN).ToPagedList(pageNumber, pageSize));

        }
        [HttpGet]
        public ActionResult SuaPN(int? MaPhieuN)
        {
            PhieuNhap PN = db.PhieuNhaps.Find(MaPhieuN);
            ViewBag.Kho = db.Khos.ToList().OrderBy(n => n.TenKho);        
            ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
            ViewBag.MaHopDong = db.HopDongs.ToList().OrderBy(n => n.SoHopDong);
            ViewBag.HD = db.HopDongs.ToList().OrderBy(n => n.SoHopDong);
            if (PN == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            return View(PN);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SuaPN(PhieuNhap PN)
        {
            ViewBag.Kho = db.Khos.ToList().OrderBy(n => n.TenKho);        
            ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
            ViewBag.HD = db.HopDongs.ToList().OrderBy(n => n.SoHopDong);
            if (ModelState.IsValid)
            {

                db.Entry(PN).State = EntityState.Modified;
                db.SaveChanges();
                Logging log = new Logging();
                log.SaveLog(PN.TenPhieuN + "-" + db.HanhDongs.Where(n => n.MaAction == 2).FirstOrDefault().TenAction, 2, int.Parse(Session["MaThanhVien"].ToString()));
                return RedirectToAction("XemPhieuNhap", "PhieuNhapXuat");
            }
            return View(PN);
        }

        public ActionResult XoaPN(int MaPhieuN)
        {
            PhieuNhap px2 = db.PhieuNhaps.Find(MaPhieuN);

            var ctx2 = (from ctx in db.ChiTietPhieuNhaps
                        where MaPhieuN == ctx.MaPhieuN
                        select new
                        {
                            ctx.MaHangNhap,
                            ctx.MaMatHang,
                            ctx.SLHangTamN,
                            ctx.MaKho,
                            ctx.MaPhieuN
                        }).AsEnumerable();

            if (px2 == null)
            {
                Response.StatusCode = 404;
                return null;
            }

            foreach (var i in ctx2)
            {
                var chitiet = db.ChiTietPhieuNhaps.FirstOrDefault(s => s.MaPhieuN == MaPhieuN);
                if (chitiet != null)
                {
                    db.ChiTietPhieuNhaps.Remove(chitiet);
                    db.SaveChanges();
                }

            }
            Logging log = new Logging();
            log.SaveLog(px2.TenPhieuN + "-" + db.HanhDongs.Where(n => n.MaAction == 3).FirstOrDefault().TenAction, 3, int.Parse(Session["MaThanhVien"].ToString()));
            db.PhieuNhaps.Remove(px2);
            db.SaveChanges();
            return RedirectToAction("XemPhieuNhap", "PhieuNhapXuat");
        }

        public ActionResult ThemPN(int? page)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 5;
            ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
            ViewBag.Kho = db.Khos.ToList().OrderBy(n => n.TenKho);
            ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH");
            ViewBag.MaHopDong = db.HopDongs.ToList().OrderBy(n => n.SoHopDong);
            ViewBag.StateList = db.LoaiMatHangs;
            ViewBag.StateList2 = db.HopDongs;
            ViewBag.StateList3 = db.ThanhViens;
            ViewBag.StateList4 = db.Khos;
            var model = new ChiTietHopDong();
            return View(model);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ThemPN(PhieuNhap pn, FormCollection f, DateTime ngaynhap)
        {
            ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
            ViewBag.Kho = db.Khos.ToList().OrderBy(n => n.TenKho);
            ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH");
            ViewBag.MaHopDong = db.HopDongs.ToList().OrderBy(n => n.SoHopDong);
            var count = db.PhieuNhaps.Count(n => n.TenPhieuN == pn.TenPhieuN);
            if (ModelState.IsValid)
            {
                var chon = db.HopDongs.Where(p => p.MaHopDong == pn.MaHopDong).FirstOrDefault();
                if (count == 0)
                {
                    if (chon != null)
                    {
                        if (ngaynhap >= chon.NgayKy && ngaynhap <= chon.NgayKT)
                        {

                            db.PhieuNhaps.Add(pn);
                            db.SaveChanges();
                            string ids = f["dataput"];
                            char[] delimiterChars = { ' ', ',', '.', ':', '\t' };
                            string[] words = ids.Split(delimiterChars, System.StringSplitOptions.RemoveEmptyEntries);
                            foreach (string word in words)
                            {
                                if (word != " " || word != ",")
                                {
                                    int a = Int32.Parse(word);
                                    ChiTietPhieuNhap mhk = db.ChiTietPhieuNhaps.SingleOrDefault(n => n.MaHangNhap == a);
                                    mhk.MaPhieuN = pn.MaPhieuN;
                                    mhk.MaKho = pn.MaKho;
                                    db.Entry(mhk).State = EntityState.Modified;

                                    MatHang mh = db.MatHangs.Where(s => s.MaMatHang == mhk.MaMatHang && mhk.MaPhieuN==pn.MaPhieuN).FirstOrDefault();
                                    mh.TongSoLuong = mh.TongSoLuong + mhk.SLHangTamN;
                                    mh.Gia = mhk.GiaHangN;
                                    mh.NgaySX = mhk.NgaySX;
                                    mh.HanSD = mhk.HanSD;
                                    db.Entry(mh).State = EntityState.Modified;
                                    db.SaveChanges();
                                }

                            }
                            Logging log = new Logging();
                            log.SaveLog(pn.TenPhieuN + "-" + db.HanhDongs.Where(n => n.MaAction == 1).FirstOrDefault().TenAction, 1, int.Parse(Session["MaThanhVien"].ToString()));
                            return RedirectToAction("XemPhieuNhap", "PhieuNhapXuat");
                        }
                        else
                        {
                            ViewBag.Message = " Vui lòng nhập ngày trong khoảng " + chon.NgayKy + " đến " + chon.NgayKT;
                            ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
                            ViewBag.Kho = db.Khos.ToList().OrderBy(n => n.TenKho);
                            ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH");
                            ViewBag.HD = db.HopDongs.ToList().OrderBy(n => n.MaHopDong);
                            ViewBag.StateList = db.LoaiMatHangs;
                            ViewBag.StateList2 = db.HopDongs;
                            ViewBag.StateList3 = db.ThanhViens;
                            ViewBag.StateList4 = db.Khos;
                            var model = new ChiTietHopDong();
                            return View(model);
                        }
                    }
                    else
                    {
                        ViewBag.Message = "khong co hop dong";

                    }
                }
                else
                {
                    ViewBag.Message1 = "Tên phiếu đã tồn tại!!!";
                    ViewBag.ThanhVien = db.ThanhViens.ToList().OrderBy(n => n.FullName);
                    ViewBag.Kho = db.Khos.ToList().OrderBy(n => n.TenKho);
                    ViewBag.MaLoaiMH = new SelectList(db.LoaiMatHangs.ToList().OrderBy(n => n.TenLoaiMH), "MaLoaiMH", "TenLoaiMH");
                    ViewBag.HD = db.HopDongs.ToList().OrderBy(n => n.MaHopDong);
                    ViewBag.StateList = db.LoaiMatHangs;
                    ViewBag.StateList2 = db.HopDongs;
                    ViewBag.StateList3 = db.ThanhViens;
                    ViewBag.StateList4 = db.Khos;
                    var model = new ChiTietHopDong();
                    return View(model);
                }

            }

            return View();

        }


        [HttpGet]
        public JsonResult DSMatHang(int MaHopDong)
        {
            var result = from hd in db.ChiTietHopDongs
                         join mh in db.MatHangs on hd.MaMatHang equals mh.MaMatHang
                         where hd.MaHopDong == MaHopDong
                         //   group by r.MaMatHang
                         select new { hd.MaMatHang, mh.TenMatHang, mh.NgaySX, mh.HanSD, hd.SoLuong, hd.Gia };
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ThemSL(int MaMH, int sl, int gia, DateTime ngaysx, DateTime hansd, int mahd)
        {

            //var k = db.MatHangs.Where(a => a.MaMatHang == MaMH && sl>0).FirstOrDefault();
            //if (k != null)
            //{
            //    k.TongSoLuong = k.TongSoLuong + sl;
            //    k.Gia = gia;
            //}
            var k = db.ChiTietHopDongs.Where(a => a.MaMatHang == MaMH && a.SoLuong > 0 && a.SoLuong >= sl && a.MaHopDong == mahd).FirstOrDefault();
            if (k != null)
            {
                k.SoLuong = k.SoLuong - sl;
                db.Entry(k).State = EntityState.Modified;
            }

            db.SaveChanges();


            ChiTietPhieuNhap mhk = new ChiTietPhieuNhap();

            mhk.MaMatHang = MaMH;
            mhk.GiaHangN = gia;
            mhk.NgaySX = ngaysx;
            mhk.HanSD = hansd;
            if (sl > 0)
            {
                mhk.SLHangTamN = sl;
            }
            db.ChiTietPhieuNhaps.Add(mhk);
            db.SaveChanges();
            int id = db.ChiTietPhieuNhaps.OrderByDescending(i => i.MaHangNhap).FirstOrDefault().MaHangNhap;
            var result = new { Success = "True", MaPhieuN = id };
            return Json(result, JsonRequestBehavior.AllowGet);
        }


        //Xem chi tiet Nhap
        public ActionResult XemCTPN(int? page, int MaPhieuN)
        {
            int pageNumber = (page ?? 1);
            int pageSize = 20;
            ViewBag.CTPN = MaPhieuN;
            return View(db.ChiTietPhieuNhaps.Where(p => p.MaPhieuN == MaPhieuN && p.SLHangTamN>0).ToList().OrderBy(n => n.MaPhieuN).ToPagedList(pageNumber, pageSize));
        }

    }
}