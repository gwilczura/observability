using MassTransit;
using Wilczura.Observability.Products.Contract.Models;
using Wilczura.Observability.Products.Ports.Models;
using Wilczura.Observability.Products.Ports.Publishers;

namespace Wilczura.Observability.Products.Adapters.ServiceBus.Publishers;

public class ProductPublisher : IProductPublisher
{
    private readonly IPublishEndpoint _publishEndpoint;

    public ProductPublisher(IPublishEndpoint publishEndpoint)
    {
        _publishEndpoint = publishEndpoint;
    }

    public async Task PublishProductChangedAsync(ProductModel product)
    {
        await _publishEndpoint.Publish(new ProductChanged
        {
            ProductId = product.ProductId,
        });
    }
}
