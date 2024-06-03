using AutoMapper;
using FinalHackBank.API.Controllers;
using FinalHackBank.CORE.Models;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    public class StatusDemandController : BaseController<StatusDemand, StatusDemandDto>
    {
        public StatusDemandController(IMapper mapper, IBaseRepository<StatusDemand> repository)
        : base(mapper, repository)
        {
        }
    }
}
