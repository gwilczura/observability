using Microsoft.Extensions.Hosting;
using Wilczura.Observability.LoadRunner.Host.Client;
using Wilczura.Observability.LoadRunner.Host.Models;

namespace Wilczura.Observability.LoadRunner.Host.ScenarioWorkers;

public class PriceSettingWorker : BackgroundService
{
    private readonly IBffHttpClient _bffHttpClient;
    private readonly Random _random;

    public PriceSettingWorker(IBffHttpClient bffHttpClient)
    {
        _bffHttpClient = bffHttpClient;
        _random = new Random();
    }

    protected async override Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var secondsDely = 4;
        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var result = await _bffHttpClient.GetStockAndPriceAsync(null, stoppingToken);

                // select data for random product
                var productIds = result.Select(a => a.ProductId).Distinct().ToList();
                var selectedProductIndex = _random.Next(productIds.Count);
                var selectedProductId = productIds[selectedProductIndex];
                var productPrices = result.Where(p=>p.ProductId == selectedProductId && p.PriceId.HasValue).ToList();

                PriceModel? priceModel = null;
                //decide edit vs new period
                var action = _random.Next(2);
                var actionName = string.Empty;
                if (action == 0 || productPrices.Count == 0)
                {
                    // add period
                    actionName = "add";
                    var maxDate = productPrices.Where(pp=>pp.DateTo.HasValue)
                        .Select(pp => pp.DateTo!.Value)
                        .DefaultIfEmpty(DateOnly.FromDateTime(DateTime.Today))
                        .Max();
                    priceModel = new PriceModel
                    {
                        ProductId = selectedProductId,
                        BasePrice = 10,
                        CurrentPrice = 10,
                        DateFrom = maxDate.AddDays(1),
                        DateTo = maxDate.AddDays(20),
                    };
                }
                else
                {
                    // edit existing
                    actionName = "edit";
                    var randomPriceIndex = _random.Next(productPrices.Count);
                    var randomElement = productPrices[randomPriceIndex];
                    var change = _random.Next(21) - 10;
                    randomElement.BasePrice += change;
                    randomElement.CurrentPrice += change;
                    priceModel = new PriceModel
                    {
                        ProductId = selectedProductId,
                        BasePrice = Math.Max(randomElement!.BasePrice!.Value, 1),
                        CurrentPrice = Math.Max(randomElement!.CurrentPrice!.Value, 1),
                        DateFrom = randomElement.DateFrom!.Value,
                        DateTo = randomElement.DateTo!.Value,
                        PriceId =randomElement.PriceId!.Value,
                    };

                }
                var upsertResult = await _bffHttpClient.UpsertPriceAsync(priceModel, stoppingToken);
                var upserted = upsertResult.First();
                Console.WriteLine($"price-change: Action [{actionName}] Product {upserted?.ProductId} Price {upserted?.CurrentPrice}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"price-change: {ex.Message}");
            }

            await Task.Delay(1000 * secondsDely);
        }
    }
}
