using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class ReplicateManualData
    {
        public Nullable<double> Revenue_ID { get; set; }
        public string Revenue_IDs { get; set; }
        public string Client { get; set; }
        public Nullable<System.DateTime> Date_added_to_the_CLR { get; set; }
        public string Implementation_Type { get; set; }
        public string Pipeline_status { get; set; }
        public string Pipeline_comments { get; set; }
        public string TXResourcing { get; set; }
        public string Service_configuration { get; set; }
        public string OBT_Reseller___Direct { get; set; }
        public string Servicing_location { get; set; }
        public Nullable<System.DateTime> ExpectedDecisionDate { get; set; }
        public Nullable<System.DateTime> Assignment_date { get; set; }
        public Nullable<System.DateTime> ResourceRequest_date { get; set; }
        public Nullable<System.DateTime> _date { get; set; }
        public string EMEA_Country_to_charge { get; set; }
        public string EMEA_Client { get; set; }
        public Nullable<double> EMEA_OBT_standard_fee { get; set; }
        public string EMEA_Included_for_accrual { get; set; }
        public Nullable<System.DateTime> EMEA_Accrual_date { get; set; }
        public string EMEA_Scope_description { get; set; }
        public string EMEA_Billing_notes { get; set; }
        public string Manual_Entry__Wave_2__Wave_3__etc_ { get; set; }
        public Nullable<double> Project_Effort { get; set; }
        public string Priority { get; set; }
        public string Resource_Status { get; set; }
        public string GlobalCISOBTLead { get; set; }
        public string RegionalCISOBTLead { get; set; }
        public string LocalDigitalOBTLead { get; set; }
        public string GlobalCISPortraitLead { get; set; }
        public string RegionalCISPortraitLead { get; set; }
        public string GlobalCISHRFeedSpecialist { get; set; }
        public Nullable<System.DateTime> GoLiveDate { get; set; }
        public string GlobalProjectManager { get; set; }
        public string RegionalProjectManager { get; set; }
        public string AssigneeFullName { get; set; }
        public string Status { get; set; }
        public string ProjectLevel { get; set; }
        public Boolean Client_check { get; set; }
        public Boolean Date_added_to_the_CLR_check { get; set; }
        public Boolean Implementation_Type_check { get; set; }
        public Boolean Pipeline_status_check { get; set; }
        public Boolean Pipeline_comments_check { get; set; }
        public Boolean TXResourcing_check { get; set; }
        public Boolean Service_configuration_check { get; set; }
        public Boolean OBT_Reseller___Direct_check { get; set; }
        public Boolean Servicing_location_check { get; set; }
        public Boolean ExpectedDecision_date_check { get; set; }
        public Boolean Assignment_date_check { get; set; }
        public Boolean ResourceRequest_date_check { get; set; }
        public Boolean EMEA_Country_to_charge_check { get; set; }
        public Boolean EMEA_Client_check { get; set; }
        public Boolean EMEA_OBT_standard_fee_check { get; set; }
        public Boolean EMEA_Included_for_accrual_check { get; set; }
        public Boolean EMEA_Accrual_date_check { get; set; }
        public Boolean EMEA_Scope_description_check { get; set; }
        public Boolean EMEA_Billing_notes_check { get; set; }
        public Boolean Manual_Entry__Wave_2__Wave_3__etc_check { get; set; }
        public Boolean Project_Effort_check { get; set; }
        public Boolean Priority_check { get; set; }
        public Boolean Resource_Status_check { get; set; }
        public string GDS { get; set; }
        public Int64 ComplexityScore { get; set; }
        public string ActivityType { get; set; }
        public Boolean GDS_check { get; set; }
        public Boolean ComplexityScore_check { get; set; }
        public Boolean ActivityType_check { get; set; }
        public Boolean GlobalCISOBTLead_check { get; set; }
        public Boolean RegionalCISOBTLead_check { get; set; }
        public Boolean LocalDigitalOBTLead_check { get; set; }
        public Boolean GlobalCISPortraitLead_check { get; set; }
        public Boolean RegionalCISPortraitLead_check { get; set; }
        public Boolean GlobalCISHRFeedSpecialist_check { get; set; }
        public Boolean GoLiveDate_check { get; set; }
        public Boolean GlobalProjectManager_check { get; set; }
        public Boolean RegionalProjectManager_check { get; set; }
        public Boolean AssigneeFullName_check { get; set; }
        public Boolean Status_check { get; set; }
        public Boolean ProjectLevel_check { get; set; }
        public String UpdateBy { get; set; }
        public System.DateTime UpdateOn { get; set; }
    }
}