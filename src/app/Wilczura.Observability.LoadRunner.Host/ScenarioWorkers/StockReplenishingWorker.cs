using Microsoft.Extensions.Hosting;
using Wilczura.Observability.LoadRunner.Host.Client;
using Wilczura.Observability.LoadRunner.Host.Models;

namespace Wilczura.Observability.LoadRunner.Host.ScenarioWorkers;

public class StockReplenishingWorker : BackgroundService
{
    private readonly IBffHttpClient _bffHttpClient;
    private readonly Random _random;

    public StockReplenishingWorker(IBffHttpClient bffHttpClient)
    {
        _bffHttpClient = bffHttpClient;
        _random = new Random();
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var secondsDely = 20;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await _bffHttpClient.GetStockAndPriceAsync(null, stoppingToken);
                foreach (var item in result.Where(a => a.StockItemId != null && a.Quantity != null && a.Quantity < 20)
                    .OrderByDescending(a => a.ProductId))
                {
                    var nextRandom = _random.Next(9);
                    var quantity = 100 * (nextRandom + 1);
                    var upsertResult = await _bffHttpClient.ChangeStockAsync(new StockChangeModel
                    {
                        StockItemId = item.StockItemId!.Value,
                        QuantityChange = quantity,
                    }, stoppingToken);
                    var upserted = upsertResult.FirstOrDefault();
                    Console.WriteLine($"stock-refresh: Product {upserted?.ProductId}/{upserted?.Quantity}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"stock-refresh: {ex.Message}");
            }

            await Task.Delay(1000 * secondsDely);
        }
    }
}