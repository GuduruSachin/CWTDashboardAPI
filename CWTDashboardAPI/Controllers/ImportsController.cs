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
//using Excel = Microsoft.Office.Interop.Excel;

namespace CWTDashboardAPI.Controllers
{
    public class ImportsController : Controller
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        // GET: ExcelImport
        SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
        OleDbConnection Econ;
        public ActionResult Index()
        {
            return View();
        }
        public void ExcelConn(string filepath)
        {
            //var connectio = string.Format(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES;'", fileName);
            string constr = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES'", filepath);
            Econ = new OleDbConnection(constr);
        }
        public void InsertdataiMeet(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "iMeet$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "DataiMeet";
            objbulk.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk.ColumnMappings.Add("Task Due Date", "Task Due Date");
            objbulk.ColumnMappings.Add("Milestone Title", "Milestone Title");
            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("Task: Task Type", "Task: Task Type");
            objbulk.ColumnMappings.Add("Task: Task Record ID Key", "Task: Task Record ID Key");
            objbulk.ColumnMappings.Add("Workspace: ELT Overall Status", "Workspace: ELT Overall Status");
            objbulk.ColumnMappings.Add("Workspace: ELT Overall Comments", "Workspace: ELT Overall Comments");
            objbulk.ColumnMappings.Add("Workspace: Project Level", "Workspace: Project Level");
            objbulk.ColumnMappings.Add("Workspace: Project Owner: Full Name", "Workspace: Project Owner: Full Name");
            objbulk.ColumnMappings.Add("Milestone: CRM Revenue ID #", "Milestone: CRM Revenue ID #");
            objbulk.ColumnMappings.Add("Milestone: Country", "Milestone: Country");
            objbulk.ColumnMappings.Add("Milestone: Project Status", "Milestone: Project Status");
            objbulk.ColumnMappings.Add("Milestone: Country Status", "Milestone: Country Status");
            objbulk.ColumnMappings.Add("Completed Date", "Completed Date");
            objbulk.ColumnMappings.Add("% Complete", "% Complete");
            objbulk.ColumnMappings.Add("Milestone Due Date", "Milestone Due Date");
            objbulk.ColumnMappings.Add("Task Status", "Task Status");
            objbulk.ColumnMappings.Add("Task Completed Date", "Task Completed Date"); 
            objbulk.ColumnMappings.Add("Milestone: Region", "Milestone: Region");
            objbulk.ColumnMappings.Add("Milestone: Milestone type", "Milestone: Milestone type");
            objbulk.ColumnMappings.Add("Milestone: Record ID Key", "Milestone: Record ID Key");
            objbulk.ColumnMappings.Add("Milestone: Project Notes", "Milestone: Project Notes");
            objbulk.ColumnMappings.Add("Milestone: Reason Code", "Milestone: Reason Code");
            objbulk.ColumnMappings.Add("Group Name", "Group Name");
            objbulk.ColumnMappings.Add("Milestone: Closed Loop Owner", "Milestone: Closed Loop Owner");
            objbulk.ColumnMappings.Add("Milestone: Cycle Time Delay Code", "Milestone: Cycle Time Delay Code");
            objbulk.ColumnMappings.Add("EltClientDelayDescription", "EltClientDelayDescription");
            con.Open();
            string s = "Truncate Table DataiMeet";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(dt);
            con.Close();
        }
        public void InsertCRMdata(string filepath,string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "CRM$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.BatchSize = 5000;
            objbulk.DestinationTableName = "CRMData";
            objbulk.ColumnMappings.Add("Account Name", "Account Name");
            objbulk.ColumnMappings.Add("Account Owner", "Account Owner");
            objbulk.ColumnMappings.Add("Customer Row ID", "Customer Row ID");
            objbulk.ColumnMappings.Add("Opportunity ID", "Opportunity ID");
            objbulk.ColumnMappings.Add("Description", "Description");
            objbulk.ColumnMappings.Add("BT Current Service Configuration", "BT Current Service Configuration");
            objbulk.ColumnMappings.Add("Opportunity Name", "Opportunity Name");
            objbulk.ColumnMappings.Add("Opportunity Owner", "Opportunity Owner");
            objbulk.ColumnMappings.Add("Close Date", "Close Date");
            objbulk.ColumnMappings.Add("Sales Stage Name", "Sales Stage Name");
            objbulk.ColumnMappings.Add("Line Win Probability", "Line Win Probability");
            objbulk.ColumnMappings.Add("Last Update Date", "Last Update Date");
            objbulk.ColumnMappings.Add("Opportunity Type", "Opportunity Type");
            objbulk.ColumnMappings.Add("Next Step", "Next Step");
            //objbulk.ColumnMappings.Add("Opportunity Scope", "Opportunity Scope");
            objbulk.ColumnMappings.Add("Opportunity Total Transactions", "Opportunity Total Transactions");
            objbulk.ColumnMappings.Add("Opportunity Total Volume USD", "Opportunity Total Volume USD");
            objbulk.ColumnMappings.Add("Awarded Date", "Awarded Date");
            objbulk.ColumnMappings.Add("LOI Date", "LOI Date");
            objbulk.ColumnMappings.Add("Country", "Country");
            objbulk.ColumnMappings.Add("Ownership (Revenue)", "Ownership (Revenue)");
            objbulk.ColumnMappings.Add("Region (Revenue)", "Region (Revenue)");
            objbulk.ColumnMappings.Add("Revenue Total Transactions", "Revenue Total Transactions");
            objbulk.ColumnMappings.Add("Revenue Total Volume USD", "Revenue Total Volume USD");
            //objbulk.ColumnMappings.Add("Client Segment", "Client Segment");
            //objbulk.ColumnMappings.Add("BT Current GDS", "BT Current GDS");
            //objbulk.ColumnMappings.Add("BT Current Online Booking Tool", "BT Current Online Booking Tool");
            //objbulk.ColumnMappings.Add("BT Incumbent", "BT Incumbent");
            //objbulk.ColumnMappings.Add("Opportunity Region", "Opportunity Region");
            //objbulk.ColumnMappings.Add("Country Scope", "Country Scope");
            //objbulk.ColumnMappings.Add("Total Awarded Volume USD", "Total Awarded Volume USD");
            objbulk.ColumnMappings.Add("Total Up-Sell Volume USD", "Total Up-Sell Volume USD");
            objbulk.ColumnMappings.Add("GDS", "GDS");
            objbulk.ColumnMappings.Add("OBT", "OBT");
            objbulk.ColumnMappings.Add("Revenue Opportunity Type", "Revenue Opportunity Type");
            objbulk.ColumnMappings.Add("Revenue Status", "Revenue Status");
            objbulk.ColumnMappings.Add("Revenue Id", "Revenue Id");
            objbulk.ColumnMappings.Add("Created Date", "Created Date");
            objbulk.ColumnMappings.Add("IsImplementationTeamsupport", "IsImplementationTeamsupport");
            con.Open();

            string s = "Truncate Table CRMData";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            try
            {
                objbulk.WriteToServer(dt);
            }
            catch (Exception ex)
            {
                ViewBag.Error3(ex.Message);
            }
            objbulk.NotifyAfter = 20000;
            objbulk.SqlRowsCopied += (sender, e) => Console.WriteLine("RowsCopied: " + e.RowsCopied);
            objbulk.BulkCopyTimeout = 100;
            con.Close();
        }
        public void InsertRolesdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "Roles$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "Roles";
            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("APAC Digital OBT Lead", "APAC Digital OBT Lead");
            objbulk.ColumnMappings.Add("APAC Digital Portrait Lead", "APAC Digital Portrait Lead");
            objbulk.ColumnMappings.Add("APAC Implementation Lead", "APAC Implementation Lead");
            objbulk.ColumnMappings.Add("EMEA CIS OBT Lead", "EMEA CIS OBT Lead");
            objbulk.ColumnMappings.Add("EMEA CIS Portrait Lead", "EMEA CIS Portrait Lead");
            objbulk.ColumnMappings.Add("EMEA Project Manager", "EMEA Project Manager");
            objbulk.ColumnMappings.Add("Global CIS HR Feed Specialist", "Global CIS HR Feed Specialist");
            objbulk.ColumnMappings.Add("Global CIS OBT Lead", "Global CIS OBT Lead");
            objbulk.ColumnMappings.Add("Global CIS Portrait Lead", "Global CIS Portrait Lead");
            objbulk.ColumnMappings.Add("Global Project Manager", "Global Project Manager");
            objbulk.ColumnMappings.Add("LATAM CIS OBT Lead", "LATAM CIS OBT Lead");
            objbulk.ColumnMappings.Add("LATAM CIS Portrait Lead", "LATAM CIS Portrait Lead");
            objbulk.ColumnMappings.Add("LATAM Project Manager", "LATAM Project Manager");
            objbulk.ColumnMappings.Add("NORAM CIS OBT Lead", "NORAM CIS OBT Lead");
            objbulk.ColumnMappings.Add("NORAM CIS Portrait Lead", "NORAM CIS Portrait Lead");
            objbulk.ColumnMappings.Add("NORAM Project Manager", "NORAM Project Manager");
            objbulk.ColumnMappings.Add("Local IN CIS Lead", "Local IN CIS Lead");
            objbulk.ColumnMappings.Add("Global CIS DQS Lead", "Global CIS DQS Lead");
            objbulk.ColumnMappings.Add("APAC_DQS", "APAC_DQS");
            objbulk.ColumnMappings.Add("DQS_Import", "DQS_Import");
            objbulk.ColumnMappings.Add("DQS_Support", "DQS_Support");
            objbulk.ColumnMappings.Add("LATAM_DQS", "LATAM_DQS");
            objbulk.ColumnMappings.Add("NORAM_DQS", "NORAM_DQS");
            objbulk.ColumnMappings.Add("DQS_Operations", "DQS_Operations");
            objbulk.ColumnMappings.Add("LocalImplementationLeadCanada", "LocalImplementationLeadCanada");
            objbulk.ColumnMappings.Add("LocalImplementationLeadUS", "LocalImplementationLeadUS");
            con.Open();
            string s = "Truncate Table Roles";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(dt);
            con.Close();
        }
        public void InsertPSDdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "PSD$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "PSD";
            objbulk.ColumnMappings.Add("Created Date", "Created Date");
            objbulk.ColumnMappings.Add("Last Update Date", "Last Update Date");
            objbulk.ColumnMappings.Add("Requestor", "Requestor");
            objbulk.ColumnMappings.Add("Account Name", "Account Name");
            objbulk.ColumnMappings.Add("Account Owner", "Account Owner");
            objbulk.ColumnMappings.Add("Customer Row ID", "Customer Row ID");
            objbulk.ColumnMappings.Add("Account Category", "Account Category");
            objbulk.ColumnMappings.Add("BT Account Record Type", "BT Account Record Type");
            objbulk.ColumnMappings.Add("Ownership (Revenue)", "Ownership (Revenue)");
            objbulk.ColumnMappings.Add("Company Alias", "Company Alias");
            objbulk.ColumnMappings.Add("Opportunity ID", "Opportunity ID");
            objbulk.ColumnMappings.Add("Description", "Description");
            objbulk.ColumnMappings.Add("Industry", "Industry");
            objbulk.ColumnMappings.Add("BT Current Service Configuration", "BT Current Service Configuration");
            objbulk.ColumnMappings.Add("BT Current GDS", "BT Current GDS");
            objbulk.ColumnMappings.Add("BT Current Online Booking Tool", "BT Current Online Booking Tool");
            objbulk.ColumnMappings.Add("BT Incumbent", "BT Incumbent");
            objbulk.ColumnMappings.Add("Opportunity Name", "Opportunity Name");
            objbulk.ColumnMappings.Add("Product Name", "Product Name");
            //objbulk.ColumnMappings.Add("Line of Business", "Line of Business");
            objbulk.ColumnMappings.Add("Opportunity Owner", "Opportunity Owner");
            objbulk.ColumnMappings.Add("Close Date", "Close Date");
            objbulk.ColumnMappings.Add("Sales Stage Name", "Sales Stage Name");
            objbulk.ColumnMappings.Add("Line Win Probability", "Line Win Probability");
            objbulk.ColumnMappings.Add("Initiative Name", "Initiative Name");
            objbulk.ColumnMappings.Add("Opportunity Type", "Opportunity Type");
            objbulk.ColumnMappings.Add("Opportunity Region", "Opportunity Region");
            objbulk.ColumnMappings.Add("Country Scope", "Country Scope");
            objbulk.ColumnMappings.Add("Energy, Resources & Marine", "Energy, Resources & Marine");
            objbulk.ColumnMappings.Add("Invoicing Period", "Invoicing Period");
            objbulk.ColumnMappings.Add("Pricing Model", "Pricing Model");
            objbulk.ColumnMappings.Add("PSD Price per Transaction", "PSD Price per Transaction");
            objbulk.ColumnMappings.Add("Total Transactions", "Total Transactions");
            objbulk.ColumnMappings.Add("Country", "Country");
            objbulk.ColumnMappings.Add("Region (Revenue)", "Region (Revenue)");
            objbulk.ColumnMappings.Add("Implementation Status", "Implementation Status");
            objbulk.ColumnMappings.Add("Total Revenue USD", "Total Revenue USD");
            objbulk.ColumnMappings.Add("Estimated Implementation Start Date", "Estimated Implementation Start Date");
            objbulk.ColumnMappings.Add("Implementation End Date", "Implementation End Date");
            objbulk.ColumnMappings.Add("Implementation Fee", "Implementation Fee");
            objbulk.ColumnMappings.Add("Go Live Date", "Go Live Date");
            objbulk.ColumnMappings.Add("Revenue Car Volume USD", "Revenue Car Volume USD");
            objbulk.ColumnMappings.Add("Revenue Status", "Revenue Status");
            objbulk.ColumnMappings.Add("Revenue Id", "Revenue Id");
            //objbulk.ColumnMappings.Add("NORAM Real Estate and Facilities", "NORAM Real Estate and Facilities");
            //objbulk.ColumnMappings.Add("Regional Ops Manager", "Regional Ops Manager");
            con.Open();
            string s = "Truncate Table PSD";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(dt);
            con.Close();
        }
        public void InsertNpsImplementationData(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "NPSImplementation$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "NPSImplementation";
            objbulk.ColumnMappings.Add("Client Name", "Client Name");
            objbulk.ColumnMappings.Add("Record ID", "Record ID");
            objbulk.ColumnMappings.Add("Company", "Company");
            objbulk.ColumnMappings.Add("Date Survey Sent", "Date Survey Sent");
            objbulk.ColumnMappings.Add("Email address (Client)", "Email address (Client)");
            objbulk.ColumnMappings.Add("NPS Indicator", "NPS Indicator");
            objbulk.ColumnMappings.Add("NPS Score", "NPS Score");
            objbulk.ColumnMappings.Add("Date survey received", "Date survey received");
            objbulk.ColumnMappings.Add("NPS Comments - What was Positive", "NPS Comments - What was Positive");
            objbulk.ColumnMappings.Add("NPS Comments - How could we have improved", "NPS Comments - How could we have improved");
            objbulk.ColumnMappings.Add("NPS Comments - What is the one thing we can do to make you happier", "NPS Comments - What is the one thing we can do to make you happier");
            objbulk.ColumnMappings.Add("Local Project Manager", "Local Project Manager");
            objbulk.ColumnMappings.Add("Online Team", "Online Team");
            objbulk.ColumnMappings.Add("DQS", "DQS");
            objbulk.ColumnMappings.Add("Status", "Status");
            objbulk.ColumnMappings.Add("Assign Leader for Closed-Loop", "Assign Leader for Closed-Loop");
            objbulk.ColumnMappings.Add("Reason type", "Reason type");
            objbulk.ColumnMappings.Add("Client Feedback", "Client Feedback");
            objbulk.ColumnMappings.Add("Action", "Action");
            objbulk.ColumnMappings.Add("DSD", "DSD");
            objbulk.ColumnMappings.Add("Global Project Manager", "Global Project Manager");
            objbulk.ColumnMappings.Add("Regional Project Manager", "Regional Project Manager");
            objbulk.ColumnMappings.Add("Country/Area of Responsibility", "Country/Area of Responsibility");
            objbulk.ColumnMappings.Add("Region", "Region");
            objbulk.ColumnMappings.Add("Client Scope", "Client Scope");
            objbulk.ColumnMappings.Add("Customer contact number", "Customer contact number");
            objbulk.ColumnMappings.Add("Client Type - New/Existing", "Client Type - New/Existing");
            objbulk.ColumnMappings.Add("Single Resource", "Single Resource");
            con.Open();
            string s = "Truncate Table NPSImplementation";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(dt);
            con.Close();
        }
        public void InsertCTOdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "CTO$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "CTO";
            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("Milestone: Project Status", "Milestone: Project Status");
            objbulk.ColumnMappings.Add("Group Name", "Group Name");
            objbulk.ColumnMappings.Add("Milestone: Region", "Milestone: Region");
            objbulk.ColumnMappings.Add("Milestone: Country", "Milestone: Country");
            objbulk.ColumnMappings.Add("Milestone Title", "Milestone Title");
            objbulk.ColumnMappings.Add("Milestone: Assignee: Full Name", "Milestone: Assignee: Full Name");
            objbulk.ColumnMappings.Add("Milestone Due Date", "Milestone Due Date");
            objbulk.ColumnMappings.Add("Milestone: Country Status", "Milestone: Country Status");
            objbulk.ColumnMappings.Add("Task List Title", "Task List Title");
            objbulk.ColumnMappings.Add("Task Title", "Task Title");
            objbulk.ColumnMappings.Add("Task: Assignee: Full Name", "Task: Assignee: Full Name");
            objbulk.ColumnMappings.Add("Task Status", "Task Status");
            objbulk.ColumnMappings.Add("Task Overdue", "Task Overdue");
            objbulk.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk.ColumnMappings.Add("Task Due Date", "Task Due Date");
            objbulk.ColumnMappings.Add("Workspace: Project Level", "Workspace: Project Level"); 
            objbulk.ColumnMappings.Add("RevenurID", "RevenurID");
            objbulk.ColumnMappings.Add("TaskType", "TaskType");
            objbulk.ColumnMappings.Add("TaskRecordIdKey", "TaskRecordIdKey");
            con.Open();
            string s = "Truncate Table CTO";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.WriteToServer(dt);
            con.Close();

            var CTOData = (from a in entity.CTOes
                           select new
                           {
                               a.Workspace_Title,
                               a.Milestone__Project_Status,
                               a.Group_Name,
                               a.Milestone__Region,
                               a.Milestone__Country,
                               a.Milestone_Title,
                               a.Milestone__Assignee__Full_Name,
                               a.Milestone_Due_Date,
                               a.Milestone__Country_Status,
                               a.Task_List_Title,
                               a.Task_Title,
                               a.Task__Assignee__Full_Name,
                               a.Task_Status,
                               a.Task_Overdue,
                               a.Task_Start_Date,
                               a.Task_Due_Date,
                               a.Workspace__Project_Level,
                               //a.Last_Comment,
                               a.RevenurID,
                               a.TaskType,
                               a.TaskRecordIdKey
                           }).ToList();
            DataTable tbl2 = new DataTable();
            tbl2.Columns.Add(new DataColumn("Milestone Title-Country:  Est Go Live Date", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Critical Overdue", typeof(float)));
            tbl2.Columns.Add(new DataColumn("Estimated Go Live", typeof(DateTime)));
            tbl2.Columns.Add(new DataColumn("Workspace Title", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone: Project Status", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Group Name", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone: Region", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone: Country", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone Title", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone: Assignee: Full Name", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone Due Date", typeof(DateTime)));
            tbl2.Columns.Add(new DataColumn("Milestone: Country Status", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Task List Title", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Task Title", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Task: Assignee: Full Name", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Task Status", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Task Overdue", typeof(float)));
            tbl2.Columns.Add(new DataColumn("Task Start Date", typeof(DateTime)));
            tbl2.Columns.Add(new DataColumn("Task Due Date", typeof(DateTime)));
            tbl2.Columns.Add(new DataColumn("Workspace: Project Level", typeof(string)));
            tbl2.Columns.Add(new DataColumn("RevenurID", typeof(string)));
            tbl2.Columns.Add(new DataColumn("TaskType", typeof(string)));
            tbl2.Columns.Add(new DataColumn("TaskRecordIdKey", typeof(string)));
            //tbl2.Columns.Add(new DataColumn("Last Comment", typeof(string)));

            foreach (var r in CTOData)
            {
                DataRow dr2 = tbl2.NewRow();
                var M_due_date = Convert.ToDateTime(r.Milestone_Due_Date);
                dr2["Milestone Title-Country:  Est Go Live Date"] = (object)(r.Milestone_Title +"-"+r.Milestone__Country+"      "+ (r.Milestone_Due_Date == null ? null : (r.Milestone__Region == "APAC" || r.Milestone__Region == "EMEA" ? M_due_date.AddDays(-28).ToShortDateString() : M_due_date.AddDays(-14).ToShortDateString()))) ?? DBNull.Value;
                dr2["Critical Overdue"] = (object)(r.Task_Overdue > 6) ?? DBNull.Value;
                //dr2["Estimated Go Live"] = (object) (r.Milestone_Due_Date == (DateTime?)null ? (DateTime?)null : Convert.ToDateTime(r.Milestone_Due_Date).AddDays(-28)) ?? DBNull.Value;
                dr2["Estimated Go Live"] = (object)(r.Milestone_Due_Date == (DateTime?)null ? (DateTime?)null : (r.Milestone__Region == "APAC" || r.Milestone__Region == "EMEA" ?  Convert.ToDateTime(r.Milestone_Due_Date).AddDays(-28).Date : Convert.ToDateTime(r.Milestone_Due_Date).AddDays(-14).Date)) ?? DBNull.Value;
                dr2["Workspace Title"] = (object)r.Workspace_Title ?? DBNull.Value;
                dr2["Milestone: Project Status"] = (object)r.Milestone__Project_Status ?? DBNull.Value;
                dr2["Group Name"] = (object)r.Group_Name ?? DBNull.Value;
                dr2["Milestone: Region"] = (object)r.Milestone__Region ?? DBNull.Value;
                dr2["Milestone: Country"] = (object)r.Milestone__Country ?? DBNull.Value;
                dr2["Milestone Title"] = (object)r.Milestone_Title ?? DBNull.Value;
                dr2["Milestone: Assignee: Full Name"] = (object)r.Milestone__Assignee__Full_Name ?? DBNull.Value;
                dr2["Milestone Due Date"] = (object)r.Milestone_Due_Date ?? DBNull.Value;
                dr2["Milestone: Country Status"] = (object)r.Milestone__Country_Status ?? DBNull.Value;
                dr2["Task List Title"] = (object)r.Task_List_Title ?? DBNull.Value;
                dr2["Task Title"] = (object)r.Task_Title ?? DBNull.Value;
                dr2["Task: Assignee: Full Name"] = (object)r.Task__Assignee__Full_Name ?? DBNull.Value;
                dr2["Task Status"] = (object)r.Task_Status ?? DBNull.Value;
                dr2["Task Overdue"] = (object)r.Task_Overdue ?? DBNull.Value;
                dr2["Task Start Date"] = (object)r.Task_Start_Date ?? DBNull.Value;
                dr2["Task Due Date"] = (object)r.Task_Due_Date ?? DBNull.Value;
                dr2["Workspace: Project Level"] = (object)r.Workspace__Project_Level ?? DBNull.Value;
                dr2["RevenurID"] = (object)r.RevenurID ?? DBNull.Value;
                dr2["TaskType"] = (object)r.TaskType ?? DBNull.Value;
                dr2["TaskRecordIdKey"] = (object)r.TaskRecordIdKey ?? DBNull.Value;
                tbl2.Rows.Add(dr2);
            }
            SqlBulkCopy objbulk2 = new SqlBulkCopy(con);
            objbulk2.DestinationTableName = "CTO";
            objbulk2.ColumnMappings.Add("Milestone Title-Country:  Est Go Live Date", "Milestone Title-Country:  Est Go Live Date");
            objbulk2.ColumnMappings.Add("Critical Overdue", "Critical Overdue");
            objbulk2.ColumnMappings.Add("Estimated Go Live", "Estimated Go Live");
            objbulk2.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk2.ColumnMappings.Add("Milestone: Project Status", "Milestone: Project Status");
            objbulk2.ColumnMappings.Add("Group Name", "Group Name");
            objbulk2.ColumnMappings.Add("Milestone: Region", "Milestone: Region");
            objbulk2.ColumnMappings.Add("Milestone: Country", "Milestone: Country");
            objbulk2.ColumnMappings.Add("Milestone Title", "Milestone Title");
            objbulk2.ColumnMappings.Add("Milestone: Assignee: Full Name", "Milestone: Assignee: Full Name");
            objbulk2.ColumnMappings.Add("Milestone Due Date", "Milestone Due Date");
            objbulk2.ColumnMappings.Add("Milestone: Country Status", "Milestone: Country Status");
            objbulk2.ColumnMappings.Add("Task List Title", "Task List Title");
            objbulk2.ColumnMappings.Add("Task Title", "Task Title");
            objbulk2.ColumnMappings.Add("Task: Assignee: Full Name", "Task: Assignee: Full Name");
            objbulk2.ColumnMappings.Add("Task Status", "Task Status");
            objbulk2.ColumnMappings.Add("Task Overdue", "Task Overdue");
            objbulk2.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk2.ColumnMappings.Add("Task Due Date", "Task Due Date");
            objbulk2.ColumnMappings.Add("Workspace: Project Level", "Workspace: Project Level");
            objbulk2.ColumnMappings.Add("RevenurID", "RevenurID");
            objbulk2.ColumnMappings.Add("TaskType", "TaskType");
            objbulk2.ColumnMappings.Add("TaskRecordIdKey", "TaskRecordIdKey");
            //objbulk2.ColumnMappings.Add("Last Comment", "Last Comment");
            con.Open();
            string s2 = "Truncate Table CTO";
            SqlCommand Com2 = new SqlCommand(s2, con);
            Com2.ExecuteNonQuery();
            objbulk2.BatchSize = 100000;
            objbulk2.BulkCopyTimeout = 0;
            objbulk2.WriteToServer(tbl2);
            con.Close();
            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "CTOData",
                    UpdatedOn = TodayDate,
                });
            entity.SaveChanges();
        }
        public void InsertCommentsData(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "Comments$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "Comments";
            objbulk.ColumnMappings.Add("ID", "ID");
            objbulk.ColumnMappings.Add("Row", "Row");
            objbulk.ColumnMappings.Add("Name", "Name");
            objbulk.ColumnMappings.Add("Date", "Date");
            objbulk.ColumnMappings.Add("Comments", "Comments");
            con.Open();
            string s = "Truncate Table Comments";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.WriteToServer(dt);
            con.Close();
            DateTime TodayDate = DateTime.Now;
        }
        public void InsertIMPSdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "IMPS$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "IMPS";
            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("Workspace: CRM Customer Row ID", "Workspace: CRM Customer Row ID");
            objbulk.ColumnMappings.Add("Workspace: Project Level", "Workspace: Project Level");
            objbulk.ColumnMappings.Add("Workspace: ELT Overall Status", "Workspace: ELT Overall Status");
            objbulk.ColumnMappings.Add("Workspace: ELT Overall Comments", "Workspace: ELT Overall Comments");
            objbulk.ColumnMappings.Add("Milestone Title", "Milestone Title");
            objbulk.ColumnMappings.Add("Milestone: Region", "Milestone: Region");
            objbulk.ColumnMappings.Add("Milestone: Record ID Key", "Milestone: Record ID Key");
            objbulk.ColumnMappings.Add("Milestone: Country", "Milestone: Country");
            objbulk.ColumnMappings.Add("Milestone: Project Status", "Milestone: Project Status");
            objbulk.ColumnMappings.Add("% Complete", "% Complete");
            objbulk.ColumnMappings.Add("Milestone: Assignee: Full Name", "Milestone: Assignee: Full Name");
            objbulk.ColumnMappings.Add("Milestone: Assignee: Reports to: Full Name", "Milestone: Assignee: Reports to: Full Name");
            objbulk.ColumnMappings.Add("Task Title", "Task Title");
            objbulk.ColumnMappings.Add("Task: Task Record ID Key", "Task: Task Record ID Key");
            objbulk.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk.ColumnMappings.Add("Milestone: CRM Revenue ID #", "Milestone: CRM Revenue ID #");
            objbulk.ColumnMappings.Add("Milestone: Project Notes", "Milestone: Project Notes");
            objbulk.ColumnMappings.Add("Milestone: Reason Code", "Milestone: Reason Code");
            objbulk.ColumnMappings.Add("Milestone: Closed Loop Owner", "Milestone: Closed Loop Owner");
            objbulk.ColumnMappings.Add("Group Name", "Group Name");
            con.Open();
            string s = "Truncate Table IMPS";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.WriteToServer(dt);
            con.Close();
            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "IMPSData",
                    UpdatedOn = TodayDate,
                });
            entity.SaveChanges();
        }
        public void InsertLLdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "LL$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "LessonLearnt";
            objbulk.ColumnMappings.Add("iMeet Workspace Title", "iMeet Workspace Title");
            objbulk.ColumnMappings.Add("Record ID", "Record ID");
            objbulk.ColumnMappings.Add("Date feedback raised", "Date feedback raised");
            objbulk.ColumnMappings.Add("Country/Area of Responsibility", "Country/Area of Responsibility");
            objbulk.ColumnMappings.Add("Region", "Region");
            objbulk.ColumnMappings.Add("What was the event/issue/concern", "What was the event/issue/concern");
            objbulk.ColumnMappings.Add("Is there any specific recognition to a person/group/process rela", "Is there any specific recognition to a person/group/process rela");
            objbulk.ColumnMappings.Add("Go Live Date", "Go Live Date");
            objbulk.ColumnMappings.Add("Reason Type", "Reason Type");
            objbulk.ColumnMappings.Add("Created by Field", "Created by Field");
            objbulk.ColumnMappings.Add("Leader", "Leader");
            objbulk.ColumnMappings.Add("Reason Code (Added by Leader)", "Reason Code (Added by Leader)");
            objbulk.ColumnMappings.Add("What do you recommend - to avoid this occurring again in future", "What do you recommend - to avoid this occurring again in future");
            objbulk.ColumnMappings.Add("Status (By Leader)", "Status (By Leader)");
            objbulk.ColumnMappings.Add("Action Taken (By Leader)", "Action Taken (By Leader)");
            con.Open();
            string s = "Truncate Table LessonLearnt";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.WriteToServer(dt);
            con.Close();
            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "LLData",
                    UpdatedOn = TodayDate,
                });
            entity.SaveChanges();
        }
        public void InsertSGdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "TestingTask$");
            string query2 = string.Format("Select * from [{0}]", "OnlineOffline$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            OleDbCommand Ecom2 = new OleDbCommand(query2, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            DataSet ds2 = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            OleDbDataAdapter oda2 = new OleDbDataAdapter(query2, Econ);
            Econ.Close();
            oda.Fill(ds);
            oda2.Fill(ds2);
            DataTable dt = ds.Tables[0];
            DataTable dt2 = ds2.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "StageGate";
            objbulk.ColumnMappings.Add("Task Title", "Task Title");
            objbulk.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk.ColumnMappings.Add("Task Due Date", "Task Due Date");
            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("Task: Assignee: Full Name", "Task: Assignee: Full Name");
            objbulk.ColumnMappings.Add("Task Status", "Task Status");
            objbulk.ColumnMappings.Add("Milestone: Assignee: Country", "Milestone: Assignee: Country");
            objbulk.ColumnMappings.Add("Milestone: Assignee: Full Name", "Milestone: Assignee: Full Name");
            objbulk.ColumnMappings.Add("Milestone: Assignee: Reports to: Full Name", "Milestone: Assignee: Reports to: Full Name");
            objbulk.ColumnMappings.Add("Milestone: Project Status", "Milestone: Project Status");
            con.Open();
            string s = "Truncate Table StageGate";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.WriteToServer(dt);
            con.Close();

            (from p in entity.StageGates
             where p.ReportName == "" || p.ReportName == null
             select p).ToList().ForEach(x => x.ReportName = "TestingTask");
            entity.SaveChanges();

            SqlBulkCopy objbulk2 = new SqlBulkCopy(con);
            objbulk2.DestinationTableName = "StageGate";
            objbulk2.ColumnMappings.Add("Task Title", "Task Title");
            objbulk2.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk2.ColumnMappings.Add("Task Due Date", "Task Due Date");
            objbulk2.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk2.ColumnMappings.Add("Task: Assignee: Full Name", "Task: Assignee: Full Name");
            objbulk2.ColumnMappings.Add("Task Status", "Task Status");
            objbulk2.ColumnMappings.Add("Milestone: Assignee: Country", "Milestone: Assignee: Country");
            objbulk2.ColumnMappings.Add("Milestone: Assignee: Full Name", "Milestone: Assignee: Full Name");
            objbulk2.ColumnMappings.Add("Milestone: Assignee: Reports to: Full Name", "Milestone: Assignee: Reports to: Full Name");
            objbulk2.ColumnMappings.Add("Milestone: Project Status", "Milestone: Project Status");
            con.Open();
            objbulk2.WriteToServer(dt2);
            con.Close();
            (from p in entity.StageGates
             where p.ReportName == "" || p.ReportName == null
             select p).ToList().ForEach(x => x.ReportName = "OnlineOffline");
            entity.SaveChanges();
            var SGData = (from a in entity.StageGates
                          select new
                          {
                              a.Task_Title,
                              a.Task_Start_Date,
                              a.Task_Due_Date,
                              a.Workspace_Title,
                              a.Task__Assignee__Full_Name,
                              a.Task_Status,
                              a.Milestone__Assignee__Country,
                              a.Milestone__Assignee__Full_Name,
                              a.Milestone__Assignee__Reports_to__Full_Name,
                              a.ReportName,
                              a.Milestone__Project_Status,
                          }).ToList();
            DataTable tbl2 = new DataTable();
            tbl2.Columns.Add(new DataColumn("Task Title", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Task Start Date", typeof(DateTime)));
            tbl2.Columns.Add(new DataColumn("Task Due Date", typeof(DateTime)));
            tbl2.Columns.Add(new DataColumn("Workspace Title", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Task: Assignee: Full Name", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Task Status", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone: Assignee: Country", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone: Assignee: Full Name", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone: Assignee: Reports to: Full Name", typeof(string)));
            tbl2.Columns.Add(new DataColumn("ReportName", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Milestone: Project Status", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Month", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Year", typeof(string)));

            foreach (var r in SGData)
            {
                DataRow dr2 = tbl2.NewRow();
                dr2["Task Title"] = (object)r.Task_Title ?? DBNull.Value;
                dr2["Task Start Date"] = (object)r.Task_Start_Date ?? DBNull.Value;
                dr2["Task Due Date"] = (object)r.Task_Due_Date  ?? DBNull.Value;
                dr2["Workspace Title"] = (object)r.Workspace_Title ?? DBNull.Value;
                dr2["Task: Assignee: Full Name"] = (object)r.Milestone__Assignee__Full_Name ?? DBNull.Value;
                dr2["Task Status"] = (object)r.Task_Status ?? DBNull.Value;
                dr2["Milestone: Assignee: Country"] = (object)r.Milestone__Assignee__Country ?? DBNull.Value;
                dr2["Milestone: Assignee: Full Name"] = (object)r.Milestone__Assignee__Full_Name ?? DBNull.Value;
                dr2["Milestone: Assignee: Reports to: Full Name"] = (object)r.Milestone__Assignee__Reports_to__Full_Name ?? DBNull.Value;
                dr2["ReportName"] = (object)r.ReportName ?? DBNull.Value;
                dr2["Milestone: Project Status"] = (object)r.Milestone__Project_Status ?? DBNull.Value;
                dr2["Month"] = r.Task_Due_Date == null ? null : Convert.ToDateTime(r.Task_Due_Date).ToString("MMM");
                dr2["Year"] = r.Task_Due_Date == null ? null : Convert.ToDateTime(r.Task_Due_Date).Year.ToString();
                tbl2.Rows.Add(dr2);
            }
            SqlBulkCopy objbulk3 = new SqlBulkCopy(con);
            objbulk3.DestinationTableName = "StageGate";
            objbulk3.ColumnMappings.Add("Task Title", "Task Title");
            objbulk3.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk3.ColumnMappings.Add("Task Due Date", "Task Due Date");
            objbulk3.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk3.ColumnMappings.Add("Task: Assignee: Full Name", "Task: Assignee: Full Name");
            objbulk3.ColumnMappings.Add("Task Status", "Task Status");
            objbulk3.ColumnMappings.Add("Milestone: Assignee: Country", "Milestone: Assignee: Country");
            objbulk3.ColumnMappings.Add("Milestone: Assignee: Full Name", "Milestone: Assignee: Full Name");
            objbulk3.ColumnMappings.Add("Milestone: Assignee: Reports to: Full Name", "Milestone: Assignee: Reports to: Full Name");
            objbulk3.ColumnMappings.Add("ReportName", "ReportName");
            objbulk3.ColumnMappings.Add("Milestone: Project Status", "Milestone: Project Status");
            objbulk3.ColumnMappings.Add("Month", "Month");
            objbulk3.ColumnMappings.Add("Year", "Year");
            con.Open();
            string s2 = "Truncate Table StageGate";
            SqlCommand Com2 = new SqlCommand(s2, con);
            Com2.ExecuteNonQuery();
            objbulk3.BatchSize = 100000;
            objbulk3.BulkCopyTimeout = 0;
            objbulk3.WriteToServer(tbl2);
            con.Close();
            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "SGData",
                    UpdatedOn = TodayDate,
                });
            entity.SaveChanges();
        }
        public void InsertAPdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "AssignePersons$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "AssignePersons";
            objbulk.ColumnMappings.Add("Task Title", "Task Title");
            objbulk.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk.ColumnMappings.Add("Task Due Date", "Task Due Date");
            objbulk.ColumnMappings.Add("Milestone Title", "Milestone Title");
            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("Task: Assigned Role: Name", "Task: Assigned Role: Name");
            objbulk.ColumnMappings.Add("Task: Assignee: Reports to: Full Name", "Task: Assignee: Reports to: Full Name");
            objbulk.ColumnMappings.Add("Task: Task Record ID Key", "Task: Task Record ID Key");
            objbulk.ColumnMappings.Add("Milestone: CRM Revenue ID #", "Milestone: CRM Revenue ID #");
            objbulk.ColumnMappings.Add("Milestone: Assignee: Reports to: Full Name", "Milestone: Assignee: Reports to: Full Name");
            objbulk.ColumnMappings.Add("Milestone: Override Regional Digital OBT Lead: Full Name", "Milestone: Override Regional Digital OBT Lead: Full Name");
            objbulk.ColumnMappings.Add("Milestone: Override Regional Digital Portrait Lead: Full Name", "Milestone: Override Regional Digital Portrait Lead: Full Name");
            objbulk.ColumnMappings.Add("Milestone: Country", "Milestone: Country");
            objbulk.ColumnMappings.Add("Task: Record ID", "Task: Record ID");
            objbulk.ColumnMappings.Add("Milestone: Record ID Key", "Milestone: Record ID Key");
            objbulk.ColumnMappings.Add("Milestone: Assignee: Full Name", "Milestone: Assignee: Full Name");
            objbulk.ColumnMappings.Add("Milestone: Local Digital OBT Lead", "Milestone: Local Digital OBT Lead");
            objbulk.ColumnMappings.Add("Milestone: Local Digital Ad Hoc Support", "Milestone: Local Digital Ad Hoc Support");
            con.Open();
            string s = "Truncate Table AssignePersons";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.WriteToServer(dt);
            con.Close();
        }
        public void InsertHEKOdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "HostExternal$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "HostExternal";
            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("Milestone Title", "Milestone Title");
            objbulk.ColumnMappings.Add("Milestone: Country", "Milestone: Country");
            objbulk.ColumnMappings.Add("Task Title", "Task Title");
            objbulk.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk.ColumnMappings.Add("Task Due Date", "Task Due Date");
            objbulk.ColumnMappings.Add("Task Completed Date", "Task Completed Date");
            objbulk.ColumnMappings.Add("Task Status", "Task Status");
            objbulk.ColumnMappings.Add("Task: Action Region", "Task: Action Region");
            objbulk.ColumnMappings.Add("Task: Record ID", "Task: Record ID");
            con.Open();
            string s = "Truncate Table HostExternal";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.WriteToServer(dt);
            con.Close();
        }

        public void InsertPDDdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "ProjectDueDate$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];

            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "ProjectDueDate";
            objbulk.ColumnMappings.Add("Milestone: Record ID Key", "Milestone: Record ID Key");
            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("Milestone Title", "Milestone Title");
            objbulk.ColumnMappings.Add("Task Title", "Task Title");
            objbulk.ColumnMappings.Add("Task: Task Type", "Task: Task Type");
            objbulk.ColumnMappings.Add("Milestone: Country", "Milestone: Country");
            objbulk.ColumnMappings.Add("Task: Assignee: Full Name", "Task: Assignee: Full Name");
            objbulk.ColumnMappings.Add("Task Start Date", "Task Start Date");
            objbulk.ColumnMappings.Add("Task Due Date", "Task Due Date");
            objbulk.ColumnMappings.Add("Task Completed Date", "Task Completed Date");
            objbulk.ColumnMappings.Add("Task Status", "Task Status");
            objbulk.ColumnMappings.Add("Milestone: CRM Revenue ID #", "Milestone: CRM Revenue ID #");
            con.Open();
            string s = "Truncate Table ProjectDueDate";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.WriteToServer(dt);
            con.Close();
        }
        
        public void InserteSowdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "eSow$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "eSowData";
            objbulk.ColumnMappings.Add("SOWName", "SOWName");
            objbulk.ColumnMappings.Add("CRMOpportunityID", "CRMOpportunityID");
            objbulk.ColumnMappings.Add("ServiceModel", "ServiceModel");
            objbulk.ColumnMappings.Add("LanguageRequirement", "LanguageRequirement");
            objbulk.ColumnMappings.Add("DSDLead", "DSDLead");
            objbulk.ColumnMappings.Add("SOWOwner", "SOWOwner");
            objbulk.ColumnMappings.Add("ImplementationLead", "ImplementationLead");
            objbulk.ColumnMappings.Add("PricingLead", "PricingLead");
            objbulk.ColumnMappings.Add("PricingLeadregion", "PricingLeadregion");
            objbulk.ColumnMappings.Add("GeographicScope", "GeographicScope");
            objbulk.ColumnMappings.Add("ProspectType", "ProspectType");
            objbulk.ColumnMappings.Add("CRMStatus", "CRMStatus");
            objbulk.ColumnMappings.Add("ClientRegion", "ClientRegion");
            objbulk.ColumnMappings.Add("DateRFPReceived", "DateRFPReceived");
            objbulk.ColumnMappings.Add("SOWCreationDate", "SOWCreationDate");
            objbulk.ColumnMappings.Add("DateDecisionExpected", "DateDecisionExpected");
            objbulk.ColumnMappings.Add("Datenotifiedofwinorloss", "Datenotifiedofwinorloss");
            objbulk.ColumnMappings.Add("GoLivedateestimated", "GoLivedateestimated");
            objbulk.ColumnMappings.Add("NumberofCountries", "NumberofCountries");
            objbulk.ColumnMappings.Add("LastModifiedDate", "LastModifiedDate");
            objbulk.ColumnMappings.Add("WebPEMStatus", "WebPEMStatus");
            objbulk.ColumnMappings.Add("SOWStatus", "SOWStatus");
            objbulk.ColumnMappings.Add("SLAInScope", "SLAInScope");
            objbulk.ColumnMappings.Add("PricingReady", "PricingReady");
            objbulk.ColumnMappings.Add("ImplementationReady", "ImplementationReady");
            objbulk.ColumnMappings.Add("AccountCategory", "AccountCategory");
            objbulk.ColumnMappings.Add("PenaltyRewardSLA", "PenaltyRewardSLA");
            objbulk.ColumnMappings.Add("SLAReporting", "SLAReporting");
            //objbulk.ColumnMappings.Add("OBT Adoption Rate", "OBT Adoption Rate");
            con.Open();
            string s = "Truncate Table eSowData";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(dt);
            con.Close();
        }
        public void InserteSowNewdata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "eSow$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "esowNewData";
            objbulk.ColumnMappings.Add("Client", "Client");
            objbulk.ColumnMappings.Add("id", "id");
            objbulk.ColumnMappings.Add("eSoW Ref", "eSoW Ref");
            objbulk.ColumnMappings.Add("crmOpportunityId", "crmOpportunityId");
            objbulk.ColumnMappings.Add("createdOn", "createdOn");
            objbulk.ColumnMappings.Add("Client Type", "Client Type");
            objbulk.ColumnMappings.Add("Proposal Type", "Proposal Type");
            objbulk.ColumnMappings.Add("Account Owner", "Account Owner");
            objbulk.ColumnMappings.Add("SoW Owner", "SoW Owner");
            objbulk.ColumnMappings.Add("Self Service", "Self Service");
            objbulk.ColumnMappings.Add("eSoW Status", "eSoW Status");
            objbulk.ColumnMappings.Add("CRM Status", "CRM Status");
            objbulk.ColumnMappings.Add("Option", "Option");
            objbulk.ColumnMappings.Add("Country Name", "Country Name");
            objbulk.ColumnMappings.Add("serviceConfig", "serviceConfig");
            objbulk.ColumnMappings.Add("Team", "Team");
            objbulk.ColumnMappings.Add("configLoc", "configLoc");
            objbulk.ColumnMappings.Add("configName", "configName");
            objbulk.ColumnMappings.Add("afterHours", "afterHours");
            objbulk.ColumnMappings.Add("GDS", "GDS");
            objbulk.ColumnMappings.Add("OBT", "OBT");
            objbulk.ColumnMappings.Add("Account Category", "Account Category");
            objbulk.ColumnMappings.Add("implementationReady", "implementationReady");
            objbulk.ColumnMappings.Add("Direct/Reseller", "Direct/Reseller");
            objbulk.ColumnMappings.Add("OBT Adoption Rate", "OBT Adoption Rate");
            con.Open();
            string s = "Truncate Table esowNewData";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(dt);
            con.Close();
        }
        string[] Opportunity_Type,Opportunity_Type3, Opportunity_Type2, Opportunity_Type4, Revenue_Opportunity_Type, Final_Revenue_Opportunity_Type;
        string[] SalesStageName, SalesStageNameHP, Rev_opportunity_type, Pipeline_Rev_opportunity_type, Opportunity_Scope, crm_countries;
        string Revenue_Status;
        DateTime ClosingDate, ClosedDateCRM;
        public ActionResult GenerateCLR()
        {
            this.StoreYesterdayCLRData();
            var CreationDate = "01-01-2020";
            DateTime ConvertedDate = Convert.ToDateTime(CreationDate);
            DateTime TodayDate = DateTime.Today.Date;
            var Status = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed,H-Hold,X-Cancelled".Split(',');
            //Opportunity_Type2 = "Re-Bid With Up-Sell,Lost Client (w/o notice)".Split(',');
            Opportunity_Type = "New Business,Up-Sell(Add Offices/Countries)".Split(',');
            Opportunity_Type2 = "Re-Bid,Renewal/Renegotiation,".Split(',');
            Opportunity_Type3 = "Re-Bid With Up-Sell,Lost Client (w/o notice)".Split(',');
            Opportunity_Type4 = "Renewal/Renegotiation,".Split(',');
            Revenue_Opportunity_Type = "Up-Sell(Add Offices/Countries),Re-Bid With Up-Sell,New Business".Split(',');
            Final_Revenue_Opportunity_Type = "Up-Sell(Add Offices/Countries),New Business,Re-Bid With Up-Sell".Split(',');
            entity.Database.CommandTimeout = 999;
            var ProductUpSellData = (from b in entity.PSDs
                                     where b.Region__Revenue_ != null
                                     join a in entity.DataiMeets on b.Revenue_Id equals a.Milestone__CRM_Revenue_ID__
                                     where a.Task_Start_Date != null
                                     where Status.Any(val => a.Milestone__Project_Status.Equals(val))
                                     select new
                                     {
                                         RevenueID = a.Milestone__CRM_Revenue_ID__,
                                         b.Revenue_Id,
                                         Client = b.Account_Name,
                                         Region = b.Region__Revenue_ ?? "",
                                         Country = b.Country ?? "",
                                         OwnerShip = b.Ownership__Revenue_ ?? "",
                                         GoLiveDate = a.Task_Start_Date,
                                         TaskStartDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone__Country == a.Milestone__Country).Task_Start_Date,
                                         //TaskDueDate = a.Task_Due_Date,
                                         //Year = a.Task_Start_Date.Value.Month.ToString("yyyy"),
                                         ProjectStatus = a.Milestone__Project_Status ?? "",
                                         ProjectStartCTCompleteDate = entity.ProjectDueDates.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == a.Milestone__CRM_Revenue_ID__).Task_Completed_Date,
                                         ProjectStartCTDueDate = entity.ProjectDueDates.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == a.Milestone__CRM_Revenue_ID__).Task_Due_Date,
                                         ProjectStartTaskStatus = entity.ProjectDueDates.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == a.Milestone__CRM_Revenue_ID__).Task_Status,
                                         CountryStatus = a.Milestone__Country_Status ?? "",
                                         TaskStatus = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone__Country == a.Milestone__Country).Task_Status,
                                         ProjectLevel = a.Workspace__Project_Level ?? "",
                                         //ProjectStartDate = a.Milestone__Project_Start_Date,
                                         //IntitialGoliveDate = a.Milestone__Initial_Go_Live_Date,
                                         CompletedDate = a.Completed_Date,
                                         TaskCompletedDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone__Country == a.Milestone__Country).Task_Completed_Date,
                                         //ProjectOwner = a.Workspace__Project_Owner__Full_Name ?? "",
                                         AssigneeFullName = entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == a.Milestone__CRM_Revenue_ID__).Milestone__Assignee__Full_Name,
                                         C__Complete = a.C__Complete ?? 0,
                                         Milestone_Due_Date = a.Milestone_Due_Date,
                                         Line_Win_Probability = b.Line_Win_Probability ?? 0,
                                         Next_Step = "",
                                         MilestoneTitle = a.Milestone_Title ?? "",
                                         Milestone__Record_ID_Key = a.Milestone__Record_ID_Key ?? "",
                                         Task__Task_Record_ID_Key = a.Task__Task_Record_ID_Key ?? "",
                                         Group_Name = a.Group_Name ?? "",
                                         Milestone__Project_Notes = a.Milestone__Project_Notes ?? "",
                                         Milestone__Reason_Code = a.Milestone__Reason_Code ?? "",
                                         Milestone__Closed_Loop_Owner = a.Milestone__Closed_Loop_Owner ?? "",
                                         Workspace_Title = a.Workspace_Title ?? "",
                                         Workspace__ELT_Overall_Status = a.Workspace__ELT_Overall_Status ?? "",
                                         Workspace__ELT_Overall_Comments = a.Workspace__ELT_Overall_Comments ?? "",
                                         Customer_Row_ID = b.Customer_Row_ID ?? 0,
                                         Opportunity_ID = b.Opportunity_ID ?? 0,
                                         Account_Name = b.Account_Name ?? "",
                                         Sales_Stage_Name = b.Sales_Stage_Name ?? "",
                                         Opportunity_Type = b.Opportunity_Type ?? "",
                                         Revenue_Opportunity_Type = b.Opportunity_Type ?? "",
                                         Revenue_Status = b.Revenue_Status ?? "",
                                         Opportunity_Owner = b.Opportunity_Owner ?? "",
                                         Opportunity_Category = b.Account_Category ?? "",
                                         Revenue_Total_Transactions = b.Total_Transactions ?? 0,
                                         CountryCode = entity.CountryIsoCodes.FirstOrDefault(x => x.CountryName == b.Country).CountryCode,
                                         //RevenueVolumeUSD = (Opportunity_Type.Any(val => b.Opportunity_Type.Equals(val)) ? b.Total_Revenue_USD
                                         //               : Opportunity_Type2.Any(val => b.Opportunity_Type.Equals(val)) ? 0
                                         //                   : Opportunity_Type.Any(val => b.Opportunity_Type.Equals(val)) ? b.Total_Revenue_USD
                                         //                       : b.Opportunity_Type == "Re-Bid With Up-Sell" ? b.Total_Revenue_USD
                                         //                           : Opportunity_Type4.Any(val => b.Opportunity_Type.Equals(val)) ? 0
                                         //                           : 0) ?? 0,

                                         RevenueVolumeUSD = (Final_Revenue_Opportunity_Type.Any(val => b.Opportunity_Type.Equals(val)) ? b.Total_Revenue_USD : (double)0) ?? (double)0,
                                         //RevenueVolumeUSD = b.Total_Revenue_USD ?? 0,
                                         //MarketLeader = b.Account_Category == "GPS" ? "Cathy Voss"
                                         //           : b.Account_Category == null ? "No data from CRM"
                                         //           : b.Region__Revenue_ == "APAC" ? "Bindu Bhatia"
                                         //           : b.Region__Revenue_ == "EMEA" ? "Chris Bowen"
                                         //           : b.Region__Revenue_ == "NORAM" ? "Barbara Bernard"
                                         //           : b.Region__Revenue_ == "LATAM" ? "Barbara Bernard"
                                         //           : "No data from CRM",
                                         GlobalProjectManager = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_Project_Manager,
                                         //ProjectConsultant = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Regional_Ops_Manager,
                                         RegionalProjectManager = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).APAC_Implementation_Lead,
                                         GlobalCISDQSLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_DQS_Lead,
                                         GlobalCISOBTLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_OBT_Lead,
                                         RegionalCISOBTLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).APAC_Digital_OBT_Lead,
                                         GlobalCISHRFeedSpecialist = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_HR_Feed_Specialist,
                                         GlobalCISPortraitLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_Portrait_Lead,
                                         RegionalCISPortraitLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).APAC_Digital_Portrait_Lead,
                                         LocalCISOBTLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Local_IN_CIS_Lead,
                                         ImplementationType = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Milestone__CRM_Revenue_ID__).Implementation_Type ?? "---",
                                         Status = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Milestone__CRM_Revenue_ID__).Status,
                                         LastUpdatedDate = b.Last_Update_Date,
                                         Description = b.Description,
                                         GDS = b.BT_Current_GDS,
                                         Awarded_Date = b.Close_Date,
                                         Close_Date = b.Close_Date,
                                         MilestoneType = a.Milestone__Milestone_type,
                                         Implementation_Fee = b.Implementation_Fee ?? 0,
                                         TypeofData = "ProductUpSell"
                                     });
            DateTime cutoff = DateTime.ParseExact("01-Sep-2025", "dd-MMM-yyyy", null);
            var GenerateCLR = (from a in entity.DataiMeets
                               where a.Task_Start_Date >= ConvertedDate
                               where a.Task_Start_Date != null
                               //where !(from b in entity.CLRBelowDatas
                               //        where b.RevenueID == a.Milestone__CRM_Revenue_ID__
                               //        select b.RevenueID).Contains(a.Milestone__CRM_Revenue_ID__)
                               where Status.Any(val => a.Milestone__Project_Status.Equals(val))
                               join b in entity.CRMDatas on a.Milestone__CRM_Revenue_ID__ equals b.Revenue_Id
                               where b.Region__Revenue_ != null
                               select new
                               {
                                   RevenueID = a.Milestone__CRM_Revenue_ID__,
                                   b.Revenue_Id,
                                   Client = b.Account_Name,
                                   //Client = "",
                                   Region = b.Region__Revenue_ ?? "",
                                   Country = b.Country ?? "",
                                   OwnerShip = b.Ownership__Revenue_ ?? "",
                                   GoLiveDate = a.Task_Start_Date,
                                   TaskStartDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone__Country == a.Milestone__Country).Task_Start_Date,
                                   //TaskDueDate = a.Task_Due_Date,
                                   //Year = a.Task_Start_Date.Value.Month.ToString("yyyy"),
                                   ProjectStatus = a.Milestone__Project_Status ?? "",
                                   ProjectStartCTCompleteDate = entity.ProjectDueDates.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == a.Milestone__CRM_Revenue_ID__ && x.Milestone_Title == a.Milestone_Title).Task_Completed_Date,
                                   ProjectStartCTDueDate = entity.ProjectDueDates.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == a.Milestone__CRM_Revenue_ID__ && x.Milestone_Title == a.Milestone_Title).Task_Due_Date,
                                   ProjectStartTaskStatus = entity.ProjectDueDates.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == a.Milestone__CRM_Revenue_ID__ && x.Milestone_Title == a.Milestone_Title).Task_Status,
                                   CountryStatus = a.Milestone__Country_Status ?? "",
                                   TaskStatus = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone__Country == a.Milestone__Country).Task_Status,
                                   ProjectLevel = a.Workspace__Project_Level ?? "",
                                   //ProjectStartDate = a.Milestone__Project_Start_Date,
                                   //IntitialGoliveDate = a.Milestone__Initial_Go_Live_Date,
                                   CompletedDate = a.Completed_Date,
                                   TaskCompletedDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone__Country == a.Milestone__Country).Task_Completed_Date,
                                   //ProjectOwner = a.Workspace__Project_Owner__Full_Name ?? "",
                                   AssigneeFullName = entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == a.Milestone__CRM_Revenue_ID__ && x.Milestone_Title == a.Milestone_Title).Milestone__Assignee__Full_Name,
                                   C__Complete = a.C__Complete ?? 0,
                                   a.Milestone_Due_Date,
                                   Line_Win_Probability = b.Line_Win_Probability ?? 0,
                                   b.Next_Step,
                                   MilestoneTitle = a.Milestone_Title ?? "",
                                   Milestone__Record_ID_Key = a.Milestone__Record_ID_Key ?? "",
                                   Task__Task_Record_ID_Key = a.Task__Task_Record_ID_Key ?? "",
                                   Group_Name = a.Group_Name ?? "",
                                   Milestone__Project_Notes = a.Milestone__Project_Notes ?? "",
                                   Milestone__Reason_Code = a.Milestone__Reason_Code ?? "",
                                   Milestone__Closed_Loop_Owner = a.Milestone__Closed_Loop_Owner ?? "",
                                   Workspace_Title = a.Workspace_Title ?? "",
                                   Workspace__ELT_Overall_Status = a.Workspace__ELT_Overall_Status ?? "",
                                   Workspace__ELT_Overall_Comments = a.Workspace__ELT_Overall_Comments ?? "",
                                   Customer_Row_ID = b.Customer_Row_ID ?? 0,
                                   Opportunity_ID = b.Opportunity_ID ?? 0,
                                   Account_Name = b.Account_Name ?? "---",
                                   Sales_Stage_Name = b.Sales_Stage_Name ?? "---",
                                   Opportunity_Type = b.Opportunity_Type ?? "---",
                                   Revenue_Opportunity_Type = b.Revenue_Opportunity_Type ?? "---",
                                   Revenue_Status = b.Revenue_Status ?? "---",
                                   Opportunity_Owner = b.Opportunity_Owner ?? "---",
                                   Opportunity_Category = b.Opportunity_Category ?? "---",
                                   Revenue_Total_Transactions = b.Revenue_Total_Transactions ?? 0,
                                   CountryCode = entity.CountryIsoCodes.FirstOrDefault(x => x.CountryName == b.Country).CountryCode,
                                   //RevenueVolumeUSD = b.Opportunity_Type == "New Business" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Up-Sell(Add Offices/Countries)" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Re-Bid" ? 0 : b.Opportunity_Type == "Renewal/Renegotiation" ? 0 : b.Opportunity_Type == null ? 0 : b.Revenue_Opportunity_Type == "" ? 0 : 0,
                                   //RevenueVolumeUSD = Opportunity_Type.Any(val => b.Opportunity_Type.Equals(val)) ? b.Revenue_Total_Volume_USD
                                   //                     : Opportunity_Type2.Any(val => b.Opportunity_Type.Equals(val))
                                   //                         ? Revenue_Opportunity_Type.Any(val => b.Revenue_Opportunity_Type.Equals(val)) ? b.Revenue_Total_Volume_USD
                                   //                         : 0
                                   //                     : 0,
                                   //Opportunity_Type = "Up-Sell(Add Offices/Countries),New Business,Up-Sell(Add Offices/Countries)".Split(',');
                                   // Opportunity_Type2 = "Re-Bid,Renewal/Renegotiation,".Split(',');
                                   // Opportunity_Type3 = "Re-Bid With Up-Sell,Lost Client (w/o notice)".Split(',');
                                   // Opportunity_Type4 = "Renewal/Renegotiation,".Split(',');
                                   // Revenue_Opportunity_Type = "Up-Sell(Add Offices/Countries),Re-Bid With Up-Sell,New Business".Split(',');
                                   //RevenueVolumeUSD = (Opportunity_Type.Any(val => b.Opportunity_Type.Equals(val)) ? b.Revenue_Total_Volume_USD
                                   //                    : Opportunity_Type2.Any(val => b.Opportunity_Type.Equals(val)) ? 0
                                   //                        : Opportunity_Type.Any(val => b.Revenue_Opportunity_Type.Equals(val)) ? b.Revenue_Total_Volume_USD
                                   //                            : b.Revenue_Opportunity_Type == "Re-Bid With Up-Sell" ? b.Total_Up_Sell_Volume_USD
                                   //                                : Opportunity_Type4.Any(val => b.Revenue_Opportunity_Type.Equals(val)) ? 0
                                   //                                : 0) ?? 0,
                                   RevenueVolumeUSD = (b.Revenue_Opportunity_Type == "Re-Bid With Up-Sell") ? b.Total_Up_Sell_Volume_USD ?? (double)0 : (Final_Revenue_Opportunity_Type.Any(val => b.Revenue_Opportunity_Type.Equals(val)) ? b.Revenue_Total_Volume_USD : (double)0) ?? (double)0,
                                   //RevenueVolumeUSD = b.Revenue_Total_Volume_USD ?? 0,
                                   //MarketLeader = b.Opportunity_Category == "GPS" ? "Cathy Voss"
                                   //                 : b.Opportunity_Category == null ? "No data from CRM"
                                   //                 : b.Region__Revenue_ == "APAC" ? "Bindu Bhatia"
                                   //                 : b.Region__Revenue_ == "EMEA" ? "Chris Bowen"
                                   //                 : b.Region__Revenue_ == "NORAM" ? "Barbara Bernard"
                                   //                 : b.Region__Revenue_ == "LATAM" ? "Barbara Bernard"
                                   //                 : "No data from CRM",
                                   GlobalProjectManager = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_Project_Manager,
                                   //ProjectConsultant = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Regional_Ops_Manager,
                                   RegionalProjectManager = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).APAC_Implementation_Lead,
                                   GlobalCISDQSLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_DQS_Lead,
                                   GlobalCISOBTLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_OBT_Lead,
                                   RegionalCISOBTLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).APAC_Digital_OBT_Lead,
                                   GlobalCISHRFeedSpecialist = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_HR_Feed_Specialist,
                                   GlobalCISPortraitLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_Portrait_Lead,
                                   RegionalCISPortraitLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).APAC_Digital_Portrait_Lead,
                                   LocalCISOBTLead = b.Region__Revenue_ == "APAC" && b.Country == "INDIA" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Local_IN_CIS_Lead : "",
                                   ImplementationType = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Milestone__CRM_Revenue_ID__).Implementation_Type ?? "---",
                                   Status = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Milestone__CRM_Revenue_ID__).Status,
                                   LastUpdatedDate = b.Last_Update_Date,
                                   Description = b.Description,
                                   GDS = b.GDS,
                                   Awarded_Date = b.Awarded_Date,
                                   Close_Date = b.Close_Date,
                                   MilestoneType = a.Milestone__Milestone_type,
                                   Implementation_Fee = (double)0,
                                   TypeofData = "Automated"
                               }).OrderBy(x => x.RevenueID).ToList();
            SalesStageName = "Contract Signed,Verbal Award".Split(',');
            SalesStageNameHP = "RFP Received,Proposal Submitted,Needs are Identified,Negotiations,RFI Received,RFI Submitted,Shortlisted,Presentation Stage".Split(',');
            Rev_opportunity_type = "New Business,Up-Sell(Add Offices/Countries),Re-Bid With Up-Sell".Split(',');
            //Re - Bid,
            Pipeline_Rev_opportunity_type = "Bid Avoidance,Re-Bid,New Business,Up-Sell(Add Offices/Countries),Re-Bid With Up-Sell,Service Model Change".Split(',');
            Revenue_Status = "LOST";
            var CloseDate = "01-01-2020";
            ClosingDate = Convert.ToDateTime(CloseDate);
            var ClosedDate = "01-01-2019";
            ClosedDateCRM = Convert.ToDateTime(ClosedDate);
            var PipelineDate = Convert.ToDateTime("01-01-2050");
            //var RevIDs = (from a in DataiMeets where a.Milestone__CRM_Revenue_ID__ != null select a.Milestone__CRM_Revenue_ID__).ToList();
            //var RevCheck = (from a in CRMDatas
            //                where !(from b in DataiMeets
            //                        where b.Milestone__CRM_Revenue_ID__ == a.Revenue_Id
            //                        select b.Milestone__CRM_Revenue_ID__).Contains(a.Revenue_Id)
            //                select a).Distinct();
            var PipelineData = (from a in entity.CRMDatas
                                where !(from b in entity.DataiMeets
                                        where b.Milestone__CRM_Revenue_ID__ == a.Revenue_Id
                                        select b.Milestone__CRM_Revenue_ID__).Contains(a.Revenue_Id)
                                where !(from b in entity.CLRBelowDatas
                                        where b.RevenueID == a.Revenue_Id
                                        select b.RevenueID).Contains(a.Revenue_Id)
                                where a.Close_Date >= ClosedDateCRM
                                where SalesStageName.Any(val => a.Sales_Stage_Name.Equals(val))
                                where Pipeline_Rev_opportunity_type.Any(val => a.Opportunity_Type.Equals(val))
                                where a.Revenue_Status != Revenue_Status
                                where a.Region__Revenue_ != null
                                //where a.Opportunity_Scope != "Local"
                                //where Status.Any(val => a.Milestone__Project_Status.Equals(val))
                                //join b in DataiMeets on a.Revenue_Id equals b.Milestone__CRM_Revenue_ID__
                                select new
                                {
                                    RevenueID = a.Revenue_Id,
                                    a.Revenue_Id,
                                    Client = a.Account_Name,
                                    Region = a.Region__Revenue_ ?? "",
                                    Country = a.Country ?? "",
                                    OwnerShip = a.Ownership__Revenue_ ?? "",
                                    GoLiveDate = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).GoLiveDate,
                                    TaskStartDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Start_Date,
                                    //TaskDueDate = a.Close_Date,
                                    //Year = a.Task_Start_Date.Value.Month.ToString("yyyy"),
                                    ProjectStatus = "P-Pipeline",
                                    ProjectStartCTCompleteDate = entity.ProjectDueDates.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Completed_Date,
                                    ProjectStartCTDueDate = entity.ProjectDueDates.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Due_Date,
                                    ProjectStartTaskStatus = "",
                                    CountryStatus = "" ?? "",
                                    TaskStatus = "",
                                    ProjectLevel = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).ProjectLevel ?? "---",
                                    //ProjectStartDate = a.Milestone__Project_Start_Date,
                                    //IntitialGoliveDate = a.Milestone__Initial_Go_Live_Date,
                                    CompletedDate = a.Close_Date,
                                    TaskCompletedDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Completed_Date,
                                    AssigneeFullName = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).AssigneeFullName,
                                    C__Complete = a.Line_Win_Probability ?? 0,
                                    Milestone_Due_Date = a.Close_Date,
                                    Line_Win_Probability = a.Line_Win_Probability ?? 0,
                                    a.Next_Step,
                                    MilestoneTitle = "" ?? "---",
                                    Milestone__Record_ID_Key = "" ?? "---",
                                    Task__Task_Record_ID_Key = "" ?? "---",
                                    Group_Name = "" ?? "---",
                                    Milestone__Project_Notes = "" ?? "---",
                                    Milestone__Reason_Code = "" ?? "---",
                                    Milestone__Closed_Loop_Owner = "" ?? "---",
                                    Workspace_Title = "" ?? "---",
                                    Workspace__ELT_Overall_Status = "" ?? "---",
                                    Workspace__ELT_Overall_Comments = "" ?? "---",
                                    Customer_Row_ID = a.Customer_Row_ID ?? 0,
                                    Opportunity_ID = a.Opportunity_ID ?? 0,
                                    Account_Name = a.Account_Name ?? "---",
                                    Sales_Stage_Name = a.Sales_Stage_Name ?? "---",
                                    Opportunity_Type = a.Opportunity_Type ?? "---",
                                    Revenue_Opportunity_Type = a.Revenue_Opportunity_Type ?? "---",
                                    Revenue_Status = a.Revenue_Status ?? "---",
                                    Opportunity_Owner = a.Opportunity_Owner ?? "---",
                                    Opportunity_Category = a.Opportunity_Category ?? "---",
                                    Revenue_Total_Transactions = a.Revenue_Total_Transactions ?? 0,
                                    CountryCode = entity.CountryIsoCodes.FirstOrDefault(x => x.CountryName == a.Country).CountryCode,
                                    //RevenueVolumeUSD = b.Opportunity_Type == "New Business" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Up-Sell(Add Offices/Countries)" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Re-Bid" ? 0 : b.Opportunity_Type == "Renewal/Renegotiation" ? 0 : b.Opportunity_Type == null ? 0 : b.Revenue_Opportunity_Type == "" ? 0 : 0,
                                    //RevenueVolumeUSD = (Opportunity_Type.Any(val => a.Opportunity_Type.Equals(val)) ? a.Revenue_Total_Volume_USD
                                    //                    : Opportunity_Type2.Any(val => a.Opportunity_Type.Equals(val)) ? 0
                                    //                        : Opportunity_Type.Any(val => a.Revenue_Opportunity_Type.Equals(val)) ? a.Revenue_Total_Volume_USD
                                    //                            : a.Revenue_Opportunity_Type == "Re-Bid With Up-Sell" ? a.Total_Up_Sell_Volume_USD
                                    //                                : Opportunity_Type4.Any(val => a.Revenue_Opportunity_Type.Equals(val)) ? 0
                                    //                                : 0) ?? 0,

                                    RevenueVolumeUSD = a.Revenue_Opportunity_Type == "Re-Bid With Up-Sell" ? a.Total_Up_Sell_Volume_USD ?? (double)0 : (Final_Revenue_Opportunity_Type.Any(val => a.Revenue_Opportunity_Type.Equals(val)) ? a.Revenue_Total_Volume_USD : (double)0) ?? (double)0,
                                    //RevenueVolumeUSD = a.Revenue_Total_Volume_USD ?? 0,
                                    //MarketLeader = a.Opportunity_Category == "GPS" ? "Cathy Voss"
                                    //                : a.Opportunity_Category == null ? "No data from CRM"
                                    //                : a.Region__Revenue_ == "APAC" ? "Bindu Bhatia"
                                    //                : a.Region__Revenue_ == "EMEA" ? "Chris Bowen"
                                    //                : a.Region__Revenue_ == "NORAM" ? "Barbara Bernard"
                                    //                : a.Region__Revenue_ == "LATAM" ? "Barbara Bernard"
                                    //                : "No data from CRM",
                                    GlobalProjectManager = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).GlobalProjectManager,
                                    //ProjectConsultant = entity.Roles.FirstOrDefault(x => x.Workspace_Title == "----").Regional_Ops_Manager,
                                    RegionalProjectManager = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).RegionalProjectManager,
                                    GlobalCISDQSLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISDQSLead,
                                    GlobalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISOBTLead,
                                    RegionalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).RegionalCISOBTLead,
                                    GlobalCISHRFeedSpecialist = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISHRFeedSpecialist,
                                    GlobalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISPortraitLead,
                                    RegionalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).RegionalCISPortraitLead,
                                    LocalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).LocalDigitalOBTLead,
                                    ImplementationType = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).Implementation_Type ?? "---",
                                    Status = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).Status,
                                    LastUpdatedDate = a.Last_Update_Date,
                                    Description = a.Description,
                                    GDS = a.GDS,
                                    Awarded_Date = a.Awarded_Date,
                                    Close_Date = a.Close_Date,
                                    MilestoneType = "",
                                    Implementation_Fee = (double)0,
                                    TypeofData = "Pipeline"
                                }).ToList();
            var HighPotentialData = (from a in entity.CRMDatas
                                     where !(from b in entity.DataiMeets
                                             where b.Milestone__CRM_Revenue_ID__ == a.Revenue_Id
                                             select b.Milestone__CRM_Revenue_ID__).Contains(a.Revenue_Id)
                                     where !(from b in entity.CLRBelowDatas
                                             where b.RevenueID == a.Revenue_Id
                                             select b.RevenueID).Contains(a.Revenue_Id)
                                     where a.Close_Date >= ClosingDate
                                     where a.Region__Revenue_ != null
                                     where SalesStageNameHP.Any(val => a.Sales_Stage_Name.Equals(val))
                                     where Pipeline_Rev_opportunity_type.Any(val => a.Opportunity_Type.Equals(val))
                                     where a.Revenue_Status != Revenue_Status
                                     where a.Line_Win_Probability >= 60 && a.Line_Win_Probability <= 99
                                     select new
                                     {
                                         RevenueID = a.Revenue_Id,
                                         a.Revenue_Id,
                                         Client = a.Account_Name,
                                         Region = a.Region__Revenue_ ?? "",
                                         Country = a.Country ?? "",
                                         OwnerShip = a.Ownership__Revenue_ ?? "",
                                         GoLiveDate = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).GoLiveDate,
                                         TaskStartDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Start_Date,
                                         ProjectStatus = "HP-High Potential",
                                         ProjectStartCTCompleteDate = entity.ProjectDueDates.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == 0).Task_Completed_Date,
                                         ProjectStartCTDueDate = entity.ProjectDueDates.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == 0).Task_Due_Date,
                                         ProjectStartTaskStatus = "",
                                         CountryStatus = "" ?? "",
                                         TaskStatus = "",
                                         ProjectLevel = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).ProjectLevel ?? "",
                                         //ProjectStartDate = a.Milestone__Project_Start_Date,
                                         //IntitialGoliveDate = a.Milestone__Initial_Go_Live_Date,
                                         CompletedDate = a.Close_Date,
                                         TaskCompletedDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Completed_Date,
                                         AssigneeFullName = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).AssigneeFullName,
                                         C__Complete = a.Line_Win_Probability ?? 0,
                                         Milestone_Due_Date = a.Close_Date,
                                         Line_Win_Probability = a.Line_Win_Probability ?? 0,
                                         a.Next_Step,
                                         MilestoneTitle = "" ?? "---",
                                         Milestone__Record_ID_Key = "" ?? "---",
                                         Task__Task_Record_ID_Key = "" ?? "---",
                                         Group_Name = "" ?? "---",
                                         Milestone__Project_Notes = "" ?? "---",
                                         Milestone__Reason_Code = "" ?? "---",
                                         Milestone__Closed_Loop_Owner = "" ?? "---",
                                         Workspace_Title = "" ?? "---",
                                         Workspace__ELT_Overall_Status = "" ?? "---",
                                         Workspace__ELT_Overall_Comments = "" ?? "---",
                                         Customer_Row_ID = a.Customer_Row_ID ?? 0,
                                         Opportunity_ID = a.Opportunity_ID ?? 0,
                                         Account_Name = a.Account_Name ?? "---",
                                         Sales_Stage_Name = a.Sales_Stage_Name ?? "---",
                                         Opportunity_Type = a.Opportunity_Type ?? "---",
                                         Revenue_Opportunity_Type = a.Revenue_Opportunity_Type ?? "---",
                                         Revenue_Status = a.Revenue_Status ?? "---",
                                         Opportunity_Owner = a.Opportunity_Owner ?? "---",
                                         Opportunity_Category = a.Opportunity_Category ?? "---",
                                         Revenue_Total_Transactions = a.Revenue_Total_Transactions ?? 0,
                                         CountryCode = entity.CountryIsoCodes.FirstOrDefault(x => x.CountryName == a.Country).CountryCode,
                                         //RevenueVolumeUSD = b.Opportunity_Type == "New Business" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Up-Sell(Add Offices/Countries)" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Re-Bid" ? 0 : b.Opportunity_Type == "Renewal/Renegotiation" ? 0 : b.Opportunity_Type == null ? 0 : b.Revenue_Opportunity_Type == "" ? 0 : 0,
                                         //RevenueVolumeUSD = (Opportunity_Type.Any(val => a.Opportunity_Type.Equals(val)) ? a.Revenue_Total_Volume_USD
                                         //                    : Opportunity_Type2.Any(val => a.Opportunity_Type.Equals(val)) ? 0
                                         //                        : Opportunity_Type.Any(val => a.Revenue_Opportunity_Type.Equals(val)) ? a.Revenue_Total_Volume_USD
                                         //                            : a.Revenue_Opportunity_Type == "Re-Bid With Up-Sell" ? a.Total_Up_Sell_Volume_USD
                                         //                                : Opportunity_Type4.Any(val => a.Revenue_Opportunity_Type.Equals(val)) ? 0
                                         //                                : 0) ?? 0,

                                         RevenueVolumeUSD = a.Revenue_Opportunity_Type == "Re-Bid With Up-Sell" ? a.Total_Up_Sell_Volume_USD ?? (double)0 : (Final_Revenue_Opportunity_Type.Any(val => a.Revenue_Opportunity_Type.Equals(val)) ? a.Revenue_Total_Volume_USD : (double)0) ?? (double)0,
                                         //RevenueVolumeUSD = a.Revenue_Total_Volume_USD ?? 0,
                                         //MarketLeader = a.Opportunity_Category == "GPS" ? "Cathy Voss"
                                         //                : a.Opportunity_Category == null ? "No data from CRM"
                                         //                : a.Region__Revenue_ == "APAC" ? "Bindu Bhatia"
                                         //                : a.Region__Revenue_ == "EMEA" ? "Chris Bowen"
                                         //                : a.Region__Revenue_ == "NORAM" ? "Barbara Bernard"
                                         //                : a.Region__Revenue_ == "LATAM" ? "Barbara Bernard"
                                         //                : "No data from CRM",
                                         GlobalProjectManager = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).GlobalProjectManager,
                                         //ProjectConsultant = entity.Roles.FirstOrDefault(x => x.Workspace_Title == "----").Regional_Ops_Manager,
                                         RegionalProjectManager = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).RegionalProjectManager,
                                         GlobalCISDQSLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISDQSLead,
                                         GlobalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISOBTLead,
                                         RegionalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).RegionalCISOBTLead,
                                         GlobalCISHRFeedSpecialist = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISHRFeedSpecialist,
                                         GlobalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISPortraitLead,
                                         RegionalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).RegionalCISPortraitLead,
                                         LocalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).LocalDigitalOBTLead,
                                         ImplementationType = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).Implementation_Type ?? "---",
                                         Status = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).Status,
                                         LastUpdatedDate = a.Last_Update_Date ?? null,
                                         Description = a.Description,
                                         GDS = a.GDS,
                                         Awarded_Date = a.Awarded_Date,
                                         Close_Date = a.Close_Date,
                                         MilestoneType = "",
                                         Implementation_Fee = (double)0,
                                         TypeofData = "Pipeline"
                                     }).ToList();
            var EpSalesStageName = "Negotiations,Proposal Submitted,Shortlisted,Presentation Stage".Split(',');
            //Re - Bid,
            var EpOpportunityType = "Bid Avoidance,Re-Bid,New Business,Service Model Change,Re-Bid With Up-Sell,Up-Sell(Add Offices/Countries),".Split(',');
            //var EarlyPotential = (from a in entity.CRMDatas
            //                    where !(from b in entity.DataiMeets
            //                            where b.Milestone__CRM_Revenue_ID__ == a.Revenue_Id
            //                            select b.Milestone__CRM_Revenue_ID__).Contains(a.Revenue_Id)
            //                    where !(from b in entity.CLRBelowDatas
            //                            where b.RevenueID == a.Revenue_Id
            //                            select b.RevenueID).Contains(a.Revenue_Id)
            //                    where a.Close_Date >= ClosedDateCRM
            //                    where EpSalesStageName.Any(val => a.Sales_Stage_Name.Equals(val))
            //                    where EpOpportunityType.Any(val => a.Opportunity_Type.Equals(val))
            //                    where a.Line_Win_Probability >= 30
            //                    where a.Line_Win_Probability <= 59
            //                    where a.Region__Revenue_ != null
            //                    where a.Opportunity_Total_Volume_USD > 15000000
            //                    select new
            //                    {
            //                        RevenueID = a.Revenue_Id,
            //                        a.Revenue_Id,
            //                        Client = a.Account_Name,
            //                        Region = a.Region__Revenue_ ?? "",
            //                        Country = a.Country ?? "",
            //                        OwnerShip = a.Ownership__Revenue_ ?? "",
            //                        GoLiveDate = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).GoLiveDate,
            //                        TaskStartDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Start_Date,
            //                        //TaskDueDate = a.Close_Date,
            //                        //Year = a.Task_Start_Date.Value.Month.ToString("yyyy"),
            //                        ProjectStatus = "EP-Early Potential",
            //                        ProjectStartCTCompleteDate = entity.ProjectDueDates.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Completed_Date,
            //                        ProjectStartCTDueDate = entity.ProjectDueDates.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Due_Date,
            //                        ProjectStartTaskStatus = "",
            //                        CountryStatus = "" ?? "",
            //                        TaskStatus = "",
            //                        ProjectLevel = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).ProjectLevel ?? "",
            //                        //ProjectStartDate = a.Milestone__Project_Start_Date,
            //                        //IntitialGoliveDate = a.Milestone__Initial_Go_Live_Date,
            //                        CompletedDate = a.Close_Date,
            //                        TaskCompletedDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Completed_Date,
            //                        AssigneeFullName = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).AssigneeFullName,
            //                        C__Complete = a.Line_Win_Probability ?? 0,
            //                        Milestone_Due_Date = a.Close_Date,
            //                        Line_Win_Probability = a.Line_Win_Probability ?? 0,
            //                        a.Next_Step,
            //                        MilestoneTitle = "" ?? "---",
            //                        Milestone__Record_ID_Key = "" ?? "---",
            //                        Task__Task_Record_ID_Key = "" ?? "---",
            //                        Group_Name = "" ?? "---",
            //                        Milestone__Project_Notes = "" ?? "---",
            //                        Milestone__Reason_Code = "" ?? "---",
            //                        Milestone__Closed_Loop_Owner = "" ?? "---",
            //                        Workspace_Title = "" ?? "---",
            //                        Workspace__ELT_Overall_Status = "" ?? "---",
            //                        Workspace__ELT_Overall_Comments = "" ?? "---",
            //                        Customer_Row_ID = a.Customer_Row_ID ?? 0,
            //                        Opportunity_ID = a.Opportunity_ID ?? 0,
            //                        Account_Name = a.Account_Name ?? "---",
            //                        Sales_Stage_Name = a.Sales_Stage_Name ?? "---",
            //                        Opportunity_Type = a.Opportunity_Type ?? "---",
            //                        Revenue_Opportunity_Type = a.Revenue_Opportunity_Type ?? "---",
            //                        Revenue_Status = a.Revenue_Status ?? "---",
            //                        Opportunity_Owner = a.Opportunity_Owner ?? "---",
            //                        Opportunity_Category = a.Opportunity_Category ?? "---",
            //                        Revenue_Total_Transactions = a.Revenue_Total_Transactions ?? 0,
            //                        CountryCode = entity.CountryIsoCodes.FirstOrDefault(x => x.CountryName == a.Country).CountryCode,
            //                        //RevenueVolumeUSD = b.Opportunity_Type == "New Business" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Up-Sell(Add Offices/Countries)" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Re-Bid" ? 0 : b.Opportunity_Type == "Renewal/Renegotiation" ? 0 : b.Opportunity_Type == null ? 0 : b.Revenue_Opportunity_Type == "" ? 0 : 0,
            //                        //RevenueVolumeUSD = a.Revenue_Total_Volume_USD ?? 0,
            //                        RevenueVolumeUSD = a.Revenue_Opportunity_Type == "Re-Bid With Up-Sell" ? a.Total_Up_Sell_Volume_USD ?? (double)0 : (Final_Revenue_Opportunity_Type.Any(val => a.Revenue_Opportunity_Type.Equals(val)) ? a.Revenue_Total_Volume_USD : (double)0) ?? (double)0,
            //                        //MarketLeader = a.Opportunity_Category == "GPS" ? "Cathy Voss"
            //                        //                : a.Opportunity_Category == null ? "No data from CRM"
            //                        //                : a.Region__Revenue_ == "APAC" ? "Bindu Bhatia"
            //                        //                : a.Region__Revenue_ == "EMEA" ? "Chris Bowen"
            //                        //                : a.Region__Revenue_ == "NORAM" ? "Barbara Bernard"
            //                        //                : a.Region__Revenue_ == "LATAM" ? "Barbara Bernard"
            //                        //                : "No data from CRM",
            //                        GlobalProjectManager = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).GlobalProjectManager,
            //                        //ProjectConsultant = entity.Roles.FirstOrDefault(x => x.Workspace_Title == "----").Regional_Ops_Manager,
            //                        RegionalProjectManager = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).RegionalProjectManager,
            //                        GlobalCISDQSLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISDQSLead,
            //                        GlobalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISOBTLead,
            //                        RegionalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).RegionalCISOBTLead,
            //                        GlobalCISHRFeedSpecialist = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISHRFeedSpecialist,
            //                        GlobalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).GlobalCISPortraitLead,
            //                        RegionalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).RegionalCISPortraitLead,
            //                        LocalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_Id).LocalDigitalOBTLead,
            //                        ImplementationType = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).Implementation_Type ?? "---",
            //                        Status = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.Revenue_Id).Status,
            //                        LastUpdatedDate = a.Last_Update_Date,
            //                        Description = a.Description,
            //                        GDS = a.GDS,
            //                        Awarded_Date = a.Awarded_Date,
            //                        Close_Date = a.Close_Date,
            //                        MilestoneType = "---",
            //                        Implementation_Fee = (double)0,
            //                        TypeofData = "Pipeline"
            //                    }).ToList();
            crm_countries = "BULGARIA,EGYPT,ESTONIA,GREECE,HUNGARY,INDONESIA,Kazakhstan,Latvia,Morocco,Norway,Panama,Poland,Turkey,Ukraine".Split(',');
            var SixSeriesData = (from a in entity.AdHocProjects
                                 select new
                                 {
                                     RevenueID = a.RevenueID,
                                     Revenue_Id = a.RevenueID,
                                     Client = a.Client,
                                     Region = a.Region ?? "",
                                     Country = a.Country ?? "",
                                     OwnerShip = "",
                                     GoLiveDate = a.GoLiveDate,
                                     TaskStartDate = a.StartDate,
                                     ProjectStatus = a.ProjectStatus,
                                     ProjectStartCTCompleteDate = a.StartDate,
                                     ProjectStartCTDueDate = a.GoLiveDate,
                                     ProjectStartTaskStatus = "",
                                     CountryStatus = "" ?? "",
                                     TaskStatus = "",
                                     ProjectLevel = "",
                                     CompletedDate = a.GoLiveDate,
                                     TaskCompletedDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == "---" && x.Milestone__Country == "---").Task_Completed_Date,
                                     AssigneeFullName = "",
                                     C__Complete = (double)0,
                                     Milestone_Due_Date = a.GoLiveDate,
                                     Line_Win_Probability = (double)0,
                                     Next_Step = "",
                                     MilestoneTitle = "" ?? "",
                                     Milestone__Record_ID_Key = "" ?? "",
                                     Task__Task_Record_ID_Key = "" ?? "",
                                     Group_Name = "" ?? "",
                                     Milestone__Project_Notes = "" ?? "",
                                     Milestone__Reason_Code = "" ?? "",
                                     Milestone__Closed_Loop_Owner = "" ?? "",
                                     Workspace_Title = "" ?? "",
                                     Workspace__ELT_Overall_Status = "" ?? "",
                                     Workspace__ELT_Overall_Comments = "" ?? "",
                                     Customer_Row_ID = (double)0,
                                     Opportunity_ID = (double)0,
                                     Account_Name = "---",
                                     Sales_Stage_Name = "---",
                                     Opportunity_Type = "---",
                                     Revenue_Opportunity_Type = "---",
                                     Revenue_Status = "---",
                                     Opportunity_Owner = "---",
                                     Opportunity_Category = "---",
                                     Revenue_Total_Transactions = (double)0,
                                     CountryCode = entity.CountryIsoCodes.FirstOrDefault(x => x.CountryName == a.Country).CountryCode,
                                     //RevenueVolumeUSD = b.Opportunity_Type == "New Business" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Up-Sell(Add Offices/Countries)" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Re-Bid" ? 0 : b.Opportunity_Type == "Renewal/Renegotiation" ? 0 : b.Opportunity_Type == null ? 0 : b.Revenue_Opportunity_Type == "" ? 0 : 0,
                                     RevenueVolumeUSD = (double)0,
                                     //MarketLeader = "---",
                                     GlobalProjectManager = "---",
                                     //ProjectConsultant = "---",
                                     RegionalProjectManager = "---",
                                     GlobalCISDQSLead = a.GlobalDQSLead,
                                     GlobalCISOBTLead = a.GlobalCISOBTLead,
                                     RegionalCISOBTLead = a.RegionalCISOBTLead,
                                     GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist,
                                     GlobalCISPortraitLead = a.GlobalCISPortraitLead,
                                     RegionalCISPortraitLead = a.RegionalCISPortraitLead,
                                     LocalCISOBTLead = a.LocalDigitalOBTLead,
                                     ImplementationType = "---",
                                     Status = a.Status,
                                     LastUpdatedDate = a.GoLiveDate,
                                     Description = "---",
                                     GDS = a.GDS,
                                     Awarded_Date = a.GoLiveDate,
                                     Close_Date = a.GoLiveDate,
                                     MilestoneType = "---",
                                     Implementation_Fee = (double)0,
                                     TypeofData = "Ad-Hoc Digital Data"
                                 }).ToList();
            //var OldData = (from a in entity.CLRBelowDatas
            //               where a.RevenueID != null
            //               //join a in DataiMeets on b.Revenue_Id equals a.Milestone__CRM_Revenue_ID__
            //               select new
            //               {
            //                   RevenueID = a.RevenueID,
            //                   Revenue_Id = a.RevenueID,
            //                   Client = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).Client,
            //                   Region = a.Region ?? "",
            //                   Country = a.Country ?? "",
            //                   OwnerShip = a.OwnerShip ?? "",
            //                   GoLiveDate = a.GoLiveDate,
            //                   TaskStartDate = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).ExternalKickoffDuedate,
            //                   //TaskDueDate = a.ExternalKickoffDuedate,
            //                   //Year = a.Task_Start_Date.Value.Month.ToString("yyyy"),
            //                   ProjectStatus = a.ProjectStatus ?? "",
            //                   ProjectStartCTCompleteDate = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).ProjectStartForCycleTime,
            //                   ProjectStartCTDueDate = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).ProjectStartForCycleTime,
            //                   ProjectStartTaskStatus = "",
            //                   CountryStatus = a.CountryStatus ?? "",
            //                   TaskStatus = "",
            //                   ProjectLevel = a.ProjectLevel ?? "",
            //                   //ProjectStartDate = a.Milestone__Project_Start_Date,
            //                   //IntitialGoliveDate = a.Milestone__Initial_Go_Live_Date,
            //                   CompletedDate = a.CompletedDate,
            //                   TaskCompletedDate = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).CompletedDate,
            //                   AssigneeFullName = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).AssigneeFullName,
            //                   C__Complete = a.PerCompleted,
            //                   Milestone_Due_Date = a.MilestoneDueDate,
            //                   Line_Win_Probability = a.Line_Win_Probability,
            //                   Next_Step = a.Next_Step,
            //                   MilestoneTitle = a.MilestoneTitle ?? "",
            //                   Milestone__Record_ID_Key = a.Milestone__Record_ID_Key ?? "",
            //                   Task__Task_Record_ID_Key = a.Task__Task_Record_ID_Key ?? "",
            //                   Group_Name = a.Group_Name ?? "",
            //                   Milestone__Project_Notes = a.Milestone__Project_Notes ?? "",
            //                   Milestone__Reason_Code = a.Milestone__Reason_Code ?? "",
            //                   Milestone__Closed_Loop_Owner = a.Milestone__Closed_Loop_Owner ?? "",
            //                   Workspace_Title = a.Workspace_Title ?? "",
            //                   Workspace__ELT_Overall_Status = a.Workspace__ELT_Overall_Status ?? "",
            //                   Workspace__ELT_Overall_Comments = a.Workspace__ELT_Overall_Comments ?? "",
            //                   Customer_Row_ID = a.Customer_Row_ID,
            //                   Opportunity_ID = a.Opportunity_ID,
            //                   Account_Name = a.Account_Name ?? "",
            //                   Sales_Stage_Name = a.Sales_Stage_Name ?? "",
            //                   Opportunity_Type = a.Opportunity_Type ?? "",
            //                   Revenue_Opportunity_Type = a.Opportunity_Type ?? "",
            //                   Revenue_Status = a.Revenue_Status ?? "",
            //                   Opportunity_Owner = a.Opportunity_Owner ?? "",
            //                   Opportunity_Category = a.Opportunity_Category ?? "",
            //                   Revenue_Total_Transactions = a.Revenue_Total_Transactions,
            //                   CountryCode = CountryIsoCodes.FirstOrDefault(x => x.CountryName == a.Country).CountryCode,
            //                   RevenueVolumeUSD = a.RevenueVolumeUSD,
            //                   MarketLeader = a.MarketLeader,
            //                   GlobalProjectManager = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).GlobalProjectManager,
            //                   ProjectConsultant = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).ProjectConsultant,
            //                   RegionalProjectManager = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).RegionalProjectManager,
            //                   GlobalCISOBTLead = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).GlobalCISOBTLead,
            //                   RegionalCISOBTLead = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).RegionalCISOBTLead,
            //                   GlobalCISHRFeedSpecialist = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).GlobalCISHRFeedSpecialist,
            //                   GlobalCISPortraitLead = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).GlobalCISPortraitLead,
            //                   RegionalCISPortraitLead = entity.CLRBelowDatas.FirstOrDefault(x => x.CLRID == a.CLRID).RegionalCISPortraitLead,
            //                   LocalCISOBTLead = Roles.FirstOrDefault(x => x.Workspace_Title == "----").Local_IN_CIS_Lead,
            //                   ImplementationType = ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).Implementation_Type,
            //                   Status = ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).Status,
            //                   LastUpdatedDate = a.MilestoneDueDate == TodayDate ? a.MilestoneDueDate : null,
            //                   Description = "",
            //                   GDS = "",
            //                   Awarded_Date = a.CompletedDate,
            //                   Close_Date = a.CompletedDate,
            //                   MilestoneType = "",
            //                   Implementation_Fee = a.Line_Win_Probability ?? 0,
            //                   TypeofData = "BelowData"
            //               });
            var FourseriesNumbers = (from a in entity.DataiMeets
                                     where a.Milestone__CRM_Revenue_ID__ == 400000000000000
                                     where a.Task__Task_Record_ID_Key != null
                                     where a.Task__Task_Record_ID_Key != ""
                                     where a.Task_Start_Date != null
                                     where a.Milestone__Region != "Global"
                                     where Status.Any(val => a.Milestone__Project_Status.Equals(val))
                                     select new
                                     {
                                         RevenueID = a.Milestone__CRM_Revenue_ID__,
                                         Revenue_Id = a.Milestone__CRM_Revenue_ID__,
                                         Client = a.Workspace_Title ?? "",
                                         Region = a.Milestone__Region ?? "",
                                         Country = a.Milestone__Country ?? "",
                                         OwnerShip = "",
                                         GoLiveDate = a.Task_Start_Date,
                                         TaskStartDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone__Country == a.Milestone__Country).Task_Start_Date,
                                         //TaskDueDate = a.Task_Due_Date,
                                         //Year = a.Task_Start_Date.Value.Month.ToString("yyyy"),
                                         ProjectStatus = a.Milestone__Project_Status ?? "",
                                         ProjectStartCTCompleteDate = entity.ProjectDueDates.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone_Title == a.Milestone_Title && x.Milestone__Country == a.Milestone__Country).Task_Completed_Date,
                                         ProjectStartCTDueDate = entity.ProjectDueDates.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone_Title == a.Milestone_Title && x.Milestone__Country == a.Milestone__Country).Task_Due_Date,
                                         ProjectStartTaskStatus = entity.ProjectDueDates.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone_Title == a.Milestone_Title && x.Milestone__Country == a.Milestone__Country).Task_Status,
                                         CountryStatus = a.Milestone__Country_Status ?? "",
                                         TaskStatus = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone__Country == a.Milestone__Country).Task_Status,
                                         ProjectLevel = a.Workspace__Project_Level ?? "",
                                         //ProjectStartDate = a.Milestone__Project_Start_Date,
                                         //IntitialGoliveDate = a.Milestone__Initial_Go_Live_Date,
                                         CompletedDate = a.Completed_Date,
                                         TaskCompletedDate = entity.HostExternals.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone__Country == a.Milestone__Country).Task_Completed_Date,
                                         //ProjectOwner = a.Workspace__Project_Owner__Full_Name ?? "",
                                         AssigneeFullName = entity.AssignePersons.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title && x.Milestone_Title == a.Milestone_Title && x.Milestone__Country == a.Milestone__Country).Milestone__Assignee__Full_Name,
                                         C__Complete = a.C__Complete ?? 0,
                                         Milestone_Due_Date = a.Milestone_Due_Date,
                                         Line_Win_Probability = (double)0,
                                         Next_Step = "",
                                         MilestoneTitle = a.Milestone_Title ?? "",
                                         Milestone__Record_ID_Key = a.Milestone__Record_ID_Key ?? "",
                                         Task__Task_Record_ID_Key = a.Task__Task_Record_ID_Key ?? "",
                                         Group_Name = a.Group_Name ?? "",
                                         Milestone__Project_Notes = a.Milestone__Project_Notes ?? "",
                                         Milestone__Reason_Code = a.Milestone__Reason_Code ?? "",
                                         Milestone__Closed_Loop_Owner = a.Milestone__Closed_Loop_Owner ?? "",
                                         Workspace_Title = a.Workspace_Title ?? "",
                                         Workspace__ELT_Overall_Status = a.Workspace__ELT_Overall_Status ?? "",
                                         Workspace__ELT_Overall_Comments = a.Workspace__ELT_Overall_Comments ?? "",
                                         Customer_Row_ID = (double)0,
                                         Opportunity_ID = (double)0,
                                         Account_Name = "---",
                                         Sales_Stage_Name = "---",
                                         Opportunity_Type = "---",
                                         Revenue_Opportunity_Type = "---",
                                         Revenue_Status = "---",
                                         Opportunity_Owner = "---",
                                         Opportunity_Category = "---",
                                         Revenue_Total_Transactions = (double)0,
                                         CountryCode = entity.CountryIsoCodes.FirstOrDefault(x => x.CountryName == a.Milestone__Country).CountryCode,
                                         //RevenueVolumeUSD = b.Opportunity_Type == "New Business" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Up-Sell(Add Offices/Countries)" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Re-Bid" ? 0 : b.Opportunity_Type == "Renewal/Renegotiation" ? 0 : b.Opportunity_Type == null ? 0 : b.Revenue_Opportunity_Type == "" ? 0 : 0,
                                         RevenueVolumeUSD = (double)0,
                                         //MarketLeader = "No data from CRM",
                                         GlobalProjectManager = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_Project_Manager,
                                         //ProjectConsultant = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Regional_Ops_Manager,
                                         RegionalProjectManager = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).APAC_Implementation_Lead,
                                         GlobalCISDQSLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_DQS_Lead,
                                         GlobalCISOBTLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_OBT_Lead,
                                         RegionalCISOBTLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).APAC_Digital_OBT_Lead,
                                         GlobalCISHRFeedSpecialist = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_HR_Feed_Specialist,
                                         GlobalCISPortraitLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Global_CIS_Portrait_Lead,
                                         RegionalCISPortraitLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).APAC_Digital_Portrait_Lead,
                                         LocalCISOBTLead = entity.Roles.FirstOrDefault(x => x.Workspace_Title == a.Workspace_Title).Local_IN_CIS_Lead,
                                         ImplementationType = entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).Implementation_Type ?? "---",
                                         Status = entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).Status,
                                         LastUpdatedDate = a.Task_Start_Date == TodayDate ? a.Task_Start_Date : null,
                                         Description = "",
                                         GDS = "",
                                         Awarded_Date = a.Milestone_Due_Date,
                                         Close_Date = a.Milestone_Due_Date,
                                         MilestoneType = a.Milestone__Milestone_type,
                                         Implementation_Fee = (double)0,
                                         TypeofData = "FourSerieData"
                                     });
            var FinalList = PipelineData.Concat(GenerateCLR).Concat(HighPotentialData).Concat(ProductUpSellData).Concat(FourseriesNumbers).Concat(SixSeriesData);
            var imp_listOne = "Single Resource Add OBT/Change OBT,Add/Change OBT,Add/Change Subunit,Add OBT,Add/Change Configuration,Existing Client,Change OBT".Split(',');
            var imp_listTwo = "Merger,Client top consolidation,Renewal/Renegotiation,Split,Product or Service,HRFeed,Re-Bid With Up-Sell,Re-Bid With Up-sell,Re-Bid,Program Update,Cancelled,Service Model Change".Split(',');
            var imp_listThree = "Up-Sell(Add Offices/Countries),New Business,Up-Sell,Product Up-Sell,Single Resource New Business".Split(',');

            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("RevenueID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Region", typeof(string)));
            tbl.Columns.Add(new DataColumn("Country", typeof(string)));
            tbl.Columns.Add(new DataColumn("OwnerShip", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("CountryStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("ProjectLevel", typeof(string)));
            tbl.Columns.Add(new DataColumn("CompletedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("AssigneeFullName", typeof(string)));
            tbl.Columns.Add(new DataColumn("MilestoneTitle", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Record_ID_Key", typeof(string)));
            tbl.Columns.Add(new DataColumn("Task__Task_Record_ID_Key", typeof(string)));
            tbl.Columns.Add(new DataColumn("Group_Name", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Project_Notes", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Reason_Code", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Closed_Loop_Owner", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace_Title", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace__ELT_Overall_Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace__ELT_Overall_Comments", typeof(string)));
            tbl.Columns.Add(new DataColumn("Customer_Row_ID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Opportunity_ID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Account_Name", typeof(string)));
            tbl.Columns.Add(new DataColumn("Sales_Stage_Name", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Type", typeof(string)));
            tbl.Columns.Add(new DataColumn("Revenue_Opportunity_Type", typeof(string)));
            tbl.Columns.Add(new DataColumn("Revenue_Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Owner", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Category", typeof(string)));
            tbl.Columns.Add(new DataColumn("Revenue_Total_Transactions", typeof(float)));
            tbl.Columns.Add(new DataColumn("CountryCode", typeof(string)));
            tbl.Columns.Add(new DataColumn("Client", typeof(string))); 
            tbl.Columns.Add(new DataColumn("GDS", typeof(string)));
            tbl.Columns.Add(new DataColumn("RevenueVolumeUSD", typeof(double)));
            tbl.Columns.Add(new DataColumn("GlobalProjectManager", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISOBTLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISHRFeedSpecialist", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISPortraitLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveMonth", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveYear", typeof(string)));
            tbl.Columns.Add(new DataColumn("Quarter", typeof(string))); 
            tbl.Columns.Add(new DataColumn("YearMonth", typeof(string))); 
            tbl.Columns.Add(new DataColumn("CycleTime", typeof(float)));
            tbl.Columns.Add(new DataColumn("PerCompleted", typeof(float)));
            tbl.Columns.Add(new DataColumn("MilestoneDueDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Line_Win_Probability", typeof(float)));
            tbl.Columns.Add(new DataColumn("Next_Step", typeof(string)));
            tbl.Columns.Add(new DataColumn("ExternalKickoffDuedate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("RegionalProjectManager", typeof(string)));
            tbl.Columns.Add(new DataColumn("RegionalCISOBTLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("RegionalCISPortraitLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("LocalDigitalOBTLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("ProjectStart_ForCycleTime", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("ImplementationType", typeof(string)));
            tbl.Columns.Add(new DataColumn("DataSourceType", typeof(string)));
            tbl.Columns.Add(new DataColumn("LastUpdateDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("AwardedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ClosedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("MilestoneType", typeof(string)));
            tbl.Columns.Add(new DataColumn("DataDescription", typeof(string)));
            tbl.Columns.Add(new DataColumn("AccountOwner", typeof(string)));
            tbl.Columns.Add(new DataColumn("TaskStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("CycleTimeCategories", typeof(string)));
            tbl.Columns.Add(new DataColumn("ImplementationFeePsd", typeof(double)));
            tbl.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("OBTReseller", typeof(string)));
            tbl.Columns.Add(new DataColumn("DigitalGDS", typeof(string)));
            tbl.Columns.Add(new DataColumn("DigitalActivityType", typeof(string)));
            tbl.Columns.Add(new DataColumn("SOWStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("ImplementationReady", typeof(string)));
            tbl.Columns.Add(new DataColumn("AccountCategory", typeof(string)));
            tbl.Columns.Add(new DataColumn("OppTOtalVolume", typeof(float)));
            tbl.Columns.Add(new DataColumn("Service_Configuration", typeof(string)));
            tbl.Columns.Add(new DataColumn("eSowGDS", typeof(string)));
            tbl.Columns.Add(new DataColumn("LocalDigitalAdHocSupport", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISDQSLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("Service_Location", typeof(string)));
            tbl.Columns.Add(new DataColumn("OBTAdoptionRate", typeof(string)));
            tbl.Columns.Add(new DataColumn("CycleTimeDelayCode", typeof(string))); 
            tbl.Columns.Add(new DataColumn("EltClientDelayDescription", typeof(string)));
            tbl.Columns.Add(new DataColumn("APAC_DQS", typeof(string)));
            tbl.Columns.Add(new DataColumn("DQS_Import", typeof(string)));
            tbl.Columns.Add(new DataColumn("DQS_Support", typeof(string)));
            tbl.Columns.Add(new DataColumn("LATAM_DQS", typeof(string)));
            tbl.Columns.Add(new DataColumn("NORAM_DQS", typeof(string)));
            tbl.Columns.Add(new DataColumn("DQS_Operations", typeof(string)));
            //tbl.Columns.Add(new DataColumn("Priority", typeof(string)));
            var status_pipeline = "P-Pipeline,EP-Early Potential,HP-High Potential".Split(',');

            foreach (var r in FinalList)
            {
                string IsImplementationTeamsupport = r.TypeofData == "Pipeline" ? entity.CRMDatas.FirstOrDefault(x => x.Revenue_Id == r.RevenueID)?.IsImplementationTeamsupport : "" ?? "";
                if ((r.Opportunity_Type == "Re-Bid" || r.Opportunity_Type == "Bid Avoidance") && r.TypeofData == "Pipeline" && (IsImplementationTeamsupport == "N" || IsImplementationTeamsupport == "" || IsImplementationTeamsupport == null))
                {
                }
                else
                {
                    DataRow dr = tbl.NewRow();
                    dr["RevenueID"] = (object)r.RevenueID ?? DBNull.Value;
                    dr["Region"] = r.Region == "" || r.Region == null || r.Region == "null" ? "---" : r.Region ?? "---";
                    dr["Country"] = (object)r.Country ?? DBNull.Value;
                    dr["OwnerShip"] = r.OwnerShip == "" || r.OwnerShip == null || r.OwnerShip == "null" ? "---" : r.OwnerShip ?? "---";
                    dr["GoLiveDate"] = r.GoLiveDate == (DateTime?)null ? PipelineDate : (object)r.GoLiveDate ?? DBNull.Value;
                    dr["ProjectStatus"] = (object)(r.ProjectStatus == "PSD-Pipeline" ? "P-Pipeline" : r.ProjectStatus) ?? DBNull.Value;
                    dr["CountryStatus"] = r.CountryStatus == "On Track" ? "Green - On Track" : r.CountryStatus == "Risk" ? "Red - Issue" : r.CountryStatus == "Possible Risk" ? "Amber - Risk" : r.CountryStatus == "" ? "---" : r.CountryStatus ?? "---";
                    dr["ProjectLevel"] = r.ProjectLevel == "" || r.ProjectLevel == null || r.ProjectLevel == "null" ? "---" : r.ProjectLevel ?? "---";
                    dr["CompletedDate"] = r.TypeofData == "Pipeline" || r.TypeofData == "Ad-Hoc Digital Data" ? DBNull.Value : (object)r.CompletedDate ?? DBNull.Value;
                    dr["AssigneeFullName"] = (object)r.AssigneeFullName ?? DBNull.Value;
                    dr["MilestoneTitle"] = r.MilestoneTitle;
                    dr["Milestone__Record_ID_Key"] = r.Milestone__Record_ID_Key;
                    dr["Task__Task_Record_ID_Key"] = r.Task__Task_Record_ID_Key;
                    dr["Group_Name"] = r.Group_Name;
                    dr["Milestone__Project_Notes"] = r.Milestone__Project_Notes;
                    dr["Milestone__Reason_Code"] = r.Milestone__Reason_Code;
                    dr["Milestone__Closed_Loop_Owner"] = r.Milestone__Closed_Loop_Owner;
                    dr["Workspace_Title"] = r.Workspace_Title == "EMEA SMB Implementations" 
                                            || r.Workspace_Title == "NORAM SME Implementations" || r.Workspace_Title == "NORAM BTS Implementations"
                                            || r.Workspace_Title == "Groupement AMF" || r.Workspace_Title == "Coupa/PANA IMP" ? r.MilestoneTitle : r.Workspace_Title;
                    dr["Workspace__ELT_Overall_Status"] = r.Workspace__ELT_Overall_Status == "Possible Risk" ? "Risk - Amber" : r.Workspace__ELT_Overall_Status == "On Track" ? "On Track - Green" : r.Workspace__ELT_Overall_Status == "Risk" ? "Issue - Red" : r.Workspace__ELT_Overall_Status;
                    dr["Workspace__ELT_Overall_Comments"] = r.Workspace__ELT_Overall_Comments;
                    dr["Customer_Row_ID"] = r.Customer_Row_ID;
                    var fourseriesoppID = r.Task__Task_Record_ID_Key.Split('-');
                    dr["Opportunity_ID"] = r.RevenueID == 400000000000000 ? Convert.ToDouble(fourseriesoppID[0])+ 400000000000000 : r.Opportunity_ID;
                    dr["Account_Name"] = r.Account_Name;
                    dr["Sales_Stage_Name"] = r.Sales_Stage_Name;
                    dr["GDS"] = r.GDS;
                    dr["Opportunity_Type"] = r.Opportunity_Type;
                    dr["Revenue_Opportunity_Type"] = r.Revenue_Opportunity_Type;
                    dr["Revenue_Status"] = r.Revenue_Status;
                    dr["Opportunity_Owner"] = r.Opportunity_Owner;
                    dr["Opportunity_Category"] = r.Opportunity_Category;
                    dr["Revenue_Total_Transactions"] = r.Revenue_Total_Transactions;
                    dr["CountryCode"] = (object)r.CountryCode ?? DBNull.Value;
                    dr["Client"] = (object)r.Client ?? DBNull.Value;
                    dr["RevenueVolumeUSD"] = r.RevenueVolumeUSD;
                    //dr["MarketLeader"] = r.MarketLeader;
                    dr["TaskStatus"] = r.ProjectStartTaskStatus;
                    dr["GlobalProjectManager"] = string.Join(", ", r.GlobalProjectManager);
                    //dr["ProjectConsultant"] = string.Join(", ", r.ProjectConsultant);
                    dr["GlobalCISOBTLead"] = string.Join(", ", r.GlobalCISOBTLead);
                    dr["LocalDigitalOBTLead"] = r.RevenueID == 400000000000000 
                                                    ? entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID && x.Workspace_Title == r.Workspace_Title)?.Milestone__Local_Digital_OBT_Lead != null 
                                                        ? entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID && x.Workspace_Title == r.Workspace_Title)?.Milestone__Local_Digital_OBT_Lead 
                                                            : r.LocalCISOBTLead
                                                    : entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID)?.Milestone__Local_Digital_OBT_Lead != null 
                                                        ? entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID)?.Milestone__Local_Digital_OBT_Lead 
                                                            : r.LocalCISOBTLead ?? "";
                     //!= "Deepak Girtola" ? r.LocalCISOBTLead : r.Region == "APAC" && r.Country == "INDIA" ? r.LocalCISOBTLead
                    dr["GlobalCISHRFeedSpecialist"] = r.GlobalCISHRFeedSpecialist;
                    dr["GlobalCISPortraitLead"] = r.GlobalCISPortraitLead;
                    dr["GoLiveMonth"] = r.TypeofData == "Pipeline" ? "Jan" : (object)Convert.ToDateTime(r.GoLiveDate).ToString("MMM") ?? DBNull.Value;
                    dr["GoLiveYear"] = r.TypeofData == "Pipeline" ? "2050" : (object)Convert.ToDateTime(r.GoLiveDate).Year.ToString() ?? DBNull.Value;
                    //dr["BacklogStarted"] = r.TypeofData == "Pipeline" ? "Started" : (object)(r.GoLiveDate > TodayDate ? "Backlog" : "Started") ?? DBNull.Value;
                    dr["Quarter"] = (object)(Convert.ToDateTime(r.GoLiveDate).Month < 4 ? "Qtr 1"
                        : (Convert.ToDateTime(r.GoLiveDate).Month >= 4 && Convert.ToDateTime(r.GoLiveDate).Month < 7) ? "Qtr 2"
                        : (Convert.ToDateTime(r.GoLiveDate).Month >= 7 && Convert.ToDateTime(r.GoLiveDate).Month < 10) ? "Qtr 3"
                        : (Convert.ToDateTime(r.GoLiveDate).Month >= 10 && Convert.ToDateTime(r.GoLiveDate).Month <= 12) ? "Qtr 4"
                        : null) ?? DBNull.Value;
                    dr["YearMonth"] = r.TypeofData == "Pipeline" ? "Jan-2050" : (object)Convert.ToDateTime(r.GoLiveDate).ToString("MMM") + "-" + Convert.ToDateTime(r.GoLiveDate).Year.ToString();
                    dr["CycleTime"] = r.TypeofData == "Pipeline" ?
                                        0 : r.TypeofData == "BelowData" ?
                                        (object)BusinessDaysUntil(r.ProjectStartCTCompleteDate ?? TodayDate, r.GoLiveDate ?? TodayDate) : r.TypeofData == "Ad-Hoc Digital Data" ?
                                        (object)BusinessDaysUntil(r.TaskStartDate ?? TodayDate, r.GoLiveDate ?? TodayDate) : r.ProjectStartTaskStatus == "completed" ?
                                        (object)BusinessDaysUntil(r.ProjectStartCTCompleteDate ?? TodayDate, r.GoLiveDate ?? TodayDate) : r.ProjectStartTaskStatus == "todo" ?
                                        (object)BusinessDaysUntil(r.ProjectStartCTDueDate ?? TodayDate, r.GoLiveDate ?? TodayDate) : 0;
                    dr["ExternalKickoffDuedate"] = (object)(r.TypeofData == "Pipeline" ?
                                                    null : r.TypeofData == "BelowData" ?
                                                        r.TaskStartDate : r.TaskStatus == "completed" ?
                                                        r.TaskCompletedDate : r.TaskStatus == "todo" ?
                                                        r.TaskStartDate : null) ?? DBNull.Value;
                    dr["PerCompleted"] = r.TypeofData == "Pipeline" || r.TypeofData == "Ad-Hoc Digital Data" ? DBNull.Value : (object)r.C__Complete ?? DBNull.Value;
                    dr["MilestoneDueDate"] = r.TypeofData == "Pipeline" || r.TypeofData == "Ad-Hoc Digital Data" ? DBNull.Value : (object)r.Milestone_Due_Date ?? DBNull.Value;
                    dr["Line_Win_Probability"] = r.Line_Win_Probability;
                    dr["Next_Step"] = r.Next_Step;
                    dr["RegionalProjectManager"] = (r.TypeofData == "BelowData" || r.TypeofData == "Pipeline" ?
                                                    r.RegionalProjectManager : r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "FourSerieData" ?
                                                    r.Region == "APAC" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.APAC_Implementation_Lead ?? ""
                                                    : r.Region == "LATAM" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.LATAM_Project_Manager ?? ""
                                                    : r.Region == "NORAM" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.NORAM_Project_Manager ?? ""
                                                    : r.Region == "EMEA" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.EMEA_Project_Manager ?? "" : "" : "");
                    dr["RegionalCISOBTLead"] = (r.TypeofData == "BelowData" || r.TypeofData == "Pipeline" || r.TypeofData == "Ad-Hoc Digital Data" ?
                                                    r.RegionalCISOBTLead : r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "FourSerieData" ?
                                                    entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID)?.Milestone__Override_Regional_Digital_OBT_Lead__Full_Name != null ?
                                                    entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID)?.Milestone__Override_Regional_Digital_OBT_Lead__Full_Name
                                                    : r.Region == "APAC" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.APAC_Digital_OBT_Lead ?? ""
                                                    : r.Region == "LATAM" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.LATAM_CIS_OBT_Lead ?? ""
                                                    : r.Region == "NORAM" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.NORAM_CIS_OBT_Lead ?? ""
                                                    : r.Region == "EMEA" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.EMEA_CIS_OBT_Lead ?? "" : "" : "");
                    dr["RegionalCISPortraitLead"] = (r.TypeofData == "BelowData" || r.TypeofData == "Pipeline" || r.TypeofData == "Ad-Hoc Digital Data" ?
                                                        r.RegionalCISPortraitLead : r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "FourSerieData" ?
                                                        entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID)?.Milestone__Override_Regional_Digital_Portrait_Lead__Full_Name != null ?
                                                        entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID)?.Milestone__Override_Regional_Digital_Portrait_Lead__Full_Name
                                                        : r.Region == "APAC" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.APAC_Digital_Portrait_Lead ?? ""
                                                        : r.Region == "LATAM" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.LATAM_CIS_Portrait_Lead ?? ""
                                                        : r.Region == "NORAM" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.NORAM_CIS_Portrait_Lead ?? ""
                                                        : r.Region == "EMEA" ? entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title)?.EMEA_CIS_Portrait_Lead ?? "" : "" : "");
                    dr["GlobalCISDQSLead"] = r.GlobalCISDQSLead == "" || r.GlobalCISDQSLead == null ? "---" : r.GlobalCISDQSLead;
                    dr["ProjectStart_ForCycleTime"] = (object)(r.TypeofData == "Pipeline" ?
                                                        null : r.TypeofData == "BelowData" ? r.ProjectStartCTCompleteDate
                                                        : r.TypeofData == "Ad-Hoc Digital Data" ? r.TaskStartDate 
                                                        : r.ProjectStartTaskStatus == "completed" ? r.ProjectStartCTCompleteDate
                                                        : r.ProjectStartTaskStatus == "todo" ? r.ProjectStartCTDueDate : null) ?? DBNull.Value;
                    dr["Status"] = (object)(r.Revenue_Status == "NO_SALE" && (r.Status == "" || r.Status == null || r.Status == "Deactivate") ? 
                                            "Deactivate" : (r.Status == "" || r.Status == null) ?
                                            "Active" : r.Status) ?? "Active";
                    dr["ImplementationType"] = (object)(r.ImplementationType == "---" || r.ImplementationType == null || r.ImplementationType == "" ?
                                                        (r.TypeofData == "FourSerieData" ? "---" : r.Opportunity_Type == "" ? "---" : r.Opportunity_Type) : r.ImplementationType) ?? "---";
                    dr["DataSourceType"] = r.TypeofData == "ProductUpSell" ? "PSD-iMeet Data"
                                            : r.TypeofData == "Automated" ? "CRM-iMeet Data"
                                            : r.TypeofData == "Ad-Hoc Digital Data" ? "Ad-Hoc Digital Data"
                                            : r.TypeofData == "Pipeline" && r.ProjectStatus == "P-Pipeline" ? "CRM-Pipeline"
                                            : r.TypeofData == "Pipeline" && r.ProjectStatus == "EP-Early Potential" ? "CRM-EarlyPotential"
                                            : r.TypeofData == "Pipeline" && r.ProjectStatus == "HP-High Potential" ? "CRM-HighPotential"
                                            : r.TypeofData == "FourSerieData" ? "iMeet 4SeriesData"
                                            : r.TypeofData == "BelowData" ? "OldData"
                                            : r.TypeofData == "Pipeline" && r.ProjectStatus == "PSD-Pipeline" ? "PSD-Pipeline" : "";
                    dr["LastUpdateDate"] = r.TypeofData == "Ad-Hoc Digital Data" ? DBNull.Value : (object)r.LastUpdatedDate ?? DBNull.Value;
                    dr["AwardedDate"] = (object)(r.TypeofData == "BelowData" || r.TypeofData == "FourSerieData" || r.TypeofData == "ProductUpSell" || r.TypeofData == "Ad-Hoc Digital Data" || (r.TypeofData == "Pipeline" && r.ProjectStatus == "PSD-Pipeline") ? (DateTime?)null : r.Awarded_Date) ?? DBNull.Value;
                    dr["ClosedDate"] = (object)(r.TypeofData == "BelowData" || r.TypeofData == "FourSerieData" || r.TypeofData == "Ad-Hoc Digital Data" ? (DateTime?)null : r.Close_Date ) ?? DBNull.Value;
                    dr["MilestoneType"] = (object)r.MilestoneType ?? DBNull.Value;
                    dr["DataDescription"] = r.Description;
                    dr["AccountOwner"] = (object)entity.CRMDatas.FirstOrDefault(x => x.Revenue_Id == r.Revenue_Id)?.Account_Owner ?? DBNull.Value;
                    dr["ImplementationFeePsd"] = r.Implementation_Fee;
                    dr["CreatedDate"] = r.TypeofData == "Ad-Hoc Digital Data" ? DBNull.Value : r.TypeofData == "ProductUpSell" ? entity.PSDs.FirstOrDefault(x=> x.Revenue_Id == r.Revenue_Id)?.Created_Date 
                                        : r.TypeofData == "Pipeline" && r.ProjectStatus == "PSD-Pipeline" ? entity.PSDs.FirstOrDefault(x => x.Revenue_Id == r.Revenue_Id)?.Created_Date
                                        : (object)entity.CRMDatas.FirstOrDefault(x => x.Revenue_Id == r.Revenue_Id)?.Created_Date ?? DBNull.Value;
                    dr["CycleTimeCategories"] = r.ImplementationType == null || r.ImplementationType == "" || r.ImplementationType == "---" ? "0"
                                        : imp_listOne.Any(val => r.ImplementationType.Equals(val)) ? "Existing Add/Change OBT"
                                        : imp_listTwo.Any(val1 => r.ImplementationType.Equals(val1)) ? "Existing Service Config Change (catch all including Spins/Mergers)"
                                        : imp_listThree.Any(val2 => r.ImplementationType.Equals(val2)) ? r.ProjectLevel == "Global" ? "New Global Including Upsell"
                                                                                                    : r.ProjectLevel == "Regional" ? "New Global Including Upsell"
                                                                                                    : r.ProjectLevel == "Local" ? "New Local Including Upsell"
                                                                                                    : r.ProjectLevel == "" || r.ProjectLevel == null ? "New Global/regional/local" : "New Global/regional/local"
                                        : "0" ?? "0";
                    dr["DigitalGDS"] = r.RevenueID == 400000000000000 ? (object)entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key)?.GDS ?? "---" 
                        : (object)entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID)?.GDS ?? "---" ?? "---";
                    dr["DigitalActivityType"] = r.RevenueID == 400000000000000 ? (object)entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key)?.ActivityType ?? "---"
                        : (object)entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID)?.ActivityType ?? "---" ?? "---";
                    dr["OBTReseller"] = entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && x.Country_Name == r.CountryCode)?.OBT == null ? "---"
                        : entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && x.Country_Name == r.CountryCode)?.OBT + " " + entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && x.Country_Name == r.CountryCode)?.Direct_Reseller;
                    dr["SOWStatus"] = entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && x.Country_Name == r.CountryCode)?.eSoW_Status ?? "---";
                    dr["ImplementationReady"] = entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && x.Country_Name == r.CountryCode)?.implementationReady ?? "---";
                    dr["AccountCategory"] = entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && x.Country_Name == r.CountryCode)?.Account_Category ?? "---";
                    dr["OppTOtalVolume"] = r.RevenueID == 400000000000000 ? 0 : entity.CRMDatas.FirstOrDefault(x => x.Revenue_Id == r.RevenueID)?.Opportunity_Total_Volume_USD ?? 0;
                    dr["eSowGDS"] = r.TypeofData == "Automated" || r.TypeofData == "Pipeline" ? entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && r.CountryCode == x.Country_Name)?.GDS : r.GDS ?? "---";
                    dr["Service_Location"] = r.TypeofData == "Automated" || r.TypeofData == "Pipeline" ? entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && r.CountryCode == x.Country_Name)?.configLoc : "---";
                    dr["Service_Configuration"] = r.TypeofData == "Automated" || r.TypeofData == "Pipeline" ? entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && r.CountryCode == x.Country_Name)?.configName : "---";
                    dr["LocalDigitalAdHocSupport"] = entity.AssignePersons.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID)?.Milestone__Local_Digital_Ad_Hoc_Support ?? "---";
                    //dr["OBTAdoptionRate"] = r.TypeofData == "Automated" || r.TypeofData == "Pipeline" ? entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && r.CountryCode == x.Country_Name)?.OBT_Adoption_Rate == null ? "---" : (entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && r.CountryCode == x.Country_Name)?.OBT_Adoption_Rate)*100 + "%" : "---";
                    dr["OBTAdoptionRate"] = r.TypeofData == "Automated" || r.TypeofData == "Pipeline" ? entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && r.CountryCode == x.Country_Name)?.OBT_Adoption_Rate == null ? "---" : (entity.esowNewDatas.FirstOrDefault(x => x.crmOpportunityId == r.Opportunity_ID && r.CountryCode == x.Country_Name)?.OBT_Adoption_Rate) : "---";
                    dr["CycleTimeDelayCode"] = r.TypeofData == "Automated" ?
                        entity.DataiMeets.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID && x.Task__Task_Record_ID_Key == r.Task__Task_Record_ID_Key).Milestone__Cycle_Time_Delay_Code == null ? "---" : entity.DataiMeets.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID && x.Task__Task_Record_ID_Key == r.Task__Task_Record_ID_Key).Milestone__Cycle_Time_Delay_Code
                        : r.TypeofData == "FourSerieData" ?
                            entity.DataiMeets.FirstOrDefault(x => x.Task__Task_Record_ID_Key == r.Task__Task_Record_ID_Key).Milestone__Cycle_Time_Delay_Code == null ? "---" : entity.DataiMeets.FirstOrDefault(x => x.Task__Task_Record_ID_Key == r.Task__Task_Record_ID_Key).Milestone__Cycle_Time_Delay_Code
                            : "---" ?? "---";
                    dr["EltClientDelayDescription"] = r.TypeofData == "Automated" ?
                        entity.DataiMeets.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID && x.Task__Task_Record_ID_Key == r.Task__Task_Record_ID_Key).EltClientDelayDescription == null ? "---" : entity.DataiMeets.FirstOrDefault(x => x.Milestone__CRM_Revenue_ID__ == r.RevenueID && x.Task__Task_Record_ID_Key == r.Task__Task_Record_ID_Key).EltClientDelayDescription
                        : r.TypeofData == "FourSerieData" ?
                            entity.DataiMeets.FirstOrDefault(x => x.Task__Task_Record_ID_Key == r.Task__Task_Record_ID_Key).EltClientDelayDescription == null ? "---" : entity.DataiMeets.FirstOrDefault(x => x.Task__Task_Record_ID_Key == r.Task__Task_Record_ID_Key).EltClientDelayDescription
                            : "---" ?? "---";
                    dr["APAC_DQS"] = r.TypeofData == "FourSerieData" || r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "Ad - Hoc Digital Data" ? 
                        entity.Roles.Where(x => x.Workspace_Title == r.Workspace_Title).Count() > 0 ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).APAC_DQS != null ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).APAC_DQS : "---" : "---" : "---";
                    //dr["APAC_DQS"] = r.TypeofData == "FourSerieData" || r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "Ad - Hoc Digital Data" ?
                    //    entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).APAC_DQS != null ?
                    //    entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).APAC_DQS : "---" : "---";
                    dr["DQS_Import"] = r.TypeofData == "FourSerieData" || r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "Ad - Hoc Digital Data" ?
                        entity.Roles.Where(x => x.Workspace_Title == r.Workspace_Title).Count() > 0 ? 
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).DQS_Import != null ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).DQS_Import : "---" : "---" : "---";
                    dr["DQS_Support"] = r.TypeofData == "FourSerieData" || r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "Ad - Hoc Digital Data" ?
                        entity.Roles.Where(x => x.Workspace_Title == r.Workspace_Title).Count() > 0 ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).DQS_Support != null ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).DQS_Support : "---" : "---" : "---";
                    dr["LATAM_DQS"] = r.TypeofData == "FourSerieData" || r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "Ad - Hoc Digital Data" ?
                        entity.Roles.Where(x => x.Workspace_Title == r.Workspace_Title).Count() > 0 ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).LATAM_DQS != null ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).LATAM_DQS : "---" : "---" : "---";
                    dr["NORAM_DQS"] = r.TypeofData == "FourSerieData" || r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "Ad - Hoc Digital Data" ?
                        entity.Roles.Where(x => x.Workspace_Title == r.Workspace_Title).Count() > 0 ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).NORAM_DQS != null ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).NORAM_DQS : "---" : "---" : "---";
                    dr["DQS_Operations"] = r.TypeofData == "FourSerieData" || r.TypeofData == "Automated" || r.TypeofData == "ProductUpSell" || r.TypeofData == "Ad - Hoc Digital Data" ?
                        entity.Roles.Where(x => x.Workspace_Title == r.Workspace_Title).Count() > 0 ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).DQS_Operations != null ?
                        entity.Roles.FirstOrDefault(x => x.Workspace_Title == r.Workspace_Title).DQS_Operations : "---" : "---" : "---";
                    //dr["Priority"] = r.RevenueID >= 600000000000000 ? AdHocProjects.Where(x => x.RevenueID == r.RevenueID).Count() > 0 ? AdHocProjects.FirstOrDefault(x => x.RevenueID == r.RevenueID)?.Priority : "---" :
                    //                 r.RevenueID == 400000000000000 ? ManualDatas.Where(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key).Count() > 0 ? ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key)?.Priority : "---" :
                    //                 r.RevenueID < 600000000000000 && r.RevenueID != 400000000000000 ? ManualDatas.Where(x => x.Revenue_ID == r.RevenueID).Count() > 0 ? ManualDatas.FirstOrDefault(x => x.Revenue_ID == r.RevenueID)?.Priority : "---" : "---" ?? "---";
                    tbl.Rows.Add(dr);
                }
            }
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "CLRData";
            objbulk.ColumnMappings.Add("RevenueID", "RevenueID");
            objbulk.ColumnMappings.Add("Region", "Region");
            objbulk.ColumnMappings.Add("Country", "Country");
            objbulk.ColumnMappings.Add("OwnerShip", "OwnerShip");
            objbulk.ColumnMappings.Add("GoLiveDate", "GoLiveDate");
            objbulk.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
            objbulk.ColumnMappings.Add("CountryStatus", "CountryStatus");
            objbulk.ColumnMappings.Add("ProjectLevel", "ProjectLevel");
            objbulk.ColumnMappings.Add("CompletedDate", "CompletedDate");
            objbulk.ColumnMappings.Add("AssigneeFullName", "AssigneeFullName");
            objbulk.ColumnMappings.Add("PerCompleted", "PerCompleted");
            objbulk.ColumnMappings.Add("MilestoneDueDate", "MilestoneDueDate");
            objbulk.ColumnMappings.Add("Line_Win_Probability", "Line_Win_Probability");
            objbulk.ColumnMappings.Add("Next_Step", "Next_Step");
            objbulk.ColumnMappings.Add("MilestoneTitle", "MilestoneTitle");
            objbulk.ColumnMappings.Add("Milestone__Record_ID_Key", "Milestone__Record_ID_Key");
            objbulk.ColumnMappings.Add("Task__Task_Record_ID_Key", "Task__Task_Record_ID_Key");
            objbulk.ColumnMappings.Add("Group_Name", "Group_Name");
            objbulk.ColumnMappings.Add("TaskStatus", "TaskStatus");
            objbulk.ColumnMappings.Add("Milestone__Project_Notes", "Milestone__Project_Notes");
            objbulk.ColumnMappings.Add("Milestone__Reason_Code", "Milestone__Reason_Code");
            objbulk.ColumnMappings.Add("Milestone__Closed_Loop_Owner", "Milestone__Closed_Loop_Owner");
            objbulk.ColumnMappings.Add("Workspace_Title", "Workspace_Title");
            objbulk.ColumnMappings.Add("Workspace__ELT_Overall_Status", "Workspace__ELT_Overall_Status");
            objbulk.ColumnMappings.Add("Workspace__ELT_Overall_Comments", "Workspace__ELT_Overall_Comments");
            objbulk.ColumnMappings.Add("Customer_Row_ID", "Customer_Row_ID");
            objbulk.ColumnMappings.Add("Opportunity_ID", "Opportunity_ID");
            objbulk.ColumnMappings.Add("Account_Name", "Account_Name");
            objbulk.ColumnMappings.Add("Sales_Stage_Name", "Sales_Stage_Name");
            objbulk.ColumnMappings.Add("Opportunity_Type", "Opportunity_Type");
            objbulk.ColumnMappings.Add("Revenue_Opportunity_Type", "Revenue_Opportunity_Type");
            objbulk.ColumnMappings.Add("Revenue_Status", "Revenue_Status");
            objbulk.ColumnMappings.Add("Opportunity_Owner", "Opportunity_Owner");
            objbulk.ColumnMappings.Add("Opportunity_Category", "Opportunity_Category");
            objbulk.ColumnMappings.Add("Revenue_Total_Transactions", "Revenue_Total_Transactions");
            objbulk.ColumnMappings.Add("CountryCode", "CountryCode");
            objbulk.ColumnMappings.Add("Client", "Client");
            objbulk.ColumnMappings.Add("RevenueVolumeUSD", "RevenueVolumeUSD");
            objbulk.ColumnMappings.Add("GlobalProjectManager", "GlobalProjectManager");
            objbulk.ColumnMappings.Add("GDS", "GDS");
            objbulk.ColumnMappings.Add("GlobalCISOBTLead", "GlobalCISOBTLead");
            objbulk.ColumnMappings.Add("LocalDigitalOBTLead", "LocalDigitalOBTLead"); 
            objbulk.ColumnMappings.Add("GlobalCISHRFeedSpecialist", "GlobalCISHRFeedSpecialist");
            objbulk.ColumnMappings.Add("GlobalCISPortraitLead", "GlobalCISPortraitLead");
            objbulk.ColumnMappings.Add("GoLiveMonth", "GoLiveMonth");
            objbulk.ColumnMappings.Add("GoLiveYear", "GoLiveYear");
            objbulk.ColumnMappings.Add("Quarter", "Quarter");
            objbulk.ColumnMappings.Add("YearMonth", "YearMonth");
            objbulk.ColumnMappings.Add("CycleTime", "CycleTime");
            objbulk.ColumnMappings.Add("ExternalKickoffDuedate", "ExternalKickoffDuedate");
            objbulk.ColumnMappings.Add("RegionalProjectManager", "RegionalProjectManager");
            objbulk.ColumnMappings.Add("RegionalCISOBTLead", "RegionalCISOBTLead");
            objbulk.ColumnMappings.Add("RegionalCISPortraitLead", "RegionalCISPortraitLead");
            objbulk.ColumnMappings.Add("ProjectStart_ForCycleTime", "ProjectStart_ForCycleTime");
            objbulk.ColumnMappings.Add("Status", "Status");
            objbulk.ColumnMappings.Add("ImplementationType", "ImplementationType");
            objbulk.ColumnMappings.Add("DataSourceType", "DataSourceType");
            objbulk.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
            objbulk.ColumnMappings.Add("AwardedDate", "AwardedDate");
            objbulk.ColumnMappings.Add("ClosedDate", "ClosedDate");
            objbulk.ColumnMappings.Add("MilestoneType", "MilestoneType");
            objbulk.ColumnMappings.Add("DataDescription", "DataDescription");
            objbulk.ColumnMappings.Add("AccountOwner", "AccountOwner");
            objbulk.ColumnMappings.Add("CycleTimeCategories", "CycleTimeCategories");
            objbulk.ColumnMappings.Add("ImplementationFeePsd", "ImplementationFeePsd");
            objbulk.ColumnMappings.Add("CreatedDate", "CreatedDate");
            objbulk.ColumnMappings.Add("OBTReseller", "OBTReseller");
            objbulk.ColumnMappings.Add("DigitalGDS", "DigitalGDS");
            objbulk.ColumnMappings.Add("DigitalActivityType", "DigitalActivityType");
            objbulk.ColumnMappings.Add("SOWStatus", "SOWStatus");
            objbulk.ColumnMappings.Add("ImplementationReady", "ImplementationReady");
            objbulk.ColumnMappings.Add("AccountCategory", "AccountCategory");
            objbulk.ColumnMappings.Add("OppTOtalVolume", "OppTOtalVolume");
            objbulk.ColumnMappings.Add("eSowGDS", "eSowGDS");
            objbulk.ColumnMappings.Add("Service_Configuration", "Service_Configuration");
            objbulk.ColumnMappings.Add("LocalDigitalAdHocSupport", "LocalDigitalAdHocSupport");
            objbulk.ColumnMappings.Add("Service_Location", "Service_Location");
            objbulk.ColumnMappings.Add("GlobalCISDQSLead", "GlobalCISDQSLead");
            objbulk.ColumnMappings.Add("OBTAdoptionRate", "OBTAdoptionRate");
            objbulk.ColumnMappings.Add("CycleTimeDelayCode", "CycleTimeDelayCode");
            objbulk.ColumnMappings.Add("EltClientDelayDescription", "EltClientDelayDescription");
            objbulk.ColumnMappings.Add("APAC_DQS", "APAC_DQS");
            objbulk.ColumnMappings.Add("DQS_Import", "DQS_Import");
            objbulk.ColumnMappings.Add("DQS_Support", "DQS_Support");
            objbulk.ColumnMappings.Add("LATAM_DQS", "LATAM_DQS");
            objbulk.ColumnMappings.Add("NORAM_DQS", "NORAM_DQS");
            objbulk.ColumnMappings.Add("DQS_Operations", "DQS_Operations");
            //objbulk.ColumnMappings.Add("Priority", "Priority");
            con.Open();
            string CLR = "Truncate Table CLRData";
            SqlCommand Com = new SqlCommand(CLR, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 1000000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(tbl);
            con.Close();
            
            var AtomaticManualInsert = (from a in entity.CLRDatas
                                        where a.RevenueID != 400000000000000
                                        where !(from b in entity.ManualDatas
                                                where b.Revenue_ID == a.RevenueID
                                                select b.Revenue_ID).Contains(a.RevenueID)
                                        join c in entity.CRMDatas on a.RevenueID equals c.Revenue_Id
                                        select new
                                        {
                                            a.RevenueID,
                                            Account_Name = c.Account_Name ?? "---",
                                            Workspace_Title = "---",
                                            Task__Task_Record_ID_Key = "---",
                                            Opportunity_Type = c.Opportunity_Type ?? "---",
                                            Revenue_Opportunity_Type = c.Revenue_Opportunity_Type ?? "---",
                                            ProjectStatus = a.ProjectStatus == "P-Pipeline" || a.ProjectStatus == "HP-High Potential" || a.ProjectStatus == "EP-Early Potential" ? a.ProjectStatus : "---" ?? "---",
                                            PipelineStatus = "Not actionable",
                                            BT_Current_Service_Configuration = c.BT_Current_Service_Configuration ?? "---",
                                            OBT = c.OBT ?? "---",
                                            status = a.Status ?? "---",
                                            Country = entity.CountryIsoCodes.FirstOrDefault(x => x.CountryName == c.Country).CountryCode ?? "---",
                                            Priority = "---"
                                        }).Distinct().ToList();
            var FourSeriesData = (from a in entity.CLRDatas
                                  where a.RevenueID == 400000000000000
                                  where a.Task__Task_Record_ID_Key != null
                                  where a.Task__Task_Record_ID_Key != ""
                                  where !(from b in entity.ManualDatas
                                          where b.TaskRecordIdKey == a.Task__Task_Record_ID_Key
                                          select b.TaskRecordIdKey).Contains(a.Task__Task_Record_ID_Key)
                                  select new
                                  {
                                      a.RevenueID,
                                      Account_Name = a.Workspace_Title ?? "---",
                                      Workspace_Title = a.Workspace_Title ?? "---",
                                      Task__Task_Record_ID_Key = a.Task__Task_Record_ID_Key ?? "---",
                                      Opportunity_Type = "---",
                                      Revenue_Opportunity_Type = "---",
                                      ProjectStatus = "---",
                                      PipelineStatus = "Not actionable",
                                      BT_Current_Service_Configuration = "---",
                                      OBT = "---",
                                      status = a.Status ?? "---",
                                      Country = a.Country ?? "---",
                                      Priority = "---"
                                  }).Distinct().ToList();
            var ManualInsertData = FourSeriesData.Concat(AtomaticManualInsert);
            DateTime TodaysDate = DateTime.Now;
            DataTable tbl2 = new DataTable();
            tbl2.Columns.Add(new DataColumn("Revenue ID", typeof(double)));
            tbl2.Columns.Add(new DataColumn("TaskRecordIdKey", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Client", typeof(string)));
            tbl2.Columns.Add(new DataColumn("iMeet Workspace Title", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Implementation Type", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Pipeline_status", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Service configuration", typeof(string)));
            tbl2.Columns.Add(new DataColumn("OBT Reseller / Direct", typeof(string)));
            tbl2.Columns.Add(new DataColumn("CLR Country", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Status", typeof(string)));
            tbl2.Columns.Add(new DataColumn("Project Effort", typeof(Int64)));
            tbl2.Columns.Add(new DataColumn("GoLiveDate", typeof(DateTime)));
            tbl2.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));
            tbl2.Columns.Add(new DataColumn("ProjectLevel", typeof(string)));
            tbl2.Columns.Add(new DataColumn("InsertedOn", typeof(DateTime)));
            tbl2.Columns.Add(new DataColumn("Priority", typeof(string)));
            foreach (var r in ManualInsertData)
            {
                DataRow dr2 = tbl2.NewRow();
                dr2["Revenue ID"] = (object)r.RevenueID ?? DBNull.Value;
                dr2["TaskRecordIdKey"] = (object)r.Task__Task_Record_ID_Key ?? "---";
                dr2["Client"] = (object)r.Account_Name ?? "---";
                dr2["iMeet Workspace Title"] = (object)r.Workspace_Title ?? "---";
                dr2["Implementation Type"] = (object)r.Opportunity_Type ?? "---";
                dr2["Pipeline_status"] = (object)r.PipelineStatus ?? "---";
                dr2["Service configuration"] = (object)r.BT_Current_Service_Configuration ?? "---";
                dr2["OBT Reseller / Direct"] = (object)r.OBT ?? "---";
                dr2["CLR Country"] = string.Join(", ", r.Country);
                dr2["Status"] = r.status ?? "---";
                dr2["Project Effort"] = 1;
                dr2["GoLiveDate"] = (object)(r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "HP-High Potential" || r.ProjectStatus == "EP-Early Potential" ? PipelineDate : (DateTime?)null) ?? DBNull.Value;
                dr2["InsertedOn"] = TodaysDate;
                dr2["ProjectStatus"] = (object)(r.ProjectStatus) ?? "---";
                dr2["ProjectLevel"] = (object)(r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "HP-High Potential" || r.ProjectStatus == "EP-Early Potential" ? 
                    entity.CLRDatas.Where(x => x.Client == r.Account_Name && status_pipeline.Any(val => x.ProjectStatus.Equals(val))).Select(x => x.Country).Distinct().Count() <= 1 ? "Local"
                    : entity.CLRDatas.Where(x => x.Client == r.Account_Name && status_pipeline.Any(val => x.ProjectStatus.Equals(val))).Select(x => x.Region).Distinct().Count() > 1 ? "Global"
                    : "Regional" 
                    : "---") ?? "---";
                dr2["Priority"] = (object)r.Priority;
                tbl2.Rows.Add(dr2);
            }
            SqlBulkCopy objbulk2 = new SqlBulkCopy(con);
            objbulk2.DestinationTableName = "ManualData";
            objbulk2.ColumnMappings.Add("Revenue ID", "Revenue ID");
            objbulk2.ColumnMappings.Add("TaskRecordIdKey", "TaskRecordIdKey");
            objbulk2.ColumnMappings.Add("Client", "Client");
            objbulk2.ColumnMappings.Add("iMeet Workspace Title", "iMeet Workspace Title");
            objbulk2.ColumnMappings.Add("Implementation Type", "Implementation Type");
            objbulk2.ColumnMappings.Add("Pipeline_status", "Pipeline_status");
            objbulk2.ColumnMappings.Add("Service configuration", "Service configuration");
            objbulk2.ColumnMappings.Add("OBT Reseller / Direct", "OBT Reseller / Direct");
            objbulk2.ColumnMappings.Add("CLR Country", "CLR Country");
            objbulk2.ColumnMappings.Add("Status", "Status");
            objbulk2.ColumnMappings.Add("Project Effort", "Project Effort");
            objbulk2.ColumnMappings.Add("GoLiveDate", "GoLiveDate");
            objbulk2.ColumnMappings.Add("InsertedOn", "InsertedOn");
            objbulk2.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
            objbulk2.ColumnMappings.Add("ProjectLevel", "ProjectLevel");
            objbulk2.ColumnMappings.Add("Priority", "Priority");
            con.Open();
            objbulk2.BatchSize = 100000;
            objbulk2.BulkCopyTimeout = 0;
            objbulk2.WriteToServer(tbl2);
            con.Close();
            //var AtomaticManualInsertPSD = (from a in entity.CLRDatas
            //                               where a.RevenueID != 400000000000000
            //                               where !(from b in entity.ManualDatas
            //                                       where b.Revenue_ID == a.RevenueID
            //                                       select b.Revenue_ID).Contains(a.RevenueID)
            //                               join c in entity.PSDs on a.RevenueID equals c.Revenue_Id
            //                               select new
            //                               {
            //                                   a.RevenueID,
            //                                   c.Account_Name,
            //                                   a.Task__Task_Record_ID_Key,
            //                                   c.Opportunity_Type, 
            //                                   ProjectStatus = a.ProjectStatus == "P-Pipeline" ? "P-Pipeline" : "---",
            //                                   PipelineStatus = "Not actionable",
            //                                   c.BT_Current_Service_Configuration,
            //                                   c.Product_Name,
            //                                   Country = entity.CountryIsoCodes.FirstOrDefault(x => x.CountryName == c.Country).CountryCode,
            //                                }).Distinct().ToList();
            //DataTable tbl3 = new DataTable();
            //tbl3.Columns.Add(new DataColumn("Revenue ID", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("TaskRecordIdKey", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("Client", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("iMeet Workspace Title", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("Implementation Type", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("Pipeline_status", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("Service configuration", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("OBT Reseller / Direct", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("CLR Country", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("Status", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("Project Effort", typeof(Int64)));
            //tbl3.Columns.Add(new DataColumn("InsertedOn", typeof(DateTime)));
            //tbl3.Columns.Add(new DataColumn("GoLiveDate", typeof(DateTime)));
            //tbl3.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));
            //foreach (var r in AtomaticManualInsertPSD)
            //{
            //    DataRow dr3 = tbl3.NewRow();
            //    dr3["Revenue ID"] = (object)r.RevenueID ?? DBNull.Value;
            //    dr3["TaskRecordIdKey"] = (object)r.Task__Task_Record_ID_Key ?? "---";
            //    dr3["Client"] = (object)r.Account_Name ?? "---";
            //    dr3["iMeet Workspace Title"] = (object)entity.CLRDatas.FirstOrDefault(x => x.RevenueID == r.RevenueID).Workspace_Title ?? "---";
            //    dr3["Implementation Type"] = (object)r.Opportunity_Type == "" ? "---" : r.Opportunity_Type ?? "---";
            //    dr3["Pipeline_status"] = (object)r.PipelineStatus ?? "---";
            //    dr3["Service configuration"] = (object)r.BT_Current_Service_Configuration ?? "---";
            //    dr3["OBT Reseller / Direct"] = (object)r.Product_Name == "" ? "---" : r.Product_Name ?? "---";
            //    dr3["CLR Country"] = string.Join(", ", r.Country);
            //    dr3["Status"] = "Active";
            //    dr3["Project Effort"] = 1;
            //    dr3["InsertedOn"] = TodaysDate;
            //    dr3["GoLiveDate"] = (object)(r.ProjectStatus == "P-Pipeline" ? PipelineDate : (DateTime?)null) ?? DBNull.Value;
            //    dr3["ProjectStatus"] = (object)(r.ProjectStatus) ?? "---";
            //    tbl3.Rows.Add(dr3);
            //}
            //SqlBulkCopy objbulk3 = new SqlBulkCopy(con);
            //objbulk3.DestinationTableName = "ManualData";
            //objbulk3.ColumnMappings.Add("Revenue ID", "Revenue ID");
            //objbulk3.ColumnMappings.Add("TaskRecordIdKey", "TaskRecordIdKey");
            //objbulk3.ColumnMappings.Add("Client", "Client");
            //objbulk3.ColumnMappings.Add("iMeet Workspace Title", "iMeet Workspace Title");
            //objbulk3.ColumnMappings.Add("Implementation Type", "Implementation Type");
            //objbulk3.ColumnMappings.Add("Pipeline_status", "Pipeline_status");
            //objbulk3.ColumnMappings.Add("Service configuration", "Service configuration");
            //objbulk3.ColumnMappings.Add("OBT Reseller / Direct", "OBT Reseller / Direct");
            //objbulk3.ColumnMappings.Add("CLR Country", "CLR Country");
            //objbulk3.ColumnMappings.Add("Status", "Status");
            //objbulk3.ColumnMappings.Add("Project Effort", "Project Effort");
            //objbulk3.ColumnMappings.Add("InsertedOn", "InsertedOn");
            //objbulk3.ColumnMappings.Add("GoLiveDate", "GoLiveDate");
            //objbulk3.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
            //con.Open();
            //objbulk3.BatchSize = 100000;
            //objbulk3.BulkCopyTimeout = 0;
            //objbulk3.WriteToServer(tbl3);
            //con.Close();
            
            var DigitalTeam = (from a in entity.CLRDatas
                               where a.RevenueID != 400000000000000
                               where !(from b in entity.DigitalTeams
                                       where b.RevenueID == a.RevenueID
                                       select b.RevenueID).Contains(a.RevenueID)
                               select new
                               {
                                   a.RevenueID,
                                   Client = a.Client ?? "",
                                   Task__Task_Record_ID_Key = "",
                                   GlobalCISOBTLead = a.GlobalCISOBTLead ?? "",
                                   RegionalCISOBTLead = a.RegionalCISOBTLead ?? "",
                                   LocalDigitalOBTLead = a.LocalDigitalOBTLead ?? "",
                                   GlobalCISPortraitLead = a.GlobalCISPortraitLead ?? "",
                                   RegionalCISPortraitLead = a.RegionalCISPortraitLead ?? "",
                                   GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist ?? "",
                                   GDS = a.GDS ?? "---",
                               }).Distinct().ToList();
            var DigitalTeamFourseries = (from a in entity.CLRDatas
                                         where a.RevenueID == 400000000000000
                                         where a.Task__Task_Record_ID_Key != null
                                         where a.Task__Task_Record_ID_Key != ""
                                         where !(from b in entity.DigitalTeams
                                                 where b.TaskRecordIdKey == a.Task__Task_Record_ID_Key
                                                 select b.TaskRecordIdKey).Contains(a.Task__Task_Record_ID_Key)
                                         select new
                                         {
                                             a.RevenueID,
                                             Client = a.Client ?? "",
                                             Task__Task_Record_ID_Key = a.Task__Task_Record_ID_Key ?? "",
                                             GlobalCISOBTLead = a.GlobalCISOBTLead ?? "",
                                             RegionalCISOBTLead = a.RegionalCISOBTLead ?? "",
                                             LocalDigitalOBTLead = a.LocalDigitalOBTLead ?? "",
                                             GlobalCISPortraitLead = a.GlobalCISPortraitLead ?? "",
                                             RegionalCISPortraitLead = a.RegionalCISPortraitLead ?? "",
                                             GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist ?? "",
                                             GDS = a.GDS ?? "---",
                                         }).Distinct().ToList();
            var DigitalInsertData = DigitalTeamFourseries.Concat(DigitalTeam);
            DataTable tbl4 = new DataTable();
            tbl4.Columns.Add(new DataColumn("RevenueID", typeof(double)));
            tbl4.Columns.Add(new DataColumn("Client", typeof(string)));
            tbl4.Columns.Add(new DataColumn("TaskRecordIdKey", typeof(string)));
            tbl4.Columns.Add(new DataColumn("GlobalCISOBTLead", typeof(string)));
            tbl4.Columns.Add(new DataColumn("RegionalCISOBTLead", typeof(string)));
            tbl4.Columns.Add(new DataColumn("LocalDigitalOBTLead", typeof(string)));
            tbl4.Columns.Add(new DataColumn("GlobalCISPortraitLead", typeof(string)));
            tbl4.Columns.Add(new DataColumn("RegionalCISPortraitLead", typeof(string)));
            tbl4.Columns.Add(new DataColumn("GlobalCISHRFeedSpecialist", typeof(string)));
            tbl4.Columns.Add(new DataColumn("GDS", typeof(string)));
            tbl4.Columns.Add(new DataColumn("ActivityType", typeof(string)));
            tbl4.Columns.Add(new DataColumn("ComplexityScore", typeof(Int64)));
            tbl4.Columns.Add(new DataColumn("InsertedOn", typeof(DateTime)));

            foreach (var r in DigitalInsertData)
            {
                DataRow dr4 = tbl4.NewRow();
                dr4["RevenueID"] = (object)r.RevenueID ?? DBNull.Value;
                dr4["Client"] = (object)r.Client;
                dr4["TaskRecordIdKey"] = (object)r.Task__Task_Record_ID_Key;
                dr4["GlobalCISOBTLead"] = (object)r.GlobalCISOBTLead ?? "---";
                dr4["RegionalCISOBTLead"] = (object)r.RegionalCISOBTLead ?? "---";
                dr4["LocalDigitalOBTLead"] = (object)r.LocalDigitalOBTLead ?? "---";
                dr4["GlobalCISPortraitLead"] = (object)r.GlobalCISPortraitLead ?? "---";
                dr4["RegionalCISPortraitLead"] = (object)r.RegionalCISPortraitLead ?? "---";
                dr4["GlobalCISHRFeedSpecialist"] = (object)r.GlobalCISHRFeedSpecialist ?? "---";
                dr4["ActivityType"] = "---";
                dr4["GDS"] = (object)r.GDS == "Amadeus" || r.GDS == "Sabre" ? r.GDS 
                    : r.GDS == "Abacus" || r.GDS == "Abacus,Sabre" ? "Sabre" 
                    : r.GDS == "Apollo" || r.GDS == "Travelport-A,Infini" || r.GDS == "Travelport-A" || r.GDS == "Travelport-G" || r.GDS == "Galileo" ? "TravelPort" : "---" ?? "---";
                dr4["ComplexityScore"] = 0;
                dr4["InsertedOn"] = TodaysDate;
                tbl4.Rows.Add(dr4);
            }
            SqlBulkCopy objbulk4 = new SqlBulkCopy(con);
            objbulk4.DestinationTableName = "DigitalTeam";
            objbulk4.ColumnMappings.Add("RevenueID", "RevenueID");
            objbulk4.ColumnMappings.Add("Client", "Client");
            objbulk4.ColumnMappings.Add("TaskRecordIdKey", "TaskRecordIdKey");
            objbulk4.ColumnMappings.Add("GlobalCISOBTLead", "GlobalCISOBTLead");
            objbulk4.ColumnMappings.Add("RegionalCISOBTLead", "RegionalCISOBTLead");
            objbulk4.ColumnMappings.Add("LocalDigitalOBTLead", "LocalDigitalOBTLead");
            objbulk4.ColumnMappings.Add("GlobalCISPortraitLead", "GlobalCISPortraitLead");
            objbulk4.ColumnMappings.Add("RegionalCISPortraitLead", "RegionalCISPortraitLead");
            objbulk4.ColumnMappings.Add("GlobalCISHRFeedSpecialist", "GlobalCISHRFeedSpecialist");
            objbulk4.ColumnMappings.Add("GDS", "GDS");
            objbulk4.ColumnMappings.Add("ActivityType", "ActivityType");
            objbulk4.ColumnMappings.Add("ComplexityScore", "ComplexityScore");
            objbulk4.ColumnMappings.Add("InsertedOn", "InsertedOn");
            con.Open();
            objbulk4.BatchSize = 100000;
            objbulk4.BulkCopyTimeout = 0;
            objbulk4.WriteToServer(tbl4);
            con.Close();
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "CLRAutomated",
                    UpdatedOn = TodaysDate,
                });
            entity.SaveChanges();
            //var data = (from a in entity.CLRDatas
            //            where a.RevenueID != 400000000000000
            //            select a).ToList();
            //foreach (var r in data)
            //{
            //    var yes_data = (from b in entity.YesterdayCLRs
            //                    where b.RevenueID == r.RevenueID
            //                    where b.TaskRecordIdKey == r.Task__Task_Record_ID_Key
            //                    select b).ToList();
            //    if (yes_data.Count > 0)
            //    {
            //        if (yes_data[0].ProjectStatus != r.ProjectStatus)
            //        {
            //            DateTime Todays_Date = DateTime.Now;
            //            entity.CLRActivities.Add(new CLRActivity
            //            {
            //                Client = r.Client,
            //                Revenue_ID = r.RevenueID,
            //                OldValue = yes_data[0].ProjectStatus,
            //                NewValue = r.ProjectStatus,
            //                ColumnName = "ProjectStatus",
            //                UpdatedDate = Todays_Date,
            //                TaskRecordIDKey = r.Task__Task_Record_ID_Key
            //            });
            //            entity.SaveChanges();
            //        }
            //        if (yes_data[0].Sales_Stage_Name != r.Sales_Stage_Name)
            //        {
            //            DateTime Todays_Date = DateTime.Now;
            //            entity.CLRActivities.Add(new CLRActivity
            //            {
            //                Client = r.Client,
            //                Revenue_ID = r.RevenueID,
            //                OldValue = yes_data[0].Sales_Stage_Name,
            //                NewValue = r.Sales_Stage_Name,
            //                ColumnName = "Sales_Stage_Name",
            //                UpdatedDate = Todays_Date,
            //                TaskRecordIDKey = r.Task__Task_Record_ID_Key
            //            });
            //            entity.SaveChanges();
            //        }
            //    }
            //}
            ViewBag.Error4 = "Success";
            return View("Index");
        }
        public ActionResult CLRActivity()
        {
            ManualDataController manualDataController = new ManualDataController();
            manualDataController.CLRActivityGenerating();
            ViewBag.Error22 = "Success";
            return View("Index");
        }
        public ActionResult GenerateTracker()
        {
            var CurrentYear = DateTime.Now.Year;
            var ProjectStatus = "A-Active/Date Confirmed,HP-High Potential,P-Pipeline,N-Active/No Date Confirmed".Split(',');
            //var ProjectStatus = "P-Pipeline".Split(',');
            DateTime firstDay = new DateTime(CurrentYear, 1, 1);
            var Tracker = (from a in entity.CLRDatas
                           where a.OwnerShip != "Minority Holding"
                           where a.OwnerShip != "Not Present"
                           where a.GoLiveDate >= firstDay
                           where a.Status == "Active"
                           //where a.RevenueID == 300000452472986
                           where ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                           where a.MilestoneType != "POS"
                           //where a.OwnerShip == "WO"
                           //where a.ProjectStart_ForCycleTime == null
                           //where a.Region == "EMEA"
                           //where a.ProjectLevel == "Local"
                           //where a.Sales_Stage_Name == "Contract Signed"
                           //where a.Service_Configuration == "Local"
                           //where a.RevenueID == 300000400035208
                           //where a.RevenueID == 300000401881785
                           select a).ToList();
            DataTable tbl3 = new DataTable();
            tbl3.Columns.Add(new DataColumn("RevenueID", typeof(double)));
            tbl3.Columns.Add(new DataColumn("Region", typeof(string)));
            tbl3.Columns.Add(new DataColumn("Country", typeof(string)));
            tbl3.Columns.Add(new DataColumn("GlobalProjectManager", typeof(string)));
            tbl3.Columns.Add(new DataColumn("RegionalProjectManager", typeof(string)));
            tbl3.Columns.Add(new DataColumn("LocalProjectManager", typeof(string)));
            tbl3.Columns.Add(new DataColumn("Client", typeof(string)));
            tbl3.Columns.Add(new DataColumn("iMeetWorkspaceTitle", typeof(string)));
            tbl3.Columns.Add(new DataColumn("OwnershipType", typeof(string)));
            tbl3.Columns.Add(new DataColumn("PriorityCustomer", typeof(string)));
            tbl3.Columns.Add(new DataColumn("Volume", typeof(double)));
            tbl3.Columns.Add(new DataColumn("ProjectLevel", typeof(string)));
            tbl3.Columns.Add(new DataColumn("ImplementationType", typeof(string)));
            tbl3.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));
            tbl3.Columns.Add(new DataColumn("GlobalDigitalOBTLead", typeof(string)));
            tbl3.Columns.Add(new DataColumn("RegionalDigitalOBTLead", typeof(string)));
            tbl3.Columns.Add(new DataColumn("LocalDigitalOBTLead", typeof(string)));
            tbl3.Columns.Add(new DataColumn("GlobalDigitalPortraitLead", typeof(string)));
            tbl3.Columns.Add(new DataColumn("RegionalDigitalPortraitLead", typeof(string)));
            tbl3.Columns.Add(new DataColumn("GlobalDigitalHRFeedSpeciallist", typeof(string)));
            tbl3.Columns.Add(new DataColumn("GDS", typeof(string)));
            tbl3.Columns.Add(new DataColumn("ComplexityScore", typeof(Int64)));
            tbl3.Columns.Add(new DataColumn("MilestoneProjectNotes", typeof(string)));
            tbl3.Columns.Add(new DataColumn("ProjectEffort", typeof(double)));
            tbl3.Columns.Add(new DataColumn("KickOff_ProposedStartDate", typeof(DateTime)));
            tbl3.Columns.Add(new DataColumn("GoLiveDate", typeof(DateTime)));
            tbl3.Columns.Add(new DataColumn("EndofHypercare", typeof(DateTime)));
            tbl3.Columns.Add(new DataColumn("CompleteDuration", typeof(string)));
            tbl3.Columns.Add(new DataColumn("PerCompleted", typeof(double)));
            tbl3.Columns.Add(new DataColumn("AssignmentDate", typeof(DateTime)));
            tbl3.Columns.Add(new DataColumn("ProjectStartDate", typeof(DateTime)));
            tbl3.Columns.Add(new DataColumn("MilestoneDueDate", typeof(DateTime)));
            tbl3.Columns.Add(new DataColumn("MilestoneDueDateByLevel", typeof(DateTime)));
            tbl3.Columns.Add(new DataColumn("ProjectDelay", typeof(double)));
            tbl3.Columns.Add(new DataColumn("TaskRecordIdKey", typeof(string)));
            tbl3.Columns.Add(new DataColumn("1stweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("2ndweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("3rdweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("4thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("5thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("6thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("7thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("8thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("9thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("10thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("11thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("12thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c13thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c14thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c15thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c16thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c17thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c18thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c19thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c20thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c21thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c22thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c23thweek", typeof(double)));
            tbl3.Columns.Add(new DataColumn("c24thweek", typeof(double)));
            DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
            DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay).Date;
            //string testing = currentWeekStartDate.Date.ToString();
            DateTime? EndofHypercare, KickOff_ProposedStartDate,Assignment_Date,KickOff_ProposedStart;
            foreach (var r in Tracker)
            {
                DataRow dr3 = tbl3.NewRow();
                dr3["RevenueID"] = (object)r.RevenueID ?? DBNull.Value;
                EndofHypercare = r.MilestoneDueDate != null ? r.MilestoneDueDate
                                    : r.GoLiveDate == null ? (DateTime?)null
                                    : r.Region == "APAC" && r.Country == "AUSTRALIA" ? r.GoLiveDate.Value.AddDays(14) : r.GoLiveDate.Value.AddDays(28);
                //string checkone = EndofHypercare.ToString();
                KickOff_ProposedStartDate = r.ProjectStart_ForCycleTime != null ? r.ProjectStart_ForCycleTime
                                    : EndofHypercare != null ? entity.Configs.FirstOrDefault(x => x.ProjectType == r.ImplementationType).Duration != null ? EndofHypercare.Value.AddDays(-(entity.Configs.FirstOrDefault(x => x.ProjectType == r.ImplementationType).Duration * 7))
                                    : (DateTime?)null : (DateTime?)null;
                //string checktwo = KickOff_ProposedStartDate.ToString();
                Assignment_Date = r.RevenueID == 400000000000000 
                                    ? entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key).Assignment_date != null 
                                        ? entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key).Assignment_date :
                                        (DateTime?)null
                                    : entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == r.RevenueID).Assignment_date != null
                                        ? entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == r.RevenueID).Assignment_date :
                                        (DateTime?)null ?? (DateTime?)null;
                //KickOff_ProposedStartDate = (object)r.ProjectLevel == "Local" ? KickOff_ProposedStart
                //            : r.ProjectLevel == "Regional" ? Assignment_Date == null ? KickOff_ProposedStart : Assignment_Date
                //            : r.ProjectLevel == "Global" ? Assignment_Date == null ? KickOff_ProposedStart : Assignment_Date : KickOff_ProposedStart;
                dr3["Region"] = (object)r.Region ?? "";
                dr3["Country"] = (object)r.Country ?? "";
                dr3["GlobalProjectManager"] = (object)r.GlobalProjectManager ?? "";
                dr3["RegionalProjectManager"] = (object)r.RegionalProjectManager ?? "";
                dr3["LocalProjectManager"] = (object)r.AssigneeFullName ?? "";
                dr3["Client"] = (object)r.Client ?? "";
                dr3["iMeetWorkspaceTitle"] = (object)r.Workspace_Title ?? "";
                dr3["OwnershipType"] = (object)r.OwnerShip ?? "";
                dr3["PriorityCustomer"] = "";
                dr3["Volume"] = (object)r.RevenueVolumeUSD ?? 0;
                dr3["ProjectLevel"] = (object)r.ProjectLevel ?? "";
                dr3["ImplementationType"] = (object)r.ImplementationType ?? "";
                dr3["ProjectStatus"] = (object)r.ProjectStatus ?? "";
                dr3["GlobalDigitalOBTLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).GlobalCISOBTLead : r.GlobalCISOBTLead ?? "";
                dr3["RegionalDigitalOBTLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).RegionalCISOBTLead : r.RegionalCISOBTLead ?? "";
                dr3["LocalDigitalOBTLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).LocalDigitalOBTLead 
                                            : r.LocalDigitalOBTLead != "Deepak Girtola" ? r.LocalDigitalOBTLead : r.Region == "APAC" && r.Country == "INDIA" ? r.LocalDigitalOBTLead : "" ?? "";
                dr3["GlobalDigitalPortraitLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).GlobalCISPortraitLead : r.GlobalCISPortraitLead ?? "";
                dr3["RegionalDigitalPortraitLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).RegionalCISPortraitLead : r.RegionalCISPortraitLead ?? "";
                dr3["GlobalDigitalHRFeedSpeciallist"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).GlobalCISHRFeedSpecialist : r.GlobalCISHRFeedSpecialist ?? "";
                dr3["GDS"] = r.RevenueID == 400000000000000 ? entity.CLRDatas.FirstOrDefault(x => x.Task__Task_Record_ID_Key == r.Task__Task_Record_ID_Key).GDS : entity.CLRDatas.FirstOrDefault(x => x.RevenueID == r.RevenueID).GDS ?? "";
                dr3["ComplexityScore"] = r.RevenueID == 400000000000000 ? entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key).ComplexityScore : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).ComplexityScore;
                dr3["MilestoneProjectNotes"] = (object)r.Milestone__Project_Notes ?? "";
                dr3["ProjectEffort"] = r.RevenueID == 400000000000000 ? entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key).Project_Effort
                    : entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == r.RevenueID).Project_Effort ?? 1;
                dr3["AssignmentDate"] = (object)Assignment_Date ?? DBNull.Value;
                dr3["KickOff_ProposedStartDate"] = (object)KickOff_ProposedStartDate ?? DBNull.Value;
                dr3["GoLiveDate"] = (object)r.GoLiveDate ?? DBNull.Value;
                dr3["EndofHypercare"] = (object) EndofHypercare ?? DBNull.Value;
                dr3["CompleteDuration"] = EndofHypercare == null || KickOff_ProposedStartDate == null ? "0 Weeks"
                    : (EndofHypercare - KickOff_ProposedStartDate).Value.TotalDays / 7 + "Weeks";
                dr3["PerCompleted"] = (object)r.PerCompleted ?? DBNull.Value;
                dr3["ProjectStartDate"] = (object)r.ProjectStart_ForCycleTime ?? DBNull.Value;
                dr3["MilestoneDueDate"] = (object)r.MilestoneDueDate ?? DBNull.Value;
                dr3["MilestoneDueDateByLevel"] = (object)r.ProjectLevel == "Local" ? KickOff_ProposedStartDate 
                            : r.ProjectLevel == "Regional" ? Assignment_Date != null ? Assignment_Date : KickOff_ProposedStartDate 
                            : r.ProjectLevel == "Global" ? Assignment_Date != null ? Assignment_Date : KickOff_ProposedStartDate : KickOff_ProposedStartDate == null ? (DateTime?)null : KickOff_ProposedStartDate;
                dr3["ProjectDelay"] = EndofHypercare == null || KickOff_ProposedStartDate == null ? 0 : (EndofHypercare - KickOff_ProposedStartDate).Value.TotalDays / 7;
                dr3["TaskRecordIdKey"] = (object)r.Task__Task_Record_ID_Key == null ? "" : r.Task__Task_Record_ID_Key;
                dr3["1stweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate >= KickOff_ProposedStartDate && currentWeekStartDate <= EndofHypercare ? 1 : 0;
                dr3["2ndweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7) <= EndofHypercare ? 1 : 0;
                dr3["3rdweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 2) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 2) <= EndofHypercare ? 1 : 0;
                dr3["4thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 3) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 3) <= EndofHypercare ? 1 : 0;
                dr3["5thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 4) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 4) <= EndofHypercare ? 1 : 0;
                dr3["6thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 5) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 5) <= EndofHypercare ? 1 : 0;
                dr3["7thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 6) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 6) <= EndofHypercare ? 1 : 0;
                dr3["8thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 7) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 7) <= EndofHypercare ? 1 : 0;
                dr3["9thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 8) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 8) <= EndofHypercare ? 1 : 0;
                dr3["10thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 9) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 9) <= EndofHypercare ? 1 : 0;
                dr3["11thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 10) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 10) <= EndofHypercare ? 1 : 0;
                dr3["12thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 11) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 11) <= EndofHypercare ? 1 : 0;
                dr3["c13thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 12) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 12) <= EndofHypercare ? 1 : 0;
                dr3["c14thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 13) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 13) <= EndofHypercare ? 1 : 0;
                dr3["c15thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 14) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 14) <= EndofHypercare ? 1 : 0;
                dr3["c16thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 15) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 15) <= EndofHypercare ? 1 : 0;
                dr3["c17thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 16) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 16) <= EndofHypercare ? 1 : 0;
                dr3["c18thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 17) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 17) <= EndofHypercare ? 1 : 0;
                dr3["c19thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 18) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 18) <= EndofHypercare ? 1 : 0;
                dr3["c20thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 19) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 19) <= EndofHypercare ? 1 : 0;
                dr3["c21thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 20) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 20) <= EndofHypercare ? 1 : 0;
                dr3["c22thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 21) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 21) <= EndofHypercare ? 1 : 0;
                dr3["c23thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 22) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 22) <= EndofHypercare ? 1 : 0;
                dr3["c24thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 23) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 23) <= EndofHypercare ? 1 : 0;
                tbl3.Rows.Add(dr3);
            }
            SqlBulkCopy objbulk3 = new SqlBulkCopy(con);
            objbulk3.DestinationTableName = "CLRTracker";
            objbulk3.ColumnMappings.Add("RevenueID", "RevenueID");
            objbulk3.ColumnMappings.Add("Region", "Region");
            objbulk3.ColumnMappings.Add("Country", "Country");
            objbulk3.ColumnMappings.Add("GlobalProjectManager", "GlobalProjectManager");
            objbulk3.ColumnMappings.Add("RegionalProjectManager", "RegionalProjectManager");
            objbulk3.ColumnMappings.Add("LocalProjectManager", "LocalProjectManager");
            objbulk3.ColumnMappings.Add("Client", "Client");
            objbulk3.ColumnMappings.Add("iMeetWorkspaceTitle", "iMeetWorkspaceTitle");
            objbulk3.ColumnMappings.Add("OwnershipType", "OwnershipType");
            objbulk3.ColumnMappings.Add("PriorityCustomer", "PriorityCustomer");
            objbulk3.ColumnMappings.Add("Volume", "Volume");
            objbulk3.ColumnMappings.Add("ProjectLevel", "ProjectLevel");
            objbulk3.ColumnMappings.Add("ImplementationType", "ImplementationType");
            objbulk3.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
            objbulk3.ColumnMappings.Add("GlobalDigitalOBTLead", "GlobalDigitalOBTLead");
            objbulk3.ColumnMappings.Add("RegionalDigitalOBTLead", "RegionalDigitalOBTLead");
            objbulk3.ColumnMappings.Add("LocalDigitalOBTLead", "LocalDigitalOBTLead");
            objbulk3.ColumnMappings.Add("GlobalDigitalPortraitLead", "GlobalDigitalPortraitLead");
            objbulk3.ColumnMappings.Add("RegionalDigitalPortraitLead", "RegionalDigitalPortraitLead");
            objbulk3.ColumnMappings.Add("GlobalDigitalHRFeedSpeciallist", "GlobalDigitalHRFeedSpeciallist");
            objbulk3.ColumnMappings.Add("GDS", "GDS");
            objbulk3.ColumnMappings.Add("ComplexityScore", "ComplexityScore");
            objbulk3.ColumnMappings.Add("MilestoneProjectNotes", "MilestoneProjectNotes");
            objbulk3.ColumnMappings.Add("ProjectEffort", "ProjectEffort");
            objbulk3.ColumnMappings.Add("KickOff_ProposedStartDate", "KickOff_ProposedStartDate");
            objbulk3.ColumnMappings.Add("GoLiveDate", "GoLiveDate");
            objbulk3.ColumnMappings.Add("EndofHypercare", "EndofHypercare");
            objbulk3.ColumnMappings.Add("CompleteDuration", "CompleteDuration");
            objbulk3.ColumnMappings.Add("PerCompleted", "PerCompleted");
            objbulk3.ColumnMappings.Add("AssignmentDate", "AssignmentDate");
            objbulk3.ColumnMappings.Add("ProjectStartDate", "ProjectStartDate");
            objbulk3.ColumnMappings.Add("MilestoneDueDate", "MilestoneDueDate");
            objbulk3.ColumnMappings.Add("MilestoneDueDateByLevel", "MilestoneDueDateByLevel");
            objbulk3.ColumnMappings.Add("ProjectDelay", "ProjectDelay");
            objbulk3.ColumnMappings.Add("TaskRecordIdKey", "TaskRecordIdKey");
            objbulk3.ColumnMappings.Add("1stweek", "1stweek");
            objbulk3.ColumnMappings.Add("2ndweek", "2ndweek");
            objbulk3.ColumnMappings.Add("3rdweek", "3rdweek");
            objbulk3.ColumnMappings.Add("4thweek", "4thweek");
            objbulk3.ColumnMappings.Add("5thweek", "5thweek");
            objbulk3.ColumnMappings.Add("6thweek", "6thweek");
            objbulk3.ColumnMappings.Add("7thweek", "7thweek");
            objbulk3.ColumnMappings.Add("8thweek", "8thweek");
            objbulk3.ColumnMappings.Add("9thweek", "9thweek");
            objbulk3.ColumnMappings.Add("10thweek", "10thweek");
            objbulk3.ColumnMappings.Add("11thweek", "11thweek");
            objbulk3.ColumnMappings.Add("12thweek", "12thweek");
            objbulk3.ColumnMappings.Add("c13thweek", "c13thweek");
            objbulk3.ColumnMappings.Add("c14thweek", "c14thweek");
            objbulk3.ColumnMappings.Add("c15thweek", "c15thweek");
            objbulk3.ColumnMappings.Add("c16thweek", "c16thweek");
            objbulk3.ColumnMappings.Add("c17thweek", "c17thweek");
            objbulk3.ColumnMappings.Add("c18thweek", "c18thweek");
            objbulk3.ColumnMappings.Add("c19thweek", "c19thweek");
            objbulk3.ColumnMappings.Add("c20thweek", "c20thweek");
            objbulk3.ColumnMappings.Add("c21thweek", "c21thweek");
            objbulk3.ColumnMappings.Add("c22thweek", "c22thweek");
            objbulk3.ColumnMappings.Add("c23thweek", "c23thweek");
            objbulk3.ColumnMappings.Add("c24thweek", "c24thweek");
            con.Open();
            string s2 = "Truncate Table CLRTracker";
            SqlCommand Com2 = new SqlCommand(s2, con);
            Com2.ExecuteNonQuery();
            objbulk3.BatchSize = 100000;
            objbulk3.BulkCopyTimeout = 0;
            objbulk3.WriteToServer(tbl3);
            con.Close();
            ViewBag.Error15 = "Success";
            return View("Index");
        }


        //public ActionResult GenerateResourceUtilization()
        //{
        //    var CurrentYear = DateTime.Now.Year;
        //    var ProjectStatus = "A-Active/Date Confirmed,HP-High Potential,P-Pipeline,N-Active/No Date Confirmed".Split(',');
        //    //var ProjectStatus = "A-Active/Date Confirmed".Split(',');
        //    DateTime firstDay = new DateTime(CurrentYear, 1, 1);
        //    DayOfWeek currentDay = DateTime.Now.DayOfWeek;
        //    int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
        //    DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
        //    var Tracker = (from a in entity.CapacityHierarchies
        //                   where a.ManagerStatus == "Active"
        //                   select a).ToList();
        //    DataTable tbl3 = new DataTable();
        //    tbl3.Columns.Add(new DataColumn("HierarchyID", typeof(double)));
        //    tbl3.Columns.Add(new DataColumn("Region", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("Level", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("ProjectLevel", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("Leader", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("Manager", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("WorkShedule", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("WorkingDays", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("Monday", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("Tuesday", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("Wednesday", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("Thursday", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("Friday", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("Comments", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("TargetedUtilization", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C1stweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C2ndweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C3rdweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C4thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C5thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C6thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C7thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C8thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("AvgUtil", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("TUWorkingDays", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("CapacityAvailable", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("CapacityAvailableTUWorkingDays", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C9thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C10thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C11thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("C12thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c13thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c14thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c15thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c16thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c17thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c18thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c19thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c20thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c21thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c22thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c23thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("c24thweek", typeof(string)));
        //    tbl3.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));

        //    var Role = "PM 1,PM 2,PM 3".Split(',');
        //    foreach (var ps in ProjectStatus)
        //    {
        //        foreach (var r in Tracker)
        //        {
        //            if (r.Level != "Digital")
        //            {
        //                DataRow dr3 = tbl3.NewRow();
        //                dr3["HierarchyID"] = (object)r.HID ?? "";
        //                dr3["Region"] = (object)r.Region ?? "";
        //                dr3["Level"] = (object)r.Level ?? "";
        //                dr3["ProjectLevel"] = (object)r.PLevel ?? "";
        //                dr3["Leader"] = (object)r.Leader ?? "";
        //                dr3["Manager"] = (object)r.Manager ?? "";
        //                dr3["WorkShedule"] = (object)r.WorkShedule ?? "";
        //                dr3["WorkingDays"] = (object)r.WorkingDays ?? "";
        //                dr3["Monday"] = (object)r.Monday ?? "";
        //                dr3["Tuesday"] = (object)r.Tuesday ?? "";
        //                dr3["Wednesday"] = (object)r.Wednesday ?? "";
        //                dr3["Thursday"] = (object)r.Thursday ?? "";
        //                dr3["Friday"] = (object)r.Friday ?? "";
        //                dr3["Comments"] = (object)r.Comments ?? "";
        //                dr3["TargetedUtilization"] = (object)r.TargetedUtilization ?? "";
        //                dr3["ProjectStatus"] = ps;
        //                dr3["C1stweek"] = (entity.CLRTrackers.Where(v1 => v1.ProjectStatus == ps && v1.ProjectLevel == "Local" && v1.LocalProjectManager == r.Manager && currentWeekStartDate >= v1.KickOff_ProposedStartDate && currentWeekStartDate <= v1.EndofHypercare).Sum(x => x.ProjectEffort) ?? 0) +
        //                                  (entity.CLRTrackers.Where(v1 => v1.ProjectStatus == ps && v1.ProjectLevel == "Local" && v1.RegionalProjectManager == r.Manager && currentWeekStartDate >= v1.KickOff_ProposedStartDate && currentWeekStartDate <= v1.EndofHypercare).Sum(x => x.ProjectEffort) ?? 0) +
        //                                  (entity.CLRTrackers.Where(v1 => v1.ProjectStatus == ps && v1.ProjectLevel == "Local" && v1.GlobalProjectManager == r.Manager && currentWeekStartDate >= v1.KickOff_ProposedStartDate && currentWeekStartDate <= v1.EndofHypercare).Sum(x => x.ProjectEffort) ?? 0) +
        //                                  (entity.CLRTrackers.Where(v1 => v1.ProjectStatus == ps && v1.ProjectLevel == "Regional" && v1.LocalProjectManager == r.Manager && currentWeekStartDate >= v1.KickOff_ProposedStartDate && currentWeekStartDate <= v1.EndofHypercare).Sum(x => x.ProjectEffort) ?? 0) +
        //                                  (entity.CLRTrackers.Where(v1 => v1.ProjectStatus == ps && v1.ProjectLevel == "Regional" && v1.RegionalProjectManager == r.Manager && currentWeekStartDate >= v1.MilestoneDueDateByLevel && currentWeekStartDate <= v1.EndofHypercare).Sum(x => x.ProjectEffort) ?? 0) +
        //                                  (entity.CLRTrackers.Where(v1 => v1.ProjectStatus == ps && v1.ProjectLevel == "Regional" && v1.GlobalProjectManager == r.Manager && currentWeekStartDate >= v1.KickOff_ProposedStartDate && currentWeekStartDate <= v1.EndofHypercare).Sum(x => x.ProjectEffort) ?? 0) +
        //                                  (entity.CLRTrackers.Where(v1 => v1.ProjectStatus == ps && v1.ProjectLevel == "Global" && v1.LocalProjectManager == r.Manager && currentWeekStartDate >= v1.KickOff_ProposedStartDate && currentWeekStartDate <= v1.EndofHypercare).Sum(x => x.ProjectEffort) ?? 0) +
        //                                  (entity.CLRTrackers.Where(v1 => v1.ProjectStatus == ps && v1.ProjectLevel == "Global" && v1.RegionalProjectManager == r.Manager && currentWeekStartDate >= v1.KickOff_ProposedStartDate && currentWeekStartDate <= v1.EndofHypercare).Sum(x => x.ProjectEffort) ?? 0) +
        //                                  (entity.CLRTrackers.Where(v1 => v1.ProjectStatus == ps && v1.ProjectLevel == "Global" && v1.GlobalProjectManager == r.Manager && currentWeekStartDate >= v1.MilestoneDueDateByLevel && currentWeekStartDate <= v1.EndofHypercare).Sum(x => x.ProjectEffort) ?? 0);
        //                dr3["C2ndweek"] = "";
        //                dr3["C3rdweek"] = "";
        //                dr3["C4thweek"] = "";
        //                dr3["C5thweek"] = "";
        //                dr3["C6thweek"] = "";
        //                dr3["C7thweek"] = "";
        //                dr3["C8thweek"] = "";
        //                dr3["AvgUtil"] = "";
        //                dr3["TUWorkingDays"] = "";
        //                dr3["CapacityAvailable"] = "";
        //                dr3["CapacityAvailableTUWorkingDays"] = "";
        //                dr3["C9thweek"] = "";
        //                dr3["C10thweek"] = "";
        //                dr3["C11thweek"] = "";
        //                dr3["C12thweek"] = "";
        //                dr3["C13thweek"] = "";
        //                dr3["C14thweek"] = "";
        //                dr3["C15thweek"] = "";
        //                dr3["C16thweek"] = "";
        //                dr3["C17thweek"] = "";
        //                dr3["C18thweek"] = "";
        //                dr3["C19thweek"] = "";
        //                dr3["C20thweek"] = "";
        //                dr3["C21stweek"] = "";
        //                dr3["C22ndweek"] = "";
        //                dr3["C23rdweek"] = "";
        //                dr3["C24thweek"] = "";
        //                tbl3.Rows.Add(dr3);
        //            }
        //        }
        //    }
        //    SqlBulkCopy objbulk3 = new SqlBulkCopy(con);
        //    objbulk3.DestinationTableName = "ResourceUtilization";
        //    objbulk3.ColumnMappings.Add("HierarchyID", "HierarchyID");
        //    objbulk3.ColumnMappings.Add("Region", "Region");
        //    objbulk3.ColumnMappings.Add("Level", "Level");
        //    objbulk3.ColumnMappings.Add("ProjectLevel", "ProjectLevel");
        //    objbulk3.ColumnMappings.Add("Leader", "Leader");
        //    objbulk3.ColumnMappings.Add("Manager", "Manager");
        //    objbulk3.ColumnMappings.Add("WorkShedule", "WorkShedule");
        //    objbulk3.ColumnMappings.Add("WorkingDays", "WorkingDays");
        //    objbulk3.ColumnMappings.Add("Monday", "Monday");
        //    objbulk3.ColumnMappings.Add("Tuesday", "Tuesday");
        //    objbulk3.ColumnMappings.Add("Wednesday", "Wednesday");
        //    objbulk3.ColumnMappings.Add("Thursday", "Thursday");
        //    objbulk3.ColumnMappings.Add("Friday", "Friday");
        //    objbulk3.ColumnMappings.Add("Comments", "Comments");
        //    objbulk3.ColumnMappings.Add("TargetedUtilization", "TargetedUtilization");
        //    objbulk3.ColumnMappings.Add("C1stweek", "C1stweek");
        //    objbulk3.ColumnMappings.Add("C2ndweek", "C2ndweek");
        //    objbulk3.ColumnMappings.Add("C3rdweek", "C3rdweek");
        //    objbulk3.ColumnMappings.Add("C4thweek", "C4thweek");
        //    objbulk3.ColumnMappings.Add("C5thweek", "C5thweek");
        //    objbulk3.ColumnMappings.Add("C6thweek", "C6thweek");
        //    objbulk3.ColumnMappings.Add("C7thweek", "C7thweek");
        //    objbulk3.ColumnMappings.Add("C8thweek", "C8thweek");
        //    objbulk3.ColumnMappings.Add("AvgUtil", "AvgUtil");
        //    objbulk3.ColumnMappings.Add("TUWorkingDays", "TUWorkingDays");
        //    objbulk3.ColumnMappings.Add("CapacityAvailable", "CapacityAvailable");
        //    objbulk3.ColumnMappings.Add("CapacityAvailableTUWorkingDays", "CapacityAvailableTUWorkingDays");
        //    objbulk3.ColumnMappings.Add("C9thweek", "C9thweek");
        //    objbulk3.ColumnMappings.Add("C10thweek", "C10thweek");
        //    objbulk3.ColumnMappings.Add("C11thweek", "C11thweek");
        //    objbulk3.ColumnMappings.Add("C12thweek", "C12thweek");
        //    objbulk3.ColumnMappings.Add("c13thweek", "c13thweek");
        //    objbulk3.ColumnMappings.Add("c14thweek", "c14thweek");
        //    objbulk3.ColumnMappings.Add("c15thweek", "c15thweek");
        //    objbulk3.ColumnMappings.Add("c16thweek", "c16thweek");
        //    objbulk3.ColumnMappings.Add("c17thweek", "c17thweek");
        //    objbulk3.ColumnMappings.Add("c18thweek", "c18thweek");
        //    objbulk3.ColumnMappings.Add("c19thweek", "c19thweek");
        //    objbulk3.ColumnMappings.Add("c20thweek", "c20thweek");
        //    objbulk3.ColumnMappings.Add("c21thweek", "c21thweek");
        //    objbulk3.ColumnMappings.Add("c22thweek", "c22thweek");
        //    objbulk3.ColumnMappings.Add("c23thweek", "c23thweek");
        //    objbulk3.ColumnMappings.Add("c24thweek", "c24thweek");
        //    objbulk3.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
        //    ViewBag.Error15 = "Success";
        //    return View("Index");
        //}

        public void ChangesNoticedAndStoringInDB()
        {
            var data = (from a in entity.CLRDatas
                        where a.RevenueID != 400000000000000
                        select a).ToList();
            foreach (var r in data)
            {
                var yes_data = (from b in entity.YesterdayCLRs
                                where b.RevenueID == r.RevenueID
                                where b.TaskRecordIdKey == r.Task__Task_Record_ID_Key
                                select b).ToList();
                if (yes_data.Count > 0)
                {
                    if (yes_data[0].ProjectStatus != r.ProjectStatus)
                    {
                        DateTime TodayDate = DateTime.Now;
                        entity.CLRActivities.Add(new CLRActivity
                        {
                            Client = r.Client,
                            Revenue_ID = r.RevenueID,
                            OldValue = yes_data[0].ProjectStatus,
                            NewValue = r.ProjectStatus,
                            ColumnName = "ProjectStatus",
                            UpdatedDate = TodayDate,
                            TaskRecordIDKey = r.Task__Task_Record_ID_Key
                        });
                        entity.SaveChanges();
                    }
                }
            }
        }
        public void StoreYesterdayCLRData()
        {
            var data = (from a in entity.CLRDatas
                        select a).ToList();
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("Client", typeof(string)));
            tbl.Columns.Add(new DataColumn("RevenueID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Region", typeof(string)));
            tbl.Columns.Add(new DataColumn("Country", typeof(string)));
            tbl.Columns.Add(new DataColumn("RevenueVolumeUSD", typeof(double)));
            tbl.Columns.Add(new DataColumn("OppTOtalVolume", typeof(double)));
            tbl.Columns.Add(new DataColumn("GoLiveDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("ProjectLevel", typeof(string)));
            tbl.Columns.Add(new DataColumn("ProjectStart_ForCycleTime", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("GoLiveMonth", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveYear", typeof(string)));
            tbl.Columns.Add(new DataColumn("Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("ImplementationType", typeof(string)));
            tbl.Columns.Add(new DataColumn("DataSourceType", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalProjectManager", typeof(string)));
            tbl.Columns.Add(new DataColumn("RegionalProjectManager", typeof(string)));
            tbl.Columns.Add(new DataColumn("AssigneeFullName", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISOBTLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISHRFeedSpecialist", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISPortraitLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("RegionalCISOBTLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("RegionalCISPortraitLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("TaskRecordIdKey", typeof(string)));
            tbl.Columns.Add(new DataColumn("Sales_Stage_Name", typeof(string)));
            foreach (var r in data)
            {
                DataRow dr = tbl.NewRow();
                dr["Client"] = (object)r.Client ?? DBNull.Value;
                dr["RevenueID"] = (object)r.RevenueID ?? DBNull.Value;
                dr["Region"] = (object)r.Region ?? DBNull.Value;
                dr["Country"] = (object)r.Country ?? DBNull.Value;
                dr["RevenueVolumeUSD"] = (object)r.RevenueVolumeUSD ?? DBNull.Value;
                dr["OppTOtalVolume"] = (object)r.OppTOtalVolume ?? DBNull.Value;
                dr["GoLiveDate"] = (object)r.GoLiveDate ?? DBNull.Value;
                dr["ProjectStatus"] = (object)r.ProjectStatus ?? DBNull.Value;
                dr["ProjectLevel"] = (object)r.ProjectLevel ?? DBNull.Value;
                dr["ProjectStart_ForCycleTime"] = (object)r.ProjectStart_ForCycleTime ?? DBNull.Value;
                dr["GoLiveMonth"] = (object)r.GoLiveMonth ?? DBNull.Value;
                dr["GoLiveYear"] = (object)r.GoLiveYear ?? DBNull.Value;
                dr["Status"] = (object)r.Status ?? DBNull.Value;
                dr["ImplementationType"] = (object)r.ImplementationType ?? DBNull.Value;
                dr["DataSourceType"] = (object)r.DataSourceType ?? DBNull.Value;
                dr["GlobalProjectManager"] = (object)r.GlobalProjectManager ?? DBNull.Value;
                dr["RegionalProjectManager"] = (object)r.RegionalProjectManager ?? DBNull.Value;
                dr["AssigneeFullName"] = (object)r.AssigneeFullName ?? DBNull.Value;
                dr["GlobalCISOBTLead"] = (object)r.GlobalCISOBTLead ?? DBNull.Value;
                dr["GlobalCISHRFeedSpecialist"] = (object)r.GlobalCISHRFeedSpecialist ?? DBNull.Value;
                dr["GlobalCISPortraitLead"] = (object)r.GlobalCISPortraitLead ?? DBNull.Value;
                dr["RegionalCISOBTLead"] = (object)r.RegionalCISOBTLead ?? DBNull.Value;
                dr["RegionalCISPortraitLead"] = (object)r.RegionalCISPortraitLead ?? DBNull.Value;
                dr["TaskRecordIdKey"] = (object)r.Task__Task_Record_ID_Key ?? DBNull.Value;
                dr["Sales_Stage_Name"] = (object)r.Sales_Stage_Name ?? DBNull.Value;
                tbl.Rows.Add(dr);
            }
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "YesterdayCLR";
            objbulk.ColumnMappings.Add("Client", "Client");
            objbulk.ColumnMappings.Add("RevenueID", "RevenueID");
            objbulk.ColumnMappings.Add("Region", "Region");
            objbulk.ColumnMappings.Add("Country", "Country");
            objbulk.ColumnMappings.Add("RevenueVolumeUSD", "RevenueVolumeUSD");
            objbulk.ColumnMappings.Add("OppTOtalVolume", "OppTOtalVolume");
            objbulk.ColumnMappings.Add("GoLiveDate", "GoLiveDate");
            objbulk.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
            objbulk.ColumnMappings.Add("ProjectLevel", "ProjectLevel");
            objbulk.ColumnMappings.Add("ProjectStart_ForCycleTime", "ProjectStart_ForCycleTime");
            objbulk.ColumnMappings.Add("GoLiveMonth", "GoLiveMonth");
            objbulk.ColumnMappings.Add("GoLiveYear", "GoLiveYear");
            objbulk.ColumnMappings.Add("Status", "Status");
            objbulk.ColumnMappings.Add("ImplementationType", "ImplementationType");
            objbulk.ColumnMappings.Add("DataSourceType", "DataSourceType");
            objbulk.ColumnMappings.Add("GlobalProjectManager", "GlobalProjectManager");
            objbulk.ColumnMappings.Add("RegionalProjectManager", "RegionalProjectManager");
            objbulk.ColumnMappings.Add("AssigneeFullName", "AssigneeFullName");
            objbulk.ColumnMappings.Add("GlobalCISOBTLead", "GlobalCISOBTLead");
            objbulk.ColumnMappings.Add("GlobalCISHRFeedSpecialist", "GlobalCISHRFeedSpecialist");
            objbulk.ColumnMappings.Add("GlobalCISPortraitLead", "GlobalCISPortraitLead");
            objbulk.ColumnMappings.Add("RegionalCISOBTLead", "RegionalCISOBTLead");
            objbulk.ColumnMappings.Add("RegionalCISPortraitLead", "RegionalCISPortraitLead");
            objbulk.ColumnMappings.Add("TaskRecordIdKey", "TaskRecordIdKey");
            objbulk.ColumnMappings.Add("Sales_Stage_Name", "Sales_Stage_Name");
            con.Open();
            string CLR = "Truncate Table YesterdayCLR";
            SqlCommand Com = new SqlCommand(CLR, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(tbl);
            con.Close();
        }

        public void StoreOldData()
        {
            var data = (from a in entity.CLRDatas
                        select a).ToList();
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("RevenueID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Region", typeof(string)));
            tbl.Columns.Add(new DataColumn("Country", typeof(string)));
            tbl.Columns.Add(new DataColumn("OwnerShip", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("CountryStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("ProjectLevel", typeof(string)));
            //tbl.Columns.Add(new DataColumn("ProjectStartDate", typeof(DateTime)));
            //tbl.Columns.Add(new DataColumn("IntitialGoliveDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("CompletedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("AssigneeFullName", typeof(string)));
            tbl.Columns.Add(new DataColumn("MilestoneTitle", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Record_ID_Key", typeof(string)));
            tbl.Columns.Add(new DataColumn("Task__Task_Record_ID_Key", typeof(string)));
            tbl.Columns.Add(new DataColumn("Group_Name", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Project_Notes", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Reason_Code", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Closed_Loop_Owner", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace_Title", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace__ELT_Overall_Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace__ELT_Overall_Comments", typeof(string)));
            tbl.Columns.Add(new DataColumn("Customer_Row_ID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Opportunity_ID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Account_Name", typeof(string)));
            tbl.Columns.Add(new DataColumn("Sales_Stage_Name", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Type", typeof(string)));
            tbl.Columns.Add(new DataColumn("Revenue_Opportunity_Type", typeof(string)));
            tbl.Columns.Add(new DataColumn("Revenue_Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Owner", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Category", typeof(string)));
            tbl.Columns.Add(new DataColumn("Revenue_Total_Transactions", typeof(float)));
            tbl.Columns.Add(new DataColumn("CountryCode", typeof(string)));
            tbl.Columns.Add(new DataColumn("Client", typeof(string)));
            tbl.Columns.Add(new DataColumn("RevenueVolumeUSD", typeof(double)));
            tbl.Columns.Add(new DataColumn("GlobalProjectManager", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISOBTLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISHRFeedSpecialist", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISPortraitLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveMonth", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveYear", typeof(string)));
            tbl.Columns.Add(new DataColumn("Quarter", typeof(string)));
            tbl.Columns.Add(new DataColumn("CycleTime", typeof(float)));
            tbl.Columns.Add(new DataColumn("PerCompleted", typeof(float)));
            tbl.Columns.Add(new DataColumn("MilestoneDueDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Line_Win_Probability", typeof(float)));
            tbl.Columns.Add(new DataColumn("Next_Step", typeof(string)));
            tbl.Columns.Add(new DataColumn("ExternalKickoffDuedate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("RegionalProjectManager", typeof(string)));
            tbl.Columns.Add(new DataColumn("RegionalCISOBTLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("RegionalCISPortraitLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("ProjectStart_ForCycleTime", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("ImplementationType", typeof(string)));
            tbl.Columns.Add(new DataColumn("DataSourceType", typeof(string)));
            tbl.Columns.Add(new DataColumn("LastUpdateDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("AwardedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ClosedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("MilestoneType", typeof(string)));
            tbl.Columns.Add(new DataColumn("DataDescription", typeof(string)));
            tbl.Columns.Add(new DataColumn("AccountOwner", typeof(string)));
            tbl.Columns.Add(new DataColumn("ImplementationFeePsd", typeof(double)));
            tbl.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
            foreach (var r in data)
            {
                DataRow dr = tbl.NewRow();
                dr["RevenueID"] = (object)r.RevenueID ?? DBNull.Value;
                dr["Region"] = (object)r.Region ?? DBNull.Value;
                dr["Country"] = (object)r.Country ?? DBNull.Value;
                dr["OwnerShip"] = (object)r.OwnerShip ?? DBNull.Value;
                dr["GoLiveDate"] = (object)r.GoLiveDate ?? DBNull.Value;
                dr["ProjectStatus"] = (object)r.ProjectStatus ?? DBNull.Value;
                dr["CountryStatus"] = r.CountryStatus;
                dr["ProjectLevel"] = r.ProjectLevel;
                //dr["ProjectStartDate"] = (object)r.ProjectStartDate ?? DBNull.Value;
                //dr["IntitialGoliveDate"] = (object)r.IntitialGoliveDate ?? DBNull.Value;
                dr["CompletedDate"] = (object)r.CompletedDate ?? DBNull.Value;
                dr["AssigneeFullName"] = (object)r.AssigneeFullName ?? DBNull.Value;
                dr["MilestoneTitle"] = r.MilestoneTitle;
                dr["Milestone__Record_ID_Key"] = r.Milestone__Record_ID_Key;
                dr["Task__Task_Record_ID_Key"] = r.Task__Task_Record_ID_Key;
                dr["Group_Name"] = r.Group_Name;
                dr["Milestone__Project_Notes"] = r.Milestone__Project_Notes;
                dr["Milestone__Reason_Code"] = r.Milestone__Reason_Code;
                dr["Milestone__Closed_Loop_Owner"] = r.Milestone__Closed_Loop_Owner;
                dr["Workspace_Title"] = r.Workspace_Title;
                dr["Workspace__ELT_Overall_Status"] = r.Workspace__ELT_Overall_Status;
                dr["Workspace__ELT_Overall_Comments"] = r.Workspace__ELT_Overall_Comments;
                dr["Customer_Row_ID"] = (object)(r.Customer_Row_ID) ?? DBNull.Value;
                dr["Opportunity_ID"] = (object)(r.Opportunity_ID) ?? DBNull.Value;
                dr["Account_Name"] = r.Account_Name;
                dr["Sales_Stage_Name"] = r.Sales_Stage_Name;
                dr["Opportunity_Type"] = r.Opportunity_Type;
                dr["Revenue_Opportunity_Type"] = r.Revenue_Opportunity_Type;
                dr["Revenue_Status"] = r.Revenue_Status;
                dr["Opportunity_Owner"] = r.Opportunity_Owner;
                dr["Opportunity_Category"] = r.Opportunity_Category;
                dr["Revenue_Total_Transactions"] = r.Revenue_Total_Transactions;
                dr["CountryCode"] = (object)r.CountryCode ?? DBNull.Value;
                dr["Client"] = (object)r.Client ?? DBNull.Value;
                dr["RevenueVolumeUSD"] = r.RevenueVolumeUSD ?? 0;
                dr["GlobalProjectManager"] = r.GlobalProjectManager;
                dr["GlobalCISOBTLead"] = r.GlobalCISOBTLead;
                dr["GlobalCISHRFeedSpecialist"] = r.GlobalCISHRFeedSpecialist;
                dr["GlobalCISPortraitLead"] = r.GlobalCISPortraitLead;
                dr["GoLiveMonth"] = (object)r.GoLiveMonth ?? DBNull.Value;
                dr["GoLiveYear"] = (object)r.GoLiveYear ?? DBNull.Value;
                dr["Quarter"] = (object)r.Quarter ?? DBNull.Value;
                //dr["CycleTime"] = (object)BusinessDaysUntil(r.GoLiveDate ?? TodayDate, r.TaskDueDate ?? TodayDate) ?? DBNull.Value;
                dr["CycleTime"] = (object)r.CycleTime;
                dr["ExternalKickoffDuedate"] = (object)r.ExternalKickoffDuedate ?? DBNull.Value;
                dr["PerCompleted"] = (object)r.PerCompleted ?? DBNull.Value;
                dr["MilestoneDueDate"] = (object)r.MilestoneDueDate ?? DBNull.Value;
                dr["Line_Win_Probability"] = (object)r.Line_Win_Probability ?? DBNull.Value;
                dr["Next_Step"] = (object)r.Next_Step;
                dr["RegionalProjectManager"] = r.RegionalProjectManager;
                dr["RegionalCISOBTLead"] = r.RegionalCISOBTLead;
                dr["RegionalCISPortraitLead"] = r.RegionalCISPortraitLead;
                dr["ProjectStart_ForCycleTime"] = (object)r.ProjectStart_ForCycleTime ?? DBNull.Value;
                dr["Status"] = r.Status;
                dr["ImplementationType"] = r.ImplementationType;
                dr["DataSourceType"] = r.DataSourceType;
                dr["LastUpdateDate"] = (object)r.LastUpdateDate ?? DBNull.Value;
                dr["AwardedDate"] = (object)r.AwardedDate ?? DBNull.Value;
                dr["ClosedDate"] = (object)r.ClosedDate ?? DBNull.Value;
                dr["MilestoneType"] = (object)r.MilestoneType ?? DBNull.Value;
                dr["DataDescription"] = r.DataDescription;
                dr["AccountOwner"] = (object)r.AccountOwner ?? DBNull.Value;
                dr["ImplementationFeePsd"] = r.ImplementationFeePsd;
                dr["CreatedDate"] = (object)r.CreatedDate ?? DBNull.Value;
                //if (r.TypeofData == "Pipeline")
                //{
                //    if (r.ProjectLevel == "Local")
                //    {
                //        if (crm_countries.Any(val => r.Country.Equals(val)))
                //        {
                //        }
                //        else
                //        {
                //            tbl.Rows.Add(dr);
                //        }
                //    }
                //    else
                //    {
                //        tbl.Rows.Add(dr);
                //    }
                //}
                //else
                //{
                tbl.Rows.Add(dr);
                //}
            }
            //ViewBag.Error4 = tbl.Rows;
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "OldCLRData";
            objbulk.ColumnMappings.Add("RevenueID", "RevenueID");
            objbulk.ColumnMappings.Add("Region", "Region");
            objbulk.ColumnMappings.Add("Country", "Country");
            objbulk.ColumnMappings.Add("OwnerShip", "OwnerShip");
            objbulk.ColumnMappings.Add("GoLiveDate", "GoLiveDate");
            objbulk.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
            objbulk.ColumnMappings.Add("CountryStatus", "CountryStatus");
            objbulk.ColumnMappings.Add("ProjectLevel", "ProjectLevel");
            //objbulk.ColumnMappings.Add("ProjectStartDate", "ProjectStartDate");
            //objbulk.ColumnMappings.Add("IntitialGoliveDate", "IntitialGoliveDate");
            objbulk.ColumnMappings.Add("CompletedDate", "CompletedDate");
            objbulk.ColumnMappings.Add("AssigneeFullName", "AssigneeFullName");
            objbulk.ColumnMappings.Add("PerCompleted", "PerCompleted");
            objbulk.ColumnMappings.Add("MilestoneDueDate", "MilestoneDueDate");
            objbulk.ColumnMappings.Add("Line_Win_Probability", "Line_Win_Probability");
            objbulk.ColumnMappings.Add("Next_Step", "Next_Step");
            objbulk.ColumnMappings.Add("MilestoneTitle", "MilestoneTitle");
            objbulk.ColumnMappings.Add("Milestone__Record_ID_Key", "Milestone__Record_ID_Key");
            objbulk.ColumnMappings.Add("Task__Task_Record_ID_Key", "Task__Task_Record_ID_Key");
            objbulk.ColumnMappings.Add("Group_Name", "Group_Name");
            objbulk.ColumnMappings.Add("Milestone__Project_Notes", "Milestone__Project_Notes");
            objbulk.ColumnMappings.Add("Milestone__Reason_Code", "Milestone__Reason_Code");
            objbulk.ColumnMappings.Add("Milestone__Closed_Loop_Owner", "Milestone__Closed_Loop_Owner");
            objbulk.ColumnMappings.Add("Workspace_Title", "Workspace_Title");
            objbulk.ColumnMappings.Add("Workspace__ELT_Overall_Status", "Workspace__ELT_Overall_Status");
            objbulk.ColumnMappings.Add("Workspace__ELT_Overall_Comments", "Workspace__ELT_Overall_Comments");
            objbulk.ColumnMappings.Add("Customer_Row_ID", "Customer_Row_ID");
            objbulk.ColumnMappings.Add("Opportunity_ID", "Opportunity_ID");
            objbulk.ColumnMappings.Add("Account_Name", "Account_Name");
            objbulk.ColumnMappings.Add("Sales_Stage_Name", "Sales_Stage_Name");
            objbulk.ColumnMappings.Add("Opportunity_Type", "Opportunity_Type");
            objbulk.ColumnMappings.Add("Revenue_Opportunity_Type", "Revenue_Opportunity_Type");
            objbulk.ColumnMappings.Add("Revenue_Status", "Revenue_Status");
            objbulk.ColumnMappings.Add("Opportunity_Owner", "Opportunity_Owner");
            objbulk.ColumnMappings.Add("Opportunity_Category", "Opportunity_Category");
            objbulk.ColumnMappings.Add("Revenue_Total_Transactions", "Revenue_Total_Transactions");
            objbulk.ColumnMappings.Add("CountryCode", "CountryCode");
            objbulk.ColumnMappings.Add("Client", "Client");
            objbulk.ColumnMappings.Add("RevenueVolumeUSD", "RevenueVolumeUSD");
            objbulk.ColumnMappings.Add("GlobalProjectManager", "GlobalProjectManager");
            objbulk.ColumnMappings.Add("GlobalCISOBTLead", "GlobalCISOBTLead");
            objbulk.ColumnMappings.Add("GlobalCISHRFeedSpecialist", "GlobalCISHRFeedSpecialist");
            objbulk.ColumnMappings.Add("GlobalCISPortraitLead", "GlobalCISPortraitLead");
            objbulk.ColumnMappings.Add("GoLiveMonth", "GoLiveMonth");
            objbulk.ColumnMappings.Add("GoLiveYear", "GoLiveYear");
            objbulk.ColumnMappings.Add("Quarter", "Quarter");
            objbulk.ColumnMappings.Add("CycleTime", "CycleTime");
            objbulk.ColumnMappings.Add("ExternalKickoffDuedate", "ExternalKickoffDuedate");
            objbulk.ColumnMappings.Add("RegionalProjectManager", "RegionalProjectManager");
            objbulk.ColumnMappings.Add("RegionalCISOBTLead", "RegionalCISOBTLead");
            objbulk.ColumnMappings.Add("RegionalCISPortraitLead", "RegionalCISPortraitLead");
            objbulk.ColumnMappings.Add("ProjectStart_ForCycleTime", "ProjectStart_ForCycleTime");
            objbulk.ColumnMappings.Add("Status", "Status");
            objbulk.ColumnMappings.Add("ImplementationType", "ImplementationType");
            objbulk.ColumnMappings.Add("DataSourceType", "DataSourceType");
            objbulk.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
            objbulk.ColumnMappings.Add("AwardedDate", "AwardedDate");
            objbulk.ColumnMappings.Add("ClosedDate", "ClosedDate");
            objbulk.ColumnMappings.Add("MilestoneType", "MilestoneType");
            objbulk.ColumnMappings.Add("DataDescription", "DataDescription");
            objbulk.ColumnMappings.Add("AccountOwner", "AccountOwner");
            objbulk.ColumnMappings.Add("ImplementationFeePsd", "ImplementationFeePsd");
            objbulk.ColumnMappings.Add("CreatedDate", "CreatedDate");
            con.Open();
            string CLR = "Truncate Table OldCLRData";
            SqlCommand Com = new SqlCommand(CLR, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(tbl);
            con.Close();
        }
        public void StoreOldOldData()
        {
            var data = (from a in entity.OldCLRDatas
                        select a).ToList();
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("RevenueID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Region", typeof(string)));
            tbl.Columns.Add(new DataColumn("Country", typeof(string)));
            tbl.Columns.Add(new DataColumn("OwnerShip", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("CountryStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("ProjectLevel", typeof(string)));
            //tbl.Columns.Add(new DataColumn("ProjectStartDate", typeof(DateTime)));
            //tbl.Columns.Add(new DataColumn("IntitialGoliveDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("CompletedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("AssigneeFullName", typeof(string)));
            tbl.Columns.Add(new DataColumn("MilestoneTitle", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Record_ID_Key", typeof(string)));
            tbl.Columns.Add(new DataColumn("Task__Task_Record_ID_Key", typeof(string)));
            tbl.Columns.Add(new DataColumn("Group_Name", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Project_Notes", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Reason_Code", typeof(string)));
            tbl.Columns.Add(new DataColumn("Milestone__Closed_Loop_Owner", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace_Title", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace__ELT_Overall_Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace__ELT_Overall_Comments", typeof(string)));
            tbl.Columns.Add(new DataColumn("Customer_Row_ID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Opportunity_ID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Account_Name", typeof(string)));
            tbl.Columns.Add(new DataColumn("Sales_Stage_Name", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Type", typeof(string)));
            tbl.Columns.Add(new DataColumn("Revenue_Opportunity_Type", typeof(string)));
            tbl.Columns.Add(new DataColumn("Revenue_Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Owner", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Category", typeof(string)));
            tbl.Columns.Add(new DataColumn("Revenue_Total_Transactions", typeof(float)));
            tbl.Columns.Add(new DataColumn("CountryCode", typeof(string)));
            tbl.Columns.Add(new DataColumn("Client", typeof(string)));
            tbl.Columns.Add(new DataColumn("RevenueVolumeUSD", typeof(double)));
            tbl.Columns.Add(new DataColumn("GlobalProjectManager", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISOBTLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISHRFeedSpecialist", typeof(string)));
            tbl.Columns.Add(new DataColumn("GlobalCISPortraitLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveMonth", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveYear", typeof(string)));
            tbl.Columns.Add(new DataColumn("Quarter", typeof(string)));
            tbl.Columns.Add(new DataColumn("CycleTime", typeof(float)));
            tbl.Columns.Add(new DataColumn("PerCompleted", typeof(float)));
            tbl.Columns.Add(new DataColumn("MilestoneDueDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Line_Win_Probability", typeof(float)));
            tbl.Columns.Add(new DataColumn("Next_Step", typeof(string)));
            tbl.Columns.Add(new DataColumn("ExternalKickoffDuedate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("RegionalProjectManager", typeof(string)));
            tbl.Columns.Add(new DataColumn("RegionalCISOBTLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("RegionalCISPortraitLead", typeof(string)));
            tbl.Columns.Add(new DataColumn("ProjectStart_ForCycleTime", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("ImplementationType", typeof(string)));
            tbl.Columns.Add(new DataColumn("DataSourceType", typeof(string)));
            tbl.Columns.Add(new DataColumn("LastUpdateDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("AwardedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ClosedDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("MilestoneType", typeof(string)));
            tbl.Columns.Add(new DataColumn("DataDescription", typeof(string)));
            tbl.Columns.Add(new DataColumn("AccountOwner", typeof(string)));
            tbl.Columns.Add(new DataColumn("ImplementationFeePsd", typeof(double)));
            tbl.Columns.Add(new DataColumn("CreatedDate", typeof(DateTime)));
            foreach (var r in data)
            {
                DataRow dr = tbl.NewRow();
                dr["RevenueID"] = (object)r.RevenueID ?? DBNull.Value;
                dr["Region"] = (object)r.Region ?? DBNull.Value;
                dr["Country"] = (object)r.Country ?? DBNull.Value;
                dr["OwnerShip"] = (object)r.OwnerShip ?? DBNull.Value;
                dr["GoLiveDate"] = (object)r.GoLiveDate ?? DBNull.Value;
                dr["ProjectStatus"] = (object)r.ProjectStatus ?? DBNull.Value;
                dr["CountryStatus"] = r.CountryStatus;
                dr["ProjectLevel"] = r.ProjectLevel;
                //dr["ProjectStartDate"] = (object)r.ProjectStartDate ?? DBNull.Value;
                //dr["IntitialGoliveDate"] = (object)r.IntitialGoliveDate ?? DBNull.Value;
                dr["CompletedDate"] = (object)r.CompletedDate ?? DBNull.Value;
                dr["AssigneeFullName"] = (object)r.AssigneeFullName ?? DBNull.Value;
                dr["MilestoneTitle"] = r.MilestoneTitle;
                dr["Milestone__Record_ID_Key"] = r.Milestone__Record_ID_Key;
                dr["Task__Task_Record_ID_Key"] = r.Task__Task_Record_ID_Key;
                dr["Group_Name"] = r.Group_Name;
                dr["Milestone__Project_Notes"] = r.Milestone__Project_Notes;
                dr["Milestone__Reason_Code"] = r.Milestone__Reason_Code;
                dr["Milestone__Closed_Loop_Owner"] = r.Milestone__Closed_Loop_Owner;
                dr["Workspace_Title"] = r.Workspace_Title;
                dr["Workspace__ELT_Overall_Status"] = r.Workspace__ELT_Overall_Status;
                dr["Workspace__ELT_Overall_Comments"] = r.Workspace__ELT_Overall_Comments;
                dr["Customer_Row_ID"] = (object)(r.Customer_Row_ID) ?? DBNull.Value;
                dr["Opportunity_ID"] = (object)(r.Opportunity_ID) ?? DBNull.Value;
                dr["Account_Name"] = r.Account_Name;
                dr["Sales_Stage_Name"] = r.Sales_Stage_Name;
                dr["Opportunity_Type"] = r.Opportunity_Type;
                dr["Revenue_Opportunity_Type"] = r.Revenue_Opportunity_Type;
                dr["Revenue_Status"] = r.Revenue_Status;
                dr["Opportunity_Owner"] = r.Opportunity_Owner;
                dr["Opportunity_Category"] = r.Opportunity_Category;
                dr["Revenue_Total_Transactions"] = r.Revenue_Total_Transactions;
                dr["CountryCode"] = (object)r.CountryCode ?? DBNull.Value;
                dr["Client"] = (object)r.Client ?? DBNull.Value;
                dr["RevenueVolumeUSD"] = r.RevenueVolumeUSD ?? 0;
                dr["GlobalProjectManager"] = r.GlobalProjectManager;
                dr["GlobalCISOBTLead"] = r.GlobalCISOBTLead;
                dr["GlobalCISHRFeedSpecialist"] = r.GlobalCISHRFeedSpecialist;
                dr["GlobalCISPortraitLead"] = r.GlobalCISPortraitLead;
                dr["GoLiveMonth"] = (object)r.GoLiveMonth ?? DBNull.Value;
                dr["GoLiveYear"] = (object)r.GoLiveYear ?? DBNull.Value;
                dr["Quarter"] = (object)r.Quarter ?? DBNull.Value;
                //dr["CycleTime"] = (object)BusinessDaysUntil(r.GoLiveDate ?? TodayDate, r.TaskDueDate ?? TodayDate) ?? DBNull.Value;
                dr["CycleTime"] = (object)r.CycleTime;
                dr["ExternalKickoffDuedate"] = (object)r.ExternalKickoffDuedate ?? DBNull.Value;
                dr["PerCompleted"] = (object)r.PerCompleted ?? DBNull.Value;
                dr["MilestoneDueDate"] = (object)r.MilestoneDueDate ?? DBNull.Value;
                dr["Line_Win_Probability"] = (object)r.Line_Win_Probability ?? DBNull.Value;
                dr["Next_Step"] = (object)r.Next_Step;
                dr["RegionalProjectManager"] = r.RegionalProjectManager;
                dr["RegionalCISOBTLead"] = r.RegionalCISOBTLead;
                dr["RegionalCISPortraitLead"] = r.RegionalCISPortraitLead;
                dr["ProjectStart_ForCycleTime"] = (object)r.ProjectStart_ForCycleTime ?? DBNull.Value;
                dr["Status"] = r.Status;
                dr["ImplementationType"] = r.ImplementationType;
                dr["DataSourceType"] = r.DataSourceType;
                dr["LastUpdateDate"] = (object)r.LastUpdateDate ?? DBNull.Value;
                dr["AwardedDate"] = (object)r.AwardedDate ?? DBNull.Value;
                dr["ClosedDate"] = (object)r.ClosedDate ?? DBNull.Value;
                dr["MilestoneType"] = (object)r.MilestoneType ?? DBNull.Value;
                dr["DataDescription"] = r.DataDescription;
                dr["AccountOwner"] = (object)r.AccountOwner ?? DBNull.Value;
                dr["ImplementationFeePsd"] = r.ImplementationFeePsd;
                dr["CreatedDate"] = (object)r.CreatedDate ?? DBNull.Value;
                //if (r.TypeofData == "Pipeline")
                //{
                //    if (r.ProjectLevel == "Local")
                //    {
                //        if (crm_countries.Any(val => r.Country.Equals(val)))
                //        {
                //        }
                //        else
                //        {
                //            tbl.Rows.Add(dr);
                //        }
                //    }
                //    else
                //    {
                //        tbl.Rows.Add(dr);
                //    }
                //}
                //else
                //{
                tbl.Rows.Add(dr);
                //}
            }
            //ViewBag.Error4 = tbl.Rows;
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "OldOldCLRData";
            objbulk.ColumnMappings.Add("RevenueID", "RevenueID");
            objbulk.ColumnMappings.Add("Region", "Region");
            objbulk.ColumnMappings.Add("Country", "Country");
            objbulk.ColumnMappings.Add("OwnerShip", "OwnerShip");
            objbulk.ColumnMappings.Add("GoLiveDate", "GoLiveDate");
            objbulk.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
            objbulk.ColumnMappings.Add("CountryStatus", "CountryStatus");
            objbulk.ColumnMappings.Add("ProjectLevel", "ProjectLevel");
            //objbulk.ColumnMappings.Add("ProjectStartDate", "ProjectStartDate");
            //objbulk.ColumnMappings.Add("IntitialGoliveDate", "IntitialGoliveDate");
            objbulk.ColumnMappings.Add("CompletedDate", "CompletedDate");
            objbulk.ColumnMappings.Add("AssigneeFullName", "AssigneeFullName");
            objbulk.ColumnMappings.Add("PerCompleted", "PerCompleted");
            objbulk.ColumnMappings.Add("MilestoneDueDate", "MilestoneDueDate");
            objbulk.ColumnMappings.Add("Line_Win_Probability", "Line_Win_Probability");
            objbulk.ColumnMappings.Add("Next_Step", "Next_Step");
            objbulk.ColumnMappings.Add("MilestoneTitle", "MilestoneTitle");
            objbulk.ColumnMappings.Add("Milestone__Record_ID_Key", "Milestone__Record_ID_Key");
            objbulk.ColumnMappings.Add("Task__Task_Record_ID_Key", "Task__Task_Record_ID_Key");
            objbulk.ColumnMappings.Add("Group_Name", "Group_Name");
            objbulk.ColumnMappings.Add("Milestone__Project_Notes", "Milestone__Project_Notes");
            objbulk.ColumnMappings.Add("Milestone__Reason_Code", "Milestone__Reason_Code");
            objbulk.ColumnMappings.Add("Milestone__Closed_Loop_Owner", "Milestone__Closed_Loop_Owner");
            objbulk.ColumnMappings.Add("Workspace_Title", "Workspace_Title");
            objbulk.ColumnMappings.Add("Workspace__ELT_Overall_Status", "Workspace__ELT_Overall_Status");
            objbulk.ColumnMappings.Add("Workspace__ELT_Overall_Comments", "Workspace__ELT_Overall_Comments");
            objbulk.ColumnMappings.Add("Customer_Row_ID", "Customer_Row_ID");
            objbulk.ColumnMappings.Add("Opportunity_ID", "Opportunity_ID");
            objbulk.ColumnMappings.Add("Account_Name", "Account_Name");
            objbulk.ColumnMappings.Add("Sales_Stage_Name", "Sales_Stage_Name");
            objbulk.ColumnMappings.Add("Opportunity_Type", "Opportunity_Type");
            objbulk.ColumnMappings.Add("Revenue_Opportunity_Type", "Revenue_Opportunity_Type");
            objbulk.ColumnMappings.Add("Revenue_Status", "Revenue_Status");
            objbulk.ColumnMappings.Add("Opportunity_Owner", "Opportunity_Owner");
            objbulk.ColumnMappings.Add("Opportunity_Category", "Opportunity_Category");
            objbulk.ColumnMappings.Add("Revenue_Total_Transactions", "Revenue_Total_Transactions");
            objbulk.ColumnMappings.Add("CountryCode", "CountryCode");
            objbulk.ColumnMappings.Add("Client", "Client");
            objbulk.ColumnMappings.Add("RevenueVolumeUSD", "RevenueVolumeUSD");
            objbulk.ColumnMappings.Add("GlobalProjectManager", "GlobalProjectManager");
            objbulk.ColumnMappings.Add("GlobalCISOBTLead", "GlobalCISOBTLead");
            objbulk.ColumnMappings.Add("GlobalCISHRFeedSpecialist", "GlobalCISHRFeedSpecialist");
            objbulk.ColumnMappings.Add("GlobalCISPortraitLead", "GlobalCISPortraitLead");
            objbulk.ColumnMappings.Add("GoLiveMonth", "GoLiveMonth");
            objbulk.ColumnMappings.Add("GoLiveYear", "GoLiveYear");
            objbulk.ColumnMappings.Add("Quarter", "Quarter");
            objbulk.ColumnMappings.Add("CycleTime", "CycleTime");
            objbulk.ColumnMappings.Add("ExternalKickoffDuedate", "ExternalKickoffDuedate");
            objbulk.ColumnMappings.Add("RegionalProjectManager", "RegionalProjectManager");
            objbulk.ColumnMappings.Add("RegionalCISOBTLead", "RegionalCISOBTLead");
            objbulk.ColumnMappings.Add("RegionalCISPortraitLead", "RegionalCISPortraitLead");
            objbulk.ColumnMappings.Add("ProjectStart_ForCycleTime", "ProjectStart_ForCycleTime");
            objbulk.ColumnMappings.Add("Status", "Status");
            objbulk.ColumnMappings.Add("ImplementationType", "ImplementationType");
            objbulk.ColumnMappings.Add("DataSourceType", "DataSourceType");
            objbulk.ColumnMappings.Add("LastUpdateDate", "LastUpdateDate");
            objbulk.ColumnMappings.Add("AwardedDate", "AwardedDate");
            objbulk.ColumnMappings.Add("ClosedDate", "ClosedDate");
            objbulk.ColumnMappings.Add("MilestoneType", "MilestoneType");
            objbulk.ColumnMappings.Add("DataDescription", "DataDescription");
            objbulk.ColumnMappings.Add("AccountOwner", "AccountOwner");
            objbulk.ColumnMappings.Add("ImplementationFeePsd", "ImplementationFeePsd");
            objbulk.ColumnMappings.Add("CreatedDate", "CreatedDate");
            con.Open();
            string CLR = "Truncate Table OldOldCLRData";
            SqlCommand Com = new SqlCommand(CLR, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(tbl);
            con.Close();
            StoreOldData();
        }
        public ActionResult WeeklyDelta(CLRData clrdata)
        {
            //    var date = (from a in entity.ReportUpdatedOns
            //                where a.ReportName == "WeeklyDelta"
            //                select new {
            //                    a.UpdatedOn
            //                }).OrderByDescending(x=>x.UpdatedOn).ToList();
            //    DateTime reportupdatedon = date.FirstOrDefault(x=>x.UpdatedOn).
            StoreOldOldData();
            var D_Status = "C-Closed,A-Active/Date Confirmed".Split(',');
            var Year = DateTime.Today.Date.Year.ToString();
            var MWD_CLRData = (from a in entity.CLRDatas
                               where a.GoLiveYear == Year
                               where a.Status == "Active"
                               where D_Status.Any(val => a.ProjectStatus.Equals(val))
                               select a);
            var MonthWiseDelta = (from a in entity.MonthWiseDeltas
                                  where a.DeltaID == entity.MonthWiseDeltas.Max(x => x.DeltaID)
                                  select new
                                  {
                                      Jan = a.CV_Jan,
                                      Feb = a.CV_Feb,
                                      Mar = a.CV_Mar,
                                      Apr = a.CV_Apr,
                                      May = a.CV_May,
                                      Jun = a.CV_Jun,
                                      Jul = a.CV_Jul,
                                      Aug = a.CV_Aug,
                                      Sep = a.CV_Sep,
                                      Oct = a.CV_Oct,
                                      Nov = a.CV_Nov,
                                      Dec = a.CV_Dec,
                                      Year = DateTime.Today.Date.Year.ToString(),
                                      InsertedDate = DateTime.Today.Date,
                                      Jan_Comments = "---",
                                      Feb_Comments = "---",
                                      Mar_Comments = "---",
                                      Apr_Comments = "---",
                                      May_Comments = "---",
                                      Jun_Comments = "---",
                                      Jul_Comments = "---",
                                      Aug_Comments = "---",
                                      Sep_Comments = "---",
                                      Oct_Comments = "---",
                                      Nov_Comments = "---",
                                      Dec_Comments = "---",
                                      CV_Jan = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Jan").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Feb = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Feb").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Mar = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Mar").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Apr = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Apr").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_May = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "May").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Jun = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Jun").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Jul = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Jul").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Aug = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Aug").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Sep = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Sep").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Oct = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Oct").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Nov = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Nov").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                      CV_Dec = MWD_CLRData.Where(x1 => x1.GoLiveMonth == "Dec").Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                  }).ToList();
            entity.MonthWiseDeltas.Add(
                new MonthWiseDelta
                {
                    Jan = MonthWiseDelta[0].Jan,
                    Feb = MonthWiseDelta[0].Feb,
                    Mar = MonthWiseDelta[0].Mar,
                    Apr = MonthWiseDelta[0].Apr,
                    May = MonthWiseDelta[0].May,
                    Jun = MonthWiseDelta[0].Jun,
                    Jul = MonthWiseDelta[0].Jul,
                    Aug = MonthWiseDelta[0].Aug,
                    Sep = MonthWiseDelta[0].Sep,
                    Oct = MonthWiseDelta[0].Oct,
                    Nov = MonthWiseDelta[0].Nov,
                    Dec = MonthWiseDelta[0].Dec,
                    Year = MonthWiseDelta[0].Year,
                    InsertedDate = MonthWiseDelta[0].InsertedDate,
                    Jan_Comments = MonthWiseDelta[0].Jan_Comments,
                    Feb_Comments = MonthWiseDelta[0].Feb_Comments,
                    Mar_Comments = MonthWiseDelta[0].Mar_Comments,
                    Apr_Comments = MonthWiseDelta[0].Apr_Comments,
                    May_Comments = MonthWiseDelta[0].May_Comments,
                    Jun_Comments = MonthWiseDelta[0].Jun_Comments,
                    Jul_Comments = MonthWiseDelta[0].Jul_Comments,
                    Aug_Comments = MonthWiseDelta[0].Aug_Comments,
                    Sep_Comments = MonthWiseDelta[0].Sep_Comments,
                    Oct_Comments = MonthWiseDelta[0].Oct_Comments,
                    Nov_Comments = MonthWiseDelta[0].Nov_Comments,
                    Dec_Comments = MonthWiseDelta[0].Dec_Comments,
                    CV_Jan = MonthWiseDelta[0].CV_Jan,
                    CV_Feb = MonthWiseDelta[0].CV_Feb,
                    CV_Mar = MonthWiseDelta[0].CV_Mar,
                    CV_Apr = MonthWiseDelta[0].CV_Apr,
                    CV_May = MonthWiseDelta[0].CV_May,
                    CV_Jun = MonthWiseDelta[0].CV_Jun,
                    CV_Jul = MonthWiseDelta[0].CV_Jul,
                    CV_Aug = MonthWiseDelta[0].CV_Aug,
                    CV_Sep = MonthWiseDelta[0].CV_Sep,
                    CV_Oct = MonthWiseDelta[0].CV_Oct,
                    CV_Nov = MonthWiseDelta[0].CV_Nov,
                    CV_Dec = MonthWiseDelta[0].CV_Dec,
                });
            entity.SaveChanges();
            DateTime TodaysDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "WeeklyDelta",
                    UpdatedOn = TodaysDate,
                });
            entity.SaveChanges();
            ViewBag.Error13 = "Success";
            return View("Index");
        }
        string D_CurrentMonth, D_CurrentYear, D_CurrentYearPrev, D_CurrentMonthPrev;

        List<string> Clients = new List<string>();
        public void StorePreviousELt()
        {
            if(DateTime.Now.Day <= 11 && DateTime.Now.Month == 1)
            {
                D_CurrentMonthPrev = "Dec";
                D_CurrentYearPrev = DateTime.Now.AddYears(-1).Year.ToString();
            }
            else if (DateTime.Now.Day <= 11)
            {
                D_CurrentMonthPrev = DateTime.Now.AddMonths(-1).ToString("MMM");
                D_CurrentYearPrev = DateTime.Now.Year.ToString();
            }
            else
            {
                D_CurrentMonthPrev = DateTime.Now.ToString("MMM");
                D_CurrentYearPrev = DateTime.Now.Year.ToString();
            }
            DateTime TodayDate = DateTime.Now;
            var D_Status = "C-Closed,A-Active/Date Confirmed".Split(',');
            var otherstatus = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            var Regions = "APAC,EMEA,LATAM,NORAM".Split(',');

            var PriorEltReport = (from a in entity.CLRDatas
                                  where a.Status == "Active"
                                  where a.Client != null
                                  where a.Client != ""
                                  where D_Status.Any(val => a.ProjectStatus.Equals(val))
                                  where Regions.Any(val => a.Region.Equals(val))
                                  where a.GoLiveMonth == D_CurrentMonthPrev
                                  where a.GoLiveYear == D_CurrentYearPrev
                                  group a by a.Client into g
                                  select new
                                  {
                                      Client = g.Key,
                                      APAC = g.Where(x => x.Region == "APAC").Sum(x => x.RevenueVolumeUSD) ?? 0,
                                      EMEA = g.Where(x => x.Region == "EMEA").Sum(x => x.RevenueVolumeUSD) ?? 0,
                                      LATAM = g.Where(x => x.Region == "LATAM").Sum(x => x.RevenueVolumeUSD) ?? 0,
                                      NORAM = g.Where(x => x.Region == "NORAM").Sum(x => x.RevenueVolumeUSD) ?? 0,
                                      Total = g.Sum(x => x.RevenueVolumeUSD) ?? 0,
                                      NBVPriorMonth = entity.EltDeltaClients.Where(x => x.Client == g.Key && x.Month == D_CurrentMonthPrev && x.Year == D_CurrentYearPrev).Count() > 0 ? entity.EltDeltaClients.Where(x => x.Client == g.Key && x.Month == D_CurrentMonthPrev && x.Year == D_CurrentYearPrev).Sum(x => x.Revenue) : 0,
                                      Workspace = entity.CLRDatas.FirstOrDefault(x => x.Client == g.Key && x.Status == "Active").Workspace_Title,
                                      TotalAccountVolume = entity.CLRDatas.Where(x => x.Client == g.Key && x.Status == "Active" && otherstatus.Any(val1 => x.ProjectStatus.Equals(val1))).Sum(x => x.RevenueVolumeUSD) ?? 0,
                                  }).ToList();
            for (var i = 0; i < PriorEltReport.Count; i++)
            {
                Clients.Add(PriorEltReport[i].Client);
            }
            var clients = (from a in entity.EltDeltaClients
                           where a.Month == D_CurrentMonthPrev
                           where a.Year == D_CurrentYearPrev
                           where a.Revenue > 0
                           where !Clients.Any(val => a.Client.Equals(val))
                           select a.Client).ToList();
            var MovedClients = (from a in entity.EltDeltaClients
                                where a.Month == D_CurrentMonthPrev
                                where a.Year == D_CurrentYearPrev
                                where a.Revenue > 0
                                where !Clients.Any(val => a.Client.Equals(val))
                                group a by a.Client into g
                                select new
                                {
                                    Client = g.Key,
                                    APAC = (double)0,
                                    EMEA = (double)0,
                                    LATAM = (double)0,
                                    NORAM = (double)0,
                                    Total = (double)0,
                                    NBVPriorMonth = (from elt in g select elt.Revenue).Sum(),
                                    Workspace = entity.CLRDatas.FirstOrDefault(x => x.Client == g.Key && x.Status == "Active").Workspace_Title,
                                    TotalAccountVolume = entity.CLRDatas.Where(x => x.Client == g.Key && x.Status == "Active").Sum(x => x.RevenueVolumeUSD) ?? 0,
                                });
            var EltDeltaCLients = PriorEltReport.Concat(MovedClients);
            DataTable D_tbl = new DataTable();
            D_tbl.Columns.Add(new DataColumn("Client", typeof(string)));
            D_tbl.Columns.Add(new DataColumn("EMEA", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("APAC", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("LATAM", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("NORAM", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("Total", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("NBAPriorMonth", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("TotalAccountVolume", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("Delta", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("Comments", typeof(string)));
            D_tbl.Columns.Add(new DataColumn("Year", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("Month", typeof(string)));
            D_tbl.Columns.Add(new DataColumn("InsertedOn", typeof(DateTime)));
            foreach (var dc in EltDeltaCLients)
            {
                DataRow dr1 = D_tbl.NewRow();
                dr1["Client"] = dc.Client;
                dr1["EMEA"] = dc.EMEA;
                dr1["APAC"] = dc.APAC;
                dr1["LATAM"] = dc.LATAM;
                dr1["NORAM"] = dc.NORAM;
                dr1["Total"] = dc.Total;
                dr1["NBAPriorMonth"] = dc.NBVPriorMonth;
                dr1["TotalAccountVolume"] = dc.TotalAccountVolume;
                dr1["Delta"] = dc.Total - dc.NBVPriorMonth;
                dr1["Comments"] = "";
                dr1["Year"] = D_CurrentYearPrev;
                dr1["Month"] = D_CurrentMonthPrev;
                dr1["InsertedOn"] = TodayDate;
                D_tbl.Rows.Add(dr1);
            }
            SqlBulkCopy D_objbulk = new SqlBulkCopy(con);
            D_objbulk.DestinationTableName = "PreviousMonthsElt";
            D_objbulk.ColumnMappings.Add("Client", "Client");
            D_objbulk.ColumnMappings.Add("EMEA", "EMEA");
            D_objbulk.ColumnMappings.Add("APAC", "APAC");
            D_objbulk.ColumnMappings.Add("LATAM", "LATAM");
            D_objbulk.ColumnMappings.Add("NORAM", "NORAM");
            D_objbulk.ColumnMappings.Add("Total", "Total");
            D_objbulk.ColumnMappings.Add("NBAPriorMonth", "NBAPriorMonth");
            D_objbulk.ColumnMappings.Add("TotalAccountVolume", "TotalAccountVolume");
            D_objbulk.ColumnMappings.Add("Delta", "Delta");
            D_objbulk.ColumnMappings.Add("Comments", "Comments");
            D_objbulk.ColumnMappings.Add("Year", "Year");
            D_objbulk.ColumnMappings.Add("Month", "Month");
            D_objbulk.ColumnMappings.Add("InsertedOn", "InsertedOn");
            con.Open();
            D_objbulk.BatchSize = 100000;
            D_objbulk.BulkCopyTimeout = 0;
            D_objbulk.WriteToServer(D_tbl);
            con.Close();

            var ELTDeltaCLients = (from a in entity.EltDeltaClients
                                   where a.Month == D_CurrentMonthPrev
                                   where a.Year == D_CurrentYearPrev
                                   where a.Revenue > 0
                                   select a.Client).ToList();

            var DeltaComments = (from a in entity.EltOldCLRDatas
                                 where ELTDeltaCLients.Any(val => a.Client.Equals(val))
                                 where a.GoLiveMonth == D_CurrentMonthPrev
                                 where a.GoLiveYear == D_CurrentYearPrev
                                 where a.RevenueVolumeUSD > 0
                                 where a.Status == "Active"
                                 where D_Status.Any(val1 => a.ProjectStatus.Equals(val1))
                                 join b in entity.CLRDatas on a.RevenueID equals b.RevenueID into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new ELTDeltaComments
                                 {
                                     Client = a.Client,
                                     RevenueID = abc.RevenueID,
                                     ProjectStatus = a.ProjectStatus,
                                     GoLiveMonth = a.GoLiveMonth,
                                     GoLiveYear = a.GoLiveYear,
                                     Country = a.Country,
                                     Region = a.Region,
                                     Workspace_Title = a.Workspace_Title,
                                     PreviousVolume = a.RevenueVolumeUSD ?? 0,
                                     CurrentVolume = abc.RevenueVolumeUSD ?? 0,
                                     RevenueVolumeUSD = 0,
                                     CurrentProjectStatus = abc.ProjectStatus,
                                     CurrentMonth = abc.GoLiveMonth,
                                     CurrentYear = abc.GoLiveYear,
                                     Comments = "",
                                     DeltaColor = ""
                                 }).Distinct().ToList();
            var RevenueIds = (from a in DeltaComments
                              select a.RevenueID).ToList();
            var OtherDeltaComments = (from a in entity.CLRDatas
                                      where !RevenueIds.Any(val => a.RevenueID.Equals(val))
                                      where a.GoLiveMonth == D_CurrentMonthPrev
                                      where a.GoLiveYear == D_CurrentYearPrev
                                      where a.RevenueVolumeUSD > 0
                                      where a.Status == "Active"
                                      where D_Status.Any(val1 => a.ProjectStatus.Equals(val1))
                                      join b in entity.EltOldCLRDatas on a.RevenueID equals b.RevenueID into ab
                                      from abc in ab.DefaultIfEmpty()
                                      select new ELTDeltaComments
                                      {
                                          Client = a.Client,
                                          RevenueID = a.RevenueID,
                                          ProjectStatus = abc.ProjectStatus == null ? "---" : abc.ProjectStatus,
                                          GoLiveMonth = abc.GoLiveMonth,
                                          GoLiveYear = abc.GoLiveYear,
                                          Country = a.Country,
                                          Region = a.Region,
                                          Workspace_Title = a.Workspace_Title,
                                          PreviousVolume = abc.RevenueVolumeUSD ?? 0,
                                          CurrentVolume = a.RevenueVolumeUSD ?? 0,
                                          RevenueVolumeUSD = 0,
                                          CurrentProjectStatus = a.ProjectStatus,
                                          CurrentMonth = a.GoLiveMonth,
                                          CurrentYear = a.GoLiveYear,
                                          Comments = "",
                                          DeltaColor = ""
                                      }).Distinct().ToList();
            var ELTDeltaComments = DeltaComments.Concat(OtherDeltaComments).ToList();
            List<double> RemovableRecords = new List<double>();
            foreach (var r in ELTDeltaComments)
            {
                var comments = "";
                if (D_Status.Any(val1 => r.CurrentProjectStatus.Equals(val1)))
                {
                    if (!D_Status.Any(val1 => r.ProjectStatus.Equals(val1)))
                    {
                        r.DeltaColor = "green";
                        r.RevenueVolumeUSD = r.CurrentVolume;
                        comments = "Project Status Moved from " + r.ProjectStatus + " to " + r.CurrentProjectStatus;
                    }
                }
                else
                {
                    r.DeltaColor = "red";
                    r.RevenueVolumeUSD = r.PreviousVolume;
                    comments = "Project Status Moved from " + r.ProjectStatus + " to " + r.CurrentProjectStatus;
                }
                if (r.GoLiveMonth != r.CurrentMonth)
                {
                    if (r.DeltaColor == "red")
                    {
                        r.DeltaColor = "red";
                        r.RevenueVolumeUSD = r.PreviousVolume;
                    }
                    else
                    {
                        if (r.CurrentMonth == D_CurrentMonthPrev)
                        {
                            r.DeltaColor = "green";
                            r.RevenueVolumeUSD = r.CurrentVolume;
                        }
                        else
                        {
                            r.DeltaColor = "red";
                            r.RevenueVolumeUSD = r.PreviousVolume;
                        }
                    }
                    if (comments != "")
                    {
                        comments += "\n";
                    }
                    comments += "Golive Date has been changed from " + r.GoLiveMonth + "-" + r.GoLiveYear + " to " + r.CurrentMonth + "-" + r.CurrentYear;
                }
                if (r.PreviousVolume != r.CurrentVolume)
                {
                    if (r.DeltaColor == "red")
                    {
                    }
                    else
                    {
                        if (r.PreviousVolume < r.CurrentVolume)
                        {
                            r.DeltaColor = "green";
                        }
                        else
                        {
                            r.DeltaColor = "red";
                        }
                    }
                    r.RevenueVolumeUSD = r.CurrentVolume - r.PreviousVolume;
                    if (comments != "")
                    {
                        comments += "\n";
                    }
                    comments += "Volume has been updated from " + r.PreviousVolume + " to " + r.CurrentVolume;
                }
                r.Comments = comments;
                if (comments == "")
                {
                    var count = ELTDeltaComments.Where(x => x.RevenueID == r.RevenueID).Count();
                    if (count > 1)
                    {
                        for (var k = 0; k < count; k++)
                        {
                            RemovableRecords.Add(r.RevenueID);
                        }
                    }
                    else
                    {
                        RemovableRecords.Add(r.RevenueID);
                    }
                }
            }
            for (int i = 0; i < RemovableRecords.Count; i++)
            {
                int index = ELTDeltaComments.FindIndex(a => a.RevenueID == RemovableRecords[i]);
                ELTDeltaComments.RemoveAt(index);
            }

            DataTable D_tbl2 = new DataTable();
            D_tbl2.Columns.Add(new DataColumn("Client", typeof(string)));
            D_tbl2.Columns.Add(new DataColumn("WorkspaceTitle", typeof(string)));
            D_tbl2.Columns.Add(new DataColumn("Month", typeof(string)));
            D_tbl2.Columns.Add(new DataColumn("Year", typeof(float)));
            D_tbl2.Columns.Add(new DataColumn("Revenue", typeof(float)));
            D_tbl2.Columns.Add(new DataColumn("RevenueId", typeof(float)));
            D_tbl2.Columns.Add(new DataColumn("Region", typeof(string)));
            D_tbl2.Columns.Add(new DataColumn("Country", typeof(string)));
            D_tbl2.Columns.Add(new DataColumn("Comment", typeof(string)));
            D_tbl2.Columns.Add(new DataColumn("InsertedOn", typeof(DateTime)));
            D_tbl2.Columns.Add(new DataColumn("Status", typeof(string)));
            D_tbl2.Columns.Add(new DataColumn("DeltaColor", typeof(string)));
            foreach (var dc in ELTDeltaComments)
            {
                DataRow dr12 = D_tbl2.NewRow();
                dr12["Client"] = dc.Client;
                dr12["WorkspaceTitle"] = dc.Workspace_Title;
                dr12["Month"] = D_CurrentMonthPrev;
                dr12["Year"] = D_CurrentYearPrev;
                dr12["Revenue"] = dc.RevenueVolumeUSD;
                dr12["RevenueId"] = dc.RevenueID;
                dr12["Region"] = dc.Region;
                dr12["Country"] = dc.Country;
                dr12["Comment"] = dc.Comments;
                dr12["InsertedOn"] = TodayDate;
                dr12["Status"] = "Active";
                dr12["DeltaColor"] = dc.DeltaColor;
                D_tbl2.Rows.Add(dr12);
            }
            SqlBulkCopy D_objbulk2 = new SqlBulkCopy(con);
            D_objbulk2.DestinationTableName = "ELTDeltaComments";
            D_objbulk2.ColumnMappings.Add("Client", "Client");
            D_objbulk2.ColumnMappings.Add("WorkspaceTitle", "WorkspaceTitle");
            D_objbulk2.ColumnMappings.Add("Month", "Month");
            D_objbulk2.ColumnMappings.Add("Year", "Year");
            D_objbulk2.ColumnMappings.Add("Revenue", "Revenue");
            D_objbulk2.ColumnMappings.Add("RevenueId", "RevenueId");
            D_objbulk2.ColumnMappings.Add("Region", "Region");
            D_objbulk2.ColumnMappings.Add("Country", "Country");
            D_objbulk2.ColumnMappings.Add("Comment", "Comment");
            D_objbulk2.ColumnMappings.Add("InsertedOn", "InsertedOn");
            D_objbulk2.ColumnMappings.Add("Status", "Status");
            D_objbulk2.ColumnMappings.Add("DeltaColor", "DeltaColor");
            con.Open();
            D_objbulk2.BatchSize = 100000;
            D_objbulk2.BulkCopyTimeout = 0;
            D_objbulk2.WriteToServer(D_tbl2);
            con.Close();
        }
        public ActionResult ELTDelta(CLRData clrdata)
        {
            StorePreviousELt();
            if (DateTime.Now.Day > 11 && DateTime.Now.Month == 12)
            {
                D_CurrentMonth = "Jan";
                D_CurrentYear = DateTime.Today.AddYears(1).Year.ToString();
            }
            else if (DateTime.Now.Day > 11 && DateTime.Now.Month < 12)
            {
                D_CurrentYear = DateTime.Today.Date.Year.ToString();
                D_CurrentMonth = DateTime.Now.AddMonths(1).ToString("MMM");
            }
            else
            {
                D_CurrentYear = DateTime.Today.Date.Year.ToString();
                D_CurrentMonth = DateTime.Now.ToString("MMM");
            }
            StoreEltoldCLRData();
            var D_Status = "C-Closed,A-Active/Date Confirmed".Split(',');
            var Data = (from a in entity.CLRDatas
                        where a.Status == "Active"
                        where a.Client != null
                        where a.Client != ""
                        where D_Status.Any(val => a.ProjectStatus.Equals(val))
                        where a.GoLiveMonth == D_CurrentMonth
                        where a.GoLiveYear == D_CurrentYear
                        select a).ToList();

            var DeltaClients = (from a in Data
                                group a by a.Client into g
                                select new
                                {
                                    Client = g.Key,
                                    RevenueVolumeUSD = Data.Where(x => x.Client == g.Key).Sum(x => x.RevenueVolumeUSD),
                                }).Distinct().OrderByDescending(x => x.RevenueVolumeUSD);
            DataTable D_tbl = new DataTable();
            D_tbl.Columns.Add(new DataColumn("Client", typeof(string)));
            D_tbl.Columns.Add(new DataColumn("Month", typeof(string)));
            D_tbl.Columns.Add(new DataColumn("Revenue", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("Year", typeof(float)));
            D_tbl.Columns.Add(new DataColumn("InsertedOn", typeof(DateTime)));

            foreach (var dc in DeltaClients)
            {
                DataRow dr1 = D_tbl.NewRow();
                dr1["Client"] = dc.Client;
                dr1["Month"] = D_CurrentMonth;
                dr1["Year"] = D_CurrentYear;
                dr1["Revenue"] = (object)dc.RevenueVolumeUSD ?? DBNull.Value;
                dr1["InsertedOn"] = (object)DateTime.Today.Date ?? DBNull.Value;
                D_tbl.Rows.Add(dr1);
            }
            SqlBulkCopy D_objbulk = new SqlBulkCopy(con);
            D_objbulk.DestinationTableName = "EltDeltaClients";
            D_objbulk.ColumnMappings.Add("Client", "Client");
            D_objbulk.ColumnMappings.Add("Month", "Month");
            D_objbulk.ColumnMappings.Add("Year", "Year");
            D_objbulk.ColumnMappings.Add("Revenue", "Revenue");
            D_objbulk.ColumnMappings.Add("InsertedOn", "InsertedOn");
            con.Open();
            D_objbulk.BatchSize = 100000;
            D_objbulk.BulkCopyTimeout = 0;
            D_objbulk.WriteToServer(D_tbl);
            con.Close();
            DateTime TodaysDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "MonthlyDelta",
                    UpdatedOn = TodaysDate,
                });
            entity.SaveChanges();
            ViewBag.Error14 = "Success";
            return View("Index");
        }
        
        public void StoreEltoldCLRData()
        {
            var data = (from a in entity.CLRDatas
                        where a.RevenueID != 400000000000000
                        where a.RevenueID < 600000000000000
                        select a).ToList();
            DataTable tbl = new DataTable();
            tbl.Columns.Add(new DataColumn("Client", typeof(string)));
            tbl.Columns.Add(new DataColumn("RevenueID", typeof(double)));
            tbl.Columns.Add(new DataColumn("Region", typeof(string)));
            tbl.Columns.Add(new DataColumn("Country", typeof(string)));
            tbl.Columns.Add(new DataColumn("OwnerShip", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveDate", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("CountryStatus", typeof(string)));
            tbl.Columns.Add(new DataColumn("ProjectLevel", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace_Title", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace__ELT_Overall_Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("Workspace__ELT_Overall_Comments", typeof(string)));
            tbl.Columns.Add(new DataColumn("Opportunity_Type", typeof(string)));
            tbl.Columns.Add(new DataColumn("RevenueVolumeUSD", typeof(double)));
            tbl.Columns.Add(new DataColumn("GoLiveMonth", typeof(string)));
            tbl.Columns.Add(new DataColumn("GoLiveYear", typeof(string)));
            tbl.Columns.Add(new DataColumn("Quarter", typeof(string)));
            tbl.Columns.Add(new DataColumn("CycleTime", typeof(float)));
            tbl.Columns.Add(new DataColumn("ProjectStart_ForCycleTime", typeof(DateTime)));
            tbl.Columns.Add(new DataColumn("Status", typeof(string)));
            tbl.Columns.Add(new DataColumn("ImplementationType", typeof(string)));
            tbl.Columns.Add(new DataColumn("DataSourceType", typeof(string)));
            tbl.Columns.Add(new DataColumn("TaskRecordIdKey", typeof(string)));
            foreach (var r in data)
            {
                DataRow dr = tbl.NewRow();
                dr["Client"] = (object)r.Client ?? DBNull.Value;
                dr["RevenueID"] = (object)r.RevenueID ?? DBNull.Value;
                dr["Region"] = (object)r.Region ?? DBNull.Value;
                dr["Country"] = (object)r.Country ?? DBNull.Value;
                dr["OwnerShip"] = (object)r.OwnerShip ?? DBNull.Value;
                dr["GoLiveDate"] = (object)r.GoLiveDate ?? DBNull.Value;
                dr["ProjectStatus"] = (object)r.ProjectStatus ?? DBNull.Value;
                dr["ProjectLevel"] = r.ProjectLevel;
                dr["Workspace_Title"] = r.Workspace_Title;
                dr["Workspace__ELT_Overall_Status"] = r.Workspace__ELT_Overall_Status;
                dr["Workspace__ELT_Overall_Comments"] = r.Workspace__ELT_Overall_Comments;
                dr["Opportunity_Type"] = r.Opportunity_Type;
                dr["RevenueVolumeUSD"] = r.RevenueVolumeUSD ?? 0;
                dr["GoLiveMonth"] = (object)r.GoLiveMonth ?? DBNull.Value;
                dr["GoLiveYear"] = (object)r.GoLiveYear ?? DBNull.Value;
                dr["Quarter"] = (object)r.Quarter ?? DBNull.Value;
                dr["CycleTime"] = (object)r.CycleTime ?? 0;
                dr["ProjectStart_ForCycleTime"] = (object)r.ProjectStart_ForCycleTime ?? DBNull.Value;
                dr["Status"] = r.Status;
                dr["ImplementationType"] = r.ImplementationType;
                dr["DataSourceType"] = r.DataSourceType;
                dr["TaskRecordIdKey"] = r.Task__Task_Record_ID_Key;
                tbl.Rows.Add(dr);
            }
            //ViewBag.Error4 = tbl.Rows;
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "EltOldCLRData";
            objbulk.ColumnMappings.Add("Client", "Client");
            objbulk.ColumnMappings.Add("RevenueID", "RevenueID");
            objbulk.ColumnMappings.Add("Region", "Region");
            objbulk.ColumnMappings.Add("Country", "Country");
            objbulk.ColumnMappings.Add("OwnerShip", "OwnerShip");
            objbulk.ColumnMappings.Add("GoLiveDate", "GoLiveDate");
            objbulk.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
            objbulk.ColumnMappings.Add("ProjectLevel", "ProjectLevel");
            objbulk.ColumnMappings.Add("Workspace_Title", "Workspace_Title");
            objbulk.ColumnMappings.Add("Workspace__ELT_Overall_Status", "Workspace__ELT_Overall_Status");
            objbulk.ColumnMappings.Add("Workspace__ELT_Overall_Comments", "Workspace__ELT_Overall_Comments");
            objbulk.ColumnMappings.Add("Opportunity_Type", "Opportunity_Type");
            objbulk.ColumnMappings.Add("RevenueVolumeUSD", "RevenueVolumeUSD");
            objbulk.ColumnMappings.Add("GoLiveMonth", "GoLiveMonth");
            objbulk.ColumnMappings.Add("GoLiveYear", "GoLiveYear");
            objbulk.ColumnMappings.Add("Quarter", "Quarter");
            objbulk.ColumnMappings.Add("CycleTime", "CycleTime");
            objbulk.ColumnMappings.Add("ProjectStart_ForCycleTime", "ProjectStart_ForCycleTime");
            objbulk.ColumnMappings.Add("Status", "Status");
            objbulk.ColumnMappings.Add("ImplementationType", "ImplementationType");
            objbulk.ColumnMappings.Add("DataSourceType", "DataSourceType");
            objbulk.ColumnMappings.Add("TaskRecordIdKey", "TaskRecordIdKey");
            con.Open();
            string CLR = "Truncate Table EltOldCLRData";
            SqlCommand Com = new SqlCommand(CLR, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(tbl);
            con.Close();
        }

        public ActionResult GeteSowData()
        {
            //string RequestID = "";
            //var httpRequest = (HttpWebRequest)WebRequest.Create("https://uat.gpsc.cwtwebpem.com/pat/esow/query");
            //httpRequest.Headers["apiKey"] = "XVdjcIpDXDI7R6v9yE2ZxAGDMztLwdocZmh0jUUihjQ";

            //var httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            //using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
            //{
            //    var result = streamReader.ReadToEnd();
            //    JObject jobject = JObject.Parse(result);
            //    RequestID = jobject["data"]["Key"].ToString();
            //}

            ViewBag.Error21 = "Success";
            return View("Index");
        }
        public static int BusinessDaysUntil(DateTime firstDay, DateTime lastDay)
        {
            DateTime TodayDate = DateTime.Today.Date;
            if (firstDay == TodayDate || lastDay == TodayDate)
            {
                return 0;
            }
            else if (firstDay == lastDay)
            {
                return 0;
            }
            else
            {
                firstDay = firstDay.Date;
                lastDay = lastDay.Date;
                if (firstDay > lastDay)
                {
                    return 0;
                    //throw new ArgumentException("Incorrect last day " + lastDay);
                }
                else
                {
                    TimeSpan span = lastDay - firstDay;
                    int businessDays = span.Days + 1;
                    int fullWeekCount = businessDays / 7;
                    // find out if there are weekends during the time exceedng the full weeks
                    if (businessDays > fullWeekCount * 7)
                    {
                        // we are here to find out if there is a 1-day or 2-days weekend
                        // in the time interval remaining after subtracting the complete weeks
                        int firstDayOfWeek = (int)firstDay.DayOfWeek;
                        int lastDayOfWeek = (int)lastDay.DayOfWeek;
                        if (lastDayOfWeek < firstDayOfWeek)
                            lastDayOfWeek += 7;
                        if (firstDayOfWeek <= 6)
                        {
                            if (lastDayOfWeek >= 7)// Both Saturday and Sunday are in the remaining time interval
                                businessDays -= 2;
                            else if (lastDayOfWeek >= 6)// Only Saturday is in the remaining time interval
                                businessDays -= 1;
                        }
                        else if (firstDayOfWeek <= 7 && lastDayOfWeek >= 7)// Only Sunday is in the remaining time interval
                            businessDays -= 1;
                    }
                    // subtract the weekends during the full weeks in the interval
                    businessDays -= fullWeekCount + fullWeekCount;
                    // subtract the number of bank holidays during the time interval
                    //foreach (DateTime bankHoliday in bankHolidays)
                    //{
                    //    DateTime bh = bankHoliday.Date;
                    //    if (firstDay <= bh && bh <= lastDay)
                    //        --businessDays;
                    //}
                    return businessDays;
                }
            }
        }
        [HttpPost]
        public ActionResult ImeetImport(HttpPostedFileBase excelfile)
        {
            if (excelfile.ContentLength == 0)
            {
                ViewBag.Error = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (excelfile.FileName.EndsWith("xls") || excelfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(excelfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    excelfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertdataiMeet(filepath, filename);
                    ViewBag.Error = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult CRMImport(HttpPostedFileBase crm_file)
        {
            if (crm_file.ContentLength == 0)
            {
                ViewBag.Error2 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (crm_file.FileName.EndsWith("xls") || crm_file.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(crm_file.FileName);
                    string filepath = "/excelfolder/" + filename;
                    crm_file.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertCRMdata(filepath, filename);
                    ViewBag.Error2 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error2 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult RolesImport(HttpPostedFileBase rolesfile)
        {
            if (rolesfile.ContentLength == 0)
            {
                ViewBag.Error3 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (rolesfile.FileName.EndsWith("xls") || rolesfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(rolesfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    rolesfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertRolesdata(filepath, filename);
                    ViewBag.Error3 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error3 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult PSDImport(HttpPostedFileBase psdfile)
        {
            if (psdfile.ContentLength == 0)
            {
                ViewBag.Error5 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (psdfile.FileName.EndsWith("xls") || psdfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(psdfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    psdfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertPSDdata(filepath, filename);
                    ViewBag.Error5 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error5 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult NPSImpImport(HttpPostedFileBase npsfile)
        {
            if (npsfile.ContentLength == 0)
            {
                ViewBag.Error5 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (npsfile.FileName.EndsWith("xls") || npsfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(npsfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    npsfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertNpsImplementationData(filepath, filename);
                    ViewBag.Error5 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error5 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult CTOImport(HttpPostedFileBase ctofile)
        {
            if (ctofile.ContentLength == 0)
            {
                ViewBag.Error6 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (ctofile.FileName.EndsWith("xls") || ctofile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(ctofile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    ctofile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertCTOdata(filepath, filename);
                    ViewBag.Error6 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error6 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult CommentsImport(HttpPostedFileBase commentfile)
        {
            if (commentfile.ContentLength == 0)
            {
                ViewBag.Error6 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (commentfile.FileName.EndsWith("xls") || commentfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(commentfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    commentfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertCommentsData(filepath, filename);
                    ViewBag.Error6 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error6 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult IMPSImport(HttpPostedFileBase impsfile)
        {
            if (impsfile.ContentLength == 0)
            {
                ViewBag.Error7 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (impsfile.FileName.EndsWith("xls") || impsfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(impsfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    impsfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertIMPSdata(filepath, filename);
                    ViewBag.Error7 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error7 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult LLImport(HttpPostedFileBase llfile)
        {
            if (llfile.ContentLength == 0)
            {
                ViewBag.Error8 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (llfile.FileName.EndsWith("xls") || llfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(llfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    llfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertLLdata(filepath, filename);
                    ViewBag.Error8 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error8 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult SGImport(HttpPostedFileBase sgfile)
        {
            if (sgfile.ContentLength == 0)
            {
                ViewBag.Error9 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (sgfile.FileName.EndsWith("xls") || sgfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(sgfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    sgfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertSGdata(filepath, filename);
                    ViewBag.Error9 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error9 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult APImport(HttpPostedFileBase apfile)
        {
            if (apfile.ContentLength == 0)
            {
                ViewBag.Error10 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (apfile.FileName.EndsWith("xls") || apfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(apfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    apfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertAPdata(filepath, filename);
                    ViewBag.Error10 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error10 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult HEKOImport(HttpPostedFileBase hekofile)
        {
            if (hekofile.ContentLength == 0)
            {
                ViewBag.Error11 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (hekofile.FileName.EndsWith("xls") || hekofile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(hekofile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    hekofile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertHEKOdata(filepath, filename);
                    ViewBag.Error11 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error11 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }

        [HttpPost]
        public ActionResult PDDImport(HttpPostedFileBase pddofile)
        {
            if (pddofile.ContentLength == 0)
            {
                ViewBag.Error12 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (pddofile.FileName.EndsWith("xls") || pddofile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(pddofile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    pddofile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertPDDdata(filepath, filename);
                    ViewBag.Error12 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error12 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult eSowImport(HttpPostedFileBase eSowfile)
        {
            if (eSowfile.ContentLength == 0)
            {
                ViewBag.Error18 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (eSowfile.FileName.EndsWith("xls") || eSowfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(eSowfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    eSowfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InserteSowdata(filepath, filename);
                    ViewBag.Error18 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error18 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult eSowNewImport(HttpPostedFileBase eSownewfile)
        {
            if (eSownewfile.ContentLength == 0)
            {
                ViewBag.Error19 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (eSownewfile.FileName.EndsWith("xls") || eSownewfile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(eSownewfile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    eSownewfile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InserteSowNewdata(filepath, filename);
                    ViewBag.Error19 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error19 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }
        [HttpPost]
        public ActionResult NPSPDF(HttpPostedFileBase npspdffile)
        {
            if (npspdffile.ContentLength == 0)
            {
                ViewBag.Error16 = "Please select the Pdf File<br>";
                return View("Index");
            }
            else
            {
                if (npspdffile.FileName.EndsWith("pdf"))
                {
                    //String path_name = "~/PDF/";
                    //var pdfPath = Path.Combine(Server.MapPath(path_name));
                    //var formFieldMap = PDFHelper.GetFormFieldNames(pdfPath);

                    //string username = "Test";
                    //string password = "12345";
                    //String file_name_pdf = "Test.pdf";

                    //var pdfContents = PDFHelper.GeneratePDF(pdfPath, formFieldMap);

                    //File.WriteAllBytes(Path.Combine(pdfPath, file_name_pdf), pdfContents);

                    string filename = "NPS" + Path.GetExtension(npspdffile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    try
                    {
                        npspdffile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                        DateTime TodaysDate = DateTime.Now;
                        entity.ReportUpdatedOns.Add(
                            new ReportUpdatedOn
                            {
                                ReportName = "NPS",
                                UpdatedOn = TodaysDate,
                            });
                        entity.SaveChanges();
                        ViewBag.Error16 = "Success";
                    }
                    catch(Exception e)
                    {
                        ViewBag.Error16 = e;

                    }
                    //InsertPDDdata(filepath, filename);
                    return View("Index");
                }
                else
                {
                    ViewBag.Error16 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }

        [HttpPost]
        public ActionResult JiraData(HttpPostedFileBase jirafile)
        {
            if (jirafile.ContentLength == 0)
            {
                ViewBag.Error18 = "Please select the Excel File<br>";
                return View("Index");
            }
            else
            {
                if (jirafile.FileName.EndsWith("xls") || jirafile.FileName.EndsWith("xlsx"))
                {
                    string filename = Guid.NewGuid() + Path.GetExtension(jirafile.FileName);
                    string filepath = "/excelfolder/" + filename;
                    jirafile.SaveAs(Path.Combine(Server.MapPath("/excelfolder"), filename));
                    InsertJiradata(filepath, filename);
                    ViewBag.Error18 = "Success";
                    return View("Index");
                }
                else
                {
                    ViewBag.Error18 = "File Type is Incorrect<br>";
                    return View("Index");
                }
            }
        }

        public void InsertJiradata(string filepath, string filename)
        {
            string fullpath = Server.MapPath("/excelfolder/") + filename;
            ExcelConn(fullpath);
            string query = string.Format("Select * from [{0}]", "JiraData$");
            OleDbCommand Ecom = new OleDbCommand(query, Econ);
            Econ.Open();
            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            Econ.Close();
            oda.Fill(ds);
            DataTable dt = ds.Tables[0];
            SqlBulkCopy objbulk = new SqlBulkCopy(con);
            objbulk.DestinationTableName = "JiraData";
            objbulk.ColumnMappings.Add("Summary", "Summary");
            objbulk.ColumnMappings.Add("Assignee", "Assignee");
            objbulk.ColumnMappings.Add("Assignee Id", "Assignee Id");
            objbulk.ColumnMappings.Add("Created", "Created");
            objbulk.ColumnMappings.Add("Description", "Description");
            objbulk.ColumnMappings.Add("Custom field (Flight Planned Go Live Date)", "Custom field (Flight Planned Go Live Date)");
            objbulk.ColumnMappings.Add("Custom field (Flight Project Complete Date)", "Custom field (Flight Project Complete Date)");
            objbulk.ColumnMappings.Add("Custom field (Flight Project Start Date)", "Custom field (Flight Project Start Date)");
            objbulk.ColumnMappings.Add("Custom field (Flight Trx Monthly)", "Custom field (Flight Trx Monthly)");
            objbulk.ColumnMappings.Add("Custom field (GDS)", "Custom field (GDS)");
            objbulk.ColumnMappings.Add("Custom field (GUID Client Top)", "Custom field (GUID Client Top)");
            objbulk.ColumnMappings.Add("Custom field (Hotel Planned Go Live Date)", "Custom field (Hotel Planned Go Live Date)");
            objbulk.ColumnMappings.Add("Custom field (Hotel Project Complete Date)", "Custom field (Hotel Project Complete Date)");
            objbulk.ColumnMappings.Add("Custom field (Hotel Project Start Date)", "Custom field (Hotel Project Start Date)");
            objbulk.ColumnMappings.Add("Custom field (Hotel Trx Monthly)", "Custom field (Hotel Trx Monthly)");
            objbulk.ColumnMappings.Add("Custom field (Implementation Type)", "Custom field (Implementation Type)");
            objbulk.ColumnMappings.Add("Issue Type", "Issue Type");
            objbulk.ColumnMappings.Add("Last Viewed", "Last Viewed");
            objbulk.ColumnMappings.Add("Custom field (Market (DIGI))", "Custom field (Market (DIGI))");
            objbulk.ColumnMappings.Add("Custom field (OBT Contract)", "Custom field (OBT Contract)");
            objbulk.ColumnMappings.Add("Custom field (OBT Used)", "Custom field (OBT Used)");
            objbulk.ColumnMappings.Add("Priority", "Priority");
            objbulk.ColumnMappings.Add("Custom field (Project Contact)", "Custom field (Project Contact)");
            objbulk.ColumnMappings.Add("Status", "Status");
            objbulk.ColumnMappings.Add("Status Category", "Status Category");
            objbulk.ColumnMappings.Add("Updated", "Updated");
            con.Open();
            string s = "Truncate Table JiraData";
            SqlCommand Com = new SqlCommand(s, con);
            Com.ExecuteNonQuery();
            objbulk.BatchSize = 100000;
            objbulk.BulkCopyTimeout = 0;
            objbulk.WriteToServer(dt);
            con.Close();
        }


    }
}