using Microsoft.EntityFrameworkCore;
using WebStore.DAL.Context;
using WebStore.Data;
using WebStore.Services.Interfaces;

namespace WebStore.Services.InSQL;

public class DbInitializer : IDbInitializer
{
    private readonly WebStoreDB _db;
    private readonly ILogger<DbInitializer> _Logger;

    public DbInitializer(WebStoreDB db, ILogger<DbInitializer> Logger)
    {
        _db = db;
        _Logger = Logger;
    }

    public async Task<bool> RemoveAsync(CancellationToken Cancel = default)
    {
        _Logger.LogInformation("Удаление БД...");
        var removed = await _db.Database.EnsureDeletedAsync(Cancel).ConfigureAwait(false);

        if (removed)
            _Logger.LogInformation("БД удалена успешно");
        else
            _Logger.LogInformation("Удаление БД не требуется (отсутствует на сервере).");
        return removed;
    }

    public async Task InitializeAsync(bool RemoveBefore = false, CancellationToken Cancel = default)
    {
        _Logger.LogInformation("Инициализация БД..");

        if(RemoveBefore)
            await RemoveAsync(Cancel).ConfigureAwait(false);

        //await _db.Database.EnsureCreatedAsync(Cancel).ConfigureAwait(false);

        var pending_migrations = await _db.Database.GetPendingMigrationsAsync(Cancel).ConfigureAwait(false);
        if (pending_migrations.Any())
        {
            _Logger.LogInformation("Выполнение миграции БД...");
            await _db.Database.MigrateAsync(Cancel).ConfigureAwait(false);
            _Logger.LogInformation("Миграция БД выполнена успешно.");
        }
        else
            _Logger.LogInformation("Миграция БД не требуется");
        
        await InitializeProductsAsync(Cancel).ConfigureAwait(false);
        
        _Logger.LogInformation("Инициализация БД выполнена успешно.");

    }


    private async Task InitializeProductsAsync(CancellationToken Cancel)
    {
        if (await _db.Products.AnyAsync().ConfigureAwait(false))
        {
            _Logger.LogInformation("Инициализация БД тестовыми данными не требуется");
            return;
        }
        _Logger.LogInformation("Инициализация БД тестовыми данными...");

        _Logger.LogInformation("Добавлений секций в БД...");
        await using (var transaction = await _db.Database.BeginTransactionAsync(Cancel))
        {
            await _db.Sections.AddRangeAsync(TestData.Sections, Cancel).ConfigureAwait(false);

            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] ON", Cancel).ConfigureAwait(false);
            
            await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Sections] OFF", Cancel).ConfigureAwait(false);

            await transaction.CommitAsync(Cancel).ConfigureAwait(false);
        }

        _Logger.LogInformation("Добавлений брендов в БД...");
        await using (var transaction = await _db.Database.BeginTransactionAsync(Cancel))
        {
            await _db.Brands.AddRangeAsync(TestData.Brands, Cancel).ConfigureAwait(false);

            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] ON", Cancel).ConfigureAwait(false);

            await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Brands] OFF", Cancel).ConfigureAwait(false);

            await transaction.CommitAsync(Cancel).ConfigureAwait(false);
        }

        _Logger.LogInformation("Добавлений товаров в БД...");
        await using (var transaction = await _db.Database.BeginTransactionAsync(Cancel))
        {
            await _db.Products.AddRangeAsync(TestData.Products, Cancel).ConfigureAwait(false);

            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] ON", Cancel).ConfigureAwait(false);

            await _db.SaveChangesAsync(Cancel).ConfigureAwait(false);
            await _db.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT [dbo].[Products] OFF", Cancel).ConfigureAwait(false);

            await transaction.CommitAsync(Cancel).ConfigureAwait(false);
        }

        _Logger.LogInformation("Инициализация БД тестовыми данными выполнена успешно.");

    }


}
