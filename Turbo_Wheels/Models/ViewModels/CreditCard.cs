using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Turbo_Wheels.Models.ViewModels
{
    public class CreditCard
    {
        [Display(Name = "Credit Card Owner")]
        [Required]
        public string CreditCardOwner { get; set; }
        [Required]
        [Display(Name = "Credit Card Number")]
        public int CreditCardNumber { get; set; }
        public int month { get; set; }
        public int year { get; set; }
        [Required]
        [Display(Name = "CVC2 CVV2 Number")]
        public int CVC2_CVV2_Number { get; set; }
    }
}