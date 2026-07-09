using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class ELTDeltaComments
    {

        public string Client { get; set; }
        public string SalesStageNameActivityData { get; set; }
        public double RevenueID { get; set; }
        public string ProjectStatus { get; set; }
        public string GoLiveMonth { get; set; }
        public string GoLiveYear { get; set; }
        public string Country { get; set; }
        public string Region { get; set; }
        public string Workspace_Title { get; set; }
        public double PreviousVolume { get; set; }
        public double CurrentVolume { get; set; }
        public double RevenueVolumeUSD { get; set; }
        public string CurrentProjectStatus { get; set; }
        public string CurrentMonth { get; set; }
        public string CurrentYear { get; set; }
        public string DeltaColor { get; set; }
        public string Comments { get; set; }
    }
}