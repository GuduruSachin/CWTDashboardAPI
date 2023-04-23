using System;
using System.Collections.Generic;
using System.Linq;
using CWTDashboardAPI.Models;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace CWTDashboardAPI.Controllers
{
    public class OnlineProjectController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        ETFilters et = new ETFilters();
        [HttpPost]
        [Route("GetOPTDropDowns")]
        public ETFilters GetOPTDropDowns(OnlineProjectTracker onlineProjectTracker)
        {
            var FilterCountry = (from a in entity.OPTCountries
                                 select new
                                 {
                                     a.CountryName,
                                     a.CountryID,
                                     a.InsertedOn
                                 }).Distinct().OrderBy(x => x.CountryID);
            var FilterAssignedTo = (from a in entity.OPTAssignedToes
                                  select new
                                  {
                                      a.AssignedTo,
                                      a.AssignedToID,
                                      a.InsertedOn
                                  }).Distinct().OrderBy(x => x.AssignedToID);
            var FilterManager = (from a in entity.OPTManagers
                                 select new
                                 {
                                     a.Manager,
                                     a.ManagerID,
                                     a.InsertedOn
                                 }).Distinct().OrderBy(x => x.ManagerID);
            et.code = 200;
            et.message = "Data Success";
            et.Country = FilterCountry;
            et.Manager = FilterManager;
            et.AssignedTo = FilterAssignedTo;
            return et;
        }
        [HttpPost]
        [Route("AddOnlineProject")]
        public IHttpActionResult AddOnlineProject(OnlineProjectTracker onlineProjectTracker)
        {
            try
            {
                entity.OnlineProjectTrackers.Add(onlineProjectTracker);
                entity.SaveChanges();
                re.code = 200;
                re.message = "Data inserted sucuessfully";
            }
            catch (Exception e)
            {
                re.code = 100;
                re.message = Convert.ToString(e);
            }
            return Content(HttpStatusCode.OK, re);
        }
        int ErrorTrackersCount;
        [HttpPost]
        [Route("GetAllOnlineProject")]
        public Response GetAllOnlineProject(OnlineProjectTracker onlineProjectTracker)
        {
            var ErrorTrackers = (from a in entity.OnlineProjectTrackers
                                 select new
                                 {
                                     a.OPTID,
                                     a.Golive,
                                     a.ActualGoLive,
                                     a.ProjectAssignedDate,
                                     a.KickOffImplementation,
                                     a.ClientName,
                                     a.ProjectType,
                                     a.Business,
                                     a.Region,
                                     a.Country,
                                     a.NoOfCountries,
                                     a.OnlineBookingTool,
                                     a.OnlineBookingType,
                                     a.AssignedTo,
                                     a.Manager,
                                     a.ProjectStatus,
                                     a.Comments
                                 }).Distinct().OrderBy(x => x.OPTID);
            ErrorTrackersCount = ErrorTrackers.AsQueryable().Count();
            if (ErrorTrackersCount.ToString() == "" || ErrorTrackersCount.ToString() == null || ErrorTrackersCount == 0)
            {
                re.Data = ErrorTrackers;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                re.Data = ErrorTrackers;
                re.code = 200;
                re.message = "Data Successfull";
            }
            return re;
        }
    }
}