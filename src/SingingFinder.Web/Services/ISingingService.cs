using System;
using System.Collections.Generic;
using SingingFinder.Core;

namespace SingingFinder.Web.Services
{
    public interface ISingingService
    {
        IEnumerable<Event> Get(DateTime start , DateTime end, Book book, SingingType singingType);
    }
}