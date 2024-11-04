using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Repositories;
using Wilczura.Observability.Prices.Ports.Services;

namespace Wilczura.Observability.Prices.Application.Services;

public class StockService : IStockService
{
    private readonly IStockRepository _stockRepository;
    private readonly IStockSourceRepository _stockSourceRepository;

    public StockService(
        IStockRepository stockRepository,
        IStockSourceRepository stockSourceRepository)
    {
        _stockRepository = stockRepository;
        _stockSourceRepository = stockSourceRepository;
    }

    public async Task<IEnumerable<StockItemModel>> RefreshStockItemAsync(long stockItemId, CancellationToken cancellationToken)
    {
        var stockFromSource = await _stockSourceRepository.GetStockItemAsync(stockItemId, cancellationToken);
        if (stockFromSource != null)
        {
            return await _stockRepository.UpsertAsync(stockFromSource);
        }

        return [];
    }
}
