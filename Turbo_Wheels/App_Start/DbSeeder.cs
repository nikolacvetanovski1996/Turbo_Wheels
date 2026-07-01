using System.Configuration;
using System.Linq;
using System.Web.Helpers;
using Turbo_Wheels.Models;

namespace Turbo_Wheels.App_Start
{
    public static class DbSeeder
    {
        public static void SeedAdmin()
        {
            using (var db = new Turbo_WheelsContext())
            {
                // If any admin exists, do nothing
                if (db.Users.Any(u => u.IsAdmin))
                    return;

                var username = ConfigurationManager.AppSettings["DefaultAdminUsername"];
                var password = ConfigurationManager.AppSettings["DefaultAdminPassword"];
                var email = ConfigurationManager.AppSettings["DefaultAdminEmail"];

                if (string.IsNullOrWhiteSpace(username) ||
                    string.IsNullOrWhiteSpace(password) ||
                    string.IsNullOrWhiteSpace(email))
                {
                    throw new ConfigurationErrorsException(
                        "Default administrator credentials are missing from Web.config.");
                }

                var admin = new User
                {
                    Username = username,
                    Password = Crypto.HashPassword(password),
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = email,
                    IsAdmin = true
                };

                db.Users.Add(admin);
                db.SaveChanges();
            }
        }
    }
}