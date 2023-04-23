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
    }
}