using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using System.Web.Mvc;
using WebQLKhoDuoc.Context;
using Excel = Microsoft.Office.Interop.Excel;
using WebQLKhoDuoc.Models;
using Rotativa;
using ExportExcel.Code;
using System.Globalization;

namespace WebQLKhoDuoc.Controllers
{
    [WebQLKhoDuoc.Models.Authorization]
    public class BaoCaoController : Controller
    {

        QLKhoDuocContext db = new QLKhoDuocContext();
        //
        // GET: /BaoCao/
        public ActionResult Index()
        {
           
            return View();        
          
        }
     
      [HttpGet]
      public JsonResult DonViList2(DateTime tungay, DateTime denngay)
      {
          var result = (from r in db.PhieuXuats
                        join p in db.DonViGNs on r.MaDonVi equals p.MaDonVi                      
                        where (r.NgayXuat>= tungay && r.NgayXuat<=denngay)
                        select new { p.TenDonVi, r.MaDonVi }).Distinct();
          return Json(result, JsonRequestBehavior.AllowGet);
      }
   
      public ActionResult BaoCaoHangXuat()
      {

          ViewBag.MaKho = new SelectList(db.Khos.ToList().OrderBy(n => n.TenKho), "MaKho", "TenKho");
          ViewBag.MaDonVi = new SelectList(db.DonViGNs.ToList().OrderBy(n => n.TenDonVi), "MaDonVi", "TenDonVi");
          ViewBag.MaPhieuX = new SelectList(db.PhieuXuats.ToList().OrderBy(n => n.MaPhieuX), "MaPhieuX", "NgayXuat");
         
          return View();
      }
                //Export excel
      public ActionResult ChiTietBCX(int MaDV, DateTime tungay, DateTime denngay)
      {
          ViewBag.tungay = DateTime.Parse(Request.Params["tungay"]);
          ViewBag.denngay = DateTime.Parse(Request.Params["denngay"]);
          ViewBag.MaDV2 = Int32.Parse(Request.Params["MaDV"]);
        

          var model = (from px in db.PhieuXuats                    
                       join ctx in db.ChiTietPhieuXuats on px.MaPhieuX equals ctx.MaPhieuX
                       join mh in db.MatHangs on ctx.MaMatHang equals mh.MaMatHang
                       where (MaDV == px.MaDonVi && px.NgayXuat >= tungay && px.NgayXuat <= denngay)
                       select new
                       {
                           TenMatHang = mh.TenMatHang,
                           SLHangTamX = ctx.SLHangTamX,
                           DonViTinh = mh.DonViTinh,
                           Gia = mh.Gia,
                           TenKho = px.Kho.TenKho,
                           TenDonVi = px.DonViGN.TenDonVi,
                           NgayXuat = px.NgayXuat,
                           ThanhTien = ctx.SLHangTamX * mh.Gia,
                           HTTT=px.HTTT,


                       }).ToList();


          ViewBag.tong = model.Sum(k => k.ThanhTien);

        List<BaoCaoX> bclist = new List<BaoCaoX>();

        foreach (var i in model)
        {
            bclist.Add(new BaoCaoX
            {

                TenMatHang = i.TenMatHang,
                SLHangTamX = i.SLHangTamX,
                DonViTinh = i.DonViTinh,
                Gia = i.Gia,
                TenKho = i.TenKho,
                TenDonVi = i.TenDonVi,
                NgayXuat = i.NgayXuat,
                ThanhTien = i.ThanhTien,
                HTTT = i.HTTT

            });
        }

        return View(bclist);
          
      }
      
    
        //Export excel 2
      //--------------------------------------
      [HttpGet]
      public FileContentResult ExportToExcel(int MaDV, DateTime tungay, DateTime denngay)
      {
           
          var model = (from px in db.PhieuXuats
                       join ctx in db.ChiTietPhieuXuats on px.MaPhieuX equals ctx.MaPhieuX
                       join mh in db.MatHangs on ctx.MaMatHang equals mh.MaMatHang
                       where (MaDV == px.MaDonVi && px.NgayXuat >= tungay && px.NgayXuat <= denngay)
                       select new
                       {
                            
                           TenMatHang = mh.TenMatHang,
                           SLHangTamX = ctx.SLHangTamX,
                           DonViTinh = mh.DonViTinh,
                           Gia = mh.Gia,
                           TenKho = px.Kho.TenKho,
                           TenDonVi = px.DonViGN.TenDonVi,
                           ThanhTien = ctx.SLHangTamX * mh.Gia,
                           HTTT = px.HTTT,
                           NgayXuat = px.NgayXuat


                       }).ToList();
          decimal giatong = model.Sum(k => k.ThanhTien);
          var model2= (from e in model.AsEnumerable()
             select new {
                           TenMatHang = e.TenMatHang,
                           SLHangTamX = e.SLHangTamX,
                           DonViTinh = e.DonViTinh,
                           Gia = e.Gia,
                           TenKho = e.TenKho,
                           TenDonVi = e.TenDonVi,
                           NgayXuat = e.NgayXuat.ToString(),
                           ThanhTien = e.ThanhTien,
                           HTTT = e.HTTT,
                        
             }).ToList();

          ViewBag.tong = model.Sum(k => k.ThanhTien);

         string[] columns = { "TenMatHang", "SLHangTamX", "DonViTinh", "Gia", "TenKho", "TenDonVi", "NgayXuat", "ThanhTien", "HTTT" };
              byte[] filecontent = ExcelExportHelper.ExportExcel(model2, "BÁO CÁO CHI TIẾT HÀNG XUẤT", "TỔNG GIÁ TRỊ: " + giatong, true, columns);
          
              return File(filecontent, ExcelExportHelper.ExcelContentType, "Baocaohangxuat.xlsx");
             
   
      }

