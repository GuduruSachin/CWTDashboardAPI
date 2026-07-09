using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Data;
using CWTDashboardAPI.Models;
using System.Globalization;

namespace CWTDashboardAPI.Controllers
{
    public class CLRColumnController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        Filters fi = new Filters();

        [HttpPost]
        [Route("GetCLRSelectedColumns")]
        public Response GetCLRSelectedColumns(CLRColumnsHiding cLRColumnsHiding)
        {
            var columnNames = entity.CLRColumnsHidings.Where(x => x.UID == cLRColumnsHiding.UID).Count();
            if(columnNames == 0)
            {
                DateTime Today = DateTime.Now;
                try
                {
                    entity.CLRColumnsHidings.Add(
                            new CLRColumnsHiding
                            {
                                UID = cLRColumnsHiding.UID,
                                Priority = true,
                                Implementation_Type = true,
                                Pipeline_status = true,
                                Pipeline_comments = true,
                                TXResourcing = true,
                                Service_location = true,
                                Service_configuration = true,
                                OBT_Reseller___Direct = true,
                                OBTAdoptionRate = true,
                                ExpectedDecisionDate_c = true,
                                Assignment_date_c = true,
                                ResourseRequestedDate_c = true,
                                UpdateOn_c = true,
                                OppVolume = true,
                                RevenueVolumeUSD = true,
                                Region = true,
                                OwnerShip = true,
                                ProjectStart_ForCycleTime_c = true,
                                GoLiveDate_c = true,
                                PerCompleted = true,
                                CycleTime = true,
                                CycleTimeCategories = true,
                                CycleTimeDelayCode = true,
                                EltClientDelayDescription = true,
                                ProjectStatus = true,
                                Milestone__Reason_Code = true,
                                CountryStatus = true,
                                ProjectLevel = true,
                                CompletedDate_c = true,
                                GlobalProjectManager = true,
                                RegionalProjectManager = true,
                                AssigneeFullName = true,
                                GlobalCISDQSLead = true,
                                APAC_DQS = true,
                                DQS_Import = true,
                                DQS_Support = true,
                                LATAM_DQS = true,
                                NORAM_DQS = true,
                                DQS_Operations = true,
                                GlobalCISOBTLead = true,
                                RegionalCISOBTLead = true,
                                LocalDigitalOBTLead = true,
                                GlobalCISPortraitLead = true,
                                RegionalCISPortraitLead = true,
                                GlobalCISHRFeedSpecialist = true,
                                GDS = true,
                                ActivityType = true,
                                ComplexityScore = true,
                                MilestoneTitle = true,
                                Group_Name = true,
                                Milestone__Project_Notes = true,
                                Milestone__Closed_Loop_Owner = true,
                                Workspace__ELT_Overall_Status = true,
                                Workspace__ELT_Overall_Comments = true,
                                NpsScore = true,
                                Customer_Row_ID = true,
                                Opportunity_ID = true,
                                AccountOwner = true,
                                Opportunity_Type = true,
                                Revenue_Status = true,
                                Revenue_Opportunity_Type = true,
                                Opportunity_Owner = true,
                                Opportunity_Category = true,
                                Revenue_Total_Transactions = true,
                                Line_Win_Probability = true,
                                Implementation_Fee__PSD_ = true,
                                Next_Step = true,
                                AwardedDate_c = true,
                                DataDescription = true,
                                Date_added_to_the_CLR_c = true,
                                CreatedDate_c = true,
                                Project_Effort = true,
                                Sales_Stage_Name = true,
                                AccountCategory = true,
                                SOWStatus = true,
                                ImplementationReady = true,
                                RecordHistory_C = true,
                                DataSourceType = true,
                                DataQuality = true,
                                CreatedOn = Today
                            });
                    entity.SaveChanges();
                    var data = (from a in entity.CLRColumnsHidings
                                where a.UID == cLRColumnsHiding.UID
                                select new
                                {
                                    a.Priority,
                                    a.Implementation_Type,
                                    a.Pipeline_status,
                                    a.Pipeline_comments,
                                    a.TXResourcing,
                                    a.Service_location,
                                    a.Service_configuration,
                                    a.OBT_Reseller___Direct,
                                    a.OBTAdoptionRate,
                                    a.ExpectedDecisionDate_c,
                                    a.Assignment_date_c,
                                    a.ResourseRequestedDate_c,
                                    a.UpdateOn_c,
                                    a.OppVolume,
                                    a.RevenueVolumeUSD,
                                    a.Region,
                                    a.OwnerShip,
                                    a.ProjectStart_ForCycleTime_c,
                                    a.GoLiveDate_c,
                                    a.PerCompleted,
                                    a.CycleTime,
                                    a.CycleTimeCategories,
                                    a.CycleTimeDelayCode,
                                    a.EltClientDelayDescription,
                                    a.ProjectStatus,
                                    a.Milestone__Reason_Code,
                                    a.CountryStatus,
                                    a.ProjectLevel,
                                    a.CompletedDate_c,
                                    a.GlobalProjectManager,
                                    a.RegionalProjectManager,
                                    a.AssigneeFullName,
                                    a.GlobalCISDQSLead,
                                    a.APAC_DQS,
                                    a.DQS_Import,
                                    a.DQS_Support,
                                    a.LATAM_DQS,
                                    a.NORAM_DQS,
                                    a.DQS_Operations,
                                    a.GlobalCISOBTLead,
                                    a.RegionalCISOBTLead,
                                    a.LocalDigitalOBTLead,
                                    a.GlobalCISPortraitLead,
                                    a.RegionalCISPortraitLead,
                                    a.GlobalCISHRFeedSpecialist,
                                    a.GDS,
                                    a.ActivityType,
                                    a.ComplexityScore,
                                    a.MilestoneTitle,
                                    a.Group_Name,
                                    a.Milestone__Project_Notes,
                                    a.Milestone__Closed_Loop_Owner,
                                    a.Workspace__ELT_Overall_Status,
                                    a.Workspace__ELT_Overall_Comments,
                                    a.NpsScore,
                                    a.Customer_Row_ID,
                                    a.Opportunity_ID,
                                    a.AccountOwner,
                                    a.Opportunity_Type,
                                    a.Revenue_Status,
                                    a.Revenue_Opportunity_Type,
                                    a.Opportunity_Owner,
                                    a.Opportunity_Category,
                                    a.Revenue_Total_Transactions,
                                    a.Line_Win_Probability,
                                    a.Implementation_Fee__PSD_,
                                    a.Next_Step,
                                    a.AwardedDate_c,
                                    a.DataDescription,
                                    a.Date_added_to_the_CLR_c,
                                    a.CreatedDate_c,
                                    a.Project_Effort,
                                    a.Sales_Stage_Name,
                                    a.AccountCategory,
                                    a.SOWStatus,
                                    a.ImplementationReady,
                                    a.RecordHistory_C,
                                    a.DataSourceType,
                                    a.DataQuality,
                                }).ToList();
                    re.code = 200;
                    re.message = "Sucess";
                    re.CLRColumns = data;
                }
                catch(Exception e)
                {
                    re.code = 400;
                    re.message = e.Message;
                }
                return re;
            }
            else
            {
                var data = (from a in entity.CLRColumnsHidings
                            where a.UID == cLRColumnsHiding.UID
                            select new {
                                a.Priority,
                                a.Implementation_Type,
                                a.Pipeline_status,
                                a.Pipeline_comments,
                                a.TXResourcing,
                                a.Service_location,
                                a.Service_configuration,
                                a.OBT_Reseller___Direct,
                                a.OBTAdoptionRate,
                                a.ExpectedDecisionDate_c,
                                a.Assignment_date_c,
                                a.ResourseRequestedDate_c,
                                a.UpdateOn_c,
                                a.OppVolume,
                                a.RevenueVolumeUSD,
                                a.Region,
                                a.OwnerShip,
                                a.ProjectStart_ForCycleTime_c,
                                a.GoLiveDate_c,
                                a.PerCompleted,
                                a.CycleTime,
                                a.CycleTimeCategories,
                                a.CycleTimeDelayCode,
                                a.EltClientDelayDescription,
                                a.ProjectStatus,
                                a.Milestone__Reason_Code,
                                a.CountryStatus,
                                a.ProjectLevel,
                                a.CompletedDate_c,
                                a.GlobalProjectManager,
                                a.RegionalProjectManager,
                                a.AssigneeFullName,
                                a.GlobalCISDQSLead,
                                a.APAC_DQS,
                                a.DQS_Import,
                                a.DQS_Support,
                                a.LATAM_DQS,
                                a.NORAM_DQS,
                                a.DQS_Operations,
                                a.GlobalCISOBTLead,
                                a.RegionalCISOBTLead,
                                a.LocalDigitalOBTLead,
                                a.GlobalCISPortraitLead,
                                a.RegionalCISPortraitLead,
                                a.GlobalCISHRFeedSpecialist,
                                a.GDS,
                                a.ActivityType,
                                a.ComplexityScore,
                                a.MilestoneTitle,
                                a.Group_Name,
                                a.Milestone__Project_Notes,
                                a.Milestone__Closed_Loop_Owner,
                                a.Workspace__ELT_Overall_Status,
                                a.Workspace__ELT_Overall_Comments,
                                a.NpsScore,
                                a.Customer_Row_ID,
                                a.Opportunity_ID,
                                a.AccountOwner,
                                a.Opportunity_Type,
                                a.Revenue_Status,
                                a.Revenue_Opportunity_Type,
                                a.Opportunity_Owner,
                                a.Opportunity_Category,
                                a.Revenue_Total_Transactions,
                                a.Line_Win_Probability,
                                a.Implementation_Fee__PSD_,
                                a.Next_Step,
                                a.AwardedDate_c,
                                a.DataDescription,
                                a.Date_added_to_the_CLR_c,
                                a.CreatedDate_c,
                                a.Project_Effort,
                                a.Sales_Stage_Name,
                                a.AccountCategory,
                                a.SOWStatus,
                                a.ImplementationReady,
                                a.RecordHistory_C,
                                a.DataSourceType,
                                a.DataQuality,
                            }).ToList();
                re.code = 200;
                re.message = "Sucess";
                re.CLRColumns = data;
                return re;
            }
            
        }

