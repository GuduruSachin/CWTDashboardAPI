using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class LessonsLearntFilter
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Region { get; set; }
        public object Status { get; set; }
    }
}