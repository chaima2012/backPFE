using FinalHackBank.CORE;
using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using FinalHackBank.EF.Repositories;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace FinalHackBank.EF
{
    public class UnitOfWork: IUnitOfWork
    {
        private readonly ApplicationDbContext _context;


        public IBidRepository bid { get; private set; }
        public IRequesttRepository requestt { get; private set; }
        public IEmployeeRepository employee { get; private set; }
        public IBaseRepository<Decision> decision { get; private set; }
        public ICallRepository call { get; private set; }
        public IDocumenttRepository documentt { get; private set; }
        public ICompanyRepository company { get; private set; }

        public IBaseRepository<StatusDemand> statusdemand { get; private set; }
        public IBaseRepository<Department> department { get; private set; }
        public IBaseRepository<Status> status { get; private set; }
        public IBaseRepository<StatusUser> statususer { get; private set; }


        public UnitOfWork(ApplicationDbContext context, 
                            IBidRepository bid, 
                            IRequesttRepository requestt, 
                            IEmployeeRepository employee , 
                            ICallRepository call , 
                            IDocumenttRepository documentt, 
                            ICompanyRepository company)

        {
            _context = context;
            this.bid = bid;
            this.requestt = requestt;
            this.employee = employee;
            this.call = call;
            this.company =company ;
            this.documentt = documentt;

            decision = new BaseRepository<Decision>(_context);
            statusdemand = new BaseRepository<StatusDemand>(_context);
            department = new BaseRepository<Department>(_context);
            status = new BaseRepository<Status>(_context);
            statususer = new BaseRepository<StatusUser>(_context);



        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
