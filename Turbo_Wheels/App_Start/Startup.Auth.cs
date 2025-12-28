using Microsoft.AspNet.Identity;
using Owin;
using System;

namespace Turbo_Wheels
{
    public partial class Startup
    {
        public void ConfigureAuth(IAppBuilder app)
        {
            // Enables the application to temporarily store user information when they are verifying the second factor in the two-factor authentication process.
            app.UseTwoFactorSignInCookie(DefaultAuthenticationTypes.TwoFactorCookie, TimeSpan.FromMinutes(5));

            // Enables the application to remember the second login verification factor such as phone or email.
            app.UseTwoFactorRememberBrowserCookie(DefaultAuthenticationTypes.TwoFactorRememberBrowserCookie);
        }
    }
}