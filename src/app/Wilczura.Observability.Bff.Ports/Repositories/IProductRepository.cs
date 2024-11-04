using Wilczura.Observability.Bff.Ports.Models;

namespace Wilczura.Observability.Bff.Ports.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductModel>> GetProductsAsync(long? productId, CancellationToken cancellationToken);
    Task<IEnumerable<ProductModel>> UpsertProductAsync(ProductModel model, CancellationToken cancellationToken);
}