        //--------------------------------------



         
      //Export excel bo
      //public string ExportPXtoExcel(int MaDV, DateTime tungay, DateTime denngay)
      //{

      //    try
      //    {
      //        Excel.Application application = new Excel.Application();
      //        Excel.Workbook workbook = application.Workbooks.Add(System.Reflection.Missing.Value);
      //        Excel.Worksheet worksheet = workbook.ActiveSheet;
      //        worksheet.Cells[4, 1] = "Tên mặt hàng";
      //        worksheet.Cells[4, 2] = "Số lượng";
      //        worksheet.Cells[4, 3] = "Đơn vị tính";
      //        worksheet.Cells[4, 4] = "Giá";
      //        worksheet.Cells[4, 5] = "Tên kho xuất";
      //        worksheet.Cells[4, 6] = "Tên đơn vị nhận";
      //        worksheet.Cells[4, 7] = "Ngày xuất";
      //        worksheet.Cells[4, 8] = "Tổng giá trị xuất";
      //        worksheet.Cells[4, 9] = "HTTT";

      //        int row = 5;
      //        var model = from px in db.PhieuXuats
      //                    join ctx in db.ChiTietPhieuXuats on px.MaPhieuX equals ctx.MaPhieuX
      //                    join mh in db.MatHangs on ctx.MaMatHang equals mh.MaMatHang
      //                    where (MaDV == px.MaDonVi && px.NgayXuat >= tungay && px.NgayXuat <= denngay)
      //                    select new
      //                    {
      //                        TenMatHang = mh.TenMatHang,
      //                        SLHangTamX = ctx.SLHangTamX,
      //                        DonViTinh = mh.DonViTinh,
      //                        Gia = mh.Gia,
      //                        TenKho = px.Kho.TenKho,
      //                        TenDonVi = px.DonViGN.TenDonVi,
      //                        //  DateTime.ParseExact(NgayXuat, "yyyy-MM-dd HH:mm tt",null) = px.NgayXuat,

      //                        TongGiaTriX = px.TongGiaTriX,
      //                        HTTT = px.HTTT

      //                    };
      //        decimal giatong = model.Sum(k => k.TongGiaTriX);

