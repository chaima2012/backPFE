using AutoMapper;
using FinalHackBank.API.Controllers;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    public class DecisionController : BaseController<Decision, DecisionDto>
    {
        public DecisionController(IMapper mapper, IBaseRepository<Decision> repository)
        : base(mapper, repository)
        {




        }
    }
}
