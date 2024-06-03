using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.EF.Repositories
{
    public class EmployeeRepository : IEmployeeRepository
    {
        protected ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<Employee>> GetAllAsync()
        {
            return await _context.Set<Employee>().ToListAsync();
        }
        public async Task<Employee> GetByIdAsync(int id)
        {
            return await _context.Set<Employee>().FindAsync(id);
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<Employee>().FindAsync(id);
            return entity != null;
        }

        public async Task<Employee> addAsync(Employee entity)
        {
            await _context.Set<Employee>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Employee> UpdateAsync(Employee entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;


        }

        public async Task<Employee> DeleteAsync(Employee entity)
        {
            _context.Set<Employee>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public IQueryable<Employee> GetQueryable()
        {
            return _context.Set<Employee>().AsQueryable();
        }

        public async Task<Employee> FirstOrDefaultAsync(Expression<Func<Employee, bool>> predicate)
        {
            return await _context.Set<Employee>().FirstOrDefaultAsync(predicate);
        }

        async Task<Employee> IEmployeeRepository.FindAsync(Expression<Func<Employee, bool>> match)
        {
            return await _context.Set<Employee>().SingleOrDefaultAsync(match);
        }


    }
}




