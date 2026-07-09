using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class eSowResponse
    {
        public int count { get; set; }
        public List<eSow_Data> data { get; set; }
        public List<string> labels { get; set; }
        public List<string> types { get; set; }
    }
    public class eSow_Data
    {
        public string Client { get; set; }
        public int? id { get; set; }

        [JsonProperty("eSoW Ref")]
        public string eSoWRef { get; set; }
        public string crmOpportunityId { get; set; }
        public string createdOn { get; set; }
        public string modifiedOn { get; set; }

        [JsonProperty("Client Type")]
        public string ClientType { get; set; }

        [JsonProperty("Account Category")]
        public string AccountCategory { get; set; }

        [JsonProperty("Proposal Type")]
        public string ProposalType { get; set; }

        [JsonProperty("Account Owner")]
        public string AccountOwner { get; set; }

        [JsonProperty("SoW Owner")]
        public string SoWOwner { get; set; }

        [JsonProperty("Self Service")]
        public string SelfService { get; set; }

        [JsonProperty("eSoW Status")]
        public string eSoWStatus { get; set; }

        [JsonProperty("CRM Status")]
        public string CRMStatus { get; set; }
        public string implementationReady { get; set; }
        public string Option { get; set; }

        [JsonProperty("Country Name")]
        public string CountryName { get; set; }
        public string serviceConfig { get; set; }
        public string Team { get; set; }
        public string configLoc { get; set; }
        public string configName { get; set; }
        public string afterHours { get; set; }
        public string GDS { get; set; }
        public string OBT { get; set; }

        [JsonProperty("OBT Adoption Rate")]
        public string OBTAdoptionRate { get; set; }

        [JsonProperty("Direct/Reseller")]
        public string DirectReseller { get; set; }
        public string Change { get; set; }
        // Add other properties as per your API response
    }
}