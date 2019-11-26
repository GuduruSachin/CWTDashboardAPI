using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class Response
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Data { get; set; }
    }
}