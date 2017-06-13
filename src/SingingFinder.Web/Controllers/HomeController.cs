using System.Web.Mvc;
using SingingFinder.Core;

namespace SingingFinder.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View(SingingRepository.singings);
        }
    }
}
