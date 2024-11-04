using Wilczura.Observability.Products.Ports.Models;

namespace Wilczura.Observability.Products.Ports.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductModel>> GetAsync(long? productId);
    Task<IEnumerable<ProductModel>> UpsertAsync(ProductModel model);
}
