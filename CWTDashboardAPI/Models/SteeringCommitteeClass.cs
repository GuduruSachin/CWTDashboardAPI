using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CWTDashboardAPI.Models
{
    public class SteeringCommitteeClass : ApiController
    {
        string  SC_ClientName;
        int SC_id,sc_count,wave_id,RG_ID;
        int? Wave_Sc_ID, RG_SC_ID;
        public Boolean UpdateSteeringCommittee(SteeringCommittee steeringCommittee)
        {
            List<SteeringCommittee> list = new List<SteeringCommittee>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.SteeringCommittees.OrderBy(a => a.SCID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    SC_ClientName = list[i].ClientName;
                    SC_id = list[i].SCID;
                    if (steeringCommittee.ClientName.Equals(SC_ClientName) && steeringCommittee.SCID.Equals(SC_id))
                    {
                        sc_count = 1;
                        break;
                    }
                    else
                    {
                        sc_count = 0;
                    }
                }
                if (sc_count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        SteeringCommittee SC = (from s in entit.SteeringCommittees
                                        where s.SCID == steeringCommittee.SCID
                                        select s).FirstOrDefault();
                        //ManualDataClass manualData = new ManualDataClass();
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.RecordStatus, steeringCommittee.RecordStatus, "Record Status", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.ClientType, steeringCommittee.ClientType, "Client Type", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.ProjectLead, steeringCommittee.ProjectLead, "Project Lead", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.ProjectStatus, steeringCommittee.ProjectStatus, "Project Status", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.ProjectTrend, steeringCommittee.ProjectTrend, "Project Trend", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.TotalBusineesVolume + "", steeringCommittee.TotalBusineesVolume + "", "Total Businees Volume", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.NewBusinessVolume + "", steeringCommittee.NewBusinessVolume + "", "New Business Volume", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.Region, steeringCommittee.Region, "Region", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.Country, steeringCommittee.Country, "Country", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.CurrentState, steeringCommittee.CurrentState, "Current State", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.CompletedKeyDeliverables, steeringCommittee.CompletedKeyDeliverables, "Completed Key Deliverables", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.ScheduledKeyDeliverables, steeringCommittee.ScheduledKeyDeliverables, "Scheduled Key Deliverables", steeringCommittee.LastUpdatedBy);
                        //manualData.auditLog(steeringCommittee.SCID, "", "SteeringCommitteeUpdate", SC.AdditionalNotes, steeringCommittee.AdditionalNotes, "Additional Notes", steeringCommittee.LastUpdatedBy);
                        SC.RecordStatus = steeringCommittee.RecordStatus;
                        SC.ClientType = steeringCommittee.ClientType;
                        SC.ProjectLead = steeringCommittee.ProjectLead;
                        SC.ProjectStatus = steeringCommittee.ProjectStatus;
                        SC.ProjectTrend = steeringCommittee.ProjectTrend;
                        SC.TotalBusineesVolume = steeringCommittee.TotalBusineesVolume;
                        SC.NewBusinessVolume = steeringCommittee.NewBusinessVolume;
                        SC.Region = steeringCommittee.Region;
                        SC.Country = steeringCommittee.Country;
                        SC.CurrentState = steeringCommittee.CurrentState;
                        SC.CompletedKeyDeliverables = steeringCommittee.CompletedKeyDeliverables;
                        SC.ScheduledKeyDeliverables = steeringCommittee.ScheduledKeyDeliverables;
                        SC.AdditionalNotes = steeringCommittee.AdditionalNotes;
                        SC.LastUpdatedBy = steeringCommittee.LastUpdatedBy;
                        DateTime TodayDate = DateTime.Now;
                        SC.LastUpdatedDate = TodayDate;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public Boolean UpdateWaves(Wave wave)
        {
            List<Wave> list = new List<Wave>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Waves.OrderBy(a => a.WaveID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    wave_id = list[i].WaveID;
                    Wave_Sc_ID = list[i].SCID;
                    if (wave.WaveID.Equals(wave_id) && wave.SCID.Equals(Wave_Sc_ID))
                    {
                        sc_count = 1;
                        break;
                    }
                    else
                    {
                        sc_count = 0;
                    }
                }
                if (sc_count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        Wave SC_W= (from w in entit.Waves
                                                where w.WaveID == wave.WaveID
                                                where w.SCID == wave.SCID
                                                select w).FirstOrDefault();
                        //ManualDataClass manualData = new ManualDataClass();
                        //manualData.auditLog(wave.WaveID, "", "WaveUpdate", SC_W.Region, wave.Region, "Region", wave.LastUpdatedBy);
                        //manualData.auditLog(wave.WaveID, "", "WaveUpdate", SC_W.Country, wave.Country, "Country", wave.LastUpdatedBy);
                        //manualData.auditLog(wave.WaveID, "", "WaveUpdate", SC_W.Scope, wave.Scope, "Scope", wave.LastUpdatedBy);
                        //manualData.auditLog(wave.WaveID, "", "WaveUpdate", SC_W.GoLiveDate + "", wave.GoLiveDate + "", "GoLiveDate", wave.LastUpdatedBy);
                        //manualData.auditLog(wave.WaveID, "", "WaveUpdate", SC_W.Status, wave.Status, "Status", wave.LastUpdatedBy);

                        SC_W.Waves = wave.Waves;
                        SC_W.Region = wave.Region;
                        SC_W.Country = wave.Country;
                        SC_W.Scope = wave.Scope;
                        SC_W.GoLiveDate = wave.GoLiveDate;
                        SC_W.Status = wave.Status;
                        SC_W.LastUpdatedBy = wave.LastUpdatedBy;
                        DateTime TodayDate = DateTime.Now;
                        SC_W.LastUpdatedDate = TodayDate;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public Boolean UpdateRiskGap(RisksGap riskgap)
        {
            List<RisksGap> list = new List<RisksGap>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.RisksGaps.OrderBy(a => a.RGID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    RG_ID = list[i].RGID;
                    RG_SC_ID = list[i].SCID;
                    if (riskgap.RGID.Equals(RG_ID) && riskgap.SCID.Equals(RG_SC_ID))
                    {
                        sc_count = 1;
                        break;
                    }
                    else
                    {
                        sc_count = 0;
                    }
                }
                if (sc_count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        RisksGap SC_RG = (from RG in entit.RisksGaps
                                     where RG.RGID == riskgap.RGID
                                     where RG.SCID == riskgap.SCID
                                     select RG).FirstOrDefault();
                        //ManualDataClass manualData = new ManualDataClass();
                        //manualData.auditLog(riskgap.RGID, "", "RiskGapUpdate", SC_RG.RisksGaps, riskgap.RisksGaps, "RisksGaps", riskgap.LastUpdatedBy);
                        //manualData.auditLog(riskgap.RGID, "", "RiskGapUpdate", SC_RG.MitigationPlan, riskgap.MitigationPlan, "MitigationPlan", riskgap.LastUpdatedBy);
                        //manualData.auditLog(riskgap.RGID, "", "RiskGapUpdate", SC_RG.SteeringCommitteeSupportNeed, riskgap.SteeringCommitteeSupportNeed, "SteeringCommitteeSupportNeed", riskgap.LastUpdatedBy);
                        //manualData.auditLog(riskgap.RGID, "", "RiskGapUpdate", SC_RG.DueDate + "", riskgap.DueDate + "", "DueDate", riskgap.LastUpdatedBy);
                        //manualData.auditLog(riskgap.RGID, "", "RiskGapUpdate", SC_RG.Owner, riskgap.Owner, "Owner", riskgap.LastUpdatedBy);
                        //manualData.auditLog(riskgap.RGID, "", "RiskGapUpdate", SC_RG.Status, riskgap.Status, "Status", riskgap.LastUpdatedBy);
                        SC_RG.Risks = riskgap.Risks;
                        SC_RG.RisksGaps = riskgap.RisksGaps;
                        SC_RG.MitigationPlan = riskgap.MitigationPlan;
                        SC_RG.SteeringCommitteeSupportNeed = riskgap.SteeringCommitteeSupportNeed;
                        SC_RG.DueDate = riskgap.DueDate;
                        SC_RG.Owner = riskgap.Owner;
                        SC_RG.Status = riskgap.Status;
                        SC_RG.LastUpdatedBy = riskgap.LastUpdatedBy;
                        DateTime TodayDate = DateTime.Now;
                        SC_RG.LastUpdatedDate = TodayDate;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
    }
}