using AutoMapper;
using Azure.Core;
using FinalHackBank.CORE.Dto;
using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.CORE.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {

            CreateMap<Bid, BidDto>();
            CreateMap<BidDto, Bid>();

            CreateMap<Role, RoleDto>();
            CreateMap<RoleDto, Role>();

            CreateMap<Status, StatusDto>();
            CreateMap<StatusDto, Status>();

            CreateMap<StatusUser, StatusUserDto>();
            CreateMap<StatusUserDto, StatusUser>();

            CreateMap<StatusDemand, StatusDemandDto>();
            CreateMap<StatusDemandDto, StatusDemand>();

            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<Request, RequesttDto>();
            CreateMap<RequesttDto, Request>();

            CreateMap<Company, CompanyDto>();
            CreateMap<CompanyDto, Company>();

            CreateMap<Employee, EmployeeDto>();
            CreateMap<EmployeeDto, Employee>();

            CreateMap<Call, CallDto>();
            CreateMap<CallDto, Call>();

            CreateMap<Decision, DecisionDto>();
            CreateMap<DecisionDto, Decision>();

            CreateMap<Documentt, DocumenttDto>();
            CreateMap<DocumenttDto, Documentt>();

            CreateMap<Department, DepartmentDto>();
            CreateMap<DepartmentDto, Department>();

        }
    }
}
