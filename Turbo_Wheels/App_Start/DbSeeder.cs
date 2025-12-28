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

                var admin = new User
                {
                    Username = "admin",
                    Password = Crypto.HashPassword("admin123"),
                    FirstName = "System",
                    LastName = "Administrator",
                    Email = "admin@example.com",
                    IsAdmin = true
                };

                db.Users.Add(admin);
                db.SaveChanges();
            }
        }
    }
}