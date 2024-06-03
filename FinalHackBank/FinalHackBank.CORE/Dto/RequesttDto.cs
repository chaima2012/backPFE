using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Dto
{
    public class RequesttDto
    {

        public int RequestId { get; set; }

        public string Description { get; set; } = null!;

        public int EmployeeId { get; set; }

        public string Daterequest { get; set; } = null!;

        public int StatusId { get; set; }

        public int DecisionId { get; set; }
    }
}
