using System;

namespace Turbo_Wheels.Models
{
    public class ReservationViewModel
    {
        public string PickupPlace { get; set; }

        public DateTime PickupDateTime { get; set; }

        public string ReturnPlace { get; set; }

        public DateTime ReturnDateTime { get; set; }
    }
}