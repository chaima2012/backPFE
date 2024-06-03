using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Interfaces
{
    public interface IBaseRepository <T> where T : class
    {

        Task<T> GetByIdAsync(int id);
        Task<ICollection<T>> GetAllAsync();
        Task<T> FindAsync(Expression<Func<T, bool>> match);
        Task<bool> Exists(int id);
        Task<T> addAsync(T entity);
        Task<T> UpdateAsync(T entity);
        Task<T> DeleteAsync(T entity);
        Task<T> GetByNameAsync<T>(string nom) where T : class, IEntityWithName;
        IQueryable<T> GetQueryable();
        Task<T> FirstOrDefaultAsync(Expression<Func<T, bool>> predicate);


    }
}
