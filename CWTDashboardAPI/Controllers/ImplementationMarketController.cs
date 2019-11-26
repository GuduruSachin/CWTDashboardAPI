using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Net.Http;
using System.Web.Http;
using CWTDashboardAPI.Models;
using System.Globalization;

namespace CWTDashboardAPI.Controllers
{
    public class ImplementationMarketController : ApiController
    {
        CWTEntities entity = new CWTEntities();
        Response re = new Response();
        Filters fi = new Filters();
        int projectstatuscount, projectstatus;
        string[] Projectstatus;
        [HttpPost]
        [Route("ImeetMilestoneProjectStatus")]
        public Response GetImeetMilestoneProjectStatus(CLR clr)
        {
            if (clr.iMeet_Milestone___Project_Status == "" || clr.iMeet_Milestone___Project_Status == null)
            {
                re.Data = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                Projectstatus = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < Projectstatus.Count(); i++)
                {
                    if (Projectstatus[i] == "" || Projectstatus[i] == "null")
                    {
                        Projectstatus[i] = null;
                    }
                }
                //IEnumerable<string> values = clr.iMeet_Milestone___Project_Status.Split(',');
                var projectstatus_list = (from a in entity.CLRs
                                          //join b in entity.CLRs on new { a.Client, clr.iMeet_Milestone___Project_Status } equals new { b.Client, b.iMeet_Milestone___Project_Status }    
                                          where Projectstatus.Any(val => a.iMeet_Milestone___Project_Status.Equals(val))
                                          select new
                                          {
                                              a.Client,
                                              a.iMeet_Project_Level,
                                              a.Region__Opportunity_,
                                              a.Implementation_Type,
                                              a.iMeet_Milestone___Project_Status,
                                              //Revenue_Total_Volume_USD = entity.CLRs.Where(x => x.Client == a.Client && x.iMeet_Project_Level == a.iMeet_Project_Level && x.Region__Opportunity_ == a.Region__Opportunity_ && x.Implementation_Type == a.Implementation_Type && x.iMeet_Milestone___Project_Status == a.iMeet_Milestone___Project_Status).Sum(xs => xs.Revenue_Total_Volume_USD),
                                              //Revenue_Total_Volume_USD = entity.CLRs.Where(x=>x.Client == a.Client).Where(x2=> x2.iMeet_Project_Level == a.iMeet_Project_Level).Where(x3 => x3.Region__Opportunity_ == a.Region__Opportunity_).Where(x4 => x4.Implementation_Type == a.Implementation_Type).Where(x5 => values.Any(x5s => x5s.Equals(x5.iMeet_Milestone___Project_Status))).Sum(xs => xs.Revenue_Total_Volume_USD),
                                          }).Distinct().OrderBy(x => x.Client);
                projectstatuscount = projectstatus_list.AsQueryable().Count();
                if (projectstatuscount.ToString() == "" || projectstatuscount.ToString() == null || projectstatuscount == 0)
                {
                    re.Data = projectstatus_list;
                    re.code = 100;
                    re.message = "No Data found";
                }
                else
                {
                    re.Data = projectstatus_list;
                    re.code = 200;
                    re.message = "Data Successfull";
                }
            }
            return re;
        }
        [HttpPost]
        [Route("ProjectStatus")]
        public Response GetProjectStatus(CLR clr)
        {
            var projectstatusdata = (from a in entity.CLRs
                                     where a.iMeet_Milestone___Project_Status != null && a.iMeet_Milestone___Project_Status != ""
                                     select new
                                     {
                                         a.iMeet_Milestone___Project_Status,
                                         isSelected = true,
                                     }).Distinct();
            projectstatus = projectstatusdata.AsQueryable().Count();
            if (projectstatus.ToString() == "" || projectstatus.ToString() == null || projectstatus == 0)
            {
                re.Data = projectstatusdata;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                re.Data = projectstatusdata;
                re.code = 200;
                re.message = "Data Successfull";
            }
            return re;
        }

