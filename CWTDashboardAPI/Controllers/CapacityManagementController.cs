using CWTDashboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CWTDashboardAPI.Controllers
{
    public class CapacityManagementController : ApiController
    {
        //CWTDashboardEntities entity = new CWTDashboardEntities();
        //Response re = new Response();
        //CapacityFilters CF = new CapacityFilters();
        //[HttpPost]
        //[Route("CapacityFiltersList")]
        //public CapacityFilters CapacityFiltersList(CLRData clr)
        //{
        //    var FilterProjectStatus = (from a in entity.CLRDatas
        //                               where a.iMeet_Milestone___Project_Status != null || a.iMeet_Milestone___Project_Status != ""
        //                               select new
        //                               {
        //                                   a.iMeet_Milestone___Project_Status,
        //                                   isSelected = true,
        //                               }).Distinct().OrderBy(x => x.iMeet_Milestone___Project_Status);
        //    var FilterProjectLevel = (from a in entity.CLRDatas
        //                              where a.iMeet_Project_Level != null
        //                              select new
        //                              {
        //                                  a.iMeet_Project_Level,
        //                                  isSelected = true,
        //                              }).Distinct().OrderBy(x => x.iMeet_Project_Level);
        //    var FilterRegion = (from a in entity.CLRDatas
        //                        select new
        //                            {
        //                                a.Region__Opportunity_,
        //                                isSelected = true,
        //                            }).Distinct().OrderBy(x => x.Region__Opportunity_);
        //    var FilterYears = (from a in entity.CLRDatas
        //                       where a.Go_Live_Year != "1900"
        //                           where a.Go_Live_Year != ""
        //                           where a.Go_Live_Year != "2016"
        //                           where a.Go_Live_Year != "2017"
        //                           where a.Go_Live_Year != "2018"
        //                            select new
        //                            {
        //                                a.Go_Live_Year,
        //                                isSelected = true,
        //                            }).Distinct().OrderBy(x => x.Go_Live_Year);
        //    var FilterQuarter = (from a in entity.CLRDatas
        //                         where a.Quarter != null
        //                             select new
        //                             {
        //                                 a.Quarter,
        //                                 isSelected = true,
        //                             }).Distinct().OrderBy(x => x.Quarter);
        //    var FilterProjectSum = (from a in entity.CLRDatas
        //                            where a.Project_Sum != null 
        //                           select new
        //                           {
        //                               a.Project_Sum,
        //                               isSelected = true,
        //                           }).Distinct().OrderBy(x => x.Project_Sum);
        //    var FilterGlobalPL = (from a in entity.CLRDatas
        //                          where a.Global_PL_Leader != null
        //                              select new
        //                              {
        //                                  a.Global_PL_Leader,
        //                                  isSelected = true,
        //                              }).Distinct().OrderBy(x => x.Global_PL_Leader);
        //    var FilterRegionalPL = (from a in entity.CLRDatas
        //                            where a.Regional_PL_Leader != null
        //                              select new
        //                              {
        //                                  a.Regional_PL_Leader,
        //                                  isSelected = true,
        //                              }).Distinct().OrderBy(x => x.Regional_PL_Leader);
        //    var FilterLocalPL = (from a in entity.CLRDatas
        //                         where a.Local_PL_Leader != null
        //                            select new
        //                            {
        //                                a.Local_PL_Leader,
        //                                isSelected = true,
        //                            }).Distinct().OrderBy(x => x.Local_PL_Leader);
        //    var FilterGlobalPM = (from a in entity.CLRDatas
        //                          where a.Global_Project_Manager != null
        //                             select new
        //                             {
        //                                 a.Global_Project_Manager,
        //                                 isSelected = true,
        //                             }).Distinct().OrderBy(x => x.Global_Project_Manager);
        //    var FilterRegionalPM = (from a in entity.CLRDatas
        //                            where a.Regional_Project_Manager != null
        //                             select new
        //                             {
        //                                 a.Regional_Project_Manager,
        //                                 isSelected = true,
        //                             }).Distinct().OrderBy(x => x.Regional_Project_Manager);
        //    CF.code = 200;
        //    CF.message = "Success";
        //    CF.ProjectStatus = FilterProjectStatus;
        //    CF.ProjectLevel = FilterProjectLevel;
        //    CF.Region = FilterRegion;
        //    CF.Years = FilterYears;
        //    CF.Quarter = FilterQuarter;
        //    CF.ProjectSum = FilterProjectSum;
        //    CF.GlobalPL = FilterGlobalPL;
        //    CF.RegionalPL = FilterRegionalPL;
        //    CF.LocalPL = FilterLocalPL;
        //    CF.GlobalPM = FilterGlobalPM;
        //    CF.RegionalPM = FilterRegionalPM;
        //    return CF;
        //}
        //string[] status_ByC,projectlvl_Byc,Region_Byc,Years_Byc, GlobalPL_C, RegionalPL_C, LocalPL_C, GlobalPM_C, RegionalPM_C;
        //[HttpPost]
        //[Route("CapacityByClient")]
        //public Response CapacityByClient(CLRData clr)
        //{
        //    if (clr.ProjectStatus == null || clr.ProjectLevel== null || clr.Region == null || clr.GoLiveYear == null || clr.GlobalProjectManager == null || clr.RegionalProjectManager == null || clr.AssigneeFullName == null || clr.Global_Project_Manager == null || clr.Regional_Project_Manager == null)
        //    {
        //        re.code = 100;
        //        re.message = "Please select the values";
        //        re.Data = null;
        //    }
        //    else
        //    {
        //        projectlvl_Byc = clr.ProjectLevel.Split(',');
        //        for (int i = 0; i < projectlvl_Byc.Count(); i++)
        //        {
        //            if (projectlvl_Byc[i] == "")
        //            {
        //                projectlvl_Byc[i] = null;
        //            }
        //        }
        //        Region_Byc = clr.Region__Opportunity_.Split(',');
        //        for (int i = 0; i < Region_Byc.Count(); i++)
        //        {
        //            if (Region_Byc[i] == "")
        //            {
        //                Region_Byc[i] = null;
        //            }
        //        }
        //        Years_Byc = clr.Go_Live_Year.Split(',');
        //        for (int i = 0; i < Years_Byc.Count(); i++)
        //        {
        //            if (Years_Byc[i] == "")
        //            {
        //                Years_Byc[i] = null;
        //            }
        //        }
        //        //ProjectSum_c = clr.Project_Sum.Split(',');
        //        //for (int i = 0; i < ProjectSum_c.Count(); i++)
        //        //{
        //        //    if (ProjectSum_c[i] == "")
        //        //    {
        //        //        ProjectSum_c[i] = null;
        //        //    }
        //        //}
        //        status_ByC = clr.iMeet_Milestone___Project_Status.Split(',');
        //        for (int i = 0; i < status_ByC.Count(); i++)
        //        {
        //            if (status_ByC[i] == "" || status_ByC[i] == "null")
        //            {
        //                status_ByC[i] = null;
        //            }
        //        }
        //        GlobalPL_C = clr.Global_PL_Leader.Split(',');
        //        for (int i = 0; i < GlobalPL_C.Count(); i++)
        //        {
        //            if (GlobalPL_C[i] == "" || GlobalPL_C[i] == "null")
        //            {
        //                GlobalPL_C[i] = null;
        //            }
        //        }
        //        RegionalPL_C = clr.Regional_PL_Leader.Split(',');
        //        for (int i = 0; i < RegionalPL_C.Count(); i++)
        //        {
        //            if (RegionalPL_C[i] == "" || RegionalPL_C[i] == "null")
        //            {
        //                RegionalPL_C[i] = null;
        //            }
        //        }
        //        LocalPL_C = clr.Local_PL_Leader.Split(',');
        //        for (int i = 0; i < LocalPL_C.Count(); i++)
        //        {
        //            if (LocalPL_C[i] == "" || LocalPL_C[i] == "null")
        //            {
        //                LocalPL_C[i] = null;
        //            }
        //        }
        //        GlobalPM_C = clr.Global_Project_Manager.Split(',');
        //        for (int i = 0; i < GlobalPM_C.Count(); i++)
        //        {
        //            if (GlobalPM_C[i] == "" || GlobalPM_C[i] == "null")
        //            {
        //                GlobalPM_C[i] = null;
        //            }
        //        }
        //        RegionalPM_C = clr.Regional_Project_Manager.Split(',');
        //        for (int i = 0; i < RegionalPM_C.Count(); i++)
        //        {
        //            if (RegionalPM_C[i] == "" || RegionalPM_C[i] == "null")
        //            {
        //                RegionalPM_C[i] = null;
        //            }
        //        }
        //        //status_ByC = "A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
        //        var CapacityByClient = (from a in entity.CLRs
        //                                where status_ByC.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
        //                                where projectlvl_Byc.Any(val2 => a.iMeet_Project_Level.Equals(val2))
        //                                where Region_Byc.Any(val3 => a.Region__Opportunity_.Equals(val3))
        //                                where Years_Byc.Any(val4 => a.Go_Live_Year.Equals(val4))
        //                                where GlobalPL_C.Any(val5 => a.Global_PL_Leader.Equals(val5))
        //                                where RegionalPL_C.Any(val5 => a.Regional_PL_Leader.Equals(val5))
        //                                where LocalPL_C.Any(val5 => a.Local_PL_Leader.Equals(val5))
        //                                where GlobalPM_C.Any(val5 => a.Global_Project_Manager.Equals(val5))
        //                                where RegionalPM_C.Any(val5 => a.Regional_Project_Manager.Equals(val5))
        //                                select new
        //                                {
        //                                    a.Region__Opportunity_,
        //                                    a.Global_PL_Leader,
        //                                    a.Regional_PL_Leader,
        //                                    a.Local_PL_Leader,
        //                                    a.Client,
        //                                    a.iMeet_Milestone___Project_Status,
        //                                    May1st = a.C06_May == null ? 0 : a.C06_May,
        //                                    May2nd = a.C13_May == null ? 0 : a.C13_May,
        //                                    May3rd = a.C20_May == null ? 0 : a.C20_May,
        //                                    May4th = a.C27_May == null ? 0 : a.C27_May,
        //                                    June1st = a.C03_Jun == null ? 0 : a.C03_Jun,
        //                                    June2nd = a.C10_Jun == null ? 0 : a.C10_Jun,
        //                                    June3rd = a.C17_Jun == null ? 0 : a.C17_Jun,
        //                                    June4th = a.C24_Jun == null ? 0 : a.C24_Jun,
        //                                    July1st = a.C01_Jul == null ? 0 : a.C01_Jul,
        //                                    July2nd = a.C08_Jul == null ? 0 : a.C08_Jul,
        //                                    July3rd = a.C15_Jul == null ? 0 : a.C15_Jul,
        //                                    July4th = a.C22_Jul == null ? 0 : a.C22_Jul,
        //                                    July5th = a.C29_Jul == null ? 0 : a.C29_Jul,
        //                                    August1st = a.C05_Aug == null ? 0 : a.C05_Aug,
        //                                    August2nd = a.C12_Aug == null ? 0 : a.C12_Aug,
        //                                    August3rd = a.C19_Aug == null ? 0 : a.C19_Aug,
        //                                    August4th = a.C26_Aug == null ? 0 : a.C26_Aug,
        //                                    September1st = a.C02_Sep == null ? 0 : a.C02_Sep,
        //                                    September2nd = a.C09_Sep == null ? 0 : a.C09_Sep,
        //                                    September3rd = a.C16_Sep == null ? 0 : a.C16_Sep,
        //                                    September4th = a.C23_Sep == null ? 0 : a.C23_Sep,
        //                                    September5th = a.C30_Sep == null ? 0 : a.C30_Sep,
        //                                    October1st = a.C07_Oct == null ? 0 : a.C07_Oct,
        //                                    October2nd = a.C14_Oct == null ? 0 : a.C14_Oct,
        //                                    October3rd = a.C21_Oct == null ? 0 : a.C21_Oct,
        //                                    October4th = a.C28_Oct == null ? 0 : a.C28_Oct,
        //                                    Novomber1st = a.C04_Nov == null ? 0 : a.C04_Nov,
        //                                    November2nd = a.C11_Nov == null ? 0 : a.C11_Nov,
        //                                    November3rd = a.C18_Nov == null ? 0 : a.C18_Nov,
        //                                    November4th = a.C25_Nov == null ? 0 : a.C25_Nov,
        //                                    December1st = a.C02_Dec == null ? 0 : a.C02_Dec,
        //                                    December2nd = a.C09_Dec == null ? 0 : a.C09_Dec,
        //                                    December3rd = a.C16_Dec == null ? 0 : a.C16_Dec,
        //                                    December4th = a.C23_Dec == null ? 0 : a.C23_Dec,
        //                                    December5th = a.C30_Dec == null ? 0 : a.C30_Dec,
        //                                    Jan1st20 = a.C01_06_2020 == null ? 0 : a.C01_06_2020,
        //                                    Jan2nd20 = a.C01_13_2020 == null ? 0 : a.C01_13_2020,
        //                                    Jan3rd20 = a.C01_20_2020 == null ? 0 : a.C01_20_2020,
        //                                    Jan4th20 = a.C01_27_2020 == null ? 0 : a.C01_27_2020,
        //                                    Feb1st20 = a.C02_03_2020 == null ? 0 : a.C02_03_2020,
        //                                    Feb2nd20 = a.C02_10_2020 == null ? 0 : a.C02_10_2020,
        //                                    Feb3rd20 = a.C02_17_2020 == null ? 0 : a.C02_17_2020,
        //                                    Feb4th20 = a.C02_24_2020 == null ? 0 : a.C02_24_2020,
        //                                    Mar1st20 = a.C03_02_2020 == null ? 0 : a.C03_02_2020,
        //                                    Mar2nd20 = a.C03_09_2020 == null ? 0 : a.C03_09_2020,
        //                                    Mar3rd20 = a.C03_16_2020 == null ? 0 : a.C03_16_2020,
        //                                    Mar4th20 = a.C03_23_2020 == null ? 0 : a.C03_23_2020,
        //                                    Mar5th20 = a.C03_30_2020 == null ? 0 : a.C03_30_2020,
        //                                    Apr1st20 = a.C04_06_2020 == null ? 0 : a.C04_06_2020,
        //                                    Apr2nd20 = a.C04_13_2020 == null ? 0 : a.C04_13_2020,
        //                                    Apr3rd20 = a.C04_20_2020 == null ? 0 : a.C04_20_2020,
        //                                    Apr4th20 = a.C04_27_2020 == null ? 0 : a.C04_27_2020,
        //                                    May1st20 = a.C05_04_2020 == null ? 0 : a.C05_04_2020,
        //                                    May2nd20 = a.C05_11_2020 == null ? 0 : a.C05_11_2020,
        //                                    May3rd20 = a.C05_18_2020 == null ? 0 : a.C05_18_2020,
        //                                    May4th20 = a.C05_25_2020 == null ? 0 : a.C05_25_2020,
        //                                    Jun1st20 = a.C06_01_2020 == null ? 0 : a.C06_01_2020,
        //                                    Jun2nd20 = a.C06_08_2020 == null ? 0 : a.C06_08_2020,
        //                                    Jun3rd20 = a.C06_15_2020 == null ? 0 : a.C06_15_2020,
        //                                    Jun4th20 = a.C06_22_2020 == null ? 0 : a.C06_22_2020,
        //                                    Jun5th20 = a.C06_29_2020 == null ? 0 : a.C06_29_2020,
        //                                    Jul1st20 = a.C07_06_2020 == null ? 0 : a.C07_06_2020,
        //                                    Jul2nd20 = a.C07_13_2020 == null ? 0 : a.C07_13_2020,
        //                                    Jul3rd20 = a.C07_20_2020 == null ? 0 : a.C07_20_2020,
        //                                    Jul4th20 = a.C07_27_2020 == null ? 0 : a.C07_27_2020,
        //                                    Aug1st20 = a.C08_03_2020 == null ? 0 : a.C08_03_2020,
        //                                    Aug2nd20 = a.C08_10_2020 == null ? 0 : a.C08_10_2020,
        //                                    Aug3rd20 = a.C08_17_2020 == null ? 0 : a.C08_17_2020,
        //                                    Aug4th20 = a.C08_24_2020 == null ? 0 : a.C08_24_2020,
        //                                    Aug5th20 = a.C08_31_2020 == null ? 0 : a.C08_31_2020,
        //                                    Sep1st20 = a.C09_07_2020 == null ? 0 : a.C09_07_2020,
        //                                    Sep2nd20 = a.C09_14_2020 == null ? 0 : a.C09_14_2020,
        //                                    Sep3rd20 = a.C09_21_2020 == null ? 0 : a.C09_21_2020,
        //                                    Sep4th20 = a.C09_28_2020 == null ? 0 : a.C09_28_2020,
        //                                    Oct1st20 = a.C10_05_2020 == null ? 0 : a.C10_05_2020,
        //                                    Oct2nd20 = a.C10_12_2020 == null ? 0 : a.C10_12_2020,
        //                                    Oct3rd20 = a.C10_19_2020 == null ? 0 : a.C10_19_2020,
        //                                    Oct4th20 = a.C10_26_2020 == null ? 0 : a.C10_26_2020,
        //                                    Nov1st20 = a.C11_02_2020 == null ? 0 : a.C11_02_2020,
        //                                    Nov2nd20 = a.C11_09_2020 == null ? 0 : a.C11_09_2020,
        //                                    Nov3rd20 = a.C11_16_2020 == null ? 0 : a.C11_16_2020,
        //                                    Nov4th20 = a.C11_23_2020 == null ? 0 : a.C11_23_2020,
        //                                    Nov5th20 = a.C11_30_2020 == null ? 0 : a.C11_30_2020,
        //                                    Dec1st20 = a.C12_07_2020 == null ? 0 : a.C12_07_2020,
        //                                    Dec2nd20 = a.C12_14_2020 == null ? 0 : a.C12_14_2020,
        //                                    Dec3rd20 = a.C12_21_2020 == null ? 0 : a.C12_21_2020,
        //                                    Dec4th20 = a.C12_28_2020 == null ? 0 : a.C12_28_2020,
        //                                }).OrderBy(x=>x.Client);
        //        re.code = 200;
        //        re.message = "Success";
        //        re.Data = CapacityByClient;
        //    }
        //    return re;
        //}

        //string[] status_ByR, projectlvl_ByR, Region_ByR, Years_ByR, GlobalPL_R, RegionalPL_R, LocalPL_R, GlobalPM_R, RegionalPM_R;
        //[HttpPost]
        //[Route("CapacityByRegion")]
        //public Response CapacityByRegion(CLR clr)
        //{
        //    if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Region__Opportunity_ == null || clr.Go_Live_Year == null)
        //    {
        //        re.code = 100;
        //        re.message = "Please select the values";
        //        re.Data = null;
        //    }
        //    else
        //    {
        //        projectlvl_ByR = clr.iMeet_Project_Level.Split(',');
        //        for (int i = 0; i < projectlvl_ByR.Count(); i++)
        //        {
        //            if (projectlvl_ByR[i] == "")
        //            {
        //                projectlvl_ByR[i] = null;
        //            }
        //        }
        //        Region_ByR = clr.Region__Opportunity_.Split(',');
        //        for (int i = 0; i < Region_ByR.Count(); i++)
        //        {
        //            if (Region_ByR[i] == "")
        //            {
        //                Region_ByR[i] = null;
        //            }
        //        }
        //        Years_ByR = clr.Go_Live_Year.Split(',');
        //        for (int i = 0; i < Years_ByR.Count(); i++)
        //        {
        //            if (Years_ByR[i] == "")
        //            {
        //                Years_ByR[i] = null;
        //            }
        //        }
        //        //ProjectSum_R = clr.Project_Sum.Split(',');
        //        //for (int i = 0; i < ProjectSum_R.Count(); i++)
        //        //{
        //        //    if (ProjectSum_R[i] == "")
        //        //    {
        //        //        ProjectSum_R[i] = null;
        //        //    }
        //        //}
        //        status_ByR = clr.iMeet_Milestone___Project_Status.Split(',');
        //        for (int i = 0; i < status_ByR.Count(); i++)
        //        {
        //            if (status_ByR[i] == "")
        //            {
        //                status_ByR[i] = null;
        //            }
        //        }
        //        GlobalPL_R = clr.Global_PL_Leader.Split(',');
        //        for (int i = 0; i < GlobalPL_R.Count(); i++)
        //        {
        //            if (GlobalPL_R[i] == "" || GlobalPL_R[i] == "null")
        //            {
        //                GlobalPL_R[i] = null;
        //            }
        //        }
        //        RegionalPL_R = clr.Regional_PL_Leader.Split(',');
        //        for (int i = 0; i < RegionalPL_R.Count(); i++)
        //        {
        //            if (RegionalPL_R[i] == "" || RegionalPL_R[i] == "null")
        //            {
        //                RegionalPL_R[i] = null;
        //            }
        //        }
        //        LocalPL_R = clr.Local_PL_Leader.Split(',');
        //        for (int i = 0; i < LocalPL_R.Count(); i++)
        //        {
        //            if (LocalPL_R[i] == "" || LocalPL_R[i] == "null")
        //            {
        //                LocalPL_R[i] = null;
        //            }
        //        }
        //        GlobalPM_R = clr.Global_Project_Manager.Split(',');
        //        for (int i = 0; i < GlobalPM_R.Count(); i++)
        //        {
        //            if (GlobalPM_R[i] == "" || GlobalPM_R[i] == "null")
        //            {
        //                GlobalPM_R[i] = null;
        //            }
        //        }
        //        RegionalPM_R = clr.Regional_Project_Manager.Split(',');
        //        for (int i = 0; i < RegionalPM_R.Count(); i++)
        //        {
        //            if (RegionalPM_R[i] == "" || RegionalPM_R[i] == "null")
        //            {
        //                RegionalPM_R[i] = null;
        //            }
        //        }
        //        var CapacityByRegion = (from a in entity.CLRs
        //                                where status_ByR.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
        //                                where projectlvl_ByR.Any(val2 => a.iMeet_Project_Level.Equals(val2))
        //                                where Region_ByR.Any(val3 => a.Region__Opportunity_.Equals(val3))
        //                                where Years_ByR.Any(val4 => a.Go_Live_Year.Equals(val4))
        //                                //where ProjectSum_R.Any(val5 => a.Project_Sum.Equals(val5))
        //                                where GlobalPL_R.Any(val5 => a.Global_PL_Leader.Equals(val5))
        //                                where RegionalPL_R.Any(val5 => a.Regional_PL_Leader.Equals(val5))
        //                                where LocalPL_R.Any(val5 => a.Local_PL_Leader.Equals(val5))
        //                                where GlobalPM_R.Any(val5 => a.Global_Project_Manager.Equals(val5))
        //                                where RegionalPM_R.Any(val5 => a.Regional_Project_Manager.Equals(val5))
        //                                select new
        //                                {
        //                                    a.Region__Opportunity_,
        //                                    a.Client,
        //                                    a.iMeet_Milestone___Project_Status,
        //                                    May1st = a.C06_May,
        //                                    May2nd = a.C13_May,
        //                                    May3rd = a.C20_May,
        //                                    May4th = a.C27_May,
        //                                    June1st = a.C03_Jun,
        //                                    June2nd = a.C10_Jun,
        //                                    June3rd = a.C17_Jun,
        //                                    June4th = a.C24_Jun,
        //                                    July1st = a.C01_Jul,
        //                                    July2nd = a.C08_Jul,
        //                                    July3rd = a.C15_Jul,
        //                                    July4th = a.C22_Jul,
        //                                    July5th = a.C29_Jul,
        //                                    August1st = a.C05_Aug,
        //                                    August2nd = a.C12_Aug,
        //                                    August3rd = a.C19_Aug,
        //                                    August4th = a.C26_Aug,
        //                                    September1st = a.C02_Sep,
        //                                    September2nd = a.C09_Sep,
        //                                    September3rd = a.C16_Sep,
        //                                    September4th = a.C23_Sep,
        //                                    September5th = a.C30_Sep,
        //                                    October1st = a.C07_Oct,
        //                                    October2nd = a.C14_Oct,
        //                                    October3rd = a.C21_Oct,
        //                                    October4th = a.C28_Oct,
        //                                    Novomber1st = a.C04_Nov,
        //                                    November2nd = a.C11_Nov,
        //                                    November3rd = a.C18_Nov,
        //                                    November4th = a.C25_Nov,
        //                                    December1st = a.C02_Dec,
        //                                    December2nd = a.C09_Dec,
        //                                    December3rd = a.C16_Dec,
        //                                    December4th = a.C23_Dec,
        //                                    December5th = a.C30_Dec,
        //                                }).OrderBy(x=>x.Region__Opportunity_).ToList();
        //        re.code = 200;
        //        re.message = "Success";
        //        re.Data = CapacityByRegion;
        //    }
        //    return re;
        //}

        //string[] status_ByGPL, projectlvl_ByGPL, Region_ByGPL, Years_ByGPL;
        //[HttpPost]
        //[Route("CapacityByGlobalPL")]
        //public Response CapacityByGlobalPL(CLR clr)
        //{
        //    if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Region__Opportunity_ == null || clr.Go_Live_Year == null)
        //    {
        //        re.code = 100;
        //        re.message = "Please select the values";
        //        re.Data = null;
        //    }
        //    else
        //    {
        //        projectlvl_ByGPL = clr.iMeet_Project_Level.Split(',');
        //        for (int i = 0; i < projectlvl_ByGPL.Count(); i++)
        //        {
        //            if (projectlvl_ByGPL[i] == "")
        //            {
        //                projectlvl_ByGPL[i] = null;
        //            }
        //        }
        //        Region_ByGPL = clr.Region__Opportunity_.Split(',');
        //        for (int i = 0; i < Region_ByGPL.Count(); i++)
        //        {
        //            if (Region_ByGPL[i] == "")
        //            {
        //                Region_ByGPL[i] = null;
        //            }
        //        }
        //        Years_ByGPL = clr.Go_Live_Year.Split(',');
        //        for (int i = 0; i < Years_ByGPL.Count(); i++)
        //        {
        //            if (Years_ByGPL[i] == "")
        //            {
        //                Years_ByGPL[i] = null;
        //            }
        //        }
        //        //ProjectSum_R = clr.Project_Sum.Split(',');
        //        //for (int i = 0; i < ProjectSum_R.Count(); i++)
        //        //{
        //        //    if (ProjectSum_R[i] == "")
        //        //    {
        //        //        ProjectSum_R[i] = null;
        //        //    }
        //        //}
        //        status_ByGPL = clr.iMeet_Milestone___Project_Status.Split(',');
        //        for (int i = 0; i < status_ByGPL.Count(); i++)
        //        {
        //            if (status_ByGPL[i] == "")
        //            {
        //                status_ByGPL[i] = null;
        //            }
        //        }
        //        var CapacityByGlobalPL = (from a in entity.CLRs
        //                                where status_ByGPL.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
        //                                where projectlvl_ByGPL.Any(val2 => a.iMeet_Project_Level.Equals(val2))
        //                                where Region_ByGPL.Any(val3 => a.Region__Opportunity_.Equals(val3))
        //                                where Years_ByGPL.Any(val4 => a.Go_Live_Year.Equals(val4))
        //                                //where ProjectSum_R.Any(val5 => a.Project_Sum.Equals(val5))
        //                                select new
        //                                {
        //                                    a.Global_PL_Leader,
        //                                    a.Client,
        //                                    a.iMeet_Milestone___Project_Status,
        //                                    May1st = a.C06_May,
        //                                    May2nd = a.C13_May,
        //                                    May3rd = a.C20_May,
        //                                    May4th = a.C27_May,
        //                                    June1st = a.C03_Jun,
        //                                    June2nd = a.C10_Jun,
        //                                    June3rd = a.C17_Jun,
        //                                    June4th = a.C24_Jun,
        //                                    July1st = a.C01_Jul,
        //                                    July2nd = a.C08_Jul,
        //                                    July3rd = a.C15_Jul,
        //                                    July4th = a.C22_Jul,
        //                                    July5th = a.C29_Jul,
        //                                    August1st = a.C05_Aug,
        //                                    August2nd = a.C12_Aug,
        //                                    August3rd = a.C19_Aug,
        //                                    August4th = a.C26_Aug,
        //                                    September1st = a.C02_Sep,
        //                                    September2nd = a.C09_Sep,
        //                                    September3rd = a.C16_Sep,
        //                                    September4th = a.C23_Sep,
        //                                    September5th = a.C30_Sep,
        //                                    October1st = a.C07_Oct,
        //                                    October2nd = a.C14_Oct,
        //                                    October3rd = a.C21_Oct,
        //                                    October4th = a.C28_Oct,
        //                                    Novomber1st = a.C04_Nov,
        //                                    November2nd = a.C11_Nov,
        //                                    November3rd = a.C18_Nov,
        //                                    November4th = a.C25_Nov,
        //                                    December1st = a.C02_Dec,
        //                                    December2nd = a.C09_Dec,
        //                                    December3rd = a.C16_Dec,
        //                                    December4th = a.C23_Dec,
        //                                    December5th = a.C30_Dec,
        //                                }).OrderBy(x => x.Global_PL_Leader).ToList();
        //        re.code = 200;
        //        re.message = "Success";
        //        re.Data = CapacityByGlobalPL;
        //    }
        //    return re;
        //}

        //string[] status_ByRPL, projectlvl_ByRPL, Region_ByRPL, Years_ByRPL;
        //[HttpPost]
        //[Route("CapacityByRegionalPL")]
        //public Response CapacityByRegionalPL(CLR clr)
        //{
        //    if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Region__Opportunity_ == null || clr.Go_Live_Year == null)
        //    {
        //        re.code = 100;
        //        re.message = "Please select the values";
        //        re.Data = null;
        //    }
        //    else
        //    {
        //        projectlvl_ByRPL = clr.iMeet_Project_Level.Split(',');
        //        for (int i = 0; i < projectlvl_ByRPL.Count(); i++)
        //        {
        //            if (projectlvl_ByRPL[i] == "" || projectlvl_ByRPL[i] == "null")
        //            {
        //                projectlvl_ByRPL[i] = null;
        //            }
        //        }
        //        Region_ByRPL = clr.Region__Opportunity_.Split(',');
        //        for (int i = 0; i < Region_ByRPL.Count(); i++)
        //        {
        //            if (Region_ByRPL[i] == "" || Region_ByRPL[i] == "null")
        //            {
        //                Region_ByRPL[i] = null;
        //            }
        //        }
        //        Years_ByRPL = clr.Go_Live_Year.Split(',');
        //        for (int i = 0; i < Years_ByRPL.Count(); i++)
        //        {
        //            if (Years_ByRPL[i] == "" || Years_ByRPL[i] == "null")
        //            {
        //                Years_ByRPL[i] = null;
        //            }
        //        }
        //        //ProjectSum_ByRPL = clr.Project_Sum.Split(',');
        //        //for (int i = 0; i < ProjectSum_ByRPL.Count(); i++)
        //        //{
        //        //    if (ProjectSum_ByRPL[i] == "" || ProjectSum_ByRPL[i] == "null")
        //        //    {
        //        //        ProjectSum_ByRPL[i] = null;
        //        //    }
        //        //}
        //        status_ByRPL = clr.iMeet_Milestone___Project_Status.Split(',');
        //        for (int i = 0; i < status_ByRPL.Count(); i++)
        //        {
        //            if (status_ByRPL[i] == "" || status_ByRPL[i] == "null")
        //            {
        //                status_ByRPL[i] = null;
        //            }
        //        }
        //        var CapacityByGlobalPL = (from a in entity.CLRs
        //                                  where status_ByRPL.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
        //                                  where projectlvl_ByRPL.Any(val2 => a.iMeet_Project_Level.Equals(val2))
        //                                  where Region_ByRPL.Any(val3 => a.Region__Opportunity_.Equals(val3))
        //                                  where Years_ByRPL.Any(val4 => a.Go_Live_Year.Equals(val4))
        //                                  //where ProjectSum_ByRPL.Any(val5 => a.Project_Sum.Equals(val5))
        //                                  select new
        //                                  {
        //                                      a.Regional_PL_Leader,
        //                                      a.Client,
        //                                      a.iMeet_Milestone___Project_Status,
        //                                      May1st = a.C06_May,
        //                                      May2nd = a.C13_May,
        //                                      May3rd = a.C20_May,
        //                                      May4th = a.C27_May,
        //                                      June1st = a.C03_Jun,
        //                                      June2nd = a.C10_Jun,
        //                                      June3rd = a.C17_Jun,
        //                                      June4th = a.C24_Jun,
        //                                      July1st = a.C01_Jul,
        //                                      July2nd = a.C08_Jul,
        //                                      July3rd = a.C15_Jul,
        //                                      July4th = a.C22_Jul,
        //                                      July5th = a.C29_Jul,
        //                                      August1st = a.C05_Aug,
        //                                      August2nd = a.C12_Aug,
        //                                      August3rd = a.C19_Aug,
        //                                      August4th = a.C26_Aug,
        //                                      September1st = a.C02_Sep,
        //                                      September2nd = a.C09_Sep,
        //                                      September3rd = a.C16_Sep,
        //                                      September4th = a.C23_Sep,
        //                                      September5th = a.C30_Sep,
        //                                      October1st = a.C07_Oct,
        //                                      October2nd = a.C14_Oct,
        //                                      October3rd = a.C21_Oct,
        //                                      October4th = a.C28_Oct,
        //                                      Novomber1st = a.C04_Nov,
        //                                      November2nd = a.C11_Nov,
        //                                      November3rd = a.C18_Nov,
        //                                      November4th = a.C25_Nov,
        //                                      December1st = a.C02_Dec,
        //                                      December2nd = a.C09_Dec,
        //                                      December3rd = a.C16_Dec,
        //                                      December4th = a.C23_Dec,
        //                                      December5th = a.C30_Dec,
        //                                  }).OrderBy(x => x.Regional_PL_Leader).ToList();
        //        re.code = 200;
        //        re.message = "Success";
        //        re.Data = CapacityByGlobalPL;
        //    }
        //    return re;
        //}
        //string[] status_ByLPL, projectlvl_ByLPL, Region_ByLPL, Years_ByLPL;
        //[HttpPost]
        //[Route("CapacityByLocalPL")]
        //public Response CapacityByLocalPL(CLR clr)
        //{
        //    if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Region__Opportunity_ == null || clr.Go_Live_Year == null)
        //    {
        //        re.code = 100;
        //        re.message = "Please select the values";
        //        re.Data = null;
        //    }
        //    else
        //    {
        //        projectlvl_ByLPL = clr.iMeet_Project_Level.Split(',');
        //        for (int i = 0; i < projectlvl_ByLPL.Count(); i++)
        //        {
        //            if (projectlvl_ByLPL[i] == "" || projectlvl_ByLPL[i] == "null")
        //            {
        //                projectlvl_ByLPL[i] = null;
        //            }
        //        }
        //        Region_ByLPL = clr.Region__Opportunity_.Split(',');
        //        for (int i = 0; i < Region_ByLPL.Count(); i++)
        //        {
        //            if (Region_ByLPL[i] == "" || Region_ByLPL[i] == "null")
        //            {
        //                Region_ByLPL[i] = null;
        //            }
        //        }
        //        Years_ByLPL = clr.Go_Live_Year.Split(',');
        //        for (int i = 0; i < Years_ByLPL.Count(); i++)
        //        {
        //            if (Years_ByLPL[i] == "" || Years_ByLPL[i] == "null")
        //            {
        //                Years_ByLPL[i] = null;
        //            }
        //        }
        //        //ProjectSum_ByLPL = clr.Project_Sum.Split(',');
        //        //for (int i = 0; i < ProjectSum_ByLPL.Count(); i++)
        //        //{
        //        //    if (ProjectSum_ByLPL[i] == "" || ProjectSum_ByLPL[i] == "null")
        //        //    {
        //        //        ProjectSum_ByLPL[i] = null;
        //        //    }
        //        //}
        //        status_ByLPL = clr.iMeet_Milestone___Project_Status.Split(',');
        //        for (int i = 0; i < status_ByLPL.Count(); i++)
        //        {
        //            if (status_ByLPL[i] == "" || status_ByLPL[i] == "null")
        //            {
        //                status_ByLPL[i] = null;
        //            }
        //        }
        //        var CapacityByGlobalPL = (from a in entity.CLRs
        //                                  where status_ByLPL.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
        //                                  where projectlvl_ByLPL.Any(val2 => a.iMeet_Project_Level.Equals(val2))
        //                                  where Region_ByLPL.Any(val3 => a.Region__Opportunity_.Equals(val3))
        //                                  where Years_ByLPL.Any(val4 => a.Go_Live_Year.Equals(val4))
        //                                  //where ProjectSum_ByLPL.Any(val5 => a.Project_Sum.Equals(val5))
        //                                  select new
        //                                  {
        //                                      a.Local_PL_Leader,
        //                                      a.Client,
        //                                      a.iMeet_Milestone___Project_Status,
        //                                      May1st = a.C06_May,
        //                                      May2nd = a.C13_May,
        //                                      May3rd = a.C20_May,
        //                                      May4th = a.C27_May,
        //                                      June1st = a.C03_Jun,
        //                                      June2nd = a.C10_Jun,
        //                                      June3rd = a.C17_Jun,
        //                                      June4th = a.C24_Jun,
        //                                      July1st = a.C01_Jul,
        //                                      July2nd = a.C08_Jul,
        //                                      July3rd = a.C15_Jul,
        //                                      July4th = a.C22_Jul,
        //                                      July5th = a.C29_Jul,
        //                                      August1st = a.C05_Aug,
        //                                      August2nd = a.C12_Aug,
        //                                      August3rd = a.C19_Aug,
        //                                      August4th = a.C26_Aug,
        //                                      September1st = a.C02_Sep,
        //                                      September2nd = a.C09_Sep,
        //                                      September3rd = a.C16_Sep,
        //                                      September4th = a.C23_Sep,
        //                                      September5th = a.C30_Sep,
        //                                      October1st = a.C07_Oct,
        //                                      October2nd = a.C14_Oct,
        //                                      October3rd = a.C21_Oct,
        //                                      October4th = a.C28_Oct,
        //                                      Novomber1st = a.C04_Nov,
        //                                      November2nd = a.C11_Nov,
        //                                      November3rd = a.C18_Nov,
        //                                      November4th = a.C25_Nov,
        //                                      December1st = a.C02_Dec,
        //                                      December2nd = a.C09_Dec,
        //                                      December3rd = a.C16_Dec,
        //                                      December4th = a.C23_Dec,
        //                                      December5th = a.C30_Dec,
        //                                  }).OrderBy(x => x.Local_PL_Leader).ToList();
        //        re.code = 200;
        //        re.message = "Success";
        //        re.Data = CapacityByGlobalPL;
        //    }
        //    return re;
        //}
    }
}