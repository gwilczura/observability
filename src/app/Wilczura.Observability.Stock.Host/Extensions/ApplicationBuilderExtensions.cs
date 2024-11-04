using Wilczura.Observability.Stock.Adapters.Postgres.Repositories;
using Wilczura.Observability.Stock.Adapters.Products.Consumers;
using Wilczura.Observability.Stock.Adapters.Products.Repositories;
using Wilczura.Observability.Stock.Adapters.ServiceBus.Publishers;
using Wilczura.Observability.Stock.Application.Services;
using Wilczura.Observability.Stock.Ports.Publishers;
using Wilczura.Observability.Stock.Ports.Repositories;
using Wilczura.Observability.Stock.Ports.Services;

namespace Wilczura.Observability.Stock.Host.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddStockApplication(
        this IHostApplicationBuilder app)
    {
        app.Services.AddScoped<IStockRepository, StockRepository>();
        app.Services.AddScoped<IStockService, StockService>();
        app.Services.AddScoped<IStockPublisher, StockPublisher>();

        app.Services.AddScoped<IProductSourceRepository, ProductSourceRepository>();
        app.Services.AddScoped<IProductRepository, ProductRepository>();
        app.Services.AddScoped<IProductService, ProductService>();
        app.Services.AddScoped<ProductChangedConsumer>();
        return app;
    }
}
