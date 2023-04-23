using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class HierarchyGraphs
    {
        public string message { get; set; }
        public int code { get; set; }
        public object RegionWiseData { get; set; }
        public object LevelWiseData { get; set; }
        public object StatusWiseData { get; set; }
        public object LeaderWiseData { get; set; }
        public object MonthWiseData { get; set; }
    }
}