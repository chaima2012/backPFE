using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Dto
{
    public class CallDto
    {
        public int CallId { get; set; }

        public string Descriptions { get; set; } = null!;

        public int StatusId { get; set; }

        public int Budget { get; set; }

        public string Remark { get; set; } = null!;
    }
}
