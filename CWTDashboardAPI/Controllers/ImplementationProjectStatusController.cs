using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CWTDashboardAPI.Models;

namespace CWTDashboardAPI.Controllers
{
    public class ImplementationProjectStatusController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        IMPSFilters fi = new IMPSFilters();
        int LevelCount, StatusCount, LeadersCount,GroupNameCount, RegionsCount;
        string[] PLCstatus, PLClevels, PLCregions, PLCAssign_Report, PLCGroupName;
        [HttpPost]
        [Route("ProjectLevelCount")]
        public Response ProjectLevelCount(IMP imp)
        {
            if (imp.Milestone__Project_Status == null || imp.Workspace__Project_Level == null || imp.Milestone__Region == null || imp.Milestone__Assignee__Reports_to__Full_Name == null || imp.Task_Start_Date == null || imp.Group_Name == null || imp.Milestone__Project_Start_Date == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                PLCstatus = imp.Milestone__Project_Status.Split(',');
                for (int i = 0; i < PLCstatus.Count(); i++)
                {
                    if (PLCstatus[i] == "")
                    {
                        PLCstatus[i] = null;
                    }
                }
                PLClevels = imp.Workspace__Project_Level.Split(',');
                for (int i = 0; i < PLClevels.Count(); i++)
                {
                    if (PLClevels[i] == "")
                    {
                        PLClevels[i] = null;
                    }
                }
                PLCregions = imp.Milestone__Region.Split(',');
                for (int i = 0; i < PLCregions.Count(); i++)
                {
                    if (PLCregions[i] == "")
                    {
                        PLCregions[i] = null;
                    }
                }
                PLCAssign_Report = imp.Milestone__Assignee__Reports_to__Full_Name.Split(',');
                for (int i = 0; i < PLCAssign_Report.Count(); i++)
                {
                    if (PLCAssign_Report[i] == "")
                    {
                        PLCAssign_Report[i] = null;
                    }
                }
                PLCGroupName = imp.Group_Name.Split(',');
                for (int i = 0; i < PLCGroupName.Count(); i++)
                {
                    if (PLCGroupName[i] == "")
                    {
                        PLCGroupName[i] = null;
                    }
                }
                var ProjectLevelCountData = (from a in entity.IMPS
                                         //where a.Task_Start_Date >= imp.Task_Start_Date
                                         where a.Task_Start_Date >= imp.Task_Start_Date && a.Task_Start_Date <= imp.Milestone__Project_Start_Date
                                         where PLCGroupName.Any(val1 => a.Group_Name.Equals(val1))
                                         where PLCstatus.Any(val1 => a.Milestone__Project_Status.Equals(val1))
                                         where PLClevels.Any(val2 => a.Workspace__Project_Level.Equals(val2))
                                         where PLCregions.Any(val3 => a.Milestone__Region.Equals(val3))
                                         where PLCAssign_Report.Any(val5 => a.Milestone__Assignee__Reports_to__Full_Name.Equals(val5))
                                         select a);
                var ProjectLevelCount = (from a in ProjectLevelCountData
                                         select new
                                         {
                                             Global = ProjectLevelCountData.Where(val1 => val1.Workspace__Project_Level == "Global").Select(x1 => x1.Workspace_Title).Count(),
                                             Local = ProjectLevelCountData.Where(val2 => val2.Workspace__Project_Level == "Local").Select(x2 => x2.Workspace_Title).Count(),
                                             Regional = ProjectLevelCountData.Where(val2 => val2.Workspace__Project_Level == "Regional").Select(x2 => x2.Workspace_Title).Count(),
                                             Blanks = ProjectLevelCountData.Where(val2 => val2.Workspace__Project_Level == null).Select(x2 => x2.Workspace_Title).Count(),
                                         }).Distinct();
                LevelCount = ProjectLevelCount.AsQueryable().Count();
                if (LevelCount.ToString() == "" || LevelCount.ToString() == null || LevelCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data Found";
                    re.Data = ProjectLevelCount;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = ProjectLevelCount;
                }
            }
            return re;
        }
        string[] SCstatus, SClevels, SCregions, SCAssign_Report, SCGroupName;
        [HttpPost]
        [Route("ProjectStatusCount")]
        public Response ProjectStatusCount(IMP imp)
        {
            if (imp.Milestone__Project_Status == null || imp.Workspace__Project_Level == null || imp.Milestone__Region == null || imp.Milestone__Assignee__Reports_to__Full_Name == null || imp.Task_Start_Date == null || imp.Group_Name == null || imp.Milestone__Project_Start_Date == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                SCstatus = imp.Milestone__Project_Status.Split(',');
                for (int i = 0; i < SCstatus.Count(); i++)
                {
                    if (SCstatus[i] == "")
                    {
                        SCstatus[i] = null;
                    }
                }
                SClevels = imp.Workspace__Project_Level.Split(',');
                for (int i = 0; i < SClevels.Count(); i++)
                {
                    if (SClevels[i] == "")
                    {
                        SClevels[i] = null;
                    }
                }
                SCregions = imp.Milestone__Region.Split(',');
                for (int i = 0; i < SCregions.Count(); i++)
                {
                    if (SCregions[i] == "")
                    {
                        SCregions[i] = null;
                    }
                }
                SCAssign_Report = imp.Milestone__Assignee__Reports_to__Full_Name.Split(',');
                for (int i = 0; i < SCAssign_Report.Count(); i++)
                {
                    if (SCAssign_Report[i] == "")
                    {
                        SCAssign_Report[i] = null;
                    }
                }
                SCGroupName = imp.Group_Name.Split(',');
                for (int i = 0; i < SCGroupName.Count(); i++)
                {
                    if (SCGroupName[i] == "")
                    {
                        SCGroupName[i] = null;
                    }
                }
                var ProjectStatusCountData = (from a in entity.IMPS
                                              where a.Task_Start_Date >= imp.Task_Start_Date && a.Task_Start_Date <= imp.Milestone__Project_Start_Date
                                              where SCGroupName.Any(val1 => a.Group_Name.Equals(val1))
                                              where SCstatus.Any(val1 => a.Milestone__Project_Status.Equals(val1))
                                              where SClevels.Any(val2 => a.Workspace__Project_Level.Equals(val2))
                                              where SCregions.Any(val3 => a.Milestone__Region.Equals(val3))
                                              where SCAssign_Report.Any(val5 => a.Milestone__Assignee__Reports_to__Full_Name.Equals(val5))
                                              select a);
                var ProjectStatusCount = (from a in ProjectStatusCountData
                                          select new
                                          {
                                              N_Active_NoDate = ProjectStatusCountData.Where(val1 => val1.Milestone__Project_Status == "N-Active/No Date Confirmed").Select(x1 => x1.Workspace_Title).Count(),
                                              A_Active_Date = ProjectStatusCountData.Where(val2 => val2.Milestone__Project_Status == "A-Active/Date Confirmed").Select(x2 => x2.Workspace_Title).Count(),
                                              C_Closed = ProjectStatusCountData.Where(val2 => val2.Milestone__Project_Status == "C-Closed").Select(x2 => x2.Workspace_Title).Count(),
                                              X_Cancelled = ProjectStatusCountData.Where(val2 => val2.Milestone__Project_Status == "X-Cancelled").Select(x2 => x2.Workspace_Title).Count(),
                                              P_Pipeline = ProjectStatusCountData.Where(val2 => val2.Milestone__Project_Status == "P-Pipeline").Select(x2 => x2.Workspace_Title).Count(),
                                              H_OnHold = ProjectStatusCountData.Where(val2 => val2.Milestone__Project_Status == "H-Hold").Select(x2 => x2.Workspace_Title).Count(),
                                              Blanks = ProjectStatusCountData.Where(val2 => val2.Milestone__Project_Status == null).Select(x2 => x2.Workspace_Title).Count(),
                                          }).Distinct();
                //var ProjectStatusCount = (from a in ProjectStatusCountData
                //                          group a by a.Milestone__Project_Status into g
                //                          select new
                //                          {
                //                              Milestone__Project_Status = g.Key,
                //                              ProjectsCount = g.Count(),
                //                          }).Distinct();
                StatusCount = ProjectStatusCount.AsQueryable().Count();
                if (StatusCount.ToString() == "" || StatusCount.ToString() == null || StatusCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data Found";
                    re.Data = ProjectStatusCount;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = ProjectStatusCount;
                }
            }
            return re;
        }
        string[] LCstatus, LClevels, LCregions, LCAssign_Report, LCGroupName;
        [HttpPost]
        [Route("ProjectLeadersCounts")]
        public Response ProjectLeadersCounts(IMP imp)
        {
            if (imp.Milestone__Project_Status == null || imp.Workspace__Project_Level == null || imp.Milestone__Region == null || imp.Milestone__Assignee__Reports_to__Full_Name == null || imp.Task_Start_Date == null || imp.Group_Name == null || imp.Milestone__Project_Start_Date == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                LCstatus = imp.Milestone__Project_Status.Split(',');
                for (int i = 0; i < LCstatus.Count(); i++)
                {
                    if (LCstatus[i] == "")
                    {
                        LCstatus[i] = null;
                    }
                }
                LClevels = imp.Workspace__Project_Level.Split(',');
                for (int i = 0; i < LClevels.Count(); i++)
                {
                    if (LClevels[i] == "")
                    {
                        LClevels[i] = null;
                    }
                }
                LCregions = imp.Milestone__Region.Split(',');
                for (int i = 0; i < LCregions.Count(); i++)
                {
                    if (LCregions[i] == "")
                    {
                        LCregions[i] = null;
                    }
                }
                LCAssign_Report = imp.Milestone__Assignee__Reports_to__Full_Name.Split(',');
                for (int i = 0; i < LCAssign_Report.Count(); i++)
                {
                    if (LCAssign_Report[i] == "")
                    {
                        LCAssign_Report[i] = null;
                    }
                }
                LCGroupName = imp.Group_Name.Split(',');
                for (int i = 0; i < LCGroupName.Count(); i++)
                {
                    if (LCGroupName[i] == "")
                    {
                        LCGroupName[i] = null;
                    }
                }
                var LeadersData = (from a in entity.IMPS
                                   where a.Task_Start_Date >= imp.Task_Start_Date && a.Task_Start_Date <= imp.Milestone__Project_Start_Date
                                   where LCGroupName.Any(val1 => a.Group_Name.Equals(val1))
                                   where LCstatus.Any(val1 => a.Milestone__Project_Status.Equals(val1))
                               where LClevels.Any(val2 => a.Workspace__Project_Level.Equals(val2))
                               where LCregions.Any(val3 => a.Milestone__Region.Equals(val3))
                               where LCAssign_Report.Any(val5 => a.Milestone__Assignee__Reports_to__Full_Name.Equals(val5))
                               select a);
                var Leaders = (from a in LeadersData
                               group a by a.Milestone__Assignee__Reports_to__Full_Name into g
                               select new
                               {
                                   Milestone__Assignee__Reports_to__Full_Name = g.Key,
                                   WorkspaceCount = LeadersData.Where(x => x.Milestone__Assignee__Reports_to__Full_Name == g.Key).Select(x => x.Workspace_Title).Count(),
                               }).OrderBy(x => x.Milestone__Assignee__Reports_to__Full_Name);
                LeadersCount = Leaders.AsQueryable().Count();
                if (LeadersCount.ToString() == "" || LeadersCount.ToString() == null || LeadersCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data Found";
                    re.Data = Leaders;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = Leaders;
                }
            }
            return re;
        }

