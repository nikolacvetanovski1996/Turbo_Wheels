using System;
using System.ComponentModel.DataAnnotations;

namespace Turbo_Wheels.Models
{
    public class ReservationInputViewModel
    {
        [Display(Name = "Place of taking")]
        public OfficeLocation PickupPlace { get; set; }

        [Display(Name = "Date of taking")]
        public DateTime PickupDate { get; set; }

        [Display(Name = "Time of taking")]
        public TimeSpan PickupTime { get; set; }

        [Display(Name = "Place of return")]
        public OfficeLocation ReturnPlace { get; set; }

        [Display(Name = "Date of return")]
        public DateTime ReturnDate { get; set; }

        [Display(Name = "Time of return")]
        public TimeSpan ReturnTime { get; set; }
    }
}