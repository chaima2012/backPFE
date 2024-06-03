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
    public class UserRepository : IUserRepository
    {
        protected ApplicationDbContext _context;
        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<ICollection<User>> GetAllAsync()
        {
            return await _context.Set<User>().ToListAsync();
        }
        public async Task<User> GetByIdAsync(int id)
        {
            return await _context.Set<User>().FindAsync(id);

        }

        public async Task<bool> Exists(int id)
        {
            var entity = await _context.Set<User>().FindAsync(id);
            return entity != null;
        }

        public async Task<User> AddAsync(User entity)
        { 
            await _context.Set<User>().AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<User> UpdateAsync(User entity)
        {
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return entity;


        }

        public async Task<User> DeleteAsync(User entity)
        {
            _context.Set<User>().Remove(entity);
            await _context.SaveChangesAsync();
            return entity;
        }


        public IQueryable<User> GetQueryable()
        {
            return _context.Set<User>().AsQueryable();
        }

        public async Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate)
        {
            return await _context.Set<User>().FirstOrDefaultAsync(predicate);
        }

        public async Task<User> FindAsync(Expression<Func<User, bool>> match)
        {
            return await _context.Set<User>().SingleOrDefaultAsync(match);
        }

    }
}







