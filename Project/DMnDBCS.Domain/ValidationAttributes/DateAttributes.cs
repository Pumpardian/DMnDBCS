using System.ComponentModel.DataAnnotations;

namespace DMnDBCS.Domain.ValidationAttributes
{
    public class DateNotAfterToday : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly date)
            {
                if (date > DateOnly.FromDateTime(DateTime.Today))
                {
                    return new ValidationResult("Date cannot be after today");
                }
            }

            return ValidationResult.Success;
        }
    }

    public class DateLessThanOrEqualAttribute(string comparisonProperty) : ValidationAttribute
    {
        private readonly string _comparisonProperty = comparisonProperty;

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value is DateOnly currentDate)
            {
                var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
                if (property == null)
                {
                    return ValidationResult.Success;
                }

                var comparisonValue = property.GetValue(validationContext.ObjectInstance);
                if (comparisonValue is DateOnly comparisonDate)
                {
                    if (currentDate > comparisonDate)
                    {
                        return new ValidationResult($"{validationContext.DisplayName} must be less than or equal to {_comparisonProperty}.");
                    }
                }
            }
            return ValidationResult.Success;
        }
    }
}
