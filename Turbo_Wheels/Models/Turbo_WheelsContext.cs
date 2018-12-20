using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace Turbo_Wheels.Models
{
    public class Turbo_WheelsContext : DbContext
    {
        // You can add custom code to this file. Changes will not be overwritten.
        // 
        // If you want Entity Framework to drop and regenerate your database
        // automatically whenever you change your model schema, please use data migrations.
        // For more information refer to the documentation:
        // http://msdn.microsoft.com/en-us/data/jj591621.aspx
    
        public Turbo_WheelsContext() : base("name=Turbo_WheelsContext")
        {
        }

        public System.Data.Entity.DbSet<Turbo_Wheels.Models.Vehicle> Vehicles { get; set; }

        public System.Data.Entity.DbSet<Turbo_Wheels.Models.User> Users { get; set; }

        public System.Data.Entity.DbSet<Turbo_Wheels.Models.Reservation> Reservations { get; set; }
    }
}
