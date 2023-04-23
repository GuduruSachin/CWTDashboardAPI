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
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        CTOFilters fi = new CTOFilters();
        int CTOCount, SWCTOCount, RWCTOCount, GNCTOCount;
        string[] groupname, projectlevel, region, Assigne;
        [HttpPost]
        [Route("CriticalTaskOverDue")]
        public Response CriticalTaskOverDue(CTO cto)
        {
            if (cto.Group_Name == null || cto.Workspace__Project_Level == null || cto.Milestone__Region == null || cto.Milestone__Assignee__Full_Name == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
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
                Assigne = cto.Milestone__Assignee__Full_Name.Split(',');
                for (int i = 0; i < Assigne.Count(); i++)
                {
                    if (Assigne[i] == "")
                    {
                        Assigne[i] = null;
                    }
                }
                var TaskOverdueList = (from a in entity.CTOes
                                       where groupname.Any(val3 => a.Group_Name.Equals(val3))
                                       where projectlevel.Any(val4 => a.Workspace__Project_Level.Equals(val4))
                                       where region.Any(val5 => a.Milestone__Region.Equals(val5))
                                       where Assigne.Any(val5 => a.Milestone__Assignee__Full_Name.Equals(val5))
                                       select new
                                       {
                                           a.Milestone_Title_Country___Est_Go_Live_Date,
                                           a.Critical_Overdue,
                                           a.Estimated_Go_Live,
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
                                           a.Milestone__Project_Start_Date,
                                           a.Last_Comment,
                                           OwnershipRevenue = entity.CLRDatas.FirstOrDefault(x => x.RevenueID == a.RevenurID).OwnerShip == "" || entity.CLRDatas.FirstOrDefault(x => x.RevenueID == a.RevenurID).OwnerShip == null ? "---" : entity.CLRDatas.FirstOrDefault(x => x.RevenueID == a.RevenurID).OwnerShip
                                       }).ToList();
                CTOCount = TaskOverdueList.AsQueryable().Count();
                if (CTOCount.ToString() == "" || CTOCount.ToString() == null || CTOCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data Found";
                    re.Data = TaskOverdueList;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = TaskOverdueList;
                }
            }
            return re;
        }
        
        [HttpPost]
        [Route("CriticalTaskOverDueExcel")]
        public Response CriticalTaskOverDueExcel(CTO cto)
        {
            var TaskOverdueList = (from a in entity.CTOes
                            select new
                            {
                                a.Milestone_Title_Country___Est_Go_Live_Date,
                                a.Critical_Overdue,
                                a.Estimated_Go_Live,
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
                                a.Milestone__Project_Start_Date,
                                a.Last_Comment
                            }).ToList();
            re.code = 200;
            re.message = "Success";
            re.Data = TaskOverdueList;
            return re;
        }

        [HttpPost]
        [Route("CriticalTaskFiltersList")]
        public CTOFilters FiltersList(CTO cto)
        {
            //var FilterProjectStatus = (from a in entity.CTOes
            //                           select new
            //                           {
            //                               a.Milestone__Project_Status,
            //                               isSelected = true,
            //                           }).Distinct().OrderBy(x => x.Milestone__Project_Status);
            //var FilterCriticalOverDue = (from a in entity.CTOes
            //                             where a.Critical_Overdue != null
            //                             select new
            //                             {
            //                                 a.Critical_Overdue,
            //                                 isSelected = true,
            //                             }).Distinct().OrderBy(x => x.Critical_Overdue);
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
            var FilterAssignePerson = (from a in entity.CTOes
                                 select new
                                 {
                                     AssigneFullName = a.Milestone__Assignee__Full_Name,
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.AssigneFullName);

            fi.code = 200;
            fi.message = "Success";
            //fi.ProjectStatus = FilterProjectStatus;
            //fi.CriticalOverDue = FilterCriticalOverDue;
            fi.GroupName = FilterGroupName;
            fi.ProjectLevel = FilterProjectLevel;
            fi.Region = FilterRegion;
            fi.AssigneFullName = FilterAssignePerson;
            return fi;
        }

        [HttpPost]
        [Route("RegionMonthWiseCTODCount")]
        public Response RegionMonthWiseCTODCount(CTO cto)
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

        string[] SWCGroupName, SWCProjectLevel, SWCRegion;
        [HttpPost]
        [Route("StatusWiseCount")]
        public Response StatusWiseCount(CTO cto)
        {
            if (cto.Group_Name == null || cto.Workspace__Project_Level == null || cto.Milestone__Region == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                SWCGroupName = cto.Group_Name.Split(',');
                for (int i = 0; i < SWCGroupName.Count(); i++)
                {
                    if (SWCGroupName[i] == "")
                    {
                        SWCGroupName[i] = null;
                    }
                }
                SWCProjectLevel = cto.Workspace__Project_Level.Split(',');
                for (int i = 0; i < SWCProjectLevel.Count(); i++)
                {
                    if (SWCProjectLevel[i] == "")
                    {
                        SWCProjectLevel[i] = null;
                    }
                }
                SWCRegion = cto.Milestone__Region.Split(',');
                for (int i = 0; i < SWCRegion.Count(); i++)
                {
                    if (SWCRegion[i] == "")
                    {
                        SWCRegion[i] = null;
                    }
                }
                var StatusWiseCount = (from a in entity.CTOes
                                       where SWCRegion.Any(val => a.Milestone__Region.Equals(val))
                                       where SWCGroupName.Any(val => a.Group_Name.Equals(val))
                                       where SWCProjectLevel.Any(val => a.Workspace__Project_Level.Equals(val))
                                       group a by a.Milestone__Project_Status into g
                                       select new
                                       {
                                           Milestone__Project_Status = g.Key == null || g.Key == "" ? "(Blank)" : g.Key,
                                           ProjectsCount = g.Count(),
                                       }).OrderBy(x => x.Milestone__Project_Status);
                SWCTOCount = StatusWiseCount.AsQueryable().Count();
                if (SWCTOCount.ToString() == "" || SWCTOCount.ToString() == null || SWCTOCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data Found";
                    re.Data = StatusWiseCount;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = StatusWiseCount;
                }
            }
            return re;
        }

        string[] RWGroupName, RWProjectLevel, RWRegion, RWAssigne;
        [HttpPost]
        [Route("RegionWiseCount")]
        public Response RegionWiseCount(CTO cto)
        {
            if (cto.Group_Name == null || cto.Workspace__Project_Level == null || cto.Milestone__Region == null || cto.Milestone__Assignee__Full_Name == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                RWGroupName = cto.Group_Name.Split(',');
                for (int i = 0; i < RWGroupName.Count(); i++)
                {
                    if (RWGroupName[i] == "")
                    {
                        RWGroupName[i] = null;
                    }
                }
                RWProjectLevel = cto.Workspace__Project_Level.Split(',');
                for (int i = 0; i < RWProjectLevel.Count(); i++)
                {
                    if (RWProjectLevel[i] == "")
                    {
                        RWProjectLevel[i] = null;
                    }
                }
                RWRegion = cto.Milestone__Region.Split(',');
                for (int i = 0; i < RWRegion.Count(); i++)
                {
                    if (RWRegion[i] == "")
                    {
                        RWRegion[i] = null;
                    }
                }
                RWAssigne = cto.Milestone__Assignee__Full_Name.Split(',');
                for (int i = 0; i < RWAssigne.Count(); i++)
                {
                    if (RWAssigne[i] == "")
                    {
                        RWAssigne[i] = null;
                    }
                }
                var RegionWiseCount = (from a in entity.CTOes
                                       where RWRegion.Any(val => a.Milestone__Region.Equals(val))
                                       where RWGroupName.Any(val => a.Group_Name.Equals(val))
                                       where RWProjectLevel.Any(val => a.Workspace__Project_Level.Equals(val))
                                       where RWAssigne.Any(val => a.Milestone__Assignee__Full_Name.Equals(val))
                                       group a by a.Milestone__Region into g
                                       select new
                                       {
                                           Milestone__Region = g.Key == null || g.Key == "" ? "(Blank)" : g.Key,
                                           ProjectsCount = g.Count(),
                                       }).OrderBy(x => x.Milestone__Region);
                RWCTOCount = RegionWiseCount.AsQueryable().Count();
                if (RWCTOCount.ToString() == "" || RWCTOCount.ToString() == null || RWCTOCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data Found";
                    re.Data = RegionWiseCount;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = RegionWiseCount;
                }
            }
            return re;
        }

        string[] GNGroupName, GNProjectLevel, GNRegion, GNAssigne;
        [HttpPost]
        [Route("GroupNameCountCTO")]
        public Response GroupNameCountCTO(CTO cto)
        {
            if (cto.Group_Name == null || cto.Workspace__Project_Level == null || cto.Milestone__Region == null || cto.Milestone__Assignee__Full_Name == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                GNGroupName = cto.Group_Name.Split(',');
                for (int i = 0; i < GNGroupName.Count(); i++)
                {
                    if (GNGroupName[i] == "")
                    {
                        GNGroupName[i] = null;
                    }
                }
                GNProjectLevel = cto.Workspace__Project_Level.Split(',');
                for (int i = 0; i < GNProjectLevel.Count(); i++)
                {
                    if (GNProjectLevel[i] == "")
                    {
                        GNProjectLevel[i] = null;
                    }
                }
                GNRegion = cto.Milestone__Region.Split(',');
                for (int i = 0; i < GNRegion.Count(); i++)
                {
                    if (GNRegion[i] == "")
                    {
                        GNRegion[i] = null;
                    }
                }
                GNAssigne = cto.Milestone__Assignee__Full_Name.Split(',');
                for (int i = 0; i < GNAssigne.Count(); i++)
                {
                    if (GNAssigne[i] == "")
                    {
                        GNAssigne[i] = null;
                    }
                }
                var GroupNamesData = (from a in entity.CTOes
                                       where GNRegion.Any(val => a.Milestone__Region.Equals(val))
                                       where GNGroupName.Any(val => a.Group_Name.Equals(val))
                                       where GNAssigne.Any(val => a.Milestone__Assignee__Full_Name.Equals(val))
                                       where GNProjectLevel.Any(val => a.Workspace__Project_Level.Equals(val))
                                       group a by a.Group_Name into g
                                       select new
                                       {
                                           Group_Name = g.Key == null || g.Key == "" ? "(Blank)" : g.Key,
                                           ProjectsCount = g.Count(),
                                       }).OrderBy(x => x.Group_Name);
                GNCTOCount = GroupNamesData.AsQueryable().Count();
                if (GNCTOCount.ToString() == "" || GNCTOCount.ToString() == null || GNCTOCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data Found";
                    re.Data = GroupNamesData;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = GroupNamesData;
                }
            }
            return re;
        }
    }
}