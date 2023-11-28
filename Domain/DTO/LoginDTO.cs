using System.ComponentModel.DataAnnotations;


namespace Domain.DTO
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(255)]
        public string UserName { get; set; }

        [Required]
        [MaxLength(255)]
        public string Password { get; set; }
    }
}
