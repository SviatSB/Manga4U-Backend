using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataInfrastructure.Interfaces
{
    public interface IRepository<T>
    {
        Task SaveChangesAsync();

        Task<T> AddAsync(T t);
        Task DeleteAsync(long id);
        Task<T?> GetAsync(long id);
        Task<IEnumerable<T>> GetAllAsync();
    }
}
