using MassTransit;
using Wilczura.Observability.Common.Host.Extensions;
using Wilczura.Observability.Common.ServiceBus;
using Wilczura.Observability.Products.Contract.Models;
using Wilczura.Observability.Stock.Adapters.Products.Consumers;
using Wilczura.Observability.Stock.Contract.Models;

namespace Wilczura.Observability.Stock.Host.Extensions;

public static class MassTransitExtensions
{
    public static IHostApplicationBuilder AddStockServiceBus(
        this IHostApplicationBuilder app, string sectionName)
    {
        IConfiguration section = string.IsNullOrWhiteSpace(sectionName)
        ? app.Configuration
        : app.Configuration.GetSection(sectionName);
        app.Services.AddMassTransit(c =>
        {
            c.AddConsumer<ProductChangedConsumer>();

            c.UsingAzureServiceBus((context, config) =>
            {
                config.Host(section.GetConnectionString("stockbus"));
                config.UseCustomConsumeLogger(context);
                config.UseSendFilter(typeof(SendFilter<>), context);
                config.UsePublishFilter(typeof(PublishFilter<>), context);
                config.UseConsumeFilter(typeof(ConsumeFilter<>), context);
                config.DeployPublishTopology = true;
                config.Message<StockChanged>(pt =>
                {
                    pt.SetEntityName("stock-changed");
                });
                config.Message<ProductChanged>(pt =>
                {
                    pt.SetEntityName("product-changed");
                });
                config.Publish<StockChanged>(pt =>
                {
                });
                config.SubscriptionEndpoint<ProductChanged>("stock-subscription", e =>
                {
                    e.ConfigureConsumer<ProductChangedConsumer>(context);
                });
                config.ConfigureEndpoints(context);
            });
        });

        return app;
    }
}
