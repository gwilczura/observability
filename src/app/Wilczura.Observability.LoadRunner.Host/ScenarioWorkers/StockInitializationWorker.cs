using Microsoft.Extensions.Hosting;
using Wilczura.Observability.LoadRunner.Host.Client;
using Wilczura.Observability.LoadRunner.Host.Models;

namespace Wilczura.Observability.LoadRunner.Host.ScenarioWorkers;

public class StockInitializationWorker : BackgroundService
{
    private readonly IBffHttpClient _bffHttpClient;

    public StockInitializationWorker(IBffHttpClient bffHttpClient)
    {
        _bffHttpClient = bffHttpClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var secondsDely = 5;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await _bffHttpClient.GetStockAndPriceAsync(null, stoppingToken);
                foreach (var item in result.Where(a=> a.Quantity == null))
                {
                    var upsertResult = await _bffHttpClient.UpsertStockItemAsync(new StockItemModel
                    {
                        ProductId = item.ProductId,
                        Quantity = 0,
                        ProductName = item.ProductName,
                    }, stoppingToken);
                    Console.WriteLine($"stock-init: Product {upsertResult.First().ProductId}");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"stock-init: {ex.Message}");
            }

            await Task.Delay(1000 * secondsDely);
        }
    }
}
