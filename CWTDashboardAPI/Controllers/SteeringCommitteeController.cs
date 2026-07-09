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
    public class SteeringCommitteeController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        SteeringCommitteeData sc_r = new SteeringCommitteeData();
        SteeringCommitteeFilters sc_f = new SteeringCommitteeFilters();
        H_Filters fi = new H_Filters();
        IMRCResponse imrs = new IMRCResponse();
        AutomatedCLRFilters CLR_F = new AutomatedCLRFilters();

        [HttpPost]
        [Route("SteeringCommitteeFilters")]
        public SteeringCommitteeFilters SteeringCommitteeFilters(SteeringCommittee clr)
        {
            var RegionCountry = (from a in entity.CountryISORegionCodes
                                 where a.CountryName != null
                                 where a.CountryName != ""
                                 select new
                                 {
                                     Region = a.Region,
                                     Country = a.CountryName,
                                 }).Distinct().OrderBy(x => x.Country);
            var Manager = (from a in entity.CapacityHierarchies
                           where a.ManagerStatus == "Active"
                           select new
                           {
                               Owner = a.Manager,
                               isSelected = true,
                           }).Distinct().OrderBy(x => x.Owner);
            var Leader = (from a in entity.CapacityHierarchies
                          where a.ManagerStatus == "Active"
                          select new
                          {
                              Owner = a.Leader,
                              isSelected = true
                          }).Distinct().OrderBy(x => x.Owner);
            var Owner = Manager.Concat(Leader).Distinct();
            sc_f.code = 200;
            sc_f.message = "Data Successfull";
            sc_f.RegionCountry = RegionCountry;
            sc_f.Owner = Owner;
            return sc_f;
        }

        [HttpPost]
        [Route("SteeringCommitteeInsert")]
        public IHttpActionResult SteeringCommitteeInsert(SteeringCommittee steeringCommittee)
        {
            DateTime TodayDate = DateTime.Now;
            steeringCommittee.InsertedBy = steeringCommittee.InsertedBy;
            steeringCommittee.LastUpdatedBy = steeringCommittee.InsertedBy;
            steeringCommittee.LastUpdatedDate = TodayDate;
            steeringCommittee.InsertedDate = TodayDate;
            steeringCommittee.Record_Status = "Active";
            entity.SteeringCommittees.Add(steeringCommittee);
            entity.SaveChanges();
            var MaxSCID = entity.SteeringCommittees.OrderByDescending(x => x.SCID).FirstOrDefault();
            sc_r.code = 200;
            sc_r.message = "Inserted Succesfully";
            sc_r.SCId = MaxSCID.SCID;
            return Content(HttpStatusCode.OK, sc_r);
        }

        [HttpPost]
        [Route("SteeringCommitteeUpdate")]
        public IHttpActionResult SteeringCommitteeUpdate(SteeringCommittee steeringCommittee)
        {
            Boolean b = new SteeringCommitteeClass().UpdateSteeringCommittee(steeringCommittee);
            if (b)
            {
                sc_r.code = 200;
                sc_r.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, sc_r);
            }
            else
            {
                sc_r.code = 100;
                sc_r.message = "Failed to Update data";
                return Content(HttpStatusCode.OK, sc_r);
            }
        }

        [HttpPost]
        [Route("WavesInsert")]
        public IHttpActionResult WavesInsert(Wave wave)
        {
            DateTime TodayDate = DateTime.Now;
            wave.LastUpdatedDate = TodayDate;
            wave.InsertedDate = TodayDate;
            wave.InsertedBy = wave.InsertedBy;
            wave.LastUpdatedBy = wave.InsertedBy;
            wave.Record_Status = "Active";
            entity.Waves.Add(wave);
            entity.SaveChanges();
            sc_r.code = 200;
            sc_r.message = "Inserted Succesfully";
            return Content(HttpStatusCode.OK, sc_r);
        }

        [HttpPost]
        [Route("WavesUpdate")]
        public IHttpActionResult WavesUpdate(Wave wave)
        {
            Boolean b = new SteeringCommitteeClass().UpdateWaves(wave);
            if (b)
            {
                sc_r.code = 200;
                sc_r.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, sc_r);
            }
            else
            {
                sc_r.code = 100;
                sc_r.message = "Failed to Update data";
                return Content(HttpStatusCode.OK, sc_r);
            }
        }
        [HttpPost]
        [Route("RiskGapInsert")]
        public IHttpActionResult RiskGapInsert(RisksGap risksGap)
        {
            DateTime TodayDate = DateTime.Now;
            risksGap.InsertedBy = risksGap.InsertedBy;
            risksGap.LastUpdatedBy = risksGap.InsertedBy;
            risksGap.LastUpdatedDate = TodayDate;
            risksGap.InsertedDate = TodayDate;
            risksGap.Record_Status = "Active";
            entity.RisksGaps.Add(risksGap);
            entity.SaveChanges();
            sc_r.code = 200;
            sc_r.message = "Inserted Succesfully";
            return Content(HttpStatusCode.OK, sc_r);
        }

        [HttpPost]
        [Route("RiskGapUpdate")]
        public IHttpActionResult RiskGapUpdate(RisksGap risksGap)
        {
            Boolean b = new SteeringCommitteeClass().UpdateRiskGap(risksGap);
            if (b)
            {
                sc_r.code = 200;
                sc_r.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, sc_r);
            }
            else
            {
                sc_r.code = 100;
                sc_r.message = "Failed to Update data";
                return Content(HttpStatusCode.OK, sc_r);
            }
        }
        [HttpPost]
        [Route("DeleteSCData")]
        public IHttpActionResult DeleteSCData(SteeringCommittee steeringCommittee)
        {
            if(steeringCommittee.SCID == null || steeringCommittee.LastUpdatedBy == null || steeringCommittee.LastUpdatedBy == "")
            {
                sc_r.code = 100;
                sc_r.message = "Something went wrong please try again after sometime";
                return Content(HttpStatusCode.OK, sc_r);
            }
            else
            {
                var checkingSC_id = entity.SteeringCommittees.Where(x => x.SCID == steeringCommittee.SCID).Count();
                if(checkingSC_id > 0)
                {
                    DateTime TodayDate = DateTime.Now;
                    SteeringCommittee sc_d = (from a in entity.SteeringCommittees
                                              where a.SCID == steeringCommittee.SCID
                                              select a).FirstOrDefault();
                    sc_d.Record_Status = "Deleted";
                    sc_d.LastUpdatedBy = steeringCommittee.LastUpdatedBy;
                    sc_d.LastUpdatedDate = TodayDate;
                    var wave_data = entity.Waves.Where(x => x.SCID == steeringCommittee.SCID).ToList();
                    wave_data.ForEach(a =>
                    {
                        a.Record_Status = "Deleted";
                        a.LastUpdatedDate = TodayDate;
                        a.LastUpdatedBy = steeringCommittee.LastUpdatedBy;
                    });
                    var riskgap_data = entity.RisksGaps.Where(x => x.SCID == steeringCommittee.SCID).ToList();
                    riskgap_data.ForEach(a =>
                    {
                        a.Record_Status = "Deleted";
                        a.LastUpdatedDate = TodayDate;
                        a.LastUpdatedBy = steeringCommittee.LastUpdatedBy;
                    });
                    entity.SaveChanges();
                    ManualDataClass manualData = new ManualDataClass();
                    manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", "Active", "Deleted", "Record_Status", steeringCommittee.LastUpdatedBy);
                    sc_r.code = 200;
                    sc_r.message = "Deleted Succesfully";
                    return Content(HttpStatusCode.OK, sc_r);
                }
                else
                {
                    sc_r.code = 100;
                    sc_r.message = "The Client you are trying to delete is not available in the data, please refresh the screen and try again.";
                    return Content(HttpStatusCode.OK, sc_r);
                }
            }
        }

        [HttpPost]
        [Route("DeleteWave")]
        public IHttpActionResult DeleteWave(Wave wave)
        {
            if (wave.WaveID == null || wave.LastUpdatedBy == null || wave.LastUpdatedBy == "")
            {
                sc_r.code = 100;
                sc_r.message = "Something went wrong please try again after sometime";
                return Content(HttpStatusCode.OK, sc_r);
            }
            else
            {
                var checkingW_id = entity.Waves.Where(x => x.WaveID == wave.WaveID).Count();
                if (checkingW_id > 0)
                {
                    DateTime TodayDate = DateTime.Now;
                    Wave sc_d = (from a in entity.Waves
                                    where a.WaveID == wave.WaveID
                                    select a).FirstOrDefault();
                    sc_d.Record_Status = "Deleted";
                    sc_d.LastUpdatedBy = wave.LastUpdatedBy;
                    sc_d.LastUpdatedDate = TodayDate;
                    entity.SaveChanges();
                    ManualDataClass manualData = new ManualDataClass();
                    manualData.auditLog(wave.WaveID, "", "WaveUpdate", "Active", "Deleted", "Record_Status", wave.LastUpdatedBy);
                    sc_r.code = 200;
                    sc_r.message = "Deleted Succesfully";
                    return Content(HttpStatusCode.OK, sc_r);
                }
                else
                {
                    sc_r.code = 100;
                    sc_r.message = "The Wave you are trying to delete is not available in the data, please refresh the screen and try again.";
                    return Content(HttpStatusCode.OK, sc_r);
                }
            }
        }

        [HttpPost]
        [Route("DeleteRiskGap")]
        public IHttpActionResult DeleteRiskGap(RisksGap risksGap)
        {
            if (risksGap.RGID == null || risksGap.LastUpdatedBy == null || risksGap.LastUpdatedBy == "")
            {
                sc_r.code = 100;
                sc_r.message = "Something went wrong please try again after sometime";
                return Content(HttpStatusCode.OK, sc_r);
            }
            else
            {
                var checkingR_id = entity.RisksGaps.Where(x => x.RGID == risksGap.RGID).Count();
                if (checkingR_id > 0)
                {
                    DateTime TodayDate = DateTime.Now;
                    RisksGap sc_d = (from a in entity.RisksGaps
                                 where a.RGID == risksGap.RGID
                                 select a).FirstOrDefault();
                    sc_d.Record_Status = "Deleted";
                    sc_d.LastUpdatedBy = risksGap.LastUpdatedBy;
                    sc_d.LastUpdatedDate = TodayDate;
                    entity.SaveChanges();
                    ManualDataClass manualData = new ManualDataClass();
                    manualData.auditLog(risksGap.RGID, "", "RiskGapUpdate", "Active", "Deleted", "Record_Status", risksGap.LastUpdatedBy);
                    sc_r.code = 200;
                    sc_r.message = "Deleted Succesfully";
                    return Content(HttpStatusCode.OK, sc_r);
                }
                else
                {
                    sc_r.code = 100;
                    sc_r.message = "The Risk Gap you are trying to delete is not available in the data, please refresh the screen and try again.";
                    return Content(HttpStatusCode.OK, sc_r);
                }
            }
        }

        [HttpPost]
        [Route("KnowledgeBase")]
        public Response getKnowledgeBase(KnowledgeBase knowledgeBase)
        {
            var data = (from a in entity.KnowledgeBases
                        select a).ToList();
            re.Data = data;
            re.message = "success";
            re.code = 200;
            return re;
        }

        [HttpPost]
        [Route("GetSCDataUsingOppID")]
        public SteeringCommitteeData GetSCDataUsingOppID(CLRData cLRData)
        {
            if(cLRData.Opportunity_ID == null)
            {
                sc_r.Data = null;
                sc_r.message = "Please enter Opportunity ID";
                sc_r.code = 100;
            }
            else
            {
                var checkingOpp_id = entity.CLRDatas.Where(x => x.Opportunity_ID == cLRData.Opportunity_ID).Count();
                if(checkingOpp_id > 0)
                {
                    var data = (from a in entity.CLRDatas
                                where a.Opportunity_ID == cLRData.Opportunity_ID
                                where a.Status == "Active"
                                //where (a.ProjectStatus == "P-Pipeline" || a.ProjectStatus == "HP-High Potential" || a.ProjectStatus == "EP-Early Potential") && (a.GlobalProjectManager != "---" || a.RegionalProjectManager != "---" || a.AssigneeFullName != "---")
                                where a.ProjectStatus != "HP-High Potential"
                                where a.ProjectStatus != "EP-Early Potential"
                                where a.ProjectStatus != "P-Pipeline"
                                select new {
                                    op_Account_Name = a.Account_Name,
                                    op_Regions = (from b in entity.CLRDatas
                                                  where b.Status == "Active"
                                                  where b.Opportunity_ID == cLRData.Opportunity_ID
                                                  select new {
                                                      Region = b.Region
                                                  }).Distinct(),
                                    op_Countries = (from b in entity.CLRDatas
                                                    where b.Status == "Active"
                                                    where b.Opportunity_ID == cLRData.Opportunity_ID
                                                    select new
                                                    {
                                                        Country = b.Country
                                                    }).Distinct(),
                                    op_ProjectLead = a.GlobalProjectManager,
                                    op_TotalVolume = a.OppTOtalVolume,
                                    op_ELTProjectStatus = a.Workspace__ELT_Overall_Status,
                                    op_CycleTimeCategory = a.CycleTimeCategories,
                                    op_RevenueVolume = entity.CLRDatas.Where(x => x.Opportunity_ID == cLRData.Opportunity_ID && x.Status == "Active").Sum(x => x.RevenueVolumeUSD)
                                });
                    sc_r.Data = data;
                    sc_r.message = "success";
                    sc_r.code = 200;
                }
                else
                {
                    sc_r.Data = null;
                    sc_r.message = "No data found using "+ cLRData.Opportunity_ID +" Opportunity ID";
                    sc_r.code = 202;
                }
            }
            return sc_r;
        }
        [HttpPost]
        [Route("SteeringCommitteeData")]
        public SteeringCommitteeData SteeringCommitteeData(SteeringCommitteeData steeringCommitteeData)
        {
            var data = (from a in entity.SteeringCommittees
                        where a.Record_Status == "Active"
                        select new
                        {
                            a.SCID,
                            a.RecordStatus,
                            a.ClientName,
                            a.ClientType,
                            a.ProjectLead,
                            a.ProjectStatus,
                            a.PreviousStatus,
                            a.TotalBusineesVolume,
                            a.NewBusinessVolume,
                            Region = a.Region == "" ? "---" : a.Region ?? "---",
                            Country = a.Country == "" ? "---" : a.Country ?? "---",
                            CurrentState = a.CurrentState == "" ? "---" : a.CurrentState ?? "---",
                            CompletedKeyDeliverables = a.CompletedKeyDeliverables == "" ? "---" : a.CompletedKeyDeliverables ?? "---",
                            ScheduledKeyDeliverables = a.ScheduledKeyDeliverables == "" ? "---" : a.ScheduledKeyDeliverables ?? "---",
                            AdditionalNotes = a.AdditionalNotes == "" ? "---" : a.AdditionalNotes ?? "---",
                            KeyAccomplishmentsSinceLastUpdateKeyDeliverables = a.KeyAccomplishmentsSinceLastUpdateKeyDeliverables == "" ? "" : a.KeyAccomplishmentsSinceLastUpdateKeyDeliverables ?? "",
                            KeyUpcomingActivitiesKeyDeliverables = a.KeyUpcomingActivitiesKeyDeliverables == "" ? "" : a.KeyUpcomingActivitiesKeyDeliverables ?? "",
                            a.InsertedBy,
                            a.InsertedDate,
                            a.LastUpdatedBy,
                            a.LastUpdatedDate,
                            Waves = (from w in entity.Waves
                                     where a.SCID == w.SCID
                                     where w.Record_Status == "Active"
                                     select new
                                     {
                                         w.WaveID,
                                         w.SCID,
                                         Region = w.Region == "" ? "---" : w.Region ?? "---",
                                         Country = w.Country == "" ? "---" : w.Country ?? "---",
                                         Scope = w.Scope == "" ? "---" : w.Scope ?? "---",
                                         w.GoLiveDate,
                                         w.Status,
                                         w.InsertedBy,
                                         w.InsertedDate,
                                         LastUpdatedBy = w.LastUpdatedBy ?? "---",
                                         w.LastUpdatedDate,
                                         w.Waves
                                     }).OrderBy(x => x.WaveID),
                            RiskGaps = (from rg in entity.RisksGaps
                                        where a.SCID == rg.SCID
                                        where rg.Record_Status == "Active"
                                        select new
                                        {
                                            rg.RGID,
                                            rg.SCID,
                                            rg.RiskCategory,
                                            rg.Region,
                                            rg.Country,
                                            RisksGaps = rg.RisksGaps == "" ? "---" : rg.RisksGaps ?? "---",
                                            MitigationPlan = rg.MitigationPlan == "" ? "---" : rg.MitigationPlan ?? "---",
                                            SteeringCommitteeSupportNeed = rg.SteeringCommitteeSupportNeed == "" ? "---" : rg.SteeringCommitteeSupportNeed ?? "---",
                                            SupportNeededDetails = rg.SupportNeededDetails == "" ? "---" : rg.SupportNeededDetails ?? "---",
                                            rg.Impact,
                                            rg.Likelihood,
                                            rg.DueDate,
                                            Owner = rg.Owner == "" ? "---" : rg.Owner ?? "---",
                                            rg.Status,
                                            rg.InsertedDate,
                                            rg.InsertedBy,
                                            LastUpdatedBy = rg.LastUpdatedBy ?? "---",
                                            rg.LastUpdatedDate,
                                            rg.Risks
                                        }).OrderByDescending(x => x.Status),
                        }).OrderBy(x => x.RecordStatus);
            sc_r.Data = data;
            sc_r.message = "Success";
            sc_r.code = 200;
            return sc_r;
        }
    }
}