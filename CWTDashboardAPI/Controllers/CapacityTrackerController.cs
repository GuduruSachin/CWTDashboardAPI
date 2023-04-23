using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Net.Http;
using System.Web.Http;
using System.Globalization;
using CWTDashboardAPI.Models;

namespace CWTDashboardAPI.Controllers
{
    public class CapacityTrackerController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        AutomatedCLRFilters ACLR_F = new AutomatedCLRFilters();
        Response re = new Response();

        int ResUtilCount, TrackerCount, YearlyAverageCount;
        string[] levels, regions, workingdays, leaders, Projectstatus;
        [HttpPost]
        [Route("ResourceUtilization")]
        public Response ResourceUtilization(CapacityHierarchy hierarchy)
        {
            if (hierarchy.Comments == null)
            {
                re.Data = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                Projectstatus = hierarchy.Comments.Split(',');
                for (int i = 0; i < Projectstatus.Count(); i++)
                {
                    if (Projectstatus[i] == "" || Projectstatus[i] == "---" || Projectstatus[i] == "null")
                    {
                        Projectstatus[i] = "";
                    }
                }
            }
            var Role = "PM 1,PM 2,PM 3".Split(',');
            var Resutil_list = (from a in entity.CapacityHierarchies
                                where a.ManagerStatus == "Active"
                                where Role.Any(val1 => a.Level.Equals(val1))
                                select new ResourceUtilization
                                {
                                    HierarchyID = a.HID,
                                    Region =  a.Region,
                                    Level =  a.Level,
                                    ProjectLevel = a.PLevel,
                                    Leader =  a.Leader,
                                    Manager = a.Manager,
                                    WorkShedule = a.WorkShedule,
                                    WorkingDays = a.WorkingDays,
                                    Monday = a.Monday,
                                    Tuesday = a.Tuesday,
                                    Wednesday = a.Wednesday,
                                    Thursday = a.Thursday,
                                    Friday = a.Friday,
                                    Comments = a.Comments == null || a.Comments == "" ? "---" : a.Comments ?? "---",
                                    TargetedUtilization =  a.TargetedUtilization,
                                    C1stweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager && v1.C1stweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager && v1.C1stweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager && v1.C1stweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0),
                                    C2ndweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager && v1.C2ndweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager && v1.C2ndweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager && v1.C2ndweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0),
                                    C3rdweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager && v1.C3rdweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager && v1.C3rdweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager && v1.C3rdweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0),
                                    C4thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager && v1.C4thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager && v1.C4thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager && v1.C4thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0),
                                    C5thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager && v1.C5thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager && v1.C5thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager && v1.C5thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0),
                                    C6thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager && v1.C6thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager && v1.C6thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager && v1.C6thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0),
                                    C7thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager && v1.C7thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager && v1.C7thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager && v1.C7thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0),
                                    C8thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager && v1.C8thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager && v1.C8thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager && v1.C8thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0),
                                    //C9thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.C9thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.C9thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.C9thweek) ?? 0),
                                    //C10thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.C10thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.C10thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.C10thweek) ?? 0),
                                    //C11thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.C11thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.C11thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.C11thweek) ?? 0),
                                    //C12thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.C12thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.C12thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.C12thweek) ?? 0),
                                    //c13thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c13thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c13thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c13thweek) ?? 0),
                                    //c14thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c14thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c14thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c14thweek) ?? 0),
                                    //c15thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c15thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c15thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c15thweek) ?? 0),
                                    //c16thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c16thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c16thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c16thweek) ?? 0),
                                    //c17thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c17thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c17thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c17thweek) ?? 0),
                                    //c18thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c18thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c18thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c18thweek) ?? 0),
                                    //c19thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c19thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c19thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c19thweek) ?? 0),
                                    //c20thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c20thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c20thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c20thweek) ?? 0),
                                    //c21thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c21thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c21thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c21thweek) ?? 0),
                                    //c22thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c22thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c22thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c22thweek) ?? 0),
                                    //c23thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c23thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c23thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c23thweek) ?? 0),
                                    //c24thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == a.Manager).Sum(x => x.c24thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == a.Manager).Sum(x => x.c24thweek) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == a.Manager).Sum(x => x.c24thweek) ?? 0),
                                }).ToList();
            foreach (var r in Resutil_list)
            {
                r.AvgUtil = (r.C1stweek + r.C2ndweek + r.C3rdweek + r.C4thweek)/4;
                r.TUWorkingDays = (r.TargetedUtilization * r.WorkingDays ?? 0)/5;
                r.CapacityAvailable = r.TargetedUtilization - r.AvgUtil;
                r.CapacityAvailableTUWorkingDays = r.TUWorkingDays - r.AvgUtil;
                r.C9thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.C9thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.C9thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.C9thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.C10thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.C10thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.C10thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.C10thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.C11thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.C11thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.C11thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.C11thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.C12thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.C12thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.C12thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.C12thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c13thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c13thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c13thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c13thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c14thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c14thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c14thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c14thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c15thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c15thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c15thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c15thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c16thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c16thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c16thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c16thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c17thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c17thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c17thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c17thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c18thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c18thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c18thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c18thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c19thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c19thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c19thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c19thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c20thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c20thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c20thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c20thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c21thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c21thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c21thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c21thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c22thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c22thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c22thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c22thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c23thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c23thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c23thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c23thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
                r.c24thweek = (entity.CLRTrackers.Where(v1 => v1.LocalProjectManager == r.Manager && v1.c24thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.RegionalProjectManager == r.Manager && v1.c24thweek == 1).Sum(x => x.ProjectEffort) ?? 0) + (entity.CLRTrackers.Where(v1 => v1.GlobalProjectManager == r.Manager && v1.c24thweek == 1 && Projectstatus.Any(val => v1.ProjectStatus.Equals(val))).Sum(x => x.ProjectEffort) ?? 0);
            }
            var ManagersCount = (from a in entity.CLRTrackers
                                 where Projectstatus.Any(val => a.ProjectStatus.Equals(val))
                                 select new {
                                     a.RevenueID,
                                     Manager = (from b in entity.CapacityHierarchies
                                                where b.ManagerStatus == "Active"
                                                where b.Level == "Digital"
                                                where b.Manager == a.LocalDigitalOBTLead || b.Manager == a.GlobalDigitalOBTLead || b.Manager == a.RegionalDigitalOBTLead || b.Manager == a.GlobalDigitalPortraitLead || b.Manager == a.RegionalDigitalPortraitLead || b.Manager == a.GlobalDigitalHRFeedSpeciallist
                                                select new {
                                                    Manager = b.Manager
                                                }).ToList(),
                                     a.ComplexityScore,
                                     a.C1stweek,
                                     a.C2ndweek,
                                     a.C3rdweek,
                                     a.C4thweek,
                                     a.C5thweek,
                                     a.C6thweek,
                                     a.C7thweek,
                                     a.C8thweek,
                                     a.C9thweek,
                                     a.C10thweek,
                                     a.C11thweek,
                                     a.C12thweek,
                                     a.c13thweek,
                                     a.c14thweek,
                                     a.c15thweek,
                                     a.c16thweek,
                                     a.c17thweek,
                                     a.c18thweek,
                                     a.c19thweek,
                                     a.c20thweek,
                                     a.c21thweek,
                                     a.c22thweek,
                                     a.c23thweek,
                                     a.c24thweek,
                                 }).ToList();
            var Resutil_list2 = (from a in entity.CapacityHierarchies
                                 where a.ManagerStatus == "Active"
                                 where a.Level == "Digital"
                                 select new ResourceUtilization
                                 {
                                     HierarchyID = a.HID,
                                     Region = a.Region,
                                     Level = a.Level,
                                     ProjectLevel = a.PLevel,
                                     Leader = a.Leader,
                                     Manager = a.Manager,
                                     WorkShedule = a.WorkShedule,
                                     WorkingDays = a.WorkingDays,
                                     Monday = a.Monday,
                                     Tuesday = a.Tuesday,
                                     Wednesday = a.Wednesday,
                                     Thursday = a.Thursday,
                                     Friday = a.Friday,
                                     Comments = a.Comments == null || a.Comments == "" ? "---" : a.Comments ?? "---",
                                     TargetedUtilization = a.TargetedUtilization,
                                 }).ToList();
            foreach (var r in Resutil_list2)
            {
                r.C1stweek = (from b in ManagersCount
                              where b.C1stweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore}).Sum(x =>x.ComplexityScore) ?? 0;
                r.C2ndweek = (from b in ManagersCount
                              where b.C2ndweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.C3rdweek = (from b in ManagersCount
                              where b.C3rdweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.C4thweek = (from b in ManagersCount
                              where b.C4thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.AvgUtil = (r.C1stweek + r.C2ndweek + r.C3rdweek + r.C4thweek) / 4;
                r.TUWorkingDays = (r.TargetedUtilization * r.WorkingDays ?? 0) / 5;
                r.CapacityAvailable = r.TargetedUtilization - r.AvgUtil;
                r.CapacityAvailableTUWorkingDays = r.TUWorkingDays - r.AvgUtil;
                r.C5thweek = (from b in ManagersCount
                              where b.C5thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.C6thweek = (from b in ManagersCount
                              where b.C6thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.C7thweek = (from b in ManagersCount
                              where b.C7thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.C8thweek = (from b in ManagersCount
                              where b.C8thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.C9thweek = (from b in ManagersCount
                              where b.C9thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.C10thweek = (from b in ManagersCount
                              where b.C10thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.C11thweek = (from b in ManagersCount
                              where b.C11thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.C12thweek = (from b in ManagersCount
                              where b.C12thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c13thweek = (from b in ManagersCount
                              where b.c13thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c14thweek = (from b in ManagersCount
                              where b.c14thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c15thweek = (from b in ManagersCount
                              where b.c15thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c16thweek = (from b in ManagersCount
                              where b.c16thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c17thweek = (from b in ManagersCount
                              where b.c17thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c18thweek = (from b in ManagersCount
                              where b.c18thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c19thweek = (from b in ManagersCount
                              where b.c19thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c20thweek = (from b in ManagersCount
                              where b.c20thweek == 1
                              from c in b.Manager
                              where c.Manager == r.Manager
                              select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c21thweek = (from b in ManagersCount
                               where b.c21thweek == 1
                               from c in b.Manager
                               where c.Manager == r.Manager
                               select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c22thweek = (from b in ManagersCount
                               where b.c22thweek == 1
                               from c in b.Manager
                               where c.Manager == r.Manager
                               select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c23thweek = (from b in ManagersCount
                               where b.c23thweek == 1
                               from c in b.Manager
                               where c.Manager == r.Manager
                               select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
                r.c24thweek = (from b in ManagersCount
                               where b.c24thweek == 1
                               from c in b.Manager
                               where c.Manager == r.Manager
                               select new { b.ComplexityScore }).Sum(x => x.ComplexityScore) ?? 0;
            }
            var Result_data = Resutil_list.Concat(Resutil_list2);
            ResUtilCount = Result_data.AsQueryable().Count();
            if (ResUtilCount.ToString() == "" || ResUtilCount.ToString() == null || ResUtilCount == 0)
            {
                re.Data = Result_data;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                re.Data = Result_data;
                re.code = 200;
                re.message = "Data Successfull";
            }
            return re;
        }

        [HttpPost]
        [Route("ResourceUtilizationFilters")]
        public AutomatedCLRFilters ResourceUtilizationFilters(CapacityHierarchy capacityHierarchy)
        {
            var FilterRegion = (from a in entity.CapacityHierarchies
                                where a.ManagerStatus == "Active"
                                select new
                                {
                                    a.Region,
                                    isSelected = true
                                }).Distinct().OrderBy(x => x.Region).ToList();
            //var FilterRegion = (from a in entity.CapacityHierarchies
            //                    where a.ManagerStatus == "Active"
            //                    select new
            //                    {
            //                        a.Region,
            //                        isSelected = true
            //                    }).Distinct().OrderBy(x => x.Region).ToList();
            //var FilterRegion = (from a in entity.CapacityHierarchies
            //                    where a.ManagerStatus == "Active"
            //                    select new
            //                    {
            //                        a.Region,
            //                        isSelected = true
            //                    }).Distinct().OrderBy(x => x.Region).ToList();
            //var FilterRegion = (from a in entity.CapacityHierarchies
            //                    where a.ManagerStatus == "Active"
            //                    select new
            //                    {
            //                        a.Region,
            //                        isSelected = true
            //                    }).Distinct().OrderBy(x => x.Region).ToList();
            return ACLR_F;
        }
        [HttpPost]
        [Route("Tracker")]
        public Response Tracker(CLRTracker tracker)
        {
            var TrackerList = (from a in entity.CLRTrackers
                               select new
                               {
                                   a.TrackerId,
                                   a.RevenueID,
                                   Region = a.Region == "" ? "---" : a.Region ?? "---",
                                   Country = a.Country == "" ? "---" : a.Country ?? "---",
                                   GManager = a.GlobalProjectManager == "" ? "---" : a.GlobalProjectManager ?? "---",
                                   RManager = a.RegionalProjectManager == "" ? "---" : a.RegionalProjectManager ?? "---",
                                   LManager = a.LocalProjectManager == "" ? "---" : a.LocalProjectManager ?? "---",
                                   Client = a.Client == "" ? "---" : a.Client ?? "---",
                                   iMeetWorkspaceTitle = a.iMeetWorkspaceTitle == "" ? "---" : a.iMeetWorkspaceTitle ?? "---",
                                   OwnershipType = a.OwnershipType == "" ? "---" : a.OwnershipType ?? "---",
                                   RevenueVolume = a.Volume ?? 0,
                                   Project_Level = a.ProjectLevel == "" ? "---" : a.ProjectLevel ?? "---",
                                   ImplementationType = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                   ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                   ProjectEffort = a.ProjectEffort ?? 0,
                                   GlobalDigitalOBTLead = a.GlobalDigitalOBTLead == "" ? "---" : a.GlobalDigitalOBTLead ?? "---",
                                   RegionalDigitalOBTLead = a.RegionalDigitalOBTLead == "" ? "---" : a.RegionalDigitalOBTLead ?? "---",
                                   LocalDigitalOBTLead = a.LocalDigitalOBTLead == "" ? "---" : a.LocalDigitalOBTLead ?? "---",
                                   GlobalDigitalPortraitLead = a.GlobalDigitalPortraitLead == "" ? "---" : a.GlobalDigitalPortraitLead ?? "---",
                                   RegionalDigitalPortraitLead = a.RegionalDigitalPortraitLead == "" ? "---" : a.RegionalDigitalPortraitLead ?? "---",
                                   GlobalDigitalHRFeedSpeciallist = a.GlobalDigitalHRFeedSpeciallist == "" ? "---" : a.GlobalDigitalHRFeedSpeciallist ?? "---",
                                   GDS = a.GDS == "" ? "---" : a.GDS ?? "---",
                                   ComplexityScore = a.ComplexityScore ?? 0,
                                   Proposed_Start_Date__iMeet_ = a.KickOff_ProposedStartDate,
                                   Proposed_End_Date__Formula_ = a.EndofHypercare,
                                   Go_Live_Date__iMeet_ = a.GoLiveDate,
                                   //Complete_Duration__Formula_ = a.CompleteDuration,
                                   a.ProjectStartDate,
                                   MilestoneProjectNotes = a.MilestoneProjectNotes == "" ? "---" : a.MilestoneProjectNotes ?? "---",
                                   a.MilestoneDueDate,
                                   PerCompleted = a.PerCompleted ?? 0,
                                   ProjectDelay = a.ProjectDelay ?? 0,
                                   FirstWeek = a.C1stweek,
                                   SecondWeek = a.C2ndweek,
                                   ThirdWeek = a.C3rdweek,
                                   FourthWeek = a.C4thweek,
                                   FivthWeek = a.C5thweek,
                                   SixthWeek = a.C6thweek,
                                   SeventhWeek = a.C7thweek,
                                   EighthWeek = a.C8thweek,
                                   NinthWeek = a.C9thweek,
                                   TenthWeek = a.C10thweek,
                                   EleventhWeek = a.C11thweek,
                                   twelvethWeek = a.C12thweek,
                                   C13thweek = a.c13thweek,
                                   C14thWeek = a.c14thweek,
                                   C15thWeek = a.c15thweek,
                                   C16thWeek = a.c16thweek,
                                   C17thWeek = a.c17thweek,
                                   C18thWeek = a.c18thweek,
                                   C19thWeek = a.c19thweek,
                                   C20thWeek = a.c20thweek,
                                   C21stWeek = a.c21thweek,
                                   C22ndWeek = a.c22thweek,
                                   C23rdWeek = a.c23thweek,
                                   C24thWeek = a.c24thweek,
                               }).OrderBy(x => x.Client);
            TrackerCount = TrackerList.AsQueryable().Count();
            if (TrackerCount.ToString() == "" || TrackerCount.ToString() == null || TrackerCount == 0)
            {
                re.Data = TrackerList;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                re.Data = TrackerList;
                re.code = 200;
                re.message = "Data Successfull";
            }
            return re;
        }
        //[HttpPost]
        //[Route("YearlyAverage")]
        //public Response YearlyAverage(CLRTracker tracker)
        //{
        //    var YearlyAverageList = (from a in entity.CapacityHierarchies
        //                             select new
        //                             {
        //                                 a.Region,
        //                                 a.Level,
        //                                 a.Leader,
        //                                 a.Manager,
        //                                 Jan2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Jan) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Jan) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Jan) ?? 0),
        //                                 Feb2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Feb) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Feb) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Feb) ?? 0),
        //                                 Mar2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Mar) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Mar) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Mar) ?? 0),
        //                                 Apr2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Apr) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Apr) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Apr) ?? 0),
        //                                 May2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.May) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.May) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.May) ?? 0),
        //                                 Jun2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Jun) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Jun) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Jun) ?? 0),
        //                                 Jul2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Jul) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Jul) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Jul) ?? 0),
        //                                 //Aug2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Aug) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Aug) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Aug) ?? 0),
        //                                 //Sep2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Sep) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Sep) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Sep) ?? 0),
        //                                 //Oct2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Oct) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Oct) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Oct) ?? 0),
        //                                 //Nov2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Nov) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Nov) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Nov) ?? 0),
        //                                 //Dec2020 = (entity.Trackers.Where(v1 => v1.Local_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Dec) ?? 0) + (entity.Trackers.Where(v1 => v1.Regional_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Dec) ?? 0) + (entity.Trackers.Where(v1 => v1.Global_Project_Manager__iMeet_ == a.Manager).Sum(x => x.Dec) ?? 0),
        //                             });
        //    YearlyAverageCount = YearlyAverageList.AsQueryable().Count();
        //    if (YearlyAverageCount.ToString() == "" || YearlyAverageCount.ToString() == null || YearlyAverageCount == 0)
        //    {
        //        re.Data = YearlyAverageList;
        //        re.code = 100;
        //        re.message = "No Data found";
        //    }
        //    else
        //    {
        //        re.Data = YearlyAverageList;
        //        re.code = 200;
        //        re.message = "Data Successfull";
        //    }
        //    return re;
        //}
    }
}