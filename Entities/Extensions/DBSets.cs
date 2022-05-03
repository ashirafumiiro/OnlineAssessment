using Microsoft.EntityFrameworkCore;
using DataAccess.Enities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Extensions
{
    public static class DBSets
    {
        public static IQueryable<Schedule> GetActive(this DbSet<Schedule> schedules)
        {
            return schedules.Where(p => p.SystemStatusId == (int)SM.Enumerations.SystemStatus.Active);
        }

        public static IQueryable<Lab> GetActive(this DbSet<Lab> labs)
        {
            return labs.Where(p => p.SystemStatusId == (int)SM.Enumerations.SystemStatus.Active);
        }

        public static IQueryable<AspNetUser> GetActive(this DbSet<AspNetUser> users)
        {
            return users.Where(p => p.SystemStatusId == (int)SM.Enumerations.SystemStatus.Active);
        }

    }
}
