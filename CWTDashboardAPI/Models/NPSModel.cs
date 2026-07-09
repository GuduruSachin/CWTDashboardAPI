using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class NPSModel
    {
        int NPSID,count;
        long maxnum, maxnumci;
        ManualDataClass md_c_audit = new ManualDataClass();
        public Boolean UpdateNPSClientInfo(NpsImp npsImp)
        {
            List<NpsImp> list = new List<NpsImp>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.NpsImps.OrderBy(a => a.NpsId).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    NPSID = list[i].NpsId;
                    if (npsImp.NpsId.Equals(NPSID))
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
                        NpsImp vp = (from s in entit.NpsImps
                                     where s.NpsId == npsImp.NpsId
                                     select s).FirstOrDefault();
                        maxnumci = entit.AuditEntries.Select(x => x.AuditEntryId).DefaultIfEmpty(0).Max() + 1;
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.ClientName + "", npsImp.ClientName + "", "Client Name", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.ClientType + "", npsImp.ClientType + "", "Client Type", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.LocalProjectManager + "", npsImp.LocalProjectManager + "", "Local Project Manager", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.Language + "", npsImp.Language + "", "Language", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.GlobalProjectManager + "", npsImp.GlobalProjectManager + "", "Global Project Manager", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.RegionalProjectManager + "", npsImp.RegionalProjectManager + "", "Regional Project Manager", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.Country + "", npsImp.Country + "", "Country", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.Region + "", npsImp.Region + "", "Region", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.Email + "", npsImp.Email + "", "Email", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.CustomerContactNumber + "", npsImp.CustomerContactNumber + "", "Customer Contact Number", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.Company + "", npsImp.Company + "", "Company", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.GlobalDigitalPM + "", npsImp.GlobalDigitalPM + "", "Global Digital OBT Lead", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.RegionalDigitalPM + "", npsImp.RegionalDigitalPM + "", "Regional Digital OBT Lead", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.LocalDigitalPM + "", npsImp.LocalDigitalPM + "", "Local Digital OBT Lead", npsImp.UpdatedBy);
                        auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.GoLiveDate + "", npsImp.GoLiveDate + "", "Go Live Date", npsImp.UpdatedBy);
                        vp.ClientName = npsImp.ClientName;
                        vp.ClientType = npsImp.ClientType;
                        //vp.SingleResource = npsImp.SingleResource;
                        //auditLog(maxnumci, npsImp.NpsId, "", "NPS PM", vp.SingleResource + "", npsImp.SingleResource + "", "Single Resource", npsImp.UpdatedBy);
                        vp.LocalProjectManager = npsImp.LocalProjectManager;
                        vp.Language = npsImp.Language;
                        vp.GlobalProjectManager = npsImp.GlobalProjectManager;
                        vp.RegionalProjectManager = npsImp.RegionalProjectManager;
                        vp.Country = npsImp.Country;
                        vp.Region = npsImp.Region;
                        vp.Email = npsImp.Email;
                        vp.CustomerContactNumber = npsImp.CustomerContactNumber;
                        vp.Company = npsImp.Company;
                        vp.GoLiveDate = npsImp.GoLiveDate;
                        vp.GlobalDigitalPM = npsImp.GlobalDigitalPM;
                        vp.RegionalDigitalPM = npsImp.RegionalDigitalPM;
                        vp.LocalDigitalPM = npsImp.LocalDigitalPM;
                        vp.UpdatedOn = DateTime.Now;
                        vp.UpdatedBy = npsImp.UpdatedBy;
                        if (entit.AuditLogs.Where(x => x.AuditEntryId == (long?)maxnumci).Count() > 0)
                        {
                            AuditEntry ae = new AuditEntry();
                            ae.ReportName = "NPS PM";
                            ae.Record = npsImp.NpsId + "";
                            ae.UpdatedOn = DateTime.Now;
                            ae.UpdatedBy = npsImp.UpdatedBy;
                            entit.AuditEntries.Add(ae);
                        }
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }

        public Boolean UpdateNPSView(NpsImp npsImp)
        {
            List<NpsImp> list = new List<NpsImp>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.NpsImps.OrderBy(a => a.NpsId).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    NPSID = list[i].NpsId;
                    if (npsImp.NpsId.Equals(NPSID))
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
                        NpsImp vp = (from s in entit.NpsImps
                                     where s.NpsId == npsImp.NpsId
                                     select s).FirstOrDefault();
                        maxnum = entit.AuditEntries.Select(x => x.AuditEntryId).DefaultIfEmpty(0).Max() + 1;
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.DateServeySent + "", npsImp.DateServeySent + "", "Date Survey Sent", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.ClientScope + "", npsImp.ClientScope + "", "Client Scope", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.DateSurveyReceived + "", npsImp.DateSurveyReceived + "", "Date Survey Received", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.Status + "", npsImp.Status + "", "Status", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.AssignLeaderForClosedLoop + "", npsImp.AssignLeaderForClosedLoop + "", "Assign Leader For Closed Loop", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.NPSScore + "", npsImp.NPSScore + "", "NPS Score", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.OpprtunityId + "", npsImp.OpprtunityId + "", "Opprtunity Id", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.NPSIndicator + "", npsImp.NPSIndicator + "", "NPS Indicator", npsImp.UpdatedBy);                        
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.NPSCommentsWhatwasPositive + "", npsImp.NPSCommentsWhatwasPositive + "", "NPS Comments What was Positive", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.NPSComments_Howcouldwehaveimproved + "", npsImp.NPSComments_Howcouldwehaveimproved + "", "NPS Comments-How could we have improved", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.NPSComments_Whatistheonethingwecandotomakeyouhappier + "", npsImp.NPSComments_Whatistheonethingwecandotomakeyouhappier + "", "NPS Comments What is the one thing we can do to make you happier", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.NPSCommentsOne + "", npsImp.NPSCommentsOne + "", "What had a Positive Experience during your Implementation", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.NPSCommentsTwo + "", npsImp.NPSCommentsTwo + "", "What can we improve to make your experience better", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.NPSCommentsThree + "", npsImp.NPSCommentsThree + "", "What can we do to go above expectations", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.ClientFeedback + "", npsImp.ClientFeedback + "", "Client Feedback", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.Action + "", npsImp.Action + "", "Action", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.ClientName + "", npsImp.ClientName + "", "Client Name", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.ClientType + "", npsImp.ClientType + "", "Client Type", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.LocalProjectManager + "", npsImp.LocalProjectManager + "", "Local Project Manager", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.GlobalProjectManager + "", npsImp.GlobalProjectManager + "", "Global Project Manager", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.RegionalProjectManager + "", npsImp.RegionalProjectManager + "", "Regional Project Manager", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.GlobalDigitalPM + "", npsImp.GlobalDigitalPM + "", "Global Digital OBT Lead", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.RegionalDigitalPM + "", npsImp.RegionalDigitalPM + "", "Regional Digital OBT Lead", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.LocalDigitalPM + "", npsImp.LocalDigitalPM + "", "Local Digital OBT Lead", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.Country + "", npsImp.Country + "", "Country", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.Region + "", npsImp.Region + "", "Region", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.Email + "", npsImp.Email + "", "Email Id", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.CustomerContactNumber + "", npsImp.CustomerContactNumber + "", "Customer Contact Number", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.Company + "", npsImp.Company + "", "Company", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.ReasonType + "", npsImp.ReasonType + "", "Reason Type", npsImp.UpdatedBy);
                        auditLog(maxnum, npsImp.NpsId, "", "NPS Admin", vp.Language + "", npsImp.Language + "", "Language", npsImp.UpdatedBy);
                        vp.UpdatedOn = DateTime.Now;
                        vp.UpdatedBy = npsImp.UpdatedBy;
                        vp.DateServeySent = npsImp.DateServeySent;
                        vp.ClientScope = npsImp.ClientScope;
                        vp.DateSurveyReceived = npsImp.DateSurveyReceived;
                        if(npsImp.DateSurveyReceived != null)
                        {
                            vp.YearMonth = npsImp.DateSurveyReceived.Value.Year + "-" + npsImp.DateSurveyReceived.Value.ToString("MM");
                        }
                        vp.Status = npsImp.Status;
                        vp.AssignLeaderForClosedLoop = npsImp.AssignLeaderForClosedLoop;
                        vp.NPSScore = npsImp.NPSScore;
                        vp.NPSIndicator = npsImp.NPSIndicator;
                        vp.OpprtunityId = npsImp.OpprtunityId; 
                        vp.NPSCommentsWhatwasPositive = npsImp.NPSCommentsWhatwasPositive;
                        vp.NPSComments_Howcouldwehaveimproved = npsImp.NPSComments_Howcouldwehaveimproved;
                        vp.NPSComments_Whatistheonethingwecandotomakeyouhappier = npsImp.NPSComments_Whatistheonethingwecandotomakeyouhappier;
                        vp.NPSCommentsOne = npsImp.NPSCommentsOne;
                        vp.NPSCommentsTwo = npsImp.NPSCommentsTwo;
                        vp.NPSCommentsThree = npsImp.NPSCommentsThree;
                        vp.ClientFeedback = npsImp.ClientFeedback;
                        vp.Action = npsImp.Action;
                        vp.ClientName = npsImp.ClientName;
                        vp.ClientType = npsImp.ClientType;
                        vp.Language = npsImp.Language;
                        vp.GlobalProjectManager = npsImp.GlobalProjectManager;
                        vp.RegionalProjectManager = npsImp.RegionalProjectManager;
                        vp.LocalProjectManager = npsImp.LocalProjectManager;
                        vp.GlobalDigitalPM = npsImp.GlobalDigitalPM;
                        vp.RegionalDigitalPM = npsImp.RegionalDigitalPM;
                        vp.LocalDigitalPM = npsImp.LocalDigitalPM;
                        vp.Country = npsImp.Country;
                        vp.Region = npsImp.Region;
                        vp.Email = npsImp.Email;
                        vp.CustomerContactNumber = npsImp.CustomerContactNumber;
                        vp.Company = npsImp.Company;
                        vp.ReasonType = npsImp.ReasonType;
                        vp.RecordStatus = npsImp.RecordStatus;
                        if(entit.AuditLogs.Where(x => x.AuditEntryId == (long?)maxnum).Count() > 0)
                        {
                            AuditEntry ae = new AuditEntry();
                            ae.ReportName = "NPS Admin";
                            ae.Record = npsImp.NpsId+"";
                            ae.UpdatedOn = DateTime.Now;
                            ae.UpdatedBy = npsImp.UpdatedBy;
                            entit.AuditEntries.Add(ae);
                        }
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public void auditLog(long AuditEntryid,Double? RevenueID, String TaskRecordIDKey, String UsedPlatform, String OldText, String NewText, String Field, String UpdatedBy)
        {
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                if (OldText != NewText)
                {
                    AuditLog audit = new AuditLog();
                    audit.AuditEntryId = AuditEntryid;
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
        public Boolean DeleteNPS(NpsImp npsImp)
        {
            List<NpsImp> list = new List<NpsImp>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.NpsImps.OrderBy(a => a.NpsId).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    NPSID = list[i].NpsId;
                    if (npsImp.NpsId.Equals(NPSID))
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
                        NpsImp vp = (from s in entit.NpsImps
                                     where s.NpsId == npsImp.NpsId
                                     select s).FirstOrDefault();
                        vp.RecordStatus = "Deleted";
                        vp.UpdatedBy = npsImp.UpdatedBy;
                        vp.UpdatedOn = DateTime.Now;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
    }
}