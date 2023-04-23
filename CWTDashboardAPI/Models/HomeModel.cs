using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class HomeModel
    {
        string UID, TStatus, ReportName, GA_UID, UA_UID, GA_Status;
        int count,GACount,UACount, GA_TID;
        public Boolean RequestAccessTicket(UserReportAccessTicket userReportAccessTicket)
        {
            List<UserReportAccessTicket> list = new List<UserReportAccessTicket>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.UserReportAccessTickets.OrderBy(a => a.TicketID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    UID = list[i].UID;
                    ReportName = list[i].ReportName;
                    TStatus = list[i].TicketStatuts;
                    if (userReportAccessTicket.UID.Equals(UID) && userReportAccessTicket.ReportName.Equals(ReportName) && userReportAccessTicket.TicketStatuts.Equals(TStatus))
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

        public Boolean GrantAccessTicket(UserReportAccessTicket userReportAccessTicket)
        {
            List<UserReportAccessTicket> list = new List<UserReportAccessTicket>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.UserReportAccessTickets.OrderBy(a => a.TicketID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    GA_UID = list[i].UID;
                    GA_TID = list[i].TicketID;
                    GA_Status = list[i].TicketStatuts;
                    if (userReportAccessTicket.TicketID.Equals(GA_TID) && userReportAccessTicket.UID.Equals(GA_UID) && GA_Status == "Requested")
                    {
                        GACount = 1;
                        break;
                    }
                    else
                    {
                        GACount = 0;
                    }
                }
                if (GACount == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        UserReportAccessTicket vp = (from s in entit.UserReportAccessTickets
                                         where s.UID == userReportAccessTicket.UID
                                         where s.TicketID == userReportAccessTicket.TicketID
                                         select s).FirstOrDefault();
                        UserReportsAccess URA = (from a in entit.UserReportsAccesses
                                                 where a.UID == userReportAccessTicket.UID
                                                 select a).FirstOrDefault();

                        vp.TicketStatuts = userReportAccessTicket.TicketStatuts;
                        vp.AcceptedOn = DateTime.Now;
                        vp.AcceptedBy = userReportAccessTicket.AcceptedBy;
                        if(userReportAccessTicket.TicketStatuts == "Declined")
                        {
                            
                        }
                        else
                        {
                            URA.UpdatedBy = userReportAccessTicket.AcceptedBy;
                            URA.UpdatedOn = DateTime.Now;
                            if (userReportAccessTicket.ReportName == "AutomatedCLR")
                            {
                                URA.AutomatedCLR = true;
                            }
                            if (userReportAccessTicket.ReportName == "AutomatedCLREdits")
                            {
                                URA.CLREdits = true;
                            }
                            if (userReportAccessTicket.ReportName == "MarketReport")
                            {
                                URA.MarketReport = true;
                            }
                            if (userReportAccessTicket.ReportName == "CycleTime")
                            {
                                URA.CycleTime = true;
                            }
                            if (userReportAccessTicket.ReportName == "ELTReport")
                            {
                                URA.ELTReport = true;
                            }
                            if (userReportAccessTicket.ReportName == "IMPS")
                            {
                                URA.IMPS = true;
                            }
                            if (userReportAccessTicket.ReportName == "CTO")
                            {
                                URA.CTO = true;
                            }
                            if (userReportAccessTicket.ReportName == "LL")
                            {
                                URA.LessonsLearnt = true;
                            }
                            if (userReportAccessTicket.ReportName == "StageGate")
                            {
                                URA.StageGate = true;
                            }
                            if (userReportAccessTicket.ReportName == "MarketCommentEdits")
                            {
                                URA.MarketCommentsEdit = true;
                            }
                            if (userReportAccessTicket.ReportName == "CapacityTracker")
                            {
                                URA.CapacityTracker = true;
                            }
                            if (userReportAccessTicket.ReportName == "ResourceUtilization")
                            {
                                URA.ResourceUtilization = true;
                            }
                            if (userReportAccessTicket.ReportName == "CapacityHierarchy")
                            {
                                URA.C_Hierarchy = true;
                            }
                            if (userReportAccessTicket.ReportName == "CapacityHierarchyEdits")
                            {
                                URA.C_HierarchyEdits = true;
                            }
                            if (userReportAccessTicket.ReportName == "NPS")
                            {
                                URA.NPS = true;
                            }
                        }
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        string DU_UID;
        int DU_Count;
        public Boolean DeleteUserAccount(User users)
        {
            List<User> list = new List<User>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Users.OrderBy(a => a.UserID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    DU_UID = list[i].UID;
                    if (users.UID.Equals(DU_UID))
                    {
                        DU_Count = 1;
                        break;
                    }
                    else
                    {
                        DU_Count = 0;
                    }
                }
                if (DU_Count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        User user = (from s in entit.Users
                                                     where s.UID == users.UID
                                                     select s).FirstOrDefault();

                        user.UserStatus = "Deleted";
                        user.UpdatedOn = DateTime.Now;
                        user.UpdatedBy = users.UpdatedBy;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        int CH_HID,CH_Count;
        public Boolean DeleteUserinHierarchy(CapacityHierarchy capacityHierarchy)
        {
            List<CapacityHierarchy> list = new List<CapacityHierarchy>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.CapacityHierarchies.OrderBy(a => a.HID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    CH_HID = list[i].HID;
                    if (capacityHierarchy.HID.Equals(CH_HID))
                    {
                        CH_Count = 1;
                        break;
                    }
                    else
                    {
                        CH_Count = 0;
                    }
                }
                if (CH_Count == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        CapacityHierarchy C_H = (from s in entit.CapacityHierarchies
                                                 where s.HID == capacityHierarchy.HID
                                     select s).FirstOrDefault();
                        C_H.ManagerStatus = "Deleted";
                        C_H.InsertedDate = DateTime.Now;
                        C_H.InsertedBy = capacityHierarchy.InsertedBy;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public Boolean UpdatingAccess(UserReportsAccess userReportsAccess)
        {
            List<UserReportsAccess> list = new List<UserReportsAccess>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.UserReportsAccesses.OrderBy(a => a.AccessID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    UA_UID = list[i].UID;
                    //UA_TID = list[i].TicketID;
                    if (userReportsAccess.UID.Equals(UA_UID))
                    {
                        UACount = 1;
                        break;
                    }
                    else
                    {
                        UACount = 0;
                    }
                }
                if (UACount == 0)
                {
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        UserReportsAccess URA = (from s in entit.UserReportsAccesses
                                                     where s.UID == userReportsAccess.UID
                                                     select s).FirstOrDefault();
                        User user = (from s in entit.Users
                                                 where s.UID == userReportsAccess.UID
                                                 select s).FirstOrDefault();
                        string[] status = userReportsAccess.UserAccessStatus.Split(',');
                        URA.UpdatedBy = userReportsAccess.UpdatedBy;
                        URA.UpdatedOn = DateTime.Now;
                        URA.IMPS = userReportsAccess.IMPS;
                        URA.CTO = userReportsAccess.CTO;
                        URA.AutomatedCLR = userReportsAccess.AutomatedCLR;
                        URA.StageGate = userReportsAccess.StageGate;
                        URA.LessonsLearnt = userReportsAccess.LessonsLearnt;
                        URA.MarketReport = userReportsAccess.MarketReport;
                        URA.CycleTime = userReportsAccess.CycleTime;
                        URA.ELTReport = userReportsAccess.ELTReport;
                        URA.CapacityTracker = userReportsAccess.CapacityTracker;
                        URA.ResourceUtilization = userReportsAccess.ResourceUtilization;
                        URA.C_Hierarchy = userReportsAccess.C_Hierarchy;
                        URA.C_HierarchyEdits = userReportsAccess.C_HierarchyEdits;
                        URA.UserAccessStatus = status[0];
                        URA.MarketCommentsEdit = userReportsAccess.MarketCommentsEdit;
                        URA.CLREdits = userReportsAccess.CLREdits;
                        URA.NPS = userReportsAccess.NPS;
                        URA.NPSAdmin = userReportsAccess.NPSAdmin;
                        URA.NPSClientInfo = userReportsAccess.NPSClientInfo;
                        URA.NPSEdit = userReportsAccess.NPSEdit;
                        URA.DigitalReport = userReportsAccess.DigitalReport;
                        URA.PerformanceAnalysis = userReportsAccess.PerformanceAnalysis;
                        
                        user.UserStatus = status[0];
                        user.AccountStatus = status[1];
                        user.JobType = status[2];
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
    }
}