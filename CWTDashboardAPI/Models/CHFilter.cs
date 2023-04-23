using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class CHFilter
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Region { get; set; }
        public object Level { get; set; }
        public object Leader { get; set; }
        public object WorkShedule { get; set; }
        public object WorkingDays { get; set; }
    }
}