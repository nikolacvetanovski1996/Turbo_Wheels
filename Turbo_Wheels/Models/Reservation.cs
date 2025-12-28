using System;
using System.ComponentModel.DataAnnotations;

namespace Turbo_Wheels.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        [Required]
        [Display(Name = "Date and time of taking")]
        public DateTime PickupDateTime { get; set; }

        [Required]
        [Display(Name = "Place of taking")]
        public string PickupPlace { get; set; }

        [Required]
        [Display(Name = "Date and time of return")]
        public DateTime ReturnDateTime { get; set; }

        [Required]
        [Display(Name = "Place of return")]
        public string ReturnPlace { get; set; }

        [Required]
        [Display(Name = "Total price in MKD denars")]
        public int TotalPrice { get; set; }

        [Required]
        [Display(Name = "Name of Credit Card owner")]
        public string CreditCardOwner { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 4)]
        [Display(Name = "Last 4 digits of the Credit Card Number")]
        public string CreditCardLast4 { get; set; }

        [Required]
        [Display(Name = "Credit Card Expiry Date")]
        public string CreditCardExpiryDate { get; set; }

        [Required]
        public int VehicleID { get; set; }

        public Vehicle Vehicle { get; set; }

        [Required]
        public int UserID { get; set; }

        public User User { get; set; }
    }
}