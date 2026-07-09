using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Configuration;
using System.Data.OleDb;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using CWTDashboardAPI.Controllers;

namespace CWTDashboardAPI.Models
{
    public class ManualDataClass
    {
        double? RevenueId, RevID;
        int count,ManualID;
        public Boolean AddingManualdata(ManualData manualData)
        {
            List<ManualData> list = new List<ManualData>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.ManualDatas.OrderBy(a => a.Revenue_ID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    RevenueId = list[i].Revenue_ID;
                    if (manualData.Revenue_ID.Equals(RevenueId))
                    {
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Boolean UpdateManualdata(ManualData manualData)
        {
            List<ManualData> list = new List<ManualData>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.ManualDatas.OrderBy(a => a.ManualID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    RevID = list[i].Revenue_ID;
                    ManualID = list[i].ManualID;
                    if (manualData.Revenue_ID.Equals(RevID) && manualData.ManualID.Equals(ManualID))
                    {
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        ManualData vp = (from s in entit.ManualDatas
                                        where s.ManualID == manualData.ManualID
                                        where s.Revenue_ID == manualData.Revenue_ID
                                         select s).FirstOrDefault();
                        CLRData CVP;
                        if (manualData.Revenue_ID == 400000000000000)
                        {
                            CVP = (from a in entit.CLRDatas
                                   where a.RevenueID == manualData.Revenue_ID
                                   where a.Task__Task_Record_ID_Key == vp.TaskRecordIdKey
                                   select a).FirstOrDefault();
                        }
                        else
                        {
                            CVP = (from a in entit.CLRDatas
                                   where a.RevenueID == manualData.Revenue_ID
                                   select a).FirstOrDefault();
                        }
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Priority, manualData.Priority, "Priority", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Implementation_Type, manualData.Implementation_Type, "Implementation Type", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Pipeline_status, manualData.Pipeline_status, "Pipeline Status", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Pipeline_comments, manualData.Pipeline_comments, "Pipeline Comments", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.TXResourcing, manualData.TXResourcing, "TXResourcing", manualData.UpdateBy);
                        //auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Service_configuration, manualData.Service_configuration, "Service Configuration", manualData.UpdateBy);
                        //auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.OBT_Reseller___Direct, manualData.OBT_Reseller___Direct, "OBT Reseller Direct", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.ExpectedDecisionDate + "", manualData.ExpectedDecisionDate + "", "Expected Decision Date", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Assignment_date + "", manualData.Assignment_date + "", "Assignment date", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.ResourseRequestedDate + "", manualData.ResourseRequestedDate + "", "Resource Requested date", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Status, manualData.Status, "Status", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Project_Effort + "", manualData.Project_Effort + "", "Project Effort", manualData.UpdateBy);
                        vp.UpdateBy = manualData.UpdateBy;
                        vp.UpdateOn = DateTime.Now;
                        vp.Priority = manualData.Priority;
                        vp.Implementation_Type = manualData.Implementation_Type;
                        vp.Pipeline_status = manualData.Pipeline_status;
                        vp.Pipeline_comments = manualData.Pipeline_comments;
                        vp.TXResourcing = manualData.TXResourcing;
                        //vp.Service_configuration = manualData.Service_configuration;
                        //vp.OBT_Reseller___Direct = manualData.OBT_Reseller___Direct;
                        vp.ResourseRequestedDate = manualData.ResourseRequestedDate;
                        vp.ExpectedDecisionDate = manualData.ExpectedDecisionDate;
                        vp.Assignment_date = manualData.Assignment_date;
                        vp.Project_Effort = manualData.Project_Effort;
                        vp.Status = manualData.Status;
                        CVP.ImplementationType = manualData.Implementation_Type;
                        CVP.Status = manualData.Status;
                        //CVP.OBTReseller = manualData.OBT_Reseller___Direct;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public Boolean UpdateManualReplicatingData(ReplicateManualData manualData)
        {
            List<ManualData> list = new List<ManualData>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.ManualDatas.OrderBy(a => a.Revenue_ID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    RevID = list[i].Revenue_ID;
                    if (manualData.Revenue_ID.Equals(RevID))
                    {
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        ManualData vp = (from s in entit.ManualDatas
                                         where s.Revenue_ID == manualData.Revenue_ID
                                         select s).FirstOrDefault();
                        DigitalTeam DT = (from s in entit.DigitalTeams
                                         where s.RevenueID == manualData.Revenue_ID
                                         select s).FirstOrDefault();
                        CLRData CVP = (from a in entit.CLRDatas
                                       where a.RevenueID == manualData.Revenue_ID
                                       select a).FirstOrDefault();
                        if (manualData.Implementation_Type_check == true || manualData.Pipeline_status_check == true ||
                            manualData.Pipeline_comments_check == true || manualData.GoLiveDate_check == true ||
                            //manualData.Service_configuration_check == true || manualData.OBT_Reseller___Direct_check == true || 
                            manualData.Assignment_date_check == true || manualData.Status_check == true || 
                            manualData.Project_Effort_check == true || manualData.ExpectedDecision_date_check == true)
                        {
                            vp.UpdateBy = manualData.UpdateBy;
                            vp.UpdateOn = DateTime.Now;
                        }
                        else
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                if (manualData.AssigneeFullName_check == true || manualData.GlobalProjectManager_check == true ||
                                    manualData.RegionalProjectManager_check == true || manualData.ProjectLevel_check == true)
                                {
                                    DT.UpdatedBy = manualData.UpdateBy;
                                    DT.UpdatedOn = DateTime.Now;
                                }
                                else { }
                            }
                        }
                        if (manualData.Priority_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.Priority, manualData.Priority, "Priority", manualData.UpdateBy);
                            vp.Priority = manualData.Priority;
                        }
                        if (manualData.Implementation_Type_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.Implementation_Type, manualData.Implementation_Type, "Implementation Type", manualData.UpdateBy);
                            vp.Implementation_Type = manualData.Implementation_Type;
                            CVP.ImplementationType = manualData.Implementation_Type;
                        }
                        if (manualData.Pipeline_status_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.Pipeline_status, manualData.Pipeline_status, "Pipeline Status", manualData.UpdateBy);
                            vp.Pipeline_status = manualData.Pipeline_status;
                        }
                        if (manualData.Pipeline_comments_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.Pipeline_comments, manualData.Pipeline_comments, "Pipeline Comments", manualData.UpdateBy);
                            vp.Pipeline_comments = manualData.Pipeline_comments;
                        }
                        if (manualData.TXResourcing_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.TXResourcing, manualData.TXResourcing, "TXResourcing", manualData.UpdateBy);
                            vp.TXResourcing = manualData.TXResourcing;
                        }
                        //if (manualData.Service_configuration_check == true)
                        //{
                        //    auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.Service_configuration, manualData.Service_configuration, "Service Configuration", manualData.UpdateBy);
                        //    vp.Service_configuration = manualData.Service_configuration;
                        //}
                        //if (manualData.OBT_Reseller___Direct_check == true)
                        //{
                        //    auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.OBT_Reseller___Direct, manualData.OBT_Reseller___Direct, "OBT Reseller Direct", manualData.UpdateBy);
                        //    vp.OBT_Reseller___Direct = manualData.OBT_Reseller___Direct;
                        //    CVP.OBTReseller = manualData.OBT_Reseller___Direct;
                        //}
                        if (manualData.GoLiveDate_check == true)
                        {
                            if(CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.GoLiveDate + "", manualData.GoLiveDate + "", "Go Live Date", manualData.UpdateBy);
                                vp.GoLiveDate = manualData.GoLiveDate;
                                CVP.GoLiveDate = manualData.GoLiveDate;
                                CVP.GoLiveMonth = Convert.ToDateTime(manualData.GoLiveDate).ToString("MMM");
                                CVP.GoLiveYear = Convert.ToDateTime(manualData.GoLiveDate).Year.ToString();
                                CVP.YearMonth = Convert.ToDateTime(manualData.GoLiveDate).ToString("MMM") + "-" + Convert.ToDateTime(manualData.GoLiveDate).Year.ToString();
                                CVP.Quarter = Convert.ToDateTime(manualData.GoLiveDate).Month < 4 ? "Qtr 1"
                                            : (Convert.ToDateTime(manualData.GoLiveDate).Month >= 4 && Convert.ToDateTime(manualData.GoLiveDate).Month < 7) ? "Qtr 2"
                                            : (Convert.ToDateTime(manualData.GoLiveDate).Month >= 7 && Convert.ToDateTime(manualData.GoLiveDate).Month < 10) ? "Qtr 3"
                                            : (Convert.ToDateTime(manualData.GoLiveDate).Month >= 10 && Convert.ToDateTime(manualData.GoLiveDate).Month <= 12) ? "Qtr 4"
                                            : "" ?? "";
                            }
                        }
                        if (manualData.AssigneeFullName_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.AssigneeFullName, manualData.AssigneeFullName, "Pipeline Status", manualData.UpdateBy);
                                vp.AssigneeFullName = manualData.AssigneeFullName;
                                CVP.AssigneeFullName = manualData.AssigneeFullName;
                            }
                        }
                        if (manualData.GlobalProjectManager_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.GlobalProjectManager, manualData.GlobalProjectManager, "Global Project Manager", manualData.UpdateBy);
                                vp.GlobalProjectManager = manualData.GlobalProjectManager;
                                CVP.GlobalProjectManager = manualData.GlobalProjectManager;
                            }
                        }
                        if (manualData.RegionalProjectManager_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.RegionalProjectManager, manualData.RegionalProjectManager, "Regional Project Manager", manualData.UpdateBy);
                                vp.RegionalProjectManager = manualData.RegionalProjectManager;
                                CVP.RegionalProjectManager = manualData.RegionalProjectManager;
                            }
                        }
                        if (manualData.Assignment_date_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.Assignment_date + "", manualData.Assignment_date + "", "Assignment Date", manualData.UpdateBy);
                            vp.Assignment_date = manualData.Assignment_date;
                        }
                        if (manualData.ExpectedDecision_date_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.ExpectedDecisionDate + "", manualData.ExpectedDecisionDate + "", "Expected Decision Date", manualData.UpdateBy);
                            vp.ExpectedDecisionDate = manualData.ExpectedDecisionDate;
                        }
                        if (manualData.ResourceRequest_date_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.ResourseRequestedDate + "", manualData.ResourceRequest_date + "", "Resource Requested Date", manualData.UpdateBy);
                            vp.ResourseRequestedDate = manualData.ResourceRequest_date;
                        }
                        if (manualData.Status_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.Status, manualData.Status, "Status", manualData.UpdateBy);
                            vp.Status = manualData.Status;
                            CVP.Status = manualData.Status;
                        }
                        if (manualData.ProjectLevel_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.ProjectLevel, manualData.ProjectLevel, "Project Level", manualData.UpdateBy);
                                vp.ProjectLevel = manualData.ProjectLevel;
                                CVP.ProjectLevel = manualData.ProjectLevel;
                            }
                        }
                        if (manualData.Project_Effort_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", vp.Project_Effort + "", manualData.Project_Effort + "", "Project Effort", manualData.UpdateBy);
                            vp.Project_Effort = manualData.Project_Effort;
                        }
                        if(manualData.ComplexityScore_check == true || manualData.ActivityType_check == true)//manualData.GDS_check == true || 
                        {
                            DT.UpdatedBy = manualData.UpdateBy;
                            DT.UpdatedOn = DateTime.Now;
                        }
                        else
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                if (manualData.GlobalCISOBTLead_check == true || manualData.RegionalCISOBTLead_check == true ||
                                    manualData.LocalDigitalOBTLead_check == true || manualData.GlobalCISPortraitLead_check == true ||
                                    manualData.RegionalCISPortraitLead_check == true || manualData.GlobalCISHRFeedSpecialist_check == true)
                                {
                                    DT.UpdatedBy = manualData.UpdateBy;
                                    DT.UpdatedOn = DateTime.Now;
                                }
                                else { }
                            }
                        }
                        if (manualData.GlobalCISOBTLead_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", DT.GlobalCISOBTLead, manualData.GlobalCISOBTLead, "Global Digital OBT Lead", manualData.UpdateBy);
                                DT.GlobalCISOBTLead = manualData.GlobalCISOBTLead;
                                CVP.GlobalCISOBTLead = manualData.GlobalCISOBTLead;
                            }
                        }
                        if (manualData.RegionalCISOBTLead_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", DT.RegionalCISOBTLead, manualData.RegionalCISOBTLead, "Regional Digital OBT Lead", manualData.UpdateBy);
                                DT.RegionalCISOBTLead = manualData.RegionalCISOBTLead;
                                CVP.RegionalCISOBTLead = manualData.RegionalCISOBTLead;
                            }
                        }
                        if (manualData.LocalDigitalOBTLead_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", DT.LocalDigitalOBTLead, manualData.LocalDigitalOBTLead, "Local Digital OBT Lead", manualData.UpdateBy);
                                DT.LocalDigitalOBTLead = manualData.LocalDigitalOBTLead;
                                CVP.LocalDigitalOBTLead = manualData.LocalDigitalOBTLead;
                            }
                        }
                        if (manualData.GlobalCISPortraitLead_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", DT.GlobalCISPortraitLead, manualData.GlobalCISPortraitLead, "Global Digital Portrait Lead", manualData.UpdateBy);
                                DT.GlobalCISPortraitLead = manualData.GlobalCISPortraitLead;
                                CVP.GlobalCISPortraitLead = manualData.GlobalCISPortraitLead;
                            }
                        }
                        if (manualData.RegionalCISPortraitLead_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", DT.RegionalCISPortraitLead, manualData.RegionalCISPortraitLead, "Regional Digital Portrait Lead", manualData.UpdateBy);
                                DT.RegionalCISPortraitLead = manualData.RegionalCISPortraitLead;
                                CVP.RegionalCISPortraitLead = manualData.RegionalCISPortraitLead;
                            }
                        }
                        if (manualData.GlobalCISHRFeedSpecialist_check == true)
                        {
                            if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                            {
                                auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", DT.GlobalCISHRFeedSpecialist + "", manualData.GlobalCISHRFeedSpecialist + "", "Global Digital HR Feed Specialist", manualData.UpdateBy);
                                DT.GlobalCISHRFeedSpecialist = manualData.GlobalCISHRFeedSpecialist;
                                CVP.GlobalCISHRFeedSpecialist = manualData.GlobalCISHRFeedSpecialist;
                            }
                        }
                        //if (manualData.GDS_check == true)
                        //{
                        //    auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", DT.GDS, manualData.GDS, "GDS", manualData.UpdateBy);
                        //    DT.GDS = manualData.GDS;
                        //}
                        if (manualData.ComplexityScore_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", DT.ComplexityScore + "", manualData.ComplexityScore + "", "Complexity Score", manualData.UpdateBy);
                            DT.ComplexityScore = manualData.ComplexityScore;
                            CVP.ComplexityScore = manualData.ComplexityScore;
                        }
                        if (manualData.ActivityType_check == true)
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "Replication", DT.ActivityType, manualData.ActivityType, "Activity Type", manualData.UpdateBy);
                            DT.ActivityType = manualData.ActivityType;
                            CVP.DigitalActivityType = manualData.ActivityType;
                        }
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public Boolean UpdateManualPipelinedata(ManualData manualData)
        {
            List<ManualData> list = new List<ManualData>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.ManualDatas.OrderBy(a => a.ManualID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    RevID = list[i].Revenue_ID;
                    ManualID = list[i].ManualID;
                    if (manualData.Revenue_ID.Equals(RevID) && manualData.ManualID.Equals(ManualID))
                    {
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        ManualData vp = (from s in entit.ManualDatas
                                         where s.ManualID == manualData.ManualID
                                         select s).FirstOrDefault();
                        CLRData CVP;
                        if (manualData.Revenue_ID == 400000000000000)
                        {
                            CVP = (from a in entit.CLRDatas
                                   where a.RevenueID == manualData.Revenue_ID
                                   where a.Task__Task_Record_ID_Key == vp.TaskRecordIdKey
                                   select a).FirstOrDefault();
                        }
                        else
                        {
                            CVP = (from a in entit.CLRDatas
                                   where a.RevenueID == manualData.Revenue_ID
                                   select a).FirstOrDefault();
                        }
                        if (CVP.ProjectStatus == "HP-High Potential" || CVP.ProjectStatus == "EP-Early Potential" || CVP.ProjectStatus == "P-Pipeline")
                        {
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.GoLiveDate + "", manualData.GoLiveDate + "", "Go Live Date", manualData.UpdateBy);
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.GlobalProjectManager, manualData.GlobalProjectManager, "Global Project Manager", manualData.UpdateBy);
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.RegionalProjectManager, manualData.RegionalProjectManager, "Regional Project Manager", manualData.UpdateBy);
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.AssigneeFullName, manualData.AssigneeFullName, "Local Project Manager", manualData.UpdateBy);
                            auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.ProjectLevel, manualData.ProjectLevel, "Project Level", manualData.UpdateBy);
                            //auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.GlobalCISDQSLead, manualData.GlobalCISDQSLead, "Global DQS Lead", manualData.UpdateBy);
                        }
                        else {
                        }
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Priority, manualData.Priority, "Priority", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Implementation_Type, manualData.Implementation_Type, "Implementation Type", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Pipeline_status, manualData.Pipeline_status, "Pipeline Status", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Pipeline_comments, manualData.Pipeline_comments, "Pipeline Comments", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.TXResourcing, manualData.TXResourcing, "TXResourcing", manualData.UpdateBy);
                        //auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Service_configuration, manualData.Service_configuration, "Service Configuration", manualData.UpdateBy);
                        //auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.OBT_Reseller___Direct, manualData.OBT_Reseller___Direct, "OBT Reseller Direct", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.ExpectedDecisionDate + "", manualData.ExpectedDecisionDate + "", "Expected Decision Date", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Assignment_date + "", manualData.Assignment_date + "", "Assignment date", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.ResourseRequestedDate + "", manualData.ResourseRequestedDate + "", "Resourse Requested Date", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Status, manualData.Status, "Status", manualData.UpdateBy);
                        auditLog(manualData.Revenue_ID, vp.TaskRecordIdKey, "ProjectTeamUpdate", vp.Project_Effort + "", manualData.Project_Effort + "", "Project Effort", manualData.UpdateBy);
                        vp.UpdateBy = manualData.UpdateBy;
                        vp.UpdateOn = DateTime.Now;
                        vp.Priority = manualData.Priority;
                        vp.Implementation_Type = manualData.Implementation_Type;
                        vp.Pipeline_status = manualData.Pipeline_status;
                        vp.Pipeline_comments = manualData.Pipeline_comments;
                        vp.TXResourcing = manualData.TXResourcing;
                        vp.ExpectedDecisionDate = manualData.ExpectedDecisionDate;
                        vp.Assignment_date = manualData.Assignment_date;
                        vp.ResourseRequestedDate = manualData.ResourseRequestedDate;
                        vp.Status = manualData.Status;
                        vp.ProjectLevel = manualData.ProjectLevel;
                        vp.Project_Effort = manualData.Project_Effort;
                        vp.GoLiveDate = manualData.GoLiveDate;
                        vp.GlobalProjectManager = manualData.GlobalProjectManager;
                        vp.RegionalProjectManager = manualData.RegionalProjectManager;
                        //vp.GlobalCISDQSLead = manualData.GlobalCISDQSLead;
                        vp.AssigneeFullName = manualData.AssigneeFullName;
                        CVP.ImplementationType = manualData.Implementation_Type;
                        CVP.ProjectLevel = manualData.ProjectLevel;
                        CVP.Status = manualData.Status;
                        CVP.GlobalProjectManager = manualData.GlobalProjectManager;
                        CVP.RegionalProjectManager = manualData.RegionalProjectManager;
                        CVP.AssigneeFullName = manualData.AssigneeFullName;
                        CVP.GoLiveDate = manualData.GoLiveDate;
                        if(manualData.GoLiveDate == null)
                        {}
                        else
                        {
                            CVP.GoLiveMonth = Convert.ToDateTime(manualData.GoLiveDate).ToString("MMM");
                            CVP.GoLiveYear = Convert.ToDateTime(manualData.GoLiveDate).Year.ToString();
                            CVP.YearMonth = Convert.ToDateTime(manualData.GoLiveDate).ToString("MMM") + "-" + Convert.ToDateTime(manualData.GoLiveDate).Year.ToString();
                            CVP.Quarter = Convert.ToDateTime(manualData.GoLiveDate).Month < 4 ? "Qtr 1"
                                        : (Convert.ToDateTime(manualData.GoLiveDate).Month >= 4 && Convert.ToDateTime(manualData.GoLiveDate).Month < 7) ? "Qtr 2"
                                        : (Convert.ToDateTime(manualData.GoLiveDate).Month >= 7 && Convert.ToDateTime(manualData.GoLiveDate).Month < 10) ? "Qtr 3"
                                        : (Convert.ToDateTime(manualData.GoLiveDate).Month >= 10 && Convert.ToDateTime(manualData.GoLiveDate).Month <= 12) ? "Qtr 4"
                                        : "" ?? "";
                        }
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public void auditLog(Double? RevenueID,String TaskRecordIDKey,String UsedPlatform,String OldText,String NewText,String Field,String UpdatedBy)
        {
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                if (OldText != NewText)
                {
                    AuditLog audit = new AuditLog();
                    audit.RevenueID = (double)RevenueID;
                    audit.TaskRecordIDKey = TaskRecordIDKey;
                    audit.UsedPlatForm = UsedPlatform;
                    audit.OldValue = OldText;
                    audit.NewValue = NewText;
                    audit.Field = Field;
                    audit.UpdatedOn = DateTime.Now;
                    audit.UpdatedBy = UpdatedBy;
                    audit.Status = "Active";
                    entity.AuditLogs.Add(audit);
                    entity.SaveChanges();
                }
                else
                {
                }
            }
        }
        double D_RevID;
        int D_DTid;
        public Boolean UpdateManualDigitaldata(DigitalTeamModel digitalTeam)
        {
            List<DigitalTeam> list = new List<DigitalTeam>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.DigitalTeams.OrderBy(a => a.DTID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    D_RevID = list[i].RevenueID;
                    D_DTid = list[i].DTID;
                    if (digitalTeam.RevenueID.Equals(D_RevID) && digitalTeam.DTID.Equals(D_DTid))
                    {
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        DigitalTeam vp = (from s in entit.DigitalTeams
                                         where s.DTID == digitalTeam.DTID
                                         select s).FirstOrDefault();
                        CLRData clr = (from s in entit.CLRDatas
                                          where s.CLRID == digitalTeam.CLRID
                                          select s).FirstOrDefault();
                        //auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.GDS, digitalTeam.GDS, "GDS", digitalTeam.UpdatedBy);
                        auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.ComplexityScore+"", digitalTeam.ComplexityScore+"", "Complexity Score", digitalTeam.UpdatedBy);
                        auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.ActivityType, digitalTeam.ActivityType, "Activity Type", digitalTeam.UpdatedBy);
                        if(clr.ProjectStatus == "HP-High Potential" || clr.ProjectStatus == "EP-Early Potential" || clr.ProjectStatus == "P-Pipeline")
                        {
                            auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.GlobalCISOBTLead, digitalTeam.GlobalCISOBTLead, "Global Digital OBT Lead", digitalTeam.UpdatedBy);
                            auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.RegionalCISOBTLead, digitalTeam.RegionalCISOBTLead, "Regional Digital OBT Lead", digitalTeam.UpdatedBy);
                            auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.LocalDigitalOBTLead, digitalTeam.LocalDigitalOBTLead, "Local Digital OBT Lead", digitalTeam.UpdatedBy);
                            auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.GlobalCISPortraitLead, digitalTeam.GlobalCISPortraitLead, "Global Digital Portrait Lead", digitalTeam.UpdatedBy);
                            auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.RegionalCISPortraitLead, digitalTeam.RegionalCISPortraitLead, "Regional Digital Portrait Lead", digitalTeam.UpdatedBy);
                            auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.GlobalCISHRFeedSpecialist, digitalTeam.GlobalCISHRFeedSpecialist, "Global Digital HR Feed Specialist", digitalTeam.UpdatedBy);
                            auditLog(digitalTeam.RevenueID, vp.TaskRecordIdKey, "DigitalTeamUpdate", vp.GlobalCISDQSLead, digitalTeam.GlobalCISDQSLead, "Global DQS Lead", digitalTeam.UpdatedBy);
                        }
                        else{}
                        vp.UpdatedBy = digitalTeam.UpdatedBy;
                        vp.UpdatedOn = DateTime.Now;
                        vp.GlobalCISOBTLead = digitalTeam.GlobalCISOBTLead;
                        vp.RegionalCISOBTLead = digitalTeam.RegionalCISOBTLead;
                        vp.LocalDigitalOBTLead = digitalTeam.LocalDigitalOBTLead;
                        vp.GlobalCISPortraitLead = digitalTeam.GlobalCISPortraitLead;
                        vp.RegionalCISPortraitLead = digitalTeam.RegionalCISPortraitLead;
                        vp.GlobalCISHRFeedSpecialist = digitalTeam.GlobalCISHRFeedSpecialist;
                        vp.GlobalCISDQSLead = digitalTeam.GlobalCISDQSLead;
                        //vp.GDS = digitalTeam.GDS;
                        vp.ActivityType = digitalTeam.ActivityType;
                        vp.ComplexityScore = digitalTeam.ComplexityScore;
                        clr.DigitalActivityType = digitalTeam.ActivityType;
                        clr.ComplexityScore = digitalTeam.ComplexityScore;
                        clr.GlobalCISDQSLead = digitalTeam.GlobalCISOBTLead;
                        clr.RegionalCISOBTLead = digitalTeam.RegionalCISOBTLead;
                        clr.LocalDigitalOBTLead = digitalTeam.LocalDigitalOBTLead;
                        clr.GlobalCISPortraitLead = digitalTeam.GlobalCISPortraitLead;
                        clr.RegionalCISPortraitLead = digitalTeam.RegionalCISPortraitLead;
                        clr.GlobalCISHRFeedSpecialist = digitalTeam.GlobalCISHRFeedSpecialist;
                        clr.GlobalCISDQSLead = digitalTeam.GlobalCISDQSLead;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }

        public Boolean GenerateTracker()
        {
            ImportsController imports_controller = new ImportsController();
            imports_controller.GenerateTracker();
            //CWTDashboardEntities entity = new CWTDashboardEntities();
            //SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["con"].ConnectionString);
            //var CurrentYear = DateTime.Now.Year;
            //var ProjectStatus = "A-Active/Date Confirmed,HP-High Potential,P-Pipeline,N-Active/No Date Confirmed".Split(',');
            //DateTime firstDay = new DateTime(CurrentYear, 1, 1);
            //var Tracker = (from a in entity.CLRDatas
            //               where a.OwnerShip != "Minority Holding"
            //               where a.OwnerShip != "Not Present"
            //               where a.GoLiveDate >= firstDay
            //               where a.Status == "Active"
            //               where ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
            //               where a.MilestoneType != "POS"
            //               select a).ToList();
            //DataTable tbl3 = new DataTable();
            //tbl3.Columns.Add(new DataColumn("RevenueID", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("Region", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("Country", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("GlobalProjectManager", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("RegionalProjectManager", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("LocalProjectManager", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("Client", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("iMeetWorkspaceTitle", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("OwnershipType", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("PriorityCustomer", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("Volume", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("ProjectLevel", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("ImplementationType", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("ProjectStatus", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("GlobalDigitalOBTLead", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("RegionalDigitalOBTLead", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("LocalDigitalOBTLead", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("GlobalDigitalPortraitLead", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("RegionalDigitalPortraitLead", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("GlobalDigitalHRFeedSpeciallist", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("GDS", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("ComplexityScore", typeof(Int64)));
            //tbl3.Columns.Add(new DataColumn("MilestoneProjectNotes", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("ProjectEffort", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("KickOff_ProposedStartDate", typeof(DateTime)));
            //tbl3.Columns.Add(new DataColumn("GoLiveDate", typeof(DateTime)));
            //tbl3.Columns.Add(new DataColumn("EndofHypercare", typeof(DateTime)));
            //tbl3.Columns.Add(new DataColumn("CompleteDuration", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("PerCompleted", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("ProjectStartDate", typeof(DateTime)));
            //tbl3.Columns.Add(new DataColumn("MilestoneDueDate", typeof(DateTime)));
            //tbl3.Columns.Add(new DataColumn("ProjectDelay", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("TaskRecordIdKey", typeof(string)));
            //tbl3.Columns.Add(new DataColumn("1stweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("2ndweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("3rdweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("4thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("5thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("6thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("7thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("8thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("9thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("10thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("11thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("12thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c13thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c14thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c15thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c16thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c17thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c18thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c19thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c20thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c21thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c22thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c23thweek", typeof(double)));
            //tbl3.Columns.Add(new DataColumn("c24thweek", typeof(double)));
            //DayOfWeek currentDay = DateTime.Now.DayOfWeek;
            //int daysTillCurrentDay = currentDay - DayOfWeek.Monday;
            //DateTime currentWeekStartDate = DateTime.Now.AddDays(-daysTillCurrentDay);
            //DateTime? EndofHypercare, KickOff_ProposedStartDate;
            //foreach (var r in Tracker)
            //{
            //    DataRow dr3 = tbl3.NewRow();
            //    dr3["RevenueID"] = (object)r.RevenueID ?? DBNull.Value;
            //    EndofHypercare = r.MilestoneDueDate != null ? r.MilestoneDueDate
            //                        : r.GoLiveDate == null ? (DateTime?)null
            //                        : r.Region == "APAC" && r.Country == "AUSTRALIA" ? r.GoLiveDate.Value.AddDays(14) : r.GoLiveDate.Value.AddDays(28);
            //    KickOff_ProposedStartDate = r.ProjectStart_ForCycleTime != null ? r.ProjectStart_ForCycleTime
            //                        : EndofHypercare != null ? entity.Configs.FirstOrDefault(x => x.ProjectType == r.ImplementationType).Duration != null ? EndofHypercare.Value.AddDays(-(entity.Configs.FirstOrDefault(x => x.ProjectType == r.ImplementationType).Duration * 7))
            //                        : (DateTime?)null : (DateTime?)null;
            //    dr3["Region"] = (object)r.Region ?? "";
            //    dr3["Country"] = (object)r.Country ?? "";
            //    dr3["GlobalProjectManager"] = (object)r.GlobalProjectManager ?? "";
            //    dr3["RegionalProjectManager"] = (object)r.RegionalProjectManager ?? "";
            //    dr3["LocalProjectManager"] = (object)r.AssigneeFullName ?? "";
            //    dr3["Client"] = (object)r.Client ?? "";
            //    dr3["iMeetWorkspaceTitle"] = (object)r.Workspace_Title ?? "";
            //    dr3["OwnershipType"] = (object)r.OwnerShip ?? "";
            //    dr3["PriorityCustomer"] = "";
            //    dr3["Volume"] = (object)r.RevenueVolumeUSD ?? 0;
            //    dr3["ProjectLevel"] = (object)r.ProjectLevel ?? "";
            //    dr3["ImplementationType"] = (object)r.ImplementationType ?? "";
            //    dr3["ProjectStatus"] = (object)r.ProjectStatus ?? "";
            //    dr3["GlobalDigitalOBTLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).GlobalCISOBTLead : r.GlobalCISOBTLead ?? "";
            //    dr3["RegionalDigitalOBTLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).RegionalCISOBTLead : r.RegionalCISOBTLead ?? "";
            //    dr3["LocalDigitalOBTLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).LocalDigitalOBTLead
            //                                : r.LocalDigitalOBTLead != "Deepak Girtola" ? r.LocalDigitalOBTLead : r.Region == "APAC" && r.Country == "INDIA" ? r.LocalDigitalOBTLead : "" ?? "";
            //    dr3["GlobalDigitalPortraitLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).GlobalCISPortraitLead : r.GlobalCISPortraitLead ?? "";
            //    dr3["RegionalDigitalPortraitLead"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).RegionalCISPortraitLead : r.RegionalCISPortraitLead ?? "";
            //    dr3["GlobalDigitalHRFeedSpeciallist"] = r.ProjectStatus == "P-Pipeline" || r.ProjectStatus == "EP-Early Potential" || r.ProjectStatus == "HP-High Potential" ? entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).GlobalCISHRFeedSpecialist : r.GlobalCISHRFeedSpecialist ?? "";
            //    dr3["GDS"] = r.RevenueID == 400000000000000 ? entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key).GDS : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).GDS ?? "";
            //    dr3["ComplexityScore"] = r.RevenueID == 400000000000000 ? entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key).ComplexityScore : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == r.RevenueID).ComplexityScore;
            //    dr3["MilestoneProjectNotes"] = (object)r.Milestone__Project_Notes ?? "";
            //    dr3["ProjectEffort"] = r.RevenueID == 400000000000000 ? entity.ManualDatas.FirstOrDefault(x => x.TaskRecordIdKey == r.Task__Task_Record_ID_Key).Project_Effort
            //        : entity.ManualDatas.FirstOrDefault(x => x.Revenue_ID == r.RevenueID).Project_Effort ?? 1;
            //    dr3["KickOff_ProposedStartDate"] = KickOff_ProposedStartDate;
            //    dr3["GoLiveDate"] = (object)r.GoLiveDate ?? DBNull.Value;
            //    dr3["EndofHypercare"] = EndofHypercare;
            //    dr3["CompleteDuration"] = EndofHypercare == null || KickOff_ProposedStartDate == null ? "0 Weeks"
            //        : (EndofHypercare - KickOff_ProposedStartDate).Value.TotalDays / 7 + "Weeks";
            //    dr3["PerCompleted"] = (object)r.PerCompleted ?? DBNull.Value;
            //    dr3["ProjectStartDate"] = (object)r.ProjectStart_ForCycleTime ?? DBNull.Value;
            //    dr3["MilestoneDueDate"] = (object)r.MilestoneDueDate ?? DBNull.Value;
            //    dr3["ProjectDelay"] = EndofHypercare == null || KickOff_ProposedStartDate == null ? 0
            //        : (EndofHypercare - KickOff_ProposedStartDate).Value.TotalDays / 7;
            //    dr3["TaskRecordIdKey"] = (object)r.Task__Task_Record_ID_Key == null ? "" : r.Task__Task_Record_ID_Key;
            //    dr3["1stweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate >= KickOff_ProposedStartDate && currentWeekStartDate <= EndofHypercare ? 1 : 0;
            //    dr3["2ndweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7) <= EndofHypercare ? 1 : 0;
            //    dr3["3rdweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 2) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 2) <= EndofHypercare ? 1 : 0;
            //    dr3["4thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 3) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 3) <= EndofHypercare ? 1 : 0;
            //    dr3["5thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 4) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 4) <= EndofHypercare ? 1 : 0;
            //    dr3["6thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 5) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 5) <= EndofHypercare ? 1 : 0;
            //    dr3["7thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 6) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 6) <= EndofHypercare ? 1 : 0;
            //    dr3["8thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 7) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 7) <= EndofHypercare ? 1 : 0;
            //    dr3["9thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 8) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 8) <= EndofHypercare ? 1 : 0;
            //    dr3["10thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 9) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 9) <= EndofHypercare ? 1 : 0;
            //    dr3["11thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 10) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 10) <= EndofHypercare ? 1 : 0;
            //    dr3["12thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 11) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 11) <= EndofHypercare ? 1 : 0;
            //    dr3["c13thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 12) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 12) <= EndofHypercare ? 1 : 0;
            //    dr3["c14thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 13) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 13) <= EndofHypercare ? 1 : 0;
            //    dr3["c15thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 14) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 14) <= EndofHypercare ? 1 : 0;
            //    dr3["c16thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 15) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 15) <= EndofHypercare ? 1 : 0;
            //    dr3["c17thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 16) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 16) <= EndofHypercare ? 1 : 0;
            //    dr3["c18thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 17) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 17) <= EndofHypercare ? 1 : 0;
            //    dr3["c19thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 18) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 18) <= EndofHypercare ? 1 : 0;
            //    dr3["c20thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 19) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 19) <= EndofHypercare ? 1 : 0;
            //    dr3["c21thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 20) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 20) <= EndofHypercare ? 1 : 0;
            //    dr3["c22thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 21) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 21) <= EndofHypercare ? 1 : 0;
            //    dr3["c23thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 22) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 22) <= EndofHypercare ? 1 : 0;
            //    dr3["c24thweek"] = KickOff_ProposedStartDate == null || EndofHypercare == null ? 0 : currentWeekStartDate.AddDays(7 * 23) >= KickOff_ProposedStartDate && currentWeekStartDate.AddDays(7 * 23) <= EndofHypercare ? 1 : 0;
            //    tbl3.Rows.Add(dr3);
            //}
            //SqlBulkCopy objbulk3 = new SqlBulkCopy(con);
            //objbulk3.DestinationTableName = "CLRTracker";
            //objbulk3.ColumnMappings.Add("RevenueID", "RevenueID");
            //objbulk3.ColumnMappings.Add("Region", "Region");
            //objbulk3.ColumnMappings.Add("Country", "Country");
            //objbulk3.ColumnMappings.Add("GlobalProjectManager", "GlobalProjectManager");
            //objbulk3.ColumnMappings.Add("RegionalProjectManager", "RegionalProjectManager");
            //objbulk3.ColumnMappings.Add("LocalProjectManager", "LocalProjectManager");
            //objbulk3.ColumnMappings.Add("Client", "Client");
            //objbulk3.ColumnMappings.Add("iMeetWorkspaceTitle", "iMeetWorkspaceTitle");
            //objbulk3.ColumnMappings.Add("OwnershipType", "OwnershipType");
            //objbulk3.ColumnMappings.Add("PriorityCustomer", "PriorityCustomer");
            //objbulk3.ColumnMappings.Add("Volume", "Volume");
            //objbulk3.ColumnMappings.Add("ProjectLevel", "ProjectLevel");
            //objbulk3.ColumnMappings.Add("ImplementationType", "ImplementationType");
            //objbulk3.ColumnMappings.Add("ProjectStatus", "ProjectStatus");
            //objbulk3.ColumnMappings.Add("GlobalDigitalOBTLead", "GlobalDigitalOBTLead");
            //objbulk3.ColumnMappings.Add("RegionalDigitalOBTLead", "RegionalDigitalOBTLead");
            //objbulk3.ColumnMappings.Add("LocalDigitalOBTLead", "LocalDigitalOBTLead");
            //objbulk3.ColumnMappings.Add("GlobalDigitalPortraitLead", "GlobalDigitalPortraitLead");
            //objbulk3.ColumnMappings.Add("RegionalDigitalPortraitLead", "RegionalDigitalPortraitLead");
            //objbulk3.ColumnMappings.Add("GlobalDigitalHRFeedSpeciallist", "GlobalDigitalHRFeedSpeciallist");
            //objbulk3.ColumnMappings.Add("GDS", "GDS");
            //objbulk3.ColumnMappings.Add("ComplexityScore", "ComplexityScore");
            //objbulk3.ColumnMappings.Add("MilestoneProjectNotes", "MilestoneProjectNotes");
            //objbulk3.ColumnMappings.Add("ProjectEffort", "ProjectEffort");
            //objbulk3.ColumnMappings.Add("KickOff_ProposedStartDate", "KickOff_ProposedStartDate");
            //objbulk3.ColumnMappings.Add("GoLiveDate", "GoLiveDate");
            //objbulk3.ColumnMappings.Add("EndofHypercare", "EndofHypercare");
            //objbulk3.ColumnMappings.Add("CompleteDuration", "CompleteDuration");
            //objbulk3.ColumnMappings.Add("PerCompleted", "PerCompleted");
            //objbulk3.ColumnMappings.Add("ProjectStartDate", "ProjectStartDate");
            //objbulk3.ColumnMappings.Add("MilestoneDueDate", "MilestoneDueDate");
            //objbulk3.ColumnMappings.Add("ProjectDelay", "ProjectDelay");
            //objbulk3.ColumnMappings.Add("TaskRecordIdKey", "TaskRecordIdKey");
            //objbulk3.ColumnMappings.Add("1stweek", "1stweek");
            //objbulk3.ColumnMappings.Add("2ndweek", "2ndweek");
            //objbulk3.ColumnMappings.Add("3rdweek", "3rdweek");
            //objbulk3.ColumnMappings.Add("4thweek", "4thweek");
            //objbulk3.ColumnMappings.Add("5thweek", "5thweek");
            //objbulk3.ColumnMappings.Add("6thweek", "6thweek");
            //objbulk3.ColumnMappings.Add("7thweek", "7thweek");
            //objbulk3.ColumnMappings.Add("8thweek", "8thweek");
            //objbulk3.ColumnMappings.Add("9thweek", "9thweek");
            //objbulk3.ColumnMappings.Add("10thweek", "10thweek");
            //objbulk3.ColumnMappings.Add("11thweek", "11thweek");
            //objbulk3.ColumnMappings.Add("12thweek", "12thweek");
            //objbulk3.ColumnMappings.Add("c13thweek", "c13thweek");
            //objbulk3.ColumnMappings.Add("c14thweek", "c14thweek");
            //objbulk3.ColumnMappings.Add("c15thweek", "c15thweek");
            //objbulk3.ColumnMappings.Add("c16thweek", "c16thweek");
            //objbulk3.ColumnMappings.Add("c17thweek", "c17thweek");
            //objbulk3.ColumnMappings.Add("c18thweek", "c18thweek");
            //objbulk3.ColumnMappings.Add("c19thweek", "c19thweek");
            //objbulk3.ColumnMappings.Add("c20thweek", "c20thweek");
            //objbulk3.ColumnMappings.Add("c21thweek", "c21thweek");
            //objbulk3.ColumnMappings.Add("c22thweek", "c22thweek");
            //objbulk3.ColumnMappings.Add("c23thweek", "c23thweek");
            //objbulk3.ColumnMappings.Add("c24thweek", "c24thweek");
            //con.Open();
            //string s2 = "Truncate Table CLRTracker";
            //SqlCommand Com2 = new SqlCommand(s2, con);
            //Com2.ExecuteNonQuery();
            //objbulk3.BatchSize = 100000;
            //objbulk3.BulkCopyTimeout = 0;
            //objbulk3.WriteToServer(tbl3);
            //con.Close();
            return true;
        }
    }
}