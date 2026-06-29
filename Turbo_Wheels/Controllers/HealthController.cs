using System.Web.Mvc;
using Turbo_Wheels.Filters;

namespace Turbo_Wheels.Controllers
{
    [AllowAnonymous]
    public class HealthController : Controller
    {
        [NoCache]
        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Head)]
        public ActionResult Index()
        {
            return Content("Healthy", "text/plain");
        }
    }
}