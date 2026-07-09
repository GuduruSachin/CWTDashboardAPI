using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class Adhoc
    {
        double? RevenueID, U_RevenueID, D_RevenueID;
        public int count;
        public Boolean AddingAdhoc(AdHocProject adHocProject)
        {
            List<AdHocProject> list = new List<AdHocProject>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.AdHocProjects.OrderBy(a => a.AHID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    RevenueID = list[i].RevenueID;
                    if (adHocProject.RevenueID.Equals(RevenueID))
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
        public Boolean UpdateAdhoc(AdHocProject adHocProject)
        {
            List<AdHocProject> list = new List<AdHocProject>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.AdHocProjects.OrderBy(a => a.AHID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    U_RevenueID = list[i].RevenueID;
                    if (adHocProject.RevenueID.Equals(U_RevenueID))
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
                        AdHocProject vp = (from s in entit.AdHocProjects
                                        where s.RevenueID == adHocProject.RevenueID
                                        select s).FirstOrDefault();
                        ManualData md = (from a in entit.ManualDatas
                                         where a.Revenue_ID == adHocProject.RevenueID
                                         select a).FirstOrDefault();
                        CLRData CLR = (from a in entit.CLRDatas
                                          where a.RevenueID == adHocProject.RevenueID
                                          select a).FirstOrDefault();
                        ManualDataClass manualData = new ManualDataClass();
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.Client, adHocProject.Client, "Client", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.StartDate+"", adHocProject.StartDate+"", "Start Date", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.GoLiveDate+"", adHocProject.GoLiveDate+"", "Go Live Date", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.Country, adHocProject.Country, "Country", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.Region, adHocProject.Region, "Region", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.Comments, adHocProject.Comments, "Comments", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.Priority, adHocProject.Priority, "Priority", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.Pipeline_Comments, adHocProject.Pipeline_Comments, "Pipeline Comments", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.GlobalCISOBTLead, adHocProject.GlobalCISOBTLead, "Global Digital OBT Lead", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.RegionalCISOBTLead, adHocProject.RegionalCISOBTLead, "Regional Digital OBT Lead", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.LocalDigitalOBTLead, adHocProject.LocalDigitalOBTLead, "Local Digital OBT Lead", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.GlobalCISPortraitLead, adHocProject.GlobalCISPortraitLead, "Global Digital Portrait Lead", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.RegionalCISPortraitLead, adHocProject.RegionalCISPortraitLead, "Regional Digital Portrait Lead", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.GlobalCISHRFeedSpecialist, adHocProject.GlobalCISHRFeedSpecialist, "Global Digital HRFeed Specialist", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.GDS, adHocProject.GDS, "GDS", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.ComplexityScore+"", adHocProject.ComplexityScore+"", "Complexity Score", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.ActivityType, adHocProject.ActivityType, "Activity Type", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.Status, adHocProject.Status, "Status", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.ProjectStatus, adHocProject.ProjectStatus, "Project Status", adHocProject.UpdatedBy);
                        manualData.auditLog(adHocProject.RevenueID, "", "AdhocUpdate", vp.GlobalDQSLead, adHocProject.GlobalDQSLead, "Global DQS Lead", adHocProject.UpdatedBy);
                        vp.Client = adHocProject.Client;
                        vp.StartDate = adHocProject.StartDate;
                        vp.GoLiveDate = adHocProject.GoLiveDate;
                        vp.Country = adHocProject.Country;
                        vp.Region = adHocProject.Region;
                        vp.Comments = adHocProject.Comments;
                        vp.GlobalCISOBTLead = adHocProject.GlobalCISOBTLead;
                        vp.RegionalCISOBTLead = adHocProject.RegionalCISOBTLead;
                        vp.LocalDigitalOBTLead = adHocProject.LocalDigitalOBTLead;
                        vp.GlobalCISPortraitLead = adHocProject.GlobalCISPortraitLead;
                        vp.RegionalCISPortraitLead = adHocProject.RegionalCISPortraitLead;
                        vp.GlobalCISHRFeedSpecialist = adHocProject.GlobalCISHRFeedSpecialist;
                        vp.GlobalDQSLead = adHocProject.GlobalDQSLead;
                        vp.GDS = adHocProject.GDS;
                        vp.ComplexityScore = adHocProject.ComplexityScore;
                        vp.ActivityType = adHocProject.ActivityType;
                        vp.Status = adHocProject.Status;
                        vp.ProjectStatus = adHocProject.ProjectStatus;
                        vp.UpdatedOn = DateTime.Now;
                        vp.UpdatedBy = adHocProject.UpdatedBy;
                        vp.Priority = adHocProject.Priority;
                        vp.Pipeline_Comments = adHocProject.Pipeline_Comments;
                        md.Status = adHocProject.Status;
                        md.UpdateOn = DateTime.Now;
                        md.UpdateBy = adHocProject.UpdatedBy;
                        md.Pipeline_comments = adHocProject.Pipeline_Comments;
                        md.Priority = adHocProject.Priority;
                        CLR.Region = adHocProject.Region;
                        CLR.Country = adHocProject.Country;
                        CLR.Client = adHocProject.Client;
                        CLR.GoLiveDate = adHocProject.GoLiveDate;
                        CLR.ProjectStart_ForCycleTime = adHocProject.StartDate;
                        CLR.ProjectStatus = adHocProject.ProjectStatus;
                        CLR.GlobalCISOBTLead = adHocProject.GlobalCISOBTLead;
                        CLR.RegionalCISOBTLead = adHocProject.RegionalCISOBTLead;
                        CLR.LocalDigitalOBTLead = adHocProject.LocalDigitalOBTLead;
                        CLR.GlobalCISPortraitLead = adHocProject.GlobalCISPortraitLead;
                        CLR.RegionalCISPortraitLead = adHocProject.RegionalCISPortraitLead;
                        CLR.GlobalCISHRFeedSpecialist = adHocProject.GlobalCISHRFeedSpecialist;
                        CLR.GlobalCISDQSLead = adHocProject.GlobalDQSLead;
                        CLR.GDS = adHocProject.GDS;
                        CLR.Status = adHocProject.Status;
                        CLR.GoLiveMonth = Convert.ToDateTime(adHocProject.GoLiveDate).ToString("MMM");
                        CLR.GoLiveYear = Convert.ToDateTime(adHocProject.GoLiveDate).Year.ToString();
                        CLR.Quarter = (Convert.ToDateTime(adHocProject.GoLiveDate).Month < 4 ? "Qtr 1"
                            : (Convert.ToDateTime(adHocProject.GoLiveDate).Month >= 4 && Convert.ToDateTime(adHocProject.GoLiveDate).Month < 7) ? "Qtr 2"
                            : (Convert.ToDateTime(adHocProject.GoLiveDate).Month >= 7 && Convert.ToDateTime(adHocProject.GoLiveDate).Month < 10) ? "Qtr 3"
                            : (Convert.ToDateTime(adHocProject.GoLiveDate).Month >= 10 && Convert.ToDateTime(adHocProject.GoLiveDate).Month <= 12) ? "Qtr 4"
                            : null);
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public Boolean DeleteAdhoc(AdHocProject adHocProject)
        {
            List<AdHocProject> list = new List<AdHocProject>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.AdHocProjects.OrderBy(a => a.AHID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    D_RevenueID = list[i].RevenueID;
                    if (adHocProject.RevenueID.Equals(D_RevenueID))
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
                        AdHocProject vp = (from s in entit.AdHocProjects
                                        where s.RevenueID == adHocProject.RevenueID
                                           select s).FirstOrDefault();
                        vp.Status = "Delete";
                        vp.UpdatedOn = DateTime.Now;
                        vp.UpdatedBy = adHocProject.UpdatedBy;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
    }
}