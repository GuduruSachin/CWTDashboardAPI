using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class SteeringCommitteeFilters
    {
        public string message { get; set; }
        public int code { get; set; }
        public object RegionCountry { get; set; }
        public object Owner { get; set; }
    }
}