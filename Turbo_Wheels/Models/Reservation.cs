using System;
using System.ComponentModel.DataAnnotations;

namespace Turbo_Wheels.Models
{
    public enum OfficeLocation
    {
        [Display(Name = "Skopje airport")]
        SkopjeAirport,

        [Display(Name = "Karposh 2 office")]
        Karposh2Office,

        [Display(Name = "Kisela Voda office")]
        KiselaVodaOffice
    }

    public enum PaymentMethod
    {
        [Display(Name = "Credit card")]
        CreditCard,

        [Display(Name = "Pay on pickup")]
        PayOnPickup
    }

    public class Reservation
    {
        [Key]
        public int ReservationID { get; set; }

        [Display(Name = "Date and time of taking")]
        public DateTime PickupDateTime { get; set; }
        
        [Display(Name = "Place of taking")]
        public OfficeLocation PickupPlace { get; set; }

        [Display(Name = "Date and time of return")]
        public DateTime ReturnDateTime { get; set; }

        [Display(Name = "Place of return")]
        public OfficeLocation ReturnPlace { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Total price in MKD denars")]
        public int TotalPrice { get; set; }

        [Display(Name = "Payment method")]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(100)]
        [Display(Name = "Name of credit card owner")]
        public string CreditCardOwner { get; set; }

        [StringLength(4, MinimumLength = 4)]
        [RegularExpression(@"^\d{4}$")]
        [Display(Name = "Last 4 digits of the credit card number")]
        public string CreditCardLast4 { get; set; }

        [StringLength(5)]
        [RegularExpression(@"^(0[1-9]|1[0-2])\/\d{2}$")]
        [Display(Name = "Credit card expiry date")]
        public string CreditCardExpiryDate { get; set; }

        public int VehicleID { get; set; }

        public Vehicle Vehicle { get; set; }

        public int UserID { get; set; }

        public User User { get; set; }
    }
}