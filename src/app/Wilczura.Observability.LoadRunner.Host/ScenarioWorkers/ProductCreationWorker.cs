using Microsoft.Extensions.Hosting;
using Wilczura.Observability.LoadRunner.Host.Client;
using Wilczura.Observability.LoadRunner.Host.Models;

namespace Wilczura.Observability.LoadRunner.Host.ScenarioWorkers;

public class ProductCreationWorker : BackgroundService
{
    private readonly IBffHttpClient _bffHttpClient;

    public ProductCreationWorker(IBffHttpClient bffHttpClient)
    {
        _bffHttpClient = bffHttpClient;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var secondsDely = 20;
        while(!stoppingToken.IsCancellationRequested)
        {
            var model = new ProductModel
            {
                Name = $"Random Product {Guid.NewGuid()}"
            };

            try
            {
                var result = await _bffHttpClient.UpsertProductAsync(model, stoppingToken);
                Console.WriteLine($"product-created: {result.First().ProductId}");
            }
            catch(Exception ex )
            {
                Console.WriteLine($"product-created: {ex.Message}");
            }

            await Task.Delay(1000 * secondsDely);
        }
    }
}
