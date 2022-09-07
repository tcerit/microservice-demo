using System;
using Core.Data;
using Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration
{
	public class RepositoryServiceScope:IRepositortServiceScope
	{

        private readonly IServiceScope _scope;

        public RepositoryServiceScope(IServiceScope scope) => _scope = scope;


        public IRepository<TEntity> GetRepository<TEntity>() where TEntity : Entity => _scope.ServiceProvider.GetRequiredService<IRepository<TEntity>>();


        private bool _disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool calledFromCodeNotTheGarbageCollector)
        {
            if (_disposed)
                return;
            if (calledFromCodeNotTheGarbageCollector)
            {
                // dispose of manged resources in here
                _scope?.Dispose();
            }
            _disposed = true;
        }

        ~RepositoryServiceScope() { Dispose(false); }
    }

        
    
}

