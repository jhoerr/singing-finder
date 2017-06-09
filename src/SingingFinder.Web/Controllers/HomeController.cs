using SingingFinder.Web.Models;
using Microsoft.FSharp.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.Kestrel.Internal.Http;

namespace SingingFinder.Web.Controllers
{
    /// <summary>
    /// A controller intercepts the incoming browser request and returns
    /// an HTML view (.cshtml file) or any other type of data.
    /// </summary>
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            // The view being returned is calculated based on the name of the
            // controller (Home) and the name of the action method (Index).
            // So in this case, the view returned is /Views/Home/Index.cshtml.
            return View();
        }

        public IActionResult About()
        {
            // Creates a model and passes it on to the view.
            Employee[] model =
            {
                new Employee { Name = "Alfred", Title = "Manager" },
                new Employee { Name = "Sarah", Title = "Accountant" }
            };

            SingingFinder.Core.Date.findFirstSundayInMonth


            return View(model);
        }
    }
}
