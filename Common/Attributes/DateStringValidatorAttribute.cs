using System.ComponentModel.DataAnnotations;

namespace Common.Attributes
{
    public class DateStringValidatorAttribute : RegularExpressionAttribute
    {
        private const string RegexDateOfBirth = "^\\d{4}-\\d{2}-\\d{2}$";
        private const string InvalidDateOfBirthMessage = "Invalid date format. YYYY-MM-dd is required.";
        public DateStringValidatorAttribute(): base(RegexDateOfBirth)
        {
            ErrorMessage = InvalidDateOfBirthMessage;
        }
    }
}
