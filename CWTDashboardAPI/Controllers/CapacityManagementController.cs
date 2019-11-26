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
        CWTEntities entity = new CWTEntities();
        Response re = new Response();
        CapacityFilters CF = new CapacityFilters();
        [HttpPost]
        [Route("CapacityFiltersList")]
        public CapacityFilters CapacityFiltersList(CLR clr)
        {
            var FilterProjectStatus = (from a in entity.CLRs
                                       where a.iMeet_Milestone___Project_Status != null || a.iMeet_Milestone___Project_Status != ""
                                       select new
                                       {
                                           a.iMeet_Milestone___Project_Status,
                                           isSelected = true,
                                       }).Distinct().OrderBy(x => x.iMeet_Milestone___Project_Status);
            var FilterProjectLevel = (from a in entity.CLRs
                                      where a.iMeet_Project_Level != null
                                      select new
                                      {
                                          a.iMeet_Project_Level,
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.iMeet_Project_Level);
            var FilterRegion = (from a in entity.CLRs
                                select new
                                {
                                    a.Region__Opportunity_,
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Region__Opportunity_);
            var FilterYears = (from a in entity.CLRs
                               where a.Go_Live_Year != null || a.Go_Live_Year != "1900"
                               select new
                               {
                                   a.Go_Live_Year,
                                   isSelected = true,
                               }).Distinct().OrderBy(x => x.Go_Live_Year);
            var FilterProjectSum = (from a in entity.CLRs
                               where a.Project_Sum != null 
                               select new
                               {
                                   a.Project_Sum,
                                   isSelected = true,
                               }).Distinct().OrderBy(x => x.Project_Sum);
            CF.code = 200;
            CF.message = "Success";
            CF.ProjectStatus = FilterProjectStatus;
            CF.ProjectLevel = FilterProjectLevel;
            CF.Region = FilterRegion;
            CF.Years = FilterYears;
            CF.ProjectSum = FilterProjectSum;
            return CF;
        }

        string[] status_ByC,projectlvl_Byc,Region_Byc,Years_Byc, ProjectSum_c;
        [HttpPost]
        [Route("CapacityByClient")]
        public Response CapacityByClient(CLR clr)
        {
            if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level== null || clr.Region__Opportunity_ == null || clr.Go_Live_Year == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                projectlvl_Byc = clr.iMeet_Project_Level.Split(',');
                for (int i = 0; i < projectlvl_Byc.Count(); i++)
                {
                    if (projectlvl_Byc[i] == "")
                    {
                        projectlvl_Byc[i] = null;
                    }
                }
                Region_Byc = clr.Region__Opportunity_.Split(',');
                for (int i = 0; i < Region_Byc.Count(); i++)
                {
                    if (Region_Byc[i] == "")
                    {
                        Region_Byc[i] = null;
                    }
                }
                Years_Byc = clr.Go_Live_Year.Split(',');
                for (int i = 0; i < Years_Byc.Count(); i++)
                {
                    if (Years_Byc[i] == "")
                    {
                        Years_Byc[i] = null;
                    }
                }
                //ProjectSum_c = clr.Project_Sum.Split(',');
                //for (int i = 0; i < ProjectSum_c.Count(); i++)
                //{
                //    if (ProjectSum_c[i] == "")
                //    {
                //        ProjectSum_c[i] = null;
                //    }
                //}
                status_ByC = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < status_ByC.Count(); i++)
                {
                    if (status_ByC[i] == "" || status_ByC[i] == "null")
                    {
                        status_ByC[i] = null;
                    }
                }
                //status_ByC = "A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
                var CapacityByClient = (from a in entity.CLRs
                                        where status_ByC.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
                                        where projectlvl_Byc.Any(val2 => a.iMeet_Project_Level.Equals(val2))
                                        where Region_Byc.Any(val3 => a.Region__Opportunity_.Equals(val3))
                                        where Years_Byc.Any(val4 => a.Go_Live_Year.Equals(val4))
                                        //where ProjectSum_c.Any(val5 => a.Project_Sum.Equals(val5))
                                        select new
                                        {
                                            a.Client,
                                            a.iMeet_Milestone___Project_Status,
                                            May1st = a.C06_May == null ? 0 : a.C06_May,
                                            May2nd = a.C13_May == null ? 0  : a.C13_May,
                                            May3rd = a.C20_May == null ? 0  : a.C20_May,
                                            May4th = a.C27_May == null ? 0  : a.C27_May,
                                            June1st = a.C03_Jun == null ? 0  : a.C03_Jun,
                                            June2nd = a.C10_Jun == null ? 0  : a.C10_Jun,
                                            June3rd = a.C17_Jun == null ? 0  : a.C17_Jun,
                                            June4th = a.C24_Jun == null ? 0  : a.C24_Jun,
                                            July1st = a.C01_Jul == null ? 0  : a.C01_Jul,
                                            July2nd = a.C08_Jul == null ? 0  : a.C08_Jul,
                                            July3rd = a.C15_Jul == null ? 0  : a.C15_Jul,
                                            July4th = a.C22_Jul == null ? 0  : a.C22_Jul,
                                            July5th = a.C29_Jul == null ? 0  : a.C29_Jul,
                                            August1st = a.C05_Aug == null ? 0  : a.C05_Aug,
                                            August2nd = a.C12_Aug == null ? 0  : a.C12_Aug,
                                            August3rd = a.C19_Aug == null ? 0  : a.C19_Aug,
                                            August4th = a.C26_Aug == null ? 0  : a.C26_Aug,
                                            September1st = a.C02_Sep == null ? 0  : a.C02_Sep,
                                            September2nd = a.C09_Sep == null ? 0  : a.C09_Sep,
                                            September3rd = a.C16_Sep == null ? 0  : a.C16_Sep,
                                            September4th = a.C23_Sep == null ? 0  : a.C23_Sep,
                                            September5th = a.C30_Sep == null ? 0  : a.C30_Sep,
                                            October1st = a.C07_Oct == null ? 0  : a.C07_Oct,
                                            October2nd = a.C14_Oct == null ? 0  : a.C14_Oct,
                                            October3rd = a.C21_Oct == null ? 0  : a.C21_Oct,
                                            October4th = a.C28_Oct == null ? 0  : a.C28_Oct,
                                            Novomber1st = a.C04_Nov == null ? 0  : a.C04_Nov,
                                            November2nd = a.C11_Nov == null ? 0  : a.C11_Nov,
                                            November3rd = a.C18_Nov == null ? 0  : a.C18_Nov,
                                            November4th = a.C25_Nov == null ? 0  : a.C25_Nov,
                                            December1st = a.C02_Dec == null ? 0  : a.C02_Dec,
                                            December2nd = a.C09_Dec == null ? 0  : a.C09_Dec,
                                            December3rd = a.C16_Dec == null ? 0  : a.C16_Dec,
                                            December4th = a.C23_Dec == null ? 0  : a.C23_Dec,
                                            December5th = a.C30_Dec == null ? 0  : a.C30_Dec,
                                        }).OrderBy(x=>x.Client);
                re.code = 200;
                re.message = "Success";
                re.Data = CapacityByClient;
            }
            return re;
        }

        string[] status_ByR, projectlvl_ByR, Region_ByR, Years_ByR, ProjectSum_R;
        [HttpPost]
        [Route("CapacityByRegion")]
        public Response CapacityByRegion(CLR clr)
        {
            if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Region__Opportunity_ == null || clr.Go_Live_Year == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                projectlvl_ByR = clr.iMeet_Project_Level.Split(',');
                for (int i = 0; i < projectlvl_ByR.Count(); i++)
                {
                    if (projectlvl_ByR[i] == "")
                    {
                        projectlvl_ByR[i] = null;
                    }
                }
                Region_ByR = clr.Region__Opportunity_.Split(',');
                for (int i = 0; i < Region_ByR.Count(); i++)
                {
                    if (Region_ByR[i] == "")
                    {
                        Region_ByR[i] = null;
                    }
                }
                Years_ByR = clr.Go_Live_Year.Split(',');
                for (int i = 0; i < Years_ByR.Count(); i++)
                {
                    if (Years_ByR[i] == "")
                    {
                        Years_ByR[i] = null;
                    }
                }
                //ProjectSum_R = clr.Project_Sum.Split(',');
                //for (int i = 0; i < ProjectSum_R.Count(); i++)
                //{
                //    if (ProjectSum_R[i] == "")
                //    {
                //        ProjectSum_R[i] = null;
                //    }
                //}
                status_ByR = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < status_ByR.Count(); i++)
                {
                    if (status_ByR[i] == "")
                    {
                        status_ByR[i] = null;
                    }
                }
                var CapacityByRegion = (from a in entity.CLRs
                                        where status_ByR.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
                                        where projectlvl_ByR.Any(val2 => a.iMeet_Project_Level.Equals(val2))
                                        where Region_ByR.Any(val3 => a.Region__Opportunity_.Equals(val3))
                                        where Years_ByR.Any(val4 => a.Go_Live_Year.Equals(val4))
                                        //where ProjectSum_R.Any(val5 => a.Project_Sum.Equals(val5))
                                        select new
                                        {
                                            a.Region__Opportunity_,
                                            a.Client,
                                            a.iMeet_Milestone___Project_Status,
                                            May1st = a.C06_May,
                                            May2nd = a.C13_May,
                                            May3rd = a.C20_May,
                                            May4th = a.C27_May,
                                            June1st = a.C03_Jun,
                                            June2nd = a.C10_Jun,
                                            June3rd = a.C17_Jun,
                                            June4th = a.C24_Jun,
                                            July1st = a.C01_Jul,
                                            July2nd = a.C08_Jul,
                                            July3rd = a.C15_Jul,
                                            July4th = a.C22_Jul,
                                            July5th = a.C29_Jul,
                                            August1st = a.C05_Aug,
                                            August2nd = a.C12_Aug,
                                            August3rd = a.C19_Aug,
                                            August4th = a.C26_Aug,
                                            September1st = a.C02_Sep,
                                            September2nd = a.C09_Sep,
                                            September3rd = a.C16_Sep,
                                            September4th = a.C23_Sep,
                                            September5th = a.C30_Sep,
                                            October1st = a.C07_Oct,
                                            October2nd = a.C14_Oct,
                                            October3rd = a.C21_Oct,
                                            October4th = a.C28_Oct,
                                            Novomber1st = a.C04_Nov,
                                            November2nd = a.C11_Nov,
                                            November3rd = a.C18_Nov,
                                            November4th = a.C25_Nov,
                                            December1st = a.C02_Dec,
                                            December2nd = a.C09_Dec,
                                            December3rd = a.C16_Dec,
                                            December4th = a.C23_Dec,
                                            December5th = a.C30_Dec,
                                        }).OrderBy(x=>x.Region__Opportunity_).ToList();
                re.code = 200;
                re.message = "Success";
                re.Data = CapacityByRegion;
            }
            return re;
        }

        string[] status_ByGPL, projectlvl_ByGPL, Region_ByGPL, Years_ByGPL, ProjectSum_ByGPL;
        [HttpPost]
        [Route("CapacityByGlobalPL")]
        public Response CapacityByGlobalPL(CLR clr)
        {
            if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Region__Opportunity_ == null || clr.Go_Live_Year == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                projectlvl_ByGPL = clr.iMeet_Project_Level.Split(',');
                for (int i = 0; i < projectlvl_ByGPL.Count(); i++)
                {
                    if (projectlvl_ByGPL[i] == "")
                    {
                        projectlvl_ByGPL[i] = null;
                    }
                }
                Region_ByGPL = clr.Region__Opportunity_.Split(',');
                for (int i = 0; i < Region_ByGPL.Count(); i++)
                {
                    if (Region_ByGPL[i] == "")
                    {
                        Region_ByGPL[i] = null;
                    }
                }
                Years_ByGPL = clr.Go_Live_Year.Split(',');
                for (int i = 0; i < Years_ByGPL.Count(); i++)
                {
                    if (Years_ByGPL[i] == "")
                    {
                        Years_ByGPL[i] = null;
                    }
                }
                //ProjectSum_R = clr.Project_Sum.Split(',');
                //for (int i = 0; i < ProjectSum_R.Count(); i++)
                //{
                //    if (ProjectSum_R[i] == "")
                //    {
                //        ProjectSum_R[i] = null;
                //    }
                //}
                status_ByGPL = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < status_ByGPL.Count(); i++)
                {
                    if (status_ByGPL[i] == "")
                    {
                        status_ByGPL[i] = null;
                    }
                }
                var CapacityByGlobalPL = (from a in entity.CLRs
                                        where status_ByGPL.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
                                        where projectlvl_ByGPL.Any(val2 => a.iMeet_Project_Level.Equals(val2))
                                        where Region_ByGPL.Any(val3 => a.Region__Opportunity_.Equals(val3))
                                        where Years_ByGPL.Any(val4 => a.Go_Live_Year.Equals(val4))
                                        //where ProjectSum_R.Any(val5 => a.Project_Sum.Equals(val5))
                                        select new
                                        {
                                            a.Global_PL_Leader,
                                            a.Client,
                                            a.iMeet_Milestone___Project_Status,
                                            May1st = a.C06_May,
                                            May2nd = a.C13_May,
                                            May3rd = a.C20_May,
                                            May4th = a.C27_May,
                                            June1st = a.C03_Jun,
                                            June2nd = a.C10_Jun,
                                            June3rd = a.C17_Jun,
                                            June4th = a.C24_Jun,
                                            July1st = a.C01_Jul,
                                            July2nd = a.C08_Jul,
                                            July3rd = a.C15_Jul,
                                            July4th = a.C22_Jul,
                                            July5th = a.C29_Jul,
                                            August1st = a.C05_Aug,
                                            August2nd = a.C12_Aug,
                                            August3rd = a.C19_Aug,
                                            August4th = a.C26_Aug,
                                            September1st = a.C02_Sep,
                                            September2nd = a.C09_Sep,
                                            September3rd = a.C16_Sep,
                                            September4th = a.C23_Sep,
                                            September5th = a.C30_Sep,
                                            October1st = a.C07_Oct,
                                            October2nd = a.C14_Oct,
                                            October3rd = a.C21_Oct,
                                            October4th = a.C28_Oct,
                                            Novomber1st = a.C04_Nov,
                                            November2nd = a.C11_Nov,
                                            November3rd = a.C18_Nov,
                                            November4th = a.C25_Nov,
                                            December1st = a.C02_Dec,
                                            December2nd = a.C09_Dec,
                                            December3rd = a.C16_Dec,
                                            December4th = a.C23_Dec,
                                            December5th = a.C30_Dec,
                                        }).OrderBy(x => x.Global_PL_Leader).ToList();
                re.code = 200;
                re.message = "Success";
                re.Data = CapacityByGlobalPL;
            }
            return re;
        }

        string[] status_ByRPL, projectlvl_ByRPL, Region_ByRPL, Years_ByRPL, ProjectSum_ByRPL;
        [HttpPost]
        [Route("CapacityByRegionalPL")]
        public Response CapacityByRegionalPL(CLR clr)
        {
            if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Region__Opportunity_ == null || clr.Go_Live_Year == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                projectlvl_ByRPL = clr.iMeet_Project_Level.Split(',');
                for (int i = 0; i < projectlvl_ByRPL.Count(); i++)
                {
                    if (projectlvl_ByRPL[i] == "" || projectlvl_ByRPL[i] == "null")
                    {
                        projectlvl_ByRPL[i] = null;
                    }
                }
                Region_ByRPL = clr.Region__Opportunity_.Split(',');
                for (int i = 0; i < Region_ByRPL.Count(); i++)
                {
                    if (Region_ByRPL[i] == "" || Region_ByRPL[i] == "null")
                    {
                        Region_ByRPL[i] = null;
                    }
                }
                Years_ByRPL = clr.Go_Live_Year.Split(',');
                for (int i = 0; i < Years_ByRPL.Count(); i++)
                {
                    if (Years_ByRPL[i] == "" || Years_ByRPL[i] == "null")
                    {
                        Years_ByRPL[i] = null;
                    }
                }
                //ProjectSum_ByRPL = clr.Project_Sum.Split(',');
                //for (int i = 0; i < ProjectSum_ByRPL.Count(); i++)
                //{
                //    if (ProjectSum_ByRPL[i] == "" || ProjectSum_ByRPL[i] == "null")
                //    {
                //        ProjectSum_ByRPL[i] = null;
                //    }
                //}
                status_ByRPL = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < status_ByRPL.Count(); i++)
                {
                    if (status_ByRPL[i] == "" || status_ByRPL[i] == "null")
                    {
                        status_ByRPL[i] = null;
                    }
                }
                var CapacityByGlobalPL = (from a in entity.CLRs
                                          where status_ByRPL.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
                                          where projectlvl_ByRPL.Any(val2 => a.iMeet_Project_Level.Equals(val2))
                                          where Region_ByRPL.Any(val3 => a.Region__Opportunity_.Equals(val3))
                                          where Years_ByRPL.Any(val4 => a.Go_Live_Year.Equals(val4))
                                          //where ProjectSum_ByRPL.Any(val5 => a.Project_Sum.Equals(val5))
                                          select new
                                          {
                                              a.Regional_PL_Leader,
                                              a.Client,
                                              a.iMeet_Milestone___Project_Status,
                                              May1st = a.C06_May,
                                              May2nd = a.C13_May,
                                              May3rd = a.C20_May,
                                              May4th = a.C27_May,
                                              June1st = a.C03_Jun,
                                              June2nd = a.C10_Jun,
                                              June3rd = a.C17_Jun,
                                              June4th = a.C24_Jun,
                                              July1st = a.C01_Jul,
                                              July2nd = a.C08_Jul,
                                              July3rd = a.C15_Jul,
                                              July4th = a.C22_Jul,
                                              July5th = a.C29_Jul,
                                              August1st = a.C05_Aug,
                                              August2nd = a.C12_Aug,
                                              August3rd = a.C19_Aug,
                                              August4th = a.C26_Aug,
                                              September1st = a.C02_Sep,
                                              September2nd = a.C09_Sep,
                                              September3rd = a.C16_Sep,
                                              September4th = a.C23_Sep,
                                              September5th = a.C30_Sep,
                                              October1st = a.C07_Oct,
                                              October2nd = a.C14_Oct,
                                              October3rd = a.C21_Oct,
                                              October4th = a.C28_Oct,
                                              Novomber1st = a.C04_Nov,
                                              November2nd = a.C11_Nov,
                                              November3rd = a.C18_Nov,
                                              November4th = a.C25_Nov,
                                              December1st = a.C02_Dec,
                                              December2nd = a.C09_Dec,
                                              December3rd = a.C16_Dec,
                                              December4th = a.C23_Dec,
                                              December5th = a.C30_Dec,
                                          }).OrderBy(x => x.Regional_PL_Leader).ToList();
                re.code = 200;
                re.message = "Success";
                re.Data = CapacityByGlobalPL;
            }
            return re;
        }

        string[] status_ByLPL, projectlvl_ByLPL, Region_ByLPL, Years_ByLPL, ProjectSum_ByLPL;
        [HttpPost]
        [Route("CapacityByLocalPL")]
        public Response CapacityByLocalPL(CLR clr)
        {
            if (clr.iMeet_Milestone___Project_Status == null || clr.iMeet_Project_Level == null || clr.Region__Opportunity_ == null || clr.Go_Live_Year == null)
            {
                re.code = 100;
                re.message = "Please select the values";
                re.Data = null;
            }
            else
            {
                projectlvl_ByLPL = clr.iMeet_Project_Level.Split(',');
                for (int i = 0; i < projectlvl_ByLPL.Count(); i++)
                {
                    if (projectlvl_ByLPL[i] == "" || projectlvl_ByLPL[i] == "null")
                    {
                        projectlvl_ByLPL[i] = null;
                    }
                }
                Region_ByLPL = clr.Region__Opportunity_.Split(',');
                for (int i = 0; i < Region_ByLPL.Count(); i++)
                {
                    if (Region_ByLPL[i] == "" || Region_ByLPL[i] == "null")
                    {
                        Region_ByLPL[i] = null;
                    }
                }
                Years_ByLPL = clr.Go_Live_Year.Split(',');
                for (int i = 0; i < Years_ByLPL.Count(); i++)
                {
                    if (Years_ByLPL[i] == "" || Years_ByLPL[i] == "null")
                    {
                        Years_ByLPL[i] = null;
                    }
                }
                //ProjectSum_ByLPL = clr.Project_Sum.Split(',');
                //for (int i = 0; i < ProjectSum_ByLPL.Count(); i++)
                //{
                //    if (ProjectSum_ByLPL[i] == "" || ProjectSum_ByLPL[i] == "null")
                //    {
                //        ProjectSum_ByLPL[i] = null;
                //    }
                //}
                status_ByLPL = clr.iMeet_Milestone___Project_Status.Split(',');
                for (int i = 0; i < status_ByLPL.Count(); i++)
                {
                    if (status_ByLPL[i] == "" || status_ByLPL[i] == "null")
                    {
                        status_ByLPL[i] = null;
                    }
                }
                var CapacityByGlobalPL = (from a in entity.CLRs
                                          where status_ByLPL.Any(val1 => a.iMeet_Milestone___Project_Status.Equals(val1))
                                          where projectlvl_ByLPL.Any(val2 => a.iMeet_Project_Level.Equals(val2))
                                          where Region_ByLPL.Any(val3 => a.Region__Opportunity_.Equals(val3))
                                          where Years_ByLPL.Any(val4 => a.Go_Live_Year.Equals(val4))
                                          //where ProjectSum_ByLPL.Any(val5 => a.Project_Sum.Equals(val5))
                                          select new
                                          {
                                              a.Local_PL_Leader,
                                              a.Client,
                                              a.iMeet_Milestone___Project_Status,
                                              May1st = a.C06_May,
                                              May2nd = a.C13_May,
                                              May3rd = a.C20_May,
                                              May4th = a.C27_May,
                                              June1st = a.C03_Jun,
                                              June2nd = a.C10_Jun,
                                              June3rd = a.C17_Jun,
                                              June4th = a.C24_Jun,
                                              July1st = a.C01_Jul,
                                              July2nd = a.C08_Jul,
                                              July3rd = a.C15_Jul,
                                              July4th = a.C22_Jul,
                                              July5th = a.C29_Jul,
                                              August1st = a.C05_Aug,
                                              August2nd = a.C12_Aug,
                                              August3rd = a.C19_Aug,
                                              August4th = a.C26_Aug,
                                              September1st = a.C02_Sep,
                                              September2nd = a.C09_Sep,
                                              September3rd = a.C16_Sep,
                                              September4th = a.C23_Sep,
                                              September5th = a.C30_Sep,
                                              October1st = a.C07_Oct,
                                              October2nd = a.C14_Oct,
                                              October3rd = a.C21_Oct,
                                              October4th = a.C28_Oct,
                                              Novomber1st = a.C04_Nov,
                                              November2nd = a.C11_Nov,
                                              November3rd = a.C18_Nov,
                                              November4th = a.C25_Nov,
                                              December1st = a.C02_Dec,
                                              December2nd = a.C09_Dec,
                                              December3rd = a.C16_Dec,
                                              December4th = a.C23_Dec,
                                              December5th = a.C30_Dec,
                                          }).OrderBy(x => x.Local_PL_Leader).ToList();
                re.code = 200;
                re.message = "Success";
                re.Data = CapacityByGlobalPL;
            }
            return re;
        }
    }
}