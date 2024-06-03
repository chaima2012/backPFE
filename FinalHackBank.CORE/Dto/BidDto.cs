using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Dto
{
    public class BidDto
    {
        public int BidId { get; set; }

        public int CompanyId { get; set; }

        public int DocumentId { get; set; }

        public double AmountTtc { get; set; }

        public int? Winner { get; set; }

        public int Idcall { get; set; }

    }
}
