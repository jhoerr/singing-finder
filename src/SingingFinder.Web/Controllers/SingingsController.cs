using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SingingFinder.Core;

namespace SingingFinder.Web.Controllers
{
    public class SingingsController : Controller
    {
        // GET: Singings
        public ActionResult Index(DateTime? start = null, DateTime? end = null) 
            => Json(Singings(), JsonRequestBehavior.AllowGet);

        public ActionResult Map(DateTime? start = null, DateTime? end = null)
            => PartialView("MapPartial", Singings(start, end));

        private IEnumerable<Event> Singings(DateTime? start = null, DateTime? end = null) 
            => SingingRepository.singingsInRange(ResolveStart(start), ResolveEnd(end), 0);

        private static DateTime ResolveStart(DateTime? start)
            => start ?? DateTime.Today;

        private static DateTime ResolveEnd(DateTime? end) 
            => end ?? DateTime.Today.AddYears(1).AddDays(1.0);

    }
}