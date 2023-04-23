using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class CTOFilters
    {
        public string message { get; set; }
        public int code { get; set; }
        public object ProjectStatus { get; set; }
        public object  CriticalOverDue{ get; set; }
        public object GroupName { get; set; }
        public object ProjectLevel { get; set; }
        public object Region { get; set; }
        public object Country { get; set; }
        public object AssigneFullName { get; set; }
    }
    public class CycleTimeCategories
    {
        public string CycleTimeCategory { get; set; }
        public int January_PC { get; set; }
        public int February_PC { get; set; }
        public int March_PC { get; set; }
        public int April_PC { get; set; }
        public int May_PC { get; set; }
        public int June_PC { get; set; }
        public int H_One_PC { get; set; }
        public int July_PC { get; set; }
        public int August_PC { get; set; }
        public int September_PC { get; set; }
        public int October_PC { get; set; }
        public int November_PC { get; set; }
        public int December_PC { get; set; }
        public int H_Two_PC { get; set; }
        public int Total_PC { get; set; }
        public double January_A { get; set; }
        public double February_A { get; set; }
        public double March_A { get; set; }
        public double April_A { get; set; }
        public double May_A { get; set; }
        public double June_A { get; set; }
        public double H_One_A { get; set; }
        public double July_A { get; set; }
        public double August_A { get; set; }
        public double September_A { get; set; }
        public double October_A { get; set; }
        public double November_A { get; set; }
        public double December_A { get; set; }
        public double H_Two_A { get; set; }
        public double Total_A { get; set; }
    }

    public class ResourceUtilization {
        public double HierarchyID { get; set; }
        public string Region { get; set; }
        public string Level { get; set; }
        public string Leader { get; set; }
        public string WorkShedule { get; set; }
        public double Monday { get; set; }
        public double Tuesday { get; set; }
        public double Wednesday { get; set; }
        public double Thursday { get; set; }
        public double Friday { get; set; }
        public string Manager { get; set; }
        public string Comments { get; set; }
        public string ProjectLevel { get; set; }
        public Nullable<double> WorkingDays { get; set; }
        public string PLevel { get; set; }
        public double TargetedUtilization { get; set; }
        public double AvgUtil { get; set; }
        public double TUWorkingDays { get; set; }
        public double CapacityAvailable { get; set; }
        public double CapacityAvailableTUWorkingDays { get; set; }
        public double C1stweek { get; set; }
        public double C2ndweek { get; set; }
        public double C3rdweek { get; set; }
        public double C4thweek { get; set; }
        public double C5thweek { get; set; }
        public double C6thweek { get; set; }
        public double C7thweek { get; set; }
        public double C8thweek { get; set; }
        public double C9thweek { get; set; }
        public double C10thweek { get; set; }
        public double C11thweek { get; set; }
        public double C12thweek { get; set; }
        public double c13thweek { get; set; }
        public double c14thweek { get; set; }
        public double c15thweek { get; set; }
        public double c16thweek { get; set; }
        public double c17thweek { get; set; }
        public double c18thweek { get; set; }
        public double c19thweek { get; set; }
        public double c20thweek { get; set; }
        public double c21thweek { get; set; }
        public double c22thweek { get; set; }
        public double c23thweek { get; set; }
        public double c24thweek { get; set; }
    }

}