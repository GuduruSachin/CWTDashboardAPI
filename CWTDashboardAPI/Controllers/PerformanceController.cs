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
    public class PerformanceController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        H_Filters h_f = new H_Filters();
        Response re = new Response();
        AutomatedCLRFilters CLR_F = new AutomatedCLRFilters();
        HierarchyGraphs hg = new HierarchyGraphs();
        // GET: Performance
        string[] PD_years, PD_quarters, PD_status, PD_levels, PD_regions;

        [HttpPost]
        [Route("PerformanceData")]
        public Response PerformanceData(CLRData clr)
        {
            if (clr.ProjectStatus == null || clr.Region == null || clr.ProjectLevel == null || clr.Quarter == null || clr.GoLiveYear == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                PD_status = clr.ProjectStatus.Split(',');
                for (int i = 0; i < PD_status.Count(); i++)
                {
                    if (PD_status[i] == "" || PD_status[i] == "null")
                    {
                        PD_status[i] = null;
                    }
                }
                PD_levels = clr.ProjectLevel.Split(',');
                for (int i = 0; i < PD_levels.Count(); i++)
                {
                    if (PD_levels[i] == "" || PD_levels[i] == "null")
                    {
                        PD_levels[i] = null;
                    }
                }
                PD_years = clr.GoLiveYear.Split(',');
                for (int i = 0; i < PD_years.Count(); i++)
                {
                    if (PD_years[i] == "" || PD_years[i] == "null")
                    {
                        PD_years[i] = null;
                    }
                }
                PD_quarters = clr.Quarter.Split(',');
                for (int i = 0; i < PD_quarters.Count(); i++)
                {
                    if (PD_quarters[i] == "" || PD_quarters[i] == "null")
                    {
                        PD_quarters[i] = null;
                    }
                }
                PD_regions = clr.Region.Split(',');
                for (int i = 0; i < PD_regions.Count(); i++)
                {
                    if (PD_regions[i] == "" || PD_regions[i] == "null")
                    {
                        PD_regions[i] = null;
                    }
                }
                var clrData = (from a in entity.CLRDatas
                               where PD_years.Any(val => a.GoLiveYear.Equals(val))
                               where PD_quarters.Any(val => a.Quarter.Equals(val))
                               where PD_levels.Any(val => a.ProjectLevel.Equals(val))
                               where PD_status.Any(val => a.ProjectStatus.Equals(val))
                               where PD_regions.Any(val => a.Region.Equals(val))
                               select a).AsEnumerable();
                var PerformanceData = (from a in entity.Hierarchies
                                       group a by a.Sr_Leader into g
                                       select new
                                       {
                                           SeniorLeader = g.Key,
                                           Volume = (from sl_v in entity.Hierarchies
                                                     where sl_v.Sr_Leader == g.Key
                                                     join b in clrData on sl_v.Name equals b.AssigneeFullName
                                                     select b.RevenueVolumeUSD).Sum(),
                                           Projects = (from sl_p in entity.Hierarchies
                                                       where sl_p.Sr_Leader == g.Key
                                                       join c in clrData on sl_p.Name equals c.AssigneeFullName
                                                       select c.AssigneeFullName).Count(),
                                           CycleTime = 0,
                                           Innovation = 0,
                                           NPS = 10,
                                           SeniorIcon = false,
                                           Leaders = (from sl_l in entity.Hierarchies
                                                      where sl_l.Sr_Leader == g.Key
                                                      group sl_l by sl_l.LeaderOne into gl
                                                      select new
                                                      {
                                                          Leader = gl.Key,
                                                          LeaderCycleTime = 0,
                                                          LeaderInnovation = 0,
                                                          LeaderNPS = 10,
                                                          Leadericon = false,
                                                          LeaderVolume = (from l_v in entity.Hierarchies
                                                                          where l_v.Sr_Leader == g.Key
                                                                          where l_v.LeaderOne == gl.Key
                                                                          join d in clrData on l_v.Name equals d.AssigneeFullName
                                                                          select d.RevenueVolumeUSD).Sum(),
                                                          LeaderProjects = (from l_p in entity.Hierarchies
                                                                            where l_p.Sr_Leader == g.Key
                                                                            where l_p.LeaderOne == gl.Key
                                                                            join e in clrData on l_p.Name equals e.AssigneeFullName
                                                                            select e.AssigneeFullName).Count(),
                                                          Assignees = (from l_a in entity.Hierarchies
                                                                       where l_a.Sr_Leader == g.Key
                                                                       where l_a.LeaderOne == gl.Key
                                                                       group l_a by l_a.Name into gn
                                                                       select new
                                                                       {
                                                                           AssigneCycleTime = 0,
                                                                           AssigneInnovation = 0,
                                                                           AssigneNPS = 10,
                                                                           Assigne = gn.Key,
                                                                           AssigneVolume = (from a_v in entity.Hierarchies
                                                                                            where a_v.Sr_Leader == g.Key
                                                                                            where a_v.LeaderOne == gl.Key
                                                                                            where a_v.Name == gn.Key
                                                                                            join f in clrData on a_v.Name equals f.AssigneeFullName
                                                                                            select f.RevenueVolumeUSD).Sum(),
                                                                           AssigneProjects = (from a_v in entity.Hierarchies
                                                                                              where a_v.Sr_Leader == g.Key
                                                                                              where a_v.LeaderOne == gl.Key
                                                                                              where a_v.Name == gn.Key
                                                                                              join h in clrData on a_v.Name equals h.AssigneeFullName
                                                                                              select h.AssigneeFullName).Count(),
                                                                       })
                                                      })
                                       });
                re.code = 200;
                re.Data = PerformanceData;
                re.message = "Success";
            }
            return re;
        }

        [HttpPost]
        [Route("ProjectTeamPerformanceFilters")]
        public AutomatedCLRFilters ProjectTeamPerformanceFilters(CLRData cLRData)
        {
            var FilterRegion = (from a in entity.CapacityHierarchies
                                where a.Level != "Digital"
                                select new
                                {
                                    Region = a.Region == null || a.Region == "" ? "---" : a.Region ?? "---",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Region);
            var FilterQuarter = (from a in entity.CLRDatas
                                 select new
                                 {
                                     Quarter = a.Quarter == null || a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.Quarter);
            var FilterGlobalProjectManager = (from a in entity.CapacityHierarchies
                                              where a.ManagerStatus == "Active"
                                              where a.Level != "Digital"
                                              select new
                                              {
                                                  GlobalProjectManager = a.Leader == null || a.Leader == "" ? "---" : a.Leader ?? "---",
                                                  isSelected = true,
                                              }).Distinct().OrderBy(x => x.GlobalProjectManager);
            var FilterRegionalProjectManager = (from a in entity.CapacityHierarchies
                                              where a.ManagerStatus == "Active"
                                              where a.Level == "Digital"
                                              select new
                                              {
                                                  RegionalProjectManager = a.Leader == null || a.Leader == "" ? "---" : a.Leader ?? "---",
                                                  isSelected = true,
                                              }).Distinct().OrderBy(x => x.RegionalProjectManager);
            //var FilterRegionalProjectManager = (from a in entity.CapacityHierarchies
            //                                    where a.ManagerStatus == "Active"
            //                                    where a.Level != "Digital"
            //                                    select new
            //                                    {
            //                                        RegionalProjectManager = a.Leader == null || a.Leader == "" ? "---" : a.Leader ?? "---",
            //                                        isSelected = true,
            //                                    }).Distinct().OrderBy(x => x.RegionalProjectManager);
            //var FilterLocalProjectManager = (from a in entity.CapacityHierarchies
            //                                 where a.Level != "Digital"
            //                                 where a.ManagerStatus == "Active"
            //                                 select new
            //                                 {
            //                                     LocalProjectManager = a.Leader == null || a.Leader == "" ? "---" : a.Leader ?? "---",
            //                                     isSelected = true,
            //                                 }).Distinct().OrderBy(x => x.LocalProjectManager);
            var FilterYears = (from a in entity.CLRDatas
                               where a.GoLiveYear != "2019"
                               where a.GoLiveYear != "2050"
                               select new
                               {
                                   Year = a.GoLiveYear == null || a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                   isSelected = true,
                               }).Distinct().OrderBy(x => x.Year);
            var FilterProjectLevel = (from a in entity.CapacityHierarchies
                               select new
                               {
                                   ProjectLevel = a.PLevel == null || a.PLevel == "" ? "---" : a.PLevel ?? "---",
                                   isSelected = true,
                               }).Distinct().OrderBy(x => x.ProjectLevel);
            CLR_F.FilterRegion = FilterRegion;
            CLR_F.FilterQuarter = FilterQuarter;
            CLR_F.FilterYears = FilterYears;
            CLR_F.FilterProjectLevel = FilterProjectLevel;
            CLR_F.FilterGlobalProjectManager = FilterGlobalProjectManager;
            CLR_F.FilterRegionalProjectManager = FilterRegionalProjectManager;
            //CLR_F.FilterRegionalProjectManager = FilterRegionalProjectManager;
            //CLR_F.FilterLocalProjectManager = FilterLocalProjectManager;
            CLR_F.code = 200;
            CLR_F.message = "Success";
            return CLR_F;
        }
        string[] Quarters, Regions, GlobalManagers, ProjectLevel;
        [HttpPost]
        [Route("GetProjectTeamPerformance")]
        public Response GetProjectTeamPerformance(CLRData cLRData)
        {
            if (cLRData.Quarter == null || cLRData.Region == null || cLRData.GlobalProjectManager == null || cLRData.GoLiveYear == null || cLRData.ProjectLevel == null)
            {
                re.Data = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                Quarters = cLRData.Quarter.Split(',');
                for (int i = 0; i < Quarters.Count(); i++)
                {
                    if (Quarters[i] == "" || Quarters[i] == "---" || Quarters[i] == "null")
                    {
                        Quarters[i] = "";
                    }
                }
                Regions = cLRData.Region.Split(',');
                for (int i = 0; i < Regions.Count(); i++)
                {
                    if (Regions[i] == "" || Regions[i] == "---" || Regions[i] == "null")
                    {
                        Regions[i] = "";
                    }
                }
                GlobalManagers = cLRData.GlobalProjectManager.Split(',');
                for (int i = 0; i < GlobalManagers.Count(); i++)
                {
                    if (GlobalManagers[i] == "" || GlobalManagers[i] == "---" || GlobalManagers[i] == "null")
                    {
                        GlobalManagers[i] = "";
                    }
                }
                ProjectLevel = cLRData.ProjectLevel.Split(',');
                for (int i = 0; i < ProjectLevel.Count(); i++)
                {
                    if (ProjectLevel[i] == "" || ProjectLevel[i] == "---" || ProjectLevel[i] == "null")
                    {
                        ProjectLevel[i] = "";
                    }
                }
                var FilteredCLRData = (from a in entity.CLRDatas
                                       where a.Status == "Active"
                                       where a.GoLiveYear == cLRData.GoLiveYear
                                       where a.ProjectStatus == "C-Closed"
                                       where Quarters.Any(val => a.Quarter.Equals(val))
                                       select a);
                var GlobalData = (from a in entity.CapacityHierarchies
                                  where a.Level != "Digital"
                                  where a.ManagerStatus == "Active"
                                  where ProjectLevel.Any(val => a.PLevel.Equals(val))
                                  where GlobalManagers.Any(val => a.Leader.Equals(val))
                                  where Regions.Any(val => a.Region.Equals(val))
                                  group a by a.Manager into g
                                  select new
                                  {
                                      Manager = g.Key,
                                      Leader = entity.CapacityHierarchies.FirstOrDefault(x=>x.Manager == g.Key).Leader,
                                      ClientCount = FilteredCLRData.Where(x => x.GlobalProjectManager == g.Key).Select(x => x.Client).Count(),
                                      RevenueVolume = FilteredCLRData.Where(x => x.GlobalProjectManager == g.Key).Sum(x=>x.RevenueVolumeUSD) ?? 0,
                                  }).ToList();
                var ManagerWiseData = (from a in entity.CapacityHierarchies
                                       where a.Level != "Digital"
                                       where a.ManagerStatus == "Active"
                                       where GlobalManagers.Any(val => a.Leader.Equals(val))
                                       where ProjectLevel.Any(val => a.PLevel.Equals(val))
                                       where Regions.Any(val => a.Region.Equals(val))
                                       group a by a.Manager into g
                                       select new
                                       {
                                           Manager = g.Key,
                                           Leader = entity.CapacityHierarchies.FirstOrDefault(x => x.Manager == g.Key).Leader,
                                           Role = entity.CapacityHierarchies.FirstOrDefault(x => x.Manager == g.Key).Level,
                                           ProjectLevel = entity.CapacityHierarchies.FirstOrDefault(x => x.Manager == g.Key).PLevel,
                                           Region = entity.CapacityHierarchies.FirstOrDefault(x => x.Manager == g.Key).Region,
                                           //RelativeCapacity = 
                                           ProjectEffort = ((from b in FilteredCLRData
                                                                where b.GlobalProjectManager == g.Key
                                                                where b.RevenueID != 400000000000000
                                                                join b_g in entity.ManualDatas on b.RevenueID equals b_g.Revenue_ID
                                                                select new {
                                                                    b.RevenueID,
                                                                    b.GlobalProjectManager,
                                                                    b.Task__Task_Record_ID_Key,
                                                                    b_g.Project_Effort,
                                                                }).Sum(x => x.Project_Effort) ?? 0) +
                                                            ((from c in FilteredCLRData
                                                                where c.RegionalProjectManager == g.Key
                                                                where c.RevenueID != 400000000000000
                                                                join c_g in entity.ManualDatas on c.RevenueID equals c_g.Revenue_ID
                                                                select new
                                                                {
                                                                    c.RevenueID,
                                                                    c.GlobalProjectManager,
                                                                    c.Task__Task_Record_ID_Key,
                                                                    c_g.Project_Effort,
                                                                }).Sum(x => x.Project_Effort) ?? 0) +
                                                            ((from d in FilteredCLRData
                                                                where d.AssigneeFullName == g.Key
                                                                where d.RevenueID != 400000000000000
                                                                join d_g in entity.ManualDatas on d.RevenueID equals d_g.Revenue_ID
                                                                select new
                                                                {
                                                                    d.RevenueID,
                                                                    d.GlobalProjectManager,
                                                                    d.Task__Task_Record_ID_Key,
                                                                    d_g.Project_Effort,
                                                                }).Sum(x => x.Project_Effort) ?? 0) +
                                                            ((from b in FilteredCLRData
                                                                 where b.GlobalProjectManager == g.Key
                                                                 where b.RevenueID == 400000000000000
                                                                 join b_g in entity.ManualDatas on b.Task__Task_Record_ID_Key equals b_g.TaskRecordIdKey
                                                                 select new
                                                                 {
                                                                     b.RevenueID,
                                                                     b.GlobalProjectManager,
                                                                     b.Task__Task_Record_ID_Key,
                                                                     b_g.Project_Effort,
                                                                 }).Sum(x => x.Project_Effort) ?? 0) +
                                                            ((from c in FilteredCLRData
                                                                 where c.RegionalProjectManager == g.Key
                                                                 where c.RevenueID == 400000000000000
                                                                 join c_g in entity.ManualDatas on c.Task__Task_Record_ID_Key equals c_g.TaskRecordIdKey
                                                                 select new
                                                                 {
                                                                     c.RevenueID,
                                                                     c.GlobalProjectManager,
                                                                     c.Task__Task_Record_ID_Key,
                                                                     c_g.Project_Effort,
                                                                 }).Sum(x => x.Project_Effort) ?? 0) +
                                                            ((from d in FilteredCLRData
                                                                 where d.AssigneeFullName == g.Key
                                                                 where d.RevenueID == 400000000000000
                                                                 join d_g in entity.ManualDatas on d.Task__Task_Record_ID_Key equals d_g.TaskRecordIdKey
                                                                 select new
                                                                 {
                                                                     d.RevenueID,
                                                                     d.GlobalProjectManager,
                                                                     d.Task__Task_Record_ID_Key,
                                                                     d_g.Project_Effort,
                                                                 }).Sum(x => x.Project_Effort) ?? 0),
                                           RevenueVolume = FilteredCLRData.Where(x => x.GlobalProjectManager == g.Key || x.RegionalProjectManager == g.Key || x.AssigneeFullName == g.Key).Sum(x => x.RevenueVolumeUSD) ?? 0,
                                           ClientCount = (FilteredCLRData.Where(x => x.GlobalProjectManager == g.Key).Select(x => x.Client).Count()) + (FilteredCLRData.Where(x => x.RegionalProjectManager == g.Key).Select(x => x.Client).Count()) + (FilteredCLRData.Where(x => x.AssigneeFullName == g.Key).Select(x => x.Client).Count()),
                                       }).ToList();
                re.GlobalManager = ManagerWiseData.Where(x => x.ClientCount > 0);
                //re.RegionalManager = ManagerWiseData.Where(x => x.ClientCount_RM > 0);
                //re.LocalManager = ManagerWiseData.Where(x => x.ClientCount_LM > 0);
                re.code = 200;
                re.message = "Success";
            }
            return re;
        }

        [HttpPost]
        [Route("GetDigitalTeamPerformance")]
        public Response GetDigitalTeamPerformance(CLRData cLRData)
        {
            if (cLRData.Quarter == null || cLRData.Region == null || cLRData.GlobalProjectManager == null || cLRData.GoLiveYear == null || cLRData.ProjectLevel == null)
            {
                re.Data = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                Quarters = cLRData.Quarter.Split(',');
                for (int i = 0; i < Quarters.Count(); i++)
                {
                    if (Quarters[i] == "" || Quarters[i] == "---" || Quarters[i] == "null")
                    {
                        Quarters[i] = "";
                    }
                }
                Regions = cLRData.Region.Split(',');
                for (int i = 0; i < Regions.Count(); i++)
                {
                    if (Regions[i] == "" || Regions[i] == "---" || Regions[i] == "null")
                    {
                        Regions[i] = "";
                    }
                }
                GlobalManagers = cLRData.GlobalProjectManager.Split(',');
                for (int i = 0; i < GlobalManagers.Count(); i++)
                {
                    if (GlobalManagers[i] == "" || GlobalManagers[i] == "---" || GlobalManagers[i] == "null")
                    {
                        GlobalManagers[i] = "";
                    }
                }
                ProjectLevel = cLRData.ProjectLevel.Split(',');
                for (int i = 0; i < ProjectLevel.Count(); i++)
                {
                    if (ProjectLevel[i] == "" || ProjectLevel[i] == "---" || ProjectLevel[i] == "null")
                    {
                        ProjectLevel[i] = "";
                    }
                }
                var FilteredCLRData = (from a in entity.CLRDatas
                                       where a.Status == "Active"
                                       where a.GoLiveYear == cLRData.GoLiveYear
                                       where a.ProjectStatus == "C-Closed"
                                       where Quarters.Any(val => a.Quarter.Equals(val))
                                       select a);
                var ManagerWiseData = (from a in entity.CapacityHierarchies
                                       where a.Level == "Digital"
                                       where a.ManagerStatus == "Active"
                                       where GlobalManagers.Any(val => a.Leader.Equals(val))
                                       where ProjectLevel.Any(val => a.PLevel.Equals(val))
                                       where Regions.Any(val => a.Region.Equals(val))
                                       group a by a.Manager into g
                                       select new
                                       {
                                           Manager = g.Key,
                                           Leader = entity.CapacityHierarchies.FirstOrDefault(x => x.Manager == g.Key).Leader,
                                           Role = entity.CapacityHierarchies.FirstOrDefault(x => x.Manager == g.Key).Level,
                                           ProjectLevel = entity.CapacityHierarchies.FirstOrDefault(x => x.Manager == g.Key).PLevel,
                                           Region = entity.CapacityHierarchies.FirstOrDefault(x => x.Manager == g.Key).Region,
                                           //RelativeCapacity = 
                                           ProjectEffort = ((from b in FilteredCLRData
                                                             where b.GlobalCISOBTLead == g.Key || b.RegionalCISOBTLead == g.Key || b.LocalDigitalOBTLead == g.Key || b.GlobalCISPortraitLead == g.Key || b.RegionalCISPortraitLead == g.Key || b.GlobalCISHRFeedSpecialist == g.Key
                                                             where b.RevenueID != 400000000000000
                                                             join b_g in entity.DigitalTeams on b.RevenueID equals b_g.RevenueID
                                                             select new
                                                             {
                                                                 b.RevenueID,
                                                                 b.GlobalCISOBTLead,
                                                                 b.RegionalCISOBTLead,
                                                                 b.LocalDigitalOBTLead,
                                                                 b.Task__Task_Record_ID_Key,
                                                                 ComplexityScore = (double?)b_g.ComplexityScore,
                                                             }).Sum(x => x.ComplexityScore) ?? 0) +
                                                            ((from b in FilteredCLRData
                                                              where b.GlobalCISOBTLead == g.Key || b.RegionalCISOBTLead == g.Key || b.LocalDigitalOBTLead == g.Key || b.GlobalCISPortraitLead == g.Key || b.RegionalCISPortraitLead == g.Key || b.GlobalCISHRFeedSpecialist == g.Key
                                                              where b.RevenueID == 400000000000000
                                                              join b_g in entity.DigitalTeams on b.Task__Task_Record_ID_Key equals b_g.TaskRecordIdKey
                                                              select new
                                                              {
                                                                  b.RevenueID,
                                                                  b.GlobalCISOBTLead,
                                                                  b.RegionalCISOBTLead,
                                                                  b.LocalDigitalOBTLead,
                                                                  b.Task__Task_Record_ID_Key,
                                                                  ComplexityScore = (double?)b_g.ComplexityScore,
                                                              }).Sum(x => x.ComplexityScore) ?? 0),
                                           RevenueVolume = FilteredCLRData.Where(x => x.GlobalCISOBTLead == g.Key || x.RegionalCISOBTLead == g.Key || x.LocalDigitalOBTLead == g.Key || x.GlobalCISPortraitLead == g.Key || x.RegionalCISPortraitLead == g.Key || x.GlobalCISHRFeedSpecialist == g.Key).Sum(x => x.RevenueVolumeUSD) ?? 0,
                                           ClientCount = (FilteredCLRData.Where(x => x.GlobalCISOBTLead == g.Key || x.RegionalCISOBTLead == g.Key || x.LocalDigitalOBTLead == g.Key || x.GlobalCISPortraitLead == g.Key || x.RegionalCISPortraitLead == g.Key || x.GlobalCISHRFeedSpecialist == g.Key).Select(x => x.Client).Count()),
                                       }).ToList();
                re.GlobalManager = ManagerWiseData.Where(x => x.ClientCount > 0);
                //re.RegionalManager = ManagerWiseData.Where(x => x.ClientCount_RM > 0);
                //re.LocalManager = ManagerWiseData.Where(x => x.ClientCount_LM > 0);
                re.code = 200;
                re.message = "Success";
            }
            return re;
        }

        string[] P_Quarters;
        string P_GlobalManagers;
        [HttpPost]
        [Route("P_ManagerWisePerformanceData")]
        public Response P_ManagerWisePerformanceData(CLRData cLRData)
        {
            if (cLRData.Quarter == null || cLRData.GlobalProjectManager == null || cLRData.GoLiveYear == null)
            {
                re.Data = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                P_Quarters = cLRData.Quarter.Split(',');
                for (int i = 0; i < P_Quarters.Count(); i++)
                {
                    if (P_Quarters[i] == "" || P_Quarters[i] == "---" || P_Quarters[i] == "null")
                    {
                        P_Quarters[i] = "";
                    }
                }
                P_GlobalManagers = cLRData.GlobalProjectManager;
                var FilteredCLRData = (from a in entity.CLRDatas
                                           where a.GoLiveYear == cLRData.GoLiveYear
                                           where a.ProjectStatus == "C-Closed"
                                           where a.RevenueID != 400000000000000
                                           where a.Status == "Active"
                                           where P_Quarters.Any(val => a.Quarter.Equals(val))
                                           select a);
                var FourSeriesFilteredCLRData = (from a in entity.CLRDatas
                                                   where a.GoLiveYear == cLRData.GoLiveYear
                                                   where a.ProjectStatus == "C-Closed"
                                                   where a.Status == "Active"
                                                   where a.RevenueID == 400000000000000
                                                   where P_Quarters.Any(val => a.Quarter.Equals(val))
                                                   select a);
                var G_ManagerFourseries = (from a in FourSeriesFilteredCLRData
                                           where a.GlobalProjectManager == P_GlobalManagers
                                           select new
                                           {
                                               Client = a.Client == "" || a.Client == null ? "---" : a.Client ?? "---",
                                               a.RevenueID,
                                               a.Task__Task_Record_ID_Key,
                                               a.RevenueVolumeUSD,
                                               Region = a.Region == "" || a.Region == null ? "---" : a.Region ?? "---",
                                               GlobalProjectManager = a.GlobalProjectManager == "" || a.GlobalProjectManager == null ? "---" : a.GlobalProjectManager ?? "---",
                                               RegionalProjectManager = a.RegionalProjectManager == "" || a.RegionalProjectManager == null ? "---" : a.RegionalProjectManager ?? "---",
                                               AssigneeFullName = a.AssigneeFullName == "" || a.AssigneeFullName == null ? "---" : a.AssigneeFullName ?? "---",
                                               a.GoLiveDate,
                                               ProjectStatus = a.ProjectStatus == "" || a.ProjectStatus == null ? "---" : a.ProjectStatus ?? "---",
                                               ProjectLevel = a.ProjectLevel == "" || a.ProjectLevel == null ? "---" : a.ProjectLevel ?? "---",
                                               ProjectEffort = entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).Project_Effort
                                           }).ToList();
                var G_Manager = (from a in FilteredCLRData
                                 where a.GlobalProjectManager == P_GlobalManagers
                                 select new
                                 {
                                     Client = a.Client == "" || a.Client == null ? "---" : a.Client ?? "---",
                                     a.RevenueID,
                                     a.Task__Task_Record_ID_Key,
                                     a.RevenueVolumeUSD,
                                     Region = a.Region == "" || a.Region == null ? "---" : a.Region ?? "---",
                                     GlobalProjectManager = a.GlobalProjectManager == "" || a.GlobalProjectManager == null ? "---" : a.GlobalProjectManager ?? "---",
                                     RegionalProjectManager = a.RegionalProjectManager == "" || a.RegionalProjectManager == null ? "---" : a.RegionalProjectManager ?? "---",
                                     AssigneeFullName = a.AssigneeFullName == "" || a.AssigneeFullName == null ? "---" : a.AssigneeFullName ?? "---",
                                     a.GoLiveDate,
                                     ProjectStatus = a.ProjectStatus == "" || a.ProjectStatus == null ? "---" : a.ProjectStatus ?? "---",
                                     ProjectLevel = a.ProjectLevel == "" || a.ProjectLevel == null ? "---" : a.ProjectLevel ?? "---",
                                     ProjectEffort = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).Project_Effort
                                 }).ToList();
                var R_Manager = (from a in FilteredCLRData
                                 where a.RegionalProjectManager == P_GlobalManagers
                                 select new
                                 {
                                     Client = a.Client == "" || a.Client == null ? "---" : a.Client ?? "---",
                                     a.RevenueID,
                                     a.Task__Task_Record_ID_Key,
                                     a.RevenueVolumeUSD,
                                     Region = a.Region == "" || a.Region == null ? "---" : a.Region ?? "---",
                                     GlobalProjectManager = a.GlobalProjectManager == "" || a.GlobalProjectManager == null ? "---" : a.GlobalProjectManager ?? "---",
                                     RegionalProjectManager = a.RegionalProjectManager == "" || a.RegionalProjectManager == null ? "---" : a.RegionalProjectManager ?? "---",
                                     AssigneeFullName = a.AssigneeFullName == "" || a.AssigneeFullName == null ? "---" : a.AssigneeFullName ?? "---",
                                     a.GoLiveDate,
                                     ProjectStatus = a.ProjectStatus == "" || a.ProjectStatus == null ? "---" : a.ProjectStatus ?? "---",
                                     ProjectLevel = a.ProjectLevel == "" || a.ProjectLevel == null ? "---" : a.ProjectLevel ?? "---",
                                     ProjectEffort = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).Project_Effort
                                 }).ToList();
                var R_ManagerFourSeries = (from a in FourSeriesFilteredCLRData
                                           where a.RegionalProjectManager == P_GlobalManagers
                                             select new
                                             {
                                                 Client = a.Client == "" || a.Client == null ? "---" : a.Client ?? "---",
                                                 a.RevenueID,
                                                 a.Task__Task_Record_ID_Key,
                                                 a.RevenueVolumeUSD,
                                                 Region = a.Region == "" || a.Region == null ? "---" : a.Region ?? "---",
                                                 GlobalProjectManager = a.GlobalProjectManager == "" || a.GlobalProjectManager == null ? "---" : a.GlobalProjectManager ?? "---",
                                                 RegionalProjectManager = a.RegionalProjectManager == "" || a.RegionalProjectManager == null ? "---" : a.RegionalProjectManager ?? "---",
                                                 AssigneeFullName = a.AssigneeFullName == "" || a.AssigneeFullName == null ? "---" : a.AssigneeFullName ?? "---",
                                                 a.GoLiveDate,
                                                 ProjectStatus = a.ProjectStatus == "" || a.ProjectStatus == null ? "---" : a.ProjectStatus ?? "---",
                                                 ProjectLevel = a.ProjectLevel == "" || a.ProjectLevel == null ? "---" : a.ProjectLevel ?? "---",
                                                 ProjectEffort = entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).Project_Effort
                                             }).ToList();
                var L_Manager = (from a in FilteredCLRData
                                 where a.AssigneeFullName == P_GlobalManagers
                                 select new
                                 {
                                     Client = a.Client == "" || a.Client == null ? "---" : a.Client ?? "---",
                                     a.RevenueID,
                                     a.Task__Task_Record_ID_Key,
                                     a.RevenueVolumeUSD,
                                     Region = a.Region == "" || a.Region == null ? "---" : a.Region ?? "---",
                                     GlobalProjectManager = a.GlobalProjectManager == "" || a.GlobalProjectManager == null ? "---" : a.GlobalProjectManager ?? "---",
                                     RegionalProjectManager = a.RegionalProjectManager == "" || a.RegionalProjectManager == null ? "---" : a.RegionalProjectManager ?? "---",
                                     AssigneeFullName = a.AssigneeFullName == "" || a.AssigneeFullName == null ? "---" : a.AssigneeFullName ?? "---",
                                     a.GoLiveDate,
                                     ProjectStatus = a.ProjectStatus == "" || a.ProjectStatus == null ? "---" : a.ProjectStatus ?? "---",
                                     ProjectLevel = a.ProjectLevel == "" || a.ProjectLevel == null ? "---" : a.ProjectLevel ?? "---",
                                     ProjectEffort = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).Project_Effort
                                 }).ToList();
                var L_ManagerFourSeries = (from a in FourSeriesFilteredCLRData
                                 where a.AssigneeFullName == P_GlobalManagers
                                 select new
                                 {
                                     Client = a.Client == "" || a.Client == null ? "---" : a.Client ?? "---",
                                     a.RevenueID,
                                     a.Task__Task_Record_ID_Key,
                                     a.RevenueVolumeUSD,
                                     Region = a.Region == "" || a.Region == null ? "---" : a.Region ?? "---",
                                     GlobalProjectManager = a.GlobalProjectManager == "" || a.GlobalProjectManager == null ? "---" : a.GlobalProjectManager ?? "---",
                                     RegionalProjectManager = a.RegionalProjectManager == "" || a.RegionalProjectManager == null ? "---" : a.RegionalProjectManager ?? "---",
                                     AssigneeFullName = a.AssigneeFullName == "" || a.AssigneeFullName == null ? "---" : a.AssigneeFullName ?? "---",
                                     a.GoLiveDate,
                                     ProjectStatus = a.ProjectStatus == "" || a.ProjectStatus == null ? "---" : a.ProjectStatus ?? "---",
                                     ProjectLevel = a.ProjectLevel == "" || a.ProjectLevel == null ? "---" : a.ProjectLevel ?? "---",
                                     ProjectEffort = entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).Project_Effort
                                 }).ToList();
                var Managers = G_Manager.Concat(R_Manager).Concat(L_Manager).Concat(G_ManagerFourseries).Concat(R_ManagerFourSeries).Concat(L_ManagerFourSeries).ToList().Distinct();
                re.Data = Managers;
                re.code = 200;
                re.message = "Success";
            }
            return re;
        }

        [HttpPost]
        [Route("D_ManagerWisePerformanceData")]
        public Response D_ManagerWisePerformanceData(CLRData cLRData)
        {
            if (cLRData.Quarter == null || cLRData.GlobalProjectManager == null || cLRData.GoLiveYear == null)
            {
                re.Data = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                P_Quarters = cLRData.Quarter.Split(',');
                for (int i = 0; i < P_Quarters.Count(); i++)
                {
                    if (P_Quarters[i] == "" || P_Quarters[i] == "---" || P_Quarters[i] == "null")
                    {
                        P_Quarters[i] = "";
                    }
                }
                P_GlobalManagers = cLRData.GlobalProjectManager;
                var FilteredCLRData = (from a in entity.CLRDatas
                                       where a.GoLiveYear == cLRData.GoLiveYear
                                       where a.ProjectStatus == "C-Closed"
                                       where a.RevenueID != 400000000000000
                                       where a.Status == "Active"
                                       where P_Quarters.Any(val => a.Quarter.Equals(val))
                                       select a);
                var FourSeriesFilteredCLRData = (from a in entity.CLRDatas
                                                 where a.GoLiveYear == cLRData.GoLiveYear
                                                 where a.ProjectStatus == "C-Closed"
                                                 where a.Status == "Active"
                                                 where a.RevenueID == 400000000000000
                                                 where P_Quarters.Any(val => a.Quarter.Equals(val))
                                                 select a);
                var G_ManagerFourseries = (from a in FourSeriesFilteredCLRData
                                           where a.GlobalCISOBTLead == P_GlobalManagers || a.RegionalCISOBTLead == P_GlobalManagers || a.LocalDigitalOBTLead == P_GlobalManagers || a.GlobalCISPortraitLead == P_GlobalManagers || a.RegionalCISPortraitLead == P_GlobalManagers || a.GlobalCISHRFeedSpecialist == P_GlobalManagers
                                           select new
                                           {
                                               Client = a.Client == "" || a.Client == null ? "---" : a.Client ?? "---",
                                               a.RevenueID,
                                               a.Task__Task_Record_ID_Key,
                                               a.RevenueVolumeUSD,
                                               Region = a.Region == "" || a.Region == null ? "---" : a.Region ?? "---",
                                               GlobalCISOBTLead = a.GlobalCISOBTLead == "" || a.GlobalCISOBTLead == null ? "---" : a.GlobalCISOBTLead ?? "---",
                                               RegionalCISOBTLead = a.RegionalCISOBTLead == "" || a.RegionalCISOBTLead == null ? "---" : a.RegionalCISOBTLead ?? "---",
                                               LocalDigitalOBTLead = a.LocalDigitalOBTLead == "" || a.LocalDigitalOBTLead == null ? "---" : a.LocalDigitalOBTLead ?? "---",
                                               GlobalCISPortraitLead = a.GlobalCISPortraitLead == "" || a.GlobalCISPortraitLead == null ? "---" : a.GlobalCISPortraitLead ?? "---",
                                               RegionalCISPortraitLead = a.RegionalCISPortraitLead == "" || a.RegionalCISPortraitLead == null ? "---" : a.RegionalCISPortraitLead ?? "---",
                                               GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist == "" || a.GlobalCISHRFeedSpecialist == null ? "---" : a.GlobalCISHRFeedSpecialist ?? "---",
                                               a.GoLiveDate,
                                               ProjectStatus = a.ProjectStatus == "" || a.ProjectStatus == null ? "---" : a.ProjectStatus ?? "---",
                                               ProjectLevel = a.ProjectLevel == "" || a.ProjectLevel == null ? "---" : a.ProjectLevel ?? "---",
                                               ProjectEffort = entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).ComplexityScore,
                                               a.Country
                                           }).ToList();
                var G_Manager = (from a in FilteredCLRData
                                 where a.GlobalCISOBTLead == P_GlobalManagers || a.RegionalCISOBTLead == P_GlobalManagers || a.LocalDigitalOBTLead == P_GlobalManagers || a.GlobalCISPortraitLead == P_GlobalManagers || a.RegionalCISPortraitLead == P_GlobalManagers || a.GlobalCISHRFeedSpecialist == P_GlobalManagers
                                 select new
                                 {
                                     Client = a.Client == "" || a.Client == null ? "---" : a.Client ?? "---",
                                     a.RevenueID,
                                     a.Task__Task_Record_ID_Key,
                                     a.RevenueVolumeUSD,
                                     Region = a.Region == "" || a.Region == null ? "---" : a.Region ?? "---",
                                     GlobalCISOBTLead = a.GlobalCISOBTLead == "" || a.GlobalCISOBTLead == null ? "---" : a.GlobalCISOBTLead ?? "---",
                                     RegionalCISOBTLead = a.RegionalCISOBTLead == "" || a.RegionalCISOBTLead == null ? "---" : a.RegionalCISOBTLead ?? "---",
                                     LocalDigitalOBTLead = a.LocalDigitalOBTLead == "" || a.LocalDigitalOBTLead == null ? "---" : a.LocalDigitalOBTLead ?? "---",
                                     GlobalCISPortraitLead = a.GlobalCISPortraitLead == "" || a.GlobalCISPortraitLead == null ? "---" : a.GlobalCISPortraitLead ?? "---",
                                     RegionalCISPortraitLead = a.RegionalCISPortraitLead == "" || a.RegionalCISPortraitLead == null ? "---" : a.RegionalCISPortraitLead ?? "---",
                                     GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist == "" || a.GlobalCISHRFeedSpecialist == null ? "---" : a.GlobalCISHRFeedSpecialist ?? "---",
                                     a.GoLiveDate,
                                     ProjectStatus = a.ProjectStatus == "" || a.ProjectStatus == null ? "---" : a.ProjectStatus ?? "---",
                                     ProjectLevel = a.ProjectLevel == "" || a.ProjectLevel == null ? "---" : a.ProjectLevel ?? "---",
                                     ProjectEffort = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).ComplexityScore,
                                     a.Country
                                 }).ToList();
                var Managers = G_Manager.Concat(G_ManagerFourseries).Distinct();
                re.Data = Managers;
                re.code = 200;
                re.message = "Success";
            }
            return re;
        }
    }
}