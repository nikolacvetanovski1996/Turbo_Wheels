using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Turbo_Wheels.Models
{
    public class User
    {
        [Required]
        public int userID { get; set; }
        [Required]
        [Display(Name = "Username")]
        public string username { get; set; }
        [Required]
        [Display(Name = "Password")]
        [DataType(DataType.Password)]
        public string password { get; set; }
        [Display(Name = "Admin")]
        public bool isAdmin { get; set; }
        [Required]
        [Display(Name = "First Name")]
        public string firstName { get; set; }
        [Display(Name = "Last Name")]
        [Required]
        public string lastName { get; set; }
        [Display(Name = "Email Address")]
        [Required]
        public string email { get; set; }
        [Display(Name = "Address")]
        public string address { get; set; }
        [Display(Name = "Phone Number")]
        [DisplayFormat(DataFormatString = "{0:0##/###-###}")]
        public int phone { get; set; }
        public virtual List<Reservation> reservation { get; set; }
    }
}