using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;

namespace Turbo_Wheels.Models
{
    public enum FuelType
    {
        Petrol,
        Diesel,
        Electric,
        Hybrid
    }
    public enum TransmissionType
    {
        Manual,
        Automatic,
        SemiAutomatic
    }
    public enum VehicleType
    {
        Sedan,
        Hatchback,
        SUV,
        Coupe,
        Convertible,
        Van,
        Truck,
        Motorcycle,
        Other
    }

    public class Vehicle
    {
        [Key]
        public int VehicleID { get; set; }

        [Display(Name = "Type")]
        public VehicleType Type { get; set; }

        [Required(ErrorMessage = "Brand is required.")]
        [Display(Name = "Brand")]
        public string Brand { get; set; }

        [Required(ErrorMessage = "Model is required.")]
        [Display(Name = "Model")]
        public string Model { get; set; }

        [Display(Name = "Type of fuel")]
        public FuelType Fuel { get; set; }

        [Display(Name = "Transmission")]
        public TransmissionType Transmission { get; set; }

        [Range(1, int.MaxValue, ErrorMessage = "Price per day must be a positive number.")]
        [Display(Name = "Price per day in MKD denars")]
        public int PricePerDay { get; set; }

        [Range(1, 10, ErrorMessage = "Number of doors must be between 1 and 10.")]
        [Display(Name = "Number of doors")]
        public int Doors { get; set; }

        [Range(1, 20, ErrorMessage = "Number of seats must be between 1 and 20.")]
        [Display(Name = "Number of seats")]
        public int Seats { get; set; }

        [Display(Name = "Airbag")]
        public bool HasAirbag { get; set; }

        [Display(Name = "ABS")]
        public bool HasAbs { get; set; }

        [Display(Name = "ESP")]
        public bool HasEsp { get; set; }

        [Display(Name = "GPS")]
        public bool HasGps { get; set; }
        
        [Required(ErrorMessage = "Vehicle image URL is required.")]
        [Display(Name = "Photo")]
        public string ImageURL { get; set; }

        [NotMapped]
        [Display(Name = "Vehicle Image")]
        public HttpPostedFileBase ImageFile { get; set; }

        public virtual List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}