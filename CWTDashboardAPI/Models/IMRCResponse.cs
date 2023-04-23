using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class IMRCResponse
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Data { get; set; }
        public object VolumeCountCycleTime { get; set; }
        public object CycleTimeData { get; set; }
        public object CycleTimeByCategories { get; set; }
    }
}