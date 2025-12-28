using System.Web.Mvc;
using System.Web.Routing;

namespace Turbo_Wheels.Filters
{
    // Restricts access to admin-only users based on session user.IsAdmin
    public class RequireAdminAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            // Allow anonymous skips
            if (filterContext.ActionDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true) ||
                filterContext.ActionDescriptor.ControllerDescriptor.IsDefined(typeof(AllowAnonymousAttribute), true))
            {
                return;
            }

            var user = filterContext.HttpContext.Session["user"] as Models.User;

            // Redirect to login if not logged in
            if (user == null)
            {
                filterContext.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "Login" }
                    });
                return;
            }

            // Check if user is admin
            if (!user.IsAdmin)
            {
                filterContext.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Unauthorized.cshtml"
                };
                return;
            }

        }
    }
}