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
    public class HierarchyController : ApiController
    {
        // GET: Hierarchy
        CWTDashboardEntities entity = new CWTDashboardEntities();
        H_Filters h_f = new H_Filters();
        Response re = new Response();
        HierarchyGraphs hg = new HierarchyGraphs();
        //List<string> LeadersList,SRLeadersList;
        object AssigneNames, LeaderTwoData, LeaderData;
        string[] Leaders, SRLeaders, LeadersTwo;
        [HttpPost]
        [Route("HierarchyFilters")]
        public H_Filters HierarchyFilters(Hierarchy hierarchy) {
            var Vp = (from a in entity.Hierarchies
                      select new
                      {
                          a.VP,
                          isSelected = true,
                      }).Distinct().OrderBy(a => a.VP);
            var Sr_leader = (from a in entity.Hierarchies
                             select new
                             {
                                 a.Sr_Leader,
                                 isSelected = true,
                             }).Distinct().OrderBy(a => a.Sr_Leader);
            if (hierarchy.Sr_Leader == null || hierarchy.Sr_Leader == "")
            {
                SRLeaders = null;
            }
            else
            {
                SRLeaders = hierarchy.Sr_Leader.Split(':');
                for (int i = 0; i < SRLeaders.Count(); i++)
                {
                    if (SRLeaders[i] == "" || SRLeaders[i] == null || SRLeaders[i] == "null")
                    {
                        SRLeaders[i] = null;
                    }
                }
            }
            if (hierarchy.LeaderOne == null || hierarchy.LeaderOne == "")
            {
                Leaders = null;
            }
            else
            {
                Leaders = hierarchy.LeaderOne.Split(':');
                for (int i = 0; i < Leaders.Count(); i++)
                {
                    if (Leaders[i] == "" || Leaders[i] == null || Leaders[i] == "null")
                    {
                        Leaders[i] = null;
                    }
                }
            }
            if (hierarchy.LeaderTwo == null || hierarchy.LeaderTwo == "")
            {
                LeadersTwo = null;
            }
            else
            {
                LeadersTwo = hierarchy.LeaderTwo.Split(':');
                for (int i = 0; i < LeadersTwo.Count(); i++)
                {
                    if (LeadersTwo[i] == "" || LeadersTwo[i] == null || LeadersTwo[i] == "null")
                    {
                        LeadersTwo[i] = null;
                    }
                }
            }
            if (hierarchy.Sr_Leader == null || hierarchy.Sr_Leader == "" || hierarchy.Sr_Leader == "null")
            {
                LeaderData = (from a in entity.Hierarchies
                              select new
                              {
                                  a.LeaderOne,
                                  isSelected = true,
                              }).Distinct().OrderBy(a => a.LeaderOne);
                if (hierarchy.LeaderOne == null || hierarchy.LeaderOne == "" || hierarchy.LeaderOne == "null")
                {
                    LeaderTwoData = (from a in entity.Hierarchies
                                     select new
                                     {
                                         a.LeaderTwo,
                                         isSelected = true,
                                     }).Distinct().OrderBy(a => a.LeaderTwo);
                    if (hierarchy.LeaderTwo == null || hierarchy.LeaderTwo == "" || hierarchy.LeaderTwo == "null")
                    {
                        AssigneNames = (from a in entity.Hierarchies
                                        select new
                                        {
                                            a.Name,
                                            isSelected = true,
                                        }).Distinct().OrderBy(a => a.Name);
                    }
                    else
                    {
                        AssigneNames = (from a in entity.Hierarchies
                                        where LeadersTwo.Any(val => a.LeaderTwo.Equals(val))
                                        select new
                                        {
                                            a.Name,
                                            isSelected = true,
                                        }).Distinct().OrderBy(a => a.Name);
                    }
                }
                else
                {
                    LeaderTwoData = (from a in entity.Hierarchies
                                     where Leaders.Any(val => a.LeaderOne.Equals(val))
                                     select new
                                     {
                                         a.LeaderTwo,
                                         isSelected = true,
                                     }).Distinct().OrderBy(a => a.LeaderTwo);
                    if (hierarchy.LeaderTwo == null || hierarchy.LeaderTwo == "" || hierarchy.LeaderTwo == "null")
                    {
                        AssigneNames = (from a in entity.Hierarchies
                                        where Leaders.Any(val => a.LeaderOne.Equals(val))
                                        select new
                                        {
                                            a.Name,
                                            isSelected = true,
                                        }).Distinct().OrderBy(a => a.Name);
                    }
                    else
                    {
                        AssigneNames = (from a in entity.Hierarchies
                                        where Leaders.Any(val => a.LeaderOne.Equals(val))
                                        where LeadersTwo.Any(val => a.LeaderTwo.Equals(val))
                                        select new
                                        {
                                            a.Name,
                                            isSelected = true,
                                        }).Distinct().OrderBy(a => a.Name);
                    }
                }

            }
            else
            {
                LeaderData = (from a in entity.Hierarchies
                              where SRLeaders.Any(val => a.Sr_Leader.Equals(val))
                              select new
                              {
                                  a.LeaderOne,
                                  isSelected = true,
                              }).Distinct().OrderBy(a => a.LeaderOne);
                if (hierarchy.LeaderOne == null || hierarchy.LeaderOne == "" || hierarchy.LeaderOne == "null")
                {
                    LeaderTwoData = (from a in entity.Hierarchies
                                     where SRLeaders.Any(val => a.Sr_Leader.Equals(val))
                                     select new
                                     {
                                         a.LeaderTwo,
                                         isSelected = true,
                                     }).Distinct().OrderBy(a => a.LeaderTwo);
                    if (hierarchy.LeaderTwo == null || hierarchy.LeaderTwo == "" || hierarchy.LeaderTwo == "null")
                    {
                        AssigneNames = (from a in entity.Hierarchies
                                        where SRLeaders.Any(val => a.Sr_Leader.Equals(val))
                                        select new
                                        {
                                            a.Name,
                                            isSelected = true,
                                        }).Distinct().OrderBy(a => a.Name);
                    }
                    else
                    {
                        AssigneNames = (from a in entity.Hierarchies
                                        where SRLeaders.Any(val => a.Sr_Leader.Equals(val))
                                        where LeadersTwo.Any(val => a.LeaderTwo.Equals(val))
                                        select new
                                        {
                                            a.Name,
                                            isSelected = true,
                                        }).Distinct().OrderBy(a => a.Name);
                    }
                }
                else
                {
                    LeaderTwoData = (from a in entity.Hierarchies
                                     where SRLeaders.Any(val => a.Sr_Leader.Equals(val))
                                     where Leaders.Any(val => a.LeaderOne.Equals(val))
                                     select new
                                     {
                                         a.LeaderTwo,
                                         isSelected = true,
                                     }).Distinct().OrderBy(a => a.LeaderTwo);
                    if (hierarchy.LeaderTwo == null || hierarchy.LeaderTwo == "" || hierarchy.LeaderTwo == "null")
                    {
                        AssigneNames = (from a in entity.Hierarchies
                                        where SRLeaders.Any(val => a.Sr_Leader.Equals(val))
                                        where Leaders.Any(val => a.LeaderOne.Equals(val))
                                        select new
                                        {
                                            a.Name,
                                            isSelected = true,
                                        }).Distinct().OrderBy(a => a.Name);
                    }
                    else
                    {
                        AssigneNames = (from a in entity.Hierarchies
                                        where SRLeaders.Any(val => a.Sr_Leader.Equals(val))
                                        where Leaders.Any(val => a.LeaderOne.Equals(val))
                                        where LeadersTwo.Any(val => a.LeaderTwo.Equals(val))
                                        select new
                                        {
                                            a.Name,
                                            isSelected = true,
                                        }).Distinct().OrderBy(a => a.Name);
                    }
                }
            }
            h_f.code = 200;
            h_f.message = "success";
            h_f.VP = Vp;
            h_f.Sr_Leader = Sr_leader;
            h_f.LeaderTwo = LeaderTwoData;
            h_f.Leader = LeaderData;
            h_f.Name = AssigneNames;
            return h_f;
        }

        string[] Opportunity_Type, Opportunity_Type2, Revenue_Opportunity_Type;
        [HttpPost]
        [Route("GeneratingClR")]
        public Response GeneratingClR(iMeetData iMeet)
        {
            var CreationDate = "01-01-2020";
            DateTime ConvertedDate = Convert.ToDateTime(CreationDate);
            Opportunity_Type = "New Business,Up-Sell(Add Offices/Countries)".Split(',');
            var Status = "C-Closed,A-Active/Date Confirmed,N-Active/No Date Confirmed".Split(',');
            Opportunity_Type2 = "Re-Bid With Up-Sell,Lost Client (w/o notice)".Split(',');
            Revenue_Opportunity_Type = "Up-Sell(Add Offices/Countries),Re-Bid With Up-Sell,New Business".Split(',');
            var GenerateCLR = (from a in entity.iMeetDatas
                               where a.Task_Start_Date >= ConvertedDate
                               where Status.Any(val => a.Milestone__Project_Status.Equals(val))
                               join b in entity.CRMDatas on a.Milestone__CRM_Revenue_ID__ equals b.Revenue_Id
                               select new
                               {
                                   RevenueID = a.Milestone__CRM_Revenue_ID__,
                                   b.Revenue_Id,
                                   Region = b.Region__Revenue_ ?? "Blanks",
                                   Country = b.Country ?? "Blanks",
                                   OwnerShip = b.Ownership__Revenue_ ?? "Blanks",
                                   GoLiveDate = a.Task_Start_Date,
                                   //Year = a.Task_Start_Date.Value.Month.ToString("yyyy"),
                                   ProjectStatus = a.Milestone__Project_Status,
                                   CountryStatus = a.Milestone__Country_Status,
                                   ProjectLevel = a.Workspace__Project_Level,
                                   ProjectStartDate = a.Milestone__Project_Start_Date,
                                   IntitialGoliveDate = a.Milestone__Initial_Go_Live_Date,
                                   CompletedDate = a.Completed_Date,
                                   //ProjectOwner = a.Workspace__Project_Owner__Full_Name,
                                   AssigneeFullName = a.Milestone__Assignee__Full_Name,
                                   MilestoneTitle = a.Milestone_Title,
                                   a.Milestone__Record_ID_Key,
                                   a.Task__Task_Record_ID_Key,
                                   a.Group_Name,
                                   a.Milestone__Project_Notes,
                                   a.Milestone__Reason_Code,
                                   a.Milestone__Closed_Loop_Owner,
                                   a.Workspace_Title,
                                   a.Workspace__ELT_Overall_Status,
                                   a.Workspace__ELT_Overall_Comments,
                                   b.Customer_Row_ID,
                                   b.Opportunity_ID,
                                   b.Account_Name,
                                   b.Sales_Stage_Name,
                                   b.Opportunity_Type,
                                   b.Revenue_Opportunity_Type,
                                   b.Revenue_Status,
                                   b.Opportunity_Owner,
                                   b.Opportunity_Category,
                                   b.Revenue_Total_Transactions,
                                   CountryCode = entity.CountryIsoCodes.Where(x => x.CountryName == b.Country).Select(x => x.CountryCode),
                                   //RevenueVolumeUSD = b.Opportunity_Type == "New Business" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Up-Sell(Add Offices/Countries)" ? b.Revenue_Total_Volume_USD : b.Opportunity_Type == "Re-Bid" ? 0 : b.Opportunity_Type == "Renewal/Renegotiation" ? 0 : b.Opportunity_Type == null ? 0 : b.Revenue_Opportunity_Type == "" ? 0 : 0,
                                   RevenueVolumeUSD = Opportunity_Type.Any(val => b.Opportunity_Type.Equals(val)) ? b.Revenue_Total_Volume_USD
                                                        : Opportunity_Type2.Any(val => b.Opportunity_Type.Equals(val))
                                                            ? Revenue_Opportunity_Type.Any(val => b.Revenue_Opportunity_Type.Equals(val)) ? b.Revenue_Total_Volume_USD
                                                            : 0
                                                        : 0,
                                   MarketLeader = b.Opportunity_Category == "GPS" ? "Cathy Voss"
                                                    : b.Opportunity_Category == null ? "No data from CRM"
                                                    : b.Region__Revenue_ == "APAC" ? "Bindu Bhatia"
                                                    : b.Region__Revenue_ == "EMEA" ? "Chris Bowen"
                                                    : b.Region__Revenue_ == "NORAM" ? "Barbara Bernard"
                                                    : b.Region__Revenue_ == "LATAM" ? "Barbara Bernard"
                                                    : "No data from CRM",

                                   GlobalProjectManager = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Global_Project_Manager).Distinct(),
                                   ProjectConsultant = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Regional_Ops_Manager).Distinct(),
                                   //RegionalProjectManager = b.Region__Revenue_ == "APAC" ? entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.APAC_IMP_Lead).Distinct()
                                   //                             : b.Region__Revenue_ == "LATAM" ? entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.LATAM_Telephony_Team).Distinct()
                                   //                             : b.Region__Revenue_ == "NORAM" ? entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.NORAM_Real_Estate_and_Facilities).Distinct()
                                   //                             : b.Region__Revenue_ == "EMEA" ? entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.EMEA_Project_Manager).Distinct()
                                   //                             : entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.EMEA_Project_Manager).Distinct(),
                                   GlobalCISOBTLead = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Global_CIS_OBT_Lead).Distinct(),
                                   GlobalCISHRFeedSpecialist = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Global_CIS_HR_Feed_Specialist).Distinct(),
                                   GlobalCISPortraitLead = entity.Roles.Where(x => x.Workspace_Title == a.Workspace_Title).Select(x => x.Global_CIS_Portrait_Lead).Distinct(),
                               }).OrderBy(x =>x.Workspace_Title);
            //re.Data = GenerateCLR.AsEnumerable()
            //                .Select(x => new MainCLR
            //                {
            //                    RevenueID = x.RevenueID,
            //                    Region = x.Region,
            //                    Country = x.Country,
            //                    OwnerShipRevenue = x.OwnerShip,
            //                    GoLiveDate = x.GoLiveDate,
            //                    ProjectStatus = x.ProjectStatus,
            //                    CountryStatus = x.CountryStatus,
            //                    ProjectLevel = x.ProjectLevel,
            //                    ProjectStartDate = x.ProjectStartDate,
            //                    IntitialGoliveDate = x.IntitialGoliveDate,
            //                    CompletedDate = x.CompletedDate,
            //                    ProjectOwner = x.ProjectOwner,
            //                    AssigneeFullName = x.AssigneeFullName,
            //                    MilestoneTitle = x.MilestoneTitle,
            //                    MilestoneRecordIDKey = x.Milestone__Record_ID_Key,
            //                    TaskRecordIDKey = x.Task__Task_Record_ID_Key,
            //                    Group_Name = x.Group_Name,
            //                    MilestoneProjectNotes = x.Milestone__Project_Notes,
            //                    MilestoneReasonCode = x.Milestone__Reason_Code,
            //                    MilestoneClosedLoopOwner = x.Milestone__Closed_Loop_Owner,
            //                    WorkspaceTitle = x.Workspace_Title,
            //                    WorkspaceELTOverallStatus = x.Workspace__ELT_Overall_Status,
            //                    WorkspaceELTOverallComments = x.Workspace__ELT_Overall_Comments,
            //                    CustomerRowID = x.Customer_Row_ID,
            //                    OpportunityID = x.Opportunity_ID,
            //                    AccountName = x.Account_Name,
            //                    SalesStageName = x.Sales_Stage_Name,
            //                    OpportunityType = x.Opportunity_Type,
            //                    RevenueOpportunityType = x.Revenue_Opportunity_Type,
            //                    RevenueStatus = x.Revenue_Status,
            //                    OpportunityOwner = x.Opportunity_Owner,
            //                    OpportunityCategory = x.Opportunity_Category,
            //                    RevenueTotalTransactions = x.Revenue_Total_Transactions,
            //                    CountryCode = x.CountryCode,
            //                    RevenueVolumeUSD = x.RevenueVolumeUSD,
            //                    MarketLeader = x.MarketLeader,
            //                    GlobalProjectManager = x.GlobalProjectManager,
            //                    ProjectConsultant = x.ProjectConsultant,
            //                    GlobalCISOBTLead = x.GlobalCISOBTLead,
            //                    GlobalCISHRFeedSpecialist = x.GlobalCISHRFeedSpecialist,
            //                    GlobalCISPortraitLead = x.GlobalCISPortraitLead,
            //                }).Take(25).ToList();
            re.code = 200;
            re.message = "Success";
            re.Data = GenerateCLR;
            return re;
        }
        string[] years, quarters, Level, status, Region, AssignToName;
        [HttpPost]
        [Route("CLRHierarchyData")]
        public Response CLRHierarchyData(CLRData clr)
        {
            if (clr.ProjectStatus == null || clr.Region == null || clr.ProjectLevel == null || clr.Quarter == null || clr.GoLiveYear == null || clr.AssigneeFullName == null)
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                //Vp = clr.VP;
                //SeniorLeader = clr.SeniorLeader;
                //Leader = clr.LeaderOne;
                status = clr.ProjectStatus.Split(',');
                for (int i = 0; i < status.Count(); i++)
                {
                    if (status[i] == "" || status[i] == "null")
                    {
                        status[i] = null;
                    }
                }
                Level = clr.ProjectLevel.Split(',');
                for (int i = 0; i < Level.Count(); i++)
                {
                    if (Level[i] == "" || Level[i] == "null")
                    {
                        Level[i] = null;
                    }
                }
                years = clr.GoLiveYear.Split(',');
                for (int i = 0; i < years.Count(); i++)
                {
                    if (years[i] == "" || years[i] == "null")
                    {
                        years[i] = null;
                    }
                }
                quarters = clr.Quarter.Split(',');
                for (int i = 0; i < quarters.Count(); i++)
                {
                    if (quarters[i] == "" || quarters[i] == "null")
                    {
                        quarters[i] = null;
                    }
                }
                Region = clr.Region.Split(',');
                for (int i = 0; i < Region.Count(); i++)
                {
                    if (Region[i] == "" || Region[i] == "null")
                    {
                        Region[i] = null;
                    }
                }
                AssignToName = clr.AssigneeFullName.Split(':');
                for (int i = 0; i < AssignToName.Count(); i++)
                {
                    if (AssignToName[i] == "" || AssignToName[i] == "null")
                    {
                        AssignToName[i] = null;
                    }
                }
                var HierarchyData = (from a in entity.CLRDatas
                                     where years.Any(val => a.GoLiveYear.Equals(val))
                                     where quarters.Any(val => a.Quarter.Equals(val))
                                     where Level.Any(val => a.ProjectLevel.Equals(val))
                                     where status.Any(val => a.ProjectStatus.Equals(val))
                                     where Region.Any(val => a.Region.Equals(val))
                                     where AssignToName.Any(val => a.AssigneeFullName.Equals(val))
                                     select new
                                     {
                                         WorkspaceTitle = a.Workspace_Title,
                                         MilestoneTitle = a.MilestoneTitle,
                                         RevenueId = a.RevenueID,
                                         RevenueVolumeUSD = a.RevenueVolumeUSD,
                                         a.Country,
                                         Region = a.Region,
                                         //Vp,
                                         //SeniorLeader,
                                         //Leader,
                                         AssigneFullName = a.AssigneeFullName,
                                         OwnershipRevenue = a.OwnerShip,
                                         TaskGoliveDate = a.GoLiveDate,
                                         ProjectStatus = a.ProjectStatus,
                                         CountryStatus = a.CountryStatus,
                                         ProjectLevel = a.ProjectLevel,
                                         //ProjectStartDate = a.iMeet_Milestone__Project_Start_Date,
                                         //InitialGoliveDate = a.iMeet_Milestone___Initial_Go_Live_Date,
                                         //LastGoliveDate = a.Milestone__Last_Go_Live_Date,
                                         CompletedDate = a.CompletedDate,
                                         //ProjectOwner = a.ProjectOwner,
                                         GlobalProjectManager = a.GlobalProjectManager,
                                         //ProjectConsultant = a.ProjectConsultant,
                                         RegionalProjectManager = a.RegionalProjectManager,
                                         GlobalCISOBTLead = a.GlobalCISOBTLead,
                                         GlobalCISHRFeed = a.GlobalCISHRFeedSpecialist,
                                         GlobalCISPortraitLead = a.GlobalCISPortraitLead,
                                         //GlobalCISRoomIT = a.,
                                         //GlobalPLLeader = a.Global_PL_Leader,
                                         RecordIdKey = a.Task__Task_Record_ID_Key,
                                         //TaskGoLiveDateRecordIDKey = a.Task__Go_Live_Date_Record_ID_Key,
                                         ELTOverallComments = a.Workspace__ELT_Overall_Comments,
                                         ELTOverallStatus = a.Workspace__ELT_Overall_Status,
                                         GroupName = a.Group_Name,
                                         ProjectNotes = a.Milestone__Project_Notes,
                                         ReasonCode = a.Milestone__Reason_Code,
                                         CustomerRowId = a.Customer_Row_ID,
                                         OpportunityId = a.Opportunity_ID,
                                         Account = a.Account_Name,
                                         SalesStageName = a.Sales_Stage_Name,
                                         OpportunityType = a.Opportunity_Type,
                                         RevenueStatus = a.Revenue_Status,
                                         RevenueOpportunityType = a.Revenue_Opportunity_Type,
                                         OpportunityOwner = a.Opportunity_Owner,
                                         MarketLeader = a.MarketLeader,
                                         RevenueTotalTransactions = a.Revenue_Total_Transactions,
                                     });
                re.code = 200;
                re.message = "success";
                re.Data = HierarchyData;
            }
            return re;
        }
        [HttpPost]
        [Route("HierarchyData")]
        public Response HierarchyData(Hierarchy hierarchy)
        {
            var HierarchyData = (from a in entity.Hierarchies
                                 where a.UserStatus == "Active"
                                 select new
                                 {
                                     a.HierarchyID,
                                     a.User_ID,
                                     a.Name,
                                     a.LeaderTwo,
                                     a.LeaderOne,
                                     a.Sr_Leader,
                                     a.VP,
                                     a.Email_Address,
                                     a.Region,
                                     a.Location,
                                     a.Role,
                                     a.Title,
                                     a.UserStatus,
                                 });
            if(HierarchyData.AsQueryable().Count() == 0 || HierarchyData.AsQueryable().Count().ToString() == null)
            {
                re.code = 100;
                re.message = "No data found";
                re.Data = null;
            }
            else
            {
                re.code = 200;
                re.message = "Data Found";
                re.Data = HierarchyData;
            }
            return re;
        }
        [HttpPost]
        [Route("HierarchyInsert")]
        public IHttpActionResult HierarchyInsert(Hierarchy hierarchy)
        {
            Boolean b = new HierarchyClass().AddingHierarchy(hierarchy);
            if (b)
            {
                entity.Hierarchies.Add(hierarchy);
                entity.SaveChanges();
                re.code = 200;
                re.message = "Added Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to add New";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("HierarchyUpdate")]
        public IHttpActionResult HierarchyUpdate(Hierarchy hierarchy)
        {
            Boolean b = new HierarchyClass().UpdateHierarchy(hierarchy);
            if (b)
            {
                re.code = 200;
                re.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Update data";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("HierarchyDelete")]
        public IHttpActionResult HierarchyDelete(Hierarchy hierarchy)
        {
            Boolean b = new HierarchyClass().DeleteHierarchy(hierarchy);
            if (b)
            {
                re.code = 200;
                re.message = "Deleted Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Delete data";
                return Content(HttpStatusCode.OK, re);
            }
        }
        
        string[] R_Years,R_Quarters,R_region,R_Level,R_Status,R_Assigne;
        [HttpPost]
        [Route("HierarchyGraphs")]
        public HierarchyGraphs HierarchyGraphs(CLRData clr)
        {
            if (clr.ProjectStatus == null || clr.GoLiveYear == null || clr.Quarter == null || clr.Region == null || clr.ProjectLevel == null || clr.AssigneeFullName == null)
            {
                hg.code = 100;
                hg.message = "Please select the Project Status";
            }
            else
            {
                R_Years = clr.GoLiveYear.Split(',');
                for (int i = 0; i < R_Years.Count(); i++)
                {
                    if (R_Years[i] == "")
                    {
                        R_Years[i] = null;
                    }
                }
                R_Quarters = clr.Quarter.Split(',');
                for (int i = 0; i < R_Quarters.Count(); i++)
                {
                    if (R_Quarters[i] == "")
                    {
                        R_Quarters[i] = null;
                    }
                }
                R_region = clr.Region.Split(',');
                for (int i = 0; i < R_region.Count(); i++)
                {
                    if (R_region[i] == "")
                    {
                        R_region[i] = null;
                    }
                }
                R_Level = clr.ProjectLevel.Split(',');
                for (int i = 0; i < R_Level.Count(); i++)
                {
                    if (R_Level[i] == "")
                    {
                        R_Level[i] = null;
                    }
                }
                R_Status = clr.ProjectStatus.Split(',');
                for (int i = 0; i < R_Status.Count(); i++)
                {
                    if (R_Status[i] == "")
                    {
                        R_Status[i] = null;
                    }
                }
                R_Assigne = clr.AssigneeFullName.Split(':');
                for (int i = 0; i < R_Assigne.Count(); i++)
                {
                    if (R_Assigne[i] == "")
                    {
                        R_Assigne[i] = null;
                    }
                }
                var RegionwiseData = (from a in entity.CLRDatas
                                      where R_Years.Any(val => a.GoLiveYear.Equals(val))
                                      where R_Quarters.Any(val => a.Quarter.Equals(val))
                                      where R_region.Any(val => a.Region.Equals(val))
                                      where R_Level.Any(val => a.ProjectLevel.Equals(val))
                                      where R_Status.Any(val => a.ProjectStatus.Equals(val))
                                      where R_Assigne.Any(val => a.AssigneeFullName.Equals(val))
                                      group a by a.Region into g
                                      select new
                                      {
                                          Region__Opportunity_ = g.Key,
                                          RevenueVolume = g.Sum(x => x.RevenueVolumeUSD),
                                          ProjectsCount = g.Count(),
                                      });
                var LevelwiseData = (from a in entity.CLRDatas
                                     where R_Years.Any(val => a.GoLiveYear.Equals(val))
                                     where R_Quarters.Any(val => a.Quarter.Equals(val))
                                     where R_region.Any(val => a.Region.Equals(val))
                                     where R_Level.Any(val => a.ProjectLevel.Equals(val))
                                     where R_Status.Any(val => a.ProjectStatus.Equals(val))
                                     where R_Assigne.Any(val => a.AssigneeFullName.Equals(val))
                                     group a by a.ProjectLevel into g
                                     select new
                                     {
                                         iMeet_Project_Level = g.Key,
                                         RevenueVolume = g.Sum(x => x.RevenueVolumeUSD),
                                         ProjectsCount = g.Count(),
                                     });
                var StatusWiseData = (from a in entity.CLRDatas
                                      where R_Years.Any(val => a.GoLiveYear.Equals(val))
                                      where R_Quarters.Any(val => a.Quarter.Equals(val))
                                      where R_region.Any(val => a.Region.Equals(val))
                                      where R_Level.Any(val => a.ProjectLevel.Equals(val))
                                      where R_Status.Any(val => a.ProjectStatus.Equals(val))
                                      where R_Assigne.Any(val => a.AssigneeFullName.Equals(val))
                                      group a by a.ProjectStatus into g
                                      select new
                                      {
                                          ProjectStatus = g.Key,
                                          RevenueVolume = g.Sum(x => x.RevenueVolumeUSD),
                                          ProjectsCount = g.Count(),
                                      });
                //var LeaderWiseData = (from a in entity.CLRs
                //                      where R_Years.Any(val => a.Go_Live_Year.Equals(val))
                //                      where R_Quarters.Any(val => a.Quarter.Equals(val))
                //                      where R_region.Any(val => a.Region__Opportunity_.Equals(val))
                //                      where R_Level.Any(val => a.iMeet_Project_Level.Equals(val))
                //                      where R_Status.Any(val => a.iMeet_Milestone___Project_Status.Equals(val))
                //                      where R_Assigne.Any(val => a.Milestone__Assignee__Full_Name.Equals(val))
                //                      group a by a.iMeet_Project_Level into g
                //                      select new
                //                      {
                //                          iMeet_Project_Level = g.Key,
                //                          RevenueVolume = g.Sum(x => x.Revenue_Total_Volume_USD),
                //                          ProjectsCount = g.Count(),
                //                      });
                var MonthWiseData = (from a in entity.CLRDatas
                                     where R_Years.Any(val => a.GoLiveYear.Equals(val))
                                     where R_Quarters.Any(val => a.Quarter.Equals(val))
                                     where R_region.Any(val => a.Region.Equals(val))
                                     where R_Level.Any(val => a.ProjectLevel.Equals(val))
                                     where R_Status.Any(val => a.ProjectStatus.Equals(val))
                                     where R_Assigne.Any(val => a.AssigneeFullName.Equals(val))
                                     where a.GoLiveMonth != null
                                     group a by a.GoLiveMonth into g
                                     select new
                                     {
                                         Go_Live_Month = g.Key,
                                         RevenueVolume = g.Sum(x => x.RevenueVolumeUSD),
                                         ProjectsCount = g.Count(),
                                     }).AsEnumerable().OrderBy(x => DateTime.ParseExact(x.Go_Live_Month, "MMM", CultureInfo.InvariantCulture).Month);
                hg.code = 200;
                hg.message = "Data success";
                hg.RegionWiseData = RegionwiseData;
                hg.LevelWiseData = LevelwiseData;
                hg.MonthWiseData = MonthWiseData;
                hg.StatusWiseData = StatusWiseData;
            }
            return hg;
        }
        string[] M_Quarters, M_region, M_Level, M_Status, M_Assigne;
        [HttpPost]
        [Route("HierarchyMonthWise")]
        public HierarchyGraphs HierarchyMonthWise(CLRData clr)
        {
            if (clr.ProjectStatus == null || clr.GoLiveYear == null || clr.Quarter == null || clr.Region == null || clr.ProjectLevel == null || clr.AssigneeFullName == null)
            {
                hg.code = 100;
                hg.message = "Please select all the values";
            }
            else
            {
                //M_Year = clr.Go_Live_Year;
                M_Quarters = clr.Quarter.Split(',');
                for (int i = 0; i < M_Quarters.Count(); i++)
                {
                    if (M_Quarters[i] == "")
                    {
                        M_Quarters[i] = null;
                    }
                }
                M_region = clr.Region.Split(',');
                for (int i = 0; i < M_region.Count(); i++)
                {
                    if (M_region[i] == "")
                    {
                        M_region[i] = null;
                    }
                }
                M_Level = clr.ProjectLevel.Split(',');
                for (int i = 0; i < M_Level.Count(); i++)
                {
                    if (M_Level[i] == "")
                    {
                        M_Level[i] = null;
                    }
                }
                M_Status = clr.ProjectStatus.Split(',');
                for (int i = 0; i < M_Status.Count(); i++)
                {
                    if (M_Status[i] == "")
                    {
                        M_Status[i] = null;
                    }
                }
                M_Assigne = clr.AssigneeFullName.Split(':');
                for (int i = 0; i < M_Assigne.Count(); i++)
                {
                    if (M_Assigne[i] == "")
                    {
                        M_Assigne[i] = null;
                    }
                }
                var MonthWiseData = (from a in entity.CLRDatas
                                     where a.GoLiveYear == clr.GoLiveYear
                                     where M_Quarters.Any(val => a.Quarter.Equals(val))
                                     where M_region.Any(val => a.Region.Equals(val))
                                     where M_Level.Any(val => a.ProjectLevel.Equals(val))
                                     where M_Status.Any(val => a.ProjectStatus.Equals(val))
                                     where M_Assigne.Any(val => a.AssigneeFullName.Equals(val))
                                     where a.GoLiveMonth != null
                                     group a by a.GoLiveMonth into g
                                     select new
                                     {
                                         Go_Live_Month = g.Key,
                                         RevenueVolume = g.Sum(x => x.RevenueVolumeUSD),
                                         ProjectsCount = g.Count(),
                                     }).AsEnumerable().OrderBy(x => DateTime.ParseExact(x.Go_Live_Month, "MMM", CultureInfo.InvariantCulture).Month);
                hg.code = 200;
                hg.message = "Data success";
                hg.MonthWiseData = MonthWiseData;
            }
            return hg;
        }
    }
    public class MainCLR
    {
        public double? RevenueID { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string OwnerShipRevenue { get; set; }
        public Nullable<System.DateTime> GoLiveDate { get; set; }
        public string Year { get; set; }
        public string Month { get; set; }
        public string Quarter { get; set; }
        public string ProjectStatus { get; set; }
        public string CountryStatus { get; set; }
        public string ProjectLevel { get; set; }
        public Nullable<System.DateTime> ProjectStartDate { get; set; }
        public Nullable<System.DateTime> IntitialGoliveDate { get; set; }
        public Nullable<System.DateTime> CompletedDate { get; set; }
        public string ProjectOwner { get; set; }
        public string AssigneeFullName { get; set; }
        public string MilestoneTitle { get; set; }
        public string MilestoneRecordIDKey { get; set; }
        public string TaskRecordIDKey { get; set; }
        public string Group_Name { get; set; }
        public string MilestoneProjectNotes { get; set; }
        public string MilestoneReasonCode { get; set; }
        public string MilestoneClosedLoopOwner { get; set; }
        public string WorkspaceTitle { get; set; }
        public string WorkspaceELTOverallStatus { get; set; }
        public string WorkspaceELTOverallComments { get; set; }
        public double? CustomerRowID { get; set; }
        public double? OpportunityID { get; set; }
        public string AccountName { get; set; }
        public string SalesStageName { get; set; }
        public string OpportunityType { get; set; }
        public string RevenueOpportunityType { get; set; }
        public string RevenueStatus { get; set; }
        public string OpportunityOwner { get; set; }
        public string OpportunityCategory { get; set; }
        public double? RevenueTotalTransactions { get; set; }
        public object CountryCode { get; set; }
        public double? RevenueVolumeUSD { get; set; }
        public string MarketLeader { get; set; }
        public object GlobalProjectManager { get; set; }
        public object ProjectConsultant { get; set; }
        public object GlobalCISOBTLead { get; set; }
        public object GlobalCISHRFeedSpecialist { get; set; }
        public object GlobalCISPortraitLead { get; set; }
    }
}