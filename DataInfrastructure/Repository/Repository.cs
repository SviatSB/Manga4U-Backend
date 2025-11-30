using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DataInfrastructure.Interfaces;

using Domain.Models;

using Microsoft.EntityFrameworkCore;

namespace DataInfrastructure.Repository
{
    public class Repository<T> : IRepository<T> where T : EntityBase
    {
        protected readonly MyDbContext _myDbContext;
        protected readonly DbSet<T> _set;

        public Repository (MyDbContext myDbContext)
        {
            _myDbContext = myDbContext;
            _set = _myDbContext.Set<T>();
        }

        public async Task<T> AddAsync(T t)
        {
            await _set.AddAsync(t);
            await _myDbContext.SaveChangesAsync();
            return t;
        }

        public async Task DeleteAsync(long id)
        {
            var entity = await _set.FindAsync(id);
            if (entity is null) return;
            _set.Remove(entity);
            await _myDbContext.SaveChangesAsync();
        }

        public async Task<T?> GetAsync(long id)
        {
            //можно было бы тут использовать asnotracking, но есть шанс получить пролемы
            return await _set.FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<IEnumerable<T>> GetAllAsync()
        {
            return await _set.ToListAsync();
        }

        public Task SaveChangesAsync() => _myDbContext.SaveChangesAsync();

    }
}
