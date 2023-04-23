using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CWTDashboardAPI.Models;

namespace CWTDashboardAPI.Controllers
{
    public class HomeScreenController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        HomeResponse h_re = new HomeResponse();
        Response re = new Response();
        IMPSFilters fi = new IMPSFilters();
        String Nm_Year, NM_Month, ROYear,ROMonth1,ROMonth2;
        string[] ROMonths,RoyMonths,PreMonths, RollingNPSMonths;
        [HttpPost]
        [Route("HomeData")]
        public HomeResponse HomeData(CLRData clr)
        {
            string Month = DateTime.Now.ToString("MMM");
            if (DateTime.Now.Month == 12)
            {
                NM_Month = "Jan";
                Nm_Year = DateTime.Now.AddYears(1).Year.ToString();
            }
            else
            {
                NM_Month = DateTime.Now.AddMonths(1).ToString("MMM");
                Nm_Year = DateTime.Now.Year.ToString();
            }
            if(DateTime.Now.Month == 11 || DateTime.Now.Month == 12)
            {
                if(DateTime.Now.Month == 11)
                {
                    ROMonth1 = "Jan";
                    ROMonth2 = "Feb";
                }
                else
                {
                    ROMonth1 = "Feb";
                    ROMonth2 = "Mar";
                }
                NM_Month = DateTime.Now.AddMonths(1).ToString("MMM");
                ROYear = DateTime.Now.AddYears(1).Year.ToString();
            }
            else
            {
                ROMonth1 = DateTime.Now.AddMonths(2).ToString("MMM");
                ROMonth2 = DateTime.Now.AddMonths(3).ToString("MMM");
                ROYear = DateTime.Now.Year.ToString();
                if (DateTime.Now.Month == 10)
                {
                    ROMonths = new string[11] { "Feb", "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                }
                else if (DateTime.Now.Month == 9)
                {
                    ROMonths = new string[2] { "Nov", "Dec" };
                }
                else if (DateTime.Now.Month == 8)
                {
                    ROMonths = new string[3] { "Oct", "Nov", "Dec" };
                }
                else if (DateTime.Now.Month == 7)
                {
                    ROMonths = new string[4] { "Sep", "Oct", "Nov", "Dec" };
                }
                else if (DateTime.Now.Month == 6)
                {
                    ROMonths = new string[5] { "Aug", "Sep", "Oct", "Nov", "Dec" };
                }
                else if (DateTime.Now.Month == 5)
                {
                    ROMonths = new string[6] { "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                }
                else if (DateTime.Now.Month == 4)
                {
                    ROMonths = new string[7] { "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                }
                else if (DateTime.Now.Month == 3)
                {
                    ROMonths = new string[8] { "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                }
                else if (DateTime.Now.Month == 2)
                {
                    ROMonths = new string[9] { "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                }
                else if (DateTime.Now.Month == 1)
                {
                    ROMonths = new string[10] { "Mar", "Apr", "May", "Jun", "Jul", "Aug", "Sep", "Oct", "Nov", "Dec" };
                }
            }
            var year = DateTime.Now.Year.ToString();
            var status = "C-Closed,A-Active/Date Confirmed".Split(',');
            var otherstatus = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            var difStatus = "N-Active/No Date Confirmed,P-Pipeline,H-Hold".Split(',');
            var TotalVolume = (from a in entity.CLRDatas
                               where a.GoLiveYear == year
                               where a.Status == "Active"
                               where status.Any(val1 => a.ProjectStatus.Equals(val1))
                               select a.RevenueVolumeUSD);
            var ActiveVolume = (from a in entity.CLRDatas
                               where a.GoLiveYear == year
                                where a.Status == "Active"
                                where a.ProjectStatus == "A-Active/Date Confirmed"
                                select a.RevenueVolumeUSD);
            var ClosedVolume = (from a in entity.CLRDatas
                                where a.GoLiveYear == year
                                where a.ProjectStatus == "C-Closed"
                                where a.Status == "Active"
                                select a.RevenueVolumeUSD);
            var DifVolume = (from a in entity.CLRDatas
                                where a.GoLiveYear == year
                                where a.Status == "Active"
                                where difStatus.Any(val1 => a.ProjectStatus.Equals(val1))
                                select a.RevenueVolumeUSD);
            var PipelineVolume = (from a in entity.CLRDatas
                                  where a.Status == "Active"
                                  where a.ProjectStatus == "P-Pipeline"
                               select a.RevenueVolumeUSD).Sum();
            var CurrentMonth = (from a in entity.CLRDatas
                                where a.GoLiveMonth == Month
                                where a.GoLiveYear == year
                                where a.Status == "Active"
                                where status.Any(val1 => a.ProjectStatus.Equals(val1))
                                group a by a.Workspace_Title into g
                                select new
                                {
                                    RevenueTotal = g.Sum(x => x.RevenueVolumeUSD)
                                }).OrderByDescending(x =>x.RevenueTotal);
            var NextMonth = (from a in entity.CLRDatas
                                where a.GoLiveMonth == NM_Month
                                where a.GoLiveYear == Nm_Year
                                where a.Status == "Active"
                                where status.Any(val1 => a.ProjectStatus.Equals(val1))
                                group a by a.Workspace_Title into g
                                select new
                                {
                                    RevenueTotal = g.Sum(x => x.RevenueVolumeUSD)
                                }).OrderByDescending(x => x.RevenueTotal);
            //var RestoftheYear = (from a in entity.CLRs
            //                     where a.Go_Live_Year == ROYear)
            h_re.code = 200;
            h_re.message = "Success";
            h_re.TotalVolume = TotalVolume.Sum()+"";
            h_re.ActiveVolume = ActiveVolume.Sum()+"";
            h_re.ClosedVolume = ClosedVolume.Sum()+"";
            h_re.P_NDC_H_Volume = DifVolume.Sum()+"";
            h_re.Projects = TotalVolume.Count()+"";
            h_re.PipelineVolume = PipelineVolume+"";
            h_re.CurrentMonth = CurrentMonth.Select(x => x.RevenueTotal).Take(25).Sum()+"";
            h_re.NextMonth = NextMonth.Select(x=>x.RevenueTotal).Take(25).Sum()+"";
            return h_re;
        }
        
        string rollingmonths;
        [HttpPost]
        [Route("HomeDetailsData")]
        public HomeResponse HomeDetailsData(CLRData clr)
        {
            var Month = DateTime.Now.ToString("MMM");
            if (DateTime.Now.Month == 12)
            {
                NM_Month = "---";
            }
            else
            {
                NM_Month = DateTime.Now.AddMonths(1).ToString("MMM");
            }
            if(DateTime.Now.Month == 12 || DateTime.Now.Month == 11)
            {
                if(DateTime.Now.Month == 12)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov".Split(',');
                }
                else
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct".Split(',');
                }
                RoyMonths = "---".Split(',');
            }
            else
            {
                if (DateTime.Now.Month == 10)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep".Split(',');
                    RoyMonths = "Dec".Split(',');
                }
                else if (DateTime.Now.Month == 9)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug".Split(',');
                    RoyMonths = "Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 8)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul".Split(',');
                    RoyMonths = "Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 7)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun".Split(',');
                    RoyMonths = "Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 6)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May".Split(',');
                    RoyMonths = "Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 5)
                {
                    PreMonths = "Jan,Feb,Mar,Apr".Split(',');
                    RoyMonths = "Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 4)
                {
                    PreMonths = "Jan,Feb,Mar".Split(',');
                    RoyMonths = "Jun,Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 3)
                {
                    PreMonths = "Jan,Feb".Split(',');
                    RoyMonths = "May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 2)
                {
                    PreMonths = "Jan".Split(',');
                    RoyMonths = "Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 1)
                {
                    PreMonths = "---".Split(',');
                    RoyMonths = "Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
            }
            var CurrentYear = DateTime.Now.Year.ToString();
            int year = DateTime.Now.Year;
            DateTime Nextyear = new DateTime(year+1, 1, 1);
            var CurrentStatus = "C-Closed,A-Active/Date Confirmed".Split(',');
            var ROYstatus = "A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            var regions = clr.Region.Split(',');
            var PreviousMonths = (from a in entity.CLRDatas
                                where PreMonths.Any(val1 => a.GoLiveMonth.Equals(val1))
                                where a.GoLiveYear == CurrentYear
                                where a.Status == "Active"
                                where a.OwnerShip != "Partner"
                                  where a.RevenueID < 600000000000000
                                  where regions.Any(val => a.Region.Equals(val))
                                where CurrentStatus.Any(val1 => a.ProjectStatus.Equals(val1))
                                select a.RevenueVolumeUSD).ToList();
            var CurrentMonth = (from a in entity.CLRDatas
                                where a.GoLiveMonth == Month
                                where a.GoLiveYear == CurrentYear
                                where a.Status == "Active"
                                where a.OwnerShip != "Partner"
                                where regions.Any(val => a.Region.Equals(val))
                                where a.RevenueID < 600000000000000
                                where CurrentStatus.Any(val1 => a.ProjectStatus.Equals(val1))
                                select a.RevenueVolumeUSD).ToList();
            var NextMonth = (from a in entity.CLRDatas
                                where a.GoLiveMonth == NM_Month
                                where a.GoLiveYear == CurrentYear
                                where a.Status == "Active"
                                where a.OwnerShip != "Partner"
                                where regions.Any(val => a.Region.Equals(val))
                             where a.RevenueID < 600000000000000
                             where CurrentStatus.Any(val1 => a.ProjectStatus.Equals(val1))
                                select a.RevenueVolumeUSD).ToList();
            var RemainingMonths = (from a in entity.CLRDatas
                                   where RoyMonths.Any(val1 => a.GoLiveMonth.Equals(val1))
                                   where a.GoLiveYear == CurrentYear
                                   where a.Status == "Active"
                                   where a.OwnerShip != "Partner"
                                   where regions.Any(val => a.Region.Equals(val))
                                   where ROYstatus.Any(val1 => a.ProjectStatus.Equals(val1))
                                   where a.RevenueID < 600000000000000
                                   select a.RevenueVolumeUSD).ToList();
            var FutureYears = (from a in entity.CLRDatas
                               where a.GoLiveDate >= Nextyear
                               where a.Status == "Active"
                               where a.OwnerShip != "Partner"
                               where regions.Any(val => a.Region.Equals(val))
                               where a.RevenueID < 600000000000000
                               where ROYstatus.Any(val1 => a.ProjectStatus.Equals(val1))
                               select a.RevenueVolumeUSD);
            var Hold = (from a in entity.CLRDatas
                                where a.Status == "Active"
                                where a.OwnerShip != "Partner"
                                where regions.Any(val => a.Region.Equals(val))
                        where a.RevenueID < 600000000000000
                        where a.ProjectStatus == "H-Hold"
                                select a.RevenueVolumeUSD);
            var Pipeline = (from a in entity.CLRDatas
                            where a.Status == "Active"
                            where a.OwnerShip != "Partner"
                            where regions.Any(val => a.Region.Equals(val))
                            where a.RevenueID < 600000000000000
                            where a.ProjectStatus == "P-Pipeline"
                            select a.RevenueVolumeUSD);
            var HighPotential = (from a in entity.CLRDatas
                                where a.Status == "Active"
                                where a.OwnerShip != "Partner"
                                where regions.Any(val => a.Region.Equals(val))
                                where a.ProjectStatus == "HP-High Potential"
                                 where a.RevenueID < 600000000000000
                                 select a.RevenueVolumeUSD);
            var ExistingServiceCT = (from a in entity.CLRDatas
                                     where regions.Any(val => a.Region.Equals(val))
                                     where a.Status == "Active"
                                     where a.CycleTimeCategories == "Existing Service Config Change (catch all including Spins/Mergers)"
                                     where a.OwnerShip == "WO"
                                     where a.ProjectStatus == "C-Closed"
                                     where a.GoLiveYear == CurrentYear
                                     where a.ProjectStart_ForCycleTime != null
                                     select a.CycleTime).ToList();
            var ExistingAddChangeCT = (from a in entity.CLRDatas
                                       where regions.Any(val => a.Region.Equals(val))
                                       where a.Status == "Active"
                                       where a.CycleTimeCategories == "Existing Add/Change OBT"
                                       where a.OwnerShip == "WO"
                                       where a.ProjectStatus == "C-Closed"
                                       where a.GoLiveYear == CurrentYear
                                       where a.ProjectStart_ForCycleTime != null
                                       select a.CycleTime).ToList();
            var NewLocalCT = (from a in entity.CLRDatas
                              where regions.Any(val => a.Region.Equals(val))
                              where a.Status == "Active"
                              where a.CycleTimeCategories == "New Local Including Upsell"
                              where a.OwnerShip == "WO"
                              where a.ProjectStatus == "C-Closed"
                              where a.GoLiveYear == CurrentYear
                              where a.ProjectStart_ForCycleTime != null
                              select a.CycleTime).ToList();
            var NewGlobalCT = (from a in entity.CLRDatas
                               where regions.Any(val => a.Region.Equals(val))
                               where a.Status == "Active"
                               where a.CycleTimeCategories == "New Global Including Upsell"
                               where a.OwnerShip == "WO"
                               where a.ProjectStatus == "C-Closed"
                               where a.GoLiveYear == CurrentYear
                               where a.ProjectStart_ForCycleTime != null
                               select a.CycleTime).ToList();
            var OverallCT = (from a in entity.CLRDatas
                             where regions.Any(val => a.Region.Equals(val))
                             where a.Status == "Active"
                             where a.CycleTimeCategories != "0"
                             where a.CycleTimeCategories != null
                             where a.CycleTimeCategories != "New Global/regional/local"
                             where a.OwnerShip == "WO"
                             where a.ProjectStatus == "C-Closed"
                             where a.GoLiveYear == CurrentYear
                             where a.ProjectStart_ForCycleTime != null
                             select a.CycleTime).ToList();
            var CreationDate = "01-01-2020";
            var ownership = "WO,JV".Split(',');
            DateTime ConvertedDate = Convert.ToDateTime(CreationDate);
            var Country = (from a in entity.CLRDatas
                           where a.Status == "Active"
                           where ownership.Any(val => a.OwnerShip.Equals(val))
                           where regions.Any(val => a.Region.Equals(val))
                           where a.GoLiveDate >= ConvertedDate
                           select new
                           {
                               Country = a.Country == null || a.Country == "" ? "---" : a.Country ?? "---",
                               isSelected = true,
                           }).Distinct().OrderBy(x => x.Country);
            int NPSCurrentYear = DateTime.Now.Year;
            var ClientTypes = "New,Existing".Split(',');
            var NpsData = (from a in entity.NpsImps
                           //where a.NpsId == 1
                              select new {
                                  NESurveySent = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateServeySent.Value.Year == NPSCurrentYear && x.RecordStatus != "Deleted").Count(),
                                  NESurveyReceived = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateSurveyReceived.Value.Year == NPSCurrentYear && x.RecordStatus != "Deleted").Count(),
                                  NEPromoter = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateSurveyReceived.Value.Year == NPSCurrentYear && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                                  NEPassive = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateSurveyReceived.Value.Year == NPSCurrentYear && x.NPSIndicator == "Passive" && x.RecordStatus != "Deleted").Count(),
                                  NEDetractor = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateSurveyReceived.Value.Year == NPSCurrentYear && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count(),
                              }).Distinct().ToList();
            int NPS_Current_Month = DateTime.Now.Month;
            for (int i = 1; i <= 12; i++)
            {
                if (i > NPS_Current_Month)
                {
                    if (i == 12)
                    {
                        rollingmonths += DateTime.Now.AddYears(-1).Year + "-" + i;
                    }
                    else if (i < 10)
                    {
                        rollingmonths += DateTime.Now.AddYears(-1).Year + "-0" + i + ",";
                    }
                    else
                    {
                        rollingmonths += DateTime.Now.AddYears(-1).Year + "-" + i + ",";
                    }
                }
                else
                {
                    if (i < 10)
                    {
                        rollingmonths += DateTime.Now.Year + "-0" + i + ",";
                    }
                    else
                    {
                        rollingmonths += DateTime.Now.Year + "-" + i + ",";
                    }
                }
            }
            RollingNPSMonths = rollingmonths.Split(',');
            var RollingNPS = (from a in entity.NpsImps
                        where a.RecordStatus != "Deleted"
                        where RollingNPSMonths.Any(val => a.YearMonth.Equals(val))
                        where a.NPSIndicator != null
                        where a.NPSIndicator != "null"
                        where a.NPSIndicator != ""
                        where ClientTypes.Any(val => a.ClientType.Equals(val))
                        where regions.Any(val => a.Region.Equals(val))
                        select a).ToList();
            var RollingNPSData = (from a in RollingNPS
                                  group a by a.YearMonth into g
                            select new
                            {
                                YearMonth = g.Key,
                                NESurveyReceived = RollingNPS.Where(x => x.YearMonth == g.Key).Count(),
                                NEPromoter = RollingNPS.Where(x => x.YearMonth == g.Key && x.NPSIndicator == "Promoter").Count(),
                                NEDetractor = RollingNPS.Where(x => x.YearMonth == g.Key && x.NPSIndicator == "Detractor").Count(),
                                NPSScore = 0
                            }).OrderBy(x => x.YearMonth).ToList();
            h_re.code = 200;
            h_re.message = "Success";
            h_re.PreMonthVolume = PreviousMonths.Sum() + "";
            h_re.PreMonthRecords = PreviousMonths.Count() + "";
            h_re.CurrentMonthVolume = CurrentMonth.Sum()+"";
            h_re.CurrentMonthRecords = CurrentMonth.Count()+"";
            h_re.NextMonthVolume = NextMonth.Sum() + "";
            h_re.NextMonthRecords = NextMonth.Count() + "";
            h_re.RoyMonthVolume = RemainingMonths.Sum() + "";
            h_re.RoyMonthRecords = RemainingMonths.Count() + "";
            h_re.ExpectedCurrentMonthVolume = PreviousMonths.Sum() + CurrentMonth.Sum() + NextMonth.Sum() + RemainingMonths.Sum() + "";
            h_re.ExpectedCurrentMonthRecords = PreviousMonths.Count() + CurrentMonth.Count() + NextMonth.Count() + RemainingMonths.Count() + "";
            h_re.FutureYearsVolume = FutureYears.Sum() + "";
            h_re.FutureYearsRecords = FutureYears.Count() + "";
            h_re.HoldVolume = Hold.Sum() + "";
            h_re.HoldRecords = Hold.Count() + "";
            h_re.PipelineVolume = Pipeline.Sum() + "";
            h_re.PipelineRecords = Pipeline.Count() + "";
            h_re.HighPotentialVolume = HighPotential.Sum() + "";
            h_re.HighPotentialRecords = HighPotential.Count() + "";
            h_re.PotentialVolume = FutureYears.Sum()+Hold.Sum()+Pipeline.Sum()+HighPotential.Sum()+"";
            h_re.PotentialRecords = FutureYears.Count()+Hold.Count()+Pipeline.Count()+HighPotential.Count()+"";
            h_re.Countries = Country;
            h_re.NpsData = NpsData;
            h_re.RollingNpsData = RollingNPSData;
            h_re.ExistingAddChangeCycleTime = ExistingAddChangeCT.Sum() + "";
            h_re.ExistingAddChangeProjectCount = ExistingAddChangeCT.Count() + "";
            h_re.ExistingServiceCycleTime = ExistingServiceCT.Sum() + "";
            h_re.ExistingServiceProjectCount = ExistingServiceCT.Count() + "";
            h_re.NewGlobalCycleTime = NewGlobalCT.Sum() + "";
            h_re.NewGlobalProjectCount = NewGlobalCT.Count() + "";
            h_re.NewLocalCycleTime = NewLocalCT.Sum() + "";
            h_re.NewLocalProjectCount = NewLocalCT.Count() + "";
            h_re.OverallCycleTime = OverallCT.Sum() + "";
            h_re.OverallProjectCount = OverallCT.Count() + "";
            return h_re;
        }

        [HttpPost]
        [Route("HomeDetailsDataWithCountry")]
        public HomeResponse HomeDetailsDataWithCountry(CLRData clr)
        {
            var Month = DateTime.Now.ToString("MMM");
            if (DateTime.Now.Month == 12)
            {
                NM_Month = "---";
            }
            else
            {
                NM_Month = DateTime.Now.AddMonths(1).ToString("MMM");
            }
            if (DateTime.Now.Month == 12 || DateTime.Now.Month == 11)
            {
                if (DateTime.Now.Month == 12)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov".Split(',');
                }
                else
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep,Oct".Split(',');
                }
                RoyMonths = "---".Split(',');
            }
            else
            {
                if (DateTime.Now.Month == 10)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug,Sep".Split(',');
                    RoyMonths = "Dec".Split(',');
                }
                else if (DateTime.Now.Month == 9)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul,Aug".Split(',');
                    RoyMonths = "Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 8)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun,Jul".Split(',');
                    RoyMonths = "Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 7)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May,Jun".Split(',');
                    RoyMonths = "Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 6)
                {
                    PreMonths = "Jan,Feb,Mar,Apr,May".Split(',');
                    RoyMonths = "Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 5)
                {
                    PreMonths = "Jan,Feb,Mar,Apr".Split(',');
                    RoyMonths = "Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 4)
                {
                    PreMonths = "Jan,Feb,Mar".Split(',');
                    RoyMonths = "Jun,Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 3)
                {
                    PreMonths = "Jan,Feb".Split(',');
                    RoyMonths = "May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 2)
                {
                    PreMonths = "Jan".Split(',');
                    RoyMonths = "Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
                else if (DateTime.Now.Month == 1)
                {
                    PreMonths = "---".Split(',');
                    RoyMonths = "Mar,Apr,May,Jun,Jul,Aug,Sep,Oct,Nov,Dec".Split(',');
                }
            }
            var CurrentYear = DateTime.Now.Year.ToString();
            int year = DateTime.Now.Year;
            DateTime Nextyear = new DateTime(year + 1, 1, 1);
            var CurrentStatus = "C-Closed,A-Active/Date Confirmed".Split(',');
            var ROYstatus = "A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            var regions = clr.Region.Split(',');
            var Countries = clr.Country.Split(',');
            var PreviousMonths = (from a in entity.CLRDatas
                                  where PreMonths.Any(val1 => a.GoLiveMonth.Equals(val1))
                                  where a.GoLiveYear == CurrentYear
                                  where a.Status == "Active"
                                  where regions.Any(val => a.Region.Equals(val))
                                  where Countries.Any(val => a.Country.Equals(val))
                                  where CurrentStatus.Any(val1 => a.ProjectStatus.Equals(val1))
                                  select a.RevenueVolumeUSD).ToList();
            var CurrentMonth = (from a in entity.CLRDatas
                                where a.GoLiveMonth == Month
                                where a.GoLiveYear == CurrentYear
                                where a.Status == "Active"
                                where regions.Any(val => a.Region.Equals(val))
                                where Countries.Any(val => a.Country.Equals(val))
                                where CurrentStatus.Any(val1 => a.ProjectStatus.Equals(val1))
                                select a.RevenueVolumeUSD).ToList();
            var NextMonth = (from a in entity.CLRDatas
                             where a.GoLiveMonth == NM_Month
                             where a.GoLiveYear == CurrentYear
                             where a.Status == "Active"
                             where regions.Any(val => a.Region.Equals(val))
                             where Countries.Any(val => a.Country.Equals(val))
                             where CurrentStatus.Any(val1 => a.ProjectStatus.Equals(val1))
                             select a.RevenueVolumeUSD).ToList();
            var RemainingMonths = (from a in entity.CLRDatas
                                   where RoyMonths.Any(val1 => a.GoLiveMonth.Equals(val1))
                                   where a.GoLiveYear == CurrentYear
                                   where a.Status == "Active"
                                   where regions.Any(val => a.Region.Equals(val))
                                   where Countries.Any(val => a.Country.Equals(val))
                                   where ROYstatus.Any(val1 => a.ProjectStatus.Equals(val1))
                                   select a.RevenueVolumeUSD).ToList();
            var FutureYears = (from a in entity.CLRDatas
                               where a.GoLiveDate >= Nextyear
                               where a.Status == "Active"
                               where regions.Any(val => a.Region.Equals(val))
                               where Countries.Any(val => a.Country.Equals(val))
                               where ROYstatus.Any(val1 => a.ProjectStatus.Equals(val1))
                               select a.RevenueVolumeUSD).ToList();
            var Hold = (from a in entity.CLRDatas
                        where a.Status == "Active"
                        where regions.Any(val => a.Region.Equals(val))
                        where Countries.Any(val => a.Country.Equals(val))
                        where a.ProjectStatus == "H-Hold"
                        select a.RevenueVolumeUSD).ToList();
            var Pipeline = (from a in entity.CLRDatas
                            where a.Status == "Active"
                            where regions.Any(val => a.Region.Equals(val))
                            where Countries.Any(val => a.Country.Equals(val))
                            where a.ProjectStatus == "P-Pipeline"
                            select a.RevenueVolumeUSD).ToList();
            var HighPotential = (from a in entity.CLRDatas
                                 where a.Status == "Active"
                                 where regions.Any(val => a.Region.Equals(val))
                                 where Countries.Any(val => a.Country.Equals(val))
                                 where a.ProjectStatus == "HP-High Potential"
                                 select a.RevenueVolumeUSD).ToList();
            var ExistingServiceCT = (from a in entity.CLRDatas
                                     where a.Status == "Active"
                                     where a.CycleTimeCategories == "Existing Service Config Change (catch all including Spins/Mergers)"
                                     where a.OwnerShip == "WO"
                                     where a.ProjectStatus == "C-Closed"
                                     where a.GoLiveYear == CurrentYear
                                     where regions.Any(val => a.Region.Equals(val))
                                     where Countries.Any(val => a.Country.Equals(val))
                                     where a.ProjectStart_ForCycleTime != null
                                     select a.CycleTime).ToList();
            var ExistingAddChangeCT = (from a in entity.CLRDatas
                                       where a.Status == "Active"
                                       where a.CycleTimeCategories == "Existing Add/Change OBT"
                                       where a.OwnerShip == "WO"
                                       where a.ProjectStatus == "C-Closed"
                                       where a.GoLiveYear == CurrentYear
                                       where regions.Any(val => a.Region.Equals(val))
                                       where Countries.Any(val => a.Country.Equals(val))
                                       where a.ProjectStart_ForCycleTime != null
                                       select a.CycleTime).ToList();
            var NewLocalCT = (from a in entity.CLRDatas
                              where a.Status == "Active"
                              where a.CycleTimeCategories == "New Local Including Upsell"
                              where a.OwnerShip == "WO"
                              where a.ProjectStatus == "C-Closed"
                              where a.GoLiveYear == CurrentYear
                              where regions.Any(val => a.Region.Equals(val))
                              where Countries.Any(val => a.Country.Equals(val))
                              where a.ProjectStart_ForCycleTime != null
                              select a.CycleTime).ToList();
            var NewGlobalCT = (from a in entity.CLRDatas
                               where a.Status == "Active"
                               where a.CycleTimeCategories == "New Global Including Upsell"
                               where a.OwnerShip == "WO"
                               where a.ProjectStatus == "C-Closed"
                               where a.GoLiveYear == CurrentYear
                               where regions.Any(val => a.Region.Equals(val))
                               where Countries.Any(val => a.Country.Equals(val))
                               where a.ProjectStart_ForCycleTime != null
                               select a.CycleTime).ToList();
            var OverallCT = (from a in entity.CLRDatas
                             where a.Status == "Active"
                             where a.CycleTimeCategories != "0"
                             where a.CycleTimeCategories != null
                             where a.CycleTimeCategories != "New Global/regional/local"
                             where a.OwnerShip == "WO"
                             where a.ProjectStatus == "C-Closed"
                             where a.GoLiveYear == CurrentYear
                             where regions.Any(val => a.Region.Equals(val))
                             where Countries.Any(val => a.Country.Equals(val))
                             where a.ProjectStart_ForCycleTime != null
                             select a.CycleTime).ToList();
            var CreationDate = "01-01-2020";
            var ownership = "WO,JV".Split(',');
            DateTime ConvertedDate = Convert.ToDateTime(CreationDate);
            var Country = (from a in entity.CLRDatas
                           where a.Status == "Active"
                           where ownership.Any(val => a.OwnerShip.Equals(val))
                           where regions.Any(val => a.Region.Equals(val))
                           where a.GoLiveDate >= ConvertedDate
                           select new
                           {
                               Country = a.Country == null || a.Country == "" ? "---" : a.Country ?? "---",
                               isSelected = true,
                           }).Distinct().OrderBy(x => x.Country);

            int NPSCurrentYear = DateTime.Now.Year;
            var ClientTypes = "New,Existing".Split(',');
            var NpsData = (from a in entity.NpsImps
                           select new
                           {
                               NESurveySent = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && Countries.Any(val => x.Country.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateServeySent.Value.Year == NPSCurrentYear && x.RecordStatus != "Deleted").Count(),
                               NESurveyReceived = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && Countries.Any(val => x.Country.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateSurveyReceived.Value.Year == NPSCurrentYear && x.RecordStatus != "Deleted").Count(),
                               NEPromoter = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && Countries.Any(val => x.Country.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateSurveyReceived.Value.Year == NPSCurrentYear && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                               NEPassive = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && Countries.Any(val => x.Country.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateSurveyReceived.Value.Year == NPSCurrentYear && x.NPSIndicator == "Passive" && x.RecordStatus != "Deleted").Count(),
                               NEDetractor = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && Countries.Any(val => x.Country.Equals(val)) && regions.Any(val => x.Region.Equals(val)) && x.DateSurveyReceived.Value.Year == NPSCurrentYear && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count(),
                           }).Distinct().ToList();
            int NPS_Current_Month = DateTime.Now.Month;
            for (int i = 1; i <= 12; i++)
            {
                if (i > NPS_Current_Month)
                {
                    if (i == 12)
                    {
                        rollingmonths += DateTime.Now.AddYears(-1).Year + "-" + i;
                    }
                    else if (i < 10)
                    {
                        rollingmonths += DateTime.Now.AddYears(-1).Year + "-0" + i + ",";
                    }
                    else
                    {
                        rollingmonths += DateTime.Now.AddYears(-1).Year + "-" + i + ",";
                    }
                }
                else
                {
                    if (i < 10)
                    {
                        rollingmonths += DateTime.Now.Year + "-0" + i + ",";
                    }
                    else
                    {
                        rollingmonths += DateTime.Now.Year + "-" + i + ",";
                    }
                }
            }
            RollingNPSMonths = rollingmonths.Split(',');
            var RollingNPS = (from a in entity.NpsImps
                              where a.RecordStatus != "Deleted"
                              where RollingNPSMonths.Any(val => a.YearMonth.Equals(val))
                              where a.NPSIndicator != null
                              where a.NPSIndicator != "null"
                              where a.NPSIndicator != ""
                              where Countries.Any(val => a.Country.Equals(val))
                              where ClientTypes.Any(val => a.ClientType.Equals(val))
                              where regions.Any(val => a.Region.Equals(val))
                              select a).ToList();
            var RollingNPSData = (from a in RollingNPS
                                  group a by a.YearMonth into g
                                  select new
                                  {
                                      YearMonth = g.Key,
                                      NESurveyReceived = RollingNPS.Where(x => x.YearMonth == g.Key).Count(),
                                      NEPromoter = RollingNPS.Where(x => x.YearMonth == g.Key && x.NPSIndicator == "Promoter").Count(),
                                      NEDetractor = RollingNPS.Where(x => x.YearMonth == g.Key && x.NPSIndicator == "Detractor").Count(),
                                      NPSScore = 0
                                  }).OrderBy(x => x.YearMonth).ToList();
            h_re.code = 200;
            h_re.message = "Success";
            h_re.PreMonthVolume = PreviousMonths.Sum() + "";
            h_re.PreMonthRecords = PreviousMonths.Count() + "";
            h_re.CurrentMonthVolume = CurrentMonth.Sum() + "";
            h_re.CurrentMonthRecords = CurrentMonth.Count() + "";
            h_re.NextMonthVolume = NextMonth.Sum() + "";
            h_re.NextMonthRecords = NextMonth.Count() + "";
            h_re.RoyMonthVolume = RemainingMonths.Sum() + "";
            h_re.RoyMonthRecords = RemainingMonths.Count() + "";
            h_re.ExpectedCurrentMonthVolume = PreviousMonths.Sum() + CurrentMonth.Sum() + NextMonth.Sum() + RemainingMonths.Sum() + "";
            h_re.ExpectedCurrentMonthRecords = PreviousMonths.Count() + CurrentMonth.Count() + NextMonth.Count() + RemainingMonths.Count() + "";
            h_re.FutureYearsVolume = FutureYears.Sum() + "";
            h_re.FutureYearsRecords = FutureYears.Count() + "";
            h_re.HoldVolume = Hold.Sum() + "";
            h_re.HoldRecords = Hold.Count() + "";
            h_re.PipelineVolume = Pipeline.Sum() + "";
            h_re.PipelineRecords = Pipeline.Count() + "";
            h_re.HighPotentialVolume = HighPotential.Sum() + "";
            h_re.HighPotentialRecords = HighPotential.Count() + "";
            h_re.PotentialVolume = FutureYears.Sum() + Hold.Sum() + Pipeline.Sum() + HighPotential.Sum() + "";
            h_re.PotentialRecords = FutureYears.Count() + Hold.Count() + Pipeline.Count() + HighPotential.Count() + "";
            h_re.Countries = Country;
            h_re.NpsData = NpsData;
            h_re.RollingNpsData = RollingNPSData;
            h_re.ExistingAddChangeCycleTime = ExistingAddChangeCT.Sum() + "";
            h_re.ExistingAddChangeProjectCount = ExistingAddChangeCT.Count() + "";
            h_re.ExistingServiceCycleTime = ExistingServiceCT.Sum() + "";
            h_re.ExistingServiceProjectCount = ExistingServiceCT.Count() + "";
            h_re.NewGlobalCycleTime = NewGlobalCT.Sum() + "";
            h_re.NewGlobalProjectCount = NewGlobalCT.Count() + "";
            h_re.NewLocalCycleTime = NewLocalCT.Sum() + "";
            h_re.NewLocalProjectCount = NewLocalCT.Count() + "";
            h_re.OverallCycleTime = OverallCT.Sum() + "";
            h_re.OverallProjectCount = OverallCT.Count() + "";
            return h_re;
        }
        [HttpPost]
        [Route("LastUpdatedOn")]
        public Response LastUpdatedOn(ReportUpdatedOn reportUpdatedOn)
        {
            var LastUpdatedon = (from a in entity.ReportUpdatedOns
                                 select new
                                 {
                                     LLData = entity.ReportUpdatedOns.Where(x => x.ReportName == "LLData").Max(x => (DateTime?)x.UpdatedOn),
                                     SGData = entity.ReportUpdatedOns.Where(x => x.ReportName == "SGData").Max(x => (DateTime?)x.UpdatedOn),
                                     IMPSData = entity.ReportUpdatedOns.Where(x => x.ReportName == "IMPSData").Max(x => (DateTime?)x.UpdatedOn),
                                     CTOData = entity.ReportUpdatedOns.Where(x => x.ReportName == "CTOData").Max(x => (DateTime?)x.UpdatedOn),
                                     AutomatedCLR = entity.ReportUpdatedOns.Where(x => x.ReportName == "CLRAutomated").Max(x => (DateTime?)x.UpdatedOn),
                                     ELT = entity.ReportUpdatedOns.Where(x => x.ReportName == "MonthlyDelta").Max(x => (DateTime?)x.UpdatedOn),
                                     NPS = entity.ReportUpdatedOns.Where(x => x.ReportName == "NPS").Max(x => (DateTime?)x.UpdatedOn),
                                 }).Distinct();
            re.code = 200;
            re.message = "Success";
            re.Data = LastUpdatedon;
            return re;
        } 
        [HttpPost]
        [Route("UserReportAccess")]
        public Response UserReportAccess(UserReportsAccess userReportsAccess)
        {
            var UserReportsList = (from a in entity.UserReportsAccesses
                                   join b in entity.Users on a.UID equals b.UID
                                   where a.UID == userReportsAccess.UID
                                     select new
                                     {
                                         a.IMPS,
                                         a.CTO,
                                         a.AutomatedCLR,
                                         a.StageGate,
                                         a.LessonsLearnt,
                                         a.CapacityTracker,
                                         a.CycleTime,
                                         a.ELTReport,
                                         a.C_Hierarchy,
                                         b.UserStatus,
                                         a.ResourceUtilization,
                                         a.NPS,
                                         a.NPSAdmin,
                                         a.NPSClientInfo,
                                         a.NPSEdit,
                                         a.DigitalReport,
                                         a.PerformanceAnalysis,
                                         a.Prospect,
                                         a.InsertedOn,
                                         a.UpdatedBy,
                                         a.UpdatedOn,
                                         a.MarketReport,
                                         a.MarketCommentsEdit,
                                         a.C_HierarchyEdits,
                                         a.CLREdits,
                                         b.AccountStatus,
                                         Notifications = entity.UserReportAccessTickets.Where(x => x.TicketStatuts == "Requested" && x.UID != a.UID).Count(),
                                     }).Distinct();
            var Username = entity.Users.FirstOrDefault(x => x.UID == userReportsAccess.UID).FirstName + " " + entity.Users.FirstOrDefault(x => x.UID == userReportsAccess.UID).LastName;
            var count = entity.NpsImps.Where(x => x.AssignLeaderForClosedLoop == Username && x.RecordStatus == "Action Required").Count();
            re.code = 200;
            re.message = "Success";
            re.Data = UserReportsList;
            re.RevenueID = count;
            return re;
        }
        [HttpPost]
        [Route("RequestAccessTicket")]
        public IHttpActionResult RequestAccessTicket(UserReportAccessTicket userReportAccessTicket)
        {
            Boolean b = new HomeModel().RequestAccessTicket(userReportAccessTicket);
            if (b)
            {
                userReportAccessTicket.RequestedOn = DateTime.Now;
                entity.UserReportAccessTickets.Add(userReportAccessTicket);
                entity.SaveChanges();
                re.code = 200;
                re.message = "Requested Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Generate Ticket";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("RequestAccessNotifications")]
        public Response RequestAccessNotifications(UserReportAccessTicket userReportAccessTicket)
        {
            var requests = (from a in entity.UserReportAccessTickets
                            where a.UID != userReportAccessTicket.UID
                            where a.TicketStatuts == "Requested"
                            join b in entity.Users on a.UID equals b.UID
                            select new {
                                a.UID,
                                Username = b.FirstName + " " + b.LastName,
                                a.TicketID,
                                a.ReportName,
                                a.RequestedOn,
                                a.TicketStatuts,
                            });
            re.code = 200;
            re.message = "Success";
            re.Data = requests;
            return re;
        }

        [HttpPost]
        [Route("GrantAccess")]
        public IHttpActionResult GrantAccess(UserReportAccessTicket userReportAccessTicket)
        {
            Boolean b = new HomeModel().GrantAccessTicket(userReportAccessTicket);
            if (b)
            {
                re.code = 200;
                re.message = "Access Granted";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Grant Access";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("DeleteUser")]
        public IHttpActionResult DeleteUser(User user)
        {
            Boolean b = new HomeModel().DeleteUserAccount(user);
            if (b)
            {
                re.code = 200;
                re.message = "Deleted Successfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Delete User";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("UserReportWiseRequests")]
        public Response UserReportWiseRequests(User users)
        {
            var data = (from a in entity.Users
                        where a.UID == users.UID
                        select new
                        {
                            a.UID,
                            AutomatedCLRticketID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "AutomatedCLR").Max(x2 => (int?)x2.TicketID) ?? 0,
                            CLREditsticketID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "AutomatedCLREdits").Max(x2 => (int?)x2.TicketID) ?? 0,
                            MarketReportticketID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "MarketReport").Max(x2 => (int?)x2.TicketID) ?? 0,
                            CycleTimeticketID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "CycleTime").Max(x2 => (int?)x2.TicketID) ?? 0,
                            ELTReportticketID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "ELTReport").Max(x2 => (int?)x2.TicketID) ?? 0,
                            IMPSticketID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "IMPS").Max(x2 => (int?)x2.TicketID) ?? 0,
                            CTOticketID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "CTO").Max(x2 => (int?)x2.TicketID) ?? 0,
                            LLticketID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "LL").Max(x2 => (int?)x2.TicketID) ?? 0,
                            StageGateticketID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "StageGate").Max(x2 => (int?)x2.TicketID) ?? 0,
                            MarketCommentEditsID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "MarketCommentEdits").Max(x2 => (int?)x2.TicketID) ?? 0,
                            NPSID = entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "NPS").Max(x2 => (int?)x2.TicketID) ?? 0,
                            //AutomatedCLR = entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "AutomatedCLR").Max(x2 => x2.TicketID)).TicketStatuts ?? "NoRequests",
                            //AutomatedCLRRequestedOn = entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == entity.UserReportAccessTickets.Where(x1 => x1.UID == users.UID && x1.ReportName == "AutomatedCLR").Max(x2 => x2.TicketID)).RequestedOn,
                        }).Distinct().ToList();
            var ticketDetails = (from a in data
                                 where a.UID == users.UID
                                 select new
                                 {
                                     a.UID,
                                     AutomatedCLR = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).AutomatedCLR,
                                     AutomatedCLRStatus = a.AutomatedCLRticketID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x=>x.TicketID == a.AutomatedCLRticketID).TicketStatuts,
                                     AutomatedCLRRequesstedOn = a.AutomatedCLRticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.AutomatedCLRticketID)?.RequestedOn,
                                     AutomatedCLRApporvedBy = a.AutomatedCLRticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.AutomatedCLRticketID).AcceptedBy,
                                     AutomatedCLRApporvedOn = a.AutomatedCLRticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.AutomatedCLRticketID)?.AcceptedOn,
                                     CLREdits = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).CLREdits,
                                     CLREditsStatus = a.CLREditsticketID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CLREditsticketID).TicketStatuts,
                                     CLREditsRequesstedOn = a.CLREditsticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CLREditsticketID)?.RequestedOn,
                                     CLREditsApporvedBy = a.CLREditsticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CLREditsticketID).AcceptedBy,
                                     CLREditsApporvedOn = a.CLREditsticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CLREditsticketID)?.AcceptedOn,
                                     MarketReport = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).MarketReport ?? false,
                                     MarketReportStatus = a.MarketReportticketID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.MarketReportticketID).TicketStatuts,
                                     MarketReportRequesstedOn = a.MarketReportticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.MarketReportticketID)?.RequestedOn,
                                     MarketReportApporvedBy = a.MarketReportticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.MarketReportticketID).AcceptedBy,
                                     MarketReportApporvedOn = a.MarketReportticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.MarketReportticketID)?.AcceptedOn,
                                     MarketCommentsEdit = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).MarketCommentsEdit,
                                     CommentEditStatus = a.MarketCommentEditsID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.MarketCommentEditsID).TicketStatuts,
                                     CommentEditRequesstedOn = a.MarketCommentEditsID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.MarketCommentEditsID)?.RequestedOn,
                                     CommentEditApporvedBy = a.MarketCommentEditsID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.MarketCommentEditsID).AcceptedBy,
                                     CommentEditApporvedOn = a.MarketCommentEditsID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.MarketCommentEditsID)?.AcceptedOn,
                                     CycleTime = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).CycleTime,
                                     CycleTimeStatus = a.CycleTimeticketID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CycleTimeticketID).TicketStatuts,
                                     CycleTimeRequesstedOn = a.CycleTimeticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CycleTimeticketID)?.RequestedOn,
                                     CycleTimeApporvedBy = a.CycleTimeticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CycleTimeticketID).AcceptedBy,
                                     CycleTimeApporvedOn = a.CycleTimeticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CycleTimeticketID)?.AcceptedOn,
                                     ELTReport = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).ELTReport,
                                     ELTReportStatus = a.ELTReportticketID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.ELTReportticketID).TicketStatuts,
                                     ELTReportRequesstedOn = a.ELTReportticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.ELTReportticketID)?.RequestedOn,
                                     ELTReportApporvedBy = a.ELTReportticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.ELTReportticketID).AcceptedBy,
                                     ELTReportApporvedOn = a.ELTReportticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.ELTReportticketID)?.AcceptedOn,
                                     IMPS = entity.UserReportsAccesses.FirstOrDefault(x=>x.UID == a.UID).IMPS,
                                     IMPSStatus = a.IMPSticketID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.IMPSticketID).TicketStatuts,
                                     IMPSRequesstedOn = a.IMPSticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.IMPSticketID)?.RequestedOn,
                                     IMPSApporvedBy = a.IMPSticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.IMPSticketID).AcceptedBy,
                                     IMPSApporvedOn = a.IMPSticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.IMPSticketID)?.AcceptedOn,
                                     CTO = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).CTO,
                                     CTOStatus = a.CTOticketID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CTOticketID).TicketStatuts,
                                     CTORequesstedOn = a.CTOticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CTOticketID)?.RequestedOn,
                                     CTOApporvedBy = a.CTOticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CTOticketID).AcceptedBy,
                                     CTOApporvedOn = a.CTOticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.CTOticketID)?.AcceptedOn,
                                     LessonsLearnt = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).LessonsLearnt,
                                     LLStatus = a.LLticketID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.LLticketID).TicketStatuts,
                                     LLRequesstedOn = a.LLticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.LLticketID)?.RequestedOn,
                                     LLApporvedBy = a.LLticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.LLticketID).AcceptedBy,
                                     LLApporvedOn = a.LLticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.LLticketID)?.AcceptedOn,
                                     StageGate = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).StageGate,
                                     StageGateStatus = a.StageGateticketID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.StageGateticketID).TicketStatuts,
                                     StageGateRequesstedOn = a.StageGateticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.StageGateticketID)?.RequestedOn,
                                     StageGateApporvedBy = a.StageGateticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.StageGateticketID).AcceptedBy,
                                     StageGateApporvedOn = a.StageGateticketID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.StageGateticketID)?.AcceptedOn,
                                     NPS = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == a.UID).NPS ?? false,
                                     NPSStatus = a.NPSID == 0 ? "NoRequests" : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.NPSID).TicketStatuts,
                                     NPSRequesstedOn = a.NPSID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.NPSID)?.RequestedOn,
                                     NPSApporvedBy = a.NPSID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.NPSID).AcceptedBy,
                                     NPSApporvedOn = a.NPSID == 0 ? null : entity.UserReportAccessTickets.FirstOrDefault(x => x.TicketID == a.NPSID)?.AcceptedOn,
                                 }).Distinct().ToList();
            re.code = 200;
            re.message = "Success";
            re.Data = ticketDetails;
            return re;
        }
        
        [HttpPost]
        [Route("UserAccessDetailsForAdmin")]
        public Response UserAccessDetailsForAdmin(UserReportsAccess userReportsAccess)
        {
            var usersData = (from a in entity.UserReportsAccesses
                             where a.UID != userReportsAccess.UID
                             join b in entity.Users on a.UID equals b.UID
                             where b.UserStatus != "Deleted"
                             select new
                             {
                                 a.UID,
                                 UserName = b.FirstName +" "+ b.LastName,
                                 a.IMPS,
                                 a.CTO,
                                 a.LessonsLearnt,
                                 a.StageGate,
                                 a.MarketReport,
                                 a.AutomatedCLR,
                                 a.MarketCommentsEdit,
                                 a.C_HierarchyEdits,
                                 a.CLREdits,
                                 a.CapacityTracker,
                                 a.ELTReport,
                                 a.CycleTime,
                                 a.C_Hierarchy,
                                 a.NPS,
                                 a.NPSAdmin,
                                 a.NPSClientInfo,
                                 a.NPSEdit,
                                 a.PerformanceAnalysis,
                                 a.DigitalReport,
                                 b.JobType,
                                 a.UserAccessStatus,
                                 a.ResourceUtilization,
                                 a.Prospect,
                                 a.InsertedOn,
                                 a.UpdatedOn,
                                 a.UpdatedBy,
                                 b.AccountStatus
                             });
            re.code = 200;
            re.message = "Success";
            re.Data = usersData;
            return re;
        }
        
        [HttpPost]
        [Route("UpdatingAccessDetails")]
        public IHttpActionResult UpdatingAccessDetails(UserReportsAccess userReportsAccess)
        {
            Boolean b = new HomeModel().UpdatingAccess(userReportsAccess);
            if (b)
            {
                re.code = 200;
                re.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Update Data";
                return Content(HttpStatusCode.OK, re);
            }
        }
    }
}