        string[] GNstatus, GNlevels, GNregions, GNAssign_Report, GNGroupName;
        [HttpPost]
        [Route("GroupNameCountIMPS")]
        public Response GroupNameCountIMPS(IMP imp)
        {
            if (imp.Milestone__Project_Status == null || imp.Workspace__Project_Level == null || imp.Milestone__Region == null || imp.Milestone__Assignee__Reports_to__Full_Name == null || imp.Task_Start_Date == null || imp.Group_Name == null || imp.Milestone__Project_Start_Date == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                GNstatus = imp.Milestone__Project_Status.Split(',');
                for (int i = 0; i < GNstatus.Count(); i++)
                {
                    if (GNstatus[i] == "")
                    {
                        GNstatus[i] = null;
                    }
                }
                GNlevels = imp.Workspace__Project_Level.Split(',');
                for (int i = 0; i < GNlevels.Count(); i++)
                {
                    if (GNlevels[i] == "")
                    {
                        GNlevels[i] = null;
                    }
                }
                GNregions = imp.Milestone__Region.Split(',');
                for (int i = 0; i < GNregions.Count(); i++)
                {
                    if (GNregions[i] == "")
                    {
                        GNregions[i] = null;
                    }
                }
                GNAssign_Report = imp.Milestone__Assignee__Reports_to__Full_Name.Split(',');
                for (int i = 0; i < GNAssign_Report.Count(); i++)
                {
                    if (GNAssign_Report[i] == "")
                    {
                        GNAssign_Report[i] = null;
                    }
                }
                GNGroupName = imp.Group_Name.Split(',');
                for (int i = 0; i < GNGroupName.Count(); i++)
                {
                    if (GNGroupName[i] == "")
                    {
                        GNGroupName[i] = null;
                    }
                }
                var GroupNamesData = (from a in entity.IMPS
                                   where a.Task_Start_Date >= imp.Task_Start_Date && a.Task_Start_Date <= imp.Milestone__Project_Start_Date
                                   where GNGroupName.Any(val1 => a.Group_Name.Equals(val1))
                                   where GNstatus.Any(val1 => a.Milestone__Project_Status.Equals(val1))
                                   where GNlevels.Any(val2 => a.Workspace__Project_Level.Equals(val2))
                                   where GNregions.Any(val3 => a.Milestone__Region.Equals(val3))
                                   where GNAssign_Report.Any(val5 => a.Milestone__Assignee__Reports_to__Full_Name.Equals(val5))
                                   select a);
                var GroupNames = (from a in GroupNamesData
                                  group a by a.Group_Name into g
                               select new
                               {
                                   Group_Name = g.Key,
                                   WorkspaceCount = GroupNamesData.Where(x => x.Group_Name == g.Key).Select(x => x.Workspace_Title).Count(),
                               }).OrderBy(x => x.Group_Name);
                GroupNameCount = GroupNames.AsQueryable().Count();
                if (GroupNameCount.ToString() == "" || GroupNameCount.ToString() == null || GroupNameCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data Found";
                    re.Data = GroupNames;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = GroupNames;
                }
            }
            return re;
        }


