using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Repositories;
using Wilczura.Observability.Products.Contract;

namespace Wilczura.Observability.Prices.Adapters.Products.Repositories;

public class ProductSourceRepository : IProductSourceRepository
{
    private readonly IProductsHttpClient _productsHttpClient;

    public ProductSourceRepository(IProductsHttpClient productsHttpClient)
    {
        _productsHttpClient = productsHttpClient;
    }

    public async Task<ProductModel?> GetProductAsync(long productId, CancellationToken cancellationToken)
    {
        var result = await _productsHttpClient.GetProductsAsync(productId, cancellationToken);
        return result.Select(a => new ProductModel
        {
            Name = a.Name,
            ProductId = a.ProductId
        }).FirstOrDefault();
    }
}
