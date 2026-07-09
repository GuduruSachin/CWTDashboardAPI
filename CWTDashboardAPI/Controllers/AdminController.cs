using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Net.Http;
using System.Web.Http;
using System.Globalization;
using CWTDashboardAPI.Models;
using System.Net.Mail;
using System.Net.Mime;

namespace CWTDashboardAPI.Controllers
{
    public class AdminController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        LoginUser LU = new LoginUser();
        // GET: Admin
        int count;
        [HttpPost]
        [Route("UsersUsageTracking")]
        public Response UsersUsageTracking(User user)
        {
            var UsageData = (from a in entity.Users
                             where a.UserStatus == "Active"
                             group a by a.UID into g
                             select new
                             { 
                                 UID = g.Key,
                                 UserName = entity.Users.FirstOrDefault(x => x.UID == g.Key).FirstName + " " + entity.Users.FirstOrDefault(x => x.UID == g.Key).LastName,
                                 Count = entity.UsersUsageofReports.Where(x => x.UID == g.Key).Sum(x => (double?)x.NoOfAttempts)?? 0,
                                 show = false,
                                 Reports = (from b in entity.UsersUsageofReports
                                            where b.UID == g.Key
                                            where b.NoOfAttempts > 0
                                            group b by b.ReportName into g_r
                                            select new
                                            {
                                                ReportName = g_r.Key,
                                                show = false,
                                                row = 1,
                                                //ReportName = entity.UsersUsageofReports.FirstOrDefault(x => x.UID == g.Key && x.ReportName == g_r.Key).ReportName,
                                                Count = entity.UsersUsageofReports.Where(x => x.UID == g.Key && x.ReportName == g_r.Key).Sum(x => x.NoOfAttempts),
                                                Types = (from c in entity.UsersUsageofReports
                                                         where c.UID == g.Key
                                                         where c.NoOfAttempts > 0
                                                         where c.ReportName == g_r.Key
                                                         group c by c.TypeofUse into g_t
                                                         select new
                                                         {
                                                             TypeofUse = g_t.Key,
                                                             show = false,
                                                             row = 1,
                                                             Count = entity.UsersUsageofReports.Where(x => x.UID == g.Key && x.ReportName == g_r.Key && x.TypeofUse == g_t.Key).Sum(x => (double?)x.NoOfAttempts) ?? 0,
                                                             LastUsedOn = entity.UsersUsageofReports.Where(x => x.UID == g.Key && x.ReportName == g_r.Key && x.TypeofUse == g_t.Key).Max(x => (DateTime?)x.LastUsedOn) ?? null
                                                         }),
                                                Audits = (from d in entity.AuditLogs
                                                         where d.UpdatedBy == g.Key
                                                         select new
                                                         {
                                                             RevenueID = d.RevenueID == 400000000000000 ? d.RevenueID + d.TaskRecordIDKey : d.RevenueID+"",
                                                             Field = d.Field,
                                                             OldValue = d.OldValue,
                                                             NewValue = d.NewValue,
                                                             PlatForm = d.UsedPlatForm,
                                                             show = false,
                                                             row = 1,
                                                             UpdatedOn = d.UpdatedOn
                                                         }),
                                                AuditCount = entity.AuditLogs.Where(x=>x.UpdatedBy == g.Key).Count(),
                                                LastUsedOn = entity.UsersUsageofReports.Where(x => x.UID == g.Key && x.ReportName == g_r.Key).Max(x => (DateTime?)x.LastUsedOn) ?? null
                                            }),
                                   LastUsedOn = entity.UsersUsageofReports.Where(x => x.UID == g.Key).Max(x => (DateTime?)x.LastUsedOn) ?? null
                             });
            count = UsageData.AsQueryable().Count();
            if (count.ToString() == "" || count.ToString() == null || count == 0)
            {
                re.code = 100;
                re.message = "No Data Found";
                re.Data = UsageData;
            }
            else
            {
                re.code = 200;
                re.message = "Success";
                re.Data = UsageData;
            }
            return re;
        }

