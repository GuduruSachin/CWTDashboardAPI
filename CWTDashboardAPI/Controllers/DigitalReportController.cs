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
    public class DigitalReportController : ApiController
    {

        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();


        string[] GDS_ProjectStatus, GDS_GoLiveYear, GDS_Quarter, GDS_GlobalCISOBTLead, GDS_Region;
        [HttpPost]
        [Route("DRRegionWiseGDS")]
        public Response DRRegionWiseGDS(CLRData cLRData)
        {
            if (cLRData.ProjectStatus == null || cLRData.GoLiveYear == null || cLRData.Quarter == null || cLRData.GlobalCISOBTLead == null || cLRData.Region == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                GDS_ProjectStatus = cLRData.ProjectStatus.Split(',');
                for (int i = 0; i < GDS_ProjectStatus.Count(); i++)
                {
                    if (GDS_ProjectStatus[i] == "" || GDS_ProjectStatus[i] == "---" || GDS_ProjectStatus[i] == "null")
                    {
                        GDS_ProjectStatus[i] = "---";
                    }
                }
                GDS_GoLiveYear = cLRData.GoLiveYear.Split(',');
                for (int i = 0; i < GDS_GoLiveYear.Count(); i++)
                {
                    if (GDS_GoLiveYear[i] == "" || GDS_GoLiveYear[i] == "---" || GDS_GoLiveYear[i] == "null")
                    {
                        GDS_GoLiveYear[i] = "---";
                    }
                }
                GDS_Quarter = cLRData.Quarter.Split(',');
                for (int i = 0; i < GDS_Quarter.Count(); i++)
                {
                    if (GDS_Quarter[i] == "" || GDS_Quarter[i] == "---" || GDS_Quarter[i] == "null")
                    {
                        GDS_Quarter[i] = "";
                    }
                }
                GDS_GlobalCISOBTLead = cLRData.GlobalCISOBTLead.Split(',');
                for (int i = 0; i < GDS_GlobalCISOBTLead.Count(); i++)
                {
                    if (GDS_GlobalCISOBTLead[i] == "" || GDS_GlobalCISOBTLead[i] == "---" || GDS_GlobalCISOBTLead[i] == "null")
                    {
                        GDS_GlobalCISOBTLead[i] = "---";
                    }
                }
                GDS_Region = cLRData.Region.Split(',');
                for (int i = 0; i < GDS_Region.Count(); i++)
                {
                    if (GDS_Region[i] == "" || GDS_Region[i] == "---" || GDS_Region[i] == "null")
                    {
                        GDS_Region[i] = "";
                    }
                }
                var clrdata_filtered = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where GDS_ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                                        where GDS_Quarter.Any(val => a.Quarter.Equals(val))
                                        where GDS_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                        where GDS_GlobalCISOBTLead.Any(val => a.GlobalCISOBTLead.Equals(val)) || GDS_GlobalCISOBTLead.Any(val => a.RegionalCISOBTLead.Equals(val))
                                        || GDS_GlobalCISOBTLead.Any(val => a.LocalDigitalOBTLead.Equals(val)) || GDS_GlobalCISOBTLead.Any(val => a.GlobalCISPortraitLead.Equals(val))
                                        || GDS_GlobalCISOBTLead.Any(val => a.RegionalCISPortraitLead.Equals(val)) || GDS_GlobalCISOBTLead.Any(val => a.GlobalCISHRFeedSpecialist.Equals(val))
                                        where GDS_Region.Any(val => a.Region.Equals(val))
                                        select a).ToList();
                var data = (from a in clrdata_filtered
                            group a by a.DigitalGDS into g
                            select new
                            {
                                GDS = g.Key == "---" || g.Key == "" || g.Key == null ? "(Blanks)" : g.Key ?? "(Blanks)",
                                EMEA = clrdata_filtered.Where(x => x.DigitalGDS == g.Key && x.Region == "EMEA").Count(),
                                APAC = clrdata_filtered.Where(x => x.DigitalGDS == g.Key && x.Region == "APAC").Count(),
                                NORAM = clrdata_filtered.Where(x => x.DigitalGDS == g.Key && x.Region == "NORAM").Count(),
                                LATAM = clrdata_filtered.Where(x => x.DigitalGDS == g.Key && x.Region == "LATAM").Count(),
                            }).OrderBy(x => x.GDS);
                re.Data = data;
            }
            return re;
        }
        string[] AT_ProjectStatus, AT_GoLiveYear, AT_Quarter, AT_GlobalCISOBTLead, AT_Region;
        [HttpPost]
        [Route("DRRegionWiseActivityType")]
        public Response DRRegionWiseActivityType(CLRData cLRData)
        {
            if (cLRData.ProjectStatus == null || cLRData.GoLiveYear == null || cLRData.Quarter == null || cLRData.GlobalCISOBTLead == null || cLRData.Region == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                AT_ProjectStatus = cLRData.ProjectStatus.Split(',');
                for (int i = 0; i < AT_ProjectStatus.Count(); i++)
                {
                    if (AT_ProjectStatus[i] == "" || AT_ProjectStatus[i] == "---" || AT_ProjectStatus[i] == "null")
                    {
                        AT_ProjectStatus[i] = "---";
                    }
                }
                AT_GoLiveYear = cLRData.GoLiveYear.Split(',');
                for (int i = 0; i < AT_GoLiveYear.Count(); i++)
                {
                    if (AT_GoLiveYear[i] == "" || AT_GoLiveYear[i] == "---" || AT_GoLiveYear[i] == "null")
                    {
                        AT_GoLiveYear[i] = "---";
                    }
                }
                AT_Quarter = cLRData.Quarter.Split(',');
                for (int i = 0; i < AT_Quarter.Count(); i++)
                {
                    if (AT_Quarter[i] == "" || AT_Quarter[i] == "---" || AT_Quarter[i] == "null")
                    {
                        AT_Quarter[i] = "";
                    }
                }
                AT_GlobalCISOBTLead = cLRData.GlobalCISOBTLead.Split(',');
                for (int i = 0; i < AT_GlobalCISOBTLead.Count(); i++)
                {
                    if (AT_GlobalCISOBTLead[i] == "" || AT_GlobalCISOBTLead[i] == "---" || AT_GlobalCISOBTLead[i] == "null")
                    {
                        AT_GlobalCISOBTLead[i] = "---";
                    }
                }
                AT_Region = cLRData.Region.Split(',');
                for (int i = 0; i < AT_Region.Count(); i++)
                {
                    if (AT_Region[i] == "" || AT_Region[i] == "---" || AT_Region[i] == "null")
                    {
                        AT_Region[i] = "";
                    }
                }
                var clrdata_filtered = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where AT_ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                                        where AT_Quarter.Any(val => a.Quarter.Equals(val))
                                        where AT_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                        where AT_GlobalCISOBTLead.Any(val => a.GlobalCISOBTLead.Equals(val)) || AT_GlobalCISOBTLead.Any(val => a.RegionalCISOBTLead.Equals(val))
                                        || AT_GlobalCISOBTLead.Any(val => a.LocalDigitalOBTLead.Equals(val)) || AT_GlobalCISOBTLead.Any(val => a.GlobalCISPortraitLead.Equals(val))
                                        || AT_GlobalCISOBTLead.Any(val => a.RegionalCISPortraitLead.Equals(val)) || AT_GlobalCISOBTLead.Any(val => a.GlobalCISHRFeedSpecialist.Equals(val))
                                        where AT_Region.Any(val => a.Region.Equals(val))
                                        select a).ToList();
                var data = (from a in clrdata_filtered
                            group a by a.DigitalActivityType into g
                            select new
                            {
                                ActivityType = g.Key == "---" || g.Key == "" || g.Key == null ? "(Blanks)" : g.Key ?? "(Blanks)",
                                EMEA = clrdata_filtered.Where(x => x.DigitalActivityType == g.Key && x.Region == "EMEA").Count(),
                                APAC = clrdata_filtered.Where(x => x.DigitalActivityType == g.Key && x.Region == "APAC").Count(),
                                NORAM = clrdata_filtered.Where(x => x.DigitalActivityType == g.Key && x.Region == "NORAM").Count(),
                                LATAM = clrdata_filtered.Where(x => x.DigitalActivityType == g.Key && x.Region == "LATAM").Count(),
                                GrandTotal = clrdata_filtered.Where(x => x.DigitalActivityType == g.Key).Count(),
                            }).OrderByDescending(x => x.GrandTotal);
                re.Data = data;
            }
            return re;
        }
        string[] Year_ProjectStatus, Year_GoLiveYear, Year_Quarter, Year_GlobalCISOBTLead, Year_Region;
        [HttpPost]
        [Route("DRRegionWiseYear")]
        public Response DRRegionWiseYear(CLRData cLRData)
        {
            if (cLRData.ProjectStatus == null || cLRData.GoLiveYear == null || cLRData.Quarter == null || cLRData.GlobalCISOBTLead == null || cLRData.Region == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                Year_ProjectStatus = cLRData.ProjectStatus.Split(',');
                for (int i = 0; i < Year_ProjectStatus.Count(); i++)
                {
                    if (Year_ProjectStatus[i] == "" || Year_ProjectStatus[i] == "---" || Year_ProjectStatus[i] == "null")
                    {
                        Year_ProjectStatus[i] = "---";
                    }
                }
                Year_GoLiveYear = cLRData.GoLiveYear.Split(',');
                for (int i = 0; i < Year_GoLiveYear.Count(); i++)
                {
                    if (Year_GoLiveYear[i] == "" || Year_GoLiveYear[i] == "---" || Year_GoLiveYear[i] == "null")
                    {
                        Year_GoLiveYear[i] = "---";
                    }
                }
                Year_Quarter = cLRData.Quarter.Split(',');
                for (int i = 0; i < Year_Quarter.Count(); i++)
                {
                    if (Year_Quarter[i] == "" || Year_Quarter[i] == "---" || Year_Quarter[i] == "null")
                    {
                        Year_Quarter[i] = "";
                    }
                }
                Year_GlobalCISOBTLead = cLRData.GlobalCISOBTLead.Split(',');
                for (int i = 0; i < Year_GlobalCISOBTLead.Count(); i++)
                {
                    if (Year_GlobalCISOBTLead[i] == "" || Year_GlobalCISOBTLead[i] == "---" || Year_GlobalCISOBTLead[i] == "null")
                    {
                        Year_GlobalCISOBTLead[i] = "---";
                    }
                }
                Year_Region = cLRData.Region.Split(',');
                for (int i = 0; i < Year_Region.Count(); i++)
                {
                    if (Year_Region[i] == "" || Year_Region[i] == "---" || Year_Region[i] == "null")
                    {
                        Year_Region[i] = "";
                    }
                }

                var clrdata_filtered = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where Year_ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                                        where Year_Quarter.Any(val => a.Quarter.Equals(val))
                                        where Year_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                        where Year_GlobalCISOBTLead.Any(val => a.GlobalCISOBTLead.Equals(val)) || Year_GlobalCISOBTLead.Any(val => a.RegionalCISOBTLead.Equals(val)) 
                                        || Year_GlobalCISOBTLead.Any(val => a.LocalDigitalOBTLead.Equals(val)) || Year_GlobalCISOBTLead.Any(val => a.GlobalCISPortraitLead.Equals(val))
                                        || Year_GlobalCISOBTLead.Any(val => a.RegionalCISPortraitLead.Equals(val)) || Year_GlobalCISOBTLead.Any(val => a.GlobalCISHRFeedSpecialist.Equals(val))
                                        where Year_Region.Any(val => a.Region.Equals(val))
                                        select a).ToList();
                var data = (from a in clrdata_filtered
                            group a by a.GoLiveYear into g
                            select new
                            {
                                Year = g.Key == "---" || g.Key == "" || g.Key == null ? "(Blanks)" : g.Key ?? "(Blanks)",
                                EMEA = clrdata_filtered.Where(x => x.GoLiveYear == g.Key && x.Region == "EMEA").Count(),
                                APAC = clrdata_filtered.Where(x => x.GoLiveYear == g.Key && x.Region == "APAC").Count(),
                                NORAM = clrdata_filtered.Where(x => x.GoLiveYear == g.Key && x.Region == "NORAM").Count(),
                                LATAM = clrdata_filtered.Where(x => x.GoLiveYear == g.Key && x.Region == "LATAM").Count(),
                            }).OrderBy(x => x.Year);
                re.Data = data;
            }
            return re;
        }
        string[] ImpT_ProjectStatus, ImpT_GoLiveYear, ImpT_Quarter, ImpT_GlobalCISOBTLead, ImpT_Region;
        [HttpPost]
        [Route("DRRegionWiseImplementationType")]
        public Response DRRegionWiseImplementationType(CLRData cLRData)
        {
            if (cLRData.ProjectStatus == null || cLRData.GoLiveYear == null || cLRData.Quarter == null || cLRData.GlobalCISOBTLead == null || cLRData.Region == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                ImpT_ProjectStatus = cLRData.ProjectStatus.Split(',');
                for (int i = 0; i < ImpT_ProjectStatus.Count(); i++)
                {
                    if (ImpT_ProjectStatus[i] == "" || ImpT_ProjectStatus[i] == "---" || ImpT_ProjectStatus[i] == "null")
                    {
                        ImpT_ProjectStatus[i] = "---";
                    }
                }
                ImpT_GoLiveYear = cLRData.GoLiveYear.Split(',');
                for (int i = 0; i < ImpT_GoLiveYear.Count(); i++)
                {
                    if (ImpT_GoLiveYear[i] == "" || ImpT_GoLiveYear[i] == "---" || ImpT_GoLiveYear[i] == "null")
                    {
                        ImpT_GoLiveYear[i] = "---";
                    }
                }
                ImpT_Quarter = cLRData.Quarter.Split(',');
                for (int i = 0; i < ImpT_Quarter.Count(); i++)
                {
                    if (ImpT_Quarter[i] == "" || ImpT_Quarter[i] == "---" || ImpT_Quarter[i] == "null")
                    {
                        ImpT_Quarter[i] = "";
                    }
                }
                ImpT_GlobalCISOBTLead = cLRData.GlobalCISOBTLead.Split(',');
                for (int i = 0; i < ImpT_GlobalCISOBTLead.Count(); i++)
                {
                    if (ImpT_GlobalCISOBTLead[i] == "" || ImpT_GlobalCISOBTLead[i] == "---" || ImpT_GlobalCISOBTLead[i] == "null")
                    {
                        ImpT_GlobalCISOBTLead[i] = "---";
                    }
                }
                ImpT_Region = cLRData.Region.Split(',');
                for (int i = 0; i < ImpT_Region.Count(); i++)
                {
                    if (ImpT_Region[i] == "" || ImpT_Region[i] == "---" || ImpT_Region[i] == "null")
                    {
                        ImpT_Region[i] = "";
                    }
                }
                var clrdata_filtered = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where ImpT_ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                                        where ImpT_Quarter.Any(val => a.Quarter.Equals(val))
                                        where ImpT_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                        where ImpT_GlobalCISOBTLead.Any(val => a.GlobalCISOBTLead.Equals(val)) || ImpT_GlobalCISOBTLead.Any(val => a.RegionalCISOBTLead.Equals(val))
                                        || ImpT_GlobalCISOBTLead.Any(val => a.LocalDigitalOBTLead.Equals(val)) || ImpT_GlobalCISOBTLead.Any(val => a.GlobalCISPortraitLead.Equals(val))
                                        || ImpT_GlobalCISOBTLead.Any(val => a.RegionalCISPortraitLead.Equals(val)) || ImpT_GlobalCISOBTLead.Any(val => a.GlobalCISHRFeedSpecialist.Equals(val))
                                        where ImpT_Region.Any(val => a.Region.Equals(val))
                                        select a).ToList();
                var data = (from a in clrdata_filtered
                            group a by a.ImplementationType into g
                            select new
                            {
                                ImplementationType = g.Key == "---" || g.Key == "" || g.Key == null ? "(Blanks)" : g.Key ?? "(Blanks)",
                                EMEA = clrdata_filtered.Where(x => x.ImplementationType == g.Key && x.Region == "EMEA").Count(),
                                APAC = clrdata_filtered.Where(x => x.ImplementationType == g.Key && x.Region == "APAC").Count(),
                                NORAM = clrdata_filtered.Where(x => x.ImplementationType == g.Key && x.Region == "NORAM").Count(),
                                LATAM = clrdata_filtered.Where(x => x.ImplementationType == g.Key && x.Region == "LATAM").Count(),
                                GrandTotal = clrdata_filtered.Where(x => x.ImplementationType == g.Key).Count(),
                            }).OrderByDescending(x => x.GrandTotal);
                re.Data = data;
            }
            return re;
        }
        string[] CR_ProjectStatus, CR_GoLiveYear, CR_Quarter, CR_GlobalCISOBTLead, CR_Region;
        [HttpPost]
        [Route("DRRegionWiseCountryStatus")]
        public Response DRRegionWiseCountryStatus(CLRData cLRData)
        {
            if (cLRData.ProjectStatus == null || cLRData.GoLiveYear == null || cLRData.Quarter == null || cLRData.GlobalCISOBTLead == null || cLRData.Region == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                CR_ProjectStatus = cLRData.ProjectStatus.Split(',');
                for (int i = 0; i < CR_ProjectStatus.Count(); i++)
                {
                    if (CR_ProjectStatus[i] == "" || CR_ProjectStatus[i] == "---" || CR_ProjectStatus[i] == "null")
                    {
                        CR_ProjectStatus[i] = "---";
                    }
                }
                CR_GoLiveYear = cLRData.GoLiveYear.Split(',');
                for (int i = 0; i < CR_GoLiveYear.Count(); i++)
                {
                    if (CR_GoLiveYear[i] == "" || CR_GoLiveYear[i] == "---" || CR_GoLiveYear[i] == "null")
                    {
                        CR_GoLiveYear[i] = "---";
                    }
                }
                CR_Quarter = cLRData.Quarter.Split(',');
                for (int i = 0; i < CR_Quarter.Count(); i++)
                {
                    if (CR_Quarter[i] == "" || CR_Quarter[i] == "---" || CR_Quarter[i] == "null")
                    {
                        CR_Quarter[i] = "";
                    }
                }
                CR_GlobalCISOBTLead = cLRData.GlobalCISOBTLead.Split(',');
                for (int i = 0; i < CR_GlobalCISOBTLead.Count(); i++)
                {
                    if (CR_GlobalCISOBTLead[i] == "" || CR_GlobalCISOBTLead[i] == "---" || CR_GlobalCISOBTLead[i] == "null")
                    {
                        CR_GlobalCISOBTLead[i] = "---";
                    }
                }
                CR_Region = cLRData.Region.Split(',');
                for (int i = 0; i < CR_Region.Count(); i++)
                {
                    if (CR_Region[i] == "" || CR_Region[i] == "---" || CR_Region[i] == "null")
                    {
                        CR_Region[i] = "";
                    }
                }
                var clrdata_filtered = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where CR_ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                                        where CR_Quarter.Any(val => a.Quarter.Equals(val))
                                        where CR_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                        where CR_GlobalCISOBTLead.Any(val => a.GlobalCISOBTLead.Equals(val)) || CR_GlobalCISOBTLead.Any(val => a.RegionalCISOBTLead.Equals(val))
                                        || CR_GlobalCISOBTLead.Any(val => a.LocalDigitalOBTLead.Equals(val)) || CR_GlobalCISOBTLead.Any(val => a.GlobalCISPortraitLead.Equals(val))
                                        || CR_GlobalCISOBTLead.Any(val => a.RegionalCISPortraitLead.Equals(val)) || CR_GlobalCISOBTLead.Any(val => a.GlobalCISHRFeedSpecialist.Equals(val))
                                        where CR_Region.Any(val => a.Region.Equals(val))
                                        select a).ToList();
                var data = (from a in clrdata_filtered
                            group a by a.CountryStatus into g
                            select new
                            {
                                CountryStatus = g.Key == "---" || g.Key == "" || g.Key == null ? "(Blanks)" : g.Key ?? "(Blanks)",
                                EMEA = clrdata_filtered.Where(x => x.CountryStatus == g.Key && x.Region == "EMEA").Count(),
                                APAC = clrdata_filtered.Where(x => x.CountryStatus == g.Key && x.Region == "APAC").Count(),
                                NORAM = clrdata_filtered.Where(x => x.CountryStatus == g.Key && x.Region == "NORAM").Count(),
                                LATAM = clrdata_filtered.Where(x => x.CountryStatus == g.Key && x.Region == "LATAM").Count(),
                            }).OrderBy(x => x.CountryStatus);
                re.Data = data;
            }
            return re;
        }
        string[] OR_ProjectStatus, OR_GoLiveYear, OR_Quarter, OR_GlobalCISOBTLead, OR_Region;
        [HttpPost]
        [Route("DRRegionWiseOBTReseller")]
        public Response DRRegionWiseOBTReseller(CLRData cLRData)
        {
            if (cLRData.ProjectStatus == null || cLRData.GoLiveYear == null || cLRData.Quarter == null || cLRData.GlobalCISOBTLead == null || cLRData.Region == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                OR_ProjectStatus = cLRData.ProjectStatus.Split(',');
                for (int i = 0; i < OR_ProjectStatus.Count(); i++)
                {
                    if (OR_ProjectStatus[i] == "" || OR_ProjectStatus[i] == "---" || OR_ProjectStatus[i] == "null")
                    {
                        OR_ProjectStatus[i] = "---";
                    }
                }
                OR_GoLiveYear = cLRData.GoLiveYear.Split(',');
                for (int i = 0; i < OR_GoLiveYear.Count(); i++)
                {
                    if (OR_GoLiveYear[i] == "" || OR_GoLiveYear[i] == "---" || OR_GoLiveYear[i] == "null")
                    {
                        OR_GoLiveYear[i] = "---";
                    }
                }
                OR_Quarter = cLRData.Quarter.Split(',');
                for (int i = 0; i < OR_Quarter.Count(); i++)
                {
                    if (OR_Quarter[i] == "" || OR_Quarter[i] == "---" || OR_Quarter[i] == "null")
                    {
                        OR_Quarter[i] = "";
                    }
                }
                OR_GlobalCISOBTLead = cLRData.GlobalCISOBTLead.Split(',');
                for (int i = 0; i < OR_GlobalCISOBTLead.Count(); i++)
                {
                    if (OR_GlobalCISOBTLead[i] == "" || OR_GlobalCISOBTLead[i] == "---" || OR_GlobalCISOBTLead[i] == "null")
                    {
                        OR_GlobalCISOBTLead[i] = "---";
                    }
                }
                OR_Region = cLRData.Region.Split(',');
                for (int i = 0; i < OR_Region.Count(); i++)
                {
                    if (OR_Region[i] == "" || OR_Region[i] == "---" || OR_Region[i] == "null")
                    {
                        OR_Region[i] = "";
                    }
                }
                var clrdata_filtered = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where OR_ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                                        where OR_Quarter.Any(val => a.Quarter.Equals(val))
                                        where OR_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                        where OR_GlobalCISOBTLead.Any(val => a.GlobalCISOBTLead.Equals(val)) || OR_GlobalCISOBTLead.Any(val => a.RegionalCISOBTLead.Equals(val))
                                        || OR_GlobalCISOBTLead.Any(val => a.LocalDigitalOBTLead.Equals(val)) || OR_GlobalCISOBTLead.Any(val => a.GlobalCISPortraitLead.Equals(val))
                                        || OR_GlobalCISOBTLead.Any(val => a.RegionalCISPortraitLead.Equals(val)) || OR_GlobalCISOBTLead.Any(val => a.GlobalCISHRFeedSpecialist.Equals(val))
                                        where OR_Region.Any(val => a.Region.Equals(val))
                                        select a).ToList();
                var data = (from a in clrdata_filtered
                            group a by a.OBTReseller into g
                            select new
                            {
                                OBTReseller = g.Key == "---" || g.Key == "" || g.Key == null ? "(Blanks)"  : g.Key ?? "(Blanks)",
                                EMEA = clrdata_filtered.Where(x => x.OBTReseller == g.Key && x.Region == "EMEA").Count(),
                                APAC = clrdata_filtered.Where(x => x.OBTReseller == g.Key && x.Region == "APAC").Count(),
                                NORAM = clrdata_filtered.Where(x => x.OBTReseller == g.Key && x.Region == "NORAM").Count(),
                                LATAM = clrdata_filtered.Where(x => x.OBTReseller == g.Key && x.Region == "LATAM").Count(),
                                GrandTotal = clrdata_filtered.Where(x => x.OBTReseller == g.Key).Count(),
                            }).OrderBy(x => x.OBTReseller);
                re.Data = data;
            }
            return re;
        }
        string[] DR_ProjectStatus, DR_GoLiveYear, DR_Quarter, DR_GlobalCISOBTLead, DR_Region;
        [HttpPost]
        [Route("DigitalReportData")]
        public Response DigitalReportData(CLRData cLRData)
        {
            if (cLRData.ProjectStatus == null || cLRData.GoLiveYear == null || cLRData.Quarter == null || cLRData.GlobalCISOBTLead == null || cLRData.Region == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                DR_ProjectStatus = cLRData.ProjectStatus.Split(',');
                for (int i = 0; i < DR_ProjectStatus.Count(); i++)
                {
                    if (DR_ProjectStatus[i] == "" || DR_ProjectStatus[i] == "---" || DR_ProjectStatus[i] == "null")
                    {
                        DR_ProjectStatus[i] = "---";
                    }
                }
                DR_GoLiveYear = cLRData.GoLiveYear.Split(',');
                for (int i = 0; i < DR_GoLiveYear.Count(); i++)
                {
                    if (DR_GoLiveYear[i] == "" || DR_GoLiveYear[i] == "---" || DR_GoLiveYear[i] == "null")
                    {
                        DR_GoLiveYear[i] = "---";
                    }
                }
                DR_Quarter = cLRData.Quarter.Split(',');
                for (int i = 0; i < DR_Quarter.Count(); i++)
                {
                    if (DR_Quarter[i] == "" || DR_Quarter[i] == "---" || DR_Quarter[i] == "null")
                    {
                        DR_Quarter[i] = "";
                    }
                }
                DR_GlobalCISOBTLead = cLRData.GlobalCISOBTLead.Split(',');
                for (int i = 0; i < DR_GlobalCISOBTLead.Count(); i++)
                {
                    if (DR_GlobalCISOBTLead[i] == "" || DR_GlobalCISOBTLead[i] == "---" || DR_GlobalCISOBTLead[i] == "null")
                    {
                        DR_GlobalCISOBTLead[i] = "---";
                    }
                }
                DR_Region = cLRData.Region.Split(',');
                for (int i = 0; i < DR_Region.Count(); i++)
                {
                    if (DR_Region[i] == "" || DR_Region[i] == "---" || DR_Region[i] == "null")
                    {
                        DR_Region[i] = "";
                    }
                }
                var clrdata_filtered = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        where DR_ProjectStatus.Any(val => a.ProjectStatus.Equals(val))
                                        where DR_Quarter.Any(val => a.Quarter.Equals(val))
                                        where DR_GoLiveYear.Any(val => a.GoLiveYear.Equals(val))
                                        where DR_GlobalCISOBTLead.Any(val => a.GlobalCISOBTLead.Equals(val))
                                        where DR_Region.Any(val => a.Region.Equals(val))
                                        select a).ToList();
                var Datalist4 = (from a in clrdata_filtered
                                 where a.RevenueID > 600000000000000
                                 join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new
                                 {
                                     a.CLRID,
                                     ManualID = abc.ManualID,
                                     Revenue_ID = a.RevenueID,
                                     Client = a.Client == "" ? "---" : a.Client ?? "---",
                                     iMeet_Workspace_Title = "---",
                                     Implementation_Type = "---",
                                     OBT_Reseller___Direct = "---",
                                     Region = a.Region == "" ? "---" : a.Region ?? "---",
                                     Country = a.Country == "" ? "---" : a.Country ?? "---",
                                     a.RevenueID,
                                     ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                     CountryStatus = "---",
                                     GlobalCISOBTLead = a.GlobalCISOBTLead == "" ? "---" : a.GlobalCISOBTLead ?? "---",
                                     RegionalCISOBTLead = a.RegionalCISOBTLead == "" ? "---" : a.RegionalCISOBTLead ?? "---",
                                     LocalDigitalOBTLead = a.LocalDigitalOBTLead == "" ? "---" : a.LocalDigitalOBTLead ?? "---",
                                     GlobalCISPortraitLead = a.GlobalCISPortraitLead == "" ? "---" : a.GlobalCISPortraitLead ?? "---",
                                     RegionalCISPortraitLead = a.RegionalCISPortraitLead == "" ? "---" : a.RegionalCISPortraitLead ?? "---",
                                     GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist == "" ? "---" : a.GlobalCISHRFeedSpecialist ?? "---",
                                     Account_Name = "---",
                                     GoLiveYear = a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                     Quarter = a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     ImplementationType = "---",
                                     RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                                     a.DataSourceType,
                                     ActivityType = a.DigitalActivityType == "" ? "---" : a.DigitalActivityType ?? "---",
                                     GDS = a.DigitalGDS == "" ? "---" : a.DigitalGDS ?? "---",
                                     DTID = entity.AdHocProjects.FirstOrDefault(x => x.RevenueID == a.RevenueID).AHID,
                                     ComplexityScore = entity.AdHocProjects.FirstOrDefault(x => x.RevenueID == a.RevenueID).ComplexityScore ?? 0,
                                 }).ToList();
                var Datalist1 = (from a in clrdata_filtered
                                 where a.ProjectStatus != "P-Pipeline"
                                 where a.ProjectStatus != "HP-High Potential"
                                 where a.ProjectStatus != "EP-Early Potential"
                                 where a.RevenueID != 400000000000000
                                 where a.RevenueID < 600000000000000
                                 join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new
                                 {
                                     a.CLRID,
                                     ManualID = abc.ManualID,
                                     Revenue_ID = abc.Revenue_ID ?? 0,
                                     Client = a.Client == "" ? "---" : a.Client ?? "---",
                                     iMeet_Workspace_Title = a.Workspace_Title == "" ? "---" : a.Workspace_Title ?? "---",
                                     Implementation_Type = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                     OBT_Reseller___Direct = abc.OBT_Reseller___Direct == "" ? "---" : abc.OBT_Reseller___Direct ?? "---",
                                     Region = a.Region == "" ? "---" : a.Region ?? "---",
                                     Country = a.Country == "" ? "---" : a.Country ?? "---",
                                     a.RevenueID,
                                     ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                     CountryStatus = a.CountryStatus == "" ? "---" : a.CountryStatus ?? "---",
                                     GlobalCISOBTLead = a.GlobalCISOBTLead == "" ? "---" : a.GlobalCISOBTLead ?? "---",
                                     RegionalCISOBTLead = a.RegionalCISOBTLead == "" ? "---" : a.RegionalCISOBTLead ?? "---",
                                     LocalDigitalOBTLead = a.LocalDigitalOBTLead == "" ? "---" : a.LocalDigitalOBTLead ?? "---",
                                     GlobalCISPortraitLead = a.GlobalCISPortraitLead == "" ? "---" : a.GlobalCISPortraitLead ?? "---",
                                     RegionalCISPortraitLead = a.RegionalCISPortraitLead == "" ? "---" : a.RegionalCISPortraitLead ?? "---",
                                     GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist == "" ? "---" : a.GlobalCISHRFeedSpecialist ?? "---",
                                     Account_Name = a.Account_Name == "" ? "---" : a.Account_Name ?? "---",
                                     GoLiveYear = a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                     Quarter = a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     ImplementationType = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                     RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                                     a.DataSourceType,
                                     ActivityType = a.DigitalActivityType == "" ? "---" : a.DigitalActivityType ?? "---",
                                     GDS = a.DigitalGDS == "" ? "---" : a.DigitalGDS ?? "---",
                                     entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).DTID,
                                     ComplexityScore = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).ComplexityScore,
                                 }).ToList();
                var Datalist3 = (from a in clrdata_filtered
                                 where a.RevenueID == 400000000000000
                                 join b in entity.ManualDatas on a.Task__Task_Record_ID_Key equals b.TaskRecordIdKey into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new
                                 {
                                     a.CLRID,
                                     ManualID = abc.ManualID,
                                     Revenue_ID = abc.Revenue_ID ?? 0,
                                     Client = a.Client == "" ? "---" : a.Client ?? "---",
                                     iMeet_Workspace_Title = a.Workspace_Title == "" ? "---" : a.Workspace_Title ?? "---",
                                     Implementation_Type = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                     OBT_Reseller___Direct = abc.OBT_Reseller___Direct == "" ? "---" : abc.OBT_Reseller___Direct ?? "---",
                                     Region = a.Region == "" ? "---" : a.Region ?? "---",
                                     Country = a.Country == "" ? "---" : a.Country ?? "---",
                                     a.RevenueID,
                                     ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                     CountryStatus = a.CountryStatus == "" ? "---" : a.CountryStatus ?? "---",
                                     GlobalCISOBTLead = a.GlobalCISOBTLead == "" ? "---" : a.GlobalCISOBTLead ?? "---",
                                     RegionalCISOBTLead = a.RegionalCISOBTLead == "" ? "---" : a.RegionalCISOBTLead ?? "---",
                                     LocalDigitalOBTLead = a.LocalDigitalOBTLead == "" ? "---" : a.LocalDigitalOBTLead ?? "---",
                                     GlobalCISPortraitLead = a.GlobalCISPortraitLead == "" ? "---" : a.GlobalCISPortraitLead ?? "---",
                                     RegionalCISPortraitLead = a.RegionalCISPortraitLead == "" ? "---" : a.RegionalCISPortraitLead ?? "---",
                                     GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist == "" ? "---" : a.GlobalCISHRFeedSpecialist ?? "---",
                                     Account_Name = a.Account_Name == "" ? "---" : a.Account_Name ?? "---",
                                     GoLiveYear = a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                     Quarter = a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     ImplementationType = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                     RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                                     a.DataSourceType,
                                     ActivityType = a.DigitalActivityType == "" ? "---" : a.DigitalActivityType ?? "---",
                                     GDS = a.DigitalGDS == "" ? "---" : a.DigitalGDS ?? "---",
                                     entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).DTID,
                                     ComplexityScore = entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).ComplexityScore,
                                 }).ToList();
                var Datalist2 = (from a in clrdata_filtered
                                 where a.ProjectStatus == "P-Pipeline" || a.ProjectStatus == "HP-High Potential" || a.ProjectStatus == "EP-Early Potential"
                                 where a.RevenueID != 400000000000000
                                 where a.RevenueID < 600000000000000
                                 join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new
                                 {
                                     a.CLRID,
                                     ManualID = abc.ManualID,
                                     Revenue_ID = abc.Revenue_ID ?? 0,
                                     Client = a.Client == "" ? "---" : a.Client ?? "---",
                                     iMeet_Workspace_Title = abc.iMeet_Workspace_Title == "" ? "---" : abc.iMeet_Workspace_Title ?? "---",
                                     Implementation_Type = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                     OBT_Reseller___Direct = abc.OBT_Reseller___Direct == "" ? "---" : abc.OBT_Reseller___Direct ?? "---",
                                     Region = a.Region == "" ? "---" : a.Region ?? "---",
                                     Country = a.Country == "" ? "---" : a.Country ?? "---",
                                     a.RevenueID,
                                     ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                     CountryStatus = a.CountryStatus == "" ? "---" : a.CountryStatus ?? "---",
                                     GlobalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISOBTLead ?? "---",
                                     RegionalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISOBTLead ?? "---",
                                     LocalDigitalOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).LocalDigitalOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).LocalDigitalOBTLead ?? "---",
                                     GlobalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISPortraitLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISPortraitLead ?? "---",
                                     RegionalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISPortraitLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISPortraitLead ?? "---",
                                     GlobalCISHRFeedSpecialist = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISHRFeedSpecialist == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISHRFeedSpecialist ?? "---",
                                     Account_Name = a.Account_Name == "" ? "---" : a.Account_Name ?? "---",
                                     GoLiveYear = a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                     Quarter = a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     ImplementationType = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                     RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                                     a.DataSourceType,
                                     ActivityType = a.DigitalActivityType == "" ? "---" : a.DigitalActivityType ?? "---",
                                     GDS = a.DigitalGDS == "" ? "---" : a.DigitalGDS ?? "---",
                                     entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).DTID,
                                     ComplexityScore = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).ComplexityScore,
                                 }).ToList();
                var Datalist = Datalist3.Concat(Datalist1).Concat(Datalist4).Concat(Datalist2);
                CLRDataCount = Datalist.AsQueryable().Count();
                if (CLRDataCount.ToString() == "" || CLRDataCount.ToString() == null || CLRDataCount == 0)
                {
                    re.Data = Datalist;
                    re.code = 100;
                    re.message = "No Data found";
                }
                else
                {
                    re.Data = Datalist;
                    re.code = 200;
                    re.message = "Data Successfull";
                }
            }
            return re;
        }
        int CLRDataCount;
    }
}
