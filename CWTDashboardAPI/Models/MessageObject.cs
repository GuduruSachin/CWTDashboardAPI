using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class MessageObject
    {
        public string queryName { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public DateTime modifiedDate { get; set; }
        public bool onlyWonOrSubmitted { get; set; }
        public bool preferredOption { get; set; }
        public bool debug { get; set; }
        public int? id { get; set; }
        public int? limit { get; set; }
        public bool includeTest { get; set; }
    }
}