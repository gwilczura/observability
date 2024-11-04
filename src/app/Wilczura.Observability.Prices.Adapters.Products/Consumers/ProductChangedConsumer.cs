using MassTransit;
using Wilczura.Observability.Prices.Ports.Services;
using Wilczura.Observability.Products.Contract.Models;

namespace Wilczura.Observability.Prices.Adapters.Products.Consumers;

public class ProductChangedConsumer : IConsumer<ProductChanged>
{
    private readonly IProductService _productService;

    public ProductChangedConsumer(
        IProductService productService)
    {
        _productService = productService;
    }

    public async Task Consume(ConsumeContext<ProductChanged> context)
    {
        await _productService.RefreshProductAsync(context.Message.ProductId, context.CancellationToken);
    }
}
