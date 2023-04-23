using CWTDashboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CWTDashboardAPI.Controllers
{
    public class StageGateController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        StageGateFilters fi = new StageGateFilters();
        int StageGateCount;
        [HttpPost]
        [Route("StageGateFiltersList")]
        public StageGateFilters StageGateFiltersList(StageGate stageGate)
        {
            //var FilterYear = entity.CLRs.Select(a => a.Go_Live_Year).Distinct().OrderByDescending(b => b.Go_Live_Year).ToList();
            var Filteryear = (from a in entity.StageGates
                              where a.Year != null && a.Year != ""
                              select new
                              {
                                  Year = a.Year,
                                  isSelected = true,
                              }).Distinct().OrderBy(x => x.Year);
            var FilterMonths = new List<StageGateMonths>()
            {
                 new StageGateMonths() {Month="Jan",isSelected=true},
                 new StageGateMonths() {Month="Feb",isSelected=true},
                 new StageGateMonths() {Month="Mar",isSelected=true},
                 new StageGateMonths() {Month="Apr",isSelected=true},
                 new StageGateMonths() {Month="May",isSelected=true},
                 new StageGateMonths() {Month="Jun",isSelected=true},
                 new StageGateMonths() {Month="Jul",isSelected=true},
                 new StageGateMonths() {Month="Aug",isSelected=true},
                 new StageGateMonths() {Month="Sep",isSelected=true},
                 new StageGateMonths() {Month="Oct",isSelected=true},
                 new StageGateMonths() {Month="Nov",isSelected=true},
                 new StageGateMonths() {Month="Dec",isSelected=true},
            };
            var FilterTaskStatus = (from a in entity.StageGates
                                    where a.Task_Status != null && a.Task_Status != ""
                                    select new
                                    {
                                        Task_Status = a.Task_Status,
                                        isSelected = true,
                                    }).Distinct().OrderBy(x => x.Task_Status);
            var FilterProjectStatus = (from a in entity.StageGates
                                       where a.Milestone__Project_Status != null && a.Milestone__Project_Status != ""
                                       select new
                                       {
                                           Project_Status = a.Milestone__Project_Status,
                                           isSelected = true,
                                       }).Distinct().OrderBy(x => x.Project_Status);
            fi.code = 200;
            fi.message = "Data Successfull";
            fi.Year = Filteryear;
            fi.Months = FilterMonths;
            fi.TaskStatus = FilterTaskStatus;
            fi.ProjectStatus = FilterProjectStatus;
            return fi;
        }
        string[] SGYears, SG_Months, SGTaskStatus,SGProjectStatus;
        [HttpPost]
        [Route("StageGateDataList1")]
        public Response StageGateDataList1(StageGate stageGate)
        {
            if (stageGate.Year == null || stageGate.Month == null || stageGate.Task_Status == null || stageGate.Milestone__Project_Status == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                SGYears = stageGate.Year.Split(',');
                for (int i = 0; i < SGYears.Count(); i++)
                {
                    if (SGYears[i] == "" || SGYears[i] == "null")
                    {
                        SGYears[i] = null;
                    }
                }
                SG_Months = stageGate.Month.Split(',');
                for (int i = 0; i < SG_Months.Count(); i++)
                {
                    if (SG_Months[i] == "" || SG_Months[i] == "null")
                    {
                        SG_Months[i] = null;
                    }
                }
                SGTaskStatus = stageGate.Task_Status.Split(',');
                for (int i = 0; i < SGTaskStatus.Count(); i++)
                {
                    if (SGTaskStatus[i] == "" || SGTaskStatus[i] == "null")
                    {
                        SGTaskStatus[i] = null;
                    }
                }
                SGProjectStatus = stageGate.Milestone__Project_Status.Split(',');
                for (int i = 0; i < SGProjectStatus.Count(); i++)
                {
                    if (SGProjectStatus[i] == "" || SGProjectStatus[i] == "null")
                    {
                        SGProjectStatus[i] = null;
                    }
                }
                var DataList = (from a in entity.StageGates
                                where a.Task_Title == "Offline Testing Completed" || a.Task_Title == "Online testing completed" || a.Task_Title == "DO NOT DELETE - Offline Testing Completed" || a.Task_Title == "DO NOT DELETE - Online testing completed" || a.Task_Title == "MyCWT Hotel Web-Mobile Testing Completed"
                                where SGYears.Any(val1 => a.Year.Equals(val1))
                                where SG_Months.Any(Val2 => a.Month.Equals(Val2))
                                where SGTaskStatus.Any(Val3 => a.Task_Status.Equals(Val3))
                                where SGProjectStatus.Any(Val3 => a.Milestone__Project_Status.Equals(Val3))
                                group a by a.Milestone__Assignee__Reports_to__Full_Name into g
                                select new
                                {
                                    Client = g.Key ?? "(Blank)",
                                    Task1 = g.Where(x => x.Task_Title == "Online testing completed").Count(),
                                    Task2 = g.Where(x => x.Task_Title == "Offline Testing Completed").Count(),
                                    Task3 = g.Where(x => x.Task_Title == "DO NOT DELETE - Offline Testing Completed").Count(),
                                    Task4 = g.Where(x => x.Task_Title == "DO NOT DELETE - Online testing completed").Count(),
                                    Task5 = g.Where(x => x.Task_Title == "MyCWT Hotel Web-Mobile Testing Completed").Count(),
                                    GrandTotal = g.Count(),
                                }).Distinct().OrderBy(x => x.Client);
                StageGateCount = DataList.AsQueryable().Count();
                if (StageGateCount.ToString() == "" || StageGateCount.ToString() == null || StageGateCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data found";
                    re.Data = null;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = DataList;
                }
            }
            return re;
        }

        string[] SG2_Years, SG2_Months, SG2_TaskStatus, SG2_ProjectStatus;
        [HttpPost]
        [Route("StageGateDataList2")]
        public Response StageGateDataList2(StageGate stageGate)
        {
            if (stageGate.Year == null || stageGate.Month == null || stageGate.Task_Status == null || stageGate.Milestone__Project_Status == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                SG2_Years = stageGate.Year.Split(',');
                for (int i = 0; i < SG2_Years.Count(); i++)
                {
                    if (SG2_Years[i] == "" || SG2_Years[i] == "null")
                    {
                        SG2_Years[i] = null;
                    }
                }
                SG2_Months = stageGate.Month.Split(',');
                for (int i = 0; i < SG2_Months.Count(); i++)
                {
                    if (SG2_Months[i] == "" || SG2_Months[i] == "null")
                    {
                        SG2_Months[i] = null;
                    }
                }
                SG2_TaskStatus = stageGate.Task_Status.Split(',');
                for (int i = 0; i < SG2_TaskStatus.Count(); i++)
                {
                    if (SG2_TaskStatus[i] == "" || SG2_TaskStatus[i] == "null")
                    {
                        SG2_TaskStatus[i] = null;
                    }
                }
                SG2_ProjectStatus = stageGate.Milestone__Project_Status.Split(',');
                for (int i = 0; i < SG2_ProjectStatus.Count(); i++)
                {
                    if (SG2_ProjectStatus[i] == "" || SG2_ProjectStatus[i] == "null")
                    {
                        SG2_ProjectStatus[i] = null;
                    }
                }
                var DataList = (from a in entity.StageGates 
                                where a.Task_Title != "Offline Testing Completed" && a.Task_Title != "Online testing completed" && a.Task_Title != "DO NOT DELETE - Offline Testing Completed" && a.Task_Title != "DO NOT DELETE - Online testing completed" && a.Task_Title != "MyCWT Hotel Web-Mobile Testing Completed"
                                where SG2_Years.Any(val1 => a.Year.Equals(val1))
                                where SG2_Months.Any(Val2 => a.Month.Equals(Val2))
                                where SG2_TaskStatus.Any(Val3 => a.Task_Status.Equals(Val3))
                                where SG2_ProjectStatus.Any(Val3 => a.Milestone__Project_Status.Equals(Val3))
                                group a by a.Milestone__Assignee__Reports_to__Full_Name into g
                                select new
                                {
                                    Client = g.Key ?? "(Blank)",
                                    Task1 = g.Where(x => x.Task_Title == "Stage gate 1 Offline - Profile management").Count(),
                                    Task2 = g.Where(x => x.Task_Title == "Stage gate 1 Online - Profile management").Count(),
                                    Task3 = g.Where(x => x.Task_Title == "Stage gate 2 Offline - Booking and Itinerary distribution").Count(),
                                    Task4 = g.Where(x => x.Task_Title == "Stage gate 2 Online - Booking and Itinerary distribution").Count(),
                                    Task5 = g.Where(x => x.Task_Title == "Stage gate 3 Offline - Ticket Issuance").Count(),
                                    Task6 = g.Where(x => x.Task_Title == "Stage gate 3 Online - Ticket issuance").Count(),
                                    Task7 = g.Where(x => x.Task_Title == "Stage gate 4 Offline - Invoicing").Count(),
                                    Task8 = g.Where(x => x.Task_Title == "Stage gate 4 Online - Invoicing").Count(),
                                    Task9 = g.Where(x => x.Task_Title == "Stage gate 5 Offline and Online - Telephony").Count(),
                                    Task10 = g.Where(x => x.Task_Title == "Stage gate 6 Offline - Fees").Count(),
                                    Task11 = g.Where(x => x.Task_Title == "Stage gate 6 Online - Fees").Count(),
                                    Task12 = g.Where(x => x.Task_Title == "Stage gate 7 Offline - AnalytIQs").Count(),
                                    Task13 = g.Where(x => x.Task_Title == "Stage gate 7 Online - AnalytIQs").Count(),
                                    Task14 = g.Where(x => x.Task_Title == "Stage gate 8 Offline - Data handoff Safety and Security").Count(),
                                    Task15 = g.Where(x => x.Task_Title == "Stage gate 8 Offline - Data handoff Safety and Secutity").Count(),
                                    Task16 = g.Where(x => x.Task_Title == "Stage gate 8 Online - Data handoff Safety and Security").Count(),
                                    Task17 = g.Where(x => x.Task_Title == "Stage gate 9 Offline - Price Optimization").Count(),
                                    Task18 = g.Where(x => x.Task_Title == "Stage gate 9 Offline - Price tracking").Count(),
                                    Task19 = g.Where(x => x.Task_Title == "Stage gate 9 Online - Price Optimization").Count(),
                                    Task20 = g.Where(x => x.Task_Title == "Stage gate 9 Online - Price tracking").Count(),
                                    Task21 = g.Where(x => x.Task_Title == "Sciex - Stage gate 1 Offline - Profile management").Count(),
                                    GrandTotal = g.Count(),
                                }).Distinct().OrderBy(x => x.Client);
                StageGateCount = DataList.AsQueryable().Count();
                if (StageGateCount.ToString() == "" || StageGateCount.ToString() == null || StageGateCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data found";
                    re.Data = null;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = DataList;
                }
            }
            return re;
        }


        string[] SGData_Years, SGData_Months, SGData_TaskStatus, SGData_ProjectStatus, SGData_ReportName;
        [HttpPost]
        [Route("StageGateDataExport")]
        public Response StageGateDataExport(StageGate stageGate)
        {
            if (stageGate.Year == null || stageGate.Month == null || stageGate.Task_Status == null || stageGate.Milestone__Project_Status == null || stageGate.ReportName == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                SGData_Years = stageGate.Year.Split(',');
                for (int i = 0; i < SGData_Years.Count(); i++)
                {
                    if (SGData_Years[i] == "" || SGData_Years[i] == "null")
                    {
                        SGData_Years[i] = null;
                    }
                }
                SGData_Months = stageGate.Month.Split(',');
                for (int i = 0; i < SGData_Months.Count(); i++)
                {
                    if (SGData_Months[i] == "" || SGData_Months[i] == "null")
                    {
                        SGData_Months[i] = null;
                    }
                }
                SGData_TaskStatus = stageGate.Task_Status.Split(',');
                for (int i = 0; i < SGData_TaskStatus.Count(); i++)
                {
                    if (SGData_TaskStatus[i] == "" || SGData_TaskStatus[i] == "null")
                    {
                        SGData_TaskStatus[i] = null;
                    }
                }
                SGData_ProjectStatus = stageGate.Milestone__Project_Status.Split(',');
                for (int i = 0; i < SGData_ProjectStatus.Count(); i++)
                {
                    if (SGData_ProjectStatus[i] == "" || SGData_ProjectStatus[i] == "null")
                    {
                        SGData_ProjectStatus[i] = null;
                    }
                }
                var StageGateDataExport = (from a in entity.StageGates
                                           //where a.ReportName == "OnlineOffline"
                                       where a.ReportName == stageGate.ReportName
                                       where SGData_Years.Any(val1 => a.Year.Equals(val1))
                                       where SGData_Months.Any(Val2 => a.Month.Equals(Val2))
                                       where SGData_TaskStatus.Any(Val3 => a.Task_Status.Equals(Val3))
                                       where SGData_ProjectStatus.Any(Val3 => a.Milestone__Project_Status.Equals(Val3))
                                       select new
                                       {
                                           a.Task_Title,
                                           a.Task_Start_Date,
                                           a.Task_Due_Date,
                                           a.Workspace_Title,
                                           a.Task__Assignee__Full_Name,
                                           a.Milestone__Assignee__Full_Name,
                                           a.Milestone__Assignee__Reports_to__Full_Name,
                                           a.Task_Status,
                                           a.Milestone__Assignee__Country,
                                           a.Milestone__Project_Status,
                                           a.Month,
                                           a.Year,
                                       });
                re.code = 200;
                re.message = "Success";
                re.Data = StageGateDataExport;
            }
            return re;
        }

        [HttpPost]
        [Route("StageGateTTData")]
        public Response StageGateTTData(StageGate stageGate)
        {
            var StageGateTTData = (from a in entity.StageGates
                                   where a.ReportName == "TestingTask"
                                   select new
                                   {
                                       a.Task_Title,
                                       a.Task_Start_Date,
                                       a.Task_Due_Date,
                                       a.Workspace_Title,
                                       a.Task__Assignee__Full_Name,
                                       a.Milestone__Assignee__Full_Name,
                                       a.Milestone__Assignee__Reports_to__Full_Name,
                                       a.Task_Status,
                                       a.Milestone__Assignee__Country,
                                       a.Milestone__Project_Status,
                                       a.Month,
                                       a.Year,
                                   });
            re.code = 200;
            re.message = "Success";
            re.Data = StageGateTTData;
            return re;
        }
    }
    
    public class StageGateMonths
    {
        public string Month { get; set; }
        public Boolean isSelected { get; set; }
    }
}