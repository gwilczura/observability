using Wilczura.Observability.Prices.Ports.Models;

namespace Wilczura.Observability.Prices.Ports.Repositories;

public interface IStockRepository
{
    Task<IEnumerable<StockItemModel>> UpsertAsync(StockItemModel stockItemModel);
}
