using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Data;
using System.Net.Http;
using System.Web.Http;
using System.Globalization;
using CWTDashboardAPI.Models;

namespace CWTDashboardAPI.Controllers
{
    public class CapacityHierarchyController : ApiController
    {
        CWTDashboardEntities entity = new CWTDashboardEntities();
        Response re = new Response();
        CHFilter fi = new CHFilter();
        AutomatedCLRFilters CLR_F = new AutomatedCLRFilters();
        int HGetCount;
        [HttpPost]
        [Route("GetCapacityHierarchy")]
        public Response GetCapacityHierarchy(CapacityHierarchy hierarchy)
        {
            var Role = "PM 1,PM 2,PM 3".Split(',');
            var Hierarchylist = (from s in entity.CapacityHierarchies
                                 where s.ManagerStatus == "Active"
                                 //where Role.Any(val1 => s.Level.Equals(val1))
                                 select new
                                 {
                                     s.HID,
                                     s.Region,
                                     s.Level,
                                     s.PLevel,
                                     s.Leader,
                                     s.WorkShedule,
                                     s.Monday,
                                     s.Tuesday,
                                     s.Wednesday,
                                     s.Thursday,
                                     s.Friday,
                                     s.WorkingDays,
                                     s.Manager,
                                     s.ProjectLevel,
                                     s.InsertedBy,
                                     s.InsertedDate,
                                     s.TargetedUtilization,
                                 }).OrderBy(x => x.Level);
            HGetCount = Hierarchylist.AsQueryable().Count();
            if (HGetCount.ToString() == "" || HGetCount.ToString() == null || HGetCount == 0)
            {
                re.Data = Hierarchylist;
                re.code = 100;
                re.message = "No Data found";
            }
            else
            {
                re.Data = Hierarchylist;
                re.code = 200;
                re.message = "Data Successfull";
            }
            return re;
        }
        [HttpPost]
        [Route("AddingCapacityHierarchy")]
        public IHttpActionResult AddingCapacityHierarchy(CapacityHierarchy hierarchy)
        {
            Boolean b = new CapacityHierarchyClass().AddingHierarchy(hierarchy);
            if (b)
            {
                hierarchy.ManagerStatus = "Active";
                hierarchy.InsertedDate = DateTime.Now;
                entity.CapacityHierarchies.Add(hierarchy);
                entity.SaveChanges();
                re.code = 200;
                re.message = "Manager added Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Add Manager";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("UpdateCapacityHierarchy")]
        public IHttpActionResult UpdateCapacityHierarchy(CapacityHierarchy hierarchy)
        {
            Boolean b = new CapacityHierarchyClass().UpdateHierarchy(hierarchy);
            if (b)
            {
                re.code = 200;
                re.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Update";
                return Content(HttpStatusCode.OK, re);
            }
        }

        [HttpPost]
        [Route("UpdateHierarchyComment")]
        public IHttpActionResult UpdateHierarchyComment(CapacityHierarchy capacityHierarchy)
        {
            Boolean b = new CapacityHierarchyClass().UpdateC_HierarchyComment(capacityHierarchy);
            if (b)
            {
                re.code = 200;
                re.message = "Updated Succesfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Update";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("ListofLeadersManagers")]
        public AutomatedCLRFilters ListofLeadersManagers(CapacityHierarchy capacityHierarchy)
        {
            var FilterLeader = (from a in entity.CapacityHierarchies
                                where a.ManagerStatus == "Active"
                                select new
                                {
                                    GlobalProjectManager = a.Leader,
                                    isSelected = true
                                }).Distinct();
            var FilterManager = (from a in entity.CapacityHierarchies
                                 where a.ManagerStatus == "Active"
                                 select new
                                {
                                    GlobalProjectManager = a.Manager,
                                    isSelected = true
                                }).Distinct();
            var FilterCountry = (from b in entity.CountryIsoCodes
                                 select new
                                 {
                                     Country = b.CountryName,
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.Country).ToList();
            var FilterAccountName = (from b in entity.CLRDatas
                                      where b.Workspace_Title != null
                                      where b.Workspace_Title != ""
                                      select new
                                      {
                                          Account_Name = b.Workspace_Title,
                                          isSelected = true,
                                      }).Distinct().OrderBy(x => x.Account_Name).ToList();
            var NPSCommentOne = (from b in entity.NpsImps
                                 select new {
                                     NPSComment = b.NPSCommentsOne == null ? "Others" : b.NPSCommentsOne,
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.NPSComment).ToList();
            var NPSCommentTwo = (from b in entity.NpsImps
                                 select new
                                 {
                                     NPSComment = b.NPSCommentsTwo == null ? "Others" : b.NPSCommentsTwo,
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.NPSComment).ToList();
            var NPSCommentThree = (from b in entity.NpsImps
                                 select new
                                 {
                                     NPSComment = b.NPSCommentsThree == null ? "Others" : b.NPSCommentsThree,
                                     isSelected = true,
                                 }).Distinct().OrderBy(x => x.NPSComment).ToList();
            var FilteredData = FilterLeader.Concat(FilterManager).Distinct().OrderBy(x => x.GlobalProjectManager);
            CLR_F.code = 200;
            CLR_F.message = "Success";
            CLR_F.FilterGlobalProjectManager = FilteredData;
            CLR_F.FilterCountry = FilterCountry;
            CLR_F.FilterAccountName = FilterAccountName;
            CLR_F.NPSCommentOne = NPSCommentOne;
            CLR_F.NPSCommentTwo = NPSCommentTwo;
            CLR_F.NPSCommentThree = NPSCommentThree;
            return CLR_F;
        }
        [HttpPost]
        [Route("DeleteUserFromCapacityHierarchy")]
        public IHttpActionResult DeleteUserFromCapacityHierarchy(CapacityHierarchy capacityHierarchy)
        {
            Boolean b = new HomeModel().DeleteUserinHierarchy(capacityHierarchy);
            if (b)
            {
                re.code = 200;
                re.message = "Deleted Successfully";
                return Content(HttpStatusCode.OK, re);
            }
            else
            {
                re.code = 100;
                re.message = "Failed to Delete User";
                return Content(HttpStatusCode.OK, re);
            }
        }
        [HttpPost]
        [Route("CapacityHierarchyFilters")]
        public CHFilter CapacityHierarchyFilters(CapacityHierarchy hierarchy)
        {
            var FilterRegion = (from a in entity.CapacityHierarchies
                                where a.Region != null
                                select new
                                {
                                    Region = a.Region,
                                    isSelected = true,
                                }).Distinct().OrderByDescending(x => x.Region);
            var FilterLevel = (from a in entity.CapacityHierarchies
                               where a.PLevel != null
                               select new
                               {
                                   Level = a.PLevel,
                                   isSelected = true,
                               }).Distinct().OrderByDescending(x => x.Level);
            var FilterLeader = (from a in entity.CapacityHierarchies
                                where a.Leader != null
                                select new
                                {
                                    Leader = a.Leader,
                                    isSelected = true,
                                }).Distinct().OrderByDescending(x => x.Leader);
            var FilterWorkShedule = (from a in entity.CapacityHierarchies
                                     where a.WorkShedule != null
                                     select new
                                     {
                                         WorkShedule = a.WorkShedule,
                                         isSelected = true,
                                     }).Distinct().OrderByDescending(x => x.WorkShedule);
            var FilterWorkingDays = (from a in entity.CapacityHierarchies
                                     select new
                                     {
                                         WorkingDays = a.WorkingDays,
                                         isSelected = true,
                                     }).Distinct().OrderByDescending(x => x.WorkingDays);
            fi.code = 200;
            fi.message = "Success";
            fi.Region = FilterRegion;
            fi.Level = FilterLevel;
            fi.Leader = FilterLeader;
            fi.WorkShedule = FilterWorkShedule;
            fi.WorkingDays = FilterWorkingDays;
            return fi;
        }
    }
}