using Wilczura.Observability.Bff.Adapters.Prices.Repositories;
using Wilczura.Observability.Bff.Adapters.Products.Repositories;
using Wilczura.Observability.Bff.Adapters.Stock.Repositories;
using Wilczura.Observability.Bff.Application.Services;
using Wilczura.Observability.Bff.Ports.Repositories;
using Wilczura.Observability.Bff.Ports.Services;

namespace Wilczura.Observability.Bff.Host.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddBffApplication(
        this IHostApplicationBuilder app)
    {
        app.Services.AddScoped<IPriceRepository, PriceRepository>();
        app.Services.AddScoped<IProductRepository, ProductRepository>();
        app.Services.AddScoped<IStockRepository, StockRepository>();

        app.Services.AddScoped<IDemoService, DemoService>();

        return app;
    }
}
