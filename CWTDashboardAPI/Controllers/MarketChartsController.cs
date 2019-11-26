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

        CWTEntities entity = new CWTEntities();
        Response re = new Response();
        Filters fi = new Filters();

        string[] CV_Projectstatus,CV_ProjectLevel,CV_GoLiveYear, CV_Go_Live_Month;
        [HttpPost]
        [Route("ChartVolumeCycleTimeCount")]
        public Response ChartVolumeCycleTimeCount(CLR clr)
        {
            if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Go_Live_Year == null || clr.Go_Live_Month == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                CV_Projectstatus = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < CV_Projectstatus.Count(); i++)
                {
                    if (CV_Projectstatus[i] == "" || CV_Projectstatus[i] == "null")
                    {
                        CV_Projectstatus[i] = null;
                    }
                }
                CV_ProjectLevel = clr.iMeet_Project_Level.Split(',');
                for (int i = 0; i < CV_ProjectLevel.Count(); i++)
                {
                    if (CV_ProjectLevel[i] == "" || CV_ProjectLevel[i] == "null")
                    {
                        CV_ProjectLevel[i] = null;
                    }
                }
                CV_GoLiveYear = clr.Go_Live_Year.Split(',');
                for (int i = 0; i < CV_GoLiveYear.Count(); i++)
                {
                    if (CV_GoLiveYear[i] == "" || CV_GoLiveYear[i] == "null")
                    {
                        CV_GoLiveYear[i] = null;
                    }
                }
                CV_Go_Live_Month = clr.Go_Live_Month.Split(',');
                for (int i = 0; i < CV_Go_Live_Month.Count(); i++)
                {
                    if (CV_Go_Live_Month[i] == "" || CV_Go_Live_Month[i] == "null")
                    {
                        CV_Go_Live_Month[i] = null;
                    }
                }
                var VolumeCycleTime = (from a in entity.CLRs
                                       where CV_Projectstatus.Any(val => a.iMeet_Milestone___Project_Status.Equals(val))
                                       where CV_ProjectLevel.Any(val => a.iMeet_Project_Level.Equals(val))
                                       where CV_GoLiveYear.Any(val => a.Go_Live_Year.Equals(val))
                                       where CV_Go_Live_Month.Any(val => a.Go_Live_Month.Equals(val))
                                       group a by a.Go_Live_Month into g
                                       select new
                                       {
                                           Go_Live_Month = g.Key,
                                           Revenue_Total_Volume_USD = g.Sum(x => x.Revenue_Total_Volume_USD),
                                           ProjectsCount = g.Count(),
                                           Average = g.Sum(x => x.Cycle_Time) / g.Count(),
                                       }).AsEnumerable().OrderBy(x => DateTime.ParseExact(x.Go_Live_Month, "MMM", CultureInfo.InvariantCulture).Month);
                              //.OrderBy(x=> x.Go_Live_Month == "Jan" ? 1 : x.Go_Live_Month == "Feb" ? 2 : x.Go_Live_Month == "Mar" ? 3 : x.Go_Live_Month == "Apr" ? 4 : x.Go_Live_Month == "May" ? 5 : x.Go_Live_Month == "Jun" ? 6 : x.Go_Live_Month == "Jul" ? 7 : x.Go_Live_Month == "Aug" ? 8 : x.Go_Live_Month == "Sep" ? 9 : x.Go_Live_Month == "Oct" ? 10 : x.Go_Live_Month == "Nov" ? 11 : x.Go_Live_Month == "Dec" ? 12 : 12 );
                re.code = 200;
                re.message = "Success";
                re.Data = VolumeCycleTime;
            }
            return re;
        }

        string Year1, Year2;

        string[] PC_Projectstatus, PC_ProjectLevel, PC_Go_Live_Month;
        [HttpPost]
        [Route("ProjectCountByYear")]
        public Response ProjectCountByYear(CLR clr)
        {
            if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Go_Live_Month == null || clr.Go_Live_Year == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                PC_Projectstatus = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < PC_Projectstatus.Count(); i++)
                {
                    if (PC_Projectstatus[i] == "" || PC_Projectstatus[i] == "null")
                    {
                        PC_Projectstatus[i] = null;
                    }
                }
                PC_ProjectLevel = clr.iMeet_Project_Level.Split(',');
                for (int i = 0; i < PC_ProjectLevel.Count(); i++)
                {
                    if (PC_ProjectLevel[i] == "" || PC_ProjectLevel[i] == "null")
                    {
                        PC_ProjectLevel[i] = null;
                    }
                }
                PC_Go_Live_Month = clr.Go_Live_Month.Split(',');
                for (int i = 0; i < PC_Go_Live_Month.Count(); i++)
                {
                    if (PC_Go_Live_Month[i] == "" || PC_Go_Live_Month[i] == "null")
                    {
                        PC_Go_Live_Month[i] = null;
                    }
                }
                var ProjectCount = (from a in entity.CLRs
                                    where PC_Projectstatus.Any(val => a.iMeet_Milestone___Project_Status.Equals(val))
                                    where PC_ProjectLevel.Any(val => a.iMeet_Project_Level.Equals(val))
                                    //where CV_GoLiveYear.Any(val => a.Go_Live_Year.Equals(val))
                                    where PC_Go_Live_Month.Any(val => a.Go_Live_Month.Equals(val))
                                    //where a.Go_Live_Year == Year1 || a.Go_Live_Year == Year2
                                    where a.Go_Live_Year == clr.Go_Live_Year
                                    select a);
                var ProjectData = (from a in entity.CLRs
                                   select new
                                   {
                                       January = ProjectCount.Where(x => x.Go_Live_Month == "Jan").Count(),
                                       February = ProjectCount.Where(x => x.Go_Live_Month == "Feb").Count(),
                                       March = ProjectCount.Where(x => x.Go_Live_Month == "Mar").Count(),
                                       April = ProjectCount.Where(x => x.Go_Live_Month == "Apr").Count(),
                                       May = ProjectCount.Where(x => x.Go_Live_Month == "May").Count(),
                                       June = ProjectCount.Where(x => x.Go_Live_Month == "Jun").Count(),
                                       July = ProjectCount.Where(x => x.Go_Live_Month == "Jul").Count(),
                                       August = ProjectCount.Where(x => x.Go_Live_Month == "Aug").Count(),
                                       September = ProjectCount.Where(x => x.Go_Live_Month == "Sep").Count(),
                                       October = ProjectCount.Where(x => x.Go_Live_Month == "Oct").Count(),
                                       November = ProjectCount.Where(x => x.Go_Live_Month == "Nov").Count(),
                                       December = ProjectCount.Where(x => x.Go_Live_Month == "Dec").Count(),
                                   }).Distinct();
                re.Data = ProjectData;
                re.code = 200;
                re.message = "Success";
            }
            return re;
        }
    }
}