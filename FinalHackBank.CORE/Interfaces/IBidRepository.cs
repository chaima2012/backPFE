using FinalHackBank.CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Interfaces
{
    public interface IBidRepository
    {
        Task<ICollection<Bid>> GetAllAsync();

        Task<Bid>GetByIdAsync(int id);

        Task<Bid> FindAsync(Expression<Func<Bid, bool>> match);

        Task<bool> Exists(int id);

        Task<Bid> AddAsync(Bid entity); 

        Task<Bid> UpdateAsync(Bid entity);

        Task<Bid> DeleteAsync(Bid entity);

        Task<Bid> FirstOrDefaultAsync(Expression<Func<Bid, bool>> predicate);
    }
}
