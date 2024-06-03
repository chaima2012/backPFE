using FinalHackBank.CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Interfaces
{
    public interface ICallRepository
    {
        Task<Call> GetByIdAsync(int id);
        Task<ICollection<Call>> GetAllAsync();
        Task<Call> FindAsync(Expression<Func<Call, bool>> match);
        Task<bool> Exists(int id);
        Task<Call> addAsync(Call entity);
        Task<Call> UpdateAsync(Call entity);
        Task<Call> DeleteAsync(Call entity);
        IQueryable<Call> GetQueryable();
        Task<Call> FirstOrDefaultAsync(Expression<Func<Call, bool>> predicate);
    }
}

