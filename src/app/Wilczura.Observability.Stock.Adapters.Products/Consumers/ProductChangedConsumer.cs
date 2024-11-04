using MassTransit;
using Wilczura.Observability.Products.Contract.Models;
using Wilczura.Observability.Stock.Ports.Services;

namespace Wilczura.Observability.Stock.Adapters.Products.Consumers;

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
