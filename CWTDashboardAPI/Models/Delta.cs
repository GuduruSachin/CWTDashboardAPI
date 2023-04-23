using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class Delta
    {
        public int deltaid, count;
        public Boolean UpdateDeltaComments(MonthWiseDelta monthWiseDelta)
        {
            List<MonthWiseDelta> list = new List<MonthWiseDelta>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.MonthWiseDeltas.OrderBy(a => a.DeltaID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    deltaid = list[i].DeltaID;
                    if (monthWiseDelta.DeltaID.Equals(deltaid))
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
                    return false;
                }
                else
                {
                    using (CWTDashboardEntities entit = new CWTDashboardEntities())
                    {
                        MonthWiseDelta vp = (from s in entit.MonthWiseDeltas
                                                  where s.DeltaID == monthWiseDelta.DeltaID
                                                  select s).FirstOrDefault();
                        vp.Jan_Comments = monthWiseDelta.Jan_Comments;
                        vp.Feb_Comments = monthWiseDelta.Feb_Comments;
                        vp.Mar_Comments = monthWiseDelta.Mar_Comments;
                        vp.Apr_Comments = monthWiseDelta.Apr_Comments;
                        vp.May_Comments = monthWiseDelta.May_Comments;
                        vp.Jun_Comments = monthWiseDelta.Jun_Comments;
                        vp.Jul_Comments = monthWiseDelta.Jul_Comments;
                        vp.Aug_Comments = monthWiseDelta.Aug_Comments;
                        vp.Sep_Comments = monthWiseDelta.Sep_Comments;
                        vp.Oct_Comments = monthWiseDelta.Oct_Comments;
                        vp.Nov_Comments = monthWiseDelta.Nov_Comments;
                        vp.Dec_Comments = monthWiseDelta.Dec_Comments;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
    }
}