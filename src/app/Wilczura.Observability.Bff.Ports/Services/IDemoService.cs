using Wilczura.Observability.Bff.Ports.Models;

namespace Wilczura.Observability.Bff.Ports.Services;

public interface IDemoService
{
    Task<IEnumerable<StockPriceModel>> GetStockAndPriceAsync(long? productId, CancellationToken cancellationToken);
    Task<IEnumerable<ProductModel>> UpsertProductAsync(ProductModel model, CancellationToken cancellationToken);
    Task<IEnumerable<StockItemModel>> UpsertStockItemAsync(StockItemModel stockItem, CancellationToken cancellationToken); 
    Task<IEnumerable<StockItemModel>> ChangeStockAsync(StockChangeModel change, CancellationToken cancellationToken);
    Task<IEnumerable<PriceModel>> UpsertPriceAsync(PriceModel price, CancellationToken cancellationToken);
}
