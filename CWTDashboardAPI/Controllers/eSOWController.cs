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
    public class eSOWController : ApiController
    {
        CWTEntities entity = new CWTEntities();
        Response re = new Response();
        eSOWFilters fi = new eSOWFilters();

        [HttpPost]
        [Route("EssentialTablesFilters")]
        public eSOWFilters EssentialTablesFilters(eSOW esow)
        {
            var FilterProspectType = (from a in entity.eSOWs
                                      where a.Prospect_Type != null
                                      select new
                                      {
                                          Prospect_Type = a.Prospect_Type,
                                          isSelected = true,
                                      }).Distinct().OrderByDescending(x=>x.Prospect_Type);
            var FilterSalesLeaderBidType= (from a in entity.eSOWs
                              select new
                              {
                                  Sales_Leader_type_and_Type_of_bid = a.Sales_Leader_type_and_Type_of_bid,
                                  isSelected = true,
                              }).Distinct().OrderByDescending(x => x.Sales_Leader_type_and_Type_of_bid);
            fi.code = 200;
            fi.message = "Success";
            fi.ProspectType = FilterProspectType;
            fi.SalesLeaderTypeAndTypeOfBid = FilterSalesLeaderBidType;
            return fi;
        }
        string[] e_SalesLeaderBidType, e_ProspectType,e_CrmStatusWon,e_CrmStatusLost;
        [HttpPost]
        [Route("EssentialTables")]
        public Response EssentialTables(eSOW esow)
        {
            if (esow.Prospect_Type == null || esow.Sales_Leader_type_and_Type_of_bid == null || esow.StartDate == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                //var CreationDate = "07/07/2019";
                var CreationDate = esow.StartDate;
                DateTime ConvertedDate = Convert.ToDateTime(CreationDate);
                e_CrmStatusWon = "Contract Signed,Verbal Award".Split(',');
                e_CrmStatusLost = "Withdrawn,No Go,Closed Lost".Split(',');
                e_SalesLeaderBidType = esow.Sales_Leader_type_and_Type_of_bid.Split(',');
                for (int i = 0; i < e_SalesLeaderBidType.Count(); i++)
                {
                    if (e_SalesLeaderBidType[i] == "" || e_SalesLeaderBidType[i] == "null")
                    {
                        e_SalesLeaderBidType[i] = null;
                    }
                }
                e_ProspectType = esow.Prospect_Type.Split(',');
                for (int i = 0; i < e_ProspectType.Count(); i++)
                {
                    if (e_ProspectType[i] == "" || e_ProspectType[i] == "null")
                    {
                        e_ProspectType[i] = null;
                    }
                }
                var Accounts_Volumes = (from a in entity.eSOWs
                                           where e_ProspectType.Any(val1 => a.Prospect_Type.Equals(val1))
                                           where e_SalesLeaderBidType.Any(val2 => a.Sales_Leader_type_and_Type_of_bid.Equals(val2))
                                           where a.Number_of_Countries > 0
                                           //where a.SOW_Creation_Date != null
                                           where a.SOW_Creation_Date > ConvertedDate
                                           group a by a.Category into g
                                           select new
                                           {
                                               Category = g.Key,
                                               SSATotal = g.Where(x => x.DSD_Lead == null).Count(),
                                               SSAWon = g.Where(x => x.DSD_Lead == null && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                               SSALost = g.Where(x => x.DSD_Lead == null && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                               SSAInPRogress = g.Where(x => x.DSD_Lead == null).Count() - g.Where(x => x.DSD_Lead == null && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Count() - g.Where(x => x.DSD_Lead == null && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                               DSDLATotal = g.Where(x => x.DSD_Lead != null).Count(),
                                               DSDLAWon = g.Where(x => x.DSD_Lead != null && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                               DSDLALost = g.Where(x => x.DSD_Lead != null && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                               DSDLAInPRogress = g.Where(x => x.DSD_Lead != null).Count() - g.Where(x => x.DSD_Lead != null && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Count() - g.Where(x => x.DSD_Lead != null && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                               SSVTotal = g.Where(x => x.DSD_Lead == null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                               SSVWon = g.Where(x => x.DSD_Lead == null && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                               SSVLost = g.Where(x => x.DSD_Lead == null && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                               SSVInPRogress = (g.Where(x => x.DSD_Lead == null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0) - (g.Where(x => x.DSD_Lead == null && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0) - (g.Where(x => x.DSD_Lead == null && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0),
                                               DSDLVTotal = g.Where(x => x.DSD_Lead != null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                               DSDLVWon = g.Where(x => x.DSD_Lead != null && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                               DSDLVLost = g.Where(x => x.DSD_Lead != null && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                               DSDLVInPRogress = (g.Where(x => x.DSD_Lead != null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0) - (g.Where(x => x.DSD_Lead != null && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0) - (g.Where(x => x.DSD_Lead != null && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0),
                                           }).OrderByDescending(x => x.Category);
                re.code = 200;
                re.message = "Success "+ e_ProspectType + e_SalesLeaderBidType;
                re.Data = Accounts_Volumes;
            }
            return re;
        }

        string[] dsd_CrmStatusLost, dsd_CrmStatusInProgress;
        [HttpPost]
        [Route("DSDmetrics")]
        public Response DSDmetrics(eSOW esow)
        {
            if (esow.StartDate == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                //var CreationDate = "09/18/2018";
                var CreationDate = esow.StartDate;
                DateTime ConvertedDate = Convert.ToDateTime(CreationDate);
                //dsd_CrmStatusWon = "Contract Signed,Verbal Award".Split(',');
                dsd_CrmStatusLost = "Withdrawn,No Go,Closed Lost".Split(',');
                dsd_CrmStatusInProgress = "Needs are Identified,RFP Received,Proposal Submitted,RFI Received,Negotiations,RFI Submitted".Split(',');
                var WinRatePerWOVerbalAward = (from a in entity.eSOWs
                                               where a.Number_of_Countries > 0
                                               where a.SOW_Creation_Date > ConvertedDate
                                               select a);
                var Data = (from a in WinRatePerWOVerbalAward
                            select new
                            {
                                DSDLAWon = WinRatePerWOVerbalAward.Where(x => x.CRM_Status == "Contract Signed" && x.DSD_Lead != null).Count(),
                                DSDLAVerbal = WinRatePerWOVerbalAward.Where(x => x.CRM_Status == "Verbal Award" && x.DSD_Lead != null).Count(),
                                DSDLALost = WinRatePerWOVerbalAward.Where(x => dsd_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status)) && x.DSD_Lead != null).Count(),
                                SSAWon = WinRatePerWOVerbalAward.Where(x => x.CRM_Status == "Contract Signed" && x.DSD_Lead == null).Count(),
                                SSAVerbal = WinRatePerWOVerbalAward.Where(x => x.CRM_Status == "Verbal Award" && x.DSD_Lead == null).Count(),
                                SSALost = WinRatePerWOVerbalAward.Where(x => dsd_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status)) && x.DSD_Lead == null).Count(),

                                DSDLVWon = WinRatePerWOVerbalAward.Where(x => x.CRM_Status == "Contract Signed" && x.DSD_Lead != null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                DSDLVVerbal = WinRatePerWOVerbalAward.Where(x => x.CRM_Status == "Verbal Award" && x.DSD_Lead != null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                DSDLVLost = WinRatePerWOVerbalAward.Where(x => dsd_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status)) && x.DSD_Lead != null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                SSVWon = WinRatePerWOVerbalAward.Where(x => x.CRM_Status == "Contract Signed" && x.DSD_Lead == null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                SSVVerbal = WinRatePerWOVerbalAward.Where(x => x.CRM_Status == "Verbal Award" && x.DSD_Lead == null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                SSVLost = WinRatePerWOVerbalAward.Where(x => dsd_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status)) && x.DSD_Lead == null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,

                                DSDLAccounts = WinRatePerWOVerbalAward.Where(x => dsd_CrmStatusInProgress.Any(xs => xs.Equals(x.CRM_Status)) && x.DSD_Lead != null).Count(),
                                DSDLSpend = WinRatePerWOVerbalAward.Where(x => dsd_CrmStatusInProgress.Any(xs => xs.Equals(x.CRM_Status)) && x.DSD_Lead != null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_),
                                DSDLAvgDaysSinceStart = "",
                                SSAccounts = WinRatePerWOVerbalAward.Where(x => dsd_CrmStatusInProgress.Any(xs => xs.Equals(x.CRM_Status)) && x.DSD_Lead == null).Count(),
                                SSSpend = WinRatePerWOVerbalAward.Where(x => dsd_CrmStatusInProgress.Any(xs => xs.Equals(x.CRM_Status)) && x.DSD_Lead == null).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_),
                                SSAvgDaysSinceStart = "",
                            }).Distinct();
                re.code = 200;
                re.message = "Success";
                re.Data = Data;
            }
            return re;
        }
        string[] ytd_CrmStatusWon,ytd_CrmStatusLost;
        [HttpPost]
        [Route("TotalYTDActivity")]
        public Response TotalYTDActivity(eSOW esow)
        {
            if (esow.StartDate == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                var CreationDate = esow.StartDate;
                DateTime ConvertedDate = Convert.ToDateTime(CreationDate);
                ytd_CrmStatusWon = "Contract Signed,Verbal Award".Split(',');
                ytd_CrmStatusLost = "Withdrawn,No Go,Closed Lost".Split(',');
                dsd_CrmStatusInProgress = "Needs are Identified,RFP Received,Proposal Submitted,RFI Received,Negotiations,RFI Submitted".Split(',');
                var TotalYTDActivity = (from a in entity.eSOWs
                                        where a.SOW_Creation_Date > ConvertedDate
                                        where a.Number_of_Countries > 0
                                        group a by a.DSD_Lead into g
                                        select new
                                        {
                                            Dsd_Lead = g.Key ?? "Self Service(No Lead)",
                                            WonAccounts = g.Where(x => ytd_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                            LostAccounts = g.Where(x => ytd_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                            TotalAccounts = g.Count(),
                                            WonVolumes = g.Where(x => ytd_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                            LostVolumes = g.Where(x => ytd_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                            TotalVolumes = g.Sum(x => x.Total_traffic_in_Million_USD__air_rail_only_) ?? 0,
                                        }).OrderBy(x=>x.Dsd_Lead);
                re.code = 200;
                re.message = "Success";
                re.Data = TotalYTDActivity;
            }
            return re;
        }
        [HttpPost]
        [Route("DavidData")]
        public Response DavidData(eSOW esow)
        {
            if (esow.StartDate == null || esow.EndDate == null)
            {
                re.code = 100;
                re.message = "Please select the date";
                re.Data = null;
            }
            else
            {
                DateTime startDate = Convert.ToDateTime(esow.StartDate);
                DateTime endDate = Convert.ToDateTime(esow.EndDate);
                e_CrmStatusWon = "Contract Signed,Verbal Award".Split(',');
                e_CrmStatusLost = "Withdrawn,No Go,Closed Lost".Split(',');
                var Accounts_Volumes = (from a in entity.eSOWs
                                        where a.Number_of_Countries > 0
                                        where a.SOW_Creation_Date >= startDate && a.SOW_Creation_Date <= endDate
                                        group a by a.Category into g
                                        select new
                                        {
                                            Category = g.Key,
                                            DDTotal = g.Count(),
                                            DDWon = g.Where(x => e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                            DDLost = g.Where(x => e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                            DDInPRogress = g.Count() - g.Where(x => e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Count() - g.Where(x => e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                            DDNBTotal = g.Where(x => x.Prospect_Type == "New Business").Count(),
                                            DDNBWon = g.Where(x => x.Prospect_Type == "New Business" && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                            DDNBLost = g.Where(x => x.Prospect_Type == "New Business" && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                            DDNBInPRogress = g.Where(x => x.Prospect_Type == "New Business").Count() - g.Where(x => x.Prospect_Type == "New Business" && e_CrmStatusWon.Any(xs => xs.Equals(x.CRM_Status))).Count() - g.Where(x => x.Prospect_Type == "New Business" && e_CrmStatusLost.Any(xs => xs.Equals(x.CRM_Status))).Count(),
                                        }).OrderByDescending(x => x.Category);
                re.code = 200;
                re.message = "Success"+ startDate+"  "+endDate;
                re.Data = Accounts_Volumes;
            }
            return re;
        }
    }
}