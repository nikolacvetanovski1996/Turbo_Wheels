using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Turbo_Wheels.App_Start;

namespace Turbo_Wheels
{
    public class MvcApplication : System.Web.HttpApplication
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
    }   
}
