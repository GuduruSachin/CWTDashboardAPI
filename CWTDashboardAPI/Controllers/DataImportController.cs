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
using System.Threading.Tasks;
using System.Net.Mail;
using System.Net.Mime;

namespace CWTDashboardAPI.Controllers
{
    public class DataImportController : Controller
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


        // iMeet Data inserting in DataiMeet Table
        //[HttpGet]
        public ActionResult iMeetImport()
        {
            try
            {
                string downloadsPath = ConfigurationManager.AppSettings["ServerDownloadsFolder"];
                // Use a folder inside your web app
                // Get the latest Excel file starting with "clr" modified today
                var latestFile = Directory.GetFiles(downloadsPath, "clr_imeet_3_0*.xlsx", SearchOption.TopDirectoryOnly)
                        .Where(file => System.IO.File.GetLastWriteTime(file).Date == DateTime.Today)
                        .OrderByDescending(file => System.IO.File.GetLastWriteTime(file))
                        .FirstOrDefault();
                if (latestFile == null)
                {
                    ViewBag.iMeet_Error = "No matching Excel file found in Downloads folder.";
                    return View("Index");
                }

                // Copy the file to excelfolder with a unique name
                string filename = Guid.NewGuid() + Path.GetExtension(latestFile);
                string excelfolderPath = Server.MapPath("/excelfolder");
                if (!Directory.Exists(excelfolderPath))
                    Directory.CreateDirectory(excelfolderPath);

                string destFilePath = Path.Combine(excelfolderPath, filename);
                System.IO.File.Copy(latestFile, destFilePath, true);

                // Call your existing method with the copied file
            
                InsertDataiMeet(destFilePath, filename);
                ViewBag.iMeet_Error = "Data imported successfully from file: " + filename;
                return View("Index");
            }
            catch (Exception ex)
            {
                // Log the error if needed
                ViewBag.iMeet_Error = "Error: " + ex.Message;
                return View("Index");
            }
        }
        public void InsertDataiMeet(string filepath, string filename)
        {
            ExcelConn(filepath);

            Econ.Open();

            // Dynamically get the first worksheet name
            DataTable schemaTable = Econ.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (schemaTable.Rows.Count == 0)
                throw new Exception("No worksheet found in Excel file.");

            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();

            string query = $"SELECT * FROM [{sheetName}]";
            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            oda.Fill(ds);
            Econ.Close();
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                throw new Exception("No records found in Excel file.");
            int rowsToUpload = dt.Rows.Count;
            // Upload using SqlBulkCopy
            SqlBulkCopy objbulk = new SqlBulkCopy(con)
            {
                DestinationTableName = "DataiMeet",
                BatchSize = 1000,
                BulkCopyTimeout = 120
            };

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
            objbulk.ColumnMappings.Add("Milestone: ELT Client delay description", "EltClientDelayDescription");

            objbulk.NotifyAfter = rowsToUpload;  // fire event once after all rows copied
            int totalRowsCopied = 0;

            objbulk.SqlRowsCopied += (sender, e) =>
            {
                totalRowsCopied = (int)e.RowsCopied;
                ViewBag.iMeet_Error = $"Copied {totalRowsCopied} rows";
            };
            con.Open();

            // Clear existing data
            SqlCommand Com = new SqlCommand("TRUNCATE TABLE DataiMeet", con);
            Com.ExecuteNonQuery();

            // Import data
            objbulk.WriteToServer(dt);
            con.Close();

            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "iMeetData",
                    UpdatedOn = TodayDate,
                    AvailableRows = rowsToUpload,
                    UploadedRows = totalRowsCopied
                });
            entity.SaveChanges();
        }


        //Roles Data Importing in Roles Table
        //[HttpGet]
        public ActionResult RolesImport()
        {
            try
            {
                string downloadsPath = ConfigurationManager.AppSettings["ServerDownloadsFolder"];

                // Get the latest Excel file starting with "clr" modified today
                var latestFile = Directory.GetFiles(downloadsPath, "roles_3_0*.xlsx", SearchOption.TopDirectoryOnly)
                        .Where(file => System.IO.File.GetLastWriteTime(file).Date == DateTime.Today)
                        .OrderByDescending(file => System.IO.File.GetLastWriteTime(file))
                        .FirstOrDefault();
                if (latestFile == null)
                {
                    ViewBag.Roles_Error = "No matching Excel file found in Downloads folder.";
                    return View("Index");
                }

                // Copy the file to excelfolder with a unique name
                string filename = Guid.NewGuid() + Path.GetExtension(latestFile);
                string excelfolderPath = Server.MapPath("/excelfolder");
                if (!Directory.Exists(excelfolderPath))
                    Directory.CreateDirectory(excelfolderPath);

                string destFilePath = Path.Combine(excelfolderPath, filename);
                System.IO.File.Copy(latestFile, destFilePath, true);

                // Call your existing method with the copied file
            
                InsertRoles(destFilePath, filename);
                ViewBag.Roles_Error = "Data imported successfully from file: " + filename;
                return View("Index");
            }
            catch (Exception e)
            {
                ViewBag.Roles_Error = e.Message;
                return View("Index");
            };

        }
        public void InsertRoles(string filepath, string filename)
        {
            ExcelConn(filepath);

            Econ.Open();

            // Dynamically get the first worksheet name
            DataTable schemaTable = Econ.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (schemaTable.Rows.Count == 0)
                throw new Exception("No worksheet found in Excel file.");

            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();

            string query = $"SELECT * FROM [{sheetName}]";
            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            oda.Fill(ds);
            Econ.Close();
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                throw new Exception("No records found in Excel file.");
            int rowsToUpload = dt.Rows.Count;
            // Upload using SqlBulkCopy
            SqlBulkCopy objbulk = new SqlBulkCopy(con)
            {
                DestinationTableName = "Roles",
                BatchSize = 1000,
                BulkCopyTimeout = 120
            };

            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("Global Project Manager", "Global Project Manager");
            objbulk.ColumnMappings.Add("APAC Implementation Lead", "APAC Implementation Lead");
            objbulk.ColumnMappings.Add("LATAM Project Manager", "LATAM Project Manager");
            objbulk.ColumnMappings.Add("NORAM Project Manager", "NORAM Project Manager");
            objbulk.ColumnMappings.Add("EMEA Project Manager", "EMEA Project Manager");
            objbulk.ColumnMappings.Add("Global Ditgital OBT Lead", "Global CIS OBT Lead");
            objbulk.ColumnMappings.Add("APAC Digital OBT Lead", "APAC Digital OBT Lead");
            objbulk.ColumnMappings.Add("LATAM Digital OBT lead", "LATAM CIS OBT Lead");
            objbulk.ColumnMappings.Add("NORAM Digital OBT lead", "NORAM CIS OBT Lead");
            objbulk.ColumnMappings.Add("EMEA Digital OBT lead", "EMEA CIS OBT Lead");
            objbulk.ColumnMappings.Add("Global Digital HR Feed Specialist", "Global CIS HR Feed Specialist");
            objbulk.ColumnMappings.Add("Global Digital Portrait lead", "Global CIS Portrait Lead");
            objbulk.ColumnMappings.Add("APAC Digital Portrait Lead", "APAC Digital Portrait Lead");
            objbulk.ColumnMappings.Add("LATAM Digital Portrait Lead", "LATAM CIS Portrait Lead");
            objbulk.ColumnMappings.Add("NORAM Digital Portrait Lead", "NORAM CIS Portrait Lead");
            objbulk.ColumnMappings.Add("EMEA Digital Portrait Lead", "EMEA CIS Portrait Lead");
            objbulk.ColumnMappings.Add("Local IN CIS Lead", "Local IN CIS Lead");
            objbulk.ColumnMappings.Add("Global DQS Lead", "Global CIS DQS Lead");
            //objbulk.ColumnMappings.Add("Global CIS HR Feed Specialist", "Global CIS HR Feed Specialist");
            //objbulk.ColumnMappings.Add("NORAM CIS Portrait Lead", "NORAM CIS Portrait Lead");
            objbulk.ColumnMappings.Add("APAC DQS", "APAC_DQS");
            objbulk.ColumnMappings.Add("DQS Import", "DQS_Import");
            objbulk.ColumnMappings.Add("DQS Support", "DQS_Support");
            objbulk.ColumnMappings.Add("LATAM DQS", "LATAM_DQS");
            objbulk.ColumnMappings.Add("NORAM DQS", "NORAM_DQS");
            objbulk.ColumnMappings.Add("DQS Operations", "DQS_Operations");
            objbulk.ColumnMappings.Add("Local Implementation Lead Canada", "LocalImplementationLeadCanada");
            objbulk.ColumnMappings.Add("Local Implementation Lead United States", "LocalImplementationLeadUS");

            objbulk.NotifyAfter = rowsToUpload;  // fire event once after all rows copied
            int totalRowsCopied = 0;

            objbulk.SqlRowsCopied += (sender, e) =>
            {
                totalRowsCopied = (int)e.RowsCopied;
                ViewBag.Roles_Error = $"Copied {totalRowsCopied} rows";
            };
            con.Open();

            // Clear existing data
            SqlCommand Com = new SqlCommand("TRUNCATE TABLE Roles", con);
            Com.ExecuteNonQuery();

            // Import data
            objbulk.WriteToServer(dt);
            con.Close();

            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "RolesData",
                    UpdatedOn = TodayDate,
                    AvailableRows = rowsToUpload,
                    UploadedRows = totalRowsCopied
                });
            entity.SaveChanges();
        }



        // Assigne Persons Data inserting in AssignePersons Table
        //[HttpGet]
        public ActionResult AssignePersonsImport()
        {
            try
            {
                string downloadsPath = ConfigurationManager.AppSettings["ServerDownloadsFolder"];

                // Get the latest Excel file starting with "clr" modified today
                var latestFile = Directory.GetFiles(downloadsPath, "imeet_assignee_persons_3_0*.xlsx", SearchOption.TopDirectoryOnly)
                        .Where(file => System.IO.File.GetLastWriteTime(file).Date == DateTime.Today)
                        .OrderByDescending(file => System.IO.File.GetLastWriteTime(file))
                        .FirstOrDefault();
                if (latestFile == null)
                {
                    ViewBag.Assigne_Persons_Error = "No matching Excel file found in Downloads folder.";
                    return View("Index");
                }

                // Copy the file to excelfolder with a unique name
                string filename = Guid.NewGuid() + Path.GetExtension(latestFile);
                string excelfolderPath = Server.MapPath("/excelfolder");
                if (!Directory.Exists(excelfolderPath))
                    Directory.CreateDirectory(excelfolderPath);

                string destFilePath = Path.Combine(excelfolderPath, filename);
                System.IO.File.Copy(latestFile, destFilePath, true);

                // Call your existing method with the copied file
            
                InsertAssignePersons(destFilePath, filename);
                ViewBag.Assigne_Persons_Error = "Data imported successfully from file: " + filename;
            }
            catch (Exception e)
            {
                ViewBag.Assigne_Persons_Error = e.Message;
            };

            return View("Index");
        }
        public void InsertAssignePersons(string filepath, string filename)
        {
            ExcelConn(filepath);

            Econ.Open();

            // Dynamically get the first worksheet name
            DataTable schemaTable = Econ.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (schemaTable.Rows.Count == 0)
                throw new Exception("No worksheet found in Excel file.");

            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();

            string query = $"SELECT * FROM [{sheetName}]";
            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            oda.Fill(ds);
            Econ.Close();
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                throw new Exception("No records found in Excel file.");
            int rowsToUpload = dt.Rows.Count;
            // Upload using SqlBulkCopy
            SqlBulkCopy objbulk = new SqlBulkCopy(con)
            {
                DestinationTableName = "AssignePersons",
                BatchSize = 1000,
                BulkCopyTimeout = 120
            };
            
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

            objbulk.NotifyAfter = rowsToUpload;  // fire event once after all rows copied
            int totalRowsCopied = 0;

            objbulk.SqlRowsCopied += (sender, e) =>
            {
                totalRowsCopied = (int)e.RowsCopied;
                ViewBag.Assigne_Persons_Error = $"Copied {totalRowsCopied} rows";
            };
            con.Open();

            // Clear existing data
            SqlCommand Com = new SqlCommand("TRUNCATE TABLE AssignePersons", con);
            Com.ExecuteNonQuery();

            // Import data
            objbulk.WriteToServer(dt);
            con.Close();
            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "AssignePersonsData",
                    UpdatedOn = TodayDate,
                    AvailableRows = rowsToUpload,
                    UploadedRows = totalRowsCopied
                });
            entity.SaveChanges();
        }


        // Project Due Date Data inserting in ProjectDueDate Table
        //[HttpGet]
        public ActionResult ProjectDueDateImport()
        {
            try
            {
                string downloadsPath = ConfigurationManager.AppSettings["ServerDownloadsFolder"];

                // Get the latest Excel file starting with "clr" modified today
                var latestFile = Directory.GetFiles(downloadsPath, "clr_projectduedate_3_0*.xlsx", SearchOption.TopDirectoryOnly)
                        .Where(file => System.IO.File.GetLastWriteTime(file).Date == DateTime.Today)
                        .OrderByDescending(file => System.IO.File.GetLastWriteTime(file))
                        .FirstOrDefault();
                if (latestFile == null)
                {
                    ViewBag.Project_Due_Date_Error = "No matching Excel file found in Downloads folder.";
                    return View("Index");
                }

                // Copy the file to excelfolder with a unique name
                string filename = Guid.NewGuid() + Path.GetExtension(latestFile);
                string excelfolderPath = Server.MapPath("/excelfolder");
                if (!Directory.Exists(excelfolderPath))
                    Directory.CreateDirectory(excelfolderPath);

                string destFilePath = Path.Combine(excelfolderPath, filename);
                System.IO.File.Copy(latestFile, destFilePath, true);

                // Call your existing method with the copied file
            
                InsertProjectDueDate(destFilePath, filename);
                ViewBag.Project_Due_Date_Error = "Data imported successfully from file: " + filename;
            }
            catch (Exception e)
            {
                ViewBag.Project_Due_Date_Error = e.Message;
            };

            return View("Index");
        }
        public void InsertProjectDueDate(string filepath, string filename)
        {
            ExcelConn(filepath);

            Econ.Open();

            // Dynamically get the first worksheet name
            DataTable schemaTable = Econ.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (schemaTable.Rows.Count == 0)
                throw new Exception("No worksheet found in Excel file.");

            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();

            string query = $"SELECT * FROM [{sheetName}]";
            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            oda.Fill(ds);
            Econ.Close();
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                throw new Exception("No records found in Excel file.");
            int rowsToUpload = dt.Rows.Count;
            // Upload using SqlBulkCopy
            SqlBulkCopy objbulk = new SqlBulkCopy(con)
            {
                DestinationTableName = "ProjectDueDate",
                BatchSize = 1000,
                BulkCopyTimeout = 120
            };

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

            objbulk.NotifyAfter = rowsToUpload;  // fire event once after all rows copied
            int totalRowsCopied = 0;

            objbulk.SqlRowsCopied += (sender, e) =>
            {
                totalRowsCopied = (int)e.RowsCopied;
                ViewBag.Project_Due_Date_Error = $"Copied {totalRowsCopied} rows";
            };
            con.Open();

            // Clear existing data
            SqlCommand Com = new SqlCommand("TRUNCATE TABLE ProjectDueDate", con);
            Com.ExecuteNonQuery();

            // Import data
            objbulk.WriteToServer(dt);
            con.Close();
            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "ProjectDueDateData",
                    UpdatedOn = TodayDate,
                    AvailableRows = rowsToUpload,
                    UploadedRows = totalRowsCopied
                });
            entity.SaveChanges();
        }



        // Implementation Project Status Data inserting in IMPS Table
        //[HttpGet]
        public ActionResult IMPSImport()
        {
            try
            {
                string downloadsPath = ConfigurationManager.AppSettings["ServerDownloadsFolder"];

                // Get the latest Excel file starting with "clr" modified today
                var latestFile = Directory.GetFiles(downloadsPath, "implementation_project_status_report_3_0*.xlsx", SearchOption.TopDirectoryOnly)
                        .Where(file => System.IO.File.GetLastWriteTime(file).Date == DateTime.Today)
                        .OrderByDescending(file => System.IO.File.GetLastWriteTime(file))
                        .FirstOrDefault();
                if (latestFile == null)
                {
                    ViewBag.IMPS_Error = "No matching Excel file found in Downloads folder.";
                    return View("Index");
                }

                // Copy the file to excelfolder with a unique name
                string filename = Guid.NewGuid() + Path.GetExtension(latestFile);
                string excelfolderPath = Server.MapPath("/excelfolder");
                if (!Directory.Exists(excelfolderPath))
                    Directory.CreateDirectory(excelfolderPath);

                string destFilePath = Path.Combine(excelfolderPath, filename);
                System.IO.File.Copy(latestFile, destFilePath, true);

                // Call your existing method with the copied file
            
                InsertIMPS(destFilePath, filename);
                ViewBag.IMPS_Error = "Data imported successfully from file: " + filename;
            }
            catch (Exception e)
            {
                ViewBag.IMPS_Error = e.Message;
            };

            return View("Index");
        }
        public void InsertIMPS(string filepath, string filename)
        {
            ExcelConn(filepath);

            Econ.Open();

            // Dynamically get the first worksheet name
            DataTable schemaTable = Econ.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (schemaTable.Rows.Count == 0)
                throw new Exception("No worksheet found in Excel file.");

            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();

            string query = $"SELECT * FROM [{sheetName}]";
            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            oda.Fill(ds);
            Econ.Close();
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                throw new Exception("No records found in Excel file.");
            int rowsToUpload = dt.Rows.Count;
            // Upload using SqlBulkCopy
            SqlBulkCopy objbulk = new SqlBulkCopy(con)
            {
                DestinationTableName = "IMPS",
                BatchSize = 1000,
                BulkCopyTimeout = 120
            };

            objbulk.ColumnMappings.Add("Workspace Title", "Workspace Title");
            objbulk.ColumnMappings.Add("Workspace: CRM OPP ID", "Workspace: CRM Customer Row ID");
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

            objbulk.NotifyAfter = rowsToUpload;  // fire event once after all rows copied
            int totalRowsCopied = 0;

            objbulk.SqlRowsCopied += (sender, e) =>
            {
                totalRowsCopied = (int)e.RowsCopied;
                ViewBag.IMPS_Error = $"Copied {totalRowsCopied} rows";
            };
            con.Open();

            // Clear existing data
            SqlCommand Com = new SqlCommand("TRUNCATE TABLE IMPS", con);
            Com.ExecuteNonQuery();

            // Import data
            objbulk.WriteToServer(dt);
            con.Close();
            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "IMPSData",
                    UpdatedOn = TodayDate,
                    AvailableRows = rowsToUpload,
                    UploadedRows = totalRowsCopied
                });
            entity.SaveChanges();
        }



        // Critical Task Over Due Data inserting in CTO Table
        //[HttpGet]
        public ActionResult CTOImport()
        {
            try
            {
                string downloadsPath = ConfigurationManager.AppSettings["ServerDownloadsFolder"];

                // Get the latest Excel file starting with "clr" modified today
                var latestFile = Directory.GetFiles(downloadsPath, "critical_tasks_overdue_summary_3_0*.xlsx", SearchOption.TopDirectoryOnly)
                        .Where(file => System.IO.File.GetLastWriteTime(file).Date == DateTime.Today)
                        .OrderByDescending(file => System.IO.File.GetLastWriteTime(file))
                        .FirstOrDefault();
                if (latestFile == null)
                {
                    ViewBag.CTO_Error = "No matching Excel file found in Downloads folder.";
                    return View("Index");
                }

                // Copy the file to excelfolder with a unique name
                string filename = Guid.NewGuid() + Path.GetExtension(latestFile);
                string excelfolderPath = Server.MapPath("/excelfolder");
                if (!Directory.Exists(excelfolderPath))
                    Directory.CreateDirectory(excelfolderPath);

                string destFilePath = Path.Combine(excelfolderPath, filename);
                System.IO.File.Copy(latestFile, destFilePath, true);

                // Call your existing method with the copied file
            
                InsertCTO(destFilePath, filename);
                ViewBag.CTO_Error = "Data imported successfully from file: " + filename;
            }
            catch (Exception e)
            {
                ViewBag.CTO_Error = e.Message;
            };

            return View("Index");
        }
        public void InsertCTO(string filepath, string filename)
        {
            ExcelConn(filepath);

            Econ.Open();

            // Dynamically get the first worksheet name
            DataTable schemaTable = Econ.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (schemaTable.Rows.Count == 0)
                throw new Exception("No worksheet found in Excel file.");

            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();

            string query = $"SELECT * FROM [{sheetName}]";
            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            oda.Fill(ds);
            Econ.Close();
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                throw new Exception("No records found in Excel file.");
            int rowsToUpload = dt.Rows.Count;
            // Upload using SqlBulkCopy
            SqlBulkCopy objbulk = new SqlBulkCopy(con)
            {
                DestinationTableName = "CTO",
                BatchSize = 1000,
                BulkCopyTimeout = 120
            };

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
            objbulk.ColumnMappings.Add("Milestone: CRM Revenue ID #", "RevenurID");
            objbulk.ColumnMappings.Add("Task: Task Type", "TaskType");
            objbulk.ColumnMappings.Add("Task: Task Record ID Key", "TaskRecordIdKey");

            objbulk.NotifyAfter = rowsToUpload;  // fire event once after all rows copied
            int totalRowsCopied = 0;

            objbulk.SqlRowsCopied += (sender, e) =>
            {
                totalRowsCopied = (int)e.RowsCopied;
                ViewBag.CTO_Error = $"Copied {totalRowsCopied} rows";
            };
            con.Open();

            // Clear existing data
            SqlCommand Com = new SqlCommand("TRUNCATE TABLE CTO", con);
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
                dr2["Milestone Title-Country:  Est Go Live Date"] = (object)(r.Milestone_Title + "-" + r.Milestone__Country + "      " + (r.Milestone_Due_Date == null ? null : (r.Milestone__Region == "APAC" || r.Milestone__Region == "EMEA" ? M_due_date.AddDays(-28).ToShortDateString() : M_due_date.AddDays(-14).ToShortDateString()))) ?? DBNull.Value;
                dr2["Critical Overdue"] = (object)(r.Task_Overdue > 6) ?? DBNull.Value;
                //dr2["Estimated Go Live"] = (object) (r.Milestone_Due_Date == (DateTime?)null ? (DateTime?)null : Convert.ToDateTime(r.Milestone_Due_Date).AddDays(-28)) ?? DBNull.Value;
                dr2["Estimated Go Live"] = (object)(r.Milestone_Due_Date == (DateTime?)null ? (DateTime?)null : (r.Milestone__Region == "APAC" || r.Milestone__Region == "EMEA" ? Convert.ToDateTime(r.Milestone_Due_Date).AddDays(-28).Date : Convert.ToDateTime(r.Milestone_Due_Date).AddDays(-14).Date)) ?? DBNull.Value;
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
            SqlBulkCopy objbulk2 = new SqlBulkCopy(con)
            {
                DestinationTableName = "CTO",
                BatchSize = 1000,
                BulkCopyTimeout = 120
            };
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

            SqlCommand Com2 = new SqlCommand("Truncate Table CTO", con);
            Com2.ExecuteNonQuery();

            objbulk2.WriteToServer(tbl2);
            con.Close();
            // Import data
            DateTime TodayDate = DateTime.Now;
            entity.ReportUpdatedOns.Add(
                new ReportUpdatedOn
                {
                    ReportName = "CTOData",
                    UpdatedOn = TodayDate,
                    AvailableRows = rowsToUpload,
                    UploadedRows = totalRowsCopied
                });
            entity.SaveChanges();
        }


        //Get eSow Data and Storing it in DB
        private readonly ApiService _apiService = new ApiService();

        //[HttpGet]
        public async Task<ActionResult> GeteSowData()
        {
            var body = new RequestBody
            {
                MessageObject = new MessageObject
                {
                    queryName = "eSoW Countries",
                    startDate = new DateTime(2020, 01, 01),
                    endDate = null,
                    modifiedDate = new DateTime(2023, 12, 31),
                    onlyWonOrSubmitted = false,
                    preferredOption = true,
                    debug = false,
                    id = null,
                    limit = null,
                    includeTest = false
                }
            };
            try
            {
                var response = await _apiService.PostAsync<eSowResponse>("pat/esow/query", body);
                if (response?.data == null || response.data.Count == 0 || response?.labels == null || response?.labels.Count == 0)
                {
                    throw new Exception("No data received from API.");
                }
                // Step 2: Convert to DataTable using labels
                DataTable table = new DataTable();
                table.Columns.Add("Client", typeof(string));
                table.Columns.Add("id", typeof(int));
                table.Columns.Add("eSoWRef", typeof(string));
                table.Columns.Add("crmOpportunityId", typeof(double));
                table.Columns.Add("createdOn", typeof(string));
                //table.Columns.Add("modifiedOn", typeof(string));
                table.Columns.Add("ClientType", typeof(string));
                table.Columns.Add("AccountCategory", typeof(string));
                table.Columns.Add("ProposalType", typeof(string));
                table.Columns.Add("AccountOwner", typeof(string));
                table.Columns.Add("SoWOwner", typeof(string));
                table.Columns.Add("SelfService", typeof(string));
                table.Columns.Add("eSoWStatus", typeof(string));
                table.Columns.Add("CRMStatus", typeof(string));
                table.Columns.Add("implementationReady", typeof(string));
                table.Columns.Add("Option", typeof(string));
                table.Columns.Add("CountryName", typeof(string));
                table.Columns.Add("serviceConfig", typeof(float));
                table.Columns.Add("Team", typeof(string));
                table.Columns.Add("configLoc", typeof(string));
                table.Columns.Add("configName", typeof(string));
                table.Columns.Add("afterHours", typeof(string));
                table.Columns.Add("GDS", typeof(string));
                table.Columns.Add("OBT", typeof(string));
                table.Columns.Add("OBTAdoptionRate", typeof(string));
                table.Columns.Add("DirectReseller", typeof(string));

                foreach (var item in response.data)
                {
                    double? crmOpportunityId = null;
                    if (!string.IsNullOrWhiteSpace(item.crmOpportunityId) &&
                        double.TryParse(item.crmOpportunityId.Trim(), out double parsedcrmOpportunityId))
                    {
                        crmOpportunityId = parsedcrmOpportunityId;
                    }
                    double? serviceConfig = null;
                    if (!string.IsNullOrWhiteSpace(item.serviceConfig) &&
                        double.TryParse(item.serviceConfig.Trim(), out double parsedserviceConfig))
                    {
                        serviceConfig = parsedserviceConfig;
                    }
                    string OBTAdoptionRate = item.OBTAdoptionRate?.ToString()?.Trim();
                    string column4Formatted = null;
                    if (!string.IsNullOrWhiteSpace(OBTAdoptionRate))
                    {
                        if (OBTAdoptionRate.Contains("%"))
                        {
                            // Already in percent format, use as-is
                            column4Formatted = OBTAdoptionRate;
                        }
                        else if (OBTAdoptionRate.Contains(","))
                        {
                            // Comma-separated values
                            var parts = OBTAdoptionRate.Split(',');
                            var transformedParts = new List<string>();

                            foreach (var part in parts)
                            {
                                if (double.TryParse(part.Trim(), out double val))
                                {
                                    transformedParts.Add((val * 100).ToString("0.##") + "%");
                                }
                            }

                            if (transformedParts.Count > 0)
                                column4Formatted = string.Join(",", transformedParts);
                        }
                        else
                        {
                            // Single numeric value
                            if (double.TryParse(OBTAdoptionRate, out double val))
                            {
                                column4Formatted = (val * 100).ToString("0.##") + "%";
                            }
                        }
                    }
                    table.Rows.Add(
                        item.Client ?? (object)DBNull.Value,
                        item.id ?? (object)DBNull.Value,
                        item.eSoWRef ?? (object)DBNull.Value,
                        crmOpportunityId.HasValue ? (object)crmOpportunityId.Value : DBNull.Value,
                        item.createdOn ?? (object)DBNull.Value,
                        //item.modifiedOn ?? (object)DBNull.Value,
                        item.ClientType ?? (object)DBNull.Value,
                        item.AccountCategory ?? (object)DBNull.Value,
                        item.ProposalType ?? (object)DBNull.Value,
                        item.AccountOwner ?? (object)DBNull.Value,
                        item.SoWOwner ?? (object)DBNull.Value,
                        item.SelfService ?? (object)DBNull.Value,
                        item.eSoWStatus ?? (object)DBNull.Value,
                        item.CRMStatus ?? (object)DBNull.Value,
                        item.implementationReady ?? (object)DBNull.Value,
                        item.Option ?? (object)DBNull.Value,
                        item.CountryName ?? (object)DBNull.Value,
                        serviceConfig.HasValue ? (object)serviceConfig.Value : DBNull.Value,
                        item.Team ?? (object)DBNull.Value,
                        item.configLoc ?? (object)DBNull.Value,
                        item.configName ?? (object)DBNull.Value,
                        item.afterHours ?? (object)DBNull.Value,
                        item.GDS ?? (object)DBNull.Value,
                        item.OBT ?? (object)DBNull.Value,
                        string.IsNullOrWhiteSpace(column4Formatted) ? (object)DBNull.Value : column4Formatted,
                        item.DirectReseller ?? (object)DBNull.Value
                        //item.Change ?? (object)DBNull.Value
                    );
                }

                int rowsToUpload = table.Rows.Count;
                SqlBulkCopy objbulk = new SqlBulkCopy(con)
                {
                    DestinationTableName = "esowNewData",
                    BatchSize = 1000,
                    BulkCopyTimeout = 120
                };
                objbulk.ColumnMappings.Add("Client", "Client");
                objbulk.ColumnMappings.Add("id", "id");
                objbulk.ColumnMappings.Add("eSoWRef", "eSoW Ref");
                objbulk.ColumnMappings.Add("crmOpportunityId", "crmOpportunityId");
                objbulk.ColumnMappings.Add("createdOn", "createdOn");
                objbulk.ColumnMappings.Add("ClientType", "Client Type");
                objbulk.ColumnMappings.Add("AccountCategory", "Account Category");
                objbulk.ColumnMappings.Add("ProposalType", "Proposal Type");
                objbulk.ColumnMappings.Add("AccountOwner", "Account Owner");
                objbulk.ColumnMappings.Add("SoWOwner", "SoW Owner");
                objbulk.ColumnMappings.Add("SelfService", "Self Service");
                objbulk.ColumnMappings.Add("eSoWStatus", "eSoW Status");
                objbulk.ColumnMappings.Add("CRMStatus", "CRM Status");
                objbulk.ColumnMappings.Add("implementationReady", "implementationReady");
                objbulk.ColumnMappings.Add("Option", "Option");
                objbulk.ColumnMappings.Add("CountryName", "Country Name");
                objbulk.ColumnMappings.Add("serviceConfig", "serviceConfig");
                objbulk.ColumnMappings.Add("Team", "Team");
                objbulk.ColumnMappings.Add("configLoc", "configLoc");
                objbulk.ColumnMappings.Add("configName", "configName");
                objbulk.ColumnMappings.Add("afterHours", "afterHours");
                objbulk.ColumnMappings.Add("GDS", "GDS");
                objbulk.ColumnMappings.Add("OBT", "OBT");
                objbulk.ColumnMappings.Add("OBTAdoptionRate", "OBT Adoption Rate");
                objbulk.ColumnMappings.Add("DirectReseller", "Direct/Reseller");
                objbulk.NotifyAfter = rowsToUpload;  // fire event once after all rows copied
                int totalRowsCopied = 0;

                objbulk.SqlRowsCopied += (sender, e) =>
                {
                    totalRowsCopied = (int)e.RowsCopied;
                    ViewBag.eSowApiError = $"Copied {totalRowsCopied} rows";
                };
                con.Open();
                string s = "Truncate Table esowNewData";
                SqlCommand Com = new SqlCommand(s, con);
                Com.ExecuteNonQuery();
                objbulk.WriteToServer(table);
                con.Close();
                // Import data
                DateTime TodayDate = DateTime.Now;
                entity.ReportUpdatedOns.Add(
                    new ReportUpdatedOn
                    {
                        ReportName = "eSowData",
                        UpdatedOn = TodayDate,
                        AvailableRows = rowsToUpload,
                        UploadedRows = totalRowsCopied
                    });
                entity.SaveChanges();
                ViewBag.eSowApiError = $"Data Success";
            }
            catch (HttpRequestException ex)
            {
                // Log this somewhere
                ViewBag.eSowApiError = $"Request failed: {ex.Message}";
            }
            catch (Exception ex)
            {
                ViewBag.eSowApiError = $"Unexpected error: {ex.Message}";
            }
            return View("Index");
        }

        //Sending a Mail regarding the Reports Update Status
        //[HttpGet]
        public async Task<ActionResult> SendReportsUpdatedSummary()
        {
            var today = DateTime.Today; // today at 00:00:00
            var tomorrow = today.AddDays(1); // next day at 00:00:00

            var reports_count = entity.ReportUpdatedOns
                             .Where(x => x.UpdatedOn >= today && x.UpdatedOn < tomorrow)
                             .Count();
            string Body;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("Implementationsupport@mycwt.com");
            mail.To.Add(new MailAddress("AChopra@mycwt.com"));
            mail.CC.Add(new MailAddress("HGourani@mycwt.com"));
            mail.CC.Add(new MailAddress("UGuduru@mycwt.com"));
            //mail.To.Add(new MailAddress("UGuduru@mycwt.com"));
            //mail.CC.Add(new MailAddress("UGuduru@mycwt.com"));
            mail.IsBodyHtml = true;
            mail.Subject = "Automated Daily Report Import Summary";
            var reports = entity.ReportUpdatedOns
                             .Where(x => x.UpdatedOn >= today && x.UpdatedOn < tomorrow && x.ReportName != "CLRAutomated")
                             .ToList();
            var missingReports = new List<string> { "ProjectDueDateData", "CTOData", "iMeetData", "IMPSData", "RolesData", "AssignePersonsData","eSowData", "CRMData" }
                     .Except(reports.Select(r => r.ReportName).Distinct())
                     .ToList();
            string rows = "";
            if (reports_count == 0)
            {
                rows = "<tr>" +
                            "<td colspan=\"4\" style=\"text-align:center;color : red; font-style:italic;\">No files have been uploaded, even though the report generation window has passed.</td>" +
                       "</tr>";
            }
            else
            {
                foreach (var report in reports)
                {
                    rows += $"<tr>" +
                                $"<td>{report.ReportName}</td>" +
                                $"<td>{report.UpdatedOn.ToString("yyyy-MM-dd hh:mm tt")}</td>" +
                                $"<td>{report.AvailableRows}</td>" +
                                $"<td>{report.UploadedRows}</td>" +
                            $"</tr>";
                }
                if (reports_count < 7)
                {
                    rows += $"<tr>" +
                                $"<td colspan=\"4\" style=\"text-align:center;color : red; font-style:italic;\">The following reports are not Uploaded: " + string.Join(", ", missingReports) + ".</td>" +
                            $"</tr>";
                }
            }

            Body = "<html><body style=\"padding:10px;\">" +
                    "<div>Hi Team,</div><br />" +
                    "<div>Please find below, the summary of the daily report imports:</div><br />" +
                    "<table border =\"1\" cellpadding=\"6\" cellspacing=\"0\" style=\"border-collapse: collapse; font-family: Arial, sans-serif;\">" +
                        "<thead style =\"background-color: #f2f2f2;\">" +
                            "<tr>" +
                                "<th> Report Name </ th >" +
                                "<th> Imported On </ th >" +
                                "<th> Rows in Source </ th >" +
                                "<th> Rows Uploaded to DB </ th >" +
                            "</tr> " +
                        "</thead >" +
                        "<tbody >" +
                            rows +
                        "</tbody>" +
                    "</table><br />" +
                    "<div style=\"font-weight:bold;\">Notes:</div>" +
                    "<div>- Any mismatch between source and uploaded rows may indicate validation skips or errors.</div>" +
                    "<div>- Please contact the Automation team if you observe inconsistencies.</div><br />" +
                    "<div>Best regards,</div>" +
                    "<div>Implementation Team</div></body></html>";
            AlternateView av1 = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);
            mail.AlternateViews.Add(av1);
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mta-hub";
            smtp.UseDefaultCredentials = false;
            smtp.Port = 25;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            try
            {
                await smtp.SendMailAsync(mail);
                ViewBag.Summary_Error = "Email sent successfully.";
            }
            catch (SmtpException ex)
            {
                // Handles SMTP-specific errors
                ViewBag.Summary_Error = "SMTP error occurred: " + ex.Message;
            }
            catch (Exception ex)
            {
                // Handles general errors
                ViewBag.Summary_Error = "An error occurred while sending the email: " + ex.Message;
            }
            return View("Index");
        }


        // CRM Data Inserting into Staging CRM Data Table 
        //[HttpGet]
        public ActionResult CRMImport()
        {
            string downloadsPath = ConfigurationManager.AppSettings["ServerDownloadsFolder"];
            // Get the latest Excel file starting with "clr" modified today
            var latestFile = Directory.GetFiles(downloadsPath, "Opportunity_Extract_v4*.xlsx", SearchOption.TopDirectoryOnly)
                    .Where(file => System.IO.File.GetLastWriteTime(file).Date == DateTime.Today)
                    .OrderByDescending(file => System.IO.File.GetLastWriteTime(file))
                    .FirstOrDefault();
            if (latestFile == null)
            {
                ViewBag.CRM_Error = "No matching Excel file found in Downloads folder.";
                return View("Index");
            }
            // Copy the file to excelfolder with a unique name
            string filename = Guid.NewGuid() + Path.GetExtension(latestFile);
            string excelfolderPath = Server.MapPath("/excelfolder");
            if (!Directory.Exists(excelfolderPath))
                Directory.CreateDirectory(excelfolderPath);
            string destFilePath = Path.Combine(excelfolderPath, filename);
            System.IO.File.Copy(latestFile, destFilePath, true);
            // Call your existing method with the copied file
            try
            {
                InsertCRM(destFilePath, filename);
                ViewBag.CRM_Error = "Data imported successfully from file: " + filename;
            }
            catch (Exception e)
            {
                ViewBag.CRM_Error = "Import failed: " + e.Message;
            };
            return View("Index");
        }
        public void InsertCRM(string filepath, string filename)
        {
            ExcelConn(filepath);
            Econ.Open();
            // Dynamically get the first worksheet name
            DataTable schemaTable = Econ.GetOleDbSchemaTable(OleDbSchemaGuid.Tables, null);
            if (schemaTable.Rows.Count == 0)
                throw new Exception("No worksheet found in Excel file.");

            string sheetName = schemaTable.Rows[0]["TABLE_NAME"].ToString();

            string query = $"SELECT * FROM [{sheetName}]";
            OleDbCommand Ecom = new OleDbCommand(query, Econ);

            DataSet ds = new DataSet();
            OleDbDataAdapter oda = new OleDbDataAdapter(query, Econ);
            oda.Fill(ds);
            Econ.Close();
            DataTable dt = ds.Tables[0];
            if (dt.Rows.Count == 0)
                throw new Exception("No records found in Excel file.");
            int rowsToUpload = dt.Rows.Count;
            // Upload using SqlBulkCopy
            SqlBulkCopy objbulk = new SqlBulkCopy(con)
            {
                DestinationTableName = "Staging_CRMData",
                BatchSize = 1000,
                BulkCopyTimeout = 120
            };
            foreach (DataColumn col in dt.Columns)
            {
                if(col.ColumnName == "Current Service Configuration")
                {
                    objbulk.ColumnMappings.Add(col.ColumnName, "BT Current Service Configuration");
                }
                else if (col.ColumnName == "Verbal Award Date")
                {
                    objbulk.ColumnMappings.Add(col.ColumnName, "Awarded Date");
                }
                else if (col.ColumnName == "Is Implementation Team support required?")
                {
                    objbulk.ColumnMappings.Add(col.ColumnName, "IsImplementationTeamsupport");
                }
                else
                {
                    objbulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
            }
            //objbulk.ColumnMappings.Add("Account Name", "Account Name");
            //objbulk.ColumnMappings.Add("Account Owner", "Account Owner");
            //objbulk.ColumnMappings.Add("Customer Row ID", "Customer Row ID");
            //objbulk.ColumnMappings.Add("Opportunity ID", "Opportunity ID");
            //objbulk.ColumnMappings.Add("Description", "Description");
            //objbulk.ColumnMappings.Add("BT Current Service Configuration", "BT Current Service Configuration");
            //objbulk.ColumnMappings.Add("Opportunity Name", "Opportunity Name");
            //objbulk.ColumnMappings.Add("Opportunity Owner", "Opportunity Owner");
            //objbulk.ColumnMappings.Add("Close Date", "Close Date");
            //objbulk.ColumnMappings.Add("Sales Stage Name", "Sales Stage Name");
            //objbulk.ColumnMappings.Add("Line Win Probability", "Line Win Probability");
            //objbulk.ColumnMappings.Add("Last Update Date", "Last Update Date");
            //objbulk.ColumnMappings.Add("Opportunity Type", "Opportunity Type");
            //objbulk.ColumnMappings.Add("Next Step", "Next Step");
            ////objbulk.ColumnMappings.Add("Opportunity Scope", "Opportunity Scope");
            //objbulk.ColumnMappings.Add("Opportunity Total Transactions", "Opportunity Total Transactions");
            //objbulk.ColumnMappings.Add("Opportunity Total Volume USD", "Opportunity Total Volume USD");
            //objbulk.ColumnMappings.Add("Awarded Date", "Awarded Date");
            //objbulk.ColumnMappings.Add("LOI Date", "LOI Date");
            //objbulk.ColumnMappings.Add("Country", "Country");
            //objbulk.ColumnMappings.Add("Ownership (Revenue)", "Ownership (Revenue)");
            //objbulk.ColumnMappings.Add("Region (Revenue)", "Region (Revenue)");
            //objbulk.ColumnMappings.Add("Revenue Total Transactions", "Revenue Total Transactions");
            //objbulk.ColumnMappings.Add("Revenue Total Volume USD", "Revenue Total Volume USD");
            ////objbulk.ColumnMappings.Add("Client Segment", "Client Segment");
            ////objbulk.ColumnMappings.Add("BT Current GDS", "BT Current GDS");
            ////objbulk.ColumnMappings.Add("BT Current Online Booking Tool", "BT Current Online Booking Tool");
            ////objbulk.ColumnMappings.Add("BT Incumbent", "BT Incumbent");
            ////objbulk.ColumnMappings.Add("Opportunity Region", "Opportunity Region");
            ////objbulk.ColumnMappings.Add("Country Scope", "Country Scope");
            ////objbulk.ColumnMappings.Add("Total Awarded Volume USD", "Total Awarded Volume USD");
            //objbulk.ColumnMappings.Add("Total Up-Sell Volume USD", "Total Up-Sell Volume USD");
            //objbulk.ColumnMappings.Add("GDS", "GDS");
            //objbulk.ColumnMappings.Add("OBT", "OBT");
            //objbulk.ColumnMappings.Add("Revenue Opportunity Type", "Revenue Opportunity Type");
            //objbulk.ColumnMappings.Add("Revenue Status", "Revenue Status");
            //objbulk.ColumnMappings.Add("Revenue Id", "Revenue Id");
            //objbulk.ColumnMappings.Add("Created Date", "Created Date");
            //objbulk.ColumnMappings.Add("IsImplementationTeamsupport", "IsImplementationTeamsupport");

            objbulk.NotifyAfter = rowsToUpload;  // fire event once after all rows copied
            int totalRowsCopied = 0;

            objbulk.SqlRowsCopied += (sender, e) =>
            {
                totalRowsCopied = (int)e.RowsCopied;
                ViewBag.CRM_Error = $"Copied {totalRowsCopied} rows";
            };
            con.Open();

            // Clear existing data
            SqlCommand Com = new SqlCommand("TRUNCATE TABLE Staging_CRMData", con);
            Com.ExecuteNonQuery();

            // Import data
            objbulk.WriteToServer(dt);
            con.Close();
            UpsertCRMData();
        }

        public void UpsertCRMData()
        {
            string mergeSql = @"
                MERGE INTO CRMData AS Target
                USING (
                    SELECT *
                    FROM Staging_CRMData
                ) AS Source
                ON Target.[Revenue Id] = Source.[Revenue Id]
                WHEN MATCHED THEN
                    UPDATE SET
                        [Account Name] = Source.[Account Name],
                        [Account Owner] = Source.[Account Owner],
                        [Customer Row ID] = Source.[Customer Row ID],
                        [Opportunity ID] = Source.[Opportunity ID],
                        [Description] = Source.[Description],
                        [BT Current Service Configuration] = Source.[BT Current Service Configuration],
                        [Opportunity Name] = Source.[Opportunity Name],
                        [Opportunity Owner] = Source.[Opportunity Owner],
                        [Close Date] = Source.[Close Date],
                        [Sales Stage Name] = Source.[Sales Stage Name],
                        [Line Win Probability] = Source.[Line Win Probability],
                        [Last Update Date] = Source.[Last Update Date],
                        [Opportunity Type] = Source.[Opportunity Type],
                        [Next Step] = Source.[Next Step],
                        [Opportunity Total Transactions] = Source.[Opportunity Total Transactions],
                        [Opportunity Total Volume USD] = Source.[Opportunity Total Volume USD],
                        [Awarded Date] = Source.[Awarded Date],
                        [LOI Date] = Source.[LOI Date],
                        [Country] = Source.[Country],
                        [Ownership (Revenue)] = Source.[Ownership (Revenue)],
                        [Region (Revenue)] = Source.[Region (Revenue)],
                        [Revenue Total Transactions] = Source.[Revenue Total Transactions],
                        [Revenue Total Volume USD] = Source.[Revenue Total Volume USD],
                        [Total Up-Sell Volume USD] = Source.[Total Up-Sell Volume USD],
                        [GDS] = Source.[GDS],
                        [OBT] = Source.[OBT],
                        [Revenue Opportunity Type] = Source.[Revenue Opportunity Type],
                        [Revenue Status] = Source.[Revenue Status],
                        [Created Date] = Source.[Created Date],
                        [IsImplementationTeamsupport] = Source.[IsImplementationTeamsupport]
                WHEN NOT MATCHED BY TARGET THEN
                    INSERT (
                        [Account Name],
                        [Account Owner],
                        [Customer Row ID],
                        [Opportunity ID],
                        [Description],
                        [BT Current Service Configuration],
                        [Opportunity Name],
                        [Opportunity Owner],
                        [Close Date],
                        [Sales Stage Name],
                        [Line Win Probability],
                        [Last Update Date],
                        [Opportunity Type],
                        [Next Step],
                        [Opportunity Total Transactions],
                        [Opportunity Total Volume USD],
                        [Awarded Date],
                        [LOI Date],
                        [Country],
                        [Ownership (Revenue)],
                        [Region (Revenue)],
                        [Revenue Total Transactions],
                        [Revenue Total Volume USD],
                        [Total Up-Sell Volume USD],
                        [GDS],
                        [OBT],
                        [Revenue Opportunity Type],
                        [Revenue Status],
                        [Revenue Id],
                        [Created Date],
                        [IsImplementationTeamsupport]
                    )
                    VALUES (
                        Source.[Account Name],
                        Source.[Account Owner],
                        Source.[Customer Row ID],
                        Source.[Opportunity ID],
                        Source.[Description],
                        Source.[BT Current Service Configuration],
                        Source.[Opportunity Name],
                        Source.[Opportunity Owner],
                        Source.[Close Date],
                        Source.[Sales Stage Name],
                        Source.[Line Win Probability],
                        Source.[Last Update Date],
                        Source.[Opportunity Type],
                        Source.[Next Step],
                        Source.[Opportunity Total Transactions],
                        Source.[Opportunity Total Volume USD],
                        Source.[Awarded Date],
                        Source.[LOI Date],
                        Source.[Country],
                        Source.[Ownership (Revenue)],
                        Source.[Region (Revenue)],
                        Source.[Revenue Total Transactions],
                        Source.[Revenue Total Volume USD],
                        Source.[Total Up-Sell Volume USD],
                        Source.[GDS],
                        Source.[OBT],
                        Source.[Revenue Opportunity Type],
                        Source.[Revenue Status],
                        Source.[Revenue Id],
                        Source.[Created Date],
                        Source.[IsImplementationTeamsupport]
                    );";
            using (SqlCommand cmd = new SqlCommand(mergeSql, con))
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }

            DateTime TodayDate = DateTime.Now;
            using (var entity = new CWTDashboardEntities())
            {
                entity.ReportUpdatedOns.Add(new ReportUpdatedOn { ReportName = "CRMData", UpdatedOn = TodayDate });
                entity.SaveChanges();
            }
        }
    }
}