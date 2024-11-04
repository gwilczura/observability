using Wilczura.Observability.Products.Adapters.Postgres.Repositories;
using Wilczura.Observability.Products.Adapters.ServiceBus.Publishers;
using Wilczura.Observability.Products.Application.Services;
using Wilczura.Observability.Products.Ports.Publishers;
using Wilczura.Observability.Products.Ports.Repositories;
using Wilczura.Observability.Products.Ports.Services;

namespace Wilczura.Observability.Products.Host.Extensions;

public static class ApplicationBuilderExtensions
{
    public static IHostApplicationBuilder AddProductsApplication(
        this IHostApplicationBuilder app)
    {
        app.Services.AddScoped<IProductRepository, ProductRepository>();
        app.Services.AddScoped<IProductService, ProductService>();
        app.Services.AddScoped<IProductPublisher, ProductPublisher>();
        return app;
    }
}
