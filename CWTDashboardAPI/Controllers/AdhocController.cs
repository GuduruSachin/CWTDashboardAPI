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
    public class AdhocController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        H_Filters h_f = new H_Filters();
        Response re = new Response();
        [HttpPost]
        [Route("AdhocInsert")]
        public IHttpActionResult AdhocInsert(AdHocProject adHocProject)
        {
            Boolean b = new Adhoc().AddingAdhoc(adHocProject);
            if (b)
            {
                adHocProject.InsertedOn = DateTime.Now;
                adHocProject.Status = "Active";
                entity.AdHocProjects.Add(adHocProject);
                ManualData manualdata = new ManualData();
                manualdata.Revenue_ID = adHocProject.RevenueID;
                manualdata.Client = adHocProject.Client;
                manualdata.GoLiveDate = adHocProject.GoLiveDate;
                manualdata.InsertedOn = DateTime.Now;
                manualdata.ProjectStatus = adHocProject.ProjectStatus;
                manualdata.Status = adHocProject.Status;
                manualdata.UpdateBy = adHocProject.InsertedBy;
                manualdata.GlobalCISDQSLead = adHocProject.GlobalDQSLead;
                manualdata.Pipeline_comments = adHocProject.Pipeline_Comments;
                manualdata.Priority = adHocProject.Priority;
                entity.ManualDatas.Add(manualdata);
                CLRData clrdata = new CLRData();
                clrdata.RevenueID = adHocProject.RevenueID ?? 0;
                clrdata.Region = adHocProject.Region;
                clrdata.Country = adHocProject.Country;
                clrdata.Client = adHocProject.Client;
                clrdata.GoLiveDate = adHocProject.GoLiveDate;
                clrdata.ProjectStart_ForCycleTime = adHocProject.StartDate;
                clrdata.ProjectStatus = adHocProject.ProjectStatus;
                clrdata.GlobalCISOBTLead = adHocProject.GlobalCISOBTLead;
                clrdata.RegionalCISOBTLead = adHocProject.RegionalCISOBTLead;
                clrdata.LocalDigitalOBTLead = adHocProject.LocalDigitalOBTLead;
                clrdata.GlobalCISPortraitLead = adHocProject.GlobalCISPortraitLead;
                clrdata.RegionalCISPortraitLead = adHocProject.RegionalCISPortraitLead;
                clrdata.GlobalCISHRFeedSpecialist = adHocProject.GlobalCISHRFeedSpecialist;
                clrdata.GlobalCISDQSLead = adHocProject.GlobalDQSLead;
                clrdata.AccountCategory = "N/A";
                clrdata.SOWStatus = "N/A";
                clrdata.ImplementationReady = "N/A";
                clrdata.DigitalActivityType = "Ad-hoc";
                clrdata.GDS = adHocProject.GDS;
                clrdata.Status = adHocProject.Status;
                clrdata.ComplexityScore = adHocProject.ComplexityScore;
                clrdata.GoLiveMonth = Convert.ToDateTime(adHocProject.GoLiveDate).ToString("MMM");
                clrdata.GoLiveYear = Convert.ToDateTime(adHocProject.GoLiveDate).Year.ToString();
                clrdata.Quarter = (Convert.ToDateTime(adHocProject.GoLiveDate).Month < 4 ? "Qtr 1"
                    : (Convert.ToDateTime(adHocProject.GoLiveDate).Month >= 4 && Convert.ToDateTime(adHocProject.GoLiveDate).Month < 7) ? "Qtr 2"
                    : (Convert.ToDateTime(adHocProject.GoLiveDate).Month >= 7 && Convert.ToDateTime(adHocProject.GoLiveDate).Month < 10) ? "Qtr 3"
                    : (Convert.ToDateTime(adHocProject.GoLiveDate).Month >= 10 && Convert.ToDateTime(adHocProject.GoLiveDate).Month <= 12) ? "Qtr 4"
                    : null);
                clrdata.DataSourceType = "Ad-Hoc Digital Data";
                entity.CLRDatas.Add(clrdata);
                DigitalTeam digitalteam = new DigitalTeam();
                digitalteam.RevenueID = adHocProject.RevenueID ?? 0;
                digitalteam.GlobalCISOBTLead = adHocProject.GlobalCISOBTLead;
                digitalteam.RegionalCISOBTLead = adHocProject.RegionalCISOBTLead;
                digitalteam.LocalDigitalOBTLead = adHocProject.LocalDigitalOBTLead;
                digitalteam.GlobalCISPortraitLead = adHocProject.GlobalCISPortraitLead;
                digitalteam.RegionalCISPortraitLead = adHocProject.RegionalCISPortraitLead;
                digitalteam.GlobalCISHRFeedSpecialist = adHocProject.GlobalCISHRFeedSpecialist;
                digitalteam.GDS = adHocProject.GDS;
                digitalteam.ComplexityScore = adHocProject.ComplexityScore ?? 0;
                digitalteam.ActivityType = adHocProject.ActivityType;
                digitalteam.Client = adHocProject.Client;
                digitalteam.TaskRecordIdKey = "";
                digitalteam.InsertedOn = DateTime.Now;
                digitalteam.UpdatedBy = adHocProject.InsertedBy;
                entity.DigitalTeams.Add(digitalteam);
                entity.SaveChanges();
                re.code = 200;
                re.message = "Added Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to add New";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("AdhocMaxRevenueID")]
        public Response AdhocMaxRevenueID(AdHocProject adHocProject)
        {
            var MaxRevenueID = entity.AdHocProjects.OrderByDescending(x => x.RevenueID).FirstOrDefault();
            re.code = 200;
            re.message = "Data Success";
            if(MaxRevenueID != null)
            {
                re.RevenueID = MaxRevenueID.RevenueID;
            }
            else
            {
                re.RevenueID = 600000000000000;
            }
            return re;
        }
        [HttpPost]
        [Route("AdhocUpdate")]
        public IHttpActionResult AdhocUpdate(AdHocProject adHocProject)
        {
            Boolean b = new Adhoc().UpdateAdhoc(adHocProject);
            if (b)
            {
                re.code = 200;
                re.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Update data";
                return Content(HttpStatusCode.OK, re);
            }
        }

        int AuditDataCount;
        [HttpPost]
        [Route("GetRecordLevelAuditLog")]
        public Response GetAuditLog(AuditLog auditlog)
        {
            var AuditLog = (from a in entity.AuditLogs
                            where a.Status == "Active"
                            where a.RevenueID == auditlog.RevenueID
                            select new
                            {
                                a.RevenueID,
                                a.TaskRecordIDKey,
                                //RevenueID = a.RevenueID == 400000000000000 ? a.RevenueID + " - " + a.TaskRecordIDKey : a.RevenueID + "",
                                OldValue = a.OldValue ?? "---",
                                Username = entity.Users.FirstOrDefault(x => x.UID == a.UpdatedBy).FirstName + " " + entity.Users.FirstOrDefault(x => x.UID == a.UpdatedBy).LastName ?? "---",
                                NewValue = a.NewValue ?? "---",
                                Field = a.Field ?? "---",
                                UsedPlatForm = a.UsedPlatForm ?? "---",
                                UpdatedBy = a.UpdatedBy ?? "---",
                                a.UpdatedOn
                            }).OrderBy(x => x.UpdatedOn).ToList();
            AuditDataCount = AuditLog.AsQueryable().Count();
            if (AuditDataCount.ToString() == "" || AuditDataCount.ToString() == null || AuditDataCount == 0)
            {
                re.Data = AuditLog;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                re.Data = AuditLog;
                re.code = 200;
                re.message = "Data Successfull";
            }
            return re;
        }

        [HttpPost]
        [Route("AdhocDelete")]
        public IHttpActionResult AdhocDelete(AdHocProject adHocProject)
        {
            Boolean b = new Adhoc().DeleteAdhoc(adHocProject);
            if (b)
            {
                re.code = 200;
                re.message = "Deleted Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Delete data";
                return Content(HttpStatusCode.OK, re);
            }
        }

    }
}
