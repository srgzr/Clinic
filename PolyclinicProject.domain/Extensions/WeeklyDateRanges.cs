using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitnesApp.Domain.Extensions
{
    public static class WeeklyDateRanges
    {

        public static DateTime GetFirstDayOfWeek(this DateTime dat)
        {
            int day = ((int) dat.DayOfWeek)==0?7 : ((int)dat.DayOfWeek);
            return dat.Date.AddDays(-day+1);
        }

        public static DateTime GetLastDayOfWeek(this DateTime dat)
        {
            int day = ((int)dat.DayOfWeek) == 0 ? 0: (7-((int)dat.DayOfWeek));
            var d = dat.Date.AddDays(day);
            return new DateTime(d.Year, d.Month, d.Day, 23,59,59);
        }
    }
}
