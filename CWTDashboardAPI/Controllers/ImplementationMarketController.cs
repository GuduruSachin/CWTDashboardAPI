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
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        Filters fi = new Filters();

        int projectstatuscount, projectstatus;
        string[] Projectstatus, imps_Year, imps_type, imps_months, imps_projectlevel, imps_region, imps_Country,imps_ownership;
        [HttpPost]
        [Route("ImeetMilestoneProjectStatus")]
        public Response GetImeetMilestoneProjectStatus(CLRData clr)
        {
            if (clr.GoLiveYear == null || clr.GoLiveMonth == null || clr.OwnerShip == null || clr.ProjectLevel == null || clr.Region == null || clr.ProjectStatus == null || clr.ImplementationType == null)
            {
                re.Data = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                Projectstatus = clr.ProjectStatus.Split(',');
                for (int i = 0; i < Projectstatus.Count(); i++)
                {
                    if (Projectstatus[i] == "" || Projectstatus[i] == "---" || Projectstatus[i] == "null")
                    {
                        Projectstatus[i] = "---";
                    }
                }
                imps_Year = clr.GoLiveYear.Split(',');
                for (int i = 0; i < imps_Year.Count(); i++)
                {
                    if (imps_Year[i] == "" || imps_Year[i] == "---" || imps_Year[i] == "null")
                    {
                        imps_Year[i] = "---";
                    }
                }
                imps_ownership = clr.OwnerShip.Split(',');
                for (int i = 0; i < imps_ownership.Count(); i++)
                {
                    if (imps_ownership[i] == "" || imps_ownership[i] == "---" || imps_ownership[i] == "null")
                    {
                        imps_ownership[i] = "---";
                    }
                }
                imps_type = clr.ImplementationType.Split(',');
                for (int i = 0; i < imps_type.Count(); i++)
                {
                    if (imps_type[i] == "" || imps_type[i] == "---" || imps_type[i] == "null")
                    {
                        imps_type[i] = "---";
                    }
                }
                imps_months = clr.GoLiveMonth.Split(',');
                for (int i = 0; i < imps_months.Count(); i++)
                {
                    if (imps_months[i] == "" || imps_months[i] == "---" || imps_months[i] == "null")
                    {
                        imps_months[i] = "---";
                    }
                }
                imps_projectlevel = clr.ProjectLevel.Split(',');
                for (int i = 0; i < imps_projectlevel.Count(); i++)
                {
                    if (imps_projectlevel[i] == "" || imps_projectlevel[i] == "---" || imps_projectlevel[i] == "null")
                    {
                        imps_projectlevel[i] = "---";
                    }
                }
                imps_region = clr.Region.Split(',');
                for (int i = 0; i < imps_region.Count(); i++)
                {
                    if (imps_region[i] == "" || imps_region[i] == "---" || imps_region[i] == "null")
                    {
                        imps_region[i] = "---";
                    }
                }
                imps_Country = clr.Country.Split(',');
                for (int i = 0; i < imps_Country.Count(); i++)
                {
                    if (imps_Country[i] == "" || imps_Country[i] == "---" || imps_Country[i] == "null")
                    {
                        imps_Country[i] = "---";
                    }
                }
                //IEnumerable<string> values = clr.iMeet_Milestone___Project_Status.Split(',');
                var projectstatus_list = (from a in entity.CLRDatas
                                          where a.Status == "Active"
                                          //join b in entity.CLRs on new { a.Client, clr.iMeet_Milestone___Project_Status } equals new { b.Client, b.iMeet_Milestone___Project_Status }    
                                          where Projectstatus.Any(val => a.ProjectStatus.Equals(val))
                                          where imps_Year.Any( val1 => a.GoLiveYear.Equals(val1))
                                          where imps_type.Any(val => a.Revenue_Opportunity_Type.Equals(val))
                                          where imps_months.Any(val => a.GoLiveMonth.Equals(val))
                                          where imps_ownership.Any(val => a.OwnerShip.Equals(val))
                                          where imps_projectlevel.Any(val => a.ProjectLevel.Equals(val))
                                          where imps_region.Any(val => a.Region.Equals(val))
                                          where a.RevenueID < 600000000000000
                                          //where imps_Country.Any(val => a.Country.Equals(val))
                                          select new
                                          {
                                              Client = a.Client == null ? "---" : a.Client ?? "---",
                                              iMeet_Project_Level = a.ProjectLevel ?? "---",
                                              Region__Opportunity_ = a.Region ?? "---",
                                              Implementation_Type = a.Revenue_Opportunity_Type ?? "---",
                                              iMeet_Milestone___Project_Status = a.ProjectStatus ?? "---",
                                              Revenue_Total_Volume_USD = a.RevenueVolumeUSD ?? 0,
                                              a.GoLiveDate,
                                              Country = a.Country ?? "---"
                                              //Revenue_Total_Volume_USD = entity.CLRDatas.Where(x => x.Client == a.Client && x.iMeet_Project_Level == a.iMeet_Project_Level && x.Region__Opportunity_ == a.Region__Opportunity_ && x.Implementation_Type == a.Implementation_Type && x.iMeet_Milestone___Project_Status == a.ProjectStatus).Sum(xs => xs.Revenue_Total_Volume_USD) ?? 0,
                                              //Revenue_Total_Volume_USD = entity.CLRs.Where(x=>x.Client == a.Client).Where(x2=> x2.iMeet_Project_Level == a.iMeet_Project_Level).Where(x3 => x3.Region__Opportunity_ == a.Region__Opportunity_).Where(x4 => x4.Implementation_Type == a.Implementation_Type).Where(x5 => values.Any(x5s => x5s.Equals(x5.iMeet_Milestone___Project_Status))).Sum(xs => xs.Revenue_Total_Volume_USD),
                                          }).OrderBy(x => x.Client);                                               
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
        [Route("MarketGoliveReport")]
        public Response MarketGoliveReport(CLRData clr)
        {
            if (clr.OwnerShip == null || clr.Region == null || clr.ProjectStatus == null || clr.ImplementationType == null || clr.ProjectLevel == null)
            {
                re.Data = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                Projectstatus = clr.ProjectStatus.Split(',');
                for (int i = 0; i < Projectstatus.Count(); i++)
                {
                    if (Projectstatus[i] == "" || Projectstatus[i] == "---" || Projectstatus[i] == "null")
                    {
                        Projectstatus[i] = "---";
                    }
                }
                imps_ownership = clr.OwnerShip.Split(',');
                for (int i = 0; i < imps_ownership.Count(); i++)
                {
                    if (imps_ownership[i] == "" || imps_ownership[i] == "---" || imps_ownership[i] == "null")
                    {
                        imps_ownership[i] = "---";
                    }
                }
                imps_type = clr.ImplementationType.Split(',');
                for (int i = 0; i < imps_type.Count(); i++)
                {
                    if (imps_type[i] == "" || imps_type[i] == "---" || imps_type[i] == "null")
                    {
                        imps_type[i] = "---";
                    }
                }
                imps_projectlevel = clr.ProjectLevel.Split(',');
                for (int i = 0; i < imps_projectlevel.Count(); i++)
                {
                    if (imps_projectlevel[i] == "" || imps_projectlevel[i] == "---" || imps_projectlevel[i] == "null")
                    {
                        imps_projectlevel[i] = "---";
                    }
                }
                imps_region = clr.Region.Split(',');
                for (int i = 0; i < imps_region.Count(); i++)
                {
                    if (imps_region[i] == "" || imps_region[i] == "---" || imps_region[i] == "null")
                    {
                        imps_region[i] = "---";
                    }
                }
                string C_Month = DateTime.Now.ToString("MMM");
                string N_Month = DateTime.Now.AddMonths(1).ToString("MMM");
                string N_Month_POne = DateTime.Now.AddMonths(2).ToString("MMM");
                string N_Month_PTwo = DateTime.Now.AddMonths(3).ToString("MMM");
                string N_Month_PThree = DateTime.Now.AddMonths(4).ToString("MMM");
                string C_Year = DateTime.Now.Year.ToString();
                string N_Year = DateTime.Now.AddMonths(1).Year.ToString();
                string N_Year_POne = DateTime.Now.AddMonths(2).Year.ToString();
                string N_Year_PTwo = DateTime.Now.AddMonths(3).Year.ToString();
                string N_Year_PThree = DateTime.Now.AddMonths(4).Year.ToString();
                var GoliveFilterData = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where ((a.GoLiveYear == C_Year && a.GoLiveMonth == C_Month) || (a.GoLiveMonth == N_Month && a.GoLiveYear == N_Year) || (a.GoLiveMonth == N_Month_POne && a.GoLiveYear == N_Year_POne) || (a.GoLiveMonth == N_Month_PTwo && a.GoLiveYear == N_Year_PTwo) || (a.GoLiveMonth == N_Month_PThree && a.GoLiveYear == N_Year_PThree))
                                        where imps_projectlevel.Any(val => a.ProjectLevel.Equals(val))
                                        where Projectstatus.Any(val => a.ProjectStatus.Equals(val))
                                        where imps_type.Any(val => a.Revenue_Opportunity_Type.Equals(val))
                                        where imps_ownership.Any(val => a.OwnerShip.Equals(val))
                                        where imps_region.Any(val => a.Region.Equals(val))
                                        where a.Client != null
                                        select a).ToList();
                var Golive_data = (from b in GoliveFilterData
                                   group b by b.Client into g
                                   select new
                                   {
                                       Client = g.Key,
                                       Country = "---",
                                       GoLiveMonth = GoliveFilterData.FirstOrDefault(x => x.Client == g.Key).GoLiveMonth,
                                       EltStatus = GoliveFilterData.FirstOrDefault(x => x.Client == g.Key).Workspace__ELT_Overall_Status,
                                       EltComments = GoliveFilterData.FirstOrDefault(x => x.Client == g.Key).Workspace__ELT_Overall_Comments == "" || GoliveFilterData.FirstOrDefault(x => x.Client == g.Key).Workspace__ELT_Overall_Comments == null ? "---" : GoliveFilterData.FirstOrDefault(x => x.Client == g.Key).Workspace__ELT_Overall_Comments,
                                       CurrentMonth = GoliveFilterData.Where(x => x.GoLiveMonth == C_Month && x.Client == g.Key).Sum(x => x.RevenueVolumeUSD),
                                       NextMonth = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month && x.Client == g.Key).Sum(x => x.RevenueVolumeUSD),
                                       NextMonthPlusOne = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_POne && x.Client == g.Key).Sum(x => x.RevenueVolumeUSD),
                                       NextMonthPlusTwo = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_PTwo && x.Client == g.Key).Sum(x => x.RevenueVolumeUSD),
                                       NextMonthPlusThree = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_PThree && x.Client == g.Key).Sum(x => x.RevenueVolumeUSD),
                                       CurrentMonthCount = GoliveFilterData.Where(x => x.GoLiveMonth == C_Month && x.Client == g.Key).Sum(x => x.Revenue_Total_Transactions),
                                       NextMonthCount = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month && x.Client == g.Key).Sum(x => x.Revenue_Total_Transactions),
                                       NextMonthPlusOneCount = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_POne && x.Client == g.Key).Sum(x => x.Revenue_Total_Transactions),
                                       NextMonthPlusTwoCount = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_PTwo && x.Client == g.Key).Sum(x => x.Revenue_Total_Transactions),
                                       NextMonthPlusThreeCount = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_PThree && x.Client == g.Key).Sum(x => x.Revenue_Total_Transactions),
                                       TotalVolume = GoliveFilterData.Where(x => x.Client == g.Key).Sum(x => x.RevenueVolumeUSD),
                                       TotalVolumeCount = GoliveFilterData.Where(x => x.Client == g.Key).Count(),
                                       CountrywiseSum = (from c in GoliveFilterData
                                                         where c.Client == g.Key
                                                         select new
                                                         {
                                                             Client = c.Client,
                                                             Country = c.Country,
                                                             EltStatus = c.CountryStatus == "" || c.CountryStatus == null ? "---" : c.CountryStatus,
                                                             EltComments = c.Workspace__ELT_Overall_Comments == "" || c.Workspace__ELT_Overall_Comments == null ? "---" : c.Workspace__ELT_Overall_Comments,
                                                             CurrentMonth = GoliveFilterData.Where(x => x.GoLiveMonth == C_Month && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x=> x.RevenueVolumeUSD),
                                                             CurrentMonthCount = GoliveFilterData.Where(x => x.GoLiveMonth == C_Month && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x => x.Revenue_Total_Transactions),
                                                             NextMonth = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x => x.RevenueVolumeUSD),
                                                             NextMonthPlusOne = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_POne && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x => x.RevenueVolumeUSD),    
                                                             NextMonthPlusTwo = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_PTwo && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x => x.RevenueVolumeUSD),
                                                             NextMonthPlusThree = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_PThree && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x => x.RevenueVolumeUSD),
                                                             NextMonthCount = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x => x.Revenue_Total_Transactions),
                                                             NextMonthPlusOneCount = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_POne && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x => x.Revenue_Total_Transactions),
                                                             NextMonthPlusTwoCount = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_PTwo && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x => x.Revenue_Total_Transactions),
                                                             NextMonthPlusThreeCount = GoliveFilterData.Where(x => x.GoLiveMonth == N_Month_PThree && x.Client == c.Client && x.Country == c.Country && x.RevenueID == c.RevenueID).Sum(x => x.Revenue_Total_Transactions),
                                                         }).Distinct(),
                                   }).OrderBy(x => x.Client);
                                   //.Where(x => x.TotalVolume > 0).OrderBy(x => x.Client);
                re.Data = Golive_data;
                re.code = 200;
                re.message = "No Data found" + C_Month + " - " + C_Year + ","  + N_Month + " - " + N_Year + "," + N_Month_POne + " - " + N_Year_POne + "," + N_Month_PTwo + " - " + N_Year_PTwo + "," + N_Month_PThree + " - " + N_Year_PThree;
            }
            return re;
        }
        //[HttpPost]
        //[Route("ProjectStatus")]
        //public Response GetProjectStatus(CLRData clr)
        //{
        //    var projectstatusdata = (from a in entity.CLRDatas
        //                             where a.Status == "Active"
        //                             where a.ProjectStatus != null && a.ProjectStatus != ""
        //                             select new
        //                             {
        //                                 iMeet_Milestone___Project_Status = a.ProjectStatus,
        //                                 isSelected = true,
        //                             }).Distinct();
        //    projectstatus = projectstatusdata.AsQueryable().Count();
        //    if (projectstatus.ToString() == "" || projectstatus.ToString() == null || projectstatus == 0)
        //    {
        //        re.Data = projectstatusdata;
        //        re.code = 100;
        //        re.message = "No Data found";
        //    }
        //    else
        //    {
        //        re.Data = projectstatusdata;
        //        re.code = 200;
        //        re.message = "Data Successfull";
        //    }
        //    return re;
        //}
        [HttpPost]
        [Route("ImeetImplementationFiltersList")]
        public Filters GetImeetImplemetationFilters(CLRData clr)
        {
            //var FilterYear = entity.CLRs.Select(a => a.Go_Live_Year).Distinct().OrderByDescending(b => b.Go_Live_Year).ToList();
            //var Filteryear = (from a in entity.CLRDatas
            //                  where a.Status == "Active"
            //                  where a.GoLiveYear != null && a.GoLiveYear != "1900" && a.GoLiveYear != ""
            //                  select new
            //                  {
            //                      Go_Live_Year = a.GoLiveYear,
            //                      isSelected = true,
            //                  }).Distinct().OrderBy(x => x.Go_Live_Year);
            var Filteryears = (from a in entity.CLRDatas
                               where a.Status == "Active"
                               where a.GoLiveYear != "2017"
                               where a.GoLiveYear != "2018"
                               select new
                               {
                                   Go_Live_Year = a.GoLiveYear == null || a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
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
            var FilterProjectLevel = (from a in entity.CLRDatas
                                      where a.Status == "Active"
                                      select new
                                      {
                                          iMeet_Project_Level = a.ProjectLevel == null || a.ProjectLevel == "" || a.ProjectLevel == "---" ? "---" : a.ProjectLevel ?? "---",
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.iMeet_Project_Level);
            var FilterOwnership = (from a in entity.CLRDatas
                                      where a.Status == "Active"
                                      select new
                                      {
                                          OwnerShip = a.OwnerShip == null || a.OwnerShip == "" || a.OwnerShip == "---" ? "---" : a.OwnerShip ?? "---",
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.OwnerShip);
            var FilterQuarter = (from a in entity.CLRDatas
                                 where a.Status == "Active"
                                 select new
                                 {
                                     Quarter = a.Quarter == null || a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.Quarter);
            var FilterImplementationType = (from a in entity.CLRDatas
                                            where a.Status == "Active"
                                            select new
                                            {
                                                Implementation_Type = a.Revenue_Opportunity_Type == null || a.Revenue_Opportunity_Type == "" ? "---" : a.Revenue_Opportunity_Type ?? "---",
                                                isSelected = true,
                                            }).Distinct().OrderBy(x => x.Implementation_Type);
            var FilterRegion = (from a in entity.CLRDatas
                                where a.Status == "Active"
                                select new
                                {
                                    Region__Opportunity_ = a.Region == null || a.Region == "" ? "---" : a.Region ?? "---",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Region__Opportunity_);
            var FilterStatus = (from a in entity.CLRDatas
                                where a.Status == "Active"
                                where a.BacklogStarted != "" && a.BacklogStarted != null
                                select new
                                {
                                    Backlog_Started = a.BacklogStarted == null || a.BacklogStarted == "" ? "---" : a.BacklogStarted ?? "---",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Backlog_Started);
            var FilterMarketLeaders = (from a in entity.CLRDatas
                                       where a.Status == "Active"
                                       select new
                                       {
                                           Market_Leader = a.MarketLeader == null || a.MarketLeader == "" ? "---" : a.MarketLeader ?? "---",
                                           isSelected = true,
                                       }).Distinct().OrderBy(x => x.Market_Leader);
            var FilterMilestoneStatus = (from a in entity.CLRDatas
                                         where a.Status == "Active"
                                         select new
                                         {
                                             iMeet_Milestone___Project_Status = a.ProjectStatus == null || a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                             isSelected = true,
                                         }).Distinct().OrderBy(x => x.iMeet_Milestone___Project_Status);
            var Country = (from a in entity.CLRDatas
                           where a.Status == "Active"
                           where a.OwnerShip == "WO"
                           select new
                           {
                               Country = a.Country == null || a.Country == "" ? "---" : a.Country ?? "---",
                               isSelected = true,
                           }).Distinct().OrderBy(x => x.Country);
            fi.code = 200;
            fi.message = "Data Successfull";
            fi.Year = Filteryears;
            fi.c_Year = Filteryears;
            fi.rp_Year = Filteryears;
            fi.Months = Months;
            fi.c_Months = Months;
            fi.ProjectLevel = FilterProjectLevel;
            fi.c_ProjectLevel = FilterProjectLevel;
            fi.Quarter = FilterQuarter;
            fi.OwnerShip = FilterOwnership;
            fi.ImplementationType = FilterImplementationType;
            fi.c_ImplementationType = FilterImplementationType;
            fi.Region = FilterRegion;
            fi.Status = FilterStatus;
            fi.MarketLeaders = FilterMarketLeaders;
            fi.MilestoneStatus = FilterMilestoneStatus;
            fi.c_MilestoneStatus = FilterMilestoneStatus;
            fi.rp_ProjectStatus = FilterMilestoneStatus;
            fi.rp_ImplementationType = FilterImplementationType;
            fi.Country = Country;
            return fi;
        }

        [HttpPost]
        [Route("MonthlyRevenue")]
        public Response MonthlyRevenue(CLRData clr)
        {
            var year = clr.GoLiveYear;
            //IEnumerable<string> years = clr.Go_Live_Years.Split(',');
            IEnumerable<string> months = clr.GoLiveMonth.Split(',');
            IEnumerable<string> quarters = clr.Quarter.Split(',');
            IEnumerable<string> projectlevel = clr.ProjectLevel.Split(',');
            IEnumerable<string> status = clr.BacklogStarted.Split(',');
            IEnumerable<string> region = clr.Region.Split(',');
            IEnumerable<string> leaders = clr.MarketLeader.Split(',');
            //IEnumerable<string> implementationtype = clr.Implementation_Type.Split(',');
            IEnumerable<string> project_status = "C-Closed,A-Active/Date Confirmed".Split(',');
            var TotalMonthlyRevenue = (from a in entity.CLRDatas
                                       where a.Status == "Active"
                                       where a.GoLiveYear == year
                                       //where years.Any(val1 => a.Go_Live_Year.Equals(val1))
                                       where months.Any(val2 => a.GoLiveMonth.Equals(val2))
                                       where quarters.Any(val3 => a.Quarter.Equals(val3))
                                       where projectlevel.Any(val4 => a.ProjectLevel.Equals(val4))
                                       where status.Any(val5 => a.BacklogStarted.Equals(val5))
                                       where region.Any(val6 => a.Region.Equals(val6))
                                       where leaders.Any(val7 => a.MarketLeader.Equals(val7)) || a.MarketLeader == null
                                       //where implementationtype.Any(val8 => a.Implementation_Type.Equals(val8)) || a.Implementation_Type == null
                                       where project_status.Any(val9 => a.ProjectStatus.Equals(val9))
                                       select a).ToList();
            var MonthlyRevenue = (from b in TotalMonthlyRevenue
                                  select new
                                  {
                                      GJanuary = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Jan").Sum(xs => xs.RevenueVolumeUSD),
                                      LJanuary = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Jan").Sum(xs => xs.RevenueVolumeUSD),
                                      RJanuary = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Jan").Sum(xs => xs.RevenueVolumeUSD),
                                      GFebruary = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Feb").Sum(xs => xs.RevenueVolumeUSD),
                                      LFebruary = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Feb").Sum(xs => xs.RevenueVolumeUSD),
                                      RFebruary = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Feb").Sum(xs => xs.RevenueVolumeUSD),
                                      GMarch = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Mar").Sum(xs => xs.RevenueVolumeUSD),
                                      LMarch = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Mar").Sum(xs => xs.RevenueVolumeUSD),
                                      RMarch = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Mar").Sum(xs => xs.RevenueVolumeUSD),
                                      GApril = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Apr").Sum(xs => xs.RevenueVolumeUSD),
                                      LApril = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Apr").Sum(xs => xs.RevenueVolumeUSD),
                                      RApril = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Apr").Sum(xs => xs.RevenueVolumeUSD),
                                      GMay = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "May").Sum(xs => xs.RevenueVolumeUSD),
                                      LMay = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "May").Sum(xs => xs.RevenueVolumeUSD),
                                      RMay = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "May").Sum(xs => xs.RevenueVolumeUSD),
                                      GJune = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Jun").Sum(xs => xs.RevenueVolumeUSD),
                                      LJune = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Jun").Sum(xs => xs.RevenueVolumeUSD),
                                      RJune = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Jun").Sum(xs => xs.RevenueVolumeUSD),
                                      GJuly = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Jul").Sum(xs => xs.RevenueVolumeUSD),
                                      LJuly = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Jul").Sum(xs => xs.RevenueVolumeUSD),
                                      RJuly = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Jul").Sum(xs => xs.RevenueVolumeUSD),
                                      GAugust = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Aug").Sum(xs => xs.RevenueVolumeUSD),
                                      LAugust = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Aug").Sum(xs => xs.RevenueVolumeUSD),
                                      RAugust = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Aug").Sum(xs => xs.RevenueVolumeUSD),
                                      GSeptember = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Sep").Sum(xs => xs.RevenueVolumeUSD),
                                      LSeptember = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Sep").Sum(xs => xs.RevenueVolumeUSD),
                                      RSeptember = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Sep").Sum(xs => xs.RevenueVolumeUSD),
                                      GOctober = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Oct").Sum(xs => xs.RevenueVolumeUSD),
                                      LOctober = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Oct").Sum(xs => xs.RevenueVolumeUSD),
                                      ROctober = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Oct").Sum(xs => xs.RevenueVolumeUSD),
                                      GNovember = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Nov").Sum(xs => xs.RevenueVolumeUSD),
                                      LNovember = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Nov").Sum(xs => xs.RevenueVolumeUSD),
                                      RNovember = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Nov").Sum(xs => xs.RevenueVolumeUSD),
                                      GDecember = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Global").Where(x1 => x1.GoLiveMonth == "Dec").Sum(xs => xs.RevenueVolumeUSD),
                                      LDecember = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Local").Where(x1 => x1.GoLiveMonth == "Dec").Sum(xs => xs.RevenueVolumeUSD),
                                      RDecember = TotalMonthlyRevenue.Where(x2 => x2.ProjectLevel == "Regional").Where(x1 => x1.GoLiveMonth == "Dec").Sum(xs => xs.RevenueVolumeUSD),
                                  }).Distinct();
            re.code = 200;
            re.message = "Success";
            re.Data = MonthlyRevenue;
            return re;
        }

        string[] y_months, y_ownership, y_projectlevel, y_status, y_region, y_Country, y_projectStatus, y_type;
        [HttpPost]
        [Route("MonthlyRevenueByYear")]
        public Response MonthlyRevenueByYear(CLRData clr)
        {
            if (clr.GoLiveYear == null || clr.GoLiveMonth == null || clr.OwnerShip == null || clr.ProjectLevel == null || clr.Region == null || clr.ProjectStatus == null || clr.ImplementationType == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                y_months = clr.GoLiveMonth.Split(',');
                for (int i = 0; i < y_months.Count(); i++)
                {
                    if (y_months[i] == "" || y_months[i] == "---" || y_months[i] == "null")
                    {
                        y_months[i] = "---";
                    }
                }
                y_ownership = clr.OwnerShip.Split(',');
                for (int i = 0; i < y_ownership.Count(); i++)
                {
                    if (y_ownership[i] == "" || y_ownership[i] == "---" || y_ownership[i] == "null")
                    {
                        y_ownership[i] = "---";
                    }
                }
                y_projectlevel = clr.ProjectLevel.Split(',');
                for (int i = 0; i < y_projectlevel.Count(); i++)
                {
                    if (y_projectlevel[i] == "" || y_projectlevel[i] == "---" || y_projectlevel[i] == "null")
                    {
                        y_projectlevel[i] = "---";
                    }
                }
                //y_status = clr.BacklogStarted.Split(',');
                //for (int i = 0; i < y_status.Count(); i++)
                //{
                //    if (y_status[i] == "")
                //    {
                //        y_status[i] = null;
                //    }
                //}
                y_region = clr.Region.Split(',');
                for (int i = 0; i < y_region.Count(); i++)
                {
                    if (y_region[i] == "" || y_region[i] == "---" || y_region[i] == "null")
                    {
                        y_region[i] = "---";
                    }
                }
                y_Country = clr.Country.Split(',');
                for (int i = 0; i < y_Country.Count(); i++)
                {
                    if (y_Country[i] == "" || y_Country[i] == "---" || y_Country[i] == "null")
                    {
                        y_Country[i] = "---";
                    }
                }
                var year = clr.GoLiveYear;
                y_projectStatus = clr.ProjectStatus.Split(',');
                for (int i = 0; i < y_projectStatus.Count(); i++)
                {
                    if (y_projectStatus[i] == "" || y_projectStatus[i] == "---" || y_projectStatus[i] == "null")
                    {
                        y_projectStatus[i] = "---";
                    }
                }
                y_type = clr.ImplementationType.Split(',');
                for (int i = 0; i < y_type.Count(); i++)
                {
                    if (y_type[i] == "" || y_type[i] == "---" || y_type[i] == "null")
                    {
                        y_type[i] = "---";
                    }
                }
                //IEnumerable<string> project_status = "C-Closed,A-Active/Date Confirmed".Split(',');
                var TotalMonthlyRevenue = (from a in entity.CLRDatas
                                           where a.Status == "Active"
                                           where a.GoLiveYear == year
                                           //where years.Any(val1 => a.Go_Live_Year.Equals(val1))
                                           where y_months.Any(val2 => a.GoLiveMonth.Equals(val2))
                                           where y_ownership.Any(val3 => a.OwnerShip.Equals(val3))
                                           where y_projectlevel.Any(val4 => a.ProjectLevel.Equals(val4))
                                           //where y_status.Any(val5 => a.BacklogStarted.Equals(val5))
                                           where y_region.Any(val6 => a.Region.Equals(val6))
                                           //where y_Country.Any(val7 => a.Country.Equals(val7))
                                           where y_type.Any(val8 => a.Revenue_Opportunity_Type.Equals(val8))
                                           where y_projectStatus.Any(val9 => a.ProjectStatus.Equals(val9))
                                           where a.RevenueID < 600000000000000
                                           select a).ToList();
                var MonthlyRevenue = (from b in TotalMonthlyRevenue
                                      select new
                                      {
                                          January = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Jan").Sum(xs => xs.RevenueVolumeUSD),
                                          February = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Feb").Sum(xs => xs.RevenueVolumeUSD),
                                          March = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Mar").Sum(xs => xs.RevenueVolumeUSD),
                                          April = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Apr").Sum(xs => xs.RevenueVolumeUSD),
                                          May = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "May").Sum(xs => xs.RevenueVolumeUSD),
                                          June = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Jun").Sum(xs => xs.RevenueVolumeUSD),
                                          July = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Jul").Sum(xs => xs.RevenueVolumeUSD),
                                          August = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Aug").Sum(xs => xs.RevenueVolumeUSD),
                                          September = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Sep").Sum(xs => xs.RevenueVolumeUSD),
                                          October = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Oct").Sum(xs => xs.RevenueVolumeUSD),
                                          November = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Nov").Sum(xs => xs.RevenueVolumeUSD),
                                          December = TotalMonthlyRevenue.Where(x1 => x1.GoLiveMonth == "Dec").Sum(xs => xs.RevenueVolumeUSD),
                                      }).Distinct();
                re.code = 200;
                re.message = "Success";
                re.Data = MonthlyRevenue;
            }
            return re;
        }

        string[] rw_status, rw_Year, rw_type, rw_months, rw_projectlevel, rw_region, rw_Country, rw_ownership; 
        [HttpPost]
        [Route("RegionWiseRevenue")]
        public Response RegionWiseReport(CLRData clr)
        {
            if (clr.GoLiveYear == null || clr.GoLiveMonth == null || clr.OwnerShip == null || clr.ProjectLevel == null || clr.Region == null || clr.ProjectStatus == null || clr.ImplementationType == null)
            {
                re.code = 100;
                re.message = "Please select the Project Status";
                re.Data = null;
            }
            else
            {
                rw_status = clr.ProjectStatus.Split(',');
                for (int i = 0; i < rw_status.Count(); i++)
                {
                    if (rw_status[i] == "" || rw_status[i] == "---" || rw_status[i] == "null")
                    {
                        rw_status[i] = "---";
                    }
                }
                rw_ownership = clr.OwnerShip.Split(',');
                for (int i = 0; i < rw_ownership.Count(); i++)
                {
                    if (rw_ownership[i] == "" || rw_ownership[i] == "---" || rw_ownership[i] == "null")
                    {
                        rw_ownership[i] = "---";
                    }
                }
                rw_Year = clr.GoLiveYear.Split(',');
                for (int i = 0; i < rw_Year.Count(); i++)
                {
                    if (rw_Year[i] == "" || rw_Year[i] == "---" || rw_Year[i] == "null")
                    {
                        rw_Year[i] = "---";
                    }
                }
                rw_type = clr.ImplementationType.Split(',');
                for (int i = 0; i < rw_type.Count(); i++)
                {
                    if (rw_type[i] == "" || rw_type[i] == "---" || rw_type[i] == "null")
                    {
                        rw_type[i] = "---";
                    }
                }
                rw_months = clr.GoLiveMonth.Split(',');
                for (int i = 0; i < rw_months.Count(); i++)
                {
                    if (rw_months[i] == "" || rw_months[i] == "---" || rw_months[i] == "null")
                    {
                        rw_months[i] = "---";
                    }
                }
                rw_projectlevel = clr.ProjectLevel.Split(',');
                for (int i = 0; i < rw_projectlevel.Count(); i++)
                {
                    if (rw_projectlevel[i] == "" || rw_projectlevel[i] == "---" || rw_projectlevel[i] == "null")
                    {
                        rw_projectlevel[i] = "---";
                    }
                }
                rw_region = clr.Region.Split(',');
                for (int i = 0; i < rw_region.Count(); i++)
                {
                    if (rw_region[i] == "" || rw_region[i] == "---" || rw_region[i] == "null")
                    {
                        rw_region[i] = "---";
                    }
                }
                rw_Country = clr.Country.Split(',');
                for (int i = 0; i < rw_Country.Count(); i++)
                {
                    if (rw_Country[i] == "" || rw_Country[i] == "---" || rw_Country[i] == "null")
                    {
                        rw_Country[i] = "---";
                    }
                }
                var RegionWiseRevenue = (from a in entity.CLRDatas
                                         where a.Status == "Active"
                                         where rw_status.Any(val => a.ProjectStatus.Equals(val))
                                         where rw_Year.Any(val => a.GoLiveYear.Equals(val))
                                         where rw_ownership.Any(val => a.OwnerShip.Equals(val))
                                         where rw_type.Any(val => a.Revenue_Opportunity_Type.Equals(val))
                                         where rw_months.Any(val => a.GoLiveMonth.Equals(val))
                                         where rw_projectlevel.Any(val => a.ProjectLevel.Equals(val))
                                         where rw_region.Any(val => a.Region.Equals(val))
                                         where a.RevenueID < 600000000000000
                                         //where rw_Country.Any(val => a.Country.Equals(val))
                                         //where a.Region != null
                                         group a by a.Region into g
                                         select new
                                         {
                                             Region__Opportunity_ = g.Key == null || g.Key == "" ? "---" : g.Key ?? "---",
                                             RevenueVolume = g.Sum(x => x.RevenueVolumeUSD),
                                             ProjectsCount = g.Count(),
                                         }).OrderBy(x => x.Region__Opportunity_).Distinct();
                re.code = 200;
                re.message = "Success";
                re.Data = RegionWiseRevenue;
            }
            return re;
        }
        
        string[] PLW_status, PLW_year, PLW_type, PLW_months, PLW_projectlevel, PLW_region, PLW_Country, PLWOwnership;
        [HttpPost]
        [Route("ProjectLevelWise")]
        public Response ProjectLevelWise(CLRData clr)
        {
            if (clr.GoLiveYear == null || clr.GoLiveMonth == null || clr.OwnerShip == null || clr.ProjectLevel == null || clr.Region == null || clr.ProjectStatus == null || clr.ImplementationType == null)
            {
                re.code = 100;
                re.message = "Please select the Project Status";
                re.Data = null;
            }
            else
            {
                PLW_status = clr.ProjectStatus.Split(',');
                for (int i = 0; i < PLW_status.Count(); i++)
                {
                    if (PLW_status[i] == "" || PLW_status[i] == "---" || PLW_status[i] == "null")
                    {
                        PLW_status[i] = "---";
                    }
                }
                PLWOwnership = clr.OwnerShip.Split(',');
                for (int i = 0; i < PLWOwnership.Count(); i++)
                {
                    if (PLWOwnership[i] == "" || PLWOwnership[i] == "---" || PLWOwnership[i] == "null")
                    {
                        PLWOwnership[i] = "---";
                    }
                }
                PLW_year = clr.GoLiveYear.Split(',');
                for (int i = 0; i < PLW_year.Count(); i++)
                {
                    if (PLW_year[i] == "" || PLW_year[i] == "---" || PLW_year[i] == "null")
                    {
                        PLW_year[i] = "---";
                    }
                }
                PLW_type = clr.ImplementationType.Split(',');
                for (int i = 0; i < PLW_type.Count(); i++)
                {
                    if (PLW_type[i] == "" || PLW_type[i] == "---" || PLW_type[i] == "null")
                    {
                        PLW_type[i] = "---";
                    }
                }
                PLW_months = clr.GoLiveMonth.Split(',');
                for (int i = 0; i < PLW_months.Count(); i++)
                {
                    if (PLW_months[i] == "" || PLW_months[i] == "---" || PLW_months[i] == "null")
                    {
                        PLW_months[i] = "---";
                    }
                }
                PLW_projectlevel = clr.ProjectLevel.Split(',');
                for (int i = 0; i < PLW_projectlevel.Count(); i++)
                {
                    if (PLW_projectlevel[i] == "" || PLW_projectlevel[i] == "---" || PLW_projectlevel[i] == "null")
                    {
                        PLW_projectlevel[i] = "---";
                    }
                }
                PLW_region = clr.Region.Split(',');
                for (int i = 0; i < PLW_region.Count(); i++)
                {
                    if (PLW_region[i] == "" || PLW_region[i] == "---" || PLW_region[i] == "null")
                    {
                        PLW_region[i] = "---";
                    }
                }
                PLW_Country = clr.Country.Split(',');
                for (int i = 0; i < PLW_Country.Count(); i++)
                {
                    if (PLW_Country[i] == "" || PLW_Country[i] == "---" || PLW_Country[i] == "null")
                    {
                        PLW_Country[i] = "---";
                    }
                }
                var ProjectLevelWise = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where PLW_status.Any(val => a.ProjectStatus.Equals(val))
                                        where PLW_year.Any(val1 => a.GoLiveYear.Equals(val1))
                                        where PLW_type.Any(val => a.Revenue_Opportunity_Type.Equals(val))
                                        where PLW_months.Any(val => a.GoLiveMonth.Equals(val))
                                        where PLWOwnership.Any(val => a.OwnerShip.Equals(val))
                                        where PLW_projectlevel.Any(val => a.ProjectLevel.Equals(val))
                                        where PLW_region.Any(val => a.Region.Equals(val))
                                        where a.ProjectLevel != "---"
                                        where a.RevenueID < 600000000000000
                                        //where PLW_Country.Any(val => a.Country.Equals(val))
                                        group a by a.ProjectLevel into g
                                        select new
                                        {
                                            //iMeet_Project_Level = g.Key == null || g.Key == "" ? "---"
                                            //    : g.Key == "Global" ? "GLobal - GCG"
                                            //    : g.Key == "Local" ? "Local - National"
                                            //    : g.Key == "Regional" ? "Regional - MCG" : "---" ?? "---",
                                            iMeet_Project_Level = g.Key == null || g.Key == "" ? "---"
                                                : g.Key == "Global" ? "Global"
                                                : g.Key == "Local" ? "Local"
                                                : g.Key == "Regional" ? "Regional" : "---" ?? "---",
                                            ProjectsCount = g.Count(),
                                            RevenueVolume = g.Sum(x => x.RevenueVolumeUSD),
                                        }).OrderBy(x => x.iMeet_Project_Level);
                re.code = 200;
                re.message = "Success";
                re.Data = ProjectLevelWise;
            }
            return re;
        }
        string[] MTR_status, MTR_Year, MTR_Month, MTR_Level, MTR_Type, MTR_Region;
        
        String[] status, otherstatus;
        string Year1, Year2, Year3, PriorYear;
        [HttpPost]
        [Route("MonthlyTotalRevenueWithDelta")]
        public Response MonthlyTotalRevenueWithDelta(CLRData clr)
        {
            status = "C-Closed,A-Active/Date Confirmed".Split(',');
            string Month = DateTime.Now.ToString("MMM");
            string year = DateTime.Now.Year.ToString();

            var CurrentData = (from a in entity.CLRDatas
                        where a.GoLiveYear == year
                        where status.Any(val1 => a.ProjectStatus.Equals(val1))
                        where a.Status == "Active"
                        select a).AsEnumerable();

            var OLDData = (from a in entity.EltOldCLRDatas
                           where a.GoLiveYear == year
                           where status.Any(val1 => a.ProjectStatus.Equals(val1))
                           where a.Status == "Active"
                           select a).AsEnumerable();

            var Data = (from a in entity.CLRDatas
                        where a.CLRID == 1
                        select new {
                            CMJan = CurrentData.Where(x => x.GoLiveMonth == "Jan").Sum(x => x.RevenueVolumeUSD),
                            CMFeb = CurrentData.Where(x => x.GoLiveMonth == "Feb").Sum(x => x.RevenueVolumeUSD),
                            CMMar = CurrentData.Where(x => x.GoLiveMonth == "Mar").Sum(x => x.RevenueVolumeUSD),
                            CMApr = CurrentData.Where(x => x.GoLiveMonth == "Apr").Sum(x => x.RevenueVolumeUSD),
                            CMMay = CurrentData.Where(x => x.GoLiveMonth == "May").Sum(x => x.RevenueVolumeUSD),
                            CMJun = CurrentData.Where(x => x.GoLiveMonth == "Jun").Sum(x => x.RevenueVolumeUSD),
                            CMJul = CurrentData.Where(x => x.GoLiveMonth == "Jul").Sum(x => x.RevenueVolumeUSD),
                            CMAug = CurrentData.Where(x => x.GoLiveMonth == "Aug").Sum(x => x.RevenueVolumeUSD),
                            CMSep = CurrentData.Where(x => x.GoLiveMonth == "Sep").Sum(x => x.RevenueVolumeUSD),
                            CMOct = CurrentData.Where(x => x.GoLiveMonth == "Oct").Sum(x => x.RevenueVolumeUSD),
                            CMNov = CurrentData.Where(x => x.GoLiveMonth == "Nov").Sum(x => x.RevenueVolumeUSD),
                            CMDec = CurrentData.Where(x => x.GoLiveMonth == "Dec").Sum(x => x.RevenueVolumeUSD),
                            CMTot = CurrentData.Sum(x => x.RevenueVolumeUSD),
                            LMJan = OLDData.Where(x => x.GoLiveMonth == "Jan").Sum(x => x.RevenueVolumeUSD),
                            LMFeb = OLDData.Where(x => x.GoLiveMonth == "Feb").Sum(x => x.RevenueVolumeUSD),
                            LMMar = OLDData.Where(x => x.GoLiveMonth == "Mar").Sum(x => x.RevenueVolumeUSD),
                            LMApr = OLDData.Where(x => x.GoLiveMonth == "Apr").Sum(x => x.RevenueVolumeUSD),
                            LMMay = OLDData.Where(x => x.GoLiveMonth == "May").Sum(x => x.RevenueVolumeUSD),
                            LMJun = OLDData.Where(x => x.GoLiveMonth == "Jun").Sum(x => x.RevenueVolumeUSD),
                            LMJul = OLDData.Where(x => x.GoLiveMonth == "Jul").Sum(x => x.RevenueVolumeUSD),
                            LMAug = OLDData.Where(x => x.GoLiveMonth == "Aug").Sum(x => x.RevenueVolumeUSD),
                            LMSep = OLDData.Where(x => x.GoLiveMonth == "Sep").Sum(x => x.RevenueVolumeUSD),
                            LMOct = OLDData.Where(x => x.GoLiveMonth == "Oct").Sum(x => x.RevenueVolumeUSD),
                            LMNov = OLDData.Where(x => x.GoLiveMonth == "Nov").Sum(x => x.RevenueVolumeUSD),
                            LMDec = OLDData.Where(x => x.GoLiveMonth == "Dec").Sum(x => x.RevenueVolumeUSD),
                            LMTot = OLDData.Sum(x => x.RevenueVolumeUSD),
                        }).Distinct().ToList();
            re.MonthlyTotalRevenueWithDelta = Data;
            re.code = 200;
            re.message = "Success";
            re.LastUpdatedOn = entity.ReportUpdatedOns.Where(x => x.ReportName == "MonthlyDelta").Max(x => x.UpdatedOn);
            return re;
        }
        int AutomatedCLRDataCount;
        [HttpPost]
        [Route("AutomatedCLRData")]
        public Response CLRData(CLRData cLRData)
        {
            var CLRData = (from a in entity.CLRDatas
                           where a.Status == "Active"
                           select new
                                {
                                    a.CLRID,
                                    a.RevenueID,
                                    a.Region,
                                    a.Country,
                                    a.OwnerShip,
                                    GoLiveDate = a.GoLiveDate,
                                    a.ProjectStatus,
                                    a.CountryStatus,
                                    a.ProjectLevel,
                                    a.CompletedDate,
                                    a.AssigneeFullName,
                                    a.MilestoneTitle,
                                    a.Milestone__Record_ID_Key,
                                    a.Task__Task_Record_ID_Key,
                                    a.Group_Name,
                                    a.Milestone__Project_Notes,
                                    a.Milestone__Reason_Code,
                                    a.Milestone__Closed_Loop_Owner,
                                    a.Workspace_Title,
                                    a.Workspace__ELT_Overall_Status,
                                    a.Workspace__ELT_Overall_Comments,
                                    a.Customer_Row_ID,
                                    a.Opportunity_ID,
                                    a.Account_Name,
                                    a.Sales_Stage_Name,
                                    a.Opportunity_Type,
                                    a.Revenue_Opportunity_Type,
                                    a.Revenue_Status,
                                    a.Opportunity_Owner,
                                    a.Opportunity_Category,
                                    a.Revenue_Total_Transactions,
                                    a.CountryCode,
                                    a.RevenueVolumeUSD,
                                    a.MarketLeader,
                                    a.GlobalProjectManager,
                                    a.ProjectConsultant,
                                    a.RegionalProjectManager,
                                    a.GlobalCISOBTLead,
                                    a.GlobalCISHRFeedSpecialist,
                                    a.GlobalCISPortraitLead,
                                    a.GoLiveMonth,
                                    a.GoLiveYear,
                                    a.BacklogStarted,
                                    a.Quarter,
                                    a.CycleTime,
                                    a.ExternalKickoffDuedate,
                                }).ToList();
            AutomatedCLRDataCount = CLRData.AsQueryable().Count();
            if (AutomatedCLRDataCount.ToString() == "" || AutomatedCLRDataCount.ToString() == null || AutomatedCLRDataCount == 0)
            {
                re.Data = CLRData;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                re.Data = CLRData;
                re.code = 200;
                re.message = "Data Successfull";
            }
            return re;
        }
    }
    public class Months
    {
        public string Go_Live_Month { get; set; }
        public Boolean isSelected { get; set; }
    }
}