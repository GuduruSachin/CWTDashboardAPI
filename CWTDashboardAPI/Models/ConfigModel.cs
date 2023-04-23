using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CWTDashboardAPI.Models
{
    public class ConfigModel
    {
        public int count, D_ConfigID, U_ConfigID, TargetID;
        string ProjectType, Year, Month;

        public Boolean AddingConfig(Config config)
        {
            List<Config> list = new List<Config>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Configs.OrderBy(a => a.ConfigID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    ProjectType = list[i].ProjectType;
                    if (config.ProjectType.Equals(ProjectType))
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        public Boolean AddingTargetCycleTime(TargetCycleTime targetCycleTime)
        {
            List<TargetCycleTime> list = new List<TargetCycleTime>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.TargetCycleTimes.OrderBy(a => a.TargetID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    Year = list[i].Year;
                    if (targetCycleTime.Year.Equals(Year) && targetCycleTime.Months.Equals(Month))
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }


        public Boolean UpdateConfig(Config config)
        {
            List<Config> list = new List<Config>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Configs.OrderBy(a => a.ConfigID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    U_ConfigID = list[i].ConfigID;
                    if (config.ConfigID.Equals(U_ConfigID))
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
                        Config vp = (from s in entit.Configs
                                     where s.ConfigID == config.ConfigID
                                     select s).FirstOrDefault();
                        vp.ProjectType = config.ProjectType;
                        vp.Duration = config.Duration;
                        vp.UpdatedOn = DateTime.Now;
                        vp.UpdatedBy = config.UpdatedBy;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }

        public Boolean UpdateTargetCycleTime(TargetCycleTime targetCycleTime)
        {
            List<TargetCycleTime> list = new List<TargetCycleTime>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.TargetCycleTimes.OrderBy(a => a.TargetID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    TargetID = list[i].TargetID;
                    if (targetCycleTime.TargetID.Equals(TargetID))
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
                        TargetCycleTime vp = (from s in entit.TargetCycleTimes
                                     where s.TargetID == targetCycleTime.TargetID
                                     select s).FirstOrDefault();
                        vp.NewGlobal = targetCycleTime.NewGlobal;
                        vp.NewRegional = targetCycleTime.NewRegional;
                        vp.NewLocal = targetCycleTime.NewLocal;
                        vp.ExistingAddChange = targetCycleTime.ExistingAddChange;
                        vp.ExistingServiceConfigChange = targetCycleTime.ExistingServiceConfigChange;
                        vp.Overall = targetCycleTime.Overall;
                        vp.UpdatedOn = DateTime.Now;
                        vp.UpdatedBy = targetCycleTime.UpdatedBy;
                        vp.Year = targetCycleTime.Year;
                        vp.Months = targetCycleTime.Months;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
        public Boolean DeleteConfig(Config config)
        {
            List<Config> list = new List<Config>();
            using (CWTDashboardEntities entity = new CWTDashboardEntities())
            {
                list = entity.Configs.OrderBy(a => a.ConfigID).ToList();
                int c = list.Count;
                for (int i = 0; i < c; i++)
                {
                    D_ConfigID = list[i].ConfigID;
                    if (config.ConfigID.Equals(D_ConfigID))
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
                        Config vp = (from s in entit.Configs
                                           where s.ConfigID == config.ConfigID
                                     select s).FirstOrDefault();
                        vp.Status = "Deleted";
                        vp.UpdatedBy = config.UpdatedBy;
                        vp.UpdatedOn = DateTime.Now;
                        entit.SaveChanges();
                    }
                    return true;
                }
            }
        }
    }
}