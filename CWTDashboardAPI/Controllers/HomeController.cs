using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using CWTDashboardAPI.Models;

namespace CWTDashboardAPI.Controllers
{
    public class HomeController : Controller
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        // GET: ExcelImport
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        OleDbConnection Econ;
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            string filename = Guid.NewGuid() + Path.GetExtension(file.FileName);
            string filepath = "/excelfolder/" + filename;
            file.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
            InsertExceldata(filepath, filename);

            return View();
        }
        private void ExcelConn(string filepath)
        {
            string constr = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=""Excel 12.0 Xml;HDR=YES;""", filepath);
            Econ = new OleDbConnection(constr);
        }

        //private void GeneratingClR()
        //{
        //    var GenerateCLR = (from a in entity.iMeetDatas
        //                       join b in entity.CRMDatas on a.Milestone__CRM_Revenue_ID__ equals b.Revenue_Id
        //                       select new
        //                       {
        //                           a.Milestone__CRM_Revenue_ID__,
        //                           b.Revenue_Id,
        //                       }).Distinct();
        //}

        private void InsertExceldata(string fileepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "TestingSheet$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);

            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "tbl_registration";
            objbulk.ColumnMappings.Add("Email", "Email");
            objbulk.ColumnMappings.Add("Password", "Password");
            objbulk.ColumnMappings.Add("Name", "Name");
            objbulk.ColumnMappings.Add("Address", "Address");
            objbulk.ColumnMappings.Add("City", "City");
            con.Open();
            string s = "Truncate Table [tbl_registration]";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.WriteToServer(dt);
            con.Close();
        }
    }
}