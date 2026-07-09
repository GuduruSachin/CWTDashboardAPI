using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class LoginUser
    {
        public String message { get; set; }
        public Int64 code { get; set; }
        public String FirstName { get; set; }
        public String LastName { get; set; }
        public String UID { get; set; }
        public Int64 UserID { get; set; }
        public String EmailId { get; set; }
        public String Password { get; set; }
        public String JobType { get; set; }
        public String UserStatus { get; set; }
        public String Manager { get; set; }
        public DateTime InsertedDate { get; set; }
        public String AccountStatus { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public String UpdatedBy { get; set; }
        public Int64 AccessID { get; set; }
        public Boolean IMPS { get; set; }
        public Boolean CTO { get; set; }
        public Boolean StageGate { get; set; }
        public Boolean LessonsLearnt { get; set; }
        public Boolean CapacityTracker { get; set; }
        public Boolean AutomatedCLR { get; set; }
        public Boolean NPS { get; set; }
        public Boolean NPSClientInfo { get; set; }
        public Boolean NPSAdmin { get; set; }
        public Boolean DigitalReport { get; set; }
        public Boolean NPSEdit { get; set; }
        public Boolean CycleTime { get; set; }
        public Boolean EltReport { get; set; }
        public Boolean C_Hierarchy { get; set; }
        public Boolean ResourceUtilization { get; set; }
        public Boolean Prospect { get; set; }
        public String UserAccessStatus { get; set; }
        public Boolean SteeringCommittee { get; set; }
        public Boolean SteeringCommitteeEdits { get; set; }
        public Boolean DDO { get; set; }
        public Boolean DDOHome { get; set; }
        public Boolean PriorityReport { get; set; }
    }
}