using System;
using System.Web;
using System.Web.Mvc;

namespace Turbo_Wheels.Filters
{
    public class NoCacheAttribute : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {
            var cache = filterContext.HttpContext.Response.Cache;
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();
            cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            base.OnResultExecuting(filterContext);
        }
    }
}