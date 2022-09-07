using System;
using Core.Data;
using Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration
{
    public class RepositoryScopeFactory<TContext> : IRepositoryScopeFactory<TContext>
        where TContext : notnull, DataContext
    {

        private readonly IServiceScopeFactory _serviceScopeFactory;
        private IRepositortServiceScope _scope;

        public RepositoryScopeFactory(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            var scope = _serviceScopeFactory.CreateScope();
            scope.ServiceProvider.GetRequiredService<TContext>();
            _scope = new RepositoryServiceScope(scope);
            

        }

        public IRepositortServiceScope CreateScope() => _scope;
        


    }
    
}

