using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Net.Http;
using System.Web.Http;
using System.Globalization;
using CWTDashboardAPI.Models;
using System.Net.Mail;
using System.Net.Mime;

namespace CWTDashboardAPI.Controllers
{
    public class UserController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        LoginUser LU = new LoginUser();
        [HttpPost]
        [Route("UserRegistration")]
        public IHttpActionResult UserRegistration(User user)
        {
            int b = new Users().AddingUserdata(user);
            if (b == 0)
            {
                DateTime TodayDate = DateTime.Now;
                user.UserStatus = "Active";
                user.AccountStatus = "User";
                user.Photo = "";
                user.InsertedDate = TodayDate;
                user.Gender = "";
                user.MobileNo = 0;
                user.UpdatedBy = "";
                user.UpdatedOn = TodayDate;
                entity.Users.Add(user);
                entity.SaveChanges();
                entity.UserReportsAccesses.Add(
                new UserReportsAccess
                {
                    UserID = entity.Users.FirstOrDefault(x => x.UID == user.UID).UserID,
                    UID = user.UID,
                    IMPS = true,
                    CTO = false,
                    AutomatedCLR = true,
                    CLREdits = false,
                    MarketReport = true,
                    StageGate = false,
                    LessonsLearnt = true,
                    CapacityTracker = false,
                    CycleTime = false,
                    ELTReport = true,
                    C_Hierarchy = false,
                    MarketCommentsEdit = false,
                    C_HierarchyEdits = false,
                    NPS = false,
                    NPSAdmin = false,
                    NPSClientInfo = true,
                    NPSEdit = false,
                    PerformanceAnalysis = false,
                    DigitalReport = false,
                    InsertedOn = TodayDate,
                    UpdatedOn = TodayDate,
                    UpdatedBy = "",
                    UserAccessStatus = "Active",
                    ResourceUtilization = false,
                    Prospect = false
                });
                entity.SaveChanges();
                re.code = 200;
                re.message = "Registered Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else if (b == 1)
            {
                re.code = 100;
                re.message = "Uid Already Exists";
                return Content(HttpStatusCode.OK, re);
            }
            else if (b == 2)
            {
                re.code = 100;
                re.message = "Email Id already Exists";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Register";
                return Content(HttpStatusCode.OK, re);
            }
        }
        string UID, EmailId;
        [HttpPost]
        [Route("UserLogin")]
        public LoginUser UserLogin(User user)
        {
            int b = new Users().UserLogin(user);
            if (b == 0)
            {
                LU.code = 200;
                LU.message = "Login Succesfull";
                if (user.UID == "" || user.UID == null)
                {
                    UID = entity.Users.FirstOrDefault(x => x.Email == user.Email).UID;
                    EmailId = user.Email;
                }
                else
                {
                    UID = user.UID;
                    EmailId = entity.Users.FirstOrDefault(x => x.UID == user.UID).Email;
                }
                LU.FirstName = entity.Users.FirstOrDefault(x => x.UID == UID).FirstName;
                LU.LastName = entity.Users.FirstOrDefault(x => x.UID == UID).LastName;
                LU.UID = UID;
                LU.UserID = entity.Users.FirstOrDefault(x => x.UID == UID).UserID;
                LU.EmailId = EmailId;
                LU.Password = entity.Users.FirstOrDefault(x => x.UID == UID).Password;
                LU.UserStatus = entity.Users.FirstOrDefault(x => x.UID == UID).UserStatus;
                LU.Manager = entity.Users.FirstOrDefault(x => x.UID == UID).Manager;
                LU.InsertedDate = entity.Users.FirstOrDefault(x => x.UID == UID).InsertedDate;
                LU.AccountStatus = entity.Users.FirstOrDefault(x => x.UID == UID).AccountStatus;
                LU.UpdatedOn = entity.Users.FirstOrDefault(x => x.UID == UID).UpdatedOn;
                LU.UpdatedBy = entity.Users.FirstOrDefault(x => x.UID == UID).UpdatedBy;
                LU.AccessID = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).AccessID;
                LU.IMPS = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).IMPS;
                LU.CTO = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).CTO;
                LU.StageGate = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).StageGate;
                LU.LessonsLearnt = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).LessonsLearnt;
                LU.CapacityTracker = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).CapacityTracker;
                LU.AutomatedCLR = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).AutomatedCLR;
                LU.CycleTime = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).CycleTime;
                LU.EltReport = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).ELTReport;
                LU.C_Hierarchy = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).C_Hierarchy;
                LU.ResourceUtilization = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).ResourceUtilization;
                LU.Prospect = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).Prospect;
                LU.UserAccessStatus = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UID).UserAccessStatus;
                return LU;
            }
            else if (b == 1)
            {
                LU.code = 101;
                LU.message = "Incorrect Password";
                return LU;
            }
            else if (b == 2)
            {
                LU.code = 102;
                LU.message = "Email ID Not Registered";
                return LU;
            }
            else if (b == 3)
            {
                LU.code = 103;
                LU.message = "UID Not Registered";
                return LU;
            }
            else
            {
                LU.code = 100;
                LU.message = "Technical issue Please Try again Later";
                return LU;
            }
        }
        
        [HttpPost]
        [Route("SendPasswordToUser")]
        public Response SendPasswordToUser(User user)
        {
            var Check = (from a in entity.Users
                         where a.Email == user.Email
                         where a.UID == user.UID
                         select a).Distinct().ToList();
            if(Check.Count() == 0)
            {
                re.code = 100;
                re.message = "Email or UID are not matching";
            }
            else
            {
                string Body;
                MailMessage mail = new MailMessage();
                mail.From = new MailAddress("CWTDashboard@mycwt.com");
                mail.To.Add(new MailAddress(user.Email));
                mail.IsBodyHtml = true;
                mail.Subject = "CWT Dashboard Login : Recover Password";
                Body = "<html><body><span style=\"font-family: verdana; font-size: 12px; \">Hi,<br />Please find below details for logging into CWT Dashboard</span>";
                Body += "<span style=\"font-family: verdana; font-size: 12px;font-weight : bold; \"><br /><br />UID : </span>" + Check.FirstOrDefault().UID;
                Body += "<span style=\"font-family: verdana; font-size: 12px;font-weight : bold;text-decoration : none; \"><br />Email : </span>" + Check.FirstOrDefault().Email;
                Body += "<span style=\"font-family: verdana; font-size: 12px;font-weight : bold; \"><br />Password : </span>" + Check.FirstOrDefault().Password;
                Body += "<br /><br /><a href=\"http://10.180.27.32/CWT/Login\" style=\"font-family: verdana; font-size: 12px;font-weight : bold; color: '#0645AD'; text-decoration : underline \"> Login to CWTDashboard </a>";
                //Body += "<br /><br /><span style=\"font-family: verdana; font-size: 12px; \">Thanks & Regards</span>";
                Body += "<br /><span style=\"font-family: verdana; font-size: 11px; color: #FF0000; font-weight: bold; font-style:italic;\">***Please do not respond to this email as this is a computer generated email***</span></html></body>";

                AlternateView av1 = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);
                mail.AlternateViews.Add(av1);
                SmtpClient smtp = new SmtpClient();
                smtp.Host = "mta-hub";
                smtp.UseDefaultCredentials = false;
                //smtp.Credentials = new System.Net.NetworkCredential("U007UXG", "Sachin95@KSK");
                smtp.Port = 25;
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                try
                {
                    smtp.Send(mail);
                    re.code = 200;
                    re.message = "Please check your Mail id. we have sent you the Account Password.";
                }
                catch (Exception ex)
                {
                    //Error, could not send the message
                    re.code = 100;
                    re.message = ex.Message;
                }
            }
            return re;
        }

        [HttpPost]
        [Route("UserDetails")]
        public LoginUser UserDetails(User user)
        {
            var UserUID = user.UID;
            if (UserUID == null || UserUID == "")
            {
                LU.code = 100;
                LU.message = "Please Login again";
            }
            else if (UserUID == entity.Users.FirstOrDefault(x => x.UID == UserUID).UID)
            {
                LU.code = 200;
                LU.message = "Success";
                LU.FirstName = entity.Users.FirstOrDefault(x => x.UID == UserUID).FirstName;
                LU.LastName = entity.Users.FirstOrDefault(x => x.UID == UserUID).LastName;
                LU.UID = UserUID;
                LU.UserID = entity.Users.FirstOrDefault(x => x.UID == UserUID).UserID;
                LU.EmailId = entity.Users.FirstOrDefault(x => x.UID == UserUID).Email;
                LU.Password = entity.Users.FirstOrDefault(x => x.UID == UserUID).Password;
                LU.JobType = entity.Users.FirstOrDefault(x => x.UID == UserUID).JobType;
                LU.UserStatus = entity.Users.FirstOrDefault(x => x.UID == UserUID).UserStatus;
                LU.Manager = entity.Users.FirstOrDefault(x => x.UID == UserUID).Manager;
                LU.InsertedDate = entity.Users.FirstOrDefault(x => x.UID == UserUID).InsertedDate;
                LU.AccountStatus = entity.Users.FirstOrDefault(x => x.UID == UserUID).AccountStatus;
                LU.UpdatedOn = entity.Users.FirstOrDefault(x => x.UID == UserUID).UpdatedOn;
                LU.UpdatedBy = entity.Users.FirstOrDefault(x => x.UID == UserUID).UpdatedBy;
                LU.AccessID = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).AccessID;
                LU.IMPS = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).IMPS;
                LU.CTO = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).CTO;
                LU.StageGate = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).StageGate;
                LU.LessonsLearnt = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).LessonsLearnt;
                LU.CapacityTracker = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).CapacityTracker;
                LU.AutomatedCLR = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).AutomatedCLR;
                LU.CycleTime = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).CycleTime;
                LU.EltReport = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).ELTReport;
                LU.C_Hierarchy = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).C_Hierarchy;
                LU.ResourceUtilization = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).ResourceUtilization;
                LU.Prospect = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).Prospect;
                LU.UserAccessStatus = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).UserAccessStatus;
                LU.NPS = entity.UserReportsAccesses.FirstOrDefault(x => x.UID == UserUID).NPS ?? false;
            }
            else
            {
                LU.code = 104;
                LU.message = "UID Not Registered";
            }
            return LU;
        }

        [HttpPost]
        [Route("UpdateDetails")]
        public IHttpActionResult UpdateDetails(User user)
        {
            int b = new Users().UserUpdate(user);
            if (b == 1)
            {
                re.code = 200;
                re.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Please Try again Later!";
                return Content(HttpStatusCode.OK, re);
            }
        }

        [HttpPost]
        [Route("UsersUsageofReports")]
        public Response UsersUsageofReports(UsersUsageofReport usersUsageofReport)
        {
            int b = new Users().UsersUsageofReports(usersUsageofReport);
            if (b == 1)
            {
                re.code = 200;
                re.message = "Updated";
            } else if (b == 0)
            {
                re.code = 200;
                re.message = "Added";
            }
            else
            {
                re.code = 100;
                re.message = "Something went wrong";
            }
            return re;
        }
        //[HttpPost]
        //[Route("updatingpsdrecords")]
        //public Response updatingpsdrecords(ManualData manualData)
        //{
        //    var data = (from a in entity.PSDs
        //                select new {
        //                    a.Revenue_Id,
        //                    a.Product_Name,
        //                }).ToList();
        //    for(int i = 0;i<data.Count; i++)
        //    {
        //        double? RevID = data[i].Revenue_Id;
        //        var OBT = data[i].Product_Name;
        //        ManualData manual_data = (from b in entity.ManualDatas
        //                                  where b.Revenue_ID == RevID
        //                                  select b).FirstOrDefault();
        //        manual_data.OBT_Reseller___Direct = OBT;
        //        entity.SaveChanges();
        //    }
        //    return re;
        //}
        [HttpPost]
        [Route("SendingEmail")]
        public Response sendEmail()
        {
            string Body;
            MailMessage mail = new MailMessage();
            mail.From = new MailAddress("UGuduru@mycwt.com");
            mail.To.Add(new MailAddress("HGourani@mycwt.com"));

            mail.IsBodyHtml = true;
            mail.Subject = "Recover Password ";

            Body = "<html><body><span style=\"font-family: verdana; font-size: 12px; \">Hi,<br /><br />Please find below password for GSC Connect</span>";
            

            Body += "<br /><br /><span style=\"font-family: verdana; font-size: 12px; \">Select 'Use GSC Connect login credentials' on Login Page</span>";
            Body += "<br /><br /><span style=\"font-family: verdana; font-size: 12px; \">Thanks & Regards</span>";
            Body += "<br /><br /><span style=\"font-family: verdana; font-size: 11px; color: #FF0000; font-weight: bold; font-style:italic;\">***Please do not respond to this email as this is a computer generated email***</span></html></body>";

            AlternateView av1 = AlternateView.CreateAlternateViewFromString(Body, null, MediaTypeNames.Text.Html);
            mail.AlternateViews.Add(av1);
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "mta-hub";
            smtp.UseDefaultCredentials = false;
            //smtp.Credentials = new System.Net.NetworkCredential("U007UXG", "Sachin95@KSK");
            smtp.Port = 25;
            smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
            //smtp.Send(mail);

            //SmtpClient client = new SmtpClient();
            //client.Host = "mta-hub";
            //client.Port = 25;
            //client.EnableSsl = true;
            //client.Timeout = 1000000;
            //client.DeliveryMethod = SmtpDeliveryMethod.Network;
            //client.UseDefaultCredentials = false;
            //client.Credentials = new NetworkCredential("U007UXG", "Sachin95@KSK");
            //MailMessage message = new MailMessage();
            //message.To.Add(new MailAddress("HGourani@mycwt.com"));
            //message.From = new MailAddress("UGuduru@mycwt.com");
            //message.Subject = "Using the new SMTP client.";
            //message.Body = "Using this new feature, you can send an email message from an application very easily.";
            try
            {
                smtp.Send(mail);
                re.message = "200";
                re.message = "success";
            }
            catch (Exception ex)
            {
                //Error, could not send the message
                re.code = 100;
                re.message = ex.Message;
            }
            return re;
        }
    }
}