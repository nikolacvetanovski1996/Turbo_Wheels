using System.Data.Entity;

namespace Turbo_Wheels.Models
{
    public class Turbo_WheelsContext : DbContext
    {
        public Turbo_WheelsContext() : base("name=Turbo_WheelsContext") { }

        public DbSet<Vehicle> Vehicles { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Reservation> Reservations { get; set; }
    }
}
