using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Turbo_Wheels.Models
{
    public class Vehicle
    {
        [Required]
        public int vehicleID { get; set; }
        [Required]
        [Display(Name = "Type")]
        public string type { get; set; }
        [Required]
        [Display(Name = "Brand")]
        public string brand { get; set; }
        [Required]
        [Display(Name = "Model")]
        public string model { get; set; }
        [Required]
        [Display(Name = "Type of fuel")]
        public string fuel { get; set; }
        [Required]
        [Display(Name = "Transmission")]
        public string transmission { get; set; }
        [Required]
        [Display(Name = "Price per day in MKD denars")]
        public int price_per_day { get; set; }
        [Required]
        [Display(Name = "Number of doors")]
        public int doors { get; set; }
        [Required]
        [Display(Name = "Number of seats")]
        public int seats { get; set; }
        [Required]
        [Display(Name = "Airbag")]
        public bool has_airbag { get; set; }
        [Required]
        [Display(Name = "ABS")]
        public bool has_ABS { get; set; }
        [Required]
        [Display(Name = "ESP")]
        public bool has_ESP { get; set; }
        [Required]
        [Display(Name = "GPS")]
        public bool has_GPS { get; set; }
        [Display(Name = "Photo")]
        [Required]
        public string imageURL { get; set; }
        [NotMapped]
        public HttpPostedFileBase ImageFile { get; set; }
        public virtual List<Reservation> reservation { get; set; }
    }
}