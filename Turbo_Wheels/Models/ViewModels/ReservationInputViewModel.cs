using System;

namespace Turbo_Wheels.Models
{
    public class ReservationInputViewModel
    {
        public string PickupPlace { get; set; }

        public DateTime PickupDate { get; set; }

        public TimeSpan PickupTime { get; set; }

        public string ReturnPlace { get; set; }

        public DateTime ReturnDate { get; set; }

        public TimeSpan ReturnTime { get; set; }
    }
}