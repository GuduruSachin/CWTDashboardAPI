using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class Filters
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Year { get; set; }
        public object c_Year { get; set; }
        public object Quarter { get; set; }
        public object Months { get; set; }
        public object c_Months { get; set; }
        public object ProjectLevel { get; set; }
        public object c_ProjectLevel { get; set; }
        public object Region { get; set; }
        public object Status { get; set; }
        public object MarketLeaders { get; set; }
        public object MilestoneStatus { get; set; }
        public object c_MilestoneStatus { get; set; }
        public object ImplementationType { get; set; }
    }
}