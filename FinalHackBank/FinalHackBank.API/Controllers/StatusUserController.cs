using AutoMapper;
using FinalHackBank.API.Controllers;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    public class StatusUserController : BaseController<StatusUser, StatusUserDto>
    {
        public StatusUserController(IMapper mapper, IBaseRepository<StatusUser> repository)
        : base(mapper, repository)
        {
        }
    }
}
