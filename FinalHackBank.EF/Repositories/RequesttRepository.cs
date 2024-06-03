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
    public class RequesttRepository : IRequesttRepository
    {
        protected ApplicationDbContext _context;
        public RequesttRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<Requestt>> GetAllAsync()
        {
            return await _context.Set<Requestt>().ToListAsync();
        }
        public async Task<Requestt> GetByIdAsync(int id)
        {
            return await _context.Set<Requestt>().FindAsync(id);
        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<Requestt>().FindAsync(id);
            return entity != null;
        }

        public async Task<Requestt> addAsync(Requestt entity)
        {
            await _context.Set<Requestt>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Requestt> UpdateAsync(Requestt entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;


        }

        public async Task<Requestt> DeleteAsync(Requestt entity)
        {
            _context.Set<Requestt>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public IQueryable<Requestt> GetQueryable()
        {
            return _context.Set<Requestt>().AsQueryable();
        }

        public async Task<Requestt> FirstOrDefaultAsync(Expression<Func<Requestt, bool>> predicate)
        {
            return await _context.Set<Requestt>().FirstOrDefaultAsync(predicate);
        }

        async Task<Requestt> IRequesttRepository.FindAsync(Expression<Func<Requestt, bool>> match)
        {
            return await _context.Set<Requestt>().SingleOrDefaultAsync(match);
        }


    }
}




