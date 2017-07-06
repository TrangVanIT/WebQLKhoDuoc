using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Rotativa;
using WebQLKhoDuoc.Context;
using PagedList;
using Excel = Microsoft.Office.Interop.Excel;
using WebQLKhoDuoc.Models;
using System.Runtime.InteropServices;
using System.IO;
using System.Data;
using System.Text;
using ExportExcel.Code;


namespace WebQLKhoDuoc.Controllers
{
    [WebQLKhoDuoc.Models.Authorization]
    public class BaoCaoNhapController : Controller
    {
        // GET: /BaocaoNhap/
        QLKhoDuocContext db = new QLKhoDuocContext();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult BaoCaoHangNhap()
        {
            ViewBag.MaNhaCC = new SelectList(db.NhaCungCaps.ToList().OrderBy(n => n.TenNhaCC), "MaNhaCC", "TenNhaCC");
            ViewBag.MaKho = new SelectList(db.Khos.ToList().OrderBy(n => n.TenKho), "MaKho", "TenKho");
            ViewBag.MaPhieuN = new SelectList(db.PhieuNhaps.ToList().OrderBy(n => n.MaPhieuN), "MaPhieuN", "NgayNhap");
            return View();
        }
        [HttpGet]
        public JsonResult NhaCCList(DateTime tungay, DateTime denngay)
        {
            var result = (from pn in db.PhieuNhaps
                          join hd in db.HopDongs on pn.MaHopDong equals hd.MaHopDong
                          join ncc in db.NhaCungCaps on hd.MaNhaCC equals ncc.MaNhaCC
                          where (pn.NgayNhap >= tungay && pn.NgayNhap <= denngay)
                          select new { hd.MaNhaCC, ncc.TenNhaCC }).Distinct();
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ChiTietBCN(int Manhacc, DateTime tungay, DateTime denngay)
        {
            ViewBag.MaNhaCC2 = Int32.Parse(Request.Params["Manhacc"]);
            ViewBag.tungay = DateTime.Parse(Request.Params["tungay"]);
            ViewBag.denngay = DateTime.Parse(Request.Params["denngay"]);
            var model = (from pn in db.PhieuNhaps
                         join ctn in db.ChiTietPhieuNhaps on pn.MaPhieuN equals ctn.MaPhieuN
                         join mh in db.MatHangs on ctn.MaMatHang equals mh.MaMatHang                      
                         join hd in db.HopDongs on pn.MaHopDong equals hd.MaHopDong
                         where (Manhacc == hd.MaNhaCC && pn.NgayNhap >= tungay && pn.NgayNhap <= denngay)
                         select new
                         {
                             SoHopDong = hd.SoHopDong,
                             NgayNhap = pn.NgayNhap,
                             TenMatHang = ctn.MatHang.TenMatHang,
                             DonViTinh = mh.DonViTinh,
                             SLHangTamN = ctn.SLHangTamN,
                             Gia = mh.Gia,
                             ThanhTien = ctn.SLHangTamN * mh.Gia,
                             TenNhaCC = hd.NhaCungCap.TenNhaCC,
                             TenKho = pn.Kho.TenKho

                         }).ToList();
            ViewBag.tong = model.Sum(k => k.ThanhTien);
            List<BaoCaoN> bclist = new List<BaoCaoN>();

            foreach (var i in model)
            {
                bclist.Add(new BaoCaoN
                {
                    SoHopDong = i.SoHopDong,
                    NgayNhap = i.NgayNhap,
                    TenMatHang = i.TenMatHang,
                    DonViTinh = i.DonViTinh,
                    SLHangTamN = i.SLHangTamN,
                    Gia = i.Gia,
                    ThanhTien = i.ThanhTien,
                    TenNhaCC = i.TenNhaCC,
                    TenKho = i.TenKho
                });
            }
            return View(bclist);
        }
        [HttpGet]
        public FileContentResult ExportToExcel(int Manhacc, DateTime tungay, DateTime denngay)
        {

            var model = (from pn in db.PhieuNhaps
                         join ctn in db.ChiTietPhieuNhaps on pn.MaPhieuN equals ctn.MaPhieuN
                         join mh in db.MatHangs on ctn.MaMatHang equals mh.MaMatHang
                         join hd in db.HopDongs on pn.MaHopDong equals hd.MaHopDong
                         where (Manhacc == hd.MaNhaCC && pn.NgayNhap >= tungay && pn.NgayNhap <= denngay)
                         select new
                         {
                             SoHopDong = hd.SoHopDong,
                             NgayNhap = pn.NgayNhap,
                             TenMatHang = ctn.MatHang.TenMatHang,
                             DonViTinh = mh.DonViTinh,
                             SLHangTamN = ctn.SLHangTamN,
                             Gia = mh.Gia,
                             ThanhTien = ctn.SLHangTamN * mh.Gia,
                             TenNhaCC = hd.NhaCungCap.TenNhaCC,
                             TenKho = pn.Kho.TenKho

                         }).ToList();
            var model2 = (from e in model.AsEnumerable()
                          select new
                          {
                              SoHopDong = e.SoHopDong,
                              NgayNhap = e.NgayNhap.ToString(),
                              TenMatHang = e.TenMatHang,
                              DonViTinh = e.DonViTinh,
                              SLHangTamN = e.SLHangTamN,
                              Gia = e.Gia,
                              ThanhTien = e.SLHangTamN * e.Gia,
                              TenNhaCC = e.TenNhaCC,
                              TenKho = e.TenKho

                          }).ToList();

            decimal tong = model.Sum(k => k.ThanhTien);
            string[] columns = { "SoHopDong", "NgayNhap", "TenMatHang", "DonViTinh", "SLHangTamN", "Gia", "ThanhTien", "TenNhaCC", "TenKho" };
            byte[] filecontent = ExcelExportHelper.ExportExcel(model2, "BÁO CÁO CHI TIẾT HÀNG NHẬP", "TỔNG GIÁ TRỊ: " + tong, true, columns);
            return File(filecontent, ExcelExportHelper.ExcelContentType, "Baocaohangnhap.xlsx");

        }
        
    //    public String ExportPNtoExcel(int Manhacc, DateTime ngaynhap)
    //    {
    //        try
    //        {
    //            Excel.Application application = new Excel.Application();
    //            Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
    //            Excel.Worksheet worksheet = workbook.ActiveSheet;
    //            worksheet.Cells[4, 1] = "Số hợp đồng";
    //            worksheet.Cells[4, 2] = "Ngày nhập";
    //            worksheet.Cells[4, 3] = "Tên mặt hàng";
    //            worksheet.Cells[4, 4] = "Đơn vị tính";
    //            worksheet.Cells[4, 5] = "Số lượng";
    //            worksheet.Cells[4, 6] = "Giá";
    //            worksheet.Cells[4, 7] = "Thành tiền";
    //            worksheet.Cells[4, 8] = "Tên nhà cung cấp";
    //            worksheet.Cells[4, 9] = "Tên kho nhập";
    //            int row = 5;
    //            var model = from pn in db.PhieuNhaps
    //                        join ctn in db.ChiTietPhieuNhaps on pn.MaPhieuN equals ctn.MaPhieuN
    //                        join mh in db.MatHangs on ctn.MaMatHang equals mh.MaMatHang
    //                        //join ct in db.ChiTietHopDongs on mh.MaMatHang equals ct.MaMatHang
    //                        join hd in db.HopDongs on pn.MaHopDong equals hd.MaHopDong
    //                        where (hd.NgayKy <= ngaynhap && hd.NgayKT >= ngaynhap && Manhacc == hd.MaNhaCC)
    //                        select new
    //                        {
    //                            SoHD = hd.SoHopDong,
    //                            NgayNhap = pn.NgayNhap,
    //                            TenMatHang = ctn.MatHang.TenMatHang,
    //                            DVT = mh.DonViTinh,
    //                            SoLuong = ctn.SLHangTamN,
    //                            Gia = mh.Gia,
    //                            ThanhTien = ctn.SLHangTamN * mh.Gia,
    //                            TenNhaCC = hd.NhaCungCap.TenNhaCC,
    //                            KhoNhap = pn.Kho.TenKho
    //                        };
    //            decimal giatong = model.Sum(k => k.ThanhTien);
    //            foreach (var t in model.ToList())
    //            {
    //                worksheet.Cells[row, 1] = t.SoHD;
    //                worksheet.Cells[row, 2] = t.NgayNhap.ToString();
    //                worksheet.Cells[row, 3] = t.TenMatHang;
    //                worksheet.Cells[row, 4] = t.DVT;
    //                worksheet.Cells[row, 5] = t.SoLuong;
    //                worksheet.Cells[row, 6] = t.Gia;
    //                worksheet.Cells[row, 7] = t.ThanhTien;
    //                worksheet.Cells[row, 8] = t.TenNhaCC;
    //                worksheet.Cells[row, 9] = t.KhoNhap;
    //                row++;
    //            }
    //            worksheet.Cells[row, 1] = "Tổng giá:" + giatong;
    //            //Format
    //            worksheet.get_Range("A1", "J1").EntireColumn.AutoFit();
    //            //     var range_heading= 
    //            worksheet.get_Range("a2", "i3").Merge(false);

    //            var chartRange = worksheet.get_Range("a2", "i3");
    //            chartRange.Font.Bold = true;
    //            chartRange.FormulaR1C1 = "BÁO CÁO CHI TIẾT HÀNG NHẬP";
    //            chartRange.HorizontalAlignment = 3;
    //            chartRange.VerticalAlignment = 3;
    //            chartRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.BlueViolet);
    //            chartRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
    //            chartRange.Font.Size = 20;              

    //            //   worksheet.get_Range("A1", "J1").Borders.Value();
    //            var a = worksheet.get_Range("A4", "I8");
    //            a.Font.Bold = true;
    //            a.Font.Color = System.Drawing.Color.Black;
    //            a.Font.Size = 13;
    //            Excel.Range formatRange;
    //            formatRange = worksheet.get_Range("A4", "I8");
    //            formatRange.BorderAround(Excel.XlLineStyle.xlContinuous,
    //            Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic,
    //            Excel.XlColorIndex.xlColorIndexAutomatic);

    //            string Filename = "E:\\chitiethangnhap.xls";
    //            workbook.SaveAs(Filename);

    //            workbook.Close();
         
    //            Marshal.ReleaseComObject(workbook);
    //            application.Quit();
    //            Marshal.FinalReleaseComObject(application);
    //            ViewBag.Result = "Done";
    //            System.Diagnostics.Process.Start(Filename);
    //        }
    //        catch (Exception ex)
    //        {
    //            ViewBag.Result = ex.Message;
    //        }
    //        return "xuat thanh cong";
    //    }
     
    }
}