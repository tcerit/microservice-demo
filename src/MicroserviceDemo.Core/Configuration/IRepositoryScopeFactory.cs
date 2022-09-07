using Core.Data;
using Core.Domain;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Configuration;

public interface IRepositoryScopeFactory<TContext>
    where TContext: notnull, DataContext

{
    IRepositortServiceScope CreateScope();
}