        string[] R_status, R_levels, R_regions, R_Assign_Report, R_GroupName;
        [HttpPost]
        [Route("RegionWiseCountIMPS")]
        public Response RegionWiseCountIMPS(IMP imp)
        {
            if (imp.Milestone__Project_Status == null || imp.Workspace__Project_Level == null || imp.Milestone__Region == null || imp.Milestone__Assignee__Reports_to__Full_Name == null || imp.Task_Start_Date == null || imp.Group_Name == null || imp.Milestone__Project_Start_Date == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                R_status = imp.Milestone__Project_Status.Split(',');
                for (int i = 0; i < R_status.Count(); i++)
                {
                    if (R_status[i] == "")
                    {
                        R_status[i] = null;
                    }
                }
                R_levels = imp.Workspace__Project_Level.Split(',');
                for (int i = 0; i < R_levels.Count(); i++)
                {
                    if (R_levels[i] == "")
                    {
                        R_levels[i] = null;
                    }
                }
                R_regions = imp.Milestone__Region.Split(',');
                for (int i = 0; i < R_regions.Count(); i++)
                {
                    if (R_regions[i] == "")
                    {
                        R_regions[i] = null;
                    }
                }
                R_Assign_Report = imp.Milestone__Assignee__Reports_to__Full_Name.Split(',');
                for (int i = 0; i < R_Assign_Report.Count(); i++)
                {
                    if (R_Assign_Report[i] == "")
                    {
                        R_Assign_Report[i] = null;
                    }
                }
                R_GroupName = imp.Group_Name.Split(',');
                for (int i = 0; i < R_GroupName.Count(); i++)
                {
                    if (R_GroupName[i] == "")
                    {
                        R_GroupName[i] = null;
                    }
                }
                var RegionsData = (from a in entity.IMPS
                                      where a.Task_Start_Date >= imp.Task_Start_Date && a.Task_Start_Date <= imp.Milestone__Project_Start_Date
                                      where R_GroupName.Any(val1 => a.Group_Name.Equals(val1))
                                      where R_status.Any(val1 => a.Milestone__Project_Status.Equals(val1))
                                      where R_levels.Any(val2 => a.Workspace__Project_Level.Equals(val2))
                                      where R_regions.Any(val3 => a.Milestone__Region.Equals(val3))
                                      where R_Assign_Report.Any(val5 => a.Milestone__Assignee__Reports_to__Full_Name.Equals(val5))
                                      select a);
                var Regions = (from a in RegionsData
                                  group a by a.Milestone__Region into g
                                  select new
                                  {
                                      Region = g.Key,
                                      WorkspaceCount = RegionsData.Where(x => x.Milestone__Region == g.Key).Select(x => x.Workspace_Title).Count(),
                                  }).OrderBy(x => x.Region);
                RegionsCount = Regions.AsQueryable().Count();
                if (RegionsCount.ToString() == "" || RegionsCount.ToString() == null || RegionsCount == 0)
                {
                    re.code = 100;
                    re.message = "No Data Found";
                    re.Data = Regions;
                }
                else
                {
                    re.code = 200;
                    re.message = "Success";
                    re.Data = Regions;
                }
            }
            return re;
        }
        [HttpPost]
        [Route("ImeetPSFilters")]
        public IMPSFilters ImeetPSFilters(IMP imp)
        {
            var FilterProjectStatus = (from a in entity.IMPS
                                          where a.Milestone__Project_Status != "H-On Hold"
                                          select new
                                          {
                                              Milestone__Project_Status = a.Milestone__Project_Status ?? "",
                                              isSelected = true,
                                          }).Distinct().OrderBy(x => x.Milestone__Project_Status);
            var FilterProjectLevel = (from a in entity.IMPS
                                       select new
                                       {
                                           Workspace__Project_Level  = a.Workspace__Project_Level ?? "",
                                           isSelected = true,
                                       }).Distinct().OrderBy(x => x.Workspace__Project_Level);
            var FilterRegion = (from a in entity.IMPS
                                      select new
                                      {
                                          Milestone__Region = a.Milestone__Region ?? "",
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.Milestone__Region);
            var FilterAssignee = (from a in entity.IMPS
                                select new
                                {
                                    Milestone__Assignee__Full_Name = a.Milestone__Assignee__Full_Name ?? "",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Milestone__Assignee__Full_Name);
            var FilterAssignee_ReportTo = (from a in entity.IMPS
                                  select new
                                  {
                                      Milestone__Assignee__Reports_to__Full_Name = a.Milestone__Assignee__Reports_to__Full_Name ?? "",
                                      isSelected = true,
                                  }).Distinct().OrderBy(x => x.Milestone__Assignee__Reports_to__Full_Name);
            var FilterGroupName = (from a in entity.IMPS
                                           select new
                                           {
                                               Group_Name = a.Group_Name ?? "",
                                               isSelected = true,
                                           }).Distinct().OrderBy(x => x.Group_Name);
            fi.message = "Success";
            fi.code = 200;
            fi.ProjectStatus = FilterProjectStatus;
            fi.ProjectLevel = FilterProjectLevel;
            fi.Region = FilterRegion;
            fi.Assignee = FilterAssignee;
            fi.Assignee_ReportTO = FilterAssignee_ReportTo;
            fi.GroupName = FilterGroupName;
            return fi;
        }
        string[] status, levels, regions, Assign_Report, GroupName;

        [HttpPost]
        [Route("ImplementationProjectStatusData")]
        public Response ImplementationProjectStatusData(IMP imp)
        {
            if(imp.Milestone__Project_Status == null || imp.Workspace__Project_Level == null || imp.Milestone__Region == null || imp.Milestone__Assignee__Reports_to__Full_Name == null || imp.Task_Start_Date == null || imp.Milestone__Project_Start_Date == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                status = imp.Milestone__Project_Status.Split(',');
                for (int i = 0; i < status.Count(); i++)
                {
                    if (status[i] == "")
                    {
                        status[i] = null;
                    }
                }
                levels = imp.Workspace__Project_Level.Split(',');
                for (int i = 0; i < levels.Count(); i++)
                {
                    if (levels[i] == "")
                    {
                        levels[i] = null;
                    }
                }
                regions = imp.Milestone__Region.Split(',');
                for (int i = 0; i < regions.Count(); i++)
                {
                    if (regions[i] == "")
                    {
                        regions[i] = null;
                    }
                }
                Assign_Report = imp.Milestone__Assignee__Reports_to__Full_Name.Split(',');
                for (int i = 0; i < Assign_Report.Count(); i++)
                {
                    if (Assign_Report[i] == "")
                    {
                        Assign_Report[i] = null;
                    }
                }
                GroupName = imp.Group_Name.Split(',');
                for (int i = 0; i < GroupName.Count(); i++)
                {
                    if (GroupName[i] == "")
                    {
                        GroupName[i] = null;
                    }
                }
                var ProjectStausFiltering = (from a in entity.IMPS
                                             //where a.Task_Start_Date > imp.Task_Start_Date
                                             where GroupName.Any(val1 => a.Group_Name.Equals(val1))
                                             where a.Task_Start_Date >= imp.Task_Start_Date && a.Task_Start_Date <= imp.Milestone__Project_Start_Date
                                             where status.Any(val1 => a.Milestone__Project_Status.Equals(val1))
                                             where levels.Any(val2 => a.Workspace__Project_Level.Equals(val2))
                                             where regions.Any(val3 => a.Milestone__Region.Equals(val3))
                                             //where Assign.Any(val4 => a.Milestone__Assignee__Full_Name.Equals(val4))
                                             where Assign_Report.Any(val5 => a.Milestone__Assignee__Reports_to__Full_Name.Equals(val5))
                                             select a).ToList();
                var ImplementationProjectStatusData = (from a in ProjectStausFiltering
                                                       select new
                                                       {
                                                           a.Workspace_Title,
                                                           a.Workspace__CRM_Customer_Row_ID,
                                                           a.Workspace__Project_Level,
                                                           a.Workspace__ELT_Overall_Status,
                                                           a.Workspace__ELT_Overall_Comments,
                                                           a.Milestone_Title,
                                                           a.Milestone__Region,
                                                           a.Milestone__Record_ID_Key,
                                                           a.Milestone__Country,
                                                           a.Milestone__Project_Status,
                                                           a.C__Complete,
                                                           a.Milestone__Project_Start_Date,
                                                           a.Milestone__Assignee__Full_Name,
                                                           a.Milestone__Assignee__Reports_to__Full_Name,
                                                           a.Task_Title,
                                                           a.Task__Task_Record_ID_Key,
                                                           a.Task_Start_Date,
                                                           a.Milestone__CRM_Revenue_ID__,
                                                           a.Milestone__Project_Notes,
                                                           a.Milestone__Reason_Code,
                                                           a.Milestone__Closed_Loop_Owner,
                                                           a.Group_Name,
                                                       });
                re.message = "Success";
                re.code = 200;
                re.Data = ImplementationProjectStatusData;
            }
            return re;
        }

        [HttpPost]
        [Route("ImplementationProjectStatusExcelData")]
        public Response ImplementationProjectStatusExcelData(IMP imp)
        {
            //var ProjectStausFiltering = (from a in entity.IMPS
            //                                where a.Task_Start_Date > imp.Task_Start_Date
            //                                where status_e.Any(val1 => a.Milestone__Project_Status.Equals(val1))
            //                                where levels_e.Any(val2 => a.Workspace__Project_Level.Equals(val2))
            //                                where regions_e.Any(val3 => a.Milestone__Region.Equals(val3))
            //                                //where Assign.Any(val4 => a.Milestone__Assignee__Full_Name.Equals(val4))
            //                                where Assign_Report_e.Any(val5 => a.Milestone__Assignee__Reports_to__Full_Name.Equals(val5))
            //                                select a).ToList();
            var ImplementationProjectStatusData = (from a in entity.IMPS
                                                    select new
                                                    {
                                                        a.Workspace_Title,
                                                        a.Workspace__CRM_Customer_Row_ID,
                                                        a.Workspace__Project_Level,
                                                        a.Workspace__ELT_Overall_Status,
                                                        a.Workspace__ELT_Overall_Comments,
                                                        a.Milestone_Title,
                                                        a.Milestone__Region,
                                                        a.Milestone__Record_ID_Key,
                                                        a.Milestone__Country,
                                                        a.Milestone__Project_Status,
                                                        a.C__Complete,
                                                        a.Milestone__Project_Start_Date,
                                                        a.Milestone__Assignee__Full_Name,
                                                        a.Milestone__Assignee__Reports_to__Full_Name,
                                                        a.Task_Title,
                                                        a.Task__Task_Record_ID_Key,
                                                        a.Task_Start_Date,
                                                        a.Milestone__CRM_Revenue_ID__,
                                                        a.Milestone__Project_Notes,
                                                        a.Milestone__Reason_Code,
                                                        a.Milestone__Closed_Loop_Owner,
                                                    });
            re.message = "Success";
            re.code = 200;
            re.Data = ImplementationProjectStatusData;
            return re;
        }

        //string[] yearmoth;
        //[HttpPost]
        //[Route("Testingarraydata")]
        //public Response Testingarraydata(EltResponce responce)
        //{
        //    yearmoth = responce.YearMonths;
        //    re.Data = yearmoth;
        //    return re;
        //}
    }
}