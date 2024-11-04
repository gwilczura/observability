using MassTransit;
using Wilczura.Observability.Prices.Contract.Models;
using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Publishers;

namespace Wilczura.Observability.Prices.Adapters.ServiceBus.Publishers;

public class PricePublisher : IPricePublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public PricePublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishPriceChangedAsync(PriceModel price)
    {
        await _publishEndpoint.Publish(new PriceChanged
        {
            PriceId = price.PriceId,
            ProductId = price.ProductId,
        });
    }
}
