using System.Collections.Generic;
using System.Web.Mvc;
using SingingFinder.Core;

namespace SingingFinder.Web.Controllers
{
    public class SingingsController : Controller
    {
        // GET: Singings
        public ActionResult Index() 
            => Json(Singings(), JsonRequestBehavior.AllowGet);

        public ActionResult Map()
            => PartialView("MapPartial", Singings());

        private IEnumerable<Singing> Singings() 
            => SingingRepository.singings;
    }
}