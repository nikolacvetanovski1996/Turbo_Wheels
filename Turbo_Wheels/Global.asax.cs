using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Turbo_Wheels.App_Start;

namespace Turbo_Wheels
{
    public class MvcApplication : HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            // Enable bundling and minification even in debug mode for consistent script/css loading
            BundleTable.EnableOptimizations = true;

            // Initial system admin seeded for first-time application access
            DbSeeder.SeedAdmin();
        }

        protected void Application_Error()
        {
            var exception = Server.GetLastError();
            Server.ClearError();

            if (exception is HttpException httpException)
            {
                switch (httpException.GetHttpCode())
                {
                    case 404:
                        Response.Redirect("~/Error/NotFound", false);
                        break;

                    default:
                        Response.Redirect("~/Error/ServerError", false);
                        break;
                }
            }
            else
            {
                Response.Redirect("~/Error/ServerError", false);
                
            }

            Context.ApplicationInstance.CompleteRequest();
        }
    }   
}
