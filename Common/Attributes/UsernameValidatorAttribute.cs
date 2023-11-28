using System.ComponentModel.DataAnnotations;

namespace Common.Attributes
{
    public class UsernameValidatorAttribute: RegularExpressionAttribute
    {
        private const string RegexUsername = "^[A-Za-z0-9.]{8,50}$";
        private const string InvalidUsernameMessage = "Username must be between 8 and 50 characters and contain only letters," +
    "digits, and periods.";
        public UsernameValidatorAttribute(): base(RegexUsername)
        {
            ErrorMessage = InvalidUsernameMessage;
        }
    }
}
