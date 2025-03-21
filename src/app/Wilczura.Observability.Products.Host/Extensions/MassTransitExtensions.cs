using MassTransit;
using Wilczura.Observability.Common.Host.Extensions;
using Wilczura.Observability.Common.ServiceBus;
using Wilczura.Observability.Products.Contract.Models;

namespace Wilczura.Observability.Products.Adapters.ServiceBus.Extensions;

public static class MassTransitExtensions
{
    public static IHostApplicationBuilder AddProductsServiceBus(
        this IHostApplicationBuilder app, string sectionName)
    {
        IConfiguration section = string.IsNullOrWhiteSpace(sectionName)
        ? app.Configuration
        : app.Configuration.GetSection(sectionName);
        app.Services.AddMassTransit(c =>
        {
            c.UsingAzureServiceBus((context, config) =>
            {
                config.Host(section.GetConnectionString("productsbus"));
                config.UseSendFilter(typeof(SendFilter<>), context);
                config.UsePublishFilter(typeof(PublishFilter<>), context);
                config.UseConsumeFilter(typeof(ConsumeFilter<>), context);
                config.DeployPublishTopology = true;
                config.Message<ProductChanged>(pt =>
                {
                    pt.SetEntityName("product-changed");
                });
                config.Publish<ProductChanged>(pt =>
                {
                });
                config.ConfigureEndpoints(context);
            });
        });
        return app;
    }
}
