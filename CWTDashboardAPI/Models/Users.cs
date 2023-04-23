using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class Users
    {
        String UID, UID_U, ReportName, TypeofUse, Email, Login_UID, Login_Email, Login_Password;
        String UID_C, Email_C, Login_UID_C, Login_Email_C, Login_Password_C;
        int count, Login_count;
        public int AddingUserdata(User users)
        {
            List<User> list = new List<User>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Users.OrderBy(a => a.UserID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    UID = list[i].UID;
                    Email = list[i].Email;
                    if (users.UID.Equals(UID))
                    {
                        count = 1;
                        break;
                    }else if (users.Email.Equals(Email))
                    {
                        count = 2;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                return count;
            }
        }
        public int UserLogin(User users)
        {
            List<User> list = new List<User>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                var LoginUID = entity.Users.FirstOrDefault(x => x.UID == users.UID);
                if(LoginUID != null)
                {
                    Login_UID = LoginUID.UID;
                }
                else
                {
                    Login_UID = null;
                }
                var LoginEmail = entity.Users.FirstOrDefault(x => x.Email == users.Email);
                if(LoginEmail != null)
                {
                    Login_Email = LoginEmail.Email;
                }
                else
                {
                    Login_Email = null;
                }
                if(users.UID == "" || users.UID == null)
                {
                    if(users.Email.Equals(Login_Email))
                    {
                        var LoginPassword = entity.Users.FirstOrDefault(x => x.Email == users.Email);
                        if (LoginPassword != null)
                        {
                            Login_Password = LoginPassword.Password;
                        }
                        else
                        {
                            Login_Password = null;
                        }
                        if (users.Password.Equals(Login_Password))
                        {
                            count = 0;
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    else
                    {
                        count = 2;
                    }
                }
                else
                {
                    if (users.UID.Equals(Login_UID))
                    {
                        var LoginPassword = entity.Users.FirstOrDefault(x => x.UID == users.UID);
                        if(LoginPassword != null)
                        {
                            Login_Password = LoginPassword.Password;
                        }
                        else
                        {
                            Login_Password = null;
                        }
                        if (users.Password.Equals(Login_Password))
                        {
                            count = 0;
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    else
                    {
                        count = 3;
                    }
                }
                return count;
            }
        }
        string UIDCheck, EmailCheck;
        public int UIDEmailCheckForResetPassword(User users)
        {
            List<User> list = new List<User>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                var LoginUID = entity.Users.FirstOrDefault(x => x.UID == users.UID);
                if (LoginUID != null)
                {
                    Login_UID = LoginUID.UID;
                }
                else
                {
                    Login_UID = null;
                }
                var LoginEmail = entity.Users.FirstOrDefault(x => x.Email == users.Email);
                if (LoginEmail != null)
                {
                    Login_Email = LoginEmail.Email;
                }
                else
                {
                    Login_Email = null;
                }
                if (users.UID == "" || users.UID == null)
                {
                    if (users.Email.Equals(Login_Email))
                    {
                        var LoginPassword = entity.Users.FirstOrDefault(x => x.Email == users.Email);
                        if (LoginPassword != null)
                        {
                            Login_Password = LoginPassword.Password;
                        }
                        else
                        {
                            Login_Password = null;
                        }
                        if (users.Password.Equals(Login_Password))
                        {
                            count = 0;
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    else
                    {
                        count = 2;
                    }
                }
                else
                {
                    if (users.UID.Equals(Login_UID))
                    {
                        var LoginPassword = entity.Users.FirstOrDefault(x => x.UID == users.UID);
                        if (LoginPassword != null)
                        {
                            Login_Password = LoginPassword.Password;
                        }
                        else
                        {
                            Login_Password = null;
                        }
                        if (users.Password.Equals(Login_Password))
                        {
                            count = 0;
                        }
                        else
                        {
                            count = 1;
                        }
                    }
                    else
                    {
                        count = 3;
                    }
                }
                return count;
            }
        }
        public int UserUpdate(User users)
        {
            List<User> list = new List<User>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Users.OrderBy(a => a.UserID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    UID_C = list[i].UID;
                    if (users.UID.Equals(UID_C))
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
                    return 0;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        User CVP = (from a in entit.Users
                                       where a.UID == users.UID
                                       select a).FirstOrDefault();
                        CVP.FirstName = users.FirstName;
                        CVP.LastName = users.LastName;
                        CVP.Email = users.Email;
                        CVP.Password = users.Password;
                        CVP.UpdatedBy = users.UID;
                        DateTime Today = DateTime.Now;
                        CVP.UpdatedOn = Today;
                        CVP.Manager = users.Manager;
                        CVP.JobType = users.JobType;
                        entit.SaveChanges();
                    }
                    return 1;
                }
            }
        }
        public int UsersUsageofReports(UsersUsageofReport usersUsageofReport)
        {
            List<UsersUsageofReport> list = new List<UsersUsageofReport>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.UsersUsageofReports.OrderBy(a => a.ID).ToList();
                int c = list.Count;
                count = 0;
                for (int i = 0; i < c; i++)
                {
                    UID_U = list[i].UID;
                    ReportName = list[i].ReportName;
                    TypeofUse = list[i].TypeofUse;
                    if (usersUsageofReport.UID.Equals(UID_U) && usersUsageofReport.ReportName.Equals(ReportName) && usersUsageofReport.TypeofUse.Equals(TypeofUse))
                    {
                        count = 1;
                        break;
                    }
                    else
                    {
                        count = 0;
                    }
                }
                if (count == 1)
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        UsersUsageofReport UUR = (from a in entit.UsersUsageofReports
                                    where a.UID == usersUsageofReport.UID
                                    where a.ReportName == usersUsageofReport.ReportName
                                    where a.TypeofUse == usersUsageofReport.TypeofUse
                                    select a).FirstOrDefault();
                        DateTime TodaysDate = DateTime.Now;
                        UUR.LastUsedOn = TodaysDate;
                        var NoOfAttempts = entit.UsersUsageofReports.FirstOrDefault(x => x.UID == usersUsageofReport.UID && x.ReportName == usersUsageofReport.ReportName && x.TypeofUse == usersUsageofReport.TypeofUse).NoOfAttempts + 1;
                        UUR.NoOfAttempts = NoOfAttempts;
                        entit.SaveChanges();
                    }
                    return 1;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        DateTime TodaysDate = DateTime.Now;
                        usersUsageofReport.NoOfAttempts = 1;
                        usersUsageofReport.LastUsedOn = TodaysDate;
                        entit.UsersUsageofReports.Add(usersUsageofReport);
                        entit.SaveChanges();
                    }
                    return 0;
                }
            }
        }
    }
}