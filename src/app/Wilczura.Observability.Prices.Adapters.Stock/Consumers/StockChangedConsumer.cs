using MassTransit;
using Wilczura.Observability.Prices.Ports.Services;
using Wilczura.Observability.Stock.Contract.Models;

namespace Wilczura.Observability.Prices.Adapters.Stock.Consumers;

public class StockChangedConsumer : IConsumer<StockChanged>
{
    private readonly IStockService _stockService;

    public StockChangedConsumer(
        IStockService stockService)
    {
        _stockService = stockService;
    }

    public async Task Consume(ConsumeContext<StockChanged> context)
    {
        await _stockService.RefreshStockItemAsync(context.Message.StockItemId, context.CancellationToken);
    }
}
