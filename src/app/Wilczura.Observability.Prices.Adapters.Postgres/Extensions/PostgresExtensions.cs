using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;

namespace Wilczura.Observability.Prices.Adapters.Postgres.Extensions;

public static class PostgresExtensions
{
    public static IHostApplicationBuilder AddPricesPostgres(
        this IHostApplicationBuilder app, string sectionName)
    {
        IConfiguration section = string.IsNullOrWhiteSpace(sectionName)
        ? app.Configuration
        : app.Configuration.GetSection(sectionName);
        var connectionString = section.GetConnectionString("prices");
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
        var dataSource = dataSourceBuilder.Build();

        app.Services.AddDbContextPool<PricesContext>(opt =>
            opt.UseNpgsql(dataSource, x => x.MigrationsHistoryTable("__ef_migrations_history"))
                .UseSnakeCaseNamingConvention());

        return app;
    }
}