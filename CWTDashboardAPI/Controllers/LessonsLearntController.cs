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
        CWTEntities entity = new CWTEntities();
        Response re = new Response();

        string[] status_ByR, projectlvl_ByR, Region_ByR, Years_ByR, ProjectSum_R;
        [HttpPost]
        [Route("LessonsLearnt")]
        public Response LessonsLearnt(LessonsLearnt lessonslearnt)
        {
            var CapacityByRegion = (from a in entity.LessonsLearnts
                                    select new
                                    {
                                        a.Actions,
                                        a.Country,
                                        a.iMeet_Workspace_Title,
                                        a.Region,
                                        a.Raised_by,
                                        a.Owner,
                                        a.Status
                                    }).ToList();
            re.code = 200;
            re.message = "Success";
            re.Data = CapacityByRegion;
            return re;
        }
    }
}