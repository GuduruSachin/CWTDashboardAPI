using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class DigitalTeamModel
    {
        public int ManualID { get; set; }
        public int DTID { get; set; }
        public int CLRID { get; set; }
        public double RevenueID { get; set; }
        public string Client { get; set; }
        public string GlobalCISOBTLead { get; set; }
        public string GlobalCISDQSLead { get; set; }
        public string RegionalCISOBTLead { get; set; }
        public string LocalDigitalOBTLead { get; set; }
        public string GlobalCISPortraitLead { get; set; }
        public string RegionalCISPortraitLead { get; set; }
        public string GlobalCISHRFeedSpecialist { get; set; }
        public string Implementation_Type { get; set; }
        public string ActivityType { get; set; }
        public string GDS { get; set; }
        public double ComplexityScore { get; set; }
        public string TaskRecordIdKey { get; set; }
        public Nullable<System.DateTime> InsertedOn { get; set; }
        public Nullable<System.DateTime> UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
    }
}