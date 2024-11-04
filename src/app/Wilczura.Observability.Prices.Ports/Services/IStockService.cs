using Wilczura.Observability.Prices.Ports.Models;

namespace Wilczura.Observability.Prices.Ports.Services;

public interface IStockService
{
    Task<IEnumerable<StockItemModel>> RefreshStockItemAsync(long stockItemId, CancellationToken cancellationToken);
}
