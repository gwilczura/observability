using Wilczura.Observability.Bff.Ports.Models;
using Wilczura.Observability.Bff.Ports.Repositories;
using Wilczura.Observability.Products.Contract;
using Wilczura.Observability.Products.Contract.Models;

namespace Wilczura.Observability.Bff.Adapters.Products.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly IProductsHttpClient _productHttpClient;

    public ProductRepository(IProductsHttpClient productHttpClient)
    {
        _productHttpClient = productHttpClient;
    }

    public async Task<IEnumerable<ProductModel>> GetProductsAsync(long? productId, CancellationToken cancellationToken)
    {
        var products = await _productHttpClient.GetProductsAsync(productId, cancellationToken);
        return products.Select(p => new ProductModel
        {
            ProductId = p.ProductId,
            Name = p.Name,
        }).ToArray();
    }

    public async Task<IEnumerable<ProductModel>> UpsertProductAsync(ProductModel model, CancellationToken cancellationToken)
    {
        var dto = new ProductDto
        {
            ProductId = model.ProductId,
            Name = model.Name,
        };
        var result = await _productHttpClient.UpsertProductAsync(dto, cancellationToken);
        return result.Select(p => new ProductModel
        {
            ProductId = p.ProductId,
            Name = p.Name,
        }).ToArray();
    }
}
