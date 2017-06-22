using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SingingFinder.Core;
using SingingFinder.Web.Models;

namespace SingingFinder.Web.Controllers
{
    [CompressContent]
    public class SingingsController : Controller
    {
        // GET: Singings
        public ActionResult Index(DateTime? start = null, DateTime? end = null, Book book = Book.All, SingingType singingType = SingingType.All) 
            => Json(Singings(start, end, book, singingType), JsonRequestBehavior.AllowGet);

        public ActionResult Map(DateTime? start = null, DateTime? end = null, Book book = Book.All, SingingType singingType = SingingType.All)
            => PartialView("MapPartial", new SingingsViewModel(Singings(start, end, book, singingType), ResolveStart(start), ResolveEnd(end), book, singingType));

        public ActionResult AnnualFourShape()
            => View(Singings(null, null, (Book)Enum.Parse(typeof(Book),"Four-Shape"), SingingType.Annual));

        public ActionResult AnnualSevenShape()
            => View(Singings(null, null, (Book)Enum.Parse(typeof(Book), "Seven-Shape"), SingingType.Annual));

        private IEnumerable<Event> Singings(DateTime? start = null, DateTime? end = null, Book book = Book.All, SingingType singingType = SingingType.All) 
            => SingingRepository.getSingingsInRange(ResolveStart(start), ResolveEnd(end), book, singingType, 0);

        private static DateTime ResolveStart(DateTime? start)
            => start ?? DateTime.Today;

        private static DateTime ResolveEnd(DateTime? end) 
            => end ?? DateTime.Today.AddYears(1).AddDays(1.0);

    }
}