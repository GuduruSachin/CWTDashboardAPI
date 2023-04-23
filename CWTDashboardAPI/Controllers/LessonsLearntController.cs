using CWTDashboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CWTDashboardAPI.Controllers
{
    public class LessonsLearntController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        LessonsLearntFilter ll = new LessonsLearntFilter();
        [HttpPost]
        [Route("LessonsLearntFiltersList")]
        public LessonsLearntFilter LessonsLearntFiltersList(LessonLearnt lessonLearnt)
        {
            var Regions = (from a in entity.LessonLearnts
                              where a.Region != null  && a.Region != ""
                              select new
                              {
                                  Region = a.Region,
                                  isSelected = true,
                              }).Distinct().OrderBy(x => x.Region);
            
            var Status = (from a in entity.LessonLearnts
                            select new
                            {
                                Status = a.Status__By_Leader_,
                                isSelected = true,
                            }).Distinct().OrderBy(x => x.Status);
            ll.code = 200;
            ll.message = "Data Successfull";
            ll.Region = Regions;
            ll.Status = Status;
            return ll;
        }

        string[] LL_status, LL_Region;
        [HttpPost]
        [Route("LessonsLearnt")]
        public Response LessonsLearnt(LessonLearnt lessonLearnt)
        {
            if (lessonLearnt.Region == null || lessonLearnt.Status__By_Leader_ == null)
            {
                re.code = 100;
                re.message = "Please select the Details";
                re.Data = null;
            }
            else
            {
                LL_status = lessonLearnt.Status__By_Leader_.Split(',');
                for (int i = 0; i < LL_status.Count(); i++)
                {
                    if (LL_status[i] == "" || LL_status[i] == "null")
                    {
                        LL_status[i] = null;
                    }
                }
                LL_Region = lessonLearnt.Region.Split(',');
                for (int i = 0; i < LL_Region.Count(); i++)
                {
                    if (LL_Region[i] == "" || LL_Region[i] == "null")
                    {
                        LL_Region[i] = null;
                    }
                }
                var Lessonslearnt = (from a in entity.LessonLearnts
                                        where LL_status.Any(val => a.Status__By_Leader_.Equals(val))
                                        where LL_Region.Any(val => a.Region.Equals(val))
                                        select new
                                        {
                                            a.iMeet_Workspace_Title,
                                            a.Record_ID,
                                            a.Date_feedback_raised,
                                            a.Country_Area_of_Responsibility,
                                            a.Region,
                                            a.What_was_the_event_issue_concern,
                                            a.Is_there_any_specific_recognition_to_a_person_group_process_rela,
                                            a.Go_Live_Date,
                                            a.Reason_Type,
                                            a.Created_by_Field,
                                            a.Leader,
                                            a.Reason_Code__Added_by_Leader_,
                                            a.What_do_you_recommend___to_avoid_this_occurring_again_in_future,
                                            a.Status__By_Leader_,
                                            a.Action_Taken__By_Leader_,
                                        }).ToList();
                re.code = 200;
                re.message = "Success";
                re.Data = Lessonslearnt;
            }
            return re;
        }

        [HttpPost]
        [Route("LessonsLearntExcel")]
        public Response LessonsLearntExcel(LessonLearnt lessonLearnt)
        {
            var Lessonslearnt = (from a in entity.LessonLearnts
                                    select new
                                    {
                                        a.iMeet_Workspace_Title,
                                        a.Record_ID,
                                        a.Date_feedback_raised,
                                        a.Country_Area_of_Responsibility,
                                        a.Region,
                                        a.What_was_the_event_issue_concern,
                                        a.Is_there_any_specific_recognition_to_a_person_group_process_rela,
                                        a.Go_Live_Date,
                                        a.Reason_Type,
                                        a.Created_by_Field,
                                        a.Leader,
                                        a.Reason_Code__Added_by_Leader_,
                                        a.What_do_you_recommend___to_avoid_this_occurring_again_in_future,
                                        a.Status__By_Leader_,
                                        a.Action_Taken__By_Leader_,
                                    }).ToList();
            re.code = 200;
            re.message = "Success";
            re.Data = Lessonslearnt;
            return re;
        }
    }
}