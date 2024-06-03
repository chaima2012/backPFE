using FinalHackBank.CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Interfaces
{
    public interface IUserRepository

    {
        Task<User> GetByIdAsync(int id);
        Task<ICollection<User>> GetAllAsync();
        Task<User> FindAsync(Expression<Func<User, bool>> match);
        Task<bool> Exists(int id);
        Task<User> AddAsync(User entity);
        Task<User> UpdateAsync(User entity);
        Task<User> DeleteAsync(User entity);
        IQueryable<User> GetQueryable();
        Task<User> FirstOrDefaultAsync(Expression<Func<User, bool>> predicate);
    }
}
