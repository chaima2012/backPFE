using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Interfaces
{
    public interface IEntityWithName
    {
        public int Id { get; set; }
        string Name { get; set; }
    }
}
