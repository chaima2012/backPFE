using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.CORE
{
    public interface IUnitOfWork: IDisposable
    {

        IBidRepository bid { get; }
        IEmployeeRepository employee { get; }
        IRequesttRepository requestt { get; }
        ICallRepository call { get; }
        IDocumenttRepository documentt { get; }
        ICompanyRepository company { get; }
        IUserRepository user { get; }


        IBaseRepository<Decision> decision { get; }
        IBaseRepository<Status> status { get; }
        IBaseRepository<StatusDemand> statusdemand { get; }
        IBaseRepository<Department> department { get; }
        IBaseRepository<StatusUser> statususer { get; }
        IBaseRepository<Role> role { get; }

        Task<int> CompleteAsync();
    }
}

