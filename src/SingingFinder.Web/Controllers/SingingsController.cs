using System;
using System.Collections.Generic;
using System.Web.Mvc;
using SingingFinder.Core;
using SingingFinder.Web.Controllers.Extensions;
using SingingFinder.Web.Models;
using SingingFinder.Web.Services;

namespace SingingFinder.Web.Controllers
{
    [CompressContent]
    public class SingingsController : Controller
    {
        private readonly ISingingService _singingService;

        public SingingsController(ISingingService singingService)
        {
            _singingService = singingService;
        }

        // GET: Singings
        public ActionResult Index(DateTime? start = null, DateTime? end = null, Book book = Book.All, SingingType singingType = SingingType.All)
            => PartialView("MapPartial", new SingingsViewModel(Singings(start, end, book, singingType), start.ResolveStart(), end.ResolveEnd(), book, singingType));

        public ActionResult AnnualFourShape()
            => View(Singings(null, null, (Book)Enum.Parse(typeof(Book),"Four-Shape"), SingingType.Annual));

        public ActionResult AnnualSevenShape()
            => View(Singings(null, null, (Book)Enum.Parse(typeof(Book), "Seven-Shape"), SingingType.Annual));

        private IEnumerable<Event> Singings(DateTime? start = null, DateTime? end = null, Book book = Book.All, SingingType singingType = SingingType.All) 
            => _singingService.Get(start.ResolveStart(), end.ResolveEnd(), book, singingType);
    }
}