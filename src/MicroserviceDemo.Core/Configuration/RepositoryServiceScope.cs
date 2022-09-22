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

        private bool disposedValue;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                }


                disposedValue = true;
            }
        }


        void IDisposable.Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }

        
    
}

