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
    public class ResourceAssignmentController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        H_Filters h_f = new H_Filters();
        Response re = new Response();
        AutomatedCLRFilters CLR_F = new AutomatedCLRFilters();
        [HttpPost]
        [Route("ResourceAssignmentFilters")]
        public AutomatedCLRFilters ResourceAssignmentFilters(CLRData cLRData)
        {
            var FilterPipelineStatus = (from a in entity.ManualDatas
                               select new
                               {
                                   Pipeline_status = a.Pipeline_status == null || a.Pipeline_status == "" ? "---" : a.Pipeline_status ?? "---",
                                   isSelected = true,
                               }).Distinct().OrderBy(x => x.Pipeline_status);
            var Projectstatus = "HP-High Potential,EP-Early Potential,P-Pipeline".Split(',');
            var FilterProjectStatus = (from a in entity.CLRDatas
                                       where Projectstatus.Any(val => a.ProjectStatus.Equals(val))
                                       select new
                                        {
                                            ProjectStatus = a.ProjectStatus == null || a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                            isSelected = true,
                                        }).Distinct().OrderBy(x => x.ProjectStatus);
            var FilterStatus = (from a in entity.CLRDatas
                                       select new
                                       {
                                           Status = a.Status == null || a.Status == "" ? "---" : a.Status ?? "---",
                                           isSelected = true,
                                       }).Distinct().OrderBy(x => x.Status);
            var FilterProjectLevel = (from a in entity.CLRDatas
                                      select new
                                      {
                                          ProjectLevel = a.ProjectLevel == null || a.ProjectLevel == "" ? "---" : a.ProjectLevel ?? "---",
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.ProjectLevel);
            CLR_F.FilterStatus = FilterStatus;
            CLR_F.FilterProjectStatus = FilterProjectStatus;
            CLR_F.FilterProjectLevel = FilterProjectLevel;
            CLR_F.FilterPipeline_status = FilterPipelineStatus;

            CLR_F.code = 200;
            CLR_F.message = "Success";
            return CLR_F;
        }

        string[] Status, ProjectStatus, ProjectLevel, PipelineStatus;
        [Route("ResourceAssignmentData")]
        public Response ResourceAssignmentData(CLRData cLRData)
        {
            //Status = cLRData.Status.Split(',');
            //for (int i = 0; i < Status.Count(); i++)
            //{
            //    if (Status[i] == "" || Status[i] == "---" || Status[i] == "null")
            //    {
            //        Status[i] = "";
            //    }
            //}
            ProjectStatus = cLRData.ProjectStatus.Split(',');
            for (int i = 0; i < ProjectStatus.Count(); i++)
            {
                if (ProjectStatus[i] == "" || ProjectStatus[i] == "---" || ProjectStatus[i] == "null")
                {
                    ProjectStatus[i] = "";
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
            //PipelineStatus = cLRData.CountryStatus.Split(',');
            //for (int i = 0; i < ProjectLevel.Count(); i++)
            //{
            //    if (PipelineStatus[i] == "" || PipelineStatus[i] == "---" || PipelineStatus[i] == "null")
            //    {
            //        PipelineStatus[i] = "";
            //    }
            //}
            var data = (from a in entity.CLRDatas
                        //where Status.Any(val => a.Status.Equals(val))
                        where a.Status == "Active"
                        where ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                        where ProjectLevel.Any(val => a.ProjectLevel.Equals(val))
                        //join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID
                        //where PipelineStatus.Any(val => b.Pipeline_status.Equals(val))
                        select new
                        {
                            a.Client,
                            a.RevenueID,
                            a.Line_Win_Probability,
                            Opportunity_Owner = a.Opportunity_Owner == "" ? "---" : a.Opportunity_Owner ?? "---",
                            a.Region,
                            a.Country,
                            ImplementationType = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                            OBTResellerDirect = a.OBTReseller == "" ? "---" : a.OBTReseller ?? "---",
                            Service_Configuration = a.Service_Configuration == "" ? "---" : a.Service_Configuration ?? "---",
                            GlobalProjectManager = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).GlobalProjectManager == "" ? "---" : entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).GlobalProjectManager ?? "---",
                            RegionalProjectManager = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).RegionalProjectManager == "" ? "---" : entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).RegionalProjectManager ?? "---",
                            LocalProjectManager = entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).AssigneeFullName == "" ? "---" : entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).AssigneeFullName ?? "---",
                            GlobalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISOBTLead == ""  ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISOBTLead ?? "---",
                            RegionalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISOBTLead ?? "---",
                            Pipeline_comments = a.RevenueID == 400000000000000 ? entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).Pipeline_comments : entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == a.RevenueID).Pipeline_comments ?? "---",
                            DataDescription = a.DataDescription == "" ? "---" : a.DataDescription ?? "---",
                            Next_Step = a.Next_Step == "" ? "---" : a.Next_Step ?? "---",
                            RevenueVolumeUSD = a.RevenueVolumeUSD ?? 0
                        }).ToList();
            re.Data = data;
            re.code = 200;
            re.message = "Success";
            return re;
        }
    }
}
