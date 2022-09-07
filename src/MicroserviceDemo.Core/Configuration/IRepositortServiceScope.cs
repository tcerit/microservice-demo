using System;
using Core.Data;
using Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration
{
	public interface IRepositortServiceScope : IDisposable
	{
        IRepository<TEntity> GetRepository<TEntity>() where TEntity : notnull, Entity;
    }
}

