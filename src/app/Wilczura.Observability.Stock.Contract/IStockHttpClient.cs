using Wilczura.Observability.Stock.Contract.Models;

namespace Wilczura.Observability.Stock.Contract;

public interface IStockHttpClient
{
    public Task<IEnumerable<StockItemDto>> GetStockItemsAsync(long? id, CancellationToken cancellationToken);
    public Task<IEnumerable<StockItemDto>> UpsertStockItemAsync(StockItemDto dto, CancellationToken cancellationToken);
    public Task<IEnumerable<StockItemDto>> ChangeStockAsync(StockChangeDto dto, CancellationToken cancellationToken);
}
