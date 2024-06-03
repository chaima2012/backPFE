using FinalHackBank.CORE.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Dto
{
   public class RoleDto : IEntityWithName
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;
    }
}
