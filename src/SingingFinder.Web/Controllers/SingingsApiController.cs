using System;
using System.Collections.Generic;
using System.Web.Http;
using SingingFinder.Core;
using SingingFinder.Web.Controllers.Extensions;
using SingingFinder.Web.Services;

namespace SingingFinder.Web.Controllers
{
    [CompressContent]
    [Route("api/singings")]
    public class SingingsApiController : ApiController
    {
        private readonly ISingingService _singingService;

        public SingingsApiController(ISingingService singingService)
        {
            _singingService = singingService;
        }

        public IEnumerable<Event> Get(DateTime? start = null, DateTime? end = null, Book book = Book.All, SingingType singingType = SingingType.All)
            => _singingService.Get(start.ResolveStart(), end.ResolveEnd(), book, singingType);
    }
}
