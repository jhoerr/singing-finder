﻿using System;
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
        public ActionResult Index(DateTime? start = null, DateTime? end = null, Book book = Book.All) 
            => Json(Singings(start, end, book), JsonRequestBehavior.AllowGet);

        public ActionResult Map(DateTime? start = null, DateTime? end = null, Book book = Book.All)
            => PartialView("MapPartial", new SingingsViewModel(Singings(start, end, book), ResolveStart(start), ResolveEnd(end), book));

        public ActionResult Annual()
            => View(Singings());

        private IEnumerable<Event> Singings(DateTime? start = null, DateTime? end = null, Book book = Book.All) 
            => SingingRepository.singingsInRange(ResolveStart(start), ResolveEnd(end), book, 0);

        private static DateTime ResolveStart(DateTime? start)
            => start ?? DateTime.Today;

        private static DateTime ResolveEnd(DateTime? end) 
            => end ?? DateTime.Today.AddYears(1).AddDays(1.0);

    }
}