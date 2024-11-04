using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Repositories;
using Wilczura.Observability.Stock.Contract;

namespace Wilczura.Observability.Prices.Adapters.Stock.Repositories;

public class StockSourceRepository : IStockSourceRepository
{
    private readonly IStockHttpClient _stockHttpClient;

    public StockSourceRepository(IStockHttpClient stockHttpClient)
    {
        _stockHttpClient = stockHttpClient;
    }

    public async Task<StockItemModel?> GetStockItemAsync(long stockItemId, CancellationToken cancellationToken)
    {
        var result = await _stockHttpClient.GetStockItemsAsync(stockItemId, cancellationToken);
        return result.Select(a => new StockItemModel
        {
            StockItemId = stockItemId,
            ProductId = a.ProductId,
            ProductName = a.ProductName,
            Quantity = a.Quantity
        }).FirstOrDefault();
    }
}
