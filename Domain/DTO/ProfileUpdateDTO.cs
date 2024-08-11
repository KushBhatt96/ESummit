using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ProfileUpdateDTO
    {
        public string Property { get; set; }
        public string Value { get; set; }
        public string? OldValue { get; set; }
    }
}
