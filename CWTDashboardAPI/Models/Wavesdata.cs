using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public partial class Wavesdata
    {
        public int WaveID { get; set; }
        public Nullable<int> SCID { get; set; }
        public string Region { get; set; }
        public string Country { get; set; }
        public string Scope { get; set; }
        public Nullable<System.DateTime> GoLiveDate { get; set; }
        public string Status { get; set; }
        public Nullable<System.DateTime> InsertedDate { get; set; }
        public string InsertedBy { get; set; }
        public string LastUpdatedBy { get; set; }
        public Nullable<System.DateTime> LastUpdatedDate { get; set; }
    }
}