      //        foreach (var t in model.ToList())
      //        {
      //            worksheet.Cells[row, 1] = t.TenMatHang;
      //            worksheet.Cells[row, 2] = t.SLHangTamX;
      //            worksheet.Cells[row, 3] = t.DonViTinh;
      //            worksheet.Cells[row, 4] = t.Gia;
      //            worksheet.Cells[row, 5] = t.TenKho;
      //            worksheet.Cells[row, 6] = t.TenDonVi;
      //            //  worksheet.Cells[row, 7] = t.NgayXuat.ToString();
      //            worksheet.Cells[row, 8] = t.TongGiaTriX;
      //            worksheet.Cells[row, 9] = t.HTTT;
      //            row++;
      //        }
      //        worksheet.Cells[row, 1] = "Tổng giá:" + giatong;

      //        //Format
      //        worksheet.get_Range("A1", "J1").EntireColumn.AutoFit();
      //        //     var range_heading= 
      //        worksheet.get_Range("a2", "i3").Merge(false);

      //        var chartRange = worksheet.get_Range("a2", "i3");
      //        chartRange.Font.Bold = true;
      //        chartRange.FormulaR1C1 = "BÁO CÁO CHI TIẾT HÀNG XUẤT";
      //        chartRange.HorizontalAlignment = 3;
      //        chartRange.VerticalAlignment = 3;
      //        chartRange.Interior.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.BlueViolet);
      //        chartRange.Font.Color = System.Drawing.ColorTranslator.ToOle(System.Drawing.Color.White);
      //        chartRange.Font.Size = 20;

      //        //   worksheet.get_Range("A1", "J1").Borders.Value();
      //        var a = worksheet.get_Range("A4", "I4");
      //        a.Font.Bold = true;
      //        a.Font.Color = System.Drawing.Color.Black;
      //        a.Font.Size = 13;
      //        Excel.Range formatRange;
      //        formatRange = worksheet.get_Range("A4", "I4");
      //        formatRange.BorderAround(Excel.XlLineStyle.xlContinuous,
      //        Excel.XlBorderWeight.xlMedium, Excel.XlColorIndex.xlColorIndexAutomatic,
      //        Excel.XlColorIndex.xlColorIndexAutomatic);
      //        workbook.SaveAs("E:\\chitiethangxuat.xls");
      //        workbook.Close();
      //        Marshal.ReleaseComObject(workbook);
      //        application.Quit();
      //        Marshal.FinalReleaseComObject(application);
      //        ViewBag.Result = "Done";
      //    }
      //    catch (Exception ex)
      //    {
      //        ViewBag.Result = ex.Message;
      //    }
      //    return "Xuất thành công!!!";

      //}

        //Export pdf bo

        //public ActionResult ExportPXtoPDF(int MaDV, DateTime tungay, DateTime denngay)
        //{
        //    var model = from px in db.PhieuXuats
        //                join ctx in db.ChiTietPhieuXuats on px.MaPhieuX equals ctx.MaPhieuX
        //                join mh in db.MatHangs on ctx.MaMatHang equals mh.MaMatHang
        //                where (MaDV == px.MaDonVi && px.NgayXuat >= tungay && px.NgayXuat <= denngay)
        //                select new
        //                {
        //                    TenMatHang = mh.TenMatHang,
        //                    SLHangTamX = ctx.SLHangTamX,
        //                    DonViTinh = mh.DonViTinh,
        //                    Gia = mh.Gia,
        //                    TenKho = px.Kho.TenKho,
        //                    TenDonVi = px.DonViGN.TenDonVi,
        //                    NgayXuat = px.NgayXuat,
        //                    ThanhTien = px.TongGiaTriX,
        //                    HTTT = px.HTTT

        //                };
        //    decimal giatong = model.Sum(k => k.ThanhTien);

        //    return new ActionAsPdf("ChiTietBCX")
        //    {
        //        FileName = Server.MapPath("~/Content/PhieuXuat.pdf")
        //    };

        //}
        //====================Tao moi DATN====================



      public ActionResult BaoCaoCTX2()
      {

          return View();
      }
      public ActionResult BaoCaoHT()
      {
          return View();
      }
      public ActionResult BaoCaoCTN2()
      {
          return View();
      }
      public ActionResult BCHangHetHan()
      {

          return View();
      }
      public ActionResult BCHieuSuatSuDung()
      {
          return View();
      }
      public ActionResult BCTongGTCapDV()
      {
          return View();
      }

	}
}