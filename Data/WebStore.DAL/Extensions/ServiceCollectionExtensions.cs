using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WebStore.DAL.Context;

namespace WebStore.DAL.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebstoreDBSqlServer(this IServiceCollection Services, string ConnectionString)
    {
        Services.AddDbContext<WebStoreDB>(opt => opt.UseSqlServer(ConnectionString));

        return Services;
    }
}
