using AutoMapper;
using FinalHackBank.API.Controllers;
using FinalHackBank.CORE.Models;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    public class RoleController : BaseController<Role, RoleDto>
    {
        public RoleController(IMapper mapper, IBaseRepository<Role> repository)
        : base(mapper, repository)
        {
        }
    }
}
