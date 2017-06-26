using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SingingFinder.Core;
using SingingFinder.Web.Controllers.Extensions;

namespace SingingFinder.Web.Services
{
    public class SingingService : ISingingService
    {
        public IEnumerable<Event> Get (DateTime start , DateTime end, Book book, SingingType singingType) 
            => SingingRepository
                .getSingingsInRange(start, end, book, singingType, 0)
                .Map(e => e.ToList());
    }

}