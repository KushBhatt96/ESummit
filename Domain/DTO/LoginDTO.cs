using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class LoginDTO
    {
        [Required]
        [MaxLength(255)]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
