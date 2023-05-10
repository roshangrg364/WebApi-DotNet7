using CoreModule.DbContextConfig;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoreModule.Base
{
    public class BaseRepository<T> : BaseRepositoryInterface<T> where T : class
    {
        private readonly ApplicationDbContext _context;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task AddAsync(T entity)
        {
            await _context.AddAsync(entity);
        }

        public void Delete(T entity)
        {
            _context.Remove(entity);
        }

        public async Task<List<T>> GetAllAsync()
        {
          return  await _context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public IQueryable<T> GetQueryable()
        {
            return _context.Set<T>();
        }

        public void Update(T entity)
        {

            EntityEntry entry = _context.Entry<T>(entity);
            entry.State = EntityState.Modified;
        }
    }
}
