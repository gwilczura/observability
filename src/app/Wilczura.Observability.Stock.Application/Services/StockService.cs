using Wilczura.Observability.Stock.Ports.Models;
using Wilczura.Observability.Stock.Ports.Publishers;
using Wilczura.Observability.Stock.Ports.Repositories;
using Wilczura.Observability.Stock.Ports.Services;

namespace Wilczura.Observability.Stock.Application.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;
    private readonly IStockPublisher _stockPublisher;

    public StockService(
        IStockRepository stockRepository,
        IStockPublisher stockPublisher)
    {
        _stockRepository = stockRepository;
        _stockPublisher = stockPublisher;
    }

    public async Task<IEnumerable<StockItemModel>> GetAsync(long? stockItemId, long? productId)
    {
        return await _stockRepository.GetAsync(stockItemId, productId);
    }

    public async Task<IEnumerable<StockItemModel>> UpsertAsync(StockItemModel model)
    {
        var result = await _stockRepository.UpsertAsync(model);
        await _stockPublisher.PublishStockChangedAsync(result.First());
        return result;
    }

    public async Task<IEnumerable<StockItemModel>> ChangeStockAsync(StockChangeModel model, CancellationToken cancellationToken)
    {
        var result = await _stockRepository.ChangeStockAsync(model, cancellationToken);
        await _stockPublisher.PublishStockChangedAsync(result.First());
        return result;
    }
}
