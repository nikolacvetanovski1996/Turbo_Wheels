using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turbo_Wheels.Models
{
    public class User
    {
        [Key]
        public int UserID { get; set; }

        [Required(ErrorMessage = "Username is required.")]
        [StringLength(100, ErrorMessage = "Username cannot exceed 100 characters.")]
        [Display(Name = "Username")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is required.")]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Admin")]
        public bool IsAdmin { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters.")]
        [Display(Name = "First name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters.")]
        [Display(Name = "Last name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required.")]
        [StringLength(100, ErrorMessage = "Email address cannot exceed 100 characters.")]
        [EmailAddress(ErrorMessage = "Invalid email address format.")]
        [Display(Name = "Email address")]
        public string Email { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        [Display(Name = "Address")]
        public string Address { get; set; }
        
        [StringLength(20, ErrorMessage = "Phone number cannot exceed 20 characters.")]
        [RegularExpression(@"^\+?[0-9\s()-]{8,20}$", ErrorMessage = "Invalid phone number format.")]
        [Display(Name = "Phone number")]
        public string Phone { get; set; }

        public virtual List<Reservation> Reservations { get; set; } = new List<Reservation>();
    }
}