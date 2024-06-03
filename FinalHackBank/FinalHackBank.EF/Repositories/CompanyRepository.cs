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
    public class CompanyRepository : ICompanyRepository
    {
        protected ApplicationDbContext _context;
        public CompanyRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<Company>> GetAllAsync()
        {
            return await _context.Set<Company>().ToListAsync();
        }
        public async Task<Company> GetByIdAsync(int id)
        {
            return await _context.Set<Company>().FindAsync(id);

        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<Company>().FindAsync(id);
            return entity != null;
        }

        public async Task<Company> addAsync(Company entity)
        {
            await _context.Set<Company>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Company> UpdateAsync(Company entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;


        }

        public async Task<Company> DeleteAsync(Company entity)
        {
            _context.Set<Company>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public IQueryable<Company> GetQueryable()
        {
            return _context.Set<Company>().AsQueryable();
        }

        public async Task<Company> FirstOrDefaultAsync(Expression<Func<Company, bool>> predicate)
        {
            return await _context.Set<Company>().FirstOrDefaultAsync(predicate);
        }

        public async Task<Company> FindAsync(Expression<Func<Company, bool>> match)
        {
            return await _context.Set<Company>().SingleOrDefaultAsync(match);
        }

    }
}







