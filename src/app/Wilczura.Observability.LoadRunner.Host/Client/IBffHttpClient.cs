using Wilczura.Observability.LoadRunner.Host.Models;

namespace Wilczura.Observability.LoadRunner.Host.Client;

public interface IBffHttpClient
{
    Task<IEnumerable<ProductModel>> UpsertProductAsync(ProductModel model, CancellationToken cancellationToken);
    Task<IEnumerable<StockPriceModel>> GetStockAndPriceAsync(long? productId, CancellationToken cancellationToken);
    Task<IEnumerable<StockItemModel>> UpsertStockItemAsync(StockItemModel stockItem, CancellationToken cancellationToken);
    Task<IEnumerable<StockItemModel>> ChangeStockAsync(StockChangeModel change, CancellationToken cancellationToken);
    Task<IEnumerable<PriceModel>> UpsertPriceAsync(PriceModel price, CancellationToken cancellationToken);
}
