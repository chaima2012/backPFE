using AutoMapper;
using FinalHackBank.API.Controllers;
using FinalHackBank.CORE.Models;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    public class StatusController : BaseController<Status, StatusDto>
    {
        public StatusController(IMapper mapper, IBaseRepository<Status> repository)
        : base(mapper, repository)
        {
        }
    }
}
