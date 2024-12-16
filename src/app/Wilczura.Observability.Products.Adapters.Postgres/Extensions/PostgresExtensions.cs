using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Npgsql;

namespace Wilczura.Observability.Products.Adapters.Postgres.Extensions;

public static class PostgresExtensions
{
    public static IHostApplicationBuilder AddProductsPostgres(
        this IHostApplicationBuilder app, string sectionName, ILogger logger)
    {
        IConfiguration section = string.IsNullOrWhiteSpace(sectionName)
        ? app.Configuration
        : app.Configuration.GetSection(sectionName);
        var connectionString = section.GetConnectionString("products");
        if (string.IsNullOrEmpty(connectionString))
        {
            logger?.LogError($"Connection string 'products' is empty");
        }
        else
        {
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            var dataSource = dataSourceBuilder.Build();

            app.Services.AddDbContextPool<ProductsContext>(opt =>
                opt.UseNpgsql(dataSource, x => x.MigrationsHistoryTable("__ef_migrations_history"))
                    .UseSnakeCaseNamingConvention());
        }

        return app;
    }
}
