using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Turbo_Wheels.Models.ViewModels
{
    public class CreditCardViewModel : IValidatableObject
    {
        [Required(ErrorMessage = "Please select a payment method.")]
        public bool? PayOnPickup { get; set; }

        [StringLength(100, ErrorMessage = "Credit card owner's name cannot exceed 100 characters.")]
        [Display(Name = "Credit card owner")]
        public string CreditCardOwner { get; set; }

        [CreditCard(ErrorMessage = "Invalid credit card number")]
        [RegularExpression(@"^[0-9 ]+$", ErrorMessage = "Credit card number must contain only digits and spaces.")]
        [Display(Name = "Credit card number")]
        public string CreditCardNumber { get; set; }

        [Range(1, 12, ErrorMessage = "Month must be between 1 and 12")]
        [Display(Name = "Expiration month")]
        public int? ExpireMonth { get; set; }

        [Range(0, 99, ErrorMessage = "Expiration year must be between 0 and 99.")]
        [Display(Name = "Expiration year")]
        public int? ExpireYear { get; set; }

        [StringLength(4, MinimumLength = 3)]
        [RegularExpression(@"^\d{3,4}$", ErrorMessage = "CVC2/CVV2 must contain 3 or 4 digits.")]
        [Display(Name = "CVC2 CVV2 number")]
        public string Cvc2Cvv2Number { get; set; }

        // Custom validation
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            // Skip credit card validation if user chooses to pay on pickup
            if (PayOnPickup == true)
                yield break;

            if (string.IsNullOrWhiteSpace(CreditCardOwner))
                yield return new ValidationResult(
                    "Credit card owner's name is required.",
                    new[] { nameof(CreditCardOwner) });

            if (string.IsNullOrWhiteSpace(CreditCardNumber))
                yield return new ValidationResult(
                    "Credit card number is required.",
                    new[] { nameof(CreditCardNumber) });

            var expireMonthHasRequiredError = false;
            if (!ExpireMonth.HasValue)
            {
                expireMonthHasRequiredError = true;
                yield return new ValidationResult(
                    "Expiration month is required.",
                    new[] { nameof(ExpireMonth) });
            }

            var expireYearHasRequiredError = false;
            if (!ExpireYear.HasValue)
            {
                expireYearHasRequiredError = true;
                yield return new ValidationResult(
                    "Expiration year is required.",
                    new[] { nameof(ExpireYear) });
            }

            if (string.IsNullOrWhiteSpace(Cvc2Cvv2Number))
                yield return new ValidationResult(
                    "CVC2/CVV2 number is required.",
                    new[] { nameof(Cvc2Cvv2Number) });

            var now = DateTime.UtcNow;
            var currentTwoDigitYear = now.Year % 100;

            int inputMonth = ExpireMonth ?? 0;
            int inputYear = ExpireYear ?? 0;
            
            if ((inputMonth < 1 || inputMonth > 12) && !expireMonthHasRequiredError)
                yield return new ValidationResult("Month must be between 1 and 12.", new[] { nameof(ExpireMonth) });

            if ((inputYear < currentTwoDigitYear || inputYear > 99) && !expireYearHasRequiredError)
                yield return new ValidationResult($"Year must be between {currentTwoDigitYear} and 99.", new[] { nameof(ExpireYear) });

            int currentCentury = now.Year / 100 * 100;
            int fullYear = currentCentury + inputYear;
            if (fullYear < now.Year)
                fullYear += 100; // handle century rollover

            if (!expireMonthHasRequiredError && !expireYearHasRequiredError &&
                inputMonth >= 1 && inputMonth <= 12 &&
                inputYear >= currentTwoDigitYear && inputYear <= 99)
            {
                ValidationResult dateError = null;
                try
                {
                    var expiryDate = new DateTime(fullYear, inputMonth, DateTime.DaysInMonth(fullYear, inputMonth), 23, 59, 59);
                    if (expiryDate < now)
                        dateError = new ValidationResult("Credit card expiration date must be in the future.", new[] { nameof(ExpireMonth) });
                }
                catch (ArgumentOutOfRangeException)
                {
                    dateError = new ValidationResult("Invalid expiration date.", new[] { nameof(ExpireMonth) });
                }

                if (dateError != null)
                    yield return dateError;
            }
        }
    }
}