using CWTDashboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CWTDashboardAPI.Controllers
{
    public class MarketChartsController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        Filters fi = new Filters();
        IMRCResponse imrs = new IMRCResponse();

        string[] CV_Projectstatus,CV_ProjectLevel,CV_GoLiveYear, CV_GoLiveMonth, CV_Region, CV_ImplementationType, CV_Country, CV_Ownership;
        [HttpPost]
        [Route("ChartVolumeCycleTimeCounts")]
        public IMRCResponse ChartVolumeCycleTimeCounts(CLRData clr)
        {
            if (clr.ProjectStatus == null || clr.ProjectLevel == null || clr.OwnerShip == null || clr.GoLiveYear == null || clr.GoLiveMonth == null || clr.Region == null || clr.ImplementationType == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                CV_Projectstatus = clr.ProjectStatus.Split(',');
                for (int i = 0; i < CV_Projectstatus.Count(); i++)
                {
                    if (CV_Projectstatus[i] == "" || CV_Projectstatus[i] == "---" || CV_Projectstatus[i] == "null")
                    {
                        CV_Projectstatus[i] = "---";
                    }
                }
                CV_ProjectLevel = clr.ProjectLevel.Split(',');
                for (int i = 0; i < CV_ProjectLevel.Count(); i++)
                {
                    if (CV_ProjectLevel[i] == "" || CV_ProjectLevel[i] == "---" || CV_ProjectLevel[i] == "null")
                    {
                        CV_ProjectLevel[i] = "---";
                    }
                }
                CV_GoLiveYear = clr.GoLiveYear.Split(',');
                for (int i = 0; i < CV_GoLiveYear.Count(); i++)
                {
                    if (CV_GoLiveYear[i] == "" || CV_GoLiveYear[i] == "---" || CV_GoLiveYear[i] == "null")
                    {
                        CV_GoLiveYear[i] = "---";
                    }
                }
                CV_GoLiveMonth = clr.GoLiveMonth.Split(',');
                for (int i = 0; i < CV_GoLiveMonth.Count(); i++)
                {
                    if (CV_GoLiveMonth[i] == "" || CV_GoLiveMonth[i] == "---" || CV_GoLiveMonth[i] == "null")
                    {
                        CV_GoLiveMonth[i] = "---";
                    }
                }
                CV_Region = clr.Region.Split(',');
                for (int i = 0; i < CV_Region.Count(); i++)
                {
                    if (CV_Region[i] == "" || CV_Region[i] == "---" || CV_Region[i] == "null")
                    {
                        CV_Region[i] = "---";
                    }
                }
                CV_ImplementationType = clr.ImplementationType.Split(',');
                for (int i = 0; i < CV_ImplementationType.Count(); i++)
                {
                    if (CV_ImplementationType[i] == "" || CV_ImplementationType[i] == "---" || CV_ImplementationType[i] == "null")
                    {
                        CV_ImplementationType[i] = "---";
                    }
                }
                CV_Country = clr.Country.Split(',');
                for (int i = 0; i < CV_Country.Count(); i++)
                {
                    if (CV_Country[i] == "" || CV_Country[i] == "---" || CV_Country[i] == "null")
                    {
                        CV_Country[i] = "---";
                    }
                }
                CV_Ownership = clr.OwnerShip.Split(',');
                for (int i = 0; i < CV_Ownership.Count(); i++)
                {
                    if (CV_Ownership[i] == "" || CV_Ownership[i] == "---" || CV_Ownership[i] == "null")
                    {
                        CV_Ownership[i] = "---";
                    }
                }
                
                var VolumeCycleData = (from a in entity.CLRDatas
                                       where a.Status == "Active"
                                       where CV_Projectstatus.Any(val => a.ProjectStatus.Equals(val))
                                       where CV_ProjectLevel.Any(val => a.ProjectLevel.Equals(val))
                                       where CV_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                       where CV_GoLiveMonth.Any(val => a.GoLiveMonth.Equals(val))
                                       where CV_Region.Any(val => a.Region.Equals(val))
                                       where CV_ImplementationType.Any(val => a.ImplementationType.Equals(val))
                                       where CV_Ownership.Any(val => a.OwnerShip.Equals(val))
                                       where a.RevenueID < 600000000000000
                                       //where CV_Country.Any(val => a.Country.Equals(val))
                                       select a);
                var VolumeCountCycleTime = (from a in VolumeCycleData
                                            group a by a.GoLiveMonth into g
                                            select new
                                            {
                                                GoLiveMonth = g.Key,
                                                Revenue_Total_Volume_USD = VolumeCycleData.Where(x => x.GoLiveMonth == g.Key).Sum(x => x.RevenueVolumeUSD),
                                                ProjectsCount = VolumeCycleData.Where(x => x.GoLiveMonth == g.Key).Count(),
                                                cycletimesum = VolumeCycleData.Where(x => x.ProjectStart_ForCycleTime != null && x.GoLiveMonth == g.Key).Sum(x => x.CycleTime),
                                                cycletimeCount = VolumeCycleData.Where(x => x.ProjectStart_ForCycleTime != null && x.GoLiveMonth == g.Key).Count(),
                                                Average = VolumeCycleData.Where(x => x.ProjectStart_ForCycleTime != null && x.GoLiveMonth == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.ProjectStart_ForCycleTime != null && x.GoLiveMonth == g.Key).Count(),
                                            }).AsEnumerable().OrderBy(x => DateTime.ParseExact(x.GoLiveMonth, "MMM", CultureInfo.InvariantCulture).Month);
                var VolumeCycleTime = (from a in entity.CLRDatas
                                       select new
                                       {
                                           January_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Jan").Sum(x => x.RevenueVolumeUSD),
                                           February_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Feb").Sum(x => x.RevenueVolumeUSD),
                                           March_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Mar").Sum(x => x.RevenueVolumeUSD),
                                           April_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Apr").Sum(x => x.RevenueVolumeUSD),
                                           May_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "May").Sum(x => x.RevenueVolumeUSD),
                                           June_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Jun").Sum(x => x.RevenueVolumeUSD),
                                           July_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Jul").Sum(x => x.RevenueVolumeUSD),
                                           August_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Aug").Sum(x => x.RevenueVolumeUSD),
                                           September_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Sep").Sum(x => x.RevenueVolumeUSD),
                                           October_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Oct").Sum(x => x.RevenueVolumeUSD),
                                           November_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Nov").Sum(x => x.RevenueVolumeUSD),
                                           December_RV = VolumeCycleData.Where(x => x.GoLiveMonth == "Dec").Sum(x => x.RevenueVolumeUSD),
                                           January_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jan").Count(),
                                           February_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Feb").Count(),
                                           March_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Mar").Count(),
                                           April_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Apr").Count(),
                                           May_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "May").Count(),
                                           June_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jun").Count(),
                                           July_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jul").Count(),
                                           August_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Aug").Count(),
                                           September_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Sep").Count(),
                                           October_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Oct").Count(),
                                           November_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Nov").Count(),
                                           December_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Dec").Count(),
                                           January_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jan" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jan" && x.ProjectStart_ForCycleTime != null).Count(),
                                           February_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Feb" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Feb" && x.ProjectStart_ForCycleTime != null).Count(),
                                           March_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Mar" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) /VolumeCycleData.Where(x => x.GoLiveMonth == "Mar" && x.ProjectStart_ForCycleTime != null).Count(),
                                           April_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Apr" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Apr" && x.ProjectStart_ForCycleTime != null).Count(),
                                           May_A = VolumeCycleData.Where(x => x.GoLiveMonth == "May" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "May" && x.ProjectStart_ForCycleTime != null).Count(),
                                           June_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jun" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) /VolumeCycleData.Where(x => x.GoLiveMonth == "Jun" && x.ProjectStart_ForCycleTime != null).Count(),
                                           July_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jul" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jul" && x.ProjectStart_ForCycleTime != null).Count(),
                                           August_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Aug" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Aug" && x.ProjectStart_ForCycleTime != null).Count(),
                                           September_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Sep" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Sep" && x.ProjectStart_ForCycleTime != null).Count(),
                                           October_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Oct" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Oct" && x.ProjectStart_ForCycleTime != null).Count(),
                                           November_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Nov" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Nov" && x.ProjectStart_ForCycleTime != null).Count(),
                                           December_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Dec" && x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Dec" && x.ProjectStart_ForCycleTime != null).Count(),
                                       }).Distinct();
                                       //.OrderBy(x=> x.GoLiveMonth == "Jan" ? 1 : x.GoLiveMonth == "Feb" ? 2 : x.GoLiveMonth == "Mar" ? 3 : x.GoLiveMonth == "Apr" ? 4 : x.GoLiveMonth == "May" ? 5 : x.GoLiveMonth == "Jun" ? 6 : x.GoLiveMonth == "Jul" ? 7 : x.GoLiveMonth == "Aug" ? 8 : x.GoLiveMonth == "Sep" ? 9 : x.GoLiveMonth == "Oct" ? 10 : x.GoLiveMonth == "Nov" ? 11 : x.GoLiveMonth == "Dec" ? 12 : 12 );
                imrs.code = 200;
                imrs.message = "Success";
                imrs.Data = VolumeCycleTime;
                imrs.VolumeCountCycleTime = VolumeCountCycleTime;
            }
            return imrs;
        }
        [HttpPost]
        [Route("CycleTimeFilters")]
        public Filters CycleTimeFilters(CLRData clr)
        {
            var Filteryears = (from a in entity.CLRDatas
                               where a.Status == "Active"
                               where a.GoLiveYear != "2017"
                               where a.GoLiveYear != "2018"
                               where a.GoLiveYear != "2019"
                               select new
                               {
                                   Go_Live_Year = a.GoLiveYear == null || a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                   isSelected = true,
                               }).Distinct().OrderBy(x => x.Go_Live_Year);
            var FilterRegion = (from a in entity.CLRDatas
                                where a.Status == "Active"
                                where a.Region != null
                                where a.Region != ""
                                where a.Region != "---"
                                select new
                                {
                                    Region__Opportunity_ = a.Region == null || a.Region == "" ? "---" : a.Region ?? "---",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Region__Opportunity_);
            var FilterStatus = (from a in entity.CLRDatas
                                where a.Status == "Active"
                                where a.ProjectStatus == "A-Active/Date Confirmed" || a.ProjectStatus == "C-Closed"
                                select new
                                {
                                    ProjectStatus = a.ProjectStatus == null || a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.ProjectStatus);
            fi.Year = Filteryears;
            fi.Region = FilterRegion;
            fi.c_MilestoneStatus = FilterStatus;
            fi.code = 200;
            fi.message = "Success";
            return fi;
        }
        string[] CT_Projectstatus, CT_ProjectLevel, CT_GoLiveYear, CT_GoLiveMonth, CTT_Region, CT_ImplementationType, CT_CycleTimeCategories;
        IQueryable<CLRData> VolumeCycleData;
        [HttpPost]
        [Route("CycleTimeChartVolumeCount")]
        public IMRCResponse CycleTimeChartVolumeCount(CLRData clr)
        {
            if (clr.ProjectStatus == null || clr.GoLiveYear == null || clr.Region == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                CT_Projectstatus = clr.ProjectStatus.Split(',');
                for (int i = 0; i < CT_Projectstatus.Count(); i++)
                {
                    if (CT_Projectstatus[i] == "" || CT_Projectstatus[i] == "---" || CT_Projectstatus[i] == "null")
                    {
                        CT_Projectstatus[i] = "---";
                    }
                }
                //CT_ProjectLevel = clr.ProjectLevel.Split(',');
                //for (int i = 0; i < CT_ProjectLevel.Count(); i++)
                //{
                //    if (CT_ProjectLevel[i] == "" || CT_ProjectLevel[i] == "null")
                //    {
                //        CT_ProjectLevel[i] = null;
                //    }
                //}
                CT_CycleTimeCategories = clr.CycleTimeCategories.Split(',');
                for (int i = 0; i < CT_CycleTimeCategories.Count(); i++)
                {
                    if (CT_CycleTimeCategories[i] == "" || CT_CycleTimeCategories[i] == "---" || CT_CycleTimeCategories[i] == "null")
                    {
                        CT_CycleTimeCategories[i] = "---";
                    }
                }
                CT_GoLiveYear = clr.GoLiveYear.Split(',');
                for (int i = 0; i < CT_GoLiveYear.Count(); i++)
                {
                    if (CT_GoLiveYear[i] == "" || CT_GoLiveYear[i] == "---" || CT_GoLiveYear[i] == "null")
                    {
                        CT_GoLiveYear[i] = "---";
                    }
                }
                //CT_GoLiveMonth = clr.GoLiveMonth.Split(',');
                //for (int i = 0; i < CT_GoLiveMonth.Count(); i++)
                //{
                //    if (CT_GoLiveMonth[i] == "" || CT_GoLiveMonth[i] == "null")
                //    {
                //        CT_GoLiveMonth[i] = null;
                //    }
                //}
                CTT_Region = clr.Region.Split(',');
                for (int i = 0; i < CTT_Region.Count(); i++)
                {
                    if (CTT_Region[i] == "" || CTT_Region[i] == "---" || CTT_Region[i] == "null")
                    {
                        CTT_Region[i] = "---";
                    }
                }
                //CT_ImplementationType = clr.ImplementationType.Split(',');
                //for (int i = 0; i < CT_ImplementationType.Count(); i++)
                //{
                //    if (CT_ImplementationType[i] == "" || CT_ImplementationType[i] == "null")
                //    {
                //        CT_ImplementationType[i] = null;
                //    }
                //}
                var CT_OwnerShip = "WO".Split(',');
                //var VolumeCycleData  = "";
                if (CT_CycleTimeCategories == null || clr.CycleTimeCategories == "Overall")
                {
                    VolumeCycleData = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where a.CycleTimeCategories != "0"
                                        where a.CycleTimeCategories != null
                                        where a.CycleTimeCategories != "New Global/regional/local"
                                        where CT_OwnerShip.Any(val => a.OwnerShip.Equals(val))
                                        where CT_Projectstatus.Any(val => a.ProjectStatus.Equals(val))
                                        where CT_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                        where CTT_Region.Any(val => a.Region.Equals(val))
                                        where a.ProjectStart_ForCycleTime != null
                                        select a);
                }
                else
                {
                    VolumeCycleData = (from a in entity.CLRDatas
                                       where a.Status == "Active"
                                       where a.CycleTimeCategories != "0"
                                       where a.CycleTimeCategories != null
                                       where a.CycleTimeCategories != "New Global/regional/local"
                                       where CT_OwnerShip.Any(val => a.OwnerShip.Equals(val))
                                       where CT_CycleTimeCategories.Any(val => a.CycleTimeCategories.Equals(val))
                                       where CT_Projectstatus.Any(val => a.ProjectStatus.Equals(val))
                                       where CT_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                       where CTT_Region.Any(val => a.Region.Equals(val))
                                       where a.ProjectStart_ForCycleTime != null
                                       select a);
                }
                //var VolumeCycleTime = (from a in entity.CLRDatas
                //                       where a.CycleTimeCategories != "0"
                //                       where a.CycleTimeCategories != "New Global/regional/local"
                //                       select new
                //                       {
                //                           January_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jan" ).Count(),
                //                           February_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Feb" ).Count(),
                //                           March_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Mar" ).Count(),
                //                           April_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Apr" ).Count(),
                //                           May_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "May" ).Count(),
                //                           June_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jun" ).Count(),
                //                           July_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jul" ).Count(),
                //                           August_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Aug" ).Count(),
                //                           September_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Sep" ).Count(),
                //                           October_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Oct" ).Count(),
                //                           November_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Nov" ).Count(),
                //                           December_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Dec" ).Count(),
                //                           January_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jan" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jan" ).Count(),
                //                           February_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Feb" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Feb" ).Count(),
                //                           March_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Mar" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Mar" ).Count(),
                //                           April_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Apr" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Apr" ).Count(),
                //                           May_A = VolumeCycleData.Where(x => x.GoLiveMonth == "May" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "May" ).Count(),
                //                           June_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jun" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jun" ).Count(),
                //                           July_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jul" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jul" ).Count(),
                //                           August_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Aug" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Aug" ).Count(),
                //                           September_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Sep" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Sep" ).Count(),
                //                           October_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Oct" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Oct" ).Count(),
                //                           November_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Nov" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Nov" ).Count(),
                //                           December_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Dec" ).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Dec" ).Count(),
                //                       }).Distinct();
                var H1 = "Jan,Feb,Mar,Apr,May,Jun".Split(',');
                var H2 = "Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                var CycleTimeByCategories = (from a in entity.CLRDatas
                                             where a.CycleTimeCategories != "0"
                                             where a.CycleTimeCategories != null
                                             where a.CycleTimeCategories != "New Global/regional/local"
                                             group a by a.CycleTimeCategories into g
                                             select new CycleTimeCategories
                                             {
                                                 CycleTimeCategory = g.Key,
                                                 January_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jan"  && x.CycleTimeCategories == g.Key).Count(),
                                                 February_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Feb"  && x.CycleTimeCategories == g.Key).Count(),
                                                 March_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Mar"  && x.CycleTimeCategories == g.Key).Count(),
                                                 April_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Apr"  && x.CycleTimeCategories == g.Key).Count(),
                                                 May_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "May"  && x.CycleTimeCategories == g.Key).Count(),
                                                 June_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jun"  && x.CycleTimeCategories == g.Key).Count(),
                                                 H_One_PC = 0,
                                                 July_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jul"  && x.CycleTimeCategories == g.Key).Count(),
                                                 August_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Aug"  && x.CycleTimeCategories == g.Key).Count(),
                                                 September_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Sep"  && x.CycleTimeCategories == g.Key).Count(),
                                                 October_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Oct"  && x.CycleTimeCategories == g.Key).Count(),
                                                 November_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Nov"  && x.CycleTimeCategories == g.Key).Count(),
                                                 December_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Dec"  && x.CycleTimeCategories == g.Key).Count(),
                                                 H_Two_PC = 0,
                                                 Total_PC = 0,
                                                 January_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jan" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jan" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 February_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Feb" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Feb" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 March_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Mar" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Mar" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 April_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Apr" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Apr" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 May_A = VolumeCycleData.Where(x => x.GoLiveMonth == "May" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "May" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 June_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jun" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jun" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 H_One_A = 0,
                                                 July_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jul" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jul" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 August_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Aug" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Aug" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 September_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Sep" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Sep" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 October_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Oct" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Oct" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 November_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Nov" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Nov" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 December_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Dec" && x.CycleTimeCategories == g.Key).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Dec" && x.CycleTimeCategories == g.Key).Count() ?? 0,
                                                 H_Two_A = 0,
                                                 Total_A = 0,
                                             }).ToList();
                foreach (var r in CycleTimeByCategories)
                {
                    r.H_One_PC = VolumeCycleData.Where(x => x.CycleTimeCategories == r.CycleTimeCategory && H1.Any(val => x.GoLiveMonth.Equals(val))).Count();
                    r.H_Two_PC = VolumeCycleData.Where(x => x.CycleTimeCategories == r.CycleTimeCategory && H2.Any(val => x.GoLiveMonth.Equals(val))).Count();
                    r.Total_PC = VolumeCycleData.Where(x => x.CycleTimeCategories == r.CycleTimeCategory).Count();
                    r.H_One_A = VolumeCycleData.Where(x => x.CycleTimeCategories == r.CycleTimeCategory && H1.Any(val => x.GoLiveMonth.Equals(val))).Sum(x=> x.CycleTime)/VolumeCycleData.Where(x => x.CycleTimeCategories == r.CycleTimeCategory && H1.Any(val => x.GoLiveMonth.Equals(val))).Count() ?? 0;
                    r.H_Two_A = VolumeCycleData.Where(x => x.CycleTimeCategories == r.CycleTimeCategory && H2.Any(val => x.GoLiveMonth.Equals(val))).Sum(x => x.CycleTime)/VolumeCycleData.Where(x => x.CycleTimeCategories == r.CycleTimeCategory && H2.Any(val => x.GoLiveMonth.Equals(val))).Count() ?? 0;
                    r.Total_A = VolumeCycleData.Where(x => x.CycleTimeCategories == r.CycleTimeCategory).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.CycleTimeCategories == r.CycleTimeCategory).Count() ?? 0;
                    //r.Total_A = VolumeCycleData.Sum(x => x.CycleTime) / VolumeCycleData.Select(x => x.CycleTime).Count() ?? 0;
                }
                var TotalCycleTImeCategories = (from a in entity.CLRDatas
                                                where a.CycleTimeCategories != "0"
                                                where a.CycleTimeCategories != null
                                                where a.CycleTimeCategories != "New Global/regional/local"
                                                select new CycleTimeCategories
                                                {
                                                    CycleTimeCategory = "Overall",
                                                    January_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jan").Count(),
                                                    February_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Feb").Count(),
                                                    March_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Mar").Count(),
                                                    April_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Apr").Count(),
                                                    May_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "May").Count(),
                                                    June_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jun").Count(),
                                                    H_One_PC = 0,
                                                    July_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Jul").Count(),
                                                    August_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Aug").Count(),
                                                    September_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Sep").Count(),
                                                    October_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Oct").Count(),
                                                    November_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Nov").Count(),
                                                    December_PC = VolumeCycleData.Where(x => x.GoLiveMonth == "Dec").Count(),
                                                    H_Two_PC = 0,
                                                    Total_PC = 0,
                                                    January_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jan").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jan").Count() ?? 0,
                                                    February_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Feb").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Feb").Count() ?? 0,
                                                    March_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Mar").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Mar").Count() ?? 0,
                                                    April_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Apr").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Apr").Count() ?? 0,
                                                    May_A = VolumeCycleData.Where(x => x.GoLiveMonth == "May").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "May").Count() ?? 0,
                                                    June_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jun").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jun").Count() ?? 0,
                                                    H_One_A = 0,
                                                    July_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Jul").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Jul").Count() ?? 0,
                                                    August_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Aug").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Aug").Count() ?? 0,
                                                    September_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Sep").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Sep").Count() ?? 0,
                                                    October_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Oct").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Oct").Count() ?? 0,
                                                    November_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Nov").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Nov").Count() ?? 0,
                                                    December_A = VolumeCycleData.Where(x => x.GoLiveMonth == "Dec").Sum(x => x.CycleTime) / VolumeCycleData.Where(x => x.GoLiveMonth == "Dec").Count() ?? 0,
                                                    H_Two_A = 0,
                                                    Total_A = 0,
                                                }).Distinct().ToList();
                foreach (var r in TotalCycleTImeCategories)
                {
                    r.H_One_PC = VolumeCycleData.Where(x => H1.Any(val => x.GoLiveMonth.Equals(val))).Count();
                    r.H_Two_PC = VolumeCycleData.Where(x => H2.Any(val => x.GoLiveMonth.Equals(val))).Count();
                    r.Total_PC = VolumeCycleData.Select(x => x.CycleTimeCategories).Count();
                    r.H_One_A = VolumeCycleData.Where(x => H1.Any(val => x.GoLiveMonth.Equals(val))).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => H1.Any(val => x.GoLiveMonth.Equals(val))).Count() ?? 0;
                    r.H_Two_A = VolumeCycleData.Where(x => H2.Any(val => x.GoLiveMonth.Equals(val))).Sum(x => x.CycleTime) / VolumeCycleData.Where(x => H2.Any(val => x.GoLiveMonth.Equals(val))).Count() ?? 0;
                    r.Total_A = VolumeCycleData.Sum(x => x.CycleTime) / VolumeCycleData.Select(x => x.CycleTime).Count() ?? 0;
                }
                var CycleTimeCategories = CycleTimeByCategories.Concat(TotalCycleTImeCategories);
                var CycleTimeData = (from a in VolumeCycleData
                                     select new
                                     {
                                         a.Client,
                                         a.RevenueID,
                                         a.Workspace_Title,
                                         a.MilestoneTitle,
                                         a.ImplementationType,
                                         a.Region,
                                         a.Country,
                                         a.ProjectStatus,
                                         a.ProjectLevel,
                                         a.GoLiveDate,
                                         a.ProjectStart_ForCycleTime,
                                         CycleTimeDelayCode = a.CycleTimeDelayCode == null || a.CycleTimeDelayCode == "" ? "---" : a.CycleTimeDelayCode ?? "---", 
                                         a.CycleTime,
                                         a.CycleTimeCategories,
                                         a.GoLiveYear,
                                         a.GoLiveMonth,
                                     }).ToList();
                imrs.code = 200;
                imrs.message = "Success";
                imrs.Data = TotalCycleTImeCategories;
                imrs.CycleTimeData = CycleTimeData;
                imrs.CycleTimeByCategories = CycleTimeCategories;
            }
            return imrs;
        }

        string[] PC_Projectstatus, PC_ProjectLevel, PC_GoLiveMonth, PC_Region, PC_ImplementationType, PC_Country, PC_Ownership;
        [HttpPost]
        [Route("ProjectCountByYear")]
        public Response ProjectCountByYear(CLRData clr)
        {
            if (clr.ProjectStatus == null || clr.ProjectLevel == null || clr.OwnerShip == null || clr.GoLiveMonth == null || clr.GoLiveYear == null || clr.Region == null || clr.ImplementationType == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                PC_Projectstatus = clr.ProjectStatus.Split(',');
                for (int i = 0; i < PC_Projectstatus.Count(); i++)
                {
                    if (PC_Projectstatus[i] == "" || PC_Projectstatus[i] == "---" || PC_Projectstatus[i] == "null")
                    {
                        PC_Projectstatus[i] = "";
                    }
                }
                PC_ProjectLevel = clr.ProjectLevel.Split(',');
                for (int i = 0; i < PC_ProjectLevel.Count(); i++)
                {
                    if (PC_ProjectLevel[i] == "" || PC_ProjectLevel[i] == "---" || PC_ProjectLevel[i] == "null")
                    {
                        PC_ProjectLevel[i] = "";
                    }
                }
                PC_GoLiveMonth = clr.GoLiveMonth.Split(',');
                for (int i = 0; i < PC_GoLiveMonth.Count(); i++)
                {
                    if (PC_GoLiveMonth[i] == "" || PC_GoLiveMonth[i] == "---" || PC_GoLiveMonth[i] == "null")
                    {
                        PC_GoLiveMonth[i] = "";
                    }
                }
                PC_Region = clr.Region.Split(',');
                for (int i = 0; i < PC_Region.Count(); i++)
                {
                    if (PC_Region[i] == "" || PC_Region[i] == "---" || PC_Region[i] == "null")
                    {
                        PC_Region[i] = "";
                    }
                }
                PC_ImplementationType = clr.ImplementationType.Split(',');
                for (int i = 0; i < PC_ImplementationType.Count(); i++)
                {
                    if (PC_ImplementationType[i] == "" || PC_ImplementationType[i] == "---" || PC_ImplementationType[i] == "null")
                    {
                        PC_ImplementationType[i] = "";
                    }
                }
                PC_Country = clr.Country.Split(',');
                for (int i = 0; i < PC_Country.Count(); i++)
                {
                    if (PC_Country[i] == "" || PC_Country[i] == "---" || PC_Country[i] == "null")
                    {
                        PC_Country[i] = "";
                    }
                }
                PC_Ownership = clr.OwnerShip.Split(',');
                for (int i = 0; i < PC_Ownership.Count(); i++)
                {
                    if (PC_Ownership[i] == "" || PC_Ownership[i] == "---" || PC_Ownership[i] == "null")
                    {
                        PC_Ownership[i] = null;
                    }
                }
                var ProjectCount = (from a in entity.CLRDatas
                                    where a.Status == "Active"
                                    where PC_Projectstatus.Any(val => a.ProjectStatus.Equals(val))
                                    where PC_ProjectLevel.Any(val => a.ProjectLevel.Equals(val))
                                    where PC_Ownership.Any(val => a.OwnerShip.Equals(val))
                                    where PC_GoLiveMonth.Any(val => a.GoLiveMonth.Equals(val))
                                    //where a.Go_Live_Year == Year1 || a.Go_Live_Year == Year2
                                    where PC_Region.Any(val => a.Region.Equals(val))
                                    where a.GoLiveYear == clr.GoLiveYear
                                    //where PC_Country.Any(val => a.Country.Equals(val))
                                    where a.RevenueID < 600000000000000
                                    where PC_ImplementationType.Any(val => a.ImplementationType.Equals(val))
                                    select a);
                var ProjectData = (from a in entity.CLRDatas
                                   select new
                                   {
                                       January = ProjectCount.Where(x => x.GoLiveMonth == "Jan").Count(),
                                       February = ProjectCount.Where(x => x.GoLiveMonth == "Feb").Count(),
                                       March = ProjectCount.Where(x => x.GoLiveMonth == "Mar").Count(),
                                       April = ProjectCount.Where(x => x.GoLiveMonth == "Apr").Count(),
                                       May = ProjectCount.Where(x => x.GoLiveMonth == "May").Count(),
                                       June = ProjectCount.Where(x => x.GoLiveMonth == "Jun").Count(),
                                       July = ProjectCount.Where(x => x.GoLiveMonth == "Jul").Count(),
                                       August = ProjectCount.Where(x => x.GoLiveMonth == "Aug").Count(),
                                       September = ProjectCount.Where(x => x.GoLiveMonth == "Sep").Count(),
                                       October = ProjectCount.Where(x => x.GoLiveMonth == "Oct").Count(),
                                       November = ProjectCount.Where(x => x.GoLiveMonth == "Nov").Count(),
                                       December = ProjectCount.Where(x => x.GoLiveMonth == "Dec").Count(),
                                   }).Distinct();
                re.Data = ProjectData;
                re.code = 200;
                re.message = "Success";
            }
            return re;
        }

        string[] CT_ProjectStatus, CT_Month, CT_Region, CT_Type, CT_level;
        [HttpPost]
        [Route("CycleTimeCharts")]
        public Response CycleTime(CLRData clr)
        {
            if (clr.ProjectStatus == null || clr.ImplementationType == null || clr.GoLiveMonth == null || clr.GoLiveYear == null || clr.Region == null || clr.ProjectLevel == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                CT_ProjectStatus = clr.ProjectStatus.Split(',');
                for (int i = 0; i < CT_ProjectStatus.Count(); i++)
                {
                    if (CT_ProjectStatus[i] == "" || CT_ProjectStatus[i] == "null")
                    {
                        CT_ProjectStatus[i] = null;
                    }
                }
                CT_level = clr.ProjectLevel.Split(',');
                for (int i = 0; i < CT_level.Count(); i++)
                {
                    if (CT_level[i] == "" || CT_level[i] == "null")
                    {
                        CT_level[i] = null;
                    }
                }
                CT_Month = clr.GoLiveMonth.Split(',');
                for (int i = 0; i < CT_Month.Count(); i++)
                {
                    if (CT_Month[i] == "" || CT_Month[i] == "null")
                    {
                        CT_Month[i] = null;
                    }
                }
                CT_Region = clr.Region.Split(',');
                for (int i = 0; i < CT_Region.Count(); i++)
                {
                    if (CT_Region[i] == "" || CT_Region[i] == "null")
                    {
                        CT_Region[i] = null;
                    }
                }
                CT_Type = clr.ImplementationType.Split(',');
                for (int i = 0; i < CT_Type.Count(); i++)
                {
                    if (CT_Type[i] == "" || CT_Type[i] == "null")
                    {
                        CT_Type[i] = null;
                    }
                }
                var ProjectCount = (from a in entity.CLRDatas
                                    where a.Status == "Active"
                                    where CT_ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                                    //where CV_GoLiveYear.Any(val => a.Go_Live_Year.Equals(val))
                                    where CT_Month.Any(val => a.GoLiveMonth.Equals(val))
                                    //where a.Go_Live_Year == Year1 || a.Go_Live_Year == Year2
                                    where CT_level.Any(val => a.ProjectLevel.Equals(val))
                                    where CT_Region.Any(val => a.Region.Equals(val))
                                    where a.GoLiveYear == clr.GoLiveYear
                                    where CT_Type.Any(val => a.ImplementationType.Equals(val))
                                    select a);
                var ProjectData = (from a in entity.CLRDatas
                                   select new
                                   {
                                       January_PC = ProjectCount.Where(x => x.GoLiveMonth == "Jan").Count(),
                                       February_PC = ProjectCount.Where(x => x.GoLiveMonth == "Feb").Count(),
                                       March_PC = ProjectCount.Where(x => x.GoLiveMonth == "Mar").Count(),
                                       April_PC = ProjectCount.Where(x => x.GoLiveMonth == "Apr").Count(),
                                       May_PC = ProjectCount.Where(x => x.GoLiveMonth == "May").Count(),
                                       June_PC = ProjectCount.Where(x => x.GoLiveMonth == "Jun").Count(),
                                       July_PC = ProjectCount.Where(x => x.GoLiveMonth == "Jul").Count(),
                                       August_PC = ProjectCount.Where(x => x.GoLiveMonth == "Aug").Count(),
                                       September_PC = ProjectCount.Where(x => x.GoLiveMonth == "Sep").Count(),
                                       October_PC = ProjectCount.Where(x => x.GoLiveMonth == "Oct").Count(),
                                       November_PC = ProjectCount.Where(x => x.GoLiveMonth == "Nov").Count(),
                                       December_PC = ProjectCount.Where(x => x.GoLiveMonth == "Dec").Count(),
                                       January_A = ProjectCount.Where(x => x.GoLiveMonth == "Jan").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Jan").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       February_A = ProjectCount.Where(x => x.GoLiveMonth == "Feb").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Feb").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       March_A = ProjectCount.Where(x => x.GoLiveMonth == "Mar").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Mar").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       April_A = ProjectCount.Where(x => x.GoLiveMonth == "Apr").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Apr").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       May_A = ProjectCount.Where(x => x.GoLiveMonth == "May").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "May").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       June_A = ProjectCount.Where(x => x.GoLiveMonth == "Jun").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Jun").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       July_A = ProjectCount.Where(x => x.GoLiveMonth == "Jul").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Jul").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       August_A = ProjectCount.Where(x => x.GoLiveMonth == "Aug").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Aug").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       September_A = ProjectCount.Where(x => x.GoLiveMonth == "Sep").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Sep").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       October_A = ProjectCount.Where(x => x.GoLiveMonth == "Oct").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Oct").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       November_A = ProjectCount.Where(x => x.GoLiveMonth == "Nov").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Nov").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                       December_A = ProjectCount.Where(x => x.GoLiveMonth == "Dec").Where(x => x.ProjectStart_ForCycleTime != null).Sum(x => x.CycleTime) / ProjectCount.Where(x => x.GoLiveMonth == "Dec").Where(x => x.ProjectStart_ForCycleTime != null).Count(),
                                   }).Distinct();
                re.code = 200;
                re.message = "Success";
                re.Data = ProjectData;
            }
            return re;
        }

        string[] Opportunity_Type, Opportunity_Type2, Revenue_Opportunity_Type;
        [HttpPost]
        [Route("Testing")]
        public Response Testing(iMeetData iMeetData)
        {
            Opportunity_Type = "New Business,Up-Sell(Add Offices/Countries)".Split(',');
            var Status = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            Opportunity_Type2 = "Re-Bid With Up-Sell,Lost Client (w/o notice)".Split(',');
            Revenue_Opportunity_Type = "Up-Sell(Add Offices/Countries),Re-Bid With Up-Sell,New Business".Split(',');
            var GenerateCLR = (from a in entity.iMeetDatas
                                   //where a.Task_Start_Date >= ConvertedDate
                                   //where Status.Any(val => a.Milestone__Project_Status.Equals(val))
                               join b in entity.CRMDatas on a.Milestone__CRM_Revenue_ID__ equals b.Revenue_Id
                               select new
                               {
                                   RevenueID = a.Milestone__CRM_Revenue_ID__,
                                   b.Revenue_Id,
                                   //Region = b.Region__Revenue_ ?? "Blanks",
                                   //Country = b.Country ?? "Blanks",
                                   //OwnerShip = b.Ownership__Revenue_ ?? "Blanks",
                                   //GoLiveDate = a.Task_Start_Date,
                                   //TaskDueDate = a.Task_Due_Date,
                                   ////Year = a.Task_Start_Date.Value.Month.ToString("yyyy"),
                                   //ProjectStatus = a.Milestone__Project_Status ?? "Blanks",
                                   //CountryStatus = a.Milestone__Country_Status ?? "Blanks",
                                   //ProjectLevel = a.Workspace__Project_Level ?? "Blanks",
                                   //ProjectStartDate = a.Milestone__Project_Start_Date,
                                   //IntitialGoliveDate = a.Milestone__Initial_Go_Live_Date,
                                   //CompletedDate = a.Completed_Date,
                                   //ProjectOwner = a.Workspace__Project_Owner__Full_Name ?? "Blanks",
                                   //AssigneeFullName = a.Milestone__Assignee__Full_Name ?? "Blanks",
                                   //MilestoneTitle = a.Milestone_Title ?? "Blanks",
                                   //Milestone__Record_ID_Key = a.Milestone__Record_ID_Key ?? "Blanks",
                                   //Task__Task_Record_ID_Key = a.Task__Task_Record_ID_Key ?? "Blanks",
                                   //Group_Name = a.Group_Name ?? "Blanks",
                                   //Milestone__Project_Notes = a.Milestone__Project_Notes ?? "Blanks",
                                   //Milestone__Reason_Code = a.Milestone__Reason_Code ?? "Blanks",
                                   //Milestone__Closed_Loop_Owner = a.Milestone__Closed_Loop_Owner ?? "Blanks",
                                   //Workspace_Title = a.Workspace_Title ?? "Blanks",
                                   //Workspace__ELT_Overall_Status = a.Workspace__ELT_Overall_Status ?? "Blanks",
                                   //Workspace__ELT_Overall_Comments = a.Workspace__ELT_Overall_Comments ?? "Blanks",
                                   //b.Customer_Row_ID,
                                   //b.Opportunity_ID,
                                   //Account_Name = b.Account_Name ?? "Blanks",
                                   //Sales_Stage_Name = b.Sales_Stage_Name ?? "Blanks",
                                   //Opportunity_Type = b.Opportunity_Type ?? "Blanks",
                                   //Revenue_Opportunity_Type = b.Revenue_Opportunity_Type ?? "Blanks",
                                   //Revenue_Status = b.Revenue_Status ?? "Blanks",
                                   //Opportunity_Owner = b.Opportunity_Owner ?? "Blanks",
                                   //Opportunity_Category = b.Opportunity_Category ?? "Blanks",
                                   //b.Revenue_Total_Transactions,
                                   //CountryCode = entity.CountryIsoCodes.Where(x => x.CountryName == b.Country).Select(x => x.CountryCode),
                                   ////RevenueVolumeUSD = b.Opportunity_Type == "New Business" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Up-Sell(Add Offices/Countries)" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Re-Bid" ? 0 : b.Opportunity_Type == "Renewal/Renegotiation" ? 0 : b.Opportunity_Type == null ? 0 : b.Revenue_Opportunity_Type == "" ? 0 : 0,
                                   //RevenueVolumeUSD = Opportunity_Type.Any(val => b.Opportunity_Type.Equals(val)) ? b.Revenue_Total_Volume_USD
                                   //                     : Opportunity_Type2.Any(val => b.Opportunity_Type.Equals(val))
                                   //                         ? Revenue_Opportunity_Type.Any(val => b.Revenue_Opportunity_Type.Equals(val)) ? b.Revenue_Total_Volume_USD
                                   //                         : 0
                                   //                     : 0,
                                   //MarketLeader = b.Opportunity_Category == "GPS" ? "Cathy Voss"
                                   //                 : b.Opportunity_Category == null ? "No data from CRM"
                                   //                 : b.Region__Revenue_ == "APAC" ? "Bindu Bhatia"
                                   //                 : b.Region__Revenue_ == "EMEA" ? "Chris Bowen"
                                   //                 : b.Region__Revenue_ == "NORAM" ? "Barbara Bernard"
                                   //                 : b.Region__Revenue_ == "LATAM" ? "Barbara Bernard"
                                   //                 : "No data from CRM",

                                   //GlobalProjectManager = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Global_Project_Manager).Distinct(),
                                   //ProjectConsultant = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Regional_Ops_Manager).Distinct(),
                                   ////RegionalProjectManager = b.Region__Revenue_ == "APAC" ? entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.APAC_IMP_Lead).Distinct()
                                   ////                             : b.Region__Revenue_ == "LATAM" ? entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.LATAM_Telephony_Team).Distinct()
                                   ////                             : b.Region__Revenue_ == "NORAM" ? entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.NORAM_Real_Estate_and_Facilities).Distinct()
                                   ////                             : b.Region__Revenue_ == "EMEA" ? entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.EMEA_Project_Manager).Distinct()
                                   ////                             : entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.EMEA_Project_Manager).Distinct(),
                                   //GlobalCISOBTLead = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Global_CIS_OBT_Lead).Distinct(),
                                   //GlobalCISHRFeedSpecialist = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Global_CIS_HR_Feed_Specialist).Distinct(),
                                   //GlobalCISPortraitLead = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Global_CIS_Portrait_Lead).Distinct(),
                               }).OrderBy(x => x.RevenueID);

            re.code = 200;
            re.message = "Success";
            re.Data = GenerateCLR;
            return re;
        }
    }

   
}
