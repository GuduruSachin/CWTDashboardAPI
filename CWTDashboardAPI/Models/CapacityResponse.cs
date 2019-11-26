using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class CapacityResponse
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Data { get; set; }
        public object SumsData { get; set; }
    }
}