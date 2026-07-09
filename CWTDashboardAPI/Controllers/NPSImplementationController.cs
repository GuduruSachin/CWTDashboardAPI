
using CWTDashboardAPI.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CWTDashboardAPI.Controllers
{
    public class NPSImplementationController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        NPSData nps = new NPSData();
        Filters fi = new Filters();
        IMRCResponse imrs = new IMRCResponse();
        [HttpPost]
        [Route("NPSData")]
        public NPSData NPSData(NpsImp npsImp)
        {
            int CurrentYear;
            if (npsImp.YearMonth == null || npsImp.YearMonth == "")
            {
                CurrentYear = DateTime.Now.Year;
            }
            else
            {
                CurrentYear = int.Parse(npsImp.YearMonth);
            }
            var ClientTypes = "New,Existing".Split(',');
            var npsdata = (from a in entity.NpsImps
                           select new
                           {
                               NewBusinessSurveySent = entity.NpsImps.Where(x => x.ClientType == "New" && x.DateServeySent.Value.Year == CurrentYear && x.RecordStatus != "Deleted").Count(),
                               NESurveySent = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && x.DateServeySent.Value.Year == CurrentYear && x.RecordStatus != "Deleted").Count(),
                               NewBusinessSurveyReceived = entity.NpsImps.Where(x => x.ClientType == "New" && x.DateSurveyReceived.Value.Year == CurrentYear && x.RecordStatus != "Deleted").Count(),
                               NESurveyReceived = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && x.DateSurveyReceived.Value.Year == CurrentYear && x.RecordStatus != "Deleted").Count(),
                               ExistingSurveySent = entity.NpsImps.Where(x => x.ClientType == "Existing" && x.DateServeySent.Value.Year == CurrentYear && x.RecordStatus != "Deleted").Count(),
                               ExistingSurveyReceived = entity.NpsImps.Where(x => x.ClientType == "Existing" && x.DateSurveyReceived.Value.Year == CurrentYear && x.RecordStatus != "Deleted").Count(),
                               NewBusinessPromoter = entity.NpsImps.Where(x => x.ClientType == "New" && x.DateSurveyReceived.Value.Year == CurrentYear && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                               NewBusinessPassive = entity.NpsImps.Where(x => x.ClientType == "New" && x.DateSurveyReceived.Value.Year == CurrentYear && x.NPSIndicator == "Passive" && x.RecordStatus != "Deleted").Count(),
                               NewBusinessDetractor = entity.NpsImps.Where(x => x.ClientType == "New" && x.DateSurveyReceived.Value.Year == CurrentYear && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count(),
                               ExistingPromoter = entity.NpsImps.Where(x => x.ClientType == "Existing" && x.DateSurveyReceived.Value.Year == CurrentYear && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                               ExistingPassive = entity.NpsImps.Where(x => x.ClientType == "Existing" && x.DateSurveyReceived.Value.Year == CurrentYear && x.NPSIndicator == "Passive" && x.RecordStatus != "Deleted").Count(),
                               ExistingDetractor = entity.NpsImps.Where(x => x.ClientType == "Existing" && x.DateSurveyReceived.Value.Year == CurrentYear && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count(),
                               NEPromoter = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && x.DateSurveyReceived.Value.Year == CurrentYear && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                               NEPassive = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && x.DateSurveyReceived.Value.Year == CurrentYear && x.NPSIndicator == "Passive" && x.RecordStatus != "Deleted").Count(),
                               NEDetractor = entity.NpsImps.Where(x => ClientTypes.Any(val => x.ClientType.Equals(val)) && x.DateSurveyReceived.Value.Year == CurrentYear && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count(),
                           }).ToList().Distinct();
            var NewRegionWiseNPSScore = (from a in entity.NpsImps
                                         where a.DateSurveyReceived.Value.Year == CurrentYear
                                         where a.ClientType == "New"
                                         where a.RecordStatus != "Deleted"
                                         group a by a.Region into g
                                         select new
                                         {
                                             Region = g.Key,
                                             PromoterCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                                             DetractorCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count(),
                                             NPSScore = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.RecordStatus != "Deleted").Count(),
                                             Score = ((entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count() - entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count()) * 100) / entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.RecordStatus != "Deleted").Count()
                                         }).ToList();
            var ExistingRegionWiseNPSScore = (from a in entity.NpsImps
                                              where a.DateSurveyReceived.Value.Year == CurrentYear
                                              where a.ClientType == "Existing"
                                              where a.RecordStatus != "Deleted"
                                              group a by a.Region into g
                                              select new
                                              {
                                                  Region = g.Key,
                                                  PromoterCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                                                  DetractorCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count(),
                                                  NPSScore = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.RecordStatus != "Deleted").Count(),
                                                  Score = ((entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count() - entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count()) * 100) / entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.RecordStatus != "Deleted").Count()
                                              }).ToList();
            var NERegionWiseNPSScore = (from a in entity.NpsImps
                                        where a.DateSurveyReceived.Value.Year == CurrentYear
                                        where ClientTypes.Any(val => a.ClientType.Equals(val))
                                        where a.RecordStatus != "Deleted"
                                        group a by a.Region into g
                                        select new
                                        {
                                            Region = g.Key,
                                            PromoterCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                                            DetractorCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count(),
                                            NPSScore = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.RecordStatus != "Deleted").Count(),
                                            Score = ((entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count() - entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count()) * 100) / entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.RecordStatus != "Deleted").Count()
                                        }).ToList();
            var NewRegionWiseNPSCount = (from a in entity.NpsImps
                                         where a.DateSurveyReceived.Value.Year == CurrentYear
                                         where a.ClientType == "New"
                                         where a.RecordStatus != "Deleted"
                                         group a by a.Region into g
                                         select new
                                         {
                                             Region = g.Key,
                                             PromoterCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                                             PassiveCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.NPSIndicator == "Passive" && x.RecordStatus != "Deleted").Count(),
                                             DetractorCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count()
                                         }).ToList();
            var ExistingRegionWiseNPSCount = (from a in entity.NpsImps
                                              where a.DateSurveyReceived.Value.Year == CurrentYear
                                              where a.ClientType == "Existing"
                                              where a.RecordStatus != "Deleted"
                                              group a by a.Region into g
                                              select new
                                              {
                                                  Region = g.Key,
                                                  PromoterCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                                                  PassiveCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.NPSIndicator == "Passive" && x.RecordStatus != "Deleted").Count(),
                                                  DetractorCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count()
                                              }).ToList();
            var NERegionWiseNPSCount = (from a in entity.NpsImps
                                        where a.DateSurveyReceived.Value.Year == CurrentYear
                                        where ClientTypes.Any(val => a.ClientType.Equals(val))
                                        where a.RecordStatus != "Deleted"
                                        group a by a.Region into g
                                        select new
                                        {
                                            Region = g.Key,
                                            PromoterCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.NPSIndicator == "Promoter" && x.RecordStatus != "Deleted").Count(),
                                            PassiveCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.NPSIndicator == "Passive" && x.RecordStatus != "Deleted").Count(),
                                            DetractorCount = entity.NpsImps.Where(x => x.Region == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.NPSIndicator == "Detractor" && x.RecordStatus != "Deleted").Count()
                                        }).ToList();
            var NewSemanticAnalysisOne = (from a in entity.NpsImps
                                          where a.DateSurveyReceived.Value.Year == CurrentYear
                                          where a.ClientType == "New"
                                          where a.NPSCommentsOne != null
                                          where a.RecordStatus != "Deleted"
                                          where a.NPSCommentsOne != ""
                                          where a.NPSCommentsOne != "null"
                                          group a by a.NPSCommentsOne into g
                                          select new
                                          {
                                              Comment = g.Key,
                                              Count = entity.NpsImps.Where(x => x.NPSCommentsOne == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.RecordStatus != "Deleted").Count()
                                          }).ToList().OrderByDescending(x => x.Count);
            var NewSemanticAnalysisTwo = (from a in entity.NpsImps
                                          where a.DateSurveyReceived.Value.Year == CurrentYear
                                          where a.ClientType == "New"
                                          where a.NPSCommentsTwo != null
                                          where a.RecordStatus != "Deleted"
                                          where a.NPSCommentsTwo != ""
                                          where a.NPSCommentsTwo != "null"
                                          group a by a.NPSCommentsTwo into g
                                          select new
                                          {
                                              Comment = g.Key,
                                              Count = entity.NpsImps.Where(x => x.NPSCommentsTwo == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.RecordStatus != "Deleted").Count()
                                          }).ToList().OrderByDescending(x => x.Count);
            var NewSemanticAnalysisThree = (from a in entity.NpsImps
                                            where a.DateSurveyReceived.Value.Year == CurrentYear
                                            where a.ClientType == "New"
                                            where a.NPSCommentsThree != null
                                            where a.NPSCommentsThree != ""
                                            where a.RecordStatus != "Deleted"
                                            where a.NPSCommentsThree != "null"
                                            group a by a.NPSCommentsThree into g
                                            select new
                                            {
                                                Comment = g.Key,
                                                Count = entity.NpsImps.Where(x => x.NPSCommentsThree == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "New" && x.RecordStatus != "Deleted").Count()
                                            }).ToList().OrderByDescending(x => x.Count);
            var ExistingSemanticAnalysisOne = (from a in entity.NpsImps
                                               where a.DateSurveyReceived.Value.Year == CurrentYear
                                               where a.ClientType == "Existing"
                                               where a.NPSCommentsOne != null
                                               where a.NPSCommentsOne != ""
                                               where a.RecordStatus != "Deleted"
                                               where a.NPSCommentsOne != "null"
                                               group a by a.NPSCommentsOne into g
                                               select new
                                               {
                                                   Comment = g.Key,
                                                   Count = entity.NpsImps.Where(x => x.NPSCommentsOne == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.RecordStatus != "Deleted").Count()
                                               }).ToList().OrderByDescending(x => x.Count);
            var ExistingSemanticAnalysisTwo = (from a in entity.NpsImps
                                               where a.DateSurveyReceived.Value.Year == CurrentYear
                                               where a.ClientType == "Existing"
                                               where a.NPSCommentsTwo != null
                                               where a.NPSCommentsTwo != ""
                                               where a.RecordStatus != "Deleted"
                                               where a.NPSCommentsTwo != "null"
                                               group a by a.NPSCommentsTwo into g
                                               select new
                                               {
                                                   Comment = g.Key,
                                                   Count = entity.NpsImps.Where(x => x.NPSCommentsTwo == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.RecordStatus != "Deleted").Count()
                                               }).ToList().OrderByDescending(x => x.Count);
            var ExistingSemanticAnalysisThree = (from a in entity.NpsImps
                                                 where a.DateSurveyReceived.Value.Year == CurrentYear
                                                 where a.ClientType == "Existing"
                                                 where a.NPSCommentsThree != null
                                                 where a.NPSCommentsThree != ""
                                                 where a.RecordStatus != "Deleted"
                                                 where a.NPSCommentsThree != "null"
                                                 group a by a.NPSCommentsThree into g
                                                 select new
                                                 {
                                                     Comment = g.Key,
                                                     Count = entity.NpsImps.Where(x => x.NPSCommentsThree == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && x.ClientType == "Existing" && x.RecordStatus != "Deleted").Count()
                                                 }).ToList().OrderByDescending(x => x.Count);
            var NESemanticAnalysisOne = (from a in entity.NpsImps
                                         where a.DateSurveyReceived.Value.Year == CurrentYear
                                         where a.NPSCommentsOne != null
                                         where a.NPSCommentsOne != ""
                                         where a.RecordStatus != "Deleted"
                                         where a.NPSCommentsOne != "null"
                                         where ClientTypes.Any(val => a.ClientType.Equals(val))
                                         group a by a.NPSCommentsOne into g
                                         select new
                                         {
                                             Comment = g.Key,
                                             Count = entity.NpsImps.Where(x => x.NPSCommentsOne == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.RecordStatus != "Deleted").Count()
                                         }).ToList().OrderByDescending(x => x.Count);
            var NESemanticAnalysisTwo = (from a in entity.NpsImps
                                         where a.DateSurveyReceived.Value.Year == CurrentYear
                                         where ClientTypes.Any(val => a.ClientType.Equals(val))
                                         where a.NPSCommentsTwo != null
                                         where a.NPSCommentsTwo != ""
                                         where a.RecordStatus != "Deleted"
                                         where a.NPSCommentsTwo != "null"
                                         group a by a.NPSCommentsTwo into g
                                         select new
                                         {
                                             Comment = g.Key,
                                             Count = entity.NpsImps.Where(x => x.NPSCommentsTwo == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.RecordStatus != "Deleted").Count()
                                         }).ToList().OrderByDescending(x => x.Count);
            var NESemanticAnalysisThree = (from a in entity.NpsImps
                                           where a.DateSurveyReceived.Value.Year == CurrentYear
                                           where ClientTypes.Any(val => a.ClientType.Equals(val))
                                           where a.NPSCommentsThree != null
                                           where a.NPSCommentsThree != ""
                                           where a.RecordStatus != "Deleted"
                                           where a.NPSCommentsThree != "null"
                                           group a by a.NPSCommentsThree into g
                                           select new
                                           {
                                               Comment = g.Key,
                                               Count = entity.NpsImps.Where(x => x.NPSCommentsThree == g.Key && x.DateSurveyReceived.Value.Year == CurrentYear && ClientTypes.Any(val => x.ClientType.Equals(val)) && x.RecordStatus != "Deleted").Count()
                                           }).ToList().OrderByDescending(x => x.Count);
            nps.code = 200;
            nps.message = "Success";
            nps.NPSvalues = npsdata;
            nps.NewRegionWiseNPSScore = NewRegionWiseNPSScore;
            nps.ExistingRegionWiseNPSScore = ExistingRegionWiseNPSScore;
            nps.NewRegionWiseNPSCount = NewRegionWiseNPSCount;
            nps.ExistingRegionWiseNPSCount = ExistingRegionWiseNPSCount;
            nps.NewSemanticAnalysisOne = NewSemanticAnalysisOne;
            nps.NewSemanticAnalysisTwo = NewSemanticAnalysisTwo;
            nps.NewSemanticAnalysisThree = NewSemanticAnalysisThree;
            nps.ExistingSemanticAnalysisOne = ExistingSemanticAnalysisOne;
            nps.ExistingSemanticAnalysisTwo = ExistingSemanticAnalysisTwo;
            nps.ExistingSemanticAnalysisThree = ExistingSemanticAnalysisThree;
            nps.NERegionWiseNPSScore = NERegionWiseNPSScore;
            nps.NERegionWiseNPSCount = NERegionWiseNPSCount;
            nps.NESemanticAnalysisOne = NESemanticAnalysisOne;
            nps.NESemanticAnalysisTwo = NESemanticAnalysisTwo;
            nps.NESemanticAnalysisThree = NESemanticAnalysisThree;
            return nps;
        }

        [HttpPost]
        [Route("NpsInsert")]
        public IHttpActionResult NpsInsert(NpsImp npsImp)
        {
            //Boolean b = new NPSModel().AddingNPS(npsImp);
            //if (b)
            //{
            npsImp.InsertedOn = DateTime.Now;
            entity.NpsImps.Add(npsImp);
            entity.SaveChanges();
            re.code = 200;
            re.message = "Added Succesfully";
            return Content(HttpStatusCode.OK, re);
            //}
            //else
            //{
            //    re.code = 100;
            //    re.message = "Failed to add New";
            //    return Content(HttpStatusCode.OK, re);
            //}
        }
        [HttpPost]
        [Route("NPSUpdate")]
        public IHttpActionResult NPSUpdate(NpsImp npsImp)
        {
            Boolean b = new NPSModel().UpdateNPSClientInfo(npsImp);
            if (b)
            {
                re.code = 200;
                re.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Update data";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("NPSViewUpdate")]
        public IHttpActionResult NPSViewUpdate(NpsImp npsImp)
        {
            Boolean b = new NPSModel().UpdateNPSView(npsImp);
            if (b)
            {
                re.code = 200;
                re.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Update data";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("NPSDelete")]
        public IHttpActionResult NPSDelete(NpsImp npsImp)
        {
            Boolean b = new NPSModel().DeleteNPS(npsImp);
            if (b)
            {
                re.code = 200;
                re.message = "Deleted Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Delete data";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("NpsMaxRecipientId")]
        public Response NpsMaxRecipientId(NpsImp npsImp)
        {
            var maxid = entity.SurveySentDetails.OrderByDescending(u => u.RecipientId).FirstOrDefault().RecipientId;
            re.code = maxid;
            return re;
        }
        [HttpPost]
        [Route("NpsViewData")]
        public NPSData NpsViewData(NpsImp npsImp)
        {
            var data = (from a in entity.NpsImps
                        where a.RecordStatus != "Deleted"
                        select new
                        {
                            a.NpsId,
                            a.ClientName,
                            a.Company,
                            a.CustomerContactNumber,
                            a.Country,
                            a.GoLiveDate,
                            a.GlobalProjectManager,
                            a.RegionalProjectManager,
                            a.LocalProjectManager,
                            a.ClientType,
                            a.Email,
                            a.Region,
                            a.Language,
                            a.SingleResource,
                            a.RecordStatus,
                            a.DateServeySent,
                            a.DateSurveyReceived,
                            a.ClientScope,
                            a.OpprtunityId,
                            a.DQS,
                            a.DSD,
                            a.OnlineTeam,
                            a.Status,
                            a.AssignLeaderForClosedLoop,
                            a.NPSScore,
                            a.NPSIndicator,
                            a.NPSCommentsWhatwasPositive,
                            a.NPSComments_Howcouldwehaveimproved,
                            a.NPSComments_Whatistheonethingwecandotomakeyouhappier,
                            a.NPSCommentsOne,
                            a.NPSCommentsTwo,
                            a.NPSCommentsThree,
                            a.ReasonType,
                            a.ClientFeedback,
                            a.Action,
                            InsertedBy = a.InsertedBy == null || a.InsertedBy == "" ? "" : entity.Users.FirstOrDefault(x => x.UID == a.InsertedBy).FirstName + " " + entity.Users.FirstOrDefault(x => x.UID == a.InsertedBy).LastName ?? "",
                            a.InsertedOn,
                            RecipientId = entity.SurveySentDetails.Where(x => x.NpsId == a.NpsId).Count() > 0 ? entity.SurveySentDetails.FirstOrDefault(x => x.NpsId == a.NpsId).RecipientId+"" : "---",
                            UpdatedBy = a.UpdatedBy == null || a.UpdatedBy == "" ? "" : entity.Users.FirstOrDefault(x => x.UID == a.UpdatedBy).FirstName + " " + entity.Users.FirstOrDefault(x => x.UID == a.UpdatedBy).LastName ?? "",
                            a.GlobalDigitalPM,
                            a.RegionalDigitalPM,
                            a.LocalDigitalPM,
                            a.UpdatedOn,
                        }).ToList().OrderByDescending(x => x.InsertedOn);
            nps.NPSViewData = data;
            nps.code = 200;
            nps.message = "Data Succesful";
            return nps;
        }
        string[] YTDMonths,RollingNPSMonths;
        string ytdmonth,rollingmonths;
        [HttpPost]
        [Route("RollingNPS")]
        public NPSData RollingNPS(NpsImp npsImp)
        {
            string Month = DateTime.Now.ToString("MM");
            int year = DateTime.Now.Year;
            int Current_Month = DateTime.Now.Month;
            for (int i = 1; i <= Current_Month; i++)
            {
                if (i == Current_Month)
                {
                    if (i < 10)
                    {
                        ytdmonth += DateTime.Now.Year + "-0" + i;
                    }
                    else
                    {
                        ytdmonth += DateTime.Now.Year + "-" + i;
                    }
                }
                else
                {
                    if (i < 10)
                    {
                        ytdmonth += DateTime.Now.Year + "-0" + i + ",";
                    }
                    else
                    {
                        ytdmonth += DateTime.Now.Year + "-" + i + ",";
                    }
                }
            }
            for(int i = 1; i <= 12; i++)
            {
                if(i > Current_Month)
                {
                    if (i == 12)
                    {
                        rollingmonths += DateTime.Now.AddYears(-1).Year + "-" + i;
                    }
                    else if(i < 10)
                    {
                        rollingmonths += DateTime.Now.AddYears(-1).Year + "-0" + i + ",";
                    }
                    else
                    {
                        rollingmonths += DateTime.Now.AddYears(-1).Year + "-" + i + ",";
                    }
                }
                else
                {
                    if(i < 10)
                    {
                        rollingmonths += DateTime.Now.Year + "-0" + i + ",";
                    }
                    else
                    {
                        rollingmonths += DateTime.Now.Year + "-" + i + ",";
                    }
                }
            }
            YTDMonths = ytdmonth.Split(',');
            RollingNPSMonths = rollingmonths.Split(',');
            string YearMonth = year + "-" + Month;
            var CurrentMonthData = (from a in entity.NpsImps
                                    where a.YearMonth == YearMonth
                                    where a.RecordStatus != "Deleted"
                                    where a.NPSIndicator != null
                                    where a.NPSIndicator != "null"
                                    where a.NPSIndicator != ""
                                    group a by a.NPSIndicator into g
                                    select new
                                    {
                                        NPSIndicator = g.Key,
                                        APACCount = entity.NpsImps.Where(x => x.Region == "APAC" && x.YearMonth == YearMonth && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        EMEACount = entity.NpsImps.Where(x => x.Region == "EMEA" && x.YearMonth == YearMonth && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        LATAMCount = entity.NpsImps.Where(x => x.Region == "LATAM" && x.YearMonth == YearMonth && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        NORAMCount = entity.NpsImps.Where(x => x.Region == "NORAM" && x.YearMonth == YearMonth && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        GlobalCount = entity.NpsImps.Where(x => x.Region == "Global" && x.YearMonth == YearMonth && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        Total = entity.NpsImps.Where(x => x.Region != null && x.YearMonth == YearMonth && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                    }).ToList().OrderBy(x => x.NPSIndicator);
            var YTDData = (from a in entity.NpsImps
                           where a.RecordStatus != "Deleted"
                           where YTDMonths.Any(val => a.YearMonth.Equals(val))
                           where a.NPSIndicator != null
                           where a.NPSIndicator != "null"
                           where a.NPSIndicator != ""
                           group a by a.NPSIndicator into g
                           select new
                           {
                               NPSIndicator = g.Key,
                               APACCount = entity.NpsImps.Where(x => x.Region == "APAC" && YTDMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                               EMEACount = entity.NpsImps.Where(x => x.Region == "EMEA" && YTDMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                               LATAMCount = entity.NpsImps.Where(x => x.Region == "LATAM" && YTDMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                               NORAMCount = entity.NpsImps.Where(x => x.Region == "NORAM" && YTDMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                               GlobalCount = entity.NpsImps.Where(x => x.Region == "Global" && YTDMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                               Total = entity.NpsImps.Where(x => x.Region != null && YTDMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                           }).ToList().OrderBy(x => x.NPSIndicator);
            var PMData = (from a in entity.NpsImps
                          where a.RecordStatus != "Deleted"
                          where RollingNPSMonths.Any(val => a.YearMonth.Equals(val))
                          where a.NPSIndicator != null
                          where a.NPSIndicator != "null"
                          where a.NPSIndicator != ""
                          group a by a.NPSIndicator into g
                          select new
                          {
                              NPSIndicator = g.Key,
                              APACCount = entity.NpsImps.Where(x => x.Region == "APAC" && RollingNPSMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                              EMEACount = entity.NpsImps.Where(x => x.Region == "EMEA" && RollingNPSMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                              LATAMCount = entity.NpsImps.Where(x => x.Region == "LATAM" && RollingNPSMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                              NORAMCount = entity.NpsImps.Where(x => x.Region == "NORAM" && RollingNPSMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                              GlobalCount = entity.NpsImps.Where(x => x.Region == "Global" && RollingNPSMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                              Total = entity.NpsImps.Where(x => x.Region != null && RollingNPSMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                          }).ToList().OrderBy(x => x.NPSIndicator);
            var YearMonths = (from a in entity.NpsImps
                              where a.RecordStatus != "Deleted"
                              where a.YearMonth != null
                              where a.YearMonth != ""
                              select new
                              {
                                  a.YearMonth,
                                  isSelected = true
                              }).ToList().Distinct().OrderByDescending(x=>x.YearMonth);
            //var datas = (from a in entity.NpsImps
            //             where a.DateSurveyReceived != null
            //             select new
            //             {
            //                 a.NpsId
            //             }).ToList();
            //nps.NESemanticAnalysisThree = datas;
            //for (int i = 0; i < datas.AsQueryable().Count(); i++)
            //{
            //    var id = datas[i].NpsId;
            //    NpsImp vp = (from s in entity.NpsImps
            //                 where s.NpsId == id
            //                 select s).FirstOrDefault();
            //    vp.YearMonth = vp.DateSurveyReceived.Value.Year +"-"+ vp.DateSurveyReceived.Value.ToString("MM");
            //    entity.SaveChanges();
            //}
            nps.NERegionWiseNPSCount = CurrentMonthData;
            nps.NewRegionWiseNPSCount = YTDData;
            nps.ExistingRegionWiseNPSCount = PMData;
            nps.NPSvalues = YearMonths;
            nps.NewRegionWiseNPSScore = YTDMonths;
            nps.ExistingRegionWiseNPSScore = RollingNPSMonths.OrderByDescending(x=> x);
            return nps;
        }
        string[] SelectedYearMonths;
        string[] s_YTDMonths, s_RollingNPSMonths;
        string s_ytdmonth, s_rollingmonths;

        [HttpPost]
        [Route("GetNPSDataByYearMonth")]
        public NPSData GetNPSDataByYearMonth(NpsImp npsImp)
        {
            if (npsImp.YearMonth == null)
            {
                nps.NPSvalues = "";
                nps.code = 100;
                nps.message = "No Data found";
            }
            else
            {
                SelectedYearMonths = npsImp.YearMonth.Split(',');
                var SelectedData = (from a in entity.NpsImps
                                    where a.RecordStatus != "Deleted"
                                    where SelectedYearMonths.Any(val => a.YearMonth.Equals(val))
                                    where a.NPSIndicator != null
                                    where a.NPSIndicator != "null"
                                    where a.NPSIndicator != ""
                                    where a.YearMonth != null
                                    group a by a.NPSIndicator into g
                                    select new
                                    {
                                        NPSIndicator = g.Key,
                                        APACCount = entity.NpsImps.Where(x => x.Region == "APAC" && SelectedYearMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        EMEACount = entity.NpsImps.Where(x => x.Region == "EMEA" && SelectedYearMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        LATAMCount = entity.NpsImps.Where(x => x.Region == "LATAM" && SelectedYearMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        NORAMCount = entity.NpsImps.Where(x => x.Region == "NORAM" && SelectedYearMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        GlobalCount = entity.NpsImps.Where(x => x.Region == "Global" && SelectedYearMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                        Total = entity.NpsImps.Where(x => x.Region != null && SelectedYearMonths.Any(val => x.YearMonth.Equals(val)) && x.NPSIndicator == g.Key && x.RecordStatus != "Deleted").Count(),
                                    }).ToList().OrderBy(x => x.NPSIndicator);
                if(SelectedYearMonths.Count() == 1) {
                    string[] yearmonth = npsImp.YearMonth.Split('-');
                    int year = int.Parse(yearmonth[0]);
                    int month = int.Parse(yearmonth[1]);
                    for (int i = 1; i <= month; i++)
                    {
                        if(i == month)
                        {
                            if (i < 10)
                            {
                                s_ytdmonth += year + "-0" + i;
                            }
                            else
                            {
                                s_ytdmonth += year + "-" + i;
                            }
                        }
                        else
                        {
                            if (i < 10)
                            {
                                s_ytdmonth += year + "-0" + i + ",";
                            }
                            else
                            {
                                s_ytdmonth += year + "-" + i + ",";
                            }
                        }
                        
                    }
                    for (int i = 1; i <= 12; i++)
                    {
                        if (i > month)
                        {
                            if (i == 12)
                            {
                                s_rollingmonths += (year - 1) + "-" + i;
                            }
                            else if (i < 10)
                            {
                                s_rollingmonths += (year - 1) + "-0" + i + ",";
                            }
                            else
                            {
                                s_rollingmonths += (year-1) + "-" + i + ",";
                            }
                        }
                        else
                        {
                            if (i < 10)
                            {
                                s_rollingmonths += year + "-0" + i + ",";
                            }
                            else
                            {
                                s_rollingmonths += year + "-" + i + ",";
                            }
                        }
                    }
                    s_YTDMonths = s_ytdmonth.Split(',');
                    s_RollingNPSMonths = s_rollingmonths.Split(',');
                    nps.NewRegionWiseNPSCount = s_YTDMonths;
                    nps.ExistingRegionWiseNPSCount = s_RollingNPSMonths;
                    var NPSindicators = "Promoter,Detractor,Passive".Split(',');
                    double promoter = entity.NpsImps.Where(x => x.NPSIndicator == "Promoter" && s_YTDMonths.Any(val => x.YearMonth.Equals(val)) && x.RecordStatus != "Deleted").Count();
                    double detractor = entity.NpsImps.Where(x => x.NPSIndicator == "Detractor" && s_YTDMonths.Any(val => x.YearMonth.Equals(val)) && x.RecordStatus != "Deleted").Count();
                    double total = entity.NpsImps.Where(x => s_YTDMonths.Any(val => x.YearMonth.Equals(val)) && x.RecordStatus != "Deleted").Count();
                    double promoter_per = (promoter*100) / total;
                    double detractor_per = (detractor *100) / total;

                    double promoter_e = entity.NpsImps.Where(x => x.NPSIndicator == "Promoter" && s_RollingNPSMonths.Any(val => x.YearMonth.Equals(val)) && x.RecordStatus != "Deleted").Count();
                    double detractor_e = entity.NpsImps.Where(x => x.NPSIndicator == "Detractor" && s_RollingNPSMonths.Any(val => x.YearMonth.Equals(val)) && x.RecordStatus != "Deleted").Count();
                    double total_e = entity.NpsImps.Where(x => s_RollingNPSMonths.Any(val => x.YearMonth.Equals(val)) && x.RecordStatus != "Deleted").Count();
                    double promoter_per_e = (promoter_e * 100) / total_e;
                    double detractor_per_e = (detractor_e * 100) / total_e;
                    nps.NewRegionWiseNPSScore = (promoter_per - detractor_per);
                    nps.ExistingRegionWiseNPSScore = (promoter_per_e - detractor_per_e);
                }
                nps.NERegionWiseNPSCount = SelectedData;
                nps.code = 200;
                nps.message = "Success";
            }
            return nps;
        }
    }
}