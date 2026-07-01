using System;
using System.ComponentModel.DataAnnotations;

namespace Turbo_Wheels.Models
{
    public class ReservationViewModel
    {
        [Display(Name = "Place of taking")]
        public OfficeLocation PickupPlace { get; set; }

        [Display(Name = "Date and time of taking")]
        public DateTime PickupDateTime { get; set; }

        [Display(Name = "Place of return")]
        public OfficeLocation ReturnPlace { get; set; }

        [Display(Name = "Date and time of return")]
        public DateTime ReturnDateTime { get; set; }
    }
}