        [HttpPost]
        [Route("GetReportsUpdatedON")]
        public Response GetReportsUpdatedON()
        {
            var today = DateTime.Today; // today at 00:00:00
            var tomorrow = today.AddDays(1); // next day at 00:00:00

            var dataCount = entity.ReportUpdatedOns
                             .Where(x => x.UpdatedOn >= today && x.UpdatedOn < tomorrow)
                             .Count();
            if (dataCount == 0)
            {
                re.message = "No reports have been uploaded yet";
                re.code = 100;
            }
            {
                var reports = entity.ReportUpdatedOns
                             .Where(x => x.UpdatedOn >= today && x.UpdatedOn < tomorrow)
                             .ToList();
                re.ReportsUpdatedON = reports;
                re.message = "Success";
                re.code = 200;
            }
            return re;
        }

        [HttpPost]
        [Route("SendReportsUpdatedSummary")]
        public Response SendReportsUpdatedSummary()
        {
            var today = DateTime.Today.AddDays(1); // today at 00:00:00
            var tomorrow = today.AddDays(1); // next day at 00:00:00

            var reports_count = entity.ReportUpdatedOns
                             .Where(x => x.UpdatedOn >= today && x.UpdatedOn < tomorrow)
                             .Count();
            
            string Body;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("Implementationsupport@mycwt.com");
            //mail.To.Add(new MailAddress("AChopra@mycwt.com"));
            //mail.CC.Add(new MailAddress("HGourani@mycwt.com"));
            //mail.CC.Add(new MailAddress("UGuduru@mycwt.com"));
            mail.To.Add(new MailAddress("UGuduru@mycwt.com"));
            //mail.CC.Add(new MailAddress("UGuduru@mycwt.com"));
            mail.IsBodyHtml = true;
            mail.Subject = "Automated Daily Report Import Summary";

            var reports = entity.ReportUpdatedOns
                             .Where(x => x.UpdatedOn >= today && x.UpdatedOn < tomorrow && x.ReportName != "CLRAutomated")
                             .ToList();

            var missingReports = new List<string> { "ProjectDueDateData", "CTOData", "iMeetData", "IMPSData", "RolesData", "AssignePersonsData"}
                     .Except(reports.Select(r => r.ReportName).Distinct())
                     .ToList();
            string rows = "";
            if (reports_count == 0)
            {
                rows = "<tr>" +
                            "<td colspan=\"4\" style=\"text-align:center;color : red; font-style:italic;\">No files have been uploaded, even though the report generation window has passed.</td>" +
                       "</tr>";
            }
            else
            {
                foreach (var report in reports)
                {
                    rows += $"<tr>" +
                                $"<td>{report.ReportName}</td>" +
                                $"<td>{report.UpdatedOn.ToString("yyyy-MM-dd hh:mm tt")}</td>" +
                                $"<td>{report.AvailableRows}</td>" +
                                $"<td>{report.UploadedRows}</td>" +
                            $"</tr>";
                }

                if (reports_count < 6)
                {
                    rows += $"<tr>" +
                                $"<td colspan=\"4\" style=\"text-align:center;color : red; font-style:italic;\">The following reports are not Uploaded: " + string.Join(", ", missingReports) + ".</td>" +
                            $"</tr>";
                }
            }
            
            Body = "<html><body style=\"padding:10px;\">" +
                    "<div>Hi Team,</div><br />" +
                    "<div>Please find below the summary of the daily report imports:</div><br />" +
                    "<table border =\"1\" cellpadding=\"6\" cellspacing=\"0\" style=\"border-collapse: collapse; font-family: Arial, sans-serif;\">" +
                        "<thead style =\"background-color: #f2f2f2;\">" +
                            "<tr>" +
                                "<th> Report Name </ th >" +
                                "<th> Imported On </ th >" +
                                "<th> Rows in Source </ th >" +
                                "<th> Rows Uploaded to DB </ th >" +
                            "</tr> " +
                        "</thead >" +
                        "<tbody >" +
                            rows +
                        "</tbody>" +
                    "</table><br />" +
                    "<div style=\"font-weight:bold;\">Notes:</div>" +
                    "<div>- Any mismatch between source and uploaded rows may indicate validation skips or errors.</div>" +
                    "<div>- Please contact the Automation team if you observe inconsistencies.</div><br />" +
                    "<div>Best regards,</div>" +
                    "<div>Implementation Team</div></body></html>";
            AlternateView av1 = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);
            mail.AlternateViews.Add(av1);
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mta-hub";
            smtp.UseDefaultCredentials = false;
            smtp.Port = 25;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            smtp.Send(mail);
            return re;
        }
    }
}