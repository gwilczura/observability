using Wilczura.Observability.Bff.Ports.Models;

namespace Wilczura.Observability.Bff.Ports.Repositories;

public interface IStockRepository
{
    Task<IEnumerable<StockItemModel>> GetStockAsync(long? stockItemId, CancellationToken cancellationToken);
    Task<IEnumerable<StockItemModel>> UpsertStockItemAsync(StockItemModel model, CancellationToken cancellationToken);
    Task<IEnumerable<StockItemModel>> ChangeStockAsync(StockChangeModel change, CancellationToken cancellationToken);

}
