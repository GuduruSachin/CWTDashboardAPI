using CWTDashboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CWTDashboardAPI.Controllers
{
    public class CriticalTaskOverDueController : ApiController
    {
        CWTEntities entity = new CWTEntities();
        Response re = new Response();
        CTOFilters fi = new CTOFilters();
        string[] projectstatus, groupname, projectlevel, region;
        [HttpPost]
        [Route("CriticalTaskOverDue")]
        public Response CriticalTaskOverDue(CTO cto)
        {
            if (cto.Milestone__Project_Status == null || cto.Group_Name == null || cto.Workspace__Project_Level == null || cto.Milestone__Region == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                projectstatus = cto.Milestone__Project_Status.Split(',');
                for (int i = 0; i < projectstatus.Count(); i++)
                {
                    if (projectstatus[i] == "")
                    {
                        projectstatus[i] = null;
                    }
                }
                //criticaloverdue = cto.CriticalTask_Overdue.Split(',');
                //for (int i = 0; i < criticaloverdue.Count(); i++)
                //{
                //    if (criticaloverdue[i] == "")
                //    {
                //        criticaloverdue[i] = null;
                //    }
                //}
                groupname = cto.Group_Name.Split(',');
                for (int i = 0; i < groupname.Count(); i++)
                {
                    if (groupname[i] == "")
                    {
                        groupname[i] = null;
                    }
                }
                projectlevel = cto.Workspace__Project_Level.Split(',');
                for (int i = 0; i < projectlevel.Count(); i++)
                {
                    if (projectlevel[i] == "")
                    {
                        projectlevel[i] = null;
                    }
                }
                region = cto.Milestone__Region.Split(',');
                for (int i = 0; i < region.Count(); i++)
                {
                    if (region[i] == "")
                    {
                        region[i] = null;
                    }
                }
                var TaskOverdueList = (from a in entity.CTOes
                                       where projectstatus.Any(val1 => a.Milestone__Project_Status.Equals(val1))
                                       where a.Critical_Overdue == 1
                                       where groupname.Any(val3 => a.Group_Name.Equals(val3))
                                       where projectlevel.Any(val4 => a.Workspace__Project_Level.Equals(val4))
                                       where region.Any(val5 => a.Milestone__Region.Equals(val5))
                                       //where country.Any(val6 => a.Milestone__Country.Equals(val6))
                                       select new
                                       {
                                           a.Workspace_Title,
                                           a.Milestone_Title_Country___Est_Go_Live_Date,
                                           a.Task_Title,
                                           a.Last_Comment,
                                           a.Critical_Overdue
                                       }).ToList();
                //var CriticalTaskOverDueList = (from b in TaskOverdueList
                //                               select new
                //                               {
                //                                   b.Workspace_Title,
                //                                   b.Milestone_Title_Country___Est_Go_Live_Date,
                //                                   b.Task_Title,
                //                                   b.Last_Comment,
                //                                   b.Critical_Overdue
                //                               });
                re.code = 200;
                re.message = "Success";
                re.Data = TaskOverdueList;
            }
            //IEnumerable<string> projectstatus = cto.Milestone__Project_Status.Split(',');
            //IEnumerable<double> criticaloverdue = cto.CriticalTask_Overdue.Split(',').Select(double.Parse);
            //IEnumerable<string> groupname = cto.Group_Name.Split(',');
            //IEnumerable<string> projectlevel = cto.Workspace__Project_Level.Split(',');
            //IEnumerable<string> region = cto.Milestone__Region.Split(',');
            //IEnumerable<string> country = cto.Milestone__Country.Split(',');
            return re;
        }
        [HttpPost]
        [Route("CriticalTaskFiltersList")]
        public CTOFilters FiltersList(CTO cto)
        {
            var FilterProjectStatus = (from a in entity.CTOes
                              select new
                              {
                                  a.Milestone__Project_Status,
                                  isSelected = true,
                              }).Distinct().OrderBy(x => x.Milestone__Project_Status);
            var FilterCriticalOverDue = (from a in entity.CTOes
                                         where a.Critical_Overdue != null
                                       select new
                                       {
                                           a.Critical_Overdue,
                                           isSelected = true,
                                       }).Distinct().OrderBy(x => x.Critical_Overdue);
            var FilterGroupName = (from a in entity.CTOes
                                         select new
                                         {
                                             a.Group_Name,
                                             isSelected = true,
                                         }).Distinct().OrderBy(x => x.Group_Name);
            var FilterProjectLevel = (from a in entity.CTOes
                                   select new
                                   {
                                       a.Workspace__Project_Level,
                                       isSelected = true,
                                   }).Distinct().OrderBy(x => x.Workspace__Project_Level);
            var FilterRegion = (from a in entity.CTOes
                                      select new
                                      {
                                          a.Milestone__Region,
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.Milestone__Region);
            var FilterCountry = (from a in entity.CTOes
                                select new
                                {
                                    a.Milestone__Country,
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Milestone__Country);

            fi.code = 200;
            fi.message = "Success";
            fi.ProjectStatus = FilterProjectStatus;
            fi.CriticalOverDue = FilterCriticalOverDue;
            fi.GroupName = FilterGroupName;
            fi.ProjectLevel = FilterProjectLevel;
            fi.Region = FilterRegion;
            fi.Country = FilterCountry;
            return fi;
        }

        [HttpPost]
        [Route("RegionMonthWiseCTODCount")]
        public  Response RegionMonthWiseCTODCount(CTO cto)
        {
            var RegionMonthWiseCTODCount = (from a in entity.CTOes
                                            group a by a.Milestone__Region into g
                                            select new
                                            {
                                                Region = g.Key,
                                                RegionCount = g.Sum(x => x.Critical_Overdue),
                                            }).OrderBy(x => x.Region);
            re.code = 200;
            re.message = "Success";
            re.Data = RegionMonthWiseCTODCount;
            return re;
        }
    }
}