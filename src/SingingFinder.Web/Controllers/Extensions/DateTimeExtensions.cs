using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SingingFinder.Web.Controllers.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime ResolveStart(this DateTime? start)
            => start ?? DateTime.Today;

        public static DateTime ResolveEnd(this DateTime? end)
            => end ?? DateTime.Today.AddYears(1).AddDays(1.0);
    }
}