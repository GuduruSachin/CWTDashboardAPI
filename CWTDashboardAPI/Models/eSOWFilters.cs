using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class eSOWFilters
    {
        public string message { get; set; }
        public int code { get; set; }
        public object ProspectType { get; set; }
        public object SalesLeaderTypeAndTypeOfBid { get; set; }
    }
}