using Wilczura.Observability.Products.Ports.Models;

namespace Wilczura.Observability.Products.Ports.Services;

public interface IProductService
{
    Task<IEnumerable<ProductModel>> GetAsync(long? productId);
    Task<IEnumerable<ProductModel>> UpsertAsync(ProductModel model);
}
