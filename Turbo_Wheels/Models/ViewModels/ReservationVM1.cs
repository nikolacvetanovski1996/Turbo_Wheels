using System;
using System.ComponentModel.DataAnnotations;

namespace Turbo_Wheels.Models
{
    public class ReservationVM1
    {
        public string pickupPlace { get; set; }
        public DateTime pickupDate { get; set; }
        public DateTime pickupTime { get; set; }
        public string returnPlace { get; set; }
        public DateTime returnDate { get; set; }
        public DateTime returnTime { get; set; }
    }
}