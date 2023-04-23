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
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Globalization;

namespace CWTDashboardAPI.Controllers
{
    public class CRMTeamController : Controller
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        // GET: CRMTeam
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        OleDbConnection Econ;
        Response re = new Response();
        ManualDataController manualData = new ManualDataController();
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult GetCLRData()
        {

            var Datalist4 = (from a in entity.CLRDatas
                             where a.RevenueID > 600000000000000
                             join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID into ab
                             from abc in ab.DefaultIfEmpty()
                             select new
                             {
                                 a.CLRID,
                                 ManualID = abc.ManualID,
                                 Revenue_ID = a.RevenueID,
                                 Client = a.Client == "" ? "---" : a.Client ?? "---",
                                 CountryCode = "---",
                                 Pipeline_status = "---",
                                 Service_configuration = a.Service_Configuration == "" || a.Service_Configuration == null ? "---" : a.Service_Configuration,
                                 OBT_Reseller___Direct = a.OBTReseller == "" || a.OBTReseller == null ? "---" : a.OBTReseller ?? "---",
                                 GoLiveDate = a.GoLiveDate,
                                 ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                 Opportunity_Type = "---",
                                 Revenue_Status = "---",
                                 RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                             }).ToList();
            var Datalist1 = (from a in entity.CLRDatas
                             where a.ProjectStatus != "P-Pipeline"
                             where a.ProjectStatus != "HP-High Potential"
                             where a.ProjectStatus != "EP-Early Potential"
                             where a.RevenueID != 400000000000000
                             where a.RevenueID < 600000000000000
                             //join c in entity.DigitalTeams on a.RevenueID equals c.RevenueID
                             join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID into ab
                             from abc in ab.DefaultIfEmpty()
                             select new
                             {
                                 a.CLRID,
                                 ManualID = abc.ManualID,
                                 Revenue_ID = abc.Revenue_ID ?? 0,
                                 Client = a.Client == "" ? "---" : a.Client ?? "---",
                                 CountryCode = a.CountryCode == "" ? "---" : a.CountryCode ?? "---",
                                 Pipeline_status = abc.Pipeline_status == "" ? "---" : abc.Pipeline_status ?? "---",
                                 Service_configuration = a.Service_Configuration == "" || a.Service_Configuration == null ? "---" : a.Service_Configuration,
                                 OBT_Reseller___Direct = a.OBTReseller == "" || a.OBTReseller == null ? "---" : a.OBTReseller ?? "---",
                                 GoLiveDate = a.GoLiveDate,
                                 ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                 Opportunity_Type = a.Opportunity_Type == "" ? "---" : a.Opportunity_Type ?? "---",
                                 Revenue_Status = a.Revenue_Status == "" ? "---" : a.Revenue_Status ?? "---",
                                 RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                             }).ToList();
            var Datalist3 = (from a in entity.CLRDatas
                             where a.RevenueID == 400000000000000
                             //join c in entity.DigitalTeams on a.Task__Task_Record_ID_Key equals c.TaskRecordIdKey
                             join b in entity.ManualDatas on a.Task__Task_Record_ID_Key equals b.TaskRecordIdKey into ab
                             from abc in ab.DefaultIfEmpty()
                             select new
                             {
                                 a.CLRID,
                                 ManualID = abc.ManualID,
                                 Revenue_ID = abc.Revenue_ID ?? 0,
                                 Client = a.Client == "" ? "---" : a.Client ?? "---",
                                 CountryCode = a.CountryCode == "" ? "---" : a.CountryCode ?? "---",
                                 Pipeline_status = abc.Pipeline_status == "" ? "---" : abc.Pipeline_status ?? "---",
                                 Service_configuration = a.Service_Configuration == "" || a.Service_Configuration == null ? "---" : a.Service_Configuration,
                                 OBT_Reseller___Direct = a.OBTReseller == "" || a.OBTReseller == null ? "---" : a.OBTReseller ?? "---",
                                 GoLiveDate = a.GoLiveDate,
                                 ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                 Opportunity_Type = a.Opportunity_Type == "" ? "---" : a.Opportunity_Type ?? "---",
                                 Revenue_Status = a.Revenue_Status == "" ? "---" : a.Revenue_Status ?? "---",
                                 RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                             }).ToList();
            var Datalist2 = (from a in entity.CLRDatas
                             where a.ProjectStatus == "P-Pipeline" || a.ProjectStatus == "HP-High Potential" || a.ProjectStatus == "EP-Early Potential"
                             where a.RevenueID != 400000000000000
                             where a.RevenueID < 600000000000000
                             //join c in entity.DigitalTeams on a.RevenueID equals c.RevenueID
                             join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID into ab
                             from abc in ab.DefaultIfEmpty()
                             select new
                             {
                                 a.CLRID,
                                 ManualID = abc.ManualID,
                                 Revenue_ID = abc.Revenue_ID ?? 0,
                                 Client = a.Client == "" ? "---" : a.Client ?? "---",
                                 CountryCode = a.CountryCode == "" ? "---" : a.CountryCode ?? "---",
                                 Pipeline_status = abc.Pipeline_status == "" ? "---" : abc.Pipeline_status ?? "---",
                                 Service_configuration = a.Service_Configuration == "" || a.Service_Configuration == null ? "---" : a.Service_Configuration,
                                 OBT_Reseller___Direct = a.OBTReseller == "" || a.OBTReseller == null ? "---" : a.OBTReseller ?? "---",
                                 GoLiveDate = abc.GoLiveDate == null ? a.GoLiveDate : abc.GoLiveDate,
                                 ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                 Opportunity_Type = a.Opportunity_Type == "" ? "---" : a.Opportunity_Type ?? "---",
                                 Revenue_Status = a.Revenue_Status == "" ? "---" : a.Revenue_Status ?? "---",
                                 RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                             }).ToList();
            var Datalist = Datalist1.Concat(Datalist2).Concat(Datalist3).Concat(Datalist4);
            DataTable table = new DataTable();
            table.Columns.Add(new DataColumn("Client", typeof(string)));
            table.Columns.Add(new DataColumn("Country Code", typeof(string)));
            table.Columns.Add(new DataColumn("Pipeline status", typeof(string)));
            table.Columns.Add(new DataColumn("Service configuration", typeof(string)));
            table.Columns.Add(new DataColumn("OBT Type", typeof(string)));
            table.Columns.Add(new DataColumn("Task Go Live Date", typeof(string)));
            table.Columns.Add(new DataColumn("iMeet Milestone Project Status", typeof(string)));
            table.Columns.Add(new DataColumn("Opportunity Type", typeof(string)));
            table.Columns.Add(new DataColumn("Revenue Status", typeof(string)));
            //table.Columns.Add(new DataColumn("Revenue ID", typeof(double)));
            //table.Columns.Add(new DataColumn("Record Status", typeof(string)));
            foreach (var r in Datalist)
            {
                DataRow dr3 = table.NewRow();
                dr3["Client"] = r.Client == "---" ? "" : r.Client;
                dr3["Country Code"] = r.CountryCode == "---" ? "" : r.CountryCode;
                dr3["Pipeline status"] = r.Pipeline_status == "---" ? "" : r.Pipeline_status;
                dr3["Service configuration"] = r.Service_configuration == "---" ? "" : r.Service_configuration;
                dr3["OBT Type"] = r.OBT_Reseller___Direct == "---" ? "" : r.OBT_Reseller___Direct;
                dr3["Task Go Live Date"] = (r.GoLiveDate)?.ToString("dd-MM-yyyy", CultureInfo.InvariantCulture);
                dr3["iMeet Milestone Project Status"] = r.ProjectStatus == "---" ? "" : r.ProjectStatus;
                dr3["Opportunity Type"] = r.Opportunity_Type == "---" ? "" : r.Opportunity_Type;
                dr3["Revenue Status"] = r.Revenue_Status == "---" ? "" : r.Revenue_Status;
                //dr3["Revenue ID"] = r.Revenue_ID;
                //dr3["Record Status"] = r.RecordStatus == "---" ? "" : r.RecordStatus;
                table.Rows.Add(dr3);
            }
            Response.Clear();
            Response.Buffer = true;
            Response.AddHeader("content-disposition", "attachment;filename=Country_Level_Report.csv");
            Response.Charset = "";
            Response.ContentType = "application/text";
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < table.Columns.Count; k++)
            {
                //add separator
                if(k == (table.Columns.Count - 1))
                {
                    sb.Append(table.Columns[k].ColumnName);
                }
                else
                {
                    sb.Append(table.Columns[k].ColumnName + ',');
                }
            }
            //append new line
            sb.Append("\r\n");
            for (int i = 0; i < table.Rows.Count; i++)
            {
                for (int k = 0; k < table.Columns.Count; k++)
                {
                    //add separator
                    if (k == (table.Columns.Count - 1))
                    {
                        sb.Append(table.Rows[i][k].ToString().Replace(",", ";"));
                    }
                    else
                    {
                        sb.Append(table.Rows[i][k].ToString().Replace(",", ";") + ',');
                    }
                }
                //append new line
                sb.Append("\r\n");
            }
            Response.Output.Write(sb.ToString());
            Response.Flush();
            Response.End();
            ViewBag.Error1 = "Success";
            return View("Index");
        }
    }
}