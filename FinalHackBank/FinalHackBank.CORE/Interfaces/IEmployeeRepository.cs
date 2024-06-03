using FinalHackBank.CORE.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace FinalHackBank.CORE.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<ICollection<Employee>> GetAllAsync();

        Task<Employee> GetByIdAsync(int id);

        Task<Employee> FindAsync(Expression<Func<Employee, bool>> match);

        Task<bool> Exists(int id);

        Task<Employee> addAsync(Employee entity);

        Task<Employee> UpdateAsync(Employee entity);

        Task<Employee> DeleteAsync(Employee entity);

        Task<Employee> FirstOrDefaultAsync(Expression<Func<Employee, bool>> predicate);
    }
}
