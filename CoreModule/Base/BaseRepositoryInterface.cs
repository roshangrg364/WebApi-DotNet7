using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Base
{
    public interface BaseRepositoryInterface<T> where T: class
    {
        Task<List<T>> GetAllAsync();
        IQueryable<T> GetQueryable();
        Task AddAsync(T entity);
        void Update(T entity);
        Task<T?> GetByIdAsync(int id);
        void Delete(T entity);
    }
}
