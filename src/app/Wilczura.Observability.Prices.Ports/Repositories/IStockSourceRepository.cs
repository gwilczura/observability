using Wilczura.Observability.Prices.Ports.Models;

namespace Wilczura.Observability.Prices.Ports.Repositories;

public interface IStockSourceRepository
{
    Task<StockItemModel?> GetStockItemAsync(long stockItemId, CancellationToken cancellationToken);
}
