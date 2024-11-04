using Wilczura.Observability.Prices.Ports.Models;

namespace Wilczura.Observability.Prices.Ports.Repositories;

public interface IProductSourceRepository
{
    Task<ProductModel?> GetProductAsync(long productId, CancellationToken cancellationToken);
}
