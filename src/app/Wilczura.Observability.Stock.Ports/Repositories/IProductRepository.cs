using Wilczura.Observability.Stock.Ports.Models;

namespace Wilczura.Observability.Stock.Ports.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<ProductModel>> UpsertAsync(ProductModel productModel);
}
