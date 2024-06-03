using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Dto
{
   public class CompanyCreationDto
    {
        public string Namee { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;

        public int StatusUserId { get; set; }
        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;
    }
}
