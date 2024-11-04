using System.Reflection;
using Wilczura.Observability.Bff.Ports.Models;
using Wilczura.Observability.Bff.Ports.Repositories;
using Wilczura.Observability.Stock.Contract;
using Wilczura.Observability.Stock.Contract.Models;

namespace Wilczura.Observability.Bff.Adapters.Stock.Repositories;

public class StockRepository : IStockRepository
{
    private readonly IStockHttpClient _stockHttpClient;

    public StockRepository(IStockHttpClient stockHttpClient)
    {
        _stockHttpClient = stockHttpClient;
    }

    public async Task<IEnumerable<StockItemModel>> GetStockAsync(long? stockItemId, CancellationToken cancellationToken)
    {
        var products = await _stockHttpClient.GetStockItemsAsync(stockItemId, cancellationToken);
        return products.Select(p => new StockItemModel
        {
            StockItemId = p.StockItemId,
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Quantity = p.Quantity,
        }).ToArray();
    }

    public async Task<IEnumerable<StockItemModel>> UpsertStockItemAsync(StockItemModel model, CancellationToken cancellationToken)
    {
        var dto = new StockItemDto
        {
            StockItemId = model.StockItemId,
            ProductId = model.ProductId,
            ProductName = model.ProductName,
            Quantity = model.Quantity,
        };
        var result = await _stockHttpClient.UpsertStockItemAsync(dto, cancellationToken);
        return result.Select(p => new StockItemModel
        {
            StockItemId = p.StockItemId,
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Quantity = p.Quantity,
        }).ToArray();
    }

    public async Task<IEnumerable<StockItemModel>> ChangeStockAsync(StockChangeModel model, CancellationToken cancellationToken)
    {
        var dto = new StockChangeDto
        {
            StockItemId = model.StockItemId,
            QuantityChange = model.QuantityChange,
        };

        var result = await _stockHttpClient.ChangeStockAsync(dto, cancellationToken);
        return result.Select(p => new StockItemModel
        {
            StockItemId = p.StockItemId,
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            Quantity = p.Quantity,
        }).ToArray();
    }
}
