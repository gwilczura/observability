using Wilczura.Observability.Prices.Ports.Models;

namespace Wilczura.Observability.Prices.Ports.Services;

public interface IProductService
{
    Task<IEnumerable<ProductModel>> RefreshProductAsync(long productId, CancellationToken cancellationToken);
}
