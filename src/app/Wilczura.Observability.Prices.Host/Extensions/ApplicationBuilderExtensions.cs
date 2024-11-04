using Wilczura.Observability.Prices.Adapters.Postgres.Repositories;
using Wilczura.Observability.Prices.Adapters.Products.Consumers;
using Wilczura.Observability.Prices.Adapters.Products.Repositories;
using Wilczura.Observability.Prices.Adapters.ServiceBus.Publishers;
using Wilczura.Observability.Prices.Adapters.Stock.Consumers;
using Wilczura.Observability.Prices.Adapters.Stock.Repositories;
using Wilczura.Observability.Prices.Application.Services;
using Wilczura.Observability.Prices.Ports.Publishers;
using Wilczura.Observability.Prices.Ports.Repositories;
using Wilczura.Observability.Prices.Ports.Services;

namespace Wilczura.Observability.Prices.Host.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddPricesApplication(
        this IHostApplicationBuilder app)
    {
        app.Services.AddScoped<IPriceRepository, PriceRepository>();
        app.Services.AddScoped<IPriceService, PriceService>();
        app.Services.AddScoped<IPricePublisher, PricePublisher>();

        app.Services.AddScoped<IProductSourceRepository, ProductSourceRepository>();
        app.Services.AddScoped<IProductRepository, ProductRepository>();
        app.Services.AddScoped<IProductService, ProductService>();
        app.Services.AddScoped<ProductChangedConsumer>();

        app.Services.AddScoped<IStockSourceRepository, StockSourceRepository>();
        app.Services.AddScoped<IStockRepository, StockRepository>();
        app.Services.AddScoped<IStockService, StockService>();
        app.Services.AddScoped<StockChangedConsumer>();
        return app;
    }
}
