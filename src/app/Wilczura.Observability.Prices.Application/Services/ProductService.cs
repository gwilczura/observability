using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Repositories;
using Wilczura.Observability.Prices.Ports.Services;

namespace Wilczura.Observability.Prices.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductSourceRepository _productSourceRepository;

    public ProductService(
        IProductRepository productRepository,
        IProductSourceRepository productSourceRepository)
    {
        _productRepository = productRepository;
        _productSourceRepository = productSourceRepository;
    }

    public async Task<IEnumerable<ProductModel>> RefreshProductAsync(long productId, CancellationToken cancellationToken)
    {
        var productFromSource = await _productSourceRepository.GetProductAsync(productId, cancellationToken);
        if(productFromSource != null)
        {
            return await _productRepository.UpsertAsync(productFromSource);
        }

        return [];
    }
}
