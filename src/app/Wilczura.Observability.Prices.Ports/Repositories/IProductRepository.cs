using Wilczura.Observability.Prices.Ports.Models;

namespace Wilczura.Observability.Prices.Ports.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductModel>> UpsertAsync(ProductModel productModel);
}
