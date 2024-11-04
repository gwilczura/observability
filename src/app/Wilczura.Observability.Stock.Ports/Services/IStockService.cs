using Wilczura.Observability.Stock.Ports.Models;

namespace Wilczura.Observability.Stock.Ports.Services;

public interface IStockService
{
    Task<IEnumerable<StockItemModel>> GetAsync(long? stockItemId, long? productId);
    Task<IEnumerable<StockItemModel>> UpsertAsync(StockItemModel model);
    Task<IEnumerable<StockItemModel>> ChangeStockAsync(StockChangeModel model, CancellationToken cancellationToken);
    
}
