using Wilczura.Observability.Products.Ports.Models;
using Wilczura.Observability.Products.Ports.Publishers;
using Wilczura.Observability.Products.Ports.Repositories;
using Wilczura.Observability.Products.Ports.Services;

namespace Wilczura.Observability.Products.Application.Services;

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly IProductPublisher _productPublisher;

    public ProductService(
        IProductRepository productRepository,
        IProductPublisher productPublisher)
    {
        _productRepository = productRepository;
        _productPublisher = productPublisher;
    }

    public async Task<IEnumerable<ProductModel>> GetAsync(long? productId)
    {
        return await _productRepository.GetAsync(productId);
    }

    public async Task<IEnumerable<ProductModel>> UpsertAsync(ProductModel model)
    {
        var result = await _productRepository.UpsertAsync(model);
        await _productPublisher.PublishProductChangedAsync(result.First());
        return result;
    }
}
