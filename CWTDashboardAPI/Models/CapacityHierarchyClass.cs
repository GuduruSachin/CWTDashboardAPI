using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class CapacityHierarchyClass
    {
        public string Manager, ManagerName;
        public int count, HierarchyID;
        public Boolean AddingHierarchy(CapacityHierarchy hierarchy)
        {
            List<CapacityHierarchy> list = new List<CapacityHierarchy>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.CapacityHierarchies.OrderBy(a => a.HID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    Manager = list[i].Manager;
                    if (hierarchy.Manager.Equals(Manager) && list[i].Manager == "Active")
                    {
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public Boolean UpdateHierarchy(CapacityHierarchy hierarchy)
        {
            List<CapacityHierarchy> list = new List<CapacityHierarchy>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.CapacityHierarchies.OrderBy(a => a.HID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    HierarchyID = list[i].HID;
                    if (hierarchy.HID.Equals(HierarchyID))
                    {
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        CapacityHierarchy hierarchydata = (from s in entit.CapacityHierarchies
                                                   where s.HID == hierarchy.HID
                                                   select s).FirstOrDefault();
                        hierarchydata.Region = hierarchy.Region;
                        hierarchydata.Level = hierarchy.Level;
                        hierarchydata.PLevel = hierarchy.PLevel;
                        hierarchydata.Leader = hierarchy.Leader;
                        hierarchydata.Manager = hierarchy.Manager;
                        hierarchydata.WorkShedule = hierarchy.WorkShedule;
                        hierarchydata.Monday = hierarchy.Monday;
                        hierarchydata.Tuesday = hierarchy.Tuesday;
                        hierarchydata.Wednesday = hierarchy.Wednesday;
                        hierarchydata.Thursday = hierarchy.Thursday;
                        hierarchydata.Friday = hierarchy.Friday;
                        hierarchydata.WorkingDays = hierarchy.WorkingDays;
                        hierarchydata.ProjectLevel = hierarchy.ProjectLevel;
                        hierarchydata.InsertedBy = hierarchy.InsertedBy;
                        hierarchydata.InsertedDate = DateTime.Now;
                        hierarchydata.TargetedUtilization = hierarchy.TargetedUtilization;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }

        public Boolean UpdateC_HierarchyComment(CapacityHierarchy hierarchy)
        {
            List<CapacityHierarchy> list = new List<CapacityHierarchy>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.CapacityHierarchies.OrderBy(a => a.HID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    HierarchyID = list[i].HID;
                    if (hierarchy.HID.Equals(HierarchyID))
                    {
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        CapacityHierarchy hierarchydata = (from s in entit.CapacityHierarchies
                                                           where s.HID == hierarchy.HID
                                                           select s).FirstOrDefault();
                        if(hierarchy.Comments == null || hierarchy.Comments == "")
                        {
                            hierarchydata.Comments = "---";
                        }
                        else
                        {
                            hierarchydata.Comments = hierarchy.Comments;
                        }
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
    }
}