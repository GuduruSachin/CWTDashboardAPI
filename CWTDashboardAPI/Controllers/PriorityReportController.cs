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
    public class PriorityReportController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        HomeResponse h_re = new HomeResponse();
        Response re = new Response();
        Filters fi = new Filters();

        [HttpPost]
        [Route("PriorityReportFiltersList")]
        public Filters PriorityReportFiltersList(CLRData clr)
        {
            //var FilterYear = entity.CLRs.Select(a => a.Go_Live_Year).Distinct().OrderByDescending(b => b.Go_Live_Year).ToList();
            //var Filteryear = (from a in entity.CLRDatas
            //                  where a.Status == "Active"
            //                  where a.GoLiveYear != null && a.GoLiveYear != "1900" && a.GoLiveYear != ""
            //                  select new
            //                  {
            //                      Go_Live_Year = a.GoLiveYear,
            //                      isSelected = true,
            //                  }).Distinct().OrderBy(x => x.Go_Live_Year);
            var FilterPriority = (from a in entity.ManualDatas
                                  where a.Priority != "---"
                                  where a.Priority != null
                                  select new
                                  {
                                      Priority = a.Priority,
                                      isSelected = true,
                                  }).Distinct().OrderBy(x => x.Priority);
            var FilterOpportunity_Type = (from a in entity.CLRDatas
                                      where a.Status == "Active"
                                      select new
                                      {
                                          Opportunity_Type = a.Opportunity_Type == null || a.Opportunity_Type == "" || a.Opportunity_Type == "---" ? "---" : a.Opportunity_Type ?? "---",
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.Opportunity_Type);
            fi.code = 200;
            fi.message = "Data Successfull";
            fi.FilterPriority = FilterPriority;
            fi.FilterOpportunity_Type = FilterOpportunity_Type;
            return fi;
        }

        string[] Priority, Opportunity_Type;
        [HttpPost]
        [Route("GetPriorityData")]
        public Response GetPriorityData(CLRData cLRData)
        {
            if (cLRData.Opportunity_Type == null || cLRData.ProjectStatus == null)
            {
                re.PriorityData = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                Priority = cLRData.ProjectStatus.Split(',');
                for (int i = 0; i < Priority.Count(); i++)
                {
                    if (Priority[i] == "" || Priority[i] == "---" || Priority[i] == "null")
                    {
                        Priority[i] = "---";
                    }
                }
                Opportunity_Type = cLRData.Opportunity_Type.Split(',');
                for (int i = 0; i < Opportunity_Type.Count(); i++)
                {
                    if (Opportunity_Type[i] == "" || Opportunity_Type[i] == "---" || Opportunity_Type[i] == "null")
                    {
                        Opportunity_Type[i] = "---";
                    }
                }
                var clr_data = (from a in entity.CLRDatas
                                where a.Status == "Active"
                                where Opportunity_Type.Any(val => a.Opportunity_Type.Equals(val))
                                join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID
                                where Priority.Any(val => b.Priority.Equals(val))
                                select new {
                                    a.Client,
                                    Workspace_Title = a.Workspace_Title ?? "---",
                                    a.Opportunity_Type,
                                    ManualCLient = b.Client,
                                    a.OppTOtalVolume,
                                    a.RevenueVolumeUSD,
                                    a.Line_Win_Probability,
                                    b.ExpectedDecisionDate,
                                    b.Assignment_date,
                                    a.Sales_Stage_Name,
                                    a.Revenue_Opportunity_Type,
                                    b.Pipeline_comments,
                                    a.Country,
                                }).ToList();
                var PriorityData = (from a in clr_data
                                    group a by a.Client into g
                                    select new
                                    {
                                        Client = g.Key,
                                        Workspace_Title = clr_data.FirstOrDefault(x => x.Client == g.Key).Workspace_Title ?? "---",
                                        OppTOtalVolume = clr_data.FirstOrDefault(x => x.Client == g.Key).OppTOtalVolume,
                                        RevenueVolumeUSD = clr_data.Where(x => x.Client == g.Key).Sum(x => x.RevenueVolumeUSD),
                                        Line_Win_Probability = clr_data.FirstOrDefault(x => x.Client == g.Key).Line_Win_Probability,
                                        Sales_Stage_Name = clr_data.FirstOrDefault(x => x.Client == g.Key).Sales_Stage_Name,
                                        Opportunity_Type = clr_data.FirstOrDefault(x => x.Client == g.Key).Opportunity_Type,
                                        ExpectedDecisionDate = clr_data.Where(x => x.Client == g.Key).Min(x => x.ExpectedDecisionDate),
                                        Assignment_date = clr_data.Where(x => x.Client == g.Key).Min(x => x.Assignment_date),
                                        Pipeline_comments = string.Join(", ",clr_data.Where(x => x.Client == g.Key).Select(x => x.Pipeline_comments).Distinct()),
                                        Country = clr_data.Where(x => x.Client == g.Key).Distinct().Count(),
                                        CountryWiseData = (from b in clr_data
                                                           where b.Client == g.Key
                                                           select b).Distinct(),
                                    }).OrderBy(x => x.Client);
                re.code = 200;
                re.message = "Success";
                re.PriorityData = PriorityData;
            }
            return re;
        }
        [HttpPost]
        [Route("ValidDuplicateRevenueVolumeChange")]
        public Response ValidDuplicateRevenueVolumeChange(CLRData cLRData)
        {
            var CreationDate = "01-01-2020";
            DateTime ConvertedDate = Convert.ToDateTime(CreationDate);
            var RevCheck = (from a in entity.CLRDatas
                            where a.GoLiveDate >= ConvertedDate
                            where a.RevenueID != 400000000000000
                            group a by a.RevenueID into g
                            where g.Count() > 1
                            select g.Key).ToList();
            for (int i = 0; i < RevCheck.Count; i++)
            {
                var revid = RevCheck[i];
                var ClientCountry = (from a in entity.CLRDatas
                                     where a.RevenueID == revid
                                     select new
                                     {
                                         a.RevenueID,
                                         a.Country,
                                         a.Client,
                                         a.GoLiveDate,
                                         a.RevenueVolumeUSD,
                                         a.Task__Task_Record_ID_Key
                                     }).Distinct().ToList();
                var RevVolume = ClientCountry[0].RevenueVolumeUSD;
                if (ClientCountry.Count() > 1)
                {
                    var CLRIDData = (from a in entity.CLRDatas
                                     where a.RevenueID == revid
                                     select a.CLRID).ToList();
                    for (int j = 0; j < CLRIDData.Count(); j++)
                    {
                        var CLRID = CLRIDData[j];
                        CLRData Vp = (from s in entity.CLRDatas
                                      where s.CLRID == CLRID
                                      select s).FirstOrDefault();
                        if (j == 0)
                        {
                            var Final_Revenue_Opportunity_Type = "Up-Sell(Add Offices/Countries),New Business,Re-Bid With Up-Sell".Split(',');
                            var Revenuevolume = entity.CRMDatas.Where(x => x.Revenue_Id == revid && Final_Revenue_Opportunity_Type.Any(val => x.Opportunity_Type.Equals(val))).Count() > 0 ? entity.CRMDatas.FirstOrDefault(x => x.Revenue_Id == revid && Final_Revenue_Opportunity_Type.Any(val => x.Opportunity_Type.Equals(val)))?.Revenue_Total_Volume_USD : 0;
                            Vp.RevenueVolumeUSD = Revenuevolume;
                        }
                        else
                        {
                            Vp.RevenueVolumeUSD = 0;
                        }
                        entity.SaveChanges();
                    }
                }
                else
                {
                }
            }
            re.code = 200;
            re.message = "Data Successfull";
            return re;
        }
    }
}