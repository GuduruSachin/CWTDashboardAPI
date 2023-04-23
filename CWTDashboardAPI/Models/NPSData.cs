using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class NPSData
    {
        public string message { get; set; }
        public int code { get; set; }
        public object NPSViewData { get; set; }
        public object NPSvalues { get; set; }
        public object NewRegionWiseNPSScore { get; set; }
        public object ExistingRegionWiseNPSScore { get; set; }
        public object NewRegionWiseNPSCount { get; set; }
        public object ExistingRegionWiseNPSCount { get; set; }
        public object NewSemanticAnalysisOne { get; set; }
        public object NewSemanticAnalysisTwo { get; set; }
        public object NewSemanticAnalysisThree { get; set; }
        public object ExistingSemanticAnalysisOne { get; set; }
        public object ExistingSemanticAnalysisTwo { get; set; }
        public object ExistingSemanticAnalysisThree { get; set; }
        public object NERegionWiseNPSScore { get; set; }
        public object NERegionWiseNPSCount { get; set; }
        public object NESemanticAnalysisOne { get; set; }
        public object NESemanticAnalysisTwo { get; set; }
        public object NESemanticAnalysisThree { get; set; }
    }
}