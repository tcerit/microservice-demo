using System;
using System.Linq.Expressions;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public class BaseRepository<T>: IRepository<T> where T : Entity
    {
        private readonly DataContext _context;
        private readonly DbSet<T> _entities;

        public BaseRepository(DataContext context)
        {
            _context = context;
            _entities = _context.Set<T>();
        }

        public async Task AddAsync(T entity)
        {
            await _entities.AddAsync(entity);
            await Save();
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

        public async Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate)
        {
            return await _context.Set<T>().Where(predicate).ToListAsync();
        }

        public async Task<T?> FindAsync(Expression<Func<T, bool>> match)
        {
            return await _context.Set<T>().SingleOrDefaultAsync(match);
        }

        public IQueryable<T> GetAll()
        {
            return _entities.AsNoTracking();
        }

        public async Task<T> GetByIdAsync(Guid id)
        {
            return await _entities.FirstOrDefaultAsync(e => e.Id == id);
        }

        public IQueryable<T> GetAllIncluding(params Expression<Func<T, object>>[] includeProperties)
        {

            IQueryable<T> queryable = GetAll();
            foreach (Expression<Func<T, object>> includeProperty in includeProperties)
            {

                queryable = queryable.Include<T, object>(includeProperty);
            }

            return queryable;
        }

        public async Task UpdateAsync(T entity)
        {
            _entities.Update(entity);
            await Save();

        }

        protected async Task Save()
        {
            await _context.SaveChangesAsync();
        }
    }
}

