using CWTDashboardAPI.Models;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CWTDashboardAPI.Controllers
{
    public class PrioritizationController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        H_Filters h_f = new H_Filters();
        Response re = new Response();
        AutomatedCLRFilters CLR_F = new AutomatedCLRFilters();
        HierarchyGraphs hg = new HierarchyGraphs();
        [HttpPost]
        [Route("PrioritizationFilters")]
        public AutomatedCLRFilters PrioritizationFilters(CLRData cLRData)
        {
            var FilterYearMonth = (from a in entity.CLRDatas
                                    where a.Status != "Active"
                                    select new
                                    {
                                        Year = a.YearMonth == null || a.YearMonth == "" ? "---" : a.YearMonth ?? "---",
                                        isSelected = true,
                                    }).Distinct().OrderBy(x => x.Year);
            var FilterRegion = (from a in entity.CLRDatas
                                where a.Status == "Active"
                                select new
                                 {
                                     Region = a.Region == null || a.Region == "" ? "---" : a.Region ?? "---",
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.Region);
            var FilterLine_Win_Probability = (from a in entity.CLRDatas
                                              where a.Status == "Active"
                                              select new
                                              {
                                                  Line_Win_Probability = a.Line_Win_Probability == null ? 0 : a.Line_Win_Probability ?? 0,
                                                  isSelected = true,
                                              }).Distinct().OrderBy(x => x.Line_Win_Probability);
            var FilterOwnerShip = (from a in entity.CLRDatas
                                    where a.Status == "Active"
                                    select new
                                    {
                                        OwnerShip = a.OwnerShip == null || a.OwnerShip == "" ? "---" : a.OwnerShip ?? "---",
                                        isSelected = true,
                                    }).Distinct().OrderBy(x => x.OwnerShip);
            CLR_F.FilterRegion = FilterRegion;
            CLR_F.FilterOwnerShip = FilterOwnerShip;
            CLR_F.FilterLine_Win_Probability = FilterLine_Win_Probability;
            CLR_F.FilterYears = FilterYearMonth;
            CLR_F.code = 200;
            CLR_F.message = "Success";
            return CLR_F;
        }

        string[] YearMonth, Regions, LineWin, OwnerShip;
        [HttpPost]
        [Route("PrioritizationData")]
        public Response PrioritizationData(CLRData cLRData)
        {
            if (cLRData.YearMonth == null || cLRData.Region == null || cLRData.Country == null || cLRData.OwnerShip == null)
            {
                re.Data = "";
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                YearMonth = cLRData.YearMonth.Split(',');
                for (int i = 0; i < YearMonth.Count(); i++)
                {
                    if (YearMonth[i] == "" || YearMonth[i] == "---" || YearMonth[i] == "null")
                    {
                        YearMonth[i] = "";
                    }
                }
                Regions = cLRData.Region.Split(',');
                for (int i = 0; i < Regions.Count(); i++)
                {
                    if (Regions[i] == "" || Regions[i] == "---" || Regions[i] == "null")
                    {
                        Regions[i] = "";
                    }
                }
                LineWin = cLRData.Country.Split(',');
                for (int i = 0; i < LineWin.Count(); i++)
                {
                    if (LineWin[i] == "" || LineWin[i] == "---" || LineWin[i] == "null")
                    {
                        LineWin[i] = "";
                    }
                }
                OwnerShip = cLRData.OwnerShip.Split(',');
                for (int i = 0; i < OwnerShip.Count(); i++)
                {
                    if (OwnerShip[i] == "" || OwnerShip[i] == "---" || OwnerShip[i] == "null")
                    {
                        OwnerShip[i] = "";
                    }
                }
                var FilteredCLRData = (from a in entity.CLRDatas
                                       where a.Status == "Active"
                                       //where LineWin.Any(val => a.Line_Win_Probability.Equals(val))
                                       where YearMonth.Any(val => a.YearMonth.Equals(val))
                                       where Regions.Any(val => a.Region.Equals(val))
                                       where OwnerShip.Any(val => a.OwnerShip.Equals(val))
                                       select a);
                var GlobalData = (from a in entity.CLRDatas
                                  where a.Status == "Active"
                                  //where LineWin.Any(val => a.Line_Win_Probability.Equals(val))
                                  where YearMonth.Any(val => a.YearMonth.Equals(val))
                                  where Regions.Any(val => a.Region.Equals(val))
                                  where OwnerShip.Any(val => a.OwnerShip.Equals(val))
                                  group a by a.ProjectStatus into g
                                  select new
                                  {
                                      ProjectStatus = g.Key,
                                      RevenueVolume = FilteredCLRData.Where(x => x.ProjectStatus == g.Key).Sum(x=>x.RevenueVolumeUSD) ?? 0
                                  }).ToList().Distinct();
                re.GlobalManager = GlobalData;
                //re.RegionalManager = ManagerWiseData.Where(x => x.ClientCount_RM > 0);
                //re.LocalManager = ManagerWiseData.Where(x => x.ClientCount_LM > 0);
                re.code = 200;
                re.message = "Success";
            }
            return re;
        }

        [HttpPost]
        [Route("GetDirFiles")]
        public Response GetDirFiles(CLRData cLRData)
        {
            string downloadsPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");

            if (!Directory.Exists(downloadsPath))
            {
                re.code = 100;
                re.message = "Downloads Folder not found";
            }
            else
            {
                var filesToDelete = Directory.GetFiles(downloadsPath, "*.xlsx", SearchOption.TopDirectoryOnly)
                                     .Where(file =>
                                     {
                                         string fileName = Path.GetFileName(file);
                                         bool startsWithClr = fileName.StartsWith("clr_imeet_3_0", StringComparison.OrdinalIgnoreCase);
                                         DateTime lastModified = File.GetLastWriteTime(file);
                                         bool isToday = lastModified.Date == DateTime.Today;
                                         return startsWithClr && isToday;
                                     }).OrderByDescending(file => File.GetLastWriteTime(file)).ToList();
                if (filesToDelete.Any())
                {
                    var filename = Path.GetFileName(filesToDelete[0]);
                    var filepath = filesToDelete[0]; // most recently modified file
                    re.code = 200;
                    re.message = "File Path : " + filepath + " File Name : " + filename;
                    re.Data = filesToDelete;
                }
                else
                {
                    re.code = 101;
                    re.message = "No matching files found today.";
                }
            }
            return re;

        }
    }
}
