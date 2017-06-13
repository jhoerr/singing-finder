using System.Web.Mvc;
using SingingFinder.Core;

namespace SingingFinder.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index() 
            => View();
    }
}
