using System;
using System.Collections.Generic;
using System.Linq;
using CWTDashboardAPI.Models;
using System.Web.Http;
using System.Net;
using System.Net.Http;

namespace CWTDashboardAPI.Controllers
{
    public class ErrorTrackerController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        ETFilters et = new ETFilters();
        [HttpPost]
        [Route("GetETDropDowns")]
        public ETFilters GetETDropDowns(ErrorTracker errorTracker)
        {
            var FilterCountry = (from a in entity.ETCountries
                                 select new
                                 {
                                     a.Country,
                                     a.CountryID,
                                     a.InsertedOn
                                 }).Distinct().OrderBy(x => x.Country);
            var FilterResource = (from a in entity.ETResources
                                  select new
                                  {
                                      a.Resource,
                                      a.ResourceID,
                                      a.InsertedOn
                                  }).Distinct().OrderBy(x => x.Resource);
            var FilterManager = (from a in entity.ETManagers
                                 select new
                                 {
                                     a.Manager,
                                     a.ManagerID,
                                     a.InsertedOn
                                 }).Distinct().OrderBy(x => x.Manager);
            et.code = 200;
            et.message = "Data Success";
            et.Country = FilterCountry;
            et.Resource = FilterResource;
            et.Manager = FilterManager;
            return et;
        }

        [HttpPost]
        [Route("AddErrorTracker")]
        public IHttpActionResult AddErrorTracker(ErrorTracker errorTracker)
        {
            try
            {
                entity.ErrorTrackers.Add(errorTracker);
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
        [Route("GetAllErrorTracker")]
        public Response GetAllErrorTracker(ErrorTracker errorTracker)
        {
            var ErrorTrackers = (from a in entity.ErrorTrackers
                                 select new
                                 {
                                     a.TrackerID,
                                     a.Region,
                                     a.Country,
                                     a.ResourceName,
                                     a.Manager,
                                     a.ProjectLevel,
                                     a.OBT,
                                     a.OBTUsed,
                                     a.TypeofError,
                                     a.FinancialImpact,
                                     a.FIDescription,
                                     a.ValueofLoss,
                                     a.ErrorDescription,
                                     a.ErrorReason,
                                     a.WhoReportedError,
                                     a.ErrorComplexityRating,
                                     a.IssueResolved,
                                     a.Comments,
                                     a.FMEA,
                                     a.InsertedOn,
                                     a.Status
                                 }).Distinct().OrderBy(x => x.TrackerID);
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