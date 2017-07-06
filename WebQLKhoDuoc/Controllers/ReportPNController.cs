using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using WebQLKhoDuoc.Models;  

namespace WebQLKhoDuoc.Controllers
{
    public class ReportPNController : Controller
    {
        //
        // GET: /ReportPN/
        public ActionResult Index()
        {
            return View();
        }


        //protected void txtshow_Click(object sender, EventArgs e)
        //{
        //    showreport();
        //}




        //private void showreport()
        //{
        //    //reset
        //    rptViewer.Reset();
        //    //datasource
        //    DataTable dt = GetData(DateTime.Parse(txttungay.Text), DateTime.Parse(txtdenngay.Text));
        //    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
        //    rptViewer.LocalReport.DataSources.Add(rds);
        //    //path
        //    rptViewer.LocalReport.ReportPath = "Report.rdlc";
        //    //Parammeters
        //    ReportParameter[] rptParams = new ReportParameter[]
        //{
        //    new ReportParameter("TuNgay",txttungay.Text),
        //    new ReportParameter("DenNgay",txtdenngay.Text)
        //};
        //    rptViewer.LocalReport.SetParameters(rptParams);
        //    //refresh
        //    rptViewer.LocalReport.Refresh();
        //}
        private DataTable GetData(DateTime Tu, DateTime Den)
        {
            DataTable dt = new DataTable();
            String constr = System.Configuration.ConfigurationManager.ConnectionStrings["WebQLKhoDuocConnectionString"].ConnectionString;
            using (SqlConnection cn = new SqlConnection(constr))
            {
                SqlCommand cmd = new SqlCommand("GetPhieuNhapbyDates", cn);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.Add("@TuNgay", SqlDbType.DateTime).Value = Tu;
                cmd.Parameters.Add("@DenNgay", SqlDbType.DateTime).Value = Den;

                SqlDataAdapter adp = new SqlDataAdapter(cmd);
                adp.Fill(dt);
            }

            return dt;
        }


        DataSet ds = new DataSet();
        public ActionResult ReportPN()
        {
            ReportViewer reportViewer = new ReportViewer();
            reportViewer.ProcessingMode = ProcessingMode.Local;
            reportViewer.SizeToReportContent = true;
            reportViewer.Width = Unit.Percentage(900);
            reportViewer.Height = Unit.Percentage(900);

            //reset
            reportViewer.Reset();
            //datasource
        //    DataTable dt = GetData(DateTime.Parse(txttungay.Text), DateTime.Parse(txtdenngay.Text));
        //    ReportDataSource rds = new ReportDataSource("DataSet1", dt);
           
        //    //path
        //   // reportViewer.LocalReport.ReportPath = "Report.rdlc";
        //    //Parammeters
        //    ReportParameter[] rptParams = new ReportParameter[]
        //{
        //    new ReportParameter("TuNgay",txttungay.Text),
        //    new ReportParameter("DenNgay",txtdenngay.Text)
        //};
        //    reportViewer.LocalReport.SetParameters(rptParams);
            //refresh
          //  reportViewer.LocalReport.Refresh();

         //   reportViewer.LocalReport.ReportPath = Request.MapPath(Request.ApplicationPath) + @"Reports\Report.rdlc";
         ////   reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSet", ds.Tables[0]));
         //   reportViewer.LocalReport.DataSources.Add(rds);

         //   ViewBag.ReportViewer = reportViewer;

            return View();
        }  
	}
}