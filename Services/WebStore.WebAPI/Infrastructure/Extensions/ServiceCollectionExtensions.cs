using WebStore.DAL.Extensions;
using WebStore.DAL.Sqlite.Extensions;
using WebStore.Interfaces.Services;
using WebStore.Services.Services.InSQL;

namespace WebStore.WebAPI.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddWebStoreDB(this IServiceCollection Services, IConfiguration Configuration)
    {
        var db_connection_string_name = Configuration["Database"];
        var db_connection_string = Configuration.GetConnectionString(db_connection_string_name);

        switch (db_connection_string_name)
        {
            default:
                throw new InvalidOperationException($"База данных формата {db_connection_string_name} не поддерживается.");
            case "SqlServer":
            case "DockerDB":
                return Services
                    .AddWebstoreDBSqlServer(db_connection_string)
                    .AddTransient<IDbInitializer, DbInitializer>();


            case "Sqlite":
                return Services
                    .AddWebStoreDBSqlite(db_connection_string)
                    .AddTransient<IDbInitializer, DbInitializer>();
        }
    }
}
