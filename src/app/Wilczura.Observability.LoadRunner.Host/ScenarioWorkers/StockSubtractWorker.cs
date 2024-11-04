using Microsoft.Extensions.Hosting;
using Wilczura.Observability.LoadRunner.Host.Client;
using Wilczura.Observability.LoadRunner.Host.Models;

namespace Wilczura.Observability.LoadRunner.Host.ScenarioWorkers;

public class StockSubtractWorker : BackgroundService
{
    private readonly IBffHttpClient _bffHttpClient;
    private readonly Random _random;

    public StockSubtractWorker(IBffHttpClient bffHttpClient)
    {
        _bffHttpClient = bffHttpClient;
        _random = new Random();
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var secondsDely = 1;
        StockChangeModel? model = null;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await _bffHttpClient.GetStockAndPriceAsync(null, stoppingToken);
                var validResults = result.Where(a => a.Quantity.HasValue && a.Quantity > 0)
                    .GroupBy(a => a.StockItemId!.Value)
                    .Select(g => g.First())
                    .Select(pp => new StockItemModel
                    {
                        ProductId = pp.ProductId,
                        ProductName = pp.ProductName,
                        Quantity = pp.Quantity!.Value,
                        StockItemId = pp.StockItemId!.Value
                    })
                    .ToList();

                var randomIndex = _random.Next(validResults.Count);
                var randomStockItem = validResults[randomIndex];

                var randomChange = (long)_random.Next(10) + 1;
                randomChange = Math.Min(randomChange, randomStockItem.Quantity);
                randomChange = -1 * randomChange;
                model = new StockChangeModel
                {
                    QuantityChange = randomChange,
                    StockItemId = randomStockItem.StockItemId
                };

                var changeResult = await _bffHttpClient.ChangeStockAsync(model, stoppingToken);
                var changedStockItem = changeResult.FirstOrDefault();

                Console.WriteLine($"stock-subtract: Product {changedStockItem?.ProductId}/{changedStockItem?.Quantity}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"stock-subtract: {model?.StockItemId}/{model?.QuantityChange} - {ex.Message}");
            }

            await Task.Delay(1000 * secondsDely);
        }
    }
}
