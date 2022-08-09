using System;
using System.Linq.Expressions;
using Core.Domain;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{
    public interface IRepository<T> where T : Entity
    {
        IQueryable<T> GetAll();

        Task<T> GetByIdAsync(Guid id);

        Task AddAsync(T entity);

        Task UpdateAsync(T entity);

        Task DeleteAsync(Guid id);

        Task<T?> FindAsync(Expression<Func<T, bool>> match);

        Task<ICollection<T>> FindAllAsync(Expression<Func<T, bool>> predicate);

        
    }
}

