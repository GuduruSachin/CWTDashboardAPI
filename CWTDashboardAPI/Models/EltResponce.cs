using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class EltResponce
    {
        public string message { get; set; }
        public int code { get; set; }
        public object Data { get; set; }
        public object YearMonth { get; set; }
        public object ELTDeltaComments { get; set; }
        public string[] YearMonths { get; set; }
        public double? TotalAmountMonth1 { get; set; }
        public double? TotalAmountPriorMonth1 { get; set; }
        public double? TotalAmountMonth2 { get; set; }
        public double? TotalAmountRemainingMonths { get; set; }
        public double? GrandTotal { get; set; }
        public string ColumnOne { get; set; }
        public string ColumnTwo { get; set; }
        public string ColumnThree { get; set; }
        public string ColumnYearName { get; set; }
        public string concatstring { get; set; }
    }
}
