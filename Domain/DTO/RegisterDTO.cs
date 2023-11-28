using Common.Attributes;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTO
{
    public class RegisterDTO
    {

        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }

        [Required]
        [DateStringValidator]
        public string DateOfBirth { get; set; }

        [Required]
        [UsernameValidator]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }
    }
}
