using Wilczura.Observability.Stock.Ports.Models;

namespace Wilczura.Observability.Stock.Ports.Services;

public interface IProductService
{
    Task<IEnumerable<ProductModel>> RefreshProductAsync(long productId, CancellationToken cancellationToken);
}
