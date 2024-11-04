using Wilczura.Observability.Stock.Ports.Models;

namespace Wilczura.Observability.Stock.Ports.Repositories;

public interface IProductSourceRepository
{
    Task<ProductModel?> GetProductAsync(long productId, CancellationToken cancellationToken);
}