        string UID;
        int clr_count;
        [HttpPost]
        [Route("CLRColumnsUpdate")]
        public Response CLRColumnsUpdate(CLRColumnsHiding cLRColumnsHiding)
        {
            List<CLRColumnsHiding> list = new List<CLRColumnsHiding>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.CLRColumnsHidings.OrderBy(a => a.UID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    UID = list[i].UID;
                    if (cLRColumnsHiding.UID.Equals(UID))
                    {
                        clr_count = 1;
                        break;
                    }
                    else
                    {
                        clr_count = 0;
                    }
                }
                if (clr_count == 0)
                {
                    re.code = 101;
                    re.message = "User Not Found";
                    return re;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        CLRColumnsHiding clr_hiding = (from clr in entit.CLRColumnsHidings
                                          where clr.UID == cLRColumnsHiding.UID
                                          select clr).FirstOrDefault();
                        clr_hiding.Priority = cLRColumnsHiding.Priority;
                        clr_hiding.Implementation_Type = cLRColumnsHiding.Implementation_Type;
                        clr_hiding.Pipeline_status = cLRColumnsHiding.Pipeline_status;
                        clr_hiding.Pipeline_comments = cLRColumnsHiding.Pipeline_comments;
                        clr_hiding.TXResourcing = cLRColumnsHiding.TXResourcing;
                        clr_hiding.Service_location = cLRColumnsHiding.Service_location;
                        clr_hiding.Service_configuration = cLRColumnsHiding.Service_configuration;
                        clr_hiding.OBT_Reseller___Direct = cLRColumnsHiding.OBT_Reseller___Direct;
                        clr_hiding.OBTAdoptionRate = cLRColumnsHiding.OBTAdoptionRate;
                        clr_hiding.ExpectedDecisionDate_c = cLRColumnsHiding.ExpectedDecisionDate_c;
                        clr_hiding.Assignment_date_c = cLRColumnsHiding.Assignment_date_c;
                        clr_hiding.ResourseRequestedDate_c = cLRColumnsHiding.ResourseRequestedDate_c;
                        clr_hiding.UpdateOn_c = cLRColumnsHiding.UpdateOn_c;
                        clr_hiding.OppVolume = cLRColumnsHiding.OppVolume;
                        clr_hiding.RevenueVolumeUSD = cLRColumnsHiding.RevenueVolumeUSD;
                        clr_hiding.Region = cLRColumnsHiding.Region;
                        clr_hiding.OwnerShip = cLRColumnsHiding.OwnerShip;
                        clr_hiding.ProjectStart_ForCycleTime_c = cLRColumnsHiding.ProjectStart_ForCycleTime_c;
                        clr_hiding.GoLiveDate_c = cLRColumnsHiding.GoLiveDate_c;
                        clr_hiding.PerCompleted = cLRColumnsHiding.PerCompleted;
                        clr_hiding.CycleTime = cLRColumnsHiding.CycleTime;
                        clr_hiding.CycleTimeCategories = cLRColumnsHiding.CycleTimeCategories;
                        clr_hiding.CycleTimeDelayCode = cLRColumnsHiding.CycleTimeDelayCode;
                        clr_hiding.EltClientDelayDescription = cLRColumnsHiding.EltClientDelayDescription;
                        clr_hiding.ProjectStatus = cLRColumnsHiding.ProjectStatus;
                        clr_hiding.Milestone__Reason_Code = cLRColumnsHiding.Milestone__Reason_Code;
                        clr_hiding.CountryStatus = cLRColumnsHiding.CountryStatus;
                        clr_hiding.ProjectLevel = cLRColumnsHiding.ProjectLevel;
                        clr_hiding.CompletedDate_c = cLRColumnsHiding.CompletedDate_c;
                        clr_hiding.GlobalProjectManager = cLRColumnsHiding.GlobalProjectManager;
                        clr_hiding.RegionalProjectManager = cLRColumnsHiding.RegionalProjectManager;
                        clr_hiding.AssigneeFullName = cLRColumnsHiding.AssigneeFullName;
                        clr_hiding.GlobalCISDQSLead = cLRColumnsHiding.GlobalCISDQSLead;
                        clr_hiding.APAC_DQS = cLRColumnsHiding.APAC_DQS;
                        clr_hiding.DQS_Import = cLRColumnsHiding.DQS_Import;
                        clr_hiding.DQS_Support = cLRColumnsHiding.DQS_Support;
                        clr_hiding.LATAM_DQS = cLRColumnsHiding.LATAM_DQS;
                        clr_hiding.NORAM_DQS = cLRColumnsHiding.NORAM_DQS;
                        clr_hiding.DQS_Operations = cLRColumnsHiding.DQS_Operations;
                        clr_hiding.GlobalCISOBTLead = cLRColumnsHiding.GlobalCISOBTLead;
                        clr_hiding.RegionalCISOBTLead = cLRColumnsHiding.RegionalCISOBTLead;
                        clr_hiding.LocalDigitalOBTLead = cLRColumnsHiding.LocalDigitalOBTLead;
                        clr_hiding.GlobalCISPortraitLead = cLRColumnsHiding.GlobalCISPortraitLead;
                        clr_hiding.RegionalCISPortraitLead = cLRColumnsHiding.RegionalCISPortraitLead;
                        clr_hiding.GlobalCISHRFeedSpecialist = cLRColumnsHiding.GlobalCISHRFeedSpecialist;
                        clr_hiding.GDS = cLRColumnsHiding.GDS;
                        clr_hiding.ActivityType = cLRColumnsHiding.ActivityType;
                        clr_hiding.ComplexityScore = cLRColumnsHiding.ComplexityScore;
                        clr_hiding.MilestoneTitle = cLRColumnsHiding.MilestoneTitle;
                        clr_hiding.Group_Name = cLRColumnsHiding.Group_Name;
                        clr_hiding.Milestone__Project_Notes = cLRColumnsHiding.Milestone__Project_Notes;
                        clr_hiding.Milestone__Closed_Loop_Owner = cLRColumnsHiding.Milestone__Closed_Loop_Owner;
                        clr_hiding.Workspace__ELT_Overall_Status = cLRColumnsHiding.Workspace__ELT_Overall_Status;
                        clr_hiding.Workspace__ELT_Overall_Comments = cLRColumnsHiding.Workspace__ELT_Overall_Comments;
                        clr_hiding.NpsScore = cLRColumnsHiding.NpsScore;
                        clr_hiding.Customer_Row_ID = cLRColumnsHiding.Customer_Row_ID;
                        clr_hiding.Opportunity_ID = cLRColumnsHiding.Opportunity_ID;
                        clr_hiding.AccountOwner = cLRColumnsHiding.AccountOwner;
                        clr_hiding.Opportunity_Type = cLRColumnsHiding.Opportunity_Type;
                        clr_hiding.Revenue_Status = cLRColumnsHiding.Revenue_Status;
                        clr_hiding.Revenue_Opportunity_Type = cLRColumnsHiding.Revenue_Opportunity_Type;
                        clr_hiding.Opportunity_Owner = cLRColumnsHiding.Opportunity_Owner;
                        clr_hiding.Opportunity_Category = cLRColumnsHiding.Opportunity_Category;
                        clr_hiding.Revenue_Total_Transactions = cLRColumnsHiding.Revenue_Total_Transactions;
                        clr_hiding.Line_Win_Probability = cLRColumnsHiding.Line_Win_Probability;
                        clr_hiding.Implementation_Fee__PSD_ = cLRColumnsHiding.Implementation_Fee__PSD_;
                        clr_hiding.Next_Step = cLRColumnsHiding.Next_Step;
                        clr_hiding.AwardedDate_c = cLRColumnsHiding.AwardedDate_c;
                        clr_hiding.DataDescription = cLRColumnsHiding.DataDescription;
                        clr_hiding.Date_added_to_the_CLR_c = cLRColumnsHiding.Date_added_to_the_CLR_c;
                        clr_hiding.CreatedDate_c = cLRColumnsHiding.CreatedDate_c;
                        clr_hiding.Project_Effort = cLRColumnsHiding.Project_Effort;
                        clr_hiding.Sales_Stage_Name = cLRColumnsHiding.Sales_Stage_Name;
                        clr_hiding.AccountCategory = cLRColumnsHiding.AccountCategory;
                        clr_hiding.SOWStatus = cLRColumnsHiding.SOWStatus;
                        clr_hiding.ImplementationReady = cLRColumnsHiding.ImplementationReady;
                        clr_hiding.RecordHistory_C = cLRColumnsHiding.RecordHistory_C;
                        clr_hiding.DataSourceType = cLRColumnsHiding.DataSourceType;
                        clr_hiding.DataQuality = cLRColumnsHiding.DataQuality;
                        DateTime TodayDate = DateTime.Now;
                        clr_hiding.UpdatedOn = TodayDate;
                        entit.SaveChanges();
                    }
                    re.code = 200;
                    re.message = "Success";
                    return re;
                }
            }
        }
    }
}