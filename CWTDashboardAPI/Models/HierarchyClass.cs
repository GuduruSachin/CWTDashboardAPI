using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class HierarchyClass
    {
        string Assigne_name, UserID;
        string U_Assigne_name, U_UserID, D_UserID;
        public int count;
        public Boolean AddingHierarchy(Hierarchy hierarchy)
        {
            List<Hierarchy> list = new List<Hierarchy>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Hierarchies.OrderBy(a => a.HierarchyID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    Assigne_name = list[i].Name;
                    UserID = list[i].User_ID;
                    if (hierarchy.Name.Equals(Assigne_name) && hierarchy.User_ID.Equals(UserID))
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
        public Boolean UpdateHierarchy(Hierarchy hierarchy)
        {
            List<Hierarchy> list = new List<Hierarchy>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Hierarchies.OrderBy(a => a.HierarchyID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    U_Assigne_name = list[i].Name;
                    U_UserID = list[i].User_ID;
                    if (hierarchy.Name.Equals(U_Assigne_name) && hierarchy.User_ID.Equals(U_UserID))
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
                        Hierarchy vp = (from s in entit.Hierarchies
                                                  where s.User_ID == hierarchy.User_ID
                                                  where s.HierarchyID == hierarchy.HierarchyID
                                                  select s).FirstOrDefault();
                        vp.Name = hierarchy.Name;
                        vp.LeaderTwo = hierarchy.LeaderTwo;
                        vp.LeaderOne = hierarchy.LeaderOne;
                        vp.Sr_Leader = hierarchy.Sr_Leader;
                        vp.VP = hierarchy.VP;
                        vp.Email_Address = hierarchy.Email_Address;
                        vp.Region = hierarchy.Region;
                        vp.Location = hierarchy.Location;
                        vp.Role = hierarchy.Role;
                        vp.Title = hierarchy.Title;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public Boolean DeleteHierarchy(Hierarchy hierarchy)
        {
            List<Hierarchy> list = new List<Hierarchy>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Hierarchies.OrderBy(a => a.HierarchyID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    D_UserID = list[i].User_ID;
                    if (hierarchy.User_ID.Equals(D_UserID))
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
                        Hierarchy vp = (from s in entit.Hierarchies
                                        where s.User_ID == hierarchy.User_ID
                                        select s).FirstOrDefault();
                        vp.UserStatus = "In-Active";
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        //public Boolean InsertMonthWiseDelta(MonthWiseDelta monthWiseDelta)
        //{
        //    List<MonthWiseDelta> list = new List<MonthWiseDelta>();
        //    using (CWTDashboardEntities entity = new CWTDashboardEntities())
        //    {
        //        list = entity.MonthWiseDeltas.OrderBy(a => a.DeltaID).ToList();
        //        int c = list.Count;
        //        for (int i = 0; i < c; i++)
        //        {
        //            InsertedDate = list[i].InsertedDate;
        //            if (monthWiseDelta.InsertedDate.Equals(InsertedDate))
        //            {
        //                count = 1;
        //                break;
        //            }
        //            else
        //            {
        //                count = 0;
        //            }
        //        }
        //        if (count == 0)
        //        {
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}
    }
}