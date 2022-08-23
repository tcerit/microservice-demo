using System;
using System.Linq.Expressions;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class BaseRepository<T> : IRepository<T> where T : Entity
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _entities;

        public BaseRepository(DataContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }


        protected async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await Save();
        }

        public virtual Task<T?> GetByIdAsync(Guid id)
        {
            return Find(p => p.Id.Equals(id)).SingleOrDefaultAsync();
        }


        public async Task DeleteAsync(Guid id)
        {
            var entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _entities.Remove(entity);
                await Save();
            }
        }

        public async Task<List<T>> GetAll()
        {
             return await _entities.AsNoTracking().ToListAsync();
        }

        public virtual Task<T?> FindAsync(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate).SingleOrDefaultAsync();
        }

        public IQueryable<T> Find(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate); 
        }
        
        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await Save();

        }
        public async Task UpdateRangeAsync(List<T> entities)
        {
            _entities.UpdateRange(entities);
            await Save();
        }

        public IQueryable<T> FindById(Guid id)
        {
            return Find(p => p.Id.Equals(id));
        }

        public Task<List<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return _entities.Where(predicate).ToListAsync();
        }
    }
}