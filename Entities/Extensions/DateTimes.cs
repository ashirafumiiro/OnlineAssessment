using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Extensions
{
    public static class DateTimes
    {
        public static bool IsGreaterThan(this DateTime dateTime, DateTime compDateTime)
        {
            int compValue = DateTime.Compare(dateTime, compDateTime);
            return compValue > 0;
        }

        public static bool IsLessThan(this DateTime dateTime, DateTime compDateTime)
        {
            int compValue = DateTime.Compare(dateTime, compDateTime);
            return compValue < 0;
        }
    }
}
