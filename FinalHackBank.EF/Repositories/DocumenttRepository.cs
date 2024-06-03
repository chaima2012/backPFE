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
    public class DocumenttRepository : IDocumenttRepository
    {
        protected ApplicationDbContext _context;
        public DocumenttRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<Documentt>> GetAllAsync()
        {
            return await _context.Set<Documentt>().ToListAsync();
        }
        public async Task<Documentt> GetByIdAsync(int id)
        {
            return await _context.Set<Documentt>().FindAsync(id);

        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<Documentt>().FindAsync(id);
            return entity != null;
        }

        public async Task<Documentt> AddAsync(Documentt entity)
        {
            await _context.Set<Documentt>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<Documentt> UpdateAsync(Documentt entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;


        }

        public async Task<Documentt> DeleteAsync(Documentt entity)
        {
            _context.Set<Documentt>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public IQueryable<Documentt> GetQueryable()
        {
            return _context.Set<Documentt>().AsQueryable();
        }

        public async Task<Documentt> FirstOrDefaultAsync(Expression<Func<Documentt, bool>> predicate)
        {
            return await _context.Set<Documentt>().FirstOrDefaultAsync(predicate);
        }

        public async Task<Documentt> FindAsync(Expression<Func<Documentt, bool>> match)
        {
            return await _context.Set<Documentt>().SingleOrDefaultAsync(match);
        }

    }
}







