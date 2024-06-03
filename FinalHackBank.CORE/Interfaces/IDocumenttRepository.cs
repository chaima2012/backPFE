using FinalHackBank.CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Interfaces
{
    public interface IDocumenttRepository

    {
        Task<Documentt> GetByIdAsync(int id);
        Task<ICollection<Documentt>> GetAllAsync();
        Task<Documentt> FindAsync(Expression<Func<Documentt, bool>> match);
        Task<bool> Exists(int id);
        Task<Documentt> AddAsync(Documentt entity);
        Task<Documentt> UpdateAsync(Documentt entity);
        Task<Documentt> DeleteAsync(Documentt entity);
        IQueryable<Documentt> GetQueryable();
        Task<Documentt> FirstOrDefaultAsync(Expression<Func<Documentt, bool>> predicate);
    }
}
