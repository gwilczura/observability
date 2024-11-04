using MassTransit;
using Wilczura.Observability.Stock.Contract.Models;
using Wilczura.Observability.Stock.Ports.Models;
using Wilczura.Observability.Stock.Ports.Publishers;

namespace Wilczura.Observability.Stock.Adapters.ServiceBus.Publishers;

public class StockPublisher : IStockPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public StockPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishStockChangedAsync(StockItemModel stockItem)
    {
        await _publishEndpoint.Publish(new StockChanged
        {
            StockItemId = stockItem.StockItemId,
            ProductId = stockItem.ProductId,
        });
    }
}
