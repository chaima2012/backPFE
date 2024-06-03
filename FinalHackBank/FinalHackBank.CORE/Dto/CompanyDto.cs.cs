using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Dto
{
   public class CompanyDto
    {
        public int CompanyId { get; set; }

        public string PhoneNumber { get; set; } = null!;

        public string Address { get; set; } = null!;
    }
}
