using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class StageGateFilters
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Year { get; set; }
        public object TaskStatus { get; set; }
        public object Months { get; set; }
        public object ProjectStatus { get; set; }
    }
}