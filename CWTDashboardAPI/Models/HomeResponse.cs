using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class HomeResponse
    {
        public string message { get; set; }
        public int code { get; set; }
        public string TotalVolume { get; set; }
        public string ActiveVolume { get; set; }
        public string ClosedVolume { get; set; }
        public string P_NDC_H_Volume { get; set; }
        public string Projects { get; set; }
        public string PipelineVolume { get; set; }
        public string CurrentMonth { get; set; }
        public string CurrentMonthVolume { get; set; }
        public string CurrentMonthNActiveVolume { get; set; }
        public string CurrentMonthRecords { get; set; }
        public string CurrentMonthNActiveRecords { get; set; }
        public string NextMonthVolume { get; set; }
        public string NextMonthNActiveVolume { get; set; }
        public string NextMonthRecords { get; set; }
        public string NextMonthNActiveRecords { get; set; }
        public string PreMonthVolume { get; set; }
        public string PreMonthRecords { get; set; }
        public string RoyMonthVolume { get; set; }
        public string RoyMonthRecords { get; set; }
        public string ExpectedCurrentMonthVolume { get; set; }
        public string ExpectedCurrentMonthRecords { get; set; }
        public string FutureYearsVolume { get; set; }
        public string FutureYearsRecords { get; set; }
        public string HoldVolume { get; set; }
        public string HoldRecords { get; set; }
        public string PipelineRecords { get; set; }
        public string HighPotentialVolume { get; set; }
        public string HighPotentialRecords { get; set; }
        public string PotentialVolume { get; set; }
        public string PotentialRecords { get; set; }
        public string NextMonth { get; set; }
        public object data { get; set; }
        public object Countries { get; set; }

        public object NpsData { get; set; }
        public object RollingNpsData { get; set; }
        public string NewGlobalProjectCount { get; set; }
        public string NewGlobalCycleTime { get; set; }
        public string NewLocalCycleTime { get; set; }
        public string NewLocalProjectCount { get; set; }
        public string ExistingAddChangeCycleTime { get; set; }
        public string ExistingAddChangeProjectCount { get; set; }
        public string ExistingServiceProjectCount { get; set; }
        public string ExistingServiceCycleTime { get; set; }
        public string OverallProjectCount { get; set; }
        public string OverallCycleTime { get; set; }
    }
}