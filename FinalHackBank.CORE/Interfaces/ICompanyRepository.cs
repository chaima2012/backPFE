using FinalHackBank.CORE.Models;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Interfaces
{
    public interface ICompanyRepository
    {
        Task<Company> GetByIdAsync(int id);
        Task<ICollection<Company>> GetAllAsync();
        Task<Company> FindAsync(Expression<Func<Company, bool>> match);
        Task<bool> Exists(int id);
        Task<Company> addAsync(Company entity);
        Task<Company> UpdateAsync(Company entity);
        Task<Company> DeleteAsync(Company entity);
        IQueryable<Company> GetQueryable();
        Task<Company> FirstOrDefaultAsync(Expression<Func<Company, bool>> predicate);
    }
}
