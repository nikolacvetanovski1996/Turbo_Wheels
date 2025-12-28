using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turbo_Wheels.Models.ViewModels
{
    public class CreditCardViewModel : IValidatableObject
    {
        [Display(Name = "Credit Card Owner")]
        [Required]
        public string CreditCardOwner { get; set; }

        [Required]
        [CreditCard(ErrorMessage = "Invalid credit card number")]
        [StringLength(19, MinimumLength = 13)]
        [Display(Name = "Credit Card Number")]
        public string CreditCardNumber { get; set; }

        [Required]
        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        public int ExpireMonth { get; set; }

        [Required]
        public int ExpireYear { get; set; }

        [Required]
        [StringLength(4, MinimumLength = 3)]
        [Display(Name = "CVC2 CVV2 Number")]
        public string Cvc2Cvv2Number { get; set; }

        // Custom validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (ExpireMonth < 1 || ExpireMonth > 12)
                yield return new ValidationResult("Month must be between 1 and 12.", new[] { nameof(ExpireMonth) });

            var now = DateTime.UtcNow;
            var currentTwoDigitYear = now.Year % 100;
            var inputYear = ExpireYear;

            if (inputYear < currentTwoDigitYear || inputYear > 99)
                yield return new ValidationResult($"Year must be between {currentTwoDigitYear} and 99.", new[] { nameof(ExpireYear) });

            int currentCentury = now.Year / 100 * 100;
            int fullYear = currentCentury + inputYear;
            if (fullYear < now.Year)
                fullYear += 100; // handle century rollover

            ValidationResult dateError = null;
            try
            {
                var expiryDate = new DateTime(fullYear, ExpireMonth, DateTime.DaysInMonth(fullYear, ExpireMonth), 23, 59, 59);
                if (expiryDate < now)
                    dateError = new ValidationResult("Credit card expiration date must be in the future.", new[] { nameof(ExpireMonth), nameof(ExpireYear) });
            }
            catch
            {
                dateError = new ValidationResult("Invalid expiration date.", new[] { nameof(ExpireMonth), nameof(ExpireYear) });
            }

            if (dateError != null)
                yield return dateError;
        }
    }
}