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
    public class ManualDataController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        H_Filters fi = new H_Filters();
        IMRCResponse imrs = new IMRCResponse();
        AutomatedCLRFilters CLR_F = new AutomatedCLRFilters();
        int MDataCount, CLRDataCount;
        // GET: ManualData
        [HttpPost]
        [Route("GetManualData")]
        public Response GetManualData(ManualData manualData)
        {
            var MDataLIst = (from a in entity.ManualDatas
                             select new
                             {
                                 a.Revenue_ID,
                                 a.Client,
                                 a.iMeet_Workspace_Title,
                                 a.Implementation_Type,
                                 a.CLR_Country,
                                 a.Pipeline_status,
                                 a.Pipeline_comments,
                                 a.Service_configuration,
                                 a.OBT_Reseller___Direct,
                                 a.Servicing_location,
                                 a.Assignment_date,
                                 a.Project_Effort,
                             }).ToList();
            MDataCount = MDataLIst.AsQueryable().Count();
            if (MDataCount.ToString() == "" || MDataCount.ToString() == null || MDataCount == 0)
            {
                re.Data = MDataLIst;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                re.Data = MDataLIst;
                re.code = 200;
                re.message = "Data Successfull";
            }
            return re;
        }
        string[] CLR_GoliveYear, CLR_RecordStatus;
        [HttpPost]
        [Route("GetCLRManualData")]
        public Response CLRData(CLRData cLRData)
        {
            if (cLRData.GoLiveYear == null || cLRData.Status == null || cLRData.GoLiveYear == "" || cLRData.Status == "")
            {
                re.Data = null;
                re.code = 100;
                re.message = "No Data found";
            }else{
                CLR_GoliveYear = cLRData.GoLiveYear.Split(',');
                for (int i = 0; i < CLR_GoliveYear.Count(); i++)
                {
                    if (CLR_GoliveYear[i] == "" || CLR_GoliveYear[i] == "null")
                    {
                        CLR_GoliveYear[i] = null;
                    }
                }
                CLR_RecordStatus = cLRData.Status.Split(',');
                for (int i = 0; i < CLR_RecordStatus.Count(); i++)
                {
                    if (CLR_RecordStatus[i] == "" || CLR_RecordStatus[i] == "null")
                    {
                        CLR_RecordStatus[i] = null;
                    }
                }
                var CurrentYear = DateTime.Now.Year + "";
                var Datalist4 = (from a in entity.CLRDatas
                                 where a.RevenueID > 600000000000000
                                 where CLR_GoliveYear.Any(val => a.GoLiveYear.Equals(val))
                                 where CLR_RecordStatus.Any(val => a.Status.Equals(val))
                                 join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new
                                 {
                                     a.CLRID,
                                     ManualID = abc.ManualID,
                                     Revenue_ID = a.RevenueID,
                                     Client = a.Client == "" ? "---" : a.Client ?? "---",
                                     a.CreatedDate,
                                     Date_added_to_the_CLR = a.LastUpdateDate,
                                     iMeet_Workspace_Title = "---",
                                     Implementation_Type = "---",
                                     Pipeline_status = "---",
                                     Pipeline_comments = "---",
                                     //Service_configuration = "---",
                                     //OBT_Reseller___Direct = "---",
                                     OBT_Reseller___Direct = a.OBTReseller == "" || a.OBTReseller == null ? "---" : a.OBTReseller ?? "---",
                                     abc.Assignment_date,
                                     abc.ResourseRequestedDate,
                                     RevenueVolumeUSD = (double)0,
                                     Region = a.Region == "" ? "---" : a.Region ?? "---",
                                     Country = a.Country == "" ? "---" : a.Country ?? "---",
                                     a.RevenueID,
                                     OwnerShip = "---",
                                     GoLiveDate = a.GoLiveDate,
                                     //GoLiveDate = a.ProjectStatus == "HP-High Potential" ? abc.GoLiveDate : a.GoLiveDate,
                                     ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                     Milestone__Reason_Code = "---",
                                     PerCompleted = 0,
                                     CountryStatus = "---",
                                     ProjectLevel = "---",
                                     a.CompletedDate,
                                     a.MilestoneDueDate,
                                     CountryCode = "---",
                                     GlobalProjectManager = "---",
                                     RegionalProjectManager = "---",
                                     GlobalCISDQSLead = a.GlobalCISDQSLead == "" ? "---" : a.GlobalCISDQSLead ?? "---",
                                     AssigneeFullName = "---",
                                     GlobalCISOBTLead = a.GlobalCISOBTLead == "" ? "---" : a.GlobalCISOBTLead ?? "---",
                                     RegionalCISOBTLead = a.RegionalCISOBTLead == "" ? "---" : a.RegionalCISOBTLead ?? "---",
                                     LocalDigitalOBTLead = a.LocalDigitalOBTLead == "" ? "---" : a.LocalDigitalOBTLead ?? "---",
                                     GlobalCISPortraitLead = a.GlobalCISPortraitLead == "" ? "---" : a.GlobalCISPortraitLead ?? "---",
                                     RegionalCISPortraitLead = a.RegionalCISPortraitLead == "" ? "---" : a.RegionalCISPortraitLead ?? "---",
                                     GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist == "" ? "---" : a.GlobalCISHRFeedSpecialist ?? "---",
                                     MilestoneTitle = "---",
                                     Group_Name = "---",
                                     Milestone__Project_Notes = "---",
                                     Milestone__Closed_Loop_Owner = "---",
                                     Workspace__ELT_Overall_Status = "---",
                                     Workspace__ELT_Overall_Comments = entity.AdHocProjects.FirstOrDefault(x => x.RevenueID == a.RevenueID).Comments ?? "---",
                                     Customer_Row_ID = (double)0,
                                     Opportunity_ID = (double)0,
                                     Account_Name = "---",
                                     Sales_Stage_Name = "---",
                                     Opportunity_Type = "---",
                                     Revenue_Status = "---",
                                     Revenue_Opportunity_Type = "---",
                                     Opportunity_Owner = "---",
                                     Opportunity_Category = "---",
                                     Revenue_Total_Transactions = (double)0,
                                     Line_Win_Probability = 0,
                                     Next_Step = "---",
                                     Implementation_Fee__PSD_ = (double)0,
                                     Project_Effort = (double)0,
                                     CycleTime = a.CycleTime ?? 0,
                                     a.ExternalKickoffDuedate,
                                     GoLiveYear = a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                     GoLiveMonth = a.GoLiveMonth == "" ? "---" : a.GoLiveMonth ?? "---",
                                     Quarter = a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     a.ProjectStart_ForCycleTime,
                                     RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                                     a.DataSourceType,
                                     DataDescription = "---",
                                     a.AwardedDate,
                                     a.ClosedDate,
                                     TaskStatus = "---",
                                     ActivityType = entity.AdHocProjects.FirstOrDefault(x => x.RevenueID == a.RevenueID).ActivityType == "" ? "---" : entity.AdHocProjects.FirstOrDefault(x => x.RevenueID == a.RevenueID).ActivityType ?? "---",
                                     //GDS = a.GDS == "" ? "---" : a.GDS ?? "---",
                                     DTID = entity.AdHocProjects.FirstOrDefault(x => x.RevenueID == a.RevenueID).AHID,
                                     ComplexityScore = entity.AdHocProjects.FirstOrDefault(x => x.RevenueID == a.RevenueID).ComplexityScore ?? 0,
                                     AccountOwner = "---",
                                     MilestoneType = "---",
                                     CycleTimeCategories = a.CycleTimeCategories == null || a.CycleTimeCategories == "" ? "---" : a.CycleTimeCategories,
                                     a.YearMonth,
                                     a.AccountCategory,
                                     a.SOWStatus,
                                     a.ImplementationReady,
                                     UpdateOn = abc.UpdateOn,
                                     OppVolume = (double)0,
                                     Service_configuration = a.Service_Configuration == "" || a.Service_Configuration == null ? "---" : a.Service_Configuration,
                                     Service_location = a.Service_Location == "" || a.Service_Location == null ? "---" : a.Service_Location,
                                     GDS = a.eSowGDS == "" || a.eSowGDS == null ? "---" : a.eSowGDS,
                                     LocalDigitalAdHocSupport = a.LocalDigitalAdHocSupport == "" || a.LocalDigitalAdHocSupport == null ? "---" : a.LocalDigitalAdHocSupport ?? "---",
                                     OBTAdoptionRate = a.OBTAdoptionRate == "" || a.OBTAdoptionRate == null ? "---" : a.OBTAdoptionRate,
                                     CycleTimeDelayCode = a.CycleTimeDelayCode == "" || a.CycleTimeDelayCode == null ? "---" : a.CycleTimeDelayCode ?? "---",
                                     EltClientDelayDescription = a.EltClientDelayDescription == "" || a.EltClientDelayDescription == null ? "---" : a.EltClientDelayDescription ?? "---",
                                     Cycletimetarget = a.CycleTimeCategories == "Existing Service Config Change (catch all including Spins/Mergers)" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).ExistingServiceConfigChange
                                                        : a.CycleTimeCategories == "New Global Including Upsell" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).NewGlobal
                                                        : a.CycleTimeCategories == "New Local Including Upsell" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).NewLocal
                                                        : a.CycleTimeCategories == "Existing Add/Change OBT" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).ExistingAddChange
                                                        : "0" ?? "0",
                                     Promoter = 0,
                                     Detractor = 0,
                                     Passive = 0,
                                     NpsTotal = 0,
                                     APAC_DQS = a.APAC_DQS == "" ? "---" : a.APAC_DQS ?? "---",
                                     DQS_Import = a.DQS_Import == "" ? "---" : a.DQS_Import ?? "---",
                                     DQS_Support = a.DQS_Support == "" ? "---" : a.DQS_Support ?? "---",
                                     LATAM_DQS = a.LATAM_DQS == "" ? "---" : a.LATAM_DQS ?? "---",
                                     NORAM_DQS = a.NORAM_DQS == "" ? "---" : a.NORAM_DQS ?? "---",
                                     DQS_Operations = a.DQS_Operations == "" ? "---" : a.DQS_Operations ?? "---",
                                 }).ToList();
                var Datalist1 = (from a in entity.CLRDatas
                                 where a.ProjectStatus != "P-Pipeline"
                                 where a.ProjectStatus != "HP-High Potential"
                                 where a.ProjectStatus != "EP-Early Potential"
                                 //where a.Status == "Active"
                                 //where a.GoLiveYear != "2020"
                                 //where a.GoLiveYear != "2021"
                                 where CLR_GoliveYear.Any(val => a.GoLiveYear.Equals(val))
                                 where CLR_RecordStatus.Any(val => a.Status.Equals(val))
                                 where a.RevenueID != 400000000000000
                                 where a.RevenueID < 600000000000000
                                 //join c in entity.DigitalTeams on a.RevenueID equals c.RevenueID
                                 join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new
                                 {
                                     a.CLRID,
                                     ManualID = abc.ManualID,
                                     Revenue_ID = abc.Revenue_ID ?? 0,
                                     Client = a.Client == "" ? "---" : a.Client ?? "---",
                                     a.CreatedDate,
                                     Date_added_to_the_CLR = a.LastUpdateDate,
                                     iMeet_Workspace_Title = a.Workspace_Title == "" ? "---" : a.Workspace_Title ?? "---",
                                     Implementation_Type = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                     Pipeline_status = abc.Pipeline_status == "" ? "---" : abc.Pipeline_status ?? "---",
                                     Pipeline_comments = abc.Pipeline_comments == "" ? "---" : abc.Pipeline_comments ?? "---",
                                     //Service_configuration = abc.Service_configuration == "" ? "---" : abc.Service_configuration ?? "---",
                                     //OBT_Reseller___Direct = abc.OBT_Reseller___Direct == "" ? "---" : abc.OBT_Reseller___Direct ?? "---",
                                     OBT_Reseller___Direct = a.OBTReseller == "" || a.OBTReseller == null ? "---" : a.OBTReseller ?? "---",
                                     abc.Assignment_date,
                                     abc.ResourseRequestedDate,
                                     RevenueVolumeUSD = a.RevenueVolumeUSD ?? 0,
                                     Region = a.Region == "" ? "---" : a.Region ?? "---",
                                     Country = a.Country == "" ? "---" : a.Country ?? "---",
                                     a.RevenueID,
                                     OwnerShip = a.OwnerShip == "" ? "---" : a.OwnerShip ?? "---",
                                     GoLiveDate = a.GoLiveDate,
                                     //GoLiveDate = a.ProjectStatus == "HP-High Potential" ? abc.GoLiveDate : a.GoLiveDate,
                                     ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                     Milestone__Reason_Code = a.Milestone__Reason_Code == "" ? "---" : a.Milestone__Reason_Code ?? "---",
                                     PerCompleted = a.PerCompleted ?? 0,
                                     CountryStatus = a.CountryStatus == "" ? "---" : a.CountryStatus ?? "---",
                                     ProjectLevel = a.ProjectLevel == "" ? "---" : a.ProjectLevel ?? "---",
                                     a.CompletedDate,
                                     a.MilestoneDueDate,
                                     CountryCode = a.CountryCode == "" ? "---" : a.CountryCode ?? "---",
                                     GlobalProjectManager = a.GlobalProjectManager == "" ? "---" : a.GlobalProjectManager ?? "---",
                                     RegionalProjectManager = a.RegionalProjectManager == "" ? "---" : a.RegionalProjectManager ?? "---",
                                     GlobalCISDQSLead = a.GlobalCISDQSLead == "" ? "---" : a.GlobalCISDQSLead ?? "---",
                                     AssigneeFullName = a.AssigneeFullName == "" ? "---" : a.AssigneeFullName ?? "---",
                                     GlobalCISOBTLead = a.GlobalCISOBTLead == "" ? "---" : a.GlobalCISOBTLead ?? "---",
                                     RegionalCISOBTLead = a.RegionalCISOBTLead == "" ? "---" : a.RegionalCISOBTLead ?? "---",
                                     LocalDigitalOBTLead = a.LocalDigitalOBTLead == "" ? "---" : a.LocalDigitalOBTLead ?? "---",
                                     GlobalCISPortraitLead = a.GlobalCISPortraitLead == "" ? "---" : a.GlobalCISPortraitLead ?? "---",
                                     RegionalCISPortraitLead = a.RegionalCISPortraitLead == "" ? "---" : a.RegionalCISPortraitLead ?? "---",
                                     GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist == "" ? "---" : a.GlobalCISHRFeedSpecialist ?? "---",
                                     MilestoneTitle = a.MilestoneTitle == "" ? "---" : a.MilestoneTitle ?? "---",
                                     Group_Name = a.Group_Name == "" ? "---" : a.Group_Name ?? "---",
                                     Milestone__Project_Notes = a.Milestone__Project_Notes == "" ? "---" : a.Milestone__Project_Notes ?? "---",
                                     Milestone__Closed_Loop_Owner = a.Milestone__Closed_Loop_Owner == "" ? "---" : a.Milestone__Closed_Loop_Owner ?? "---",
                                     Workspace__ELT_Overall_Status = a.Workspace__ELT_Overall_Status == "" ? "---" : a.Workspace__ELT_Overall_Status ?? "---",
                                     Workspace__ELT_Overall_Comments = a.Workspace__ELT_Overall_Comments == "" ? "---" : a.Workspace__ELT_Overall_Comments ?? "---",
                                     Customer_Row_ID = a.Customer_Row_ID ?? 0,
                                     Opportunity_ID = a.Opportunity_ID ?? 0,
                                     Account_Name = a.Account_Name == "" ? "---" : a.Account_Name ?? "---",
                                     Sales_Stage_Name = a.Sales_Stage_Name == "" ? "---" : a.Sales_Stage_Name ?? "---",
                                     Opportunity_Type = a.Opportunity_Type == "" ? "---" : a.Opportunity_Type ?? "---",
                                     Revenue_Status = a.Revenue_Status == "" ? "---" : a.Revenue_Status ?? "---",
                                     Revenue_Opportunity_Type = a.Revenue_Opportunity_Type == "" ? "---" : a.Revenue_Opportunity_Type ?? "---",
                                     Opportunity_Owner = a.Opportunity_Owner == "" ? "---" : a.Opportunity_Owner ?? "---",
                                     Opportunity_Category = a.Opportunity_Category == "" ? "---" : a.Opportunity_Category ?? "---",
                                     Revenue_Total_Transactions = a.Revenue_Total_Transactions ?? 0,
                                     Line_Win_Probability = a.Line_Win_Probability ?? 0,
                                     Next_Step = a.Next_Step == "" ? "---" : a.Next_Step ?? "---",
                                     Implementation_Fee__PSD_ = a.ImplementationFeePsd ?? 0,
                                     Project_Effort = abc.Project_Effort ?? 0,
                                     CycleTime = a.CycleTime ?? 0,
                                     a.ExternalKickoffDuedate,
                                     GoLiveYear = a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                     GoLiveMonth = a.GoLiveMonth == "" ? "---" : a.GoLiveMonth ?? "---",
                                     Quarter = a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     a.ProjectStart_ForCycleTime,
                                     RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                                     a.DataSourceType,
                                     DataDescription = a.DataDescription == "" ? "---" : a.DataDescription ?? "---",
                                     a.AwardedDate,
                                     a.ClosedDate,
                                     TaskStatus = a.TaskStatus == "" ? "---" : a.TaskStatus ?? "---",
                                     ActivityType = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).ActivityType == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).ActivityType ?? "---",
                                     //GDS = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GDS == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GDS ?? "---",
                                     entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).DTID,
                                     ComplexityScore = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).ComplexityScore,
                                     AccountOwner = a.AccountOwner == "" ? "---" : a.AccountOwner ?? "---",
                                     MilestoneType = a.MilestoneType == "" ? "---" : a.MilestoneType ?? "---",
                                     CycleTimeCategories = a.CycleTimeCategories == null || a.CycleTimeCategories == "" ? "---" : a.CycleTimeCategories,
                                     a.YearMonth,
                                     a.AccountCategory,
                                     a.SOWStatus,
                                     a.ImplementationReady,
                                     UpdateOn = abc.UpdateOn,
                                     OppVolume = a.OppTOtalVolume ?? 0,
                                     Service_configuration = a.Service_Configuration == "" || a.Service_Configuration == null ? "---" : a.Service_Configuration,
                                     Service_location = a.Service_Location == "" || a.Service_Location == null ? "---" : a.Service_Location,
                                     GDS = a.eSowGDS == "" || a.eSowGDS == null ? "---" : a.eSowGDS,
                                     LocalDigitalAdHocSupport = a.LocalDigitalAdHocSupport == "" || a.LocalDigitalAdHocSupport == null ? "---" : a.LocalDigitalAdHocSupport ?? "---",
                                     OBTAdoptionRate = a.OBTAdoptionRate == "" || a.OBTAdoptionRate == null ? "---" : a.OBTAdoptionRate,
                                     CycleTimeDelayCode = a.CycleTimeDelayCode == "" || a.CycleTimeDelayCode == null ? "---" : a.CycleTimeDelayCode ?? "---",
                                     EltClientDelayDescription = a.EltClientDelayDescription == "" || a.EltClientDelayDescription == null ? "---" : a.EltClientDelayDescription ?? "---",
                                     Cycletimetarget = a.CycleTimeCategories == "Existing Service Config Change (catch all including Spins/Mergers)" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).ExistingServiceConfigChange
                                                        : a.CycleTimeCategories == "New Global Including Upsell" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).NewGlobal
                                                        : a.CycleTimeCategories == "New Local Including Upsell" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).NewLocal
                                                        : a.CycleTimeCategories == "Existing Add/Change OBT" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).ExistingAddChange
                                                        : "0" ?? "0",
                                     Promoter = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID && x.NPSIndicator == "Promoter").Count(),
                                     Detractor = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID && x.NPSIndicator == "Detractor").Count(),
                                     Passive = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID && x.NPSIndicator == "Passive").Count(),
                                     NpsTotal = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID).Count(),
                                     APAC_DQS = a.APAC_DQS == "" ? "---" : a.APAC_DQS ?? "---",
                                     DQS_Import = a.DQS_Import == "" ? "---" : a.DQS_Import ?? "---",
                                     DQS_Support = a.DQS_Support == "" ? "---" : a.DQS_Support ?? "---",
                                     LATAM_DQS = a.LATAM_DQS == "" ? "---" : a.LATAM_DQS ?? "---",
                                     NORAM_DQS = a.NORAM_DQS == "" ? "---" : a.NORAM_DQS ?? "---",
                                     DQS_Operations = a.DQS_Operations == "" ? "---" : a.DQS_Operations ?? "---",
                                 }).ToList();
                var Datalist3 = (from a in entity.CLRDatas
                                 where a.RevenueID == 400000000000000
                                 //where a.Status == "Active"
                                 //where a.GoLiveYear != "2020"
                                 //where a.GoLiveYear != "2021"
                                 where CLR_GoliveYear.Any(val => a.GoLiveYear.Equals(val))
                                 where CLR_RecordStatus.Any(val => a.Status.Equals(val))
                                 //join c in entity.DigitalTeams on a.Task__Task_Record_ID_Key equals c.TaskRecordIdKey
                                 join b in entity.ManualDatas on a.Task__Task_Record_ID_Key equals b.TaskRecordIdKey into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new
                                 {
                                     a.CLRID,
                                     ManualID = abc.ManualID,
                                     Revenue_ID = abc.Revenue_ID ?? 0,
                                     Client = a.Client == "" ? "---" : a.Client ?? "---",
                                     a.CreatedDate,
                                     Date_added_to_the_CLR = a.LastUpdateDate,
                                     iMeet_Workspace_Title = a.Workspace_Title == "" ? "---" : a.Workspace_Title ?? "---",
                                     Implementation_Type = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                     Pipeline_status = abc.Pipeline_status == "" ? "---" : abc.Pipeline_status ?? "---",
                                     Pipeline_comments = abc.Pipeline_comments == "" ? "---" : abc.Pipeline_comments ?? "---",
                                     //Service_configuration = abc.Service_configuration == "" ? "---" : abc.Service_configuration ?? "---",
                                     //OBT_Reseller___Direct = abc.OBT_Reseller___Direct == "" ? "---" : abc.OBT_Reseller___Direct ?? "---",
                                     OBT_Reseller___Direct = a.OBTReseller == "" || a.OBTReseller == null ? "---" : a.OBTReseller ?? "---",
                                     abc.Assignment_date,
                                     abc.ResourseRequestedDate,
                                     RevenueVolumeUSD = a.RevenueVolumeUSD ?? 0,
                                     Region = a.Region == "" ? "---" : a.Region ?? "---",
                                     Country = a.Country == "" ? "---" : a.Country ?? "---",
                                     a.RevenueID,
                                     OwnerShip = a.OwnerShip == "" ? "---" : a.OwnerShip ?? "---",
                                     GoLiveDate = a.GoLiveDate,
                                     ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                     Milestone__Reason_Code = a.Milestone__Reason_Code == "" ? "---" : a.Milestone__Reason_Code ?? "---",
                                     PerCompleted = a.PerCompleted ?? 0,
                                     CountryStatus = a.CountryStatus == "" ? "---" : a.CountryStatus ?? "---",
                                     ProjectLevel = a.ProjectLevel == "" ? "---" : a.ProjectLevel ?? "---",
                                     a.CompletedDate,
                                     a.MilestoneDueDate,
                                     CountryCode = a.CountryCode == "" ? "---" : a.CountryCode ?? "---",
                                     GlobalProjectManager = a.GlobalProjectManager == "" ? "---" : a.GlobalProjectManager ?? "---",
                                     RegionalProjectManager = a.RegionalProjectManager == "" ? "---" : a.RegionalProjectManager ?? "---",
                                     GlobalCISDQSLead = a.GlobalCISDQSLead == "" || a.GlobalCISDQSLead == null ? "---" : a.GlobalCISDQSLead,
                                     AssigneeFullName = a.AssigneeFullName == "" ? "---" : a.AssigneeFullName ?? "---",
                                     GlobalCISOBTLead = a.GlobalCISOBTLead == "" ? "---" : a.GlobalCISOBTLead ?? "---",
                                     RegionalCISOBTLead = a.RegionalCISOBTLead == "" ? "---" : a.RegionalCISOBTLead ?? "---",
                                     LocalDigitalOBTLead = a.LocalDigitalOBTLead == "" ? "---" : a.LocalDigitalOBTLead ?? "---",
                                     GlobalCISPortraitLead = a.GlobalCISPortraitLead == "" ? "---" : a.GlobalCISPortraitLead ?? "---",
                                     RegionalCISPortraitLead = a.RegionalCISPortraitLead == "" ? "---" : a.RegionalCISPortraitLead ?? "---",
                                     GlobalCISHRFeedSpecialist = a.GlobalCISHRFeedSpecialist == "" ? "---" : a.GlobalCISHRFeedSpecialist ?? "---",
                                     MilestoneTitle = a.MilestoneTitle == "" ? "---" : a.MilestoneTitle ?? "---",
                                     Group_Name = a.Group_Name == "" ? "---" : a.Group_Name ?? "---",
                                     Milestone__Project_Notes = a.Milestone__Project_Notes == "" ? "---" : a.Milestone__Project_Notes ?? "---",
                                     Milestone__Closed_Loop_Owner = a.Milestone__Closed_Loop_Owner == "" ? "---" : a.Milestone__Closed_Loop_Owner ?? "---",
                                     Workspace__ELT_Overall_Status = a.Workspace__ELT_Overall_Status == "" ? "---" : a.Workspace__ELT_Overall_Status ?? "---",
                                     Workspace__ELT_Overall_Comments = a.Workspace__ELT_Overall_Comments == "" ? "---" : a.Workspace__ELT_Overall_Comments ?? "---",
                                     Customer_Row_ID = a.Customer_Row_ID ?? 0,
                                     Opportunity_ID = a.Opportunity_ID ?? 0,
                                     Account_Name = a.Account_Name == "" ? "---" : a.Account_Name ?? "---",
                                     Sales_Stage_Name = a.Sales_Stage_Name == "" ? "---" : a.Sales_Stage_Name ?? "---",
                                     Opportunity_Type = a.Opportunity_Type == "" ? "---" : a.Opportunity_Type ?? "---",
                                     Revenue_Status = a.Revenue_Status == "" ? "---" : a.Revenue_Status ?? "---",
                                     Revenue_Opportunity_Type = a.Revenue_Opportunity_Type == "" ? "---" : a.Revenue_Opportunity_Type ?? "---",
                                     Opportunity_Owner = a.Opportunity_Owner == "" ? "---" : a.Opportunity_Owner ?? "---",
                                     Opportunity_Category = a.Opportunity_Category == "" ? "---" : a.Opportunity_Category ?? "---",
                                     Revenue_Total_Transactions = a.Revenue_Total_Transactions ?? 0,
                                     Line_Win_Probability = a.Line_Win_Probability ?? 0,
                                     Next_Step = a.Next_Step == "" ? "---" : a.Next_Step ?? "---",
                                     Implementation_Fee__PSD_ = a.ImplementationFeePsd ?? 0,
                                     Project_Effort = abc.Project_Effort ?? 0,
                                     CycleTime = a.CycleTime ?? 0,
                                     a.ExternalKickoffDuedate,
                                     GoLiveYear = a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                     GoLiveMonth = a.GoLiveMonth == "" ? "---" : a.GoLiveMonth ?? "---",
                                     Quarter = a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     a.ProjectStart_ForCycleTime,
                                     RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                                     a.DataSourceType,
                                     DataDescription = a.DataDescription == "" ? "---" : a.DataDescription ?? "---",
                                     a.AwardedDate,
                                     a.ClosedDate,
                                     TaskStatus = a.TaskStatus == "" ? "---" : a.TaskStatus ?? "---",
                                     ActivityType = entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).ActivityType == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).ActivityType ?? "---",
                                     //GDS = entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).GDS == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).GDS ?? "---",
                                     entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).DTID,
                                     ComplexityScore = entity.DigitalTeams.FirstOrDefault(x => x.TaskRecordIdKey == a.Task__Task_Record_ID_Key).ComplexityScore,
                                     AccountOwner = a.AccountOwner == "" ? "---" : a.AccountOwner ?? "---",
                                     MilestoneType = a.MilestoneType == "" ? "---" : a.MilestoneType ?? "---",
                                     CycleTimeCategories = a.CycleTimeCategories == null || a.CycleTimeCategories == "" ? "---" : a.CycleTimeCategories,
                                     a.YearMonth,
                                     a.AccountCategory,
                                     a.SOWStatus,
                                     a.ImplementationReady,
                                     UpdateOn = abc.UpdateOn,
                                     OppVolume = a.OppTOtalVolume ?? 0,
                                     Service_configuration = a.Service_Configuration == "" || a.Service_Configuration == null ? "---" : a.Service_Configuration,
                                     Service_location = a.Service_Location == "" || a.Service_Location == null ? "---" : a.Service_Location,
                                     GDS = a.eSowGDS == "" || a.eSowGDS == null ? "---" : a.eSowGDS,
                                     LocalDigitalAdHocSupport = a.LocalDigitalAdHocSupport == "" || a.LocalDigitalAdHocSupport == null ? "---" : a.LocalDigitalAdHocSupport ?? "---",
                                     OBTAdoptionRate = a.OBTAdoptionRate == "" || a.OBTAdoptionRate == null ? "---" : a.OBTAdoptionRate,
                                     CycleTimeDelayCode = a.CycleTimeDelayCode == "" || a.CycleTimeDelayCode == null ? "---" : a.CycleTimeDelayCode ?? "---",
                                     EltClientDelayDescription = a.EltClientDelayDescription == "" || a.EltClientDelayDescription == null ? "---" : a.EltClientDelayDescription ?? "---",
                                     Cycletimetarget = a.CycleTimeCategories == "Existing Service Config Change (catch all including Spins/Mergers)" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).ExistingServiceConfigChange
                                                        : a.CycleTimeCategories == "New Global Including Upsell" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).NewGlobal
                                                        : a.CycleTimeCategories == "New Local Including Upsell" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).NewLocal
                                                        : a.CycleTimeCategories == "Existing Add/Change OBT" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).ExistingAddChange
                                                        : "0" ?? "0",
                                     Promoter = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID && x.NPSIndicator == "Promoter").Count(),
                                     Detractor = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID && x.NPSIndicator == "Detractor").Count(),
                                     Passive = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID && x.NPSIndicator == "Passive").Count(),
                                     NpsTotal = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID).Count(),
                                     APAC_DQS = a.APAC_DQS == "" ? "---" : a.APAC_DQS ?? "---",
                                     DQS_Import = a.DQS_Import == "" ? "---" : a.DQS_Import ?? "---",
                                     DQS_Support = a.DQS_Support == "" ? "---" : a.DQS_Support ?? "---",
                                     LATAM_DQS = a.LATAM_DQS == "" ? "---" : a.LATAM_DQS ?? "---",
                                     NORAM_DQS = a.NORAM_DQS == "" ? "---" : a.NORAM_DQS ?? "---",
                                     DQS_Operations = a.DQS_Operations == "" ? "---" : a.DQS_Operations ?? "---",
                                 }).ToList();
                var Datalist2 = (from a in entity.CLRDatas
                                 where a.ProjectStatus == "P-Pipeline" || a.ProjectStatus == "HP-High Potential" || a.ProjectStatus == "EP-Early Potential"
                                 where a.RevenueID != 400000000000000
                                 where a.RevenueID < 600000000000000
                                 //where a.Status == "Active"
                                 //where a.GoLiveYear != "2020"
                                 //where a.GoLiveYear != "2021"
                                 where CLR_GoliveYear.Any(val => a.GoLiveYear.Equals(val))
                                 where CLR_RecordStatus.Any(val => a.Status.Equals(val))
                                 //join c in entity.DigitalTeams on a.RevenueID equals c.RevenueID
                                 join b in entity.ManualDatas on a.RevenueID equals b.Revenue_ID into ab
                                 from abc in ab.DefaultIfEmpty()
                                 select new
                                 {
                                     a.CLRID,
                                     ManualID = abc.ManualID,
                                     Revenue_ID = abc.Revenue_ID ?? 0,
                                     Client = a.Client == "" ? "---" : a.Client ?? "---",
                                     a.CreatedDate,
                                     Date_added_to_the_CLR = a.LastUpdateDate,
                                     iMeet_Workspace_Title = "---",
                                     Implementation_Type = a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                     Pipeline_status = abc.Pipeline_status == "" ? "---" : abc.Pipeline_status ?? "---",
                                     Pipeline_comments = abc.Pipeline_comments == "" ? "---" : abc.Pipeline_comments ?? "---",
                                     //Service_configuration = abc.Service_configuration == "" ? "---" :abc.Service_configuration ?? "---",
                                     //OBT_Reseller___Direct = abc.OBT_Reseller___Direct == "" ? "---" :abc.OBT_Reseller___Direct ?? "---",
                                     OBT_Reseller___Direct = a.OBTReseller == "" || a.OBTReseller == null ? "---" : a.OBTReseller ?? "---",
                                     //abc.Servicing_location,
                                     abc.Assignment_date,
                                     abc.ResourseRequestedDate,
                                     //abc.New_Business_volume__US__,
                                     RevenueVolumeUSD = a.RevenueVolumeUSD ?? 0,
                                     Region = a.Region == "" ? "---" : a.Region ?? "---",
                                     Country = a.Country == "" ? "---" : a.Country ?? "---",
                                     a.RevenueID,
                                     OwnerShip = a.OwnerShip == "" ? "---" : a.OwnerShip ?? "---",
                                     GoLiveDate = abc.GoLiveDate == null ? a.GoLiveDate : abc.GoLiveDate,
                                     ProjectStatus = a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                     Milestone__Reason_Code = a.Milestone__Reason_Code == "" ? "---" : a.Milestone__Reason_Code ?? "---",
                                     PerCompleted = a.PerCompleted ?? 0,
                                     CountryStatus = a.CountryStatus == "" ? "---" : a.CountryStatus ?? "---",
                                     ProjectLevel = abc.ProjectLevel == "" ? "---" : abc.ProjectLevel ?? "---",
                                     a.CompletedDate,
                                     a.MilestoneDueDate,
                                     CountryCode = a.CountryCode == "" ? "---" : a.CountryCode ?? "---",
                                     GlobalProjectManager = abc.GlobalProjectManager == "" ? "---" : abc.GlobalProjectManager ?? "---",
                                     RegionalProjectManager = abc.RegionalProjectManager == "" ? "---" : abc.RegionalProjectManager ?? "---",
                                     GlobalCISDQSLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISDQSLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISDQSLead ?? "---",
                                     AssigneeFullName = abc.AssigneeFullName == "" ? "---" : abc.AssigneeFullName ?? "---",
                                     GlobalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISOBTLead ?? "---",
                                     RegionalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISOBTLead ?? "---",
                                     LocalDigitalOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).LocalDigitalOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).LocalDigitalOBTLead ?? "---",
                                     GlobalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISPortraitLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISPortraitLead ?? "---",
                                     RegionalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISPortraitLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).RegionalCISPortraitLead ?? "---",
                                     GlobalCISHRFeedSpecialist = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISHRFeedSpecialist == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GlobalCISHRFeedSpecialist ?? "---",
                                     MilestoneTitle = a.MilestoneTitle == "" ? "---" : a.MilestoneTitle ?? "---",
                                     Group_Name = a.Group_Name == "" ? "---" : a.Group_Name ?? "---",
                                     Milestone__Project_Notes = a.Milestone__Project_Notes == "" ? "---" : a.Milestone__Project_Notes ?? "---",
                                     Milestone__Closed_Loop_Owner = a.Milestone__Closed_Loop_Owner == "" ? "---" : a.Milestone__Closed_Loop_Owner ?? "---",
                                     Workspace__ELT_Overall_Status = a.Workspace__ELT_Overall_Status == "" ? "---" : a.Workspace__ELT_Overall_Status ?? "---",
                                     Workspace__ELT_Overall_Comments = a.Workspace__ELT_Overall_Comments == "" ? "---" : a.Workspace__ELT_Overall_Comments ?? "---",
                                     Customer_Row_ID = a.Customer_Row_ID ?? 0,
                                     Opportunity_ID = a.Opportunity_ID ?? 0,
                                     Account_Name = a.Account_Name == "" ? "---" : a.Account_Name ?? "---",
                                     Sales_Stage_Name = a.Sales_Stage_Name == "" ? "---" : a.Sales_Stage_Name ?? "---",
                                     Opportunity_Type = a.Opportunity_Type == "" ? "---" : a.Opportunity_Type ?? "---",
                                     Revenue_Status = a.Revenue_Status == "" ? "---" : a.Revenue_Status ?? "---",
                                     Revenue_Opportunity_Type = a.Revenue_Opportunity_Type == "" ? "---" : a.Revenue_Opportunity_Type ?? "---",
                                     Opportunity_Owner = a.Opportunity_Owner == "" ? "---" : a.Opportunity_Owner ?? "---",
                                     Opportunity_Category = a.Opportunity_Category == "" ? "---" : a.Opportunity_Category ?? "---",
                                     Revenue_Total_Transactions = a.Revenue_Total_Transactions ?? 0,
                                     Line_Win_Probability = a.Line_Win_Probability ?? 0,
                                     Next_Step = a.Next_Step == "" ? "---" : a.Next_Step ?? "---",
                                     Implementation_Fee__PSD_ = a.ImplementationFeePsd ?? 0,
                                     Project_Effort = abc.Project_Effort ?? 0,
                                     CycleTime = a.CycleTime ?? 0,
                                     a.ExternalKickoffDuedate,
                                     GoLiveYear = a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                     GoLiveMonth = a.GoLiveMonth == "" ? "---" : a.GoLiveMonth ?? "---",
                                     Quarter = a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     a.ProjectStart_ForCycleTime,
                                     RecordStatus = a.Status == "" ? "---" : a.Status ?? "---",
                                     a.DataSourceType,
                                     DataDescription = a.DataDescription == "" ? "---" : a.DataDescription ?? "---",
                                     a.AwardedDate,
                                     a.ClosedDate,
                                     TaskStatus = a.TaskStatus == "" ? "---" : a.TaskStatus ?? "---",
                                     ActivityType = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).ActivityType == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).ActivityType ?? "---",
                                     //GDS = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GDS == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).GDS ?? "---",
                                     entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).DTID,
                                     ComplexityScore = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.RevenueID).ComplexityScore,
                                     AccountOwner = a.AccountOwner == "" ? "---" : a.AccountOwner ?? "---",
                                     MilestoneType = a.MilestoneType == "" ? "---" : a.MilestoneType ?? "---",
                                     CycleTimeCategories = a.CycleTimeCategories == null || a.CycleTimeCategories == "" ? "---" : a.CycleTimeCategories,
                                     a.YearMonth,
                                     a.AccountCategory,
                                     a.SOWStatus,
                                     a.ImplementationReady,
                                     UpdateOn = abc.UpdateOn,
                                     OppVolume = a.OppTOtalVolume ?? 0,
                                     Service_configuration = a.Service_Configuration == "" || a.Service_Configuration == null ? "---" : a.Service_Configuration,
                                     Service_location = a.Service_Location == "" || a.Service_Location == null ? "---" : a.Service_Location,
                                     GDS = a.eSowGDS == "" || a.eSowGDS == null ? "---" : a.eSowGDS,
                                     LocalDigitalAdHocSupport = a.LocalDigitalAdHocSupport == "" || a.LocalDigitalAdHocSupport == null ? "---" : a.LocalDigitalAdHocSupport ?? "---",
                                     OBTAdoptionRate = a.OBTAdoptionRate == "" || a.OBTAdoptionRate == null ? "---" : a.OBTAdoptionRate,
                                     CycleTimeDelayCode = a.CycleTimeDelayCode == "" || a.CycleTimeDelayCode == null ? "---" : a.CycleTimeDelayCode ?? "---",
                                     EltClientDelayDescription = a.EltClientDelayDescription == "" || a.EltClientDelayDescription == null ? "---" : a.EltClientDelayDescription ?? "---",
                                     Cycletimetarget = a.CycleTimeCategories == "Existing Service Config Change (catch all including Spins/Mergers)" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).ExistingServiceConfigChange
                                                        : a.CycleTimeCategories == "New Global Including Upsell" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).NewGlobal
                                                        : a.CycleTimeCategories == "New Local Including Upsell" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).NewLocal
                                                        : a.CycleTimeCategories == "Existing Add/Change OBT" ? entity.TargetCycleTimes.FirstOrDefault(x => x.Year == CurrentYear).ExistingAddChange
                                                        : "0" ?? "0",
                                     Promoter = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID && x.NPSIndicator == "Promoter").Count(),
                                     Detractor = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID && x.NPSIndicator == "Detractor").Count(),
                                     Passive = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID && x.NPSIndicator == "Passive").Count(),
                                     NpsTotal = entity.NpsImps.Where(x => x.OpprtunityId == a.Opportunity_ID).Count(),
                                     APAC_DQS = a.APAC_DQS == "" ? "---" : a.APAC_DQS ?? "---",
                                     DQS_Import = a.DQS_Import == "" ? "---" : a.DQS_Import ?? "---",
                                     DQS_Support = a.DQS_Support == "" ? "---" : a.DQS_Support ?? "---",
                                     LATAM_DQS = a.LATAM_DQS == "" ? "---" : a.LATAM_DQS ?? "---",
                                     NORAM_DQS = a.NORAM_DQS == "" ? "---" : a.NORAM_DQS ?? "---",
                                     DQS_Operations = a.DQS_Operations == "" ? "---" : a.DQS_Operations ?? "---",
                                 }).ToList();
                var Datalist = Datalist1.Concat(Datalist2).Concat(Datalist3).Concat(Datalist4);
                CLRDataCount = Datalist.AsQueryable().Count();
                if (CLRDataCount.ToString() == "" || CLRDataCount.ToString() == null || CLRDataCount == 0)
                {
                    re.Data = Datalist;
                    re.code = 100;
                    re.message = "No Data found";
                }
                else
                {
                    re.Data = Datalist.OrderBy(x => x.GoLiveDate);
                    re.code = 200;
                    re.message = "Data Successfull - ";
                }
            }
            return re;
        }

        [HttpPost]
        [Route("AutomatedCLRFilters")]
        public AutomatedCLRFilters AutomatedCLRFilters(CLRData cLRData)
        {
            var FilterImplementationType = (from a in entity.CLRDatas
                                            select new
                                            {
                                                ImplementationType = a.ImplementationType == null || a.ImplementationType == "" ? "---" : a.ImplementationType ?? "---",
                                                isSelected = true,
                                            }).Distinct().OrderBy(x => x.ImplementationType);
            var FilterRegion = (from a in entity.CLRDatas
                                select new
                                {
                                    Region = a.Region == null || a.Region == "" ? "---" : a.Region ?? "---",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Region);
            var FilterQuarter = (from a in entity.CLRDatas
                                 select new
                                 {
                                     Quarter = a.Quarter == null || a.Quarter == "" ? "---" : a.Quarter ?? "---",
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.Quarter);
            var FilterYears = (from a in entity.CLRDatas
                                 select new
                                 {
                                     Year = a.GoLiveYear == null || a.GoLiveYear == "" ? "---" : a.GoLiveYear ?? "---",
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.Year);
            var FilterOwnerShip = (from a in entity.CLRDatas
                                   select new
                                   {
                                       OwnerShip = a.OwnerShip == null || a.OwnerShip == "" ? "---" : a.OwnerShip ?? "---",
                                       isSelected = true,
                                   }).Distinct().OrderBy(x => x.OwnerShip);
            var FilterProjectStatus = (from a in entity.CLRDatas
                                       select new
                                       {
                                           ProjectStatus = a.ProjectStatus == null || a.ProjectStatus == "" ? "---" : a.ProjectStatus ?? "---",
                                           isSelected = true,
                                       }).Distinct().OrderBy(x => x.ProjectStatus);
            var FilterCountryStatus = (from a in entity.CLRDatas
                                       select new
                                       {
                                           CountryStatus = a.CountryStatus == null || a.CountryStatus == "" ? "---" : a.CountryStatus ?? "---",
                                           isSelected = true,
                                       }).Distinct().OrderBy(x => x.CountryStatus);
            var FilterProjectLevel = (from a in entity.CLRDatas
                                      select new
                                      {
                                          ProjectLevel = a.ProjectLevel == null || a.ProjectLevel == "" ? "---" : a.ProjectLevel ?? "---",
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.ProjectLevel);
            var FilterGroup_Name = (from a in entity.CLRDatas
                                    select new
                                    {
                                        Group_Name = a.Group_Name == null || a.Group_Name == "" ? "---" : a.Group_Name ?? "---",
                                        isSelected = true,
                                    }).Distinct().OrderBy(x => x.Group_Name);
            var FilterWorkspace__ELT_Overall_Status = (from a in entity.CLRDatas
                                                       select new
                                                       {
                                                           Workspace__ELT_Overall_Status = a.Workspace__ELT_Overall_Status == null || a.Workspace__ELT_Overall_Status == "" ? "---" : a.Workspace__ELT_Overall_Status ?? "---",
                                                           isSelected = true,
                                                       }).Distinct().OrderBy(x => x.Workspace__ELT_Overall_Status);
            var FilterSales_Stage_Name = (from a in entity.CLRDatas
                                          select new
                                          {
                                              Sales_Stage_Name = a.Sales_Stage_Name == null || a.Sales_Stage_Name == "" ? "---" : a.Sales_Stage_Name ?? "---",
                                              isSelected = true,
                                          }).Distinct().OrderBy(x => x.Sales_Stage_Name);
            var FilterOpportunity_Type = (from a in entity.CLRDatas
                                          select new
                                          {
                                              Opportunity_Type = a.Opportunity_Type == null || a.Opportunity_Type == "" ? "---" : a.Opportunity_Type ?? "---",
                                              isSelected = true,
                                          }).Distinct().OrderBy(x => x.Opportunity_Type);
            var FilterRevenue_Status = (from a in entity.CLRDatas
                                        where a.Status == "Active"
                                        select new
                                        {
                                            Revenue_Status = a.Revenue_Status == null || a.Revenue_Status == "" ? "---" : a.Revenue_Status ?? "---",
                                            isSelected = true,
                                        }).Distinct().OrderBy(x => x.Revenue_Status);
            var FilterRevenue_Opportunity_Type = (from a in entity.CLRDatas
                                                  select new
                                                  {
                                                      Revenue_Opportunity_Type = a.Revenue_Opportunity_Type == null || a.Revenue_Opportunity_Type == "" ? "---" : a.Revenue_Opportunity_Type ?? "---",
                                                      isSelected = true,
                                                  }).Distinct().OrderBy(x => x.Revenue_Opportunity_Type);
            var FilterOpportunity_Category = (from a in entity.CLRDatas
                                              select new
                                              {
                                                  Opportunity_Category = a.Opportunity_Category == null || a.Opportunity_Category == "" ? "---" : a.Opportunity_Category ?? "---",
                                                  isSelected = true,
                                              }).Distinct().OrderBy(x => x.Opportunity_Category);
            var FilterLine_Win_Probability = (from a in entity.CLRDatas
                                              where a.Status == "Active"
                                              select new
                                              {
                                                  Line_Win_Probability = a.Line_Win_Probability == null ? 0 : a.Line_Win_Probability ?? 0,
                                                  isSelected = true,
                                              }).Distinct().OrderBy(x => x.Line_Win_Probability);
            var FilterStatus = (from a in entity.CLRDatas
                                select new
                                {
                                    Status = a.Status == null || a.Status == "" ? "---" : a.Status ?? "---",
                                    isSelected = true,
                                }).Distinct().OrderBy(x => x.Status);
            var FilterDataSourceType = (from a in entity.CLRDatas
                                        select new
                                        {
                                            DataSourceType = a.DataSourceType == null || a.DataSourceType == "" ? "---" : a.DataSourceType ?? "---",
                                            isSelected = true,
                                        }).Distinct().OrderBy(x => x.DataSourceType);
            var FilterPipeline_status = (from a in entity.ManualDatas
                                         select new
                                         {
                                             Pipeline_status = a.Pipeline_status == null || a.Pipeline_status == "" ? "---" : a.Pipeline_status ?? "---",
                                             isSelected = true,
                                         }).Distinct().OrderBy(x => x.Pipeline_status);
            var FilterOBTReseller = (from a in entity.CLRDatas
                                         select new
                                         {
                                             OBTReseller = a.OBTReseller == null || a.OBTReseller == "" ? "---" : a.OBTReseller ?? "---",
                                             isSelected = true,
                                         }).Distinct().OrderBy(x => x.OBTReseller);
            var FilterCountry = (from a in entity.CLRDatas
                                 select new
                                 {
                                     Country = a.Country == null || a.Country == "" ? "---" : a.Country ?? "---",
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.Country);
            var FilterGlobalProjectManager = (from a in entity.CLRDatas
                                              select new
                                              {
                                                  GlobalProjectManager = a.GlobalProjectManager == null || a.GlobalProjectManager == "" ? "---" : a.GlobalProjectManager ?? "---",
                                                  isSelected = true,
                                              }).Distinct().OrderBy(x => x.GlobalProjectManager);
            var FilterRegionalProjectManager = (from a in entity.CLRDatas
                                                select new
                                                {
                                                    RegionalProjectManager = a.RegionalProjectManager == null || a.RegionalProjectManager == "" ? "---" : a.RegionalProjectManager ?? "---",
                                                    isSelected = true,
                                                }).Distinct().OrderBy(x => x.RegionalProjectManager);
            var FilterGlobalDigitalOBTLead = (from a in entity.CapacityHierarchies
                                              where a.Level == "Digital"
                                              where a.ManagerStatus == "Active"
                                              where a.Leader != null
                                              where a.Leader != ""
                                              group a by a.Leader into g
                                              select new
                                              {
                                                  GlobalCISOBTLead = g.Key,
                                                  DigitalOBTManager = (from b in entity.CapacityHierarchies
                                                                       where b.Leader == g.Key
                                                                       where b.Level == "Digital"
                                                                       where b.ManagerStatus == "Active"
                                                                       select new {
                                                                           Manager = b.Manager,
                                                                           isSelected = true,
                                                                       }).OrderBy(x => x.Manager),
                                                  isSelected = true,
                                              }).OrderBy(x => x.GlobalCISOBTLead);
            //var FilterGlobalDigitalOBTLead = (from a in entity.CLRDatas
            //                                  select new
            //                                  {
            //                                      GlobalCISOBTLead = a.GlobalCISOBTLead == null || a.GlobalCISOBTLead == "" ? "---" : a.GlobalCISOBTLead ?? "---",
            //                                      isSelected = true,
            //                                  }).Distinct().OrderBy(x => x.GlobalCISOBTLead);
            var FilterGDS = (from a in entity.CLRDatas
                                                select new
                                                {
                                                    GDS = a.eSowGDS == null || a.eSowGDS == "" ? "---" : a.eSowGDS ?? "---",
                                                    isSelected = true,
                                                }).Distinct().OrderBy(x => x.GDS);
            var FilterActivityType = (from a in entity.DigitalTeams
                             select new
                             {
                                 ActivityType = a.ActivityType == null || a.ActivityType == "" ? "---" : a.ActivityType ?? "---",
                                 isSelected = true,
                             }).Distinct().OrderBy(x => x.ActivityType);
            var FilterDigitalTeam = (from a in entity.CapacityHierarchies
                                     where a.Level == "Digital"
                                     select new
                                      {
                                          Manager = a.Manager == null || a.Manager == "" ? "---" : a.Manager ?? "---",
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.Manager);
            var FilterAccountCategory = (from a in entity.CLRDatas
                                     select new
                                     {
                                         AccountCategory = a.AccountCategory == null || a.AccountCategory == "" ? "---" : a.AccountCategory ?? "---",
                                         isSelected = true,
                                     }).Distinct().OrderBy(x => x.AccountCategory);
            var FiltereSowStatus = (from a in entity.CLRDatas
                                     select new
                                     {
                                         SOWStatus = a.SOWStatus == null || a.SOWStatus == "" ? "---" : a.SOWStatus ?? "---",
                                         isSelected = true,
                                     }).Distinct().OrderBy(x => x.SOWStatus);
            var FilterServiceConfiguration = (from a in entity.CLRDatas
                                     select new
                                     {
                                         Service_Configuration = a.Service_Configuration == null || a.Service_Configuration == "" ? "---" : a.Service_Configuration ?? "---",
                                         isSelected = true,
                                     }).Distinct().OrderBy(x => x.Service_Configuration);
            var FilterCycleTimeCategory = (from a in entity.CLRDatas
                                              select new
                                              {
                                                  CycleTimeCategories = a.CycleTimeCategories == null || a.CycleTimeCategories == "" ? "---" : a.CycleTimeCategories ?? "---",
                                                  isSelected = true,
                                              }).Distinct().OrderBy(x => x.CycleTimeCategories);
            var FilterRegionWiseCountries = (from a in entity.CountryIsoCodes
                                             group a by a.Region into g
                                             select new
                                             {
                                                 Region = g.Key,
                                                 Countries = (from b in entity.CountryIsoCodes
                                                              where b.Region == g.Key
                                                              select new
                                                              {
                                                                  Country = b.CountryName,
                                                                  isSelected = true,
                                                              }).Distinct().OrderBy(x => x.Country).ToList(),
                                                 isSelected = true,
                                             }).OrderBy(x => x.Region).ToList();
            //var FilterRegionWiseCountries = entity.CLRDatas.SelectMany(X => X.Region.Select(y => new { Country = y.Country , Region = X}))
            CLR_F.FilterImplementationType = FilterImplementationType;
            CLR_F.FilterRegion = FilterRegion;
            CLR_F.FilterOwnerShip = FilterOwnerShip;
            CLR_F.FilterProjectStatus = FilterProjectStatus;
            CLR_F.FilterOBTReseller = FilterOBTReseller;
            CLR_F.FilterCountryStatus = FilterCountryStatus;
            CLR_F.FilterProjectLevel = FilterProjectLevel;
            CLR_F.FilterGroup_Name = FilterGroup_Name;
            CLR_F.FilterWorkspace__ELT_Overall_Status = FilterWorkspace__ELT_Overall_Status;
            CLR_F.FilterSales_Stage_Name = FilterSales_Stage_Name;
            CLR_F.FilterOpportunity_Type = FilterOpportunity_Type;
            CLR_F.FilterRevenue_Status = FilterRevenue_Status;
            CLR_F.FilterRevenue_Opportunity_Type = FilterRevenue_Opportunity_Type;
            CLR_F.FilterOpportunity_Category = FilterOpportunity_Category;
            CLR_F.FilterLine_Win_Probability = FilterLine_Win_Probability;
            CLR_F.FilterStatus = FilterStatus;
            CLR_F.FilterDataSourceType = FilterDataSourceType;
            CLR_F.FilterPipeline_status = FilterPipeline_status;
            CLR_F.FilterCountry = FilterCountry;
            CLR_F.FilterGlobalProjectManager = FilterGlobalProjectManager;
            CLR_F.FilterRegionalProjectManager = FilterRegionalProjectManager;
            CLR_F.FilterGDS = FilterGDS;
            CLR_F.FilterActivityType = FilterActivityType;
            CLR_F.FilterDigitalTeam = FilterDigitalTeam;
            CLR_F.FilterRegionWiseCountries = FilterRegionWiseCountries;
            CLR_F.FilterGlobalDigitalOBTLead = FilterGlobalDigitalOBTLead;
            CLR_F.FilterYears = FilterYears;
            CLR_F.FilterQuarter = FilterQuarter;
            CLR_F.FilterAccountCategory = FilterAccountCategory;
            CLR_F.FiltereSowStatus = FiltereSowStatus;
            CLR_F.FilterServiceConfiguration = FilterServiceConfiguration;
            CLR_F.CycleTimeCategories = FilterCycleTimeCategory;
            CLR_F.code = 200;
            CLR_F.message = "Success";
            return CLR_F;
        }
        [HttpPost]
        [Route("CRMExport")]
        public Response CRMEXPORT(CRMData crmdata)
        {
            var CRMData = (from a in entity.CRMDatas
                           select a);
            var DataCount = CRMData.AsQueryable().Count();
            if (DataCount.ToString() == "---" || DataCount.ToString() == null || DataCount == 0)
            {
                re.code = 100;
                re.message = "No Data Found";
                re.Data =  null;
            }
            else
            {
                re.code = 200;
                re.message = "Data Success";
                re.Data = CRMData;
            }
            return re;
        }
        [HttpPost]
        [Route("PSDExport")]
        public Response PSDExport(PSD psd)
        {
            var PSDData = (from a in entity.PSDs
                           select a);
            var DataCount = PSDData.AsQueryable().Count();
            if (DataCount.ToString() == "---" || DataCount.ToString() == null || DataCount == 0)
            {
                re.code = 200;
                re.message = "Data Success";
                re.Data = null;
            }
            else
            {
                re.code = 200;
                re.message = "Data Success";
                re.Data = PSDData;
            }
            return re;
        }
        [HttpPost]
        [Route("InsertManualColumns")]
        public IHttpActionResult InsertManualColumns(ManualData manualData)
        {
            Boolean b = new ManualDataClass().AddingManualdata(manualData);
            if (b)
            {
                entity.ManualDatas.Add(manualData);
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
        [Route("UpdateManualColumns")]
        public IHttpActionResult UpdateManualColumns(ManualData manualData)
        {
            Boolean b = new ManualDataClass().UpdateManualdata(manualData);
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
        [Route("GenerateTracker")]
        public IHttpActionResult GenerateTracker()
        {
            Boolean b = new ManualDataClass().GenerateTracker();
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
        [Route("UpdateManualPipelineColumns")]
        public IHttpActionResult UpdateManualPipelineColumns(ManualData manualData)
        {
            Boolean b = new ManualDataClass().UpdateManualPipelinedata(manualData);
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
        [Route("UpdateDigitalColumns")]
        public IHttpActionResult UpdateDigitalColumns(DigitalTeamModel digitalTeam)
        {
            Boolean b = new ManualDataClass().UpdateManualDigitaldata(digitalTeam);
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
        int RevIDCount,ManualDataCount;
        [HttpPost]
        [Route("GetRevenueId")]
        public H_Filters GetRevenueId(ManualData manualData)
        {
            var RevID = (from a in entity.CLRDatas
                         where a.Status == "Active"
                         where a.RevenueID != 400000000000000
                         select new
                         {
                             a.RevenueID,
                             a.Opportunity_ID,
                             Workspace_Title = a.Client,
                             a.Region,
                             a.CountryCode,
                             isSelected = true
                         }).Distinct().OrderBy(x => x.RevenueID);
            RevIDCount = RevID.AsQueryable().Count();
            if (RevIDCount.ToString() == "---" || RevIDCount.ToString() == null || RevIDCount == 0)
            {
                fi.RevenueId = RevID;
                fi.code = 100;
                fi.message = "No Data found";
            }
            else
            {
                fi.RevenueId = RevID;
                fi.code = 200;
                fi.message = "Data Successfull";
            }
            return fi;
        }
        [HttpPost]
        [Route("GetManualdataUsingRevID")]
        public Response GetManualdataUsingRevID(ManualData manualData)
        {
            var ManualData = (from a in entity.ManualDatas
                              where a.Revenue_ID == manualData.Revenue_ID
                              select new
                              {
                                  a.ManualID,
                                  a.Revenue_ID,
                                  a.Implementation_Type,
                                  a.Pipeline_status,
                                  a.Pipeline_comments,
                                  a.Assignment_date,
                                  a.Project_Effort,
                                  a.GoLiveDate,
                                  RecordStatus = a.Status,
                                  ProjectLevel = a.ProjectLevel,
                                  a.GlobalProjectManager,
                                  a.RegionalProjectManager,
                                  a.AssigneeFullName,
                                  GlobalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).GlobalCISOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).GlobalCISOBTLead ?? "---",
                                  RegionalCISOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).RegionalCISOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).RegionalCISOBTLead ?? "---",
                                  LocalDigitalOBTLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).LocalDigitalOBTLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).LocalDigitalOBTLead ?? "---",
                                  GlobalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).GlobalCISPortraitLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).GlobalCISPortraitLead ?? "---",
                                  RegionalCISPortraitLead = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).RegionalCISPortraitLead == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).RegionalCISPortraitLead ?? "---",
                                  GlobalCISHRFeedSpecialist = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).GlobalCISHRFeedSpecialist == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).GlobalCISHRFeedSpecialist ?? "---",
                                  ActivityType = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).ActivityType == "" ? "---" : entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).ActivityType ?? "---",
                                  ComplexityScore = entity.DigitalTeams.FirstOrDefault(x => x.RevenueID == a.Revenue_ID).ComplexityScore,

                                  //a.CLR_Country,
                                  //a.Service_configuration,
                                  //a.OBT_Reseller___Direct,
                                  //a.Servicing_location,
                              }).ToList();
            ManualDataCount = ManualData.AsQueryable().Count();
            if (ManualDataCount.ToString() == "---" || ManualDataCount.ToString() == null || ManualDataCount == 0)
            {
                re.Data = ManualData;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                re.Data = ManualData;
                re.code = 200;
                re.message = "Data Successfull";
            }
            return re;
        }
        [HttpPost]
        [Route("GetAuditLog")]
        public Response GetAuditLog(AuditLog auditlog)
        {
            var AuditLog = (from a in entity.AuditLogs
                            where a.Status == "Active"
                            where a.RevenueID > 5000000
                            select new
                            {
                                a.RevenueID,
                                a.TaskRecordIDKey,
                                //RevenueID = a.RevenueID == 400000000000000 ? a.RevenueID + " - " + a.TaskRecordIDKey : a.RevenueID + "",
                                OldValue = a.OldValue ?? "---",
                                Username = entity.Users.FirstOrDefault(x => x.UID == a.UpdatedBy).FirstName + " " + entity.Users.FirstOrDefault(x => x.UID == a.UpdatedBy).LastName ?? "---",
                                NewValue = a.NewValue ?? "---",
                                Field = a.Field ?? "---",
                                UsedPlatForm = a.UsedPlatForm ?? "---",
                                UpdatedBy = a.UpdatedBy ?? "---",
                                a.UpdatedOn
                            }).OrderBy(x=>x.UpdatedOn).ToList();
            re.code = 200;
            re.message = "Data Success";
            re.Data = AuditLog;
            return re;
        }
        string[] RevIDs;
        [HttpPost]
        [Route("ReplicatingManualData")]
        public IHttpActionResult ReplicatingManualData(ReplicateManualData manualdata)
        {
            RevIDs = manualdata.Revenue_IDs.Split(',');
            try
            {
                for (int i = 0; i < RevIDs.Length; i++)
                {
                    manualdata.Revenue_ID = double.Parse(RevIDs[i]);
                    Boolean b = new ManualDataClass().UpdateManualReplicatingData(manualdata);
                    if (b)
                    {
                        re.code = 200;
                        re.message = "Updated Succesfully";
                    }
                    else
                    {
                        re.code = 100;
                        re.message = "Failed to Update data";
                    }
                }
                return Content(HttpStatusCode.OK, re);
            }
            catch (Exception e)
            {
                re.code = 100;
                re.message = e.ToString();
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("DupRevIDTesting")]
        public Response DupRevIDTesting(CLRData CLRData)
        {
            var CreationDate = "01-01-2020";
            DateTime ConvertedDate = Convert.ToDateTime(CreationDate);
            var RevCheck = (from a in entity.CLRDatas
                            where a.GoLiveDate >= ConvertedDate
                            where a.RevenueID != 400000000000000
                            group a by a.RevenueID into g
                            where g.Count() > 1
                            select g.Key).ToList();
            for (int i = 1; i <= RevCheck.Count; i++)
            {
                var revid = RevCheck[i-1];
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
                    for (int j = 1; j <= CLRIDData.Count(); j++)
                    {
                        var CLRID = CLRIDData[j-1];
                        CLRData Vp = (from s in entity.CLRDatas
                                        where s.CLRID == CLRID
                                        select s).FirstOrDefault();
                        if(j == 1)
                        {
                            Vp.RevenueVolumeUSD = RevVolume;
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
            re.Data = RevCheck;
            re.message = "Data Success";
            re.code = RevCheck.Count;
            return re;
        }

        [HttpPost]
        [Route("UpdatingProjectLevel")]
        public Response UpdatingProjectLevel(CLRData cLRData)
        {
            var status_pipeline = "P-Pipeline,EP-Early Potential,HP-High Potential".Split(',');
            var Data = (from a in entity.CLRDatas
                        where status_pipeline.Any(val => a.ProjectStatus.Equals(val))
                        select a).ToList();
            for (int i = 0; i < Data.Count(); i++)
            {
                var rev_ID = (double)Data[i].RevenueID;
                ManualData md = (from a in entity.ManualDatas
                                    where a.Revenue_ID == rev_ID
                                    select a)?.FirstOrDefault();
                var client = Data[i].Client;
                if (md != null)
                {
                    md.ProjectLevel = Data.Where(x => x.Client == client).Select(x => x.Country).Distinct().Count() <= 1 ? "Local"
                                    : Data.Where(x => x.Client == client).Select(x => x.Region).Distinct().Count() > 1 ? "Global"
                                    : "Regional" ?? "Local";
                    entity.SaveChanges();
                }
            }
            re.message = "Success";
            re.Data = Data;
            return re;
        }
    }
}