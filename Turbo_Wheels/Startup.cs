using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Turbo_Wheels.Startup))]
namespace Turbo_Wheels
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