        [HttpPost]
        [Route("ImeetImplementationFiltersList")]
        public Filters GetImeetImplemetationFilters(CLR clr)
        {
            //var FilterYear = entity.CLRs.Select(a => a.Go_Live_Year).Distinct().OrderByDescending(b => b.Go_Live_Year).ToList();
            var Filteryear = (from a in entity.CLRs
                              where a.Go_Live_Year != null && a.Go_Live_Year != "1900" && a.Go_Live_Year != "2050" && a.Go_Live_Year != ""
                              select new
                              {
                                  Go_Live_Year = a.Go_Live_Year,
                                  isSelected = true,
                              }).Distinct().OrderBy(x => x.Go_Live_Year);
            var Months = new List<Months>()
            {
                 new Months() {Go_Live_Month="Jan",isSelected=true},
                 new Months() {Go_Live_Month="Feb",isSelected=true},
                 new Months() {Go_Live_Month="Mar",isSelected=true},
                 new Months() {Go_Live_Month="Apr",isSelected=true},
                 new Months() {Go_Live_Month="May",isSelected=true},
                 new Months() {Go_Live_Month="Jun",isSelected=true},
                 new Months() {Go_Live_Month="Jul",isSelected=true},
                 new Months() {Go_Live_Month="Aug",isSelected=true},
                 new Months() {Go_Live_Month="Sep",isSelected=true},
                 new Months() {Go_Live_Month="Oct",isSelected=true},
                 new Months() {Go_Live_Month="Nov",isSelected=true},
                 new Months() {Go_Live_Month="Dec",isSelected=true},
            };
            //var FilterMonth = (from a in entity.CLRs
            //                   where a.Go_Live_Month != "" && a.Go_Live_Month != null && a.Go_Live_Month != "2050"
            //                   select new
            //                   {
            //                       Go_Live_Month = a.Go_Live_Month ?? "",
            //                       isSelected = true,
            //                   }).Distinct().OrderBy(x => DateTime.ParseExact(x.Go_Live_Month, "MMM", CultureInfo.InvariantCulture).Month);//DateTime.ParseExact(x.Go_Live_Month, "Mmm", CultureInfo.InvariantCulture)
            var FilterProjectLevel = (from a in entity.CLRs
                                where a.iMeet_Project_Level != "" && a.iMeet_Project_Level != null
                                select new
                                {
                                    iMeet_Project_Level = a.iMeet_Project_Level ?? "",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.iMeet_Project_Level);
            var FilterQuarter = (from a in entity.CLRs
                                 where a.Quarter != "" && a.Quarter != null
                                 select new
                                 {
                                     Quarter = a.Quarter ?? "",
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.Quarter);
            var FilterImplementationType = (from a in entity.CLRs
                                select new
                                {
                                    Implementation_Type = a.Implementation_Type ?? "",
                                    isSelected = true,
                                }).Distinct().OrderByDescending(x => x.Implementation_Type);
            var FilterRegion = (from a in entity.CLRs
                                where a.Region__Opportunity_ != "" && a.Region__Opportunity_ != null
                                select new
                               {
                                   Region__Opportunity_ = a.Region__Opportunity_ ?? "",
                                   isSelected = true,
                                }).Distinct().OrderBy(x => x.Region__Opportunity_);
            var FilterStatus = (from a in entity.CLRs
                                where a.Backlog_Started != "" && a.Backlog_Started != null
                                select new
                               {
                                   Backlog_Started = a.Backlog_Started ?? "",
                                   isSelected = true,
                                }).Distinct().OrderBy(x => x.Backlog_Started);
            var FilterMarketLeaders = (from a in entity.CLRs
                                select new
                                {
                                    Market_Leader = a.Market_Leader ?? "",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Market_Leader);
            var FilterMilestoneStatus = (from a in entity.CLRs
                                       select new
                                       {
                                           iMeet_Milestone___Project_Status = a.iMeet_Milestone___Project_Status ?? "",
                                           isSelected = true,
                                       }).Distinct().OrderByDescending(x => x.iMeet_Milestone___Project_Status);
            fi.code = 200;
            fi.message = "Data Successfull";
            fi.Year = Filteryear;
            fi.c_Year = Filteryear;
            fi.Months = Months;
            fi.c_Months = Months;
            fi.ProjectLevel = FilterProjectLevel;
            fi.c_ProjectLevel = FilterProjectLevel;
            fi.Quarter = FilterQuarter;
            fi.ImplementationType = FilterImplementationType;
            fi.Region = FilterRegion;
            fi.Status = FilterStatus;
            fi.MarketLeaders = FilterMarketLeaders;
            fi.MilestoneStatus = FilterMilestoneStatus;
            fi.c_MilestoneStatus = FilterMilestoneStatus;
            return fi;
        }
        
        [HttpPost]
        [Route("MonthlyRevenue")]
        public Response MonthlyRevenue(CLR clr)
        {
            var year = clr.Go_Live_Year;
            //IEnumerable<string> years = clr.Go_Live_Years.Split(',');
            IEnumerable<string> months = clr.Go_Live_Month.Split(',');
            IEnumerable<string> quarters = clr.Quarter.Split(',');
            IEnumerable<string> projectlevel = clr.iMeet_Project_Level.Split(',');
            IEnumerable<string> status = clr.Backlog_Started.Split(',');
            IEnumerable<string> region = clr.Region__Opportunity_.Split(',');
            IEnumerable<string> leaders = clr.Market_Leader.Split(',');
            //IEnumerable<string> implementationtype = clr.Implementation_Type.Split(',');
            IEnumerable<string> project_status = "C-Closed,A-Active/Date Confirmed".Split(',');
            
            var TotalMonthlyRevenue = (from a in entity.CLRs
                                       where a.Go_Live_Year == year
                                       //where years.Any(val1 => a.Go_Live_Year.Equals(val1))
                                       where months.Any(val2 => a.Go_Live_Month.Equals(val2))
                                       where quarters.Any(val3 => a.Quarter.Equals(val3))
                                       where projectlevel.Any(val4 => a.iMeet_Project_Level.Equals(val4))
                                       where status.Any(val5 => a.Backlog_Started.Equals(val5))
                                       where region.Any(val6 => a.Region__Opportunity_.Equals(val6))
                                       where leaders.Any(val7 => a.Market_Leader.Equals(val7)) || a.Market_Leader == null
                                       //where implementationtype.Any(val8 => a.Implementation_Type.Equals(val8)) || a.Implementation_Type == null
                                       where project_status.Any(val9 => a.iMeet_Milestone___Project_Status.Equals(val9))
                                       select a).ToList();

            var MonthlyRevenue = (from b in TotalMonthlyRevenue
                                  select new
                                  {
                                      GJanuary = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Jan").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LJanuary = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Jan").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RJanuary = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Jan").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GFebruary = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Feb").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LFebruary = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Feb").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RFebruary = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Feb").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GMarch = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Mar").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LMarch = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Mar").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RMarch = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Mar").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GApril = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Apr").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LApril = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Apr").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RApril = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Apr").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GMay = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "May").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LMay = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "May").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RMay = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "May").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GJune = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Jun").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LJune = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Jun").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RJune = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Jun").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GJuly = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Jul").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LJuly = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Jul").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RJuly = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Jul").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GAugust = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Aug").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LAugust = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Aug").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RAugust = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Aug").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GSeptember = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Sep").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LSeptember = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Sep").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RSeptember = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Sep").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GOctober = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Oct").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LOctober = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Oct").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      ROctober = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Oct").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GNovember = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Nov").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LNovember = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Nov").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RNovember = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Nov").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      GDecember = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Global").Where(x1 => x1.Go_Live_Month == "Dec").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      LDecember = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Local").Where(x1 => x1.Go_Live_Month == "Dec").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      RDecember = TotalMonthlyRevenue.Where(x2 => x2.iMeet_Project_Level == "Regional").Where(x1 => x1.Go_Live_Month == "Dec").Sum(xs => xs.Revenue_Total_Volume_USD),
                                  }).Distinct();
            re.code = 200;
            re.message = "Success";
            re.Data = MonthlyRevenue;
            return re;
        }

        string[] y_months, y_quarters, y_projectlevel, y_status, y_region, y_leaders,y_projectStatus;
        [HttpPost]
        [Route("MonthlyRevenueByYear")]
        public Response MonthlyRevenueByYear(CLR clr)
        {
            if (clr.Go_Live_Year == null || clr.Go_Live_Month == null || clr.Quarter == null || clr.iMeet_Project_Level == null || clr.Backlog_Started == null || clr.Region__Opportunity_ == null || clr.Market_Leader == null || clr.iMeet_Milestone___Project_Status == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                y_months = clr.Go_Live_Month.Split(',');
                for (int i = 0; i < y_months.Count(); i++)
                {
                    if (y_months[i] == "")
                    {
                        y_months[i] = null;
                    }
                }
                y_quarters = clr.Quarter.Split(',');
                for (int i = 0; i < y_quarters.Count(); i++)
                {
                    if (y_quarters[i] == "")
                    {
                        y_quarters[i] = null;
                    }
                }
                y_projectlevel = clr.iMeet_Project_Level.Split(',');
                for (int i = 0; i < y_projectlevel.Count(); i++)
                {
                    if (y_projectlevel[i] == "")
                    {
                        y_projectlevel[i] = null;
                    }
                }
                y_status = clr.Backlog_Started.Split(',');
                for (int i = 0; i < y_status.Count(); i++)
                {
                    if (y_status[i] == "")
                    {
                        y_status[i] = null;
                    }
                }
                y_region = clr.Region__Opportunity_.Split(',');
                for (int i = 0; i < y_region.Count(); i++)
                {
                    if (y_region[i] == "")
                    {
                        y_region[i] = null;
                    }
                }
                y_leaders = clr.Market_Leader.Split(',');
                for (int i = 0; i < y_leaders.Count(); i++)
                {
                    if (y_leaders[i] == "")
                    {
                        y_leaders[i] = null;
                    }
                }
                var year = clr.Go_Live_Year;
                y_projectStatus = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < y_projectStatus.Count(); i++)
                {
                    if (y_projectStatus[i] == "")
                    {
                        y_projectStatus[i] = null;
                    }
                }
                //IEnumerable<string> project_status = "C-Closed,A-Active/Date Confirmed".Split(',');
                var TotalMonthlyRevenue = (from a in entity.CLRs
                                           where a.Go_Live_Year == year
                                           //where years.Any(val1 => a.Go_Live_Year.Equals(val1))
                                           where y_months.Any(val2 => a.Go_Live_Month.Equals(val2))
                                           where y_quarters.Any(val3 => a.Quarter.Equals(val3))
                                           where y_projectlevel.Any(val4 => a.iMeet_Project_Level.Equals(val4))
                                           where y_status.Any(val5 => a.Backlog_Started.Equals(val5))
                                           where y_region.Any(val6 => a.Region__Opportunity_.Equals(val6))
                                           where y_leaders.Any(val7 => a.Market_Leader.Equals(val7)) || a.Market_Leader == null
                                           //where implementationtype.Any(val8 => a.Implementation_Type.Equals(val8)) || a.Implementation_Type == null
                                           where y_projectStatus.Any(val9 => a.iMeet_Milestone___Project_Status.Equals(val9))
                                           select a).ToList();
                var MonthlyRevenue = (from b in TotalMonthlyRevenue
                                      select new
                                      {
                                          January = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Jan").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          February = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Feb").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          March = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Mar").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          April = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Apr").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          May = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "May").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          June = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Jun").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          July = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Jul").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          August = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Aug").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          September = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Sep").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          October = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Oct").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          November = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Nov").Sum(xs => xs.Revenue_Total_Volume_USD),
                                          December = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Dec").Sum(xs => xs.Revenue_Total_Volume_USD),
                                      }).Distinct();

                re.code = 200;
                re.message = "Success";
                re.Data = MonthlyRevenue;
            }
            //IEnumerable<string> years = clr.Go_Live_Years.Split(',');
            //IEnumerable<string> months = clr.Go_Live_Month.Split(',');
            //IEnumerable<string> quarters = clr.Quarter.Split(',');
            //IEnumerable<string> projectlevel = clr.iMeet_Project_Level.Split(',');
            //IEnumerable<string> status = clr.Backlog_Started.Split(',');
            //IEnumerable<string> region = clr.Region__Opportunity_.Split(',');
            //IEnumerable<string> leaders = clr.Market_Leader.Split(',');
            //IEnumerable<string> implementationtype = clr.Implementation_Type.Split(',');
            return re;
        }

        string[] rw_status;
        [HttpPost]
        [Route("RegionWiseRevenue")]
        public Response RegionWiseReport(CLR clr)
        {
            if (clr.iMeet_Milestone___Project_Status == null)
            {
                re.code = 100;
                re.message = "Please select the Project Status";
                re.Data = null;
            }
            else
            {
                rw_status = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < rw_status.Count(); i++)
                {
                    if (rw_status[i] == "")
                    {
                        rw_status[i] = null;
                    }
                }
                var Data = (from a in entity.CLRs
                            where rw_status.Any(val => a.iMeet_Milestone___Project_Status.Equals(val))
                            select a).ToList();
                var RegionWiseRevenue = (from a in Data
                                         select new
                                         {
                                             APAC = Data.Where(x2 => x2.Region__Opportunity_ == "APAC").Sum(xs => xs.Revenue_Total_Volume_USD),
                                             EMEA = Data.Where(x2 => x2.Region__Opportunity_ == "EMEA").Sum(xs => xs.Revenue_Total_Volume_USD),
                                             LATAM = Data.Where(x2 => x2.Region__Opportunity_ == "LATAM").Sum(xs => xs.Revenue_Total_Volume_USD),
                                             NORAM = Data.Where(x2 => x2.Region__Opportunity_ == "NORAM").Sum(xs => xs.Revenue_Total_Volume_USD),
                                             Blanks = Data.Where(x2 => x2.Region__Opportunity_ == null).Sum(xs => xs.Revenue_Total_Volume_USD),
                                         }).Distinct();
                re.code = 200;
                re.message = "Success";
                re.Data = RegionWiseRevenue;
            }
            return re;
        }

        string[] gt_years, gt_months, gt_quarters, gt_projectlevel, gt_status, gt_region, gt_leaders, gt_projectstatus;
        [HttpPost]
        [Route("MonthlyGrandTotalRevenue")]
        public Response MonthlyGrandTotalRevenue(CLR clr)
        {
            if (clr.Go_Live_Year == null || clr.Go_Live_Month == null || clr.Quarter == null || clr.iMeet_Project_Level == null || clr.Backlog_Started == null || clr.Region__Opportunity_ == null || clr.Market_Leader == null || clr.iMeet_Milestone___Project_Status == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                gt_years = clr.Go_Live_Year.Split(',');
                for (int i = 0; i < gt_years.Count(); i++)
                {
                    if (gt_years[i] == "")
                    {
                        gt_years[i] = null;
                    }
                }
                gt_months = clr.Go_Live_Month.Split(',');
                for (int i = 0; i < gt_months.Count(); i++)
                {
                    if (gt_months[i] == "")
                    {
                        gt_months[i] = null;
                    }
                }
                gt_quarters = clr.Quarter.Split(',');
                for (int i = 0; i < gt_quarters.Count(); i++)
                {
                    if (gt_quarters[i] == "")
                    {
                        gt_quarters[i] = null;
                    }
                }
                gt_projectlevel = clr.iMeet_Project_Level.Split(',');
                for (int i = 0; i < gt_projectlevel.Count(); i++)
                {
                    if (gt_projectlevel[i] == "")
                    {
                        gt_projectlevel[i] = null;
                    }
                }
                gt_status = clr.Backlog_Started.Split(',');
                for (int i = 0; i < gt_status.Count(); i++)
                {
                    if (gt_status[i] == "")
                    {
                        gt_status[i] = null;
                    }
                }
                gt_region = clr.Region__Opportunity_.Split(',');
                for (int i = 0; i < gt_region.Count(); i++)
                {
                    if (gt_region[i] == "")
                    {
                        gt_region[i] = null;
                    }
                }
                gt_leaders = clr.Market_Leader.Split(',');
                for (int i = 0; i < gt_leaders.Count(); i++)
                {
                    if (gt_leaders[i] == "")
                    {
                        gt_leaders[i] = null;
                    }
                }
                gt_projectstatus = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < gt_projectstatus.Count(); i++)
                {
                    if (gt_projectstatus[i] == "")
                    {
                        gt_projectstatus[i] = null;
                    }
                }
                //IEnumerable<string> project_status = "C-Closed,A-Active/Date Confirmed".Split(',');
                var TotalMonthlyRevenue = (from a in entity.CLRs
                                           //where a.Go_Live_Year == 2019
                                           where gt_years.Any(val1 => a.Go_Live_Year.Equals(val1))
                                           where gt_months.Any(val2 => a.Go_Live_Month.Equals(val2))
                                           where gt_quarters.Any(val3 => a.Quarter.Equals(val3))
                                           where gt_projectlevel.Any(val4 => a.iMeet_Project_Level.Equals(val4))
                                           where gt_status.Any(val5 => a.Backlog_Started.Equals(val5))
                                           where gt_region.Any(val6 => a.Region__Opportunity_.Equals(val6))
                                           where gt_leaders.Any(val7 => a.Market_Leader.Equals(val7)) || a.Market_Leader == null
                                           //where implementationtype.Any(val8 => a.Implementation_Type.Equals(val8)) || a.Implementation_Type == null
                                           where gt_projectstatus.Any(val9 => a.iMeet_Milestone___Project_Status.Equals(val9))
                                           select a).ToList();
                var MonthlyGTRevenue = (from b in TotalMonthlyRevenue
                                        select new
                                        {
                                            CWJanuary = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Jan").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWJanuary = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Jan").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWFebruary = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Feb").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWFebruary = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Feb").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWMarch = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Mar").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWMarch = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Mar").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWApril = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Apr").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWApril = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Apr").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWMay = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "May").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWMay = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "May").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWJune = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Jun").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWJune = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Jun").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWJuly = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Jul").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWJuly = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Jul").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWAugust = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Aug").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWAugust = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Aug").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWSeptember = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Sep").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWSeptember = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Sep").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWOctober = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Oct").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWOctober = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Oct").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWNovember = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Nov").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWNovember = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Nov").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            CWDecember = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Dec").Sum(xs => xs.Revenue_Total_Volume_USD),
                                            LWDecember = TotalMonthlyRevenue.Where(x1 => x1.Go_Live_Month == "Dec").Sum(xs => xs.Revenue_Total_Volume_USD),
                                        }).Distinct();
                re.code = 200;
                re.message = "Success";
                re.Data = MonthlyGTRevenue;
            }
            //IEnumerable<string> years = clr.Go_Live_Years.Split(',');
            //IEnumerable<string> months = clr.Go_Live_Month.Split(',');
            //IEnumerable<string> quarters = clr.Quarter.Split(',');
            //IEnumerable<string> projectlevel = clr.iMeet_Project_Level.Split(',');
            //IEnumerable<string> status = clr.Backlog_Started.Split(',');
            //IEnumerable<string> region = clr.Region__Opportunity_.Split(',');
            //IEnumerable<string> leaders = clr.Market_Leader.Split(',');
            //IEnumerable<string> implementationtype = clr.Implementation_Type.Split(',');
            return re;
        }
    }
    public class Months
    {
        public string Go_Live_Month { get; set; }
        public Boolean isSelected { get; set; }
    }
}