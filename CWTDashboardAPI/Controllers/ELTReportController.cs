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
        CWTEntities entity = new CWTEntities();
        Response re = new Response();
        EltResponce elt_re = new EltResponce();
        IMPSFilters fi = new IMPSFilters();
        String[] status, otherstatus;
        [HttpPost]
        [Route("CurrentMonthELT")]
        public EltResponce CurrentMonthELT(CLR clr)
        {
            PriorYear = DateTime.Now.AddYears(-1).Year.ToString();
            status = "C-Closed,A-Active/Date Confirmed".Split(',');
            otherstatus = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            string Month = DateTime.Now.ToString("MMM");
            string year = DateTime.Now.Year.ToString();
            var CurrentMonthELT = (from a in entity.CLRs
                                   where a.Go_Live_Month == "Sep"
                                   where a.Go_Live_Year == year
                                   where a.Backlog_Started == "Started"
                                   where status.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
                                   group a by a.Client into g
                                   select new
                                   {
                                       Client = g.Key,
                                       CurrentMonth = (from cm in g select cm.Revenue_Total_Volume_USD).Sum(),
                                       PriorMonthElt = (from cm in g select cm.Revenue_Total_Volume_USD).Sum(),
                                       Delta = 0,
                                       Status = "Started",
                                       //Comments = g.Where(cc => cc.Client == g.Key).Select(dd => new { UserName = g.Key, Role = string.Join(",", g.Select(ee => ee.iMeet_Milestone___Project_Status).ToList()) }),
                                       PreviousYear = entity.CLRs.Where(x1 => x1.Go_Live_Year == PriorYear).Where(x2 => x2.Backlog_Started == "Started").Where(x3 => status.Any(x3s => x3s.Equals(x3.iMeet_Milestone___Project_Status))).Where(x4 => x4.Client == g.Key).Sum(x5 => x5.Revenue_Total_Volume_USD),
                                       TotalAcountVolume = entity.CLRs.Where(v1 => v1.Client == g.Key).Where(v2 => otherstatus.Any(v2s => v2s.Equals(v2.iMeet_Milestone___Project_Status))).Sum(x5 => x5.Revenue_Total_Volume_USD),
                                   }).Distinct().OrderByDescending(x => x.CurrentMonth);

            elt_re.code = 200;
            elt_re.message = "Success";
            elt_re.ColumnOne = Month + " "+ year + " (Status:A/C)";
            elt_re.TotalAmountMonth1 = CurrentMonthELT.Sum(x => x.CurrentMonth);
            elt_re.Data = CurrentMonthELT.Take(25);
            elt_re.ColumnYearName = PriorYear + " Started";
            return elt_re;
        }
        string NM_Month, NM_Year;
        [HttpPost]
        [Route("NextMonthELT")]
        public EltResponce NextMonthELT(CLR clr)
        {
            PriorYear = DateTime.Now.AddYears(-1).Year.ToString();
            status = "C-Closed,A-Active/Date Confirmed".Split(',');
            otherstatus = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            if(DateTime.Now.Month == 12)
            {
                NM_Month = "Jan";
                NM_Year = DateTime.Now.AddYears(1).Year.ToString();
            }
            else
            {
                NM_Month = DateTime.Now.AddMonths(1).ToString("MMM");
                //NM_Month = "Sep";
                NM_Year = DateTime.Now.Year.ToString();
            }
            var NextMonthELT = (from a in entity.CLRs
                                   where a.Go_Live_Month == NM_Month
                                   where a.Go_Live_Year == NM_Year
                                   where status.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
                                   group a by a.Client into g
                                   select new
                                   {
                                       Client = g.Key,
                                       CurrentMonth = (from cm in g select cm.Revenue_Total_Volume_USD).Sum(),
                                       EltStatus = g.Where(x => x.Workspace__ELT_Overall_Status != null).Select(x => x.Workspace__ELT_Overall_Status).Distinct(),
                                       Comments = g.Where(x => x.Workspace__ELT_Overall_Comments != null).Select(x => x.Workspace__ELT_Overall_Comments).Distinct(),
                                       PreviousYear = entity.CLRs.Where(x => x.Go_Live_Year == PriorYear && x.Backlog_Started == "Started" && status.Any(xs => xs.Equals(x.iMeet_Milestone___Project_Status)) && x.Client == g.Key).Sum(x5 => x5.Revenue_Total_Volume_USD),
                                       TotalAcountVolume = entity.CLRs.Where(v1 => v1.Client == g.Key && otherstatus.Any(v2s => v2s.Equals(v1.iMeet_Milestone___Project_Status))).Sum(x5 => x5.Revenue_Total_Volume_USD),
                                   }).OrderByDescending(x => x.CurrentMonth);
            elt_re.code = 200;
            elt_re.message = "Success";
            elt_re.TotalAmountMonth1 = NextMonthELT.Sum(x => x.CurrentMonth);
            elt_re.ColumnYearName = PriorYear + " Started";
            elt_re.ColumnOne = NM_Month + " " + NM_Year + " (Status:A/C)";
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
            elt_re.Data = NextMonthELT.Take(25);
            return elt_re;
        }
        string[] months3,months4;
        string Month1, Month2;
        string Year1, Year2, Year3, PriorYear;
        [HttpPost]
        [Route("RestOfMonthsELT")]
        public EltResponce RestOfMonthsELT(CLR clr)
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
                    else
                    {
                        int j = 0;
                        for (int i = DateTime.Now.Month; i < 12; i++)
                        {
                            months3[j] = DateTime.Now.AddMonths(j + 4).ToString("MMM");
                            j++;
                        }
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
            var CurrentMonthELT = (from a in entity.CLRs
                                   where otherstatus.Any(val => a.iMeet_Milestone___Project_Status.Equals(val))
                                   where (a.Go_Live_Year == Year1 && a.Go_Live_Month == Month1) || (a.Go_Live_Year == Year2 && a.Go_Live_Month == Month2) || (a.Go_Live_Year == Year3 && months3.Any(x => a.Go_Live_Month.Equals(x)))
                                   group a by a.Client into g
                                   select new
                                   {
                                       Client = g.Key,
                                       Monthone = (from m1 in g
                                                   where m1.Go_Live_Month == Month1
                                                   where m1.Go_Live_Year == Year1
                                                   where status.Any(val1 => m1.iMeet_Milestone___Project_Status.Equals(val1))
                                                   select m1.Revenue_Total_Volume_USD).Sum(),
                                       Monthtwo = (from m2 in g
                                                   where m2.Go_Live_Month == Month2
                                                   where m2.Go_Live_Year == Year2
                                                   where status.Any(val1 => m2.iMeet_Milestone___Project_Status.Equals(val1))
                                                   select m2.Revenue_Total_Volume_USD).Sum(),
                                       //Status = "Started",
                                       Month1_N = (from m1_n in g
                                                   where m1_n.Go_Live_Month == Month1
                                                   where m1_n.Go_Live_Year == Year1
                                                   where m1_n.iMeet_Milestone___Project_Status == "N-Active/No Date Confirmed"
                                                   select m1_n.Revenue_Total_Volume_USD).Sum(),
                                       Month2_N = (from m2_n in g
                                                   where m2_n.Go_Live_Month == Month2
                                                   where m2_n.Go_Live_Year == Year2
                                                   where m2_n.iMeet_Milestone___Project_Status == "N-Active/No Date Confirmed"
                                                   select m2_n.Revenue_Total_Volume_USD).Sum(),

                                       RemainingTBC = g.Where(x1 => x1.Go_Live_Year == Year3 && months3.Any(x4s => x4s.Equals(x1.Go_Live_Month))).Sum(xs => xs.Revenue_Total_Volume_USD),
                                       TotalVolume = g.Sum(x => x.Revenue_Total_Volume_USD),
                                       EltStatus = g.Where(x => x.Workspace__ELT_Overall_Status != null).Select(x => x.Workspace__ELT_Overall_Status).Distinct(),
                                       Comments = g.Where(x => x.Workspace__ELT_Overall_Comments != null).Select(x => x.Workspace__ELT_Overall_Comments).Distinct(),
                                       PreviousYear = entity.CLRs.Where(x1 => x1.Go_Live_Year == PriorYear && x1.Backlog_Started == "Started" && status.Any(x1s => x1s.Equals(x1.iMeet_Milestone___Project_Status)) && x1.Client == g.Key).Sum(x5 => x5.Revenue_Total_Volume_USD),
                                       TotalAcountVolume = entity.CLRs.Where(v1 => v1.Client == g.Key).Where(v2 => otherstatus.Any(v2s => v2s.Equals(v2.iMeet_Milestone___Project_Status))).Sum(x5 => x5.Revenue_Total_Volume_USD),
                                   }).OrderByDescending(x=> x.TotalVolume);
            elt_re.code = 200;
            elt_re.message = "Success";
            elt_re.TotalAmountMonth1 = CurrentMonthELT.Sum(x => x.Monthone);
            elt_re.TotalAmountMonth2 = CurrentMonthELT.Sum(x => x.Monthtwo);
            elt_re.ColumnOne = Month1 + " " + Year1 + " (Status:A/C)";
            elt_re.ColumnTwo = Month2 + " " + Year2 + " (Status:A/C)";
            elt_re.ColumnThree = "Remaining " + Year3 + "/TBC (Status:A/C/N)";
            elt_re.ColumnYearName = PriorYear + " Started";
            elt_re.TotalAmountRemainingMonths = CurrentMonthELT.Sum(x=>x.Month1_N) + CurrentMonthELT.Sum(x2 => x2.Month2_N) + CurrentMonthELT.Sum(x => x.RemainingTBC);
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
                            }).Take(25).ToList();
            //elt_re.Data = CurrentMonthELT;
            return elt_re;
        }
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