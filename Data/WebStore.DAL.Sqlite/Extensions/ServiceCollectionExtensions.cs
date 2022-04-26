using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebStore.DAL.Context;

namespace WebStore.DAL.Sqlite.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebStoreDBSqlite(this IServiceCollection Services, string ConnectionString)
    {
        Services.AddDbContext<WebStoreDB>(
            opt => opt.UseSqlite(
                ConnectionString, 
                o => o.MigrationsAssembly(typeof(ServiceCollectionExtensions).Assembly.GetName().ToString())));

        return Services;
    }
}
