using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class CapacityFilters
    {
        public string message { get; set; }
        public int code { get; set; }
        public object ProjectStatus { get; set; }
        public object ProjectLevel { get; set; }
        public object Region { get; set; }
        public object Years { get; set; }
        public object ProjectSum { get; set; }
    }
}