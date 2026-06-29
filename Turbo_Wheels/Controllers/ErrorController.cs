using System.Web.Mvc;

namespace Turbo_Wheels.Controllers
{
    [AllowAnonymous]
    public class ErrorController : Controller
    {
        public ActionResult Forbidden()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 403;
            return View("~/Views/Shared/Forbidden.cshtml");
        }

        public ActionResult NotFound()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 404;
            return View("~/Views/Shared/NotFound.cshtml");
        }

        public ActionResult ServerError()
        {
            Response.TrySkipIisCustomErrors = true;
            Response.StatusCode = 500;
            return View("~/Views/Shared/Error.cshtml");
        }
    }
}