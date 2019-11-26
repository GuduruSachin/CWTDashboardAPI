using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class IMPSFilters
    {
        public string message { get; set; }
        public int code { get; set; }
        public object ProjectStatus { get; set; }
        public object ProjectLevel { get; set; }
        public object Region { get; set; }
        public object Assignee { get; set; }
        public object Assignee_ReportTO { get; set; }
    }
}