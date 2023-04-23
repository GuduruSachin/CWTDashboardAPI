using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class ETFilters
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Country { get; set; }
        public object Manager { get; set; }
        public object Resource { get; set; }
        public object AssignedTo { get; set; }
    }
}