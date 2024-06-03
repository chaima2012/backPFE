using FinalHackBank.CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Interfaces
{
    public interface IRequesttRepository
    {
        Task<Requestt> GetByIdAsync(int id);
        Task<ICollection<Requestt>> GetAllAsync();
        Task<Requestt> FindAsync(Expression<Func<Requestt, bool>> match);
        Task<bool> Exists(int id);
        Task<Requestt> addAsync(Requestt entity);
        Task<Requestt> UpdateAsync(Requestt entity);
        Task<Requestt> DeleteAsync(Requestt entity);
        IQueryable<Requestt> GetQueryable();
        Task<Requestt> FirstOrDefaultAsync(Expression<Func<Requestt, bool>> predicate);
    }
}
