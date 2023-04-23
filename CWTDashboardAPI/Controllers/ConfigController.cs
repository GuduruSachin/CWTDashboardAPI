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
    public class ConfigController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        H_Filters h_f = new H_Filters();
        Response re = new Response();

        [HttpPost]
        [Route("ConfigInsert")]
        public IHttpActionResult ConfigInsert(Config config)
        {
            Boolean b = new ConfigModel().AddingConfig(config);
            if (b)
            {
                config.InsertedOn = DateTime.Now;
                config.Status = "Active";
                entity.Configs.Add(config);
                entity.SaveChanges();
                re.code = 200;
                re.message = "Added Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to add New";
                return Content(HttpStatusCode.OK, re);
            }
        }

        [HttpPost]
        [Route("ConfigUpdate")]
        public IHttpActionResult ConfigUpdate(Config config)
        {
            Boolean b = new ConfigModel().UpdateConfig(config);
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
        [Route("TargetCycleTimeUpdate")]
        public IHttpActionResult TargetCycleTimeUpdate(TargetCycleTime targetCycleTime)
        {
            Boolean b = new ConfigModel().UpdateTargetCycleTime(targetCycleTime);
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
        [Route("TargetCycleTimeData")]
        public Response TargetCycleTimeData(TargetCycleTime targetCycleTime)
        {
            var HOneTargetCycleTimeData = (from a in entity.TargetCycleTimes
                                           where a.Year == targetCycleTime.Year
                                           where a.Months == "Y"
                                           select new
                                           {
                                               a.TargetID,
                                               a.ExistingServiceConfigChange,
                                               a.ExistingAddChange,
                                               a.NewGlobal,
                                               a.NewRegional,
                                               a.NewLocal,
                                               a.Overall,
                                               a.Year,
                                               a.Months,
                                           });
            //var HTwoTargetCycleTimeData = (from a in entity.TargetCycleTimes
            //                               where a.Year == targetCycleTime.Year
            //                               where a.Months == "H2"
            //                               select new
            //                               {
            //                                   a.TargetID,
            //                                   a.ExistingServiceConfigChange,
            //                                   a.ExistingAddChange,
            //                                   a.NewGlobal,
            //                                   a.NewRegional,
            //                                   a.NewLocal,
            //                                   a.Overall,
            //                                   a.Year,
            //                                   a.Months,
            //                               });
            var DataCount = HOneTargetCycleTimeData.AsQueryable().Count();
            if (DataCount.ToString() == "---" || DataCount.ToString() == null || DataCount == 0)
            {
                re.code = 100;
                re.message = "No Data Found";
                re.Data = null;
            }
            else
            {
                re.code = 200;
                re.message = "Data Success";
                re.Data = HOneTargetCycleTimeData;
                //re.Data = HTwoTargetCycleTimeData;
                //re.TargetCycleTimeData = HOneTargetCycleTimeData;
            }
            return re;
        }

        [HttpPost]
        [Route("TargetCycleTimeInsert")]
        public IHttpActionResult TargetCycleTimeInsert(TargetCycleTime targetCycleTime)
        {
            Boolean b = new ConfigModel().AddingTargetCycleTime(targetCycleTime);
            if (b)
            {
                targetCycleTime.InsertedOn = DateTime.Now;
                entity.TargetCycleTimes.Add(targetCycleTime);
                entity.SaveChanges();
                re.code = 200;
                re.message = "Added Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to add New";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("TargetsCycleTimeData")]
        public Response TargetsCycleTimeData(TargetCycleTime targetCycleTime)
        {
            var data = (from a in entity.TargetCycleTimes
                        select new
                        {
                            a.TargetID,
                            a.ExistingServiceConfigChange,
                            a.NewGlobal,
                            a.NewLocal,
                            a.NewRegional,
                            a.Overall,
                            a.ExistingAddChange,
                            a.Year,
                            a.Months,
                            a.InsertedBy,
                            a.InsertedOn
                        }).ToList();
            re.Data = data;
            re.message = "Data Sucesssfull";
            re.code = 200;
            return re;
        }
        [HttpPost]
        [Route("ConfigData")]
        public Response ConfigData(Config config)
        {
            var ConfigData = (from a in entity.Configs
                              where a.Status != "Deleted"
                              select new {
                                  a.ConfigID,
                                  a.ProjectType,
                                  a.Duration,
                                  a.Status,
                                  a.InsertedBy,
                                  a.InsertedOn,
                                  a.UpdatedBy,
                                  a.UpdatedOn
                              });
            var DataCount = ConfigData.AsQueryable().Count();
            if (DataCount.ToString() == "---" || DataCount.ToString() == null || DataCount == 0)
            {
                re.code = 100;
                re.message = "No Data Found";
                re.Data = null;
            }
            else
            {
                re.code = 200;
                re.message = "Data Success";
                re.Data = ConfigData;
            }
            return re;
        }
        [HttpPost]
        [Route("ConfigDelete")]
        public IHttpActionResult ConfigDelete(Config config)
        {
            Boolean b = new ConfigModel().DeleteConfig(config);
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
        [Route("ConradData")]
        public Response ConradData(Comment comment)
        {
            var data = (from a in entity.CandidateLeadsOnes
                        select new
                        {
                            a.ID,
                            a.Candidate_Name,
                            Comments = entity.Comments.Where(x=>x.Comments.Contains(a.Candidate_Name)).Select(x=>x.Comments)
                        }).OrderBy(x => x.ID);
            re.Data = data;
            re.code = 200;
            re.message = "success";
            return re;
        }
    }
}
