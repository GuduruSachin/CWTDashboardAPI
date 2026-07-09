using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CWTDashboardAPI.Models;

namespace CWTDashboardAPI.Controllers
{
    public class ELTReportController : ApiController
    {
        //public ELTReportController(CWTDashboardEntities entities)
        //{

        //}
        //private readonly CWTDashboardEntities entities;
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        EltResponce elt_re = new EltResponce();
        IMPSFilters fi = new IMPSFilters();
        String[] status, otherstatus;
        List<string> Clients = new List<string>();
        [HttpPost]
        [Route("CurrentMonthELT")]
        public EltResponce CurrentMonthELT(CLRData clr)
        {
            PriorYear = DateTime.Now.AddYears(-1).Year.ToString();
            status = "C-Closed,A-Active/Date Confirmed".Split(',');
            otherstatus = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            string Month = DateTime.Now.ToString("MMM");
            string year = DateTime.Now.Year.ToString();
            var CurrentMonthELT = (from a in entity.CLRDatas
                                   where a.Status == "Active"
                                   where a.GoLiveMonth == Month
                                   where a.GoLiveYear == year
                                   //where a.OwnerShip != "Partner"
                                   //where a.Backlog_Started == "Started"
                                   where status.Any(val1 => a.ProjectStatus.Equals(val1))
                                   group a by a.Client into g
                                   select new
                                   {
                                       Client = g.Key,
                                       APAC = (from apac in g where apac.Region == "APAC" select apac.RevenueVolumeUSD).Sum() ?? 0,
                                       EMEA = (from emea in g where emea.Region == "EMEA" select emea.RevenueVolumeUSD).Sum() ?? 0,
                                       LATAM = (from latam in g where latam.Region == "LATAM" select latam.RevenueVolumeUSD).Sum() ?? 0,
                                       NORAM = (from noram in g where noram.Region == "NORAM" select noram.RevenueVolumeUSD).Sum() ?? 0,
                                       CurrentMonth = (from cm in g select cm.RevenueVolumeUSD).Sum() ?? 0,
                                       PriorMonthElt = entity.EltDeltaClients.Where(x => x.Client == g.Key && x.Month == Month && x.Year == year).Count() > 0 ? entity.EltDeltaClients.Where(x => x.Client == g.Key && x.Month == Month && x.Year == year).Sum(x =>x.Revenue) : 0,
                                       Delta = 0,
                                       Status = "Started",
                                       //EltStatus = g.Where(x => x.Workspace__ELT_Overall_Status != null).Select(x => x.Workspace__ELT_Overall_Status).Distinct(),
                                       //Comments = g.Where(x => x.Workspace__ELT_Overall_Comments != null).Select(x => x.Workspace__ELT_Overall_Comments).Distinct(),
                                       //Comments = g.Where(cc => cc.Client == g.Key).Select(dd => new { UserName = g.Key, Role = string.Join(",", g.Select(ee => ee.iMeet_Milestone___Project_Status).ToList()) }),
                                       //RegionComment = g.Select(x => x.Region).Distinct(),
                                       //RevenueComment = g.Select(x => x.RevenueVolumeUSD).Distinct(),
                                       Workspace = g.FirstOrDefault(x => x.Client == g.Key && x.Status == "Active").Workspace_Title,
                                       //RegionComment = g.Select(x => x.Region__Opportunity_).Distinct(),
                                       //RevenueComment = g.Select(x => x.Revenue_Total_Volume_USD).Distinct(),
                                       //PreviousYear = entity.CLRDatas.Where(x1 => x1.GoLiveYear == PriorYear).Where(x2 => x2.BacklogStarted == "Started").Where(x3 => status.Any(x3s => x3s.Equals(x3.ProjectStatus))).Where(x4 => x4.Client == g.Key).Where(x5 =>x5.Status == "Active").Sum(x5 => x5.RevenueVolumeUSD),
                                       TotalAcountVolume = entity.CLRDatas.Where(v1 => v1.Client == g.Key && v1.Status == "Active").Where(v2 => otherstatus.Any(v2s => v2s.Equals(v2.ProjectStatus))).Sum(x5 => x5.RevenueVolumeUSD) ?? 0,
                                   }).Where(x => x.CurrentMonth > 0).OrderByDescending(x => x.CurrentMonth).ToList();
            for(var i = 0; i < CurrentMonthELT.Count; i++)
            {
                Clients.Add(CurrentMonthELT[i].Client);
            }
            //var data = (from a in entity.EltDeltaClients
            //            where a.Month == Month
            //            where a.Year == year
            //            where a.Revenue > 0
            //            where !Clients.Any(val => a.Client.Equals(val))
            //            select new
            //            {
            //                Workspace = entity.CLRDatas.FirstOrDefault(x => x.Client == a.Client && x.Status == "Active").Workspace_Title,
            //                a.Client,
            //                a.Revenue,
            //                a.Month,
            //                a.Year,
            //                Comments = entity.CLRDatas.FirstOrDefault(x => x.Workspace__ELT_Overall_Comments != null && x.Status == "Active" && x.Client == a.Client).Workspace__ELT_Overall_Comments
            //            }).ToList();
            var clients = (from a in entity.EltDeltaClients
                           where a.Month == Month
                           where a.Year == year
                           where a.Revenue > 0
                           where !Clients.Any(val => a.Client.Equals(val))
                           select a.Client).ToList();
            var MovedClients = (from a in entity.EltDeltaClients
                                where a.Month == Month
                                where a.Year == year
                                where a.Revenue > 0
                                where !Clients.Any(val => a.Client.Equals(val))
                                group a by a.Client into g
                                select new {
                                    Client = g.Key,
                                    APAC = (double)0,
                                    EMEA = (double)0,
                                    LATAM = (double)0,
                                    NORAM = (double)0,
                                    CurrentMonth = (double)0,
                                    PriorMonthElt = (from elt in g select elt.Revenue).Sum(),
                                    Delta = 0,
                                    Status = "Started",
                                    Workspace = entity.CLRDatas.FirstOrDefault(x => x.Client == g.Key && x.Status == "Active").Workspace_Title,
                                    TotalAcountVolume = entity.CLRDatas.Where(x => x.Client == g.Key && x.Status == "Active").Sum(x => x.RevenueVolumeUSD) ?? 0,
                                });

            var ELTDeltaCLients = (from a in entity.EltDeltaClients
                                   where a.Month == Month
                                   where a.Year == year
                                   where a.Revenue > 0
                                   select a.Client).ToList();

            var DeltaComments = (from a in entity.EltOldCLRDatas
                                    where ELTDeltaCLients.Any(val => a.Client.Equals(val))
                                    where a.GoLiveMonth == Month
                                    where a.GoLiveYear == year
                                    where a.RevenueVolumeUSD > 0
                                    where a.Status == "Active"
                                    where status.Any(val1 => a.ProjectStatus.Equals(val1))
                                    join b in entity.CLRDatas on a.RevenueID equals b.RevenueID into ab
                                    from abc in ab.DefaultIfEmpty()
                                    select new ELTDeltaComments
                                    {
                                        Client = a.Client,
                                        RevenueID = a.RevenueID,
                                        ProjectStatus = a.ProjectStatus,
                                        GoLiveMonth = a.GoLiveMonth,
                                        GoLiveYear = a.GoLiveYear,
                                        Country = a.Country,
                                        Region = a.Region,
                                        Workspace_Title = a.Workspace_Title,
                                        PreviousVolume = a.RevenueVolumeUSD ?? 0,
                                        CurrentVolume = abc.RevenueVolumeUSD ?? 0,
                                        RevenueVolumeUSD = 0,
                                        CurrentProjectStatus = abc.ProjectStatus,
                                        CurrentMonth = abc.GoLiveMonth,
                                        CurrentYear = abc.GoLiveYear,
                                        Comments = "",
                                        DeltaColor = ""
                                    }).Distinct().ToList();
            var RevenueIds = (from a in DeltaComments
                          select a.RevenueID).ToList();
            var OtherDeltaComments = (from a in entity.CLRDatas
                                 where !RevenueIds.Any(val => a.RevenueID.Equals(val))
                                 where a.GoLiveMonth == Month
                                 where a.GoLiveYear == year
                                 where a.RevenueVolumeUSD > 0
                                 where a.Status == "Active"
                                 where status.Any(val1 => a.ProjectStatus.Equals(val1))
                                 join b in entity.EltOldCLRDatas on a.RevenueID equals b.RevenueID into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new ELTDeltaComments
                                 {
                                     Client = a.Client,
                                     RevenueID = a.RevenueID,
                                     ProjectStatus = abc.ProjectStatus,
                                     GoLiveMonth = abc.GoLiveMonth,
                                     GoLiveYear = abc.GoLiveYear,
                                     Country = a.Country,
                                     Region = a.Region,
                                     Workspace_Title = a.Workspace_Title,
                                     PreviousVolume = abc.RevenueVolumeUSD ?? 0,
                                     CurrentVolume = a.RevenueVolumeUSD ?? 0,
                                     RevenueVolumeUSD = 0,
                                     CurrentProjectStatus = a.ProjectStatus,
                                     CurrentMonth = a.GoLiveMonth,
                                     CurrentYear = a.GoLiveYear,
                                     Comments = "",
                                     DeltaColor = ""
                                 }).Distinct().ToList();
            var ELTDeltaComments = DeltaComments.Concat(OtherDeltaComments).ToList();
            //var ELTDeltaComments = DeltaComments.ToList();
            List<double> RemovableRecords = new List<double>();
            foreach (var r in ELTDeltaComments)
            {
                var comments = "";
                if(status.Any(val1 => r.CurrentProjectStatus.Equals(val1)))
                {
                    if (!status.Any(val1 => r.ProjectStatus.Equals(val1)))
                    {
                        r.DeltaColor = "green";
                        r.RevenueVolumeUSD = r.CurrentVolume;
                        comments = "Project Status Moved from " + r.ProjectStatus + " to " + r.CurrentProjectStatus;
                    }
                }
                else
                {
                    r.DeltaColor = "red";
                    r.RevenueVolumeUSD = r.PreviousVolume;
                    comments = "Project Status Moved from " + r.ProjectStatus + " to " + r.CurrentProjectStatus;
                }
                if (r.GoLiveMonth != r.CurrentMonth)
                {
                    if(r.DeltaColor == "red")
                    {
                        r.DeltaColor = "red";
                        r.RevenueVolumeUSD = r.PreviousVolume;
                    }
                    else
                    {
                        if(r.CurrentMonth == Month)
                        {
                            r.DeltaColor = "green";
                            r.RevenueVolumeUSD = r.CurrentVolume;
                        }
                        else
                        {
                            r.DeltaColor = "red";
                            r.RevenueVolumeUSD = r.PreviousVolume;
                        }
                    }
                    if (comments != "")
                    {
                        comments += "\n";
                    }
                    comments += "Golive Date has been changed from " + r.GoLiveMonth + "-"+ r.GoLiveYear + " to " + r.CurrentMonth + "-" + r.CurrentYear;
                }
                if (r.PreviousVolume != r.CurrentVolume)
                {
                    if(r.DeltaColor == "red")
                    {
                    }
                    else
                    {
                        if (r.PreviousVolume < r.CurrentVolume)
                        {
                            r.DeltaColor = "green";
                        }
                        else
                        {
                            r.DeltaColor = "red";
                        }
                    }
                    r.RevenueVolumeUSD = r.CurrentVolume - r.PreviousVolume;
                    if (comments != "")
                    {
                        comments += "\n";
                    }
                    comments += "Volume has been updated from " + r.PreviousVolume + " to " + r.CurrentVolume;
                }
                r.Comments = comments;
                if(comments == "")
                {
                    var count = ELTDeltaComments.Where(x => x.RevenueID == r.RevenueID).Count();
                    if(count > 1)
                    {
                        for (var k = 0; k < count; k++)
                        {
                            RemovableRecords.Add(r.RevenueID);
                        }
                    }
                    else
                    {
                        RemovableRecords.Add(r.RevenueID);
                    }
                }
            }
            for (int i = 0; i < RemovableRecords.Count; i++)
            {
                int index = ELTDeltaComments.FindIndex(a => a.RevenueID == RemovableRecords[i]);
                ELTDeltaComments.RemoveAt(index);
            }
            elt_re.code = 200;
            elt_re.message = "Success";
            elt_re.TotalAmountMonth1 = CurrentMonthELT.Concat(MovedClients).Sum(x => x.CurrentMonth);
            elt_re.TotalAmountPriorMonth1 = CurrentMonthELT.Concat(MovedClients).Sum(x => x.PriorMonthElt);
            elt_re.ColumnOne = Month + " " + year + " (Status:Active/Closed)";
            elt_re.ColumnYearName = PriorYear + " Started";
            elt_re.Data = CurrentMonthELT.Concat(MovedClients);
            elt_re.ELTDeltaComments = ELTDeltaComments.OrderBy(x => x.Client);
            elt_re.GrandTotal = entity.EltDeltaClients.Where(x => x.Month == Month && x.Year == year).Count() > 0 ? entity.EltDeltaClients.Where(x => x.Month == Month && x.Year == year)?.Sum(x => x.Revenue) : (double)0;
            return elt_re;
        }
        string NM_Month, NM_Year;
        [HttpPost]
        [Route("NextMonthELT")]
        public EltResponce NextMonthELT(CLRData clr)
        {
            PriorYear = DateTime.Now.AddYears(-1).Year.ToString();
            status = "C-Closed,A-Active/Date Confirmed".Split(',');
            otherstatus = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            if (DateTime.Now.Month == 12)
            {
                NM_Month = "Jan";
                NM_Year = DateTime.Now.AddYears(1).Year.ToString();
            }
            else
            {
                NM_Month = DateTime.Now.AddMonths(1).ToString("MMM");
                NM_Year = DateTime.Now.Year.ToString();
            }
            var NextMonthELT = (from a in entity.CLRDatas
                                where a.Status == "Active"
                                where a.GoLiveMonth == NM_Month
                                where a.GoLiveYear == NM_Year
                                //where a.OwnerShip != "Partner"
                                where status.Any(val1 => a.ProjectStatus.Equals(val1))
                                group a by a.Client into g
                                select new
                                {
                                    Client = g.Key,
                                    APAC = (from apac in g where apac.Region == "APAC" select apac.RevenueVolumeUSD).Sum(),
                                    EMEA = (from emea in g where emea.Region == "EMEA" select emea.RevenueVolumeUSD).Sum(),
                                    LATAM = (from latam in g where latam.Region == "LATAM" select latam.RevenueVolumeUSD).Sum(),
                                    NORAM = (from noram in g where noram.Region == "NORAM" select noram.RevenueVolumeUSD).Sum(),
                                    CurrentMonth = (from cm in g select cm.RevenueVolumeUSD).Sum(),
                                    EltStatus = g.Where(x => x.Workspace__ELT_Overall_Status != null).Select(x => x.Workspace__ELT_Overall_Status).Distinct(),
                                    Comments = g.Where(x => x.Workspace__ELT_Overall_Comments != null).Select(x => x.Workspace__ELT_Overall_Comments).Distinct(),
                                    RegionComment = g.Select(x => x.Region).Distinct(),
                                    RevenueComment = g.Select(x => x.RevenueVolumeUSD).Distinct(),
                                    PreviousYear = entity.CLRDatas.Where(x => x.GoLiveYear == PriorYear && status.Any(xs => xs.Equals(x.ProjectStatus)) && x.Client == g.Key && x.Status == "Active").Sum(x5 => x5.RevenueVolumeUSD),
                                    TotalAcountVolume = entity.CLRDatas.Where(v1 => v1.Client == g.Key && otherstatus.Any(v2s => v2s.Equals(v1.ProjectStatus))).Sum(x5 => x5.RevenueVolumeUSD),
                                    //&& v1.OwnerShip != "Partner"
                                }).OrderByDescending(x => x.CurrentMonth);
            elt_re.code = 200;
            elt_re.message = "Success";
            //elt_re.TotalAmountMonth1 = NextMonthELT.Sum(x => x.CurrentMonth);
            elt_re.ColumnYearName = PriorYear + " Started";
            elt_re.ColumnOne = NM_Month + " " + NM_Year + " (Status:Active/Closed)";
            //elt_re.Data = NextMonthELT.AsEnumerable()
            //    .Select(x => new NextMonth
            //    {
            //        Client = x.Client,
            //        CurrentMonth = x.CurrentMonth,
            //        PreviousYear = x.PreviousYear,
            //        EltStatus = string.Join(", ", x.EltStatus),
            //        Comments = string.Join(", ", x.Comments),
            //        TotalAcountVolume = x.TotalAcountVolume,
            //    }).Take(25).ToList();
            elt_re.Data = NextMonthELT.ToList();
            return elt_re;
        }
        string[] months3, months4;
        string Month1, Month2;
        string Year1, Year2, Year3, PriorYear;
        //int j;
        [HttpPost]
        [Route("RestOfMonthsELT")]
        public EltResponce RestOfMonthsELT(CLRData clr)
        {
            this.entity.Database.CommandTimeout = 10000;
            PriorYear = DateTime.Now.AddYears(-1).Year.ToString();
            if (DateTime.Now.Month < 11)
            {
                Month1 = DateTime.Now.AddMonths(2).ToString("MMM");
                Year1 = DateTime.Now.Year.ToString();
                if (DateTime.Now.Month == 10)
                {
                    months3 = new string[11] { "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    Year2 = DateTime.Now.AddYears(1).Year.ToString();
                    Year3 = DateTime.Now.AddYears(1).Year.ToString();
                    Month2 = "Jan";
                }
                else
                {
                    if (DateTime.Now.Month == 9)
                    {
                        months3 = new string[2] { "Nov", "Dec" };
                    }
                    else if (DateTime.Now.Month == 8)
                    {
                        months3 = new string[3] { "Oct", "Nov", "Dec" };
                    }
                    else if (DateTime.Now.Month == 7)
                    {
                        months3 = new string[4] { "Sep", "Oct", "Nov", "Dec" };
                    }
                    else if (DateTime.Now.Month == 6)
                    {
                        months3 = new string[5] { "Aug", "Sep", "Oct", "Nov", "Dec" };
                    }
                    else if (DateTime.Now.Month == 5)
                    {
                        months3 = new string[6] { "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    }
                    else if (DateTime.Now.Month == 4)
                    {
                        months3 = new string[7] { "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    }
                    else if (DateTime.Now.Month == 3)
                    {
                        months3 = new string[8] { "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    }
                    else if (DateTime.Now.Month == 2)
                    {
                        months3 = new string[9] { "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                    }
                    else if (DateTime.Now.Month == 1)
                    {
                        months3 = new string[10] { "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                        //j = 0;
                        //for (int i = DateTime.Now.Month; i < 12; i++)
                        //{
                        //    months3[j] = DateTime.Now.AddMonths(j + 4).ToString("MMM");
                        //    j = j + 1;
                        //}
                    }
                    Month2 = DateTime.Now.AddMonths(3).ToString("MMM");
                    Year2 = DateTime.Now.Year.ToString();
                    Year3 = DateTime.Now.Year.ToString();
                }
            }
            else if (DateTime.Now.Month == 11)
            {
                Month1 = "Jan";
                Month2 = "Feb";
                Year1 = DateTime.Now.AddYears(1).Year.ToString();
                Year2 = DateTime.Now.AddYears(1).Year.ToString();
                Year3 = DateTime.Now.AddYears(1).Year.ToString();
                months3 = new string[10] { "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                months4 = new string[12] { "Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            }
            else if (DateTime.Now.Month == 12)
            {
                Month1 = "Feb";
                Month2 = "Mar";
                Year1 = DateTime.Now.AddYears(1).Year.ToString();
                Year2 = DateTime.Now.AddYears(1).Year.ToString();
                Year3 = DateTime.Now.AddYears(1).Year.ToString();
                months3 = new string[9] { "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                months4 = new string[11] { "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
            }
            status = "C-Closed,A-Active/Date Confirmed".Split(',');
            otherstatus = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            string Month = DateTime.Now.AddMonths(1).ToString("MMM");
            float year = DateTime.Now.Year;
            var CurrentMonthELT = (from a in entity.CLRDatas
                                   where a.Status == "Active"
                                   where otherstatus.Any(val => a.ProjectStatus.Equals(val))
                                   //where a.OwnerShip != "Partner"
                                   where (a.GoLiveYear == Year1 && a.GoLiveMonth == Month1) || (a.GoLiveYear == Year2 && a.GoLiveMonth == Month2) || (a.GoLiveYear == Year3 && months3.Any(x => a.GoLiveMonth.Equals(x)))
                                   group a by a.Client into g
                                   select new
                                   {
                                       Client = g.Key,
                                       Monthone = (from m1 in g
                                                   where m1.GoLiveMonth == Month1
                                                   where m1.GoLiveYear == Year1
                                                   where status.Any(val1 => m1.ProjectStatus.Equals(val1))
                                                   select m1.RevenueVolumeUSD).Sum(),
                                       Monthtwo = (from m2 in g
                                                   where m2.GoLiveMonth == Month2
                                                   where m2.GoLiveYear == Year2
                                                   where status.Any(val1 => m2.ProjectStatus.Equals(val1))
                                                   select m2.RevenueVolumeUSD).Sum(),
                                       //Status = "Started",
                                       Month1_N = (from m1_n in g
                                                   where m1_n.GoLiveMonth == Month1
                                                   where m1_n.GoLiveYear == Year1
                                                   where m1_n.ProjectStatus == "N-Active/No Date Confirmed"
                                                   select m1_n.RevenueVolumeUSD).Sum(),
                                       Month2_N = (from m2_n in g
                                                   where m2_n.GoLiveMonth == Month2
                                                   where m2_n.GoLiveYear == Year2
                                                   where m2_n.ProjectStatus == "N-Active/No Date Confirmed"
                                                   select m2_n.RevenueVolumeUSD).Sum(),
                                       RemainingTBC = g.Where(x1 => x1.GoLiveYear == Year3 && months3.Any(x4s => x4s.Equals(x1.GoLiveMonth))).Sum(xs => xs.RevenueVolumeUSD) ?? 0,
                                       TotalVolume = g.Sum(x => x.RevenueVolumeUSD) ?? 0,
                                       EltStatus = g.Where(x => x.Workspace__ELT_Overall_Status != null).Select(x => x.Workspace__ELT_Overall_Status).Distinct(),
                                       Comments = g.Where(x => x.Workspace__ELT_Overall_Comments != null).Select(x => x.Workspace__ELT_Overall_Comments).Distinct(),
                                       RegionComment = g.Select(x => x.Region).Distinct(),
                                       RevenueComment = g.Select(x => x.RevenueVolumeUSD).Distinct(),
                                       PreviousYear = entity.CLRDatas.Where(x1 => x1.GoLiveYear == PriorYear && status.Any(x1s => x1s.Equals(x1.ProjectStatus)) && x1.Client == g.Key && x1.Status == "Active").Sum(x5 => x5.RevenueVolumeUSD),
                                       TotalAcountVolume = entity.CLRDatas.Where(v1 => v1.Client == g.Key && v1.Status == "Active").Where(v2 => otherstatus.Any(v2s => v2s.Equals(v2.ProjectStatus))).Sum(x5 => x5.RevenueVolumeUSD),
                                   }).OrderByDescending(x => x.TotalVolume);
            elt_re.code = 200;
            elt_re.message = "Success";
            //elt_re.TotalAmountMonth1 = CurrentMonthELT.Sum(x => x.Monthone);
            //elt_re.TotalAmountMonth2 = CurrentMonthELT.Sum(x => x.Monthtwo);
            elt_re.ColumnOne = Month1 + " " + Year1 + " (Status:Active/Closed)";
            elt_re.ColumnTwo = Month2 + " " + Year2 + " (Status:Active/Closed)";
            elt_re.ColumnThree = "Remaining " + Year3 + "/TBC (Status:Active/Closed/N-Active)";
            elt_re.ColumnYearName = PriorYear + " Started";
            //elt_re.TotalAmountRemainingMonths = CurrentMonthELT.Sum(x => x.Month1_N) + CurrentMonthELT.Sum(x2 => x2.Month2_N) + CurrentMonthELT.Sum(x => x.RemainingTBC);
            elt_re.GrandTotal = elt_re.TotalAmountMonth1 + elt_re.TotalAmountMonth2 + elt_re.TotalAmountRemainingMonths;
            elt_re.Data = CurrentMonthELT.AsEnumerable()
                            .Select(x => new RestOfMonths
                            {
                                Client = x.Client,
                                Month1 = x.Monthone,
                                Month2 = x.Monthtwo,
                                Month1_N = x.Month1_N,
                                Month2_N = x.Month2_N,
                                RemainingTBC = x.RemainingTBC,
                                TotalVolume = x.TotalVolume,
                                PreviousYear = x.PreviousYear,
                                EltStatus = string.Join(", ", x.EltStatus),
                                Comments = string.Join(", ", x.Comments),
                                TotalAcountVolume = x.TotalAcountVolume,
                                RegionComment = string.Join(", ", x.RegionComment),
                                RevenueComment = string.Join(", ", x.RevenueComment),
                            }).ToList();
            //elt_re.Data = CurrentMonthELT;
            return elt_re;
        }
        int DataCount,PriorDataCount;

        

        [HttpPost]
        [Route("PreviousMonthsEltYearMonth")]
        public EltResponce PreviousMonthsEltYearMonth(PreviousMonthsElt previousMonthsElt)
        {
            var monthyear = (from a in entity.PreviousMonthsElts
                             select new
                             {
                                 a.Month,
                                 a.Year,
                                 isSelected = true
                             }).Distinct().OrderByDescending(x => x.Year).ToList();
            DataCount = monthyear.AsQueryable().Count();
            if (DataCount.ToString() == "" || DataCount.ToString() == null || DataCount == 0)
            {
                elt_re.YearMonth = monthyear;
                elt_re.code = 100;
                elt_re.message = "No Data found";
            }
            else
            {
                elt_re.YearMonth = monthyear;
                elt_re.code = 200;
                elt_re.message = "Data Successfull";
            }
            return elt_re;
        }

        [HttpPost]
        [Route("SelectedPriorMonthYearData")]
        public EltResponce SelectedPriorMonthYearData(PreviousMonthsElt previousMonthsElt)
        {
            var PriorData = (from a in entity.PreviousMonthsElts
                        where a.Year == previousMonthsElt.Year
                        where a.Month == previousMonthsElt.Month
                        select new
                        {
                            a.Client,
                            a.EMEA,
                            a.APAC,
                            a.LATAM,
                            a.NORAM,
                            a.Total,
                            a.NBAPriorMonth,
                            a.TotalAccountVolume,
                            a.Delta,
                            a.Comments,
                            a.Month,
                            a.Year,
                            a.InsertedOn
                        }).OrderByDescending(x => x.Total);
            PriorDataCount = PriorData.AsQueryable().Count();
            var DeltaComments = (from a in entity.ELTDeltaComments
                                 where a.Month == previousMonthsElt.Month
                                 where a.Year == previousMonthsElt.Year
                                 where a.Status == "Active"
                                 select new
                                 {
                                     a.EltCommentsId,
                                     a.Client,
                                     a.RevenueId,
                                     a.Revenue,
                                     a.Month,
                                     a.Year,
                                     a.Country,
                                     a.Region,
                                     a.WorkspaceTitle,
                                     a.Comment,
                                     a.InsertedOn,
                                     a.Status,
                                     a.DeltaColor
                                 }).Distinct().ToList();
            if (PriorDataCount.ToString() == "" || PriorDataCount.ToString() == null || PriorDataCount == 0)
            {
                elt_re.Data = PriorData;
                elt_re.ELTDeltaComments = DeltaComments;
                elt_re.code = 100;
                elt_re.message = "No Data found";
            }
            else
            {
                elt_re.Data = PriorData;
                elt_re.ELTDeltaComments = DeltaComments;
                elt_re.code = 200;
                elt_re.message = "Data Successfull";
            }
            return elt_re;
        }
        //[HttpPost]
        //[Route("")]
        //public Response EltDeltaClient(EltDeltaClient eltDeltaClient)
        //{
        //    var
        //    return re;
        //}
    }
    public class RestOfMonths {
        public string Client { get; set; }
        public Nullable<double> Month1 { get; set; }
        public Nullable<double> Month2 { get; set; }
        public Nullable<double> Month1_N { get; set; }
        public Nullable<double> Month2_N { get; set; }
        public Nullable<double> RemainingTBC { get; set; }
        public Nullable<double> PreviousYear { get; set; }
        public Nullable<double> TotalVolume { get; set; }
        public string EltStatus { get; set; }
        public string Comments { get; set; }
        public Nullable<double> TotalAcountVolume { get; set; }
        public string RegionComment { get; set; }
        public string RevenueComment { get; set; }
    }
    public class NextMonth
    {
        public string Client { get; set; }
        public Nullable<double> CurrentMonth { get; set; }
        public Nullable<double> PreviousYear { get; set; }
        public string EltStatus { get; set; }
        public string Comments { get; set; }
        public Nullable<double> TotalAcountVolume { get; set; }
    }
}