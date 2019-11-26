using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class CTOFilters
    {
        public string message { get; set; }
        public int code { get; set; }
        public object ProjectStatus { get; set; }
        public object  CriticalOverDue{ get; set; }
        public object GroupName { get; set; }
        public object ProjectLevel { get; set; }
        public object Region { get; set; }
        public object Country { get; set; }
    }
}