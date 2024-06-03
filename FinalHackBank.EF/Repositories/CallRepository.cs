using FinalHackBank.CORE.Interfaces;
using FinalHackBank.CORE.Models;
using Microsoft.AspNetCore.Cors;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.EF.Repositories
     
{
    
    public class CallRepository : ICallRepository
    {
        
        protected ApplicationDbContext _context;
        public CallRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<Call>> GetAllAsync()
        {
            return await _context.Set<Call>().ToListAsync();
        }
        public async Task<Call> GetByIdAsync(int id)
        {
            return await _context.Set<Call>().FindAsync(id);
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<Call>().FindAsync(id);
            return entity != null;
        }

        public async Task<Call> addAsync(Call entity)
        {
            await _context.Set<Call>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Call> UpdateAsync(Call entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;


        }

        public async Task<Call> DeleteAsync(Call entity)
        {
            _context.Set<Call>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public IQueryable<Call> GetQueryable()
        {
            return _context.Set<Call>().AsQueryable();
        }

        public async Task<Call> FirstOrDefaultAsync(Expression<Func<Call, bool>> predicate)
        {
            return await _context.Set<Call>().FirstOrDefaultAsync(predicate);
        }

        async Task<Call> ICallRepository.FindAsync(Expression<Func<Call, bool>> match)
        {
            return await _context.Set<Call>().SingleOrDefaultAsync(match);
        }


    }
}




