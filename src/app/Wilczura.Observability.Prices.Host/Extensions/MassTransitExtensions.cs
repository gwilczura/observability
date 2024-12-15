using MassTransit;
using Wilczura.Observability.Common.Host.Extensions;
using Wilczura.Observability.Common.ServiceBus;
using Wilczura.Observability.Prices.Adapters.Products.Consumers;
using Wilczura.Observability.Prices.Adapters.Stock.Consumers;
using Wilczura.Observability.Prices.Contract.Models;
using Wilczura.Observability.Products.Contract.Models;
using Wilczura.Observability.Stock.Contract.Models;

namespace Wilczura.Observability.Prices.Host.Extensions;

public static class MassTransitExtensions
{
    public static IHostApplicationBuilder AddPricesServiceBus(
        this IHostApplicationBuilder app, string sectionName)
    {
        IConfiguration section = string.IsNullOrWhiteSpace(sectionName)
        ? app.Configuration
        : app.Configuration.GetSection(sectionName);
        app.Services.AddMassTransit(c =>
        {
            c.AddConsumer<ProductChangedConsumer>();
            c.AddConsumer<StockChangedConsumer>();

            c.UsingAzureServiceBus((context, config) =>
            {
                config.Host(section.GetConnectionString("pricesbus"));
                config.UseCustomConsumeLogger(context);
                config.UseSendFilter(typeof(SendFilter<>), context);
                config.UsePublishFilter(typeof(PublishFilter<>), context);
                config.UseConsumeFilter(typeof(ConsumeFilter<>), context);
                config.DeployPublishTopology = true;
                config.Message<PriceChanged>(pt =>
                {
                    pt.SetEntityName("price-changed");
                });
                config.Message<ProductChanged>(pt =>
                {
                    pt.SetEntityName("product-changed");
                });
                config.Message<StockChanged>(pt =>
                {
                    pt.SetEntityName("stock-changed");
                });
                config.Publish<PriceChanged>(pt =>
                {
                });
                config.SubscriptionEndpoint<ProductChanged>("prices-subscription", e =>
                {
                    e.ConfigureConsumer<ProductChangedConsumer>(context);
                });
                config.SubscriptionEndpoint<StockChanged>("prices-subscription", e =>
                {
                    e.ConfigureConsumer<StockChangedConsumer>(context);
                });
                config.ConfigureEndpoints(context);
            });
        });

        return app;
    }
}
