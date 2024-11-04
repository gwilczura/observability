using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Stock.Contract.Models;
using Wilczura.Observability.Stock.Ports.Models;
using Wilczura.Observability.Stock.Ports.Services;

namespace Wilczura.Observability.Stock.Adapters.Controllers;

[ApiController]
[Route("[controller]")]
public class StockController : ControllerBase
{
    private readonly ILogger<StockController> _logger;
    private readonly IStockService _stockService;

    public StockController(
        ILogger<StockController> logger,
        IStockService stockService)
    {
        _logger = logger;
        _stockService = stockService;
    }

    [HttpGet]
    public async Task<IEnumerable<StockItemDto>> GetAsync(long? stockItemId, long? productId, CancellationToken cancellationToken)
    {
        var result = await _stockService.GetAsync(stockItemId, productId);
        return result.Select(x => new StockItemDto
        {
            StockItemId = x.StockItemId,
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Quantity = x.Quantity,
        }).ToArray();
    }

    [HttpPost]
    public async Task<IEnumerable<StockItemDto>> PostAsync(StockItemDto stockItem, CancellationToken cancellationToken)
    {
        var model = new StockItemModel()
        {
            StockItemId = stockItem.StockItemId,
            ProductId = stockItem.ProductId,
            Quantity = stockItem.Quantity,
        };
        var result = await _stockService.UpsertAsync(model);
        return result.Select(x => new StockItemDto
        {
            StockItemId = x.StockItemId,
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            Quantity = x.Quantity,
        }).ToArray();
    }

    [HttpPost]
    [Route("change")]
    public async Task<IEnumerable<StockItemModel>> PoststockAsync(StockChangeDto change, CancellationToken cancellationToken)
    {
        var model = new StockChangeModel
        {
            StockItemId = change.StockItemId,
            QuantityChange = change.QuantityChange,
        };

        var result = await _stockService.ChangeStockAsync(model, cancellationToken);
        return result;
    }
}
