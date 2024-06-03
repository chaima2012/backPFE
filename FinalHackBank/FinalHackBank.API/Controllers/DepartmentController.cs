using AutoMapper;
using FinalHackBank.API.Controllers;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.API.Controllers
{
    public class DepartmentController : BaseController<Department, DepartmentDto>
    {
        public DepartmentController(IMapper mapper, IBaseRepository<Department> repository)
        : base(mapper, repository)
        {
        }
    }
}
