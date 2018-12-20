using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Turbo_Wheels.Models
{
    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }
        [Required]
        [Display(Name = "Date and time of taking")]
        public DateTime pickupDateTime { get; set; }
        [Required]
        [Display(Name = "Place of taking")]
        public string pickupPlace { get; set; }
        [Required]
        [Display(Name = "Date and time of return")]
        public DateTime returnDateTime { get; set; }
        [Required]
        [Display(Name = "Place of return")]
        public string returnPlace { get; set; }
        [Required]
        [Display(Name = "Total price in MKD denars")]
        public int totalPrice { get; set; }
        [Required]
        [Display(Name = "Name of Credit Card owner")]
        public string CreditCardOwner { get; set; }
        [Required]
        [Display(Name = "Credit Card Number")]
        public int CreditCardNumber { get; set; }
        [Required]
        [Display(Name = "Credit Card Expiration Date")]
        public DateTime CreditCardExpireDate { get; set; }
        [Required]
        [Display(Name = "CVC2 CVV2 Number")]
        public int CVC2_CVV2_Number { get; set; }
        [Required]
        public int vehicleID { get; set; }
        public Vehicle vehicle { get; set; }
        [Required]
        public int userID { get; set; }
        public User user { get; set; }
    }
}