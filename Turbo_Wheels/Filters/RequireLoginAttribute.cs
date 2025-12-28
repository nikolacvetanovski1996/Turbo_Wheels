using System.Web.Mvc;
using System.Web.Routing;

namespace Turbo_Wheels.Filters
{
    // Ensures the user is authenticated before accessing protected actions
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // If action allows anonymous access, skip authentication check
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            // If user is not logged in, redirect to Login page
            if (filterContext.HttpContext.Session["user"] == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Login" }
                    });
                return;
            }
        }
    }
}