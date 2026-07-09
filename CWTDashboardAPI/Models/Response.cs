using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class Response
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Data { get; set; }
        public object TargetCycleTimeData { get; set; }
        public object GlobalManager { get; set; }
        public object RegionalManager { get; set; }
        public object LocalManager { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public double? RevenueID { get; set; }
        public object CLRColumns { get; set; }
        public object PriorityData { get; set; }
        public object ProjectStatusActivityData { get; set; }
        public object SalesStageNameActivityData { get; set; }
        public object MonthlyTotalRevenueWithDelta { get; set; }
        public object ReportsUpdatedON { get; set; }
    }
}