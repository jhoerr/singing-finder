using System;
using System.Collections.Generic;
using System.Web.Http;
using Newtonsoft.Json;
using SingingFinder.Core;
using SingingFinder.Web.Controllers.Extensions;
using SingingFinder.Web.Services;

namespace SingingFinder.Web.Controllers.Api
{
    /// <summary>
    /// Provides information and locations for Sacred Harp singings occuring within a specified time period.
    /// </summary>
    [CompressContent]
    [Route("api/singings")]
    public class SingingsController : ApiController
    {
        private readonly ISingingService _singingService;

        public SingingsController(ISingingService singingService)
        {
            _singingService = singingService;
        }

        /// <summary>
        /// Fetch all singings matching the supplied query constraints. All returned dates and times are local to the timezone of the singing location.
        /// </summary>
        /// <param name="start">The start date of the search. Only singings occuring on or after this date will be returned. Format: YYYY-MM-DD. Default: today.</param>
        /// <param name="end">The end date of the search. Only singings occuring on or before this date will be returned. Format: YYYY-MM-DD. Default: one year from today.</param>
        /// <param name="book">The Sacred Harp tune book. Default: All books.</param>
        /// <param name="singingType">The type of singing. Regular (local) singings occur frequently throughout the year on a monthly or semi-monthly basis. Annual (all-day) singings occur once a year. Default: all singing types.</param>
        /// <returns>A list of singings, their locations, and the calendar dates on which they fall.</returns>
        public IEnumerable<Event> Get(DateTime? start = null, DateTime? end = null, Book book = Book.All, SingingType singingType = SingingType.All)
            => _singingService.Get(start.ResolveStart(), end.ResolveEnd(), book, singingType);
    }
}
