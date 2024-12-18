using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Bff.Ports.Models;
using Wilczura.Observability.Bff.Ports.Services;

namespace Wilczura.Observability.Bff.Adapters.Controllers;

[ApiController]
[Route("[controller]")]
public class DemoController : ControllerBase
{

    private readonly ILogger<DemoController> _logger;
    private readonly IDemoService _demoService;

    public DemoController(ILogger<DemoController> logger, 
        IDemoService demoService)
    {
        _logger = logger;
        _demoService = demoService;
    }

    [HttpGet]
    public async Task<IEnumerable<StockPriceModel>> GetAsync(long? productId, CancellationToken cancellationToken)
    {
        var result = await _demoService.GetStockAndPriceAsync(productId, cancellationToken);
        return result;
    }

    [HttpGet]
    [Route("product")]
    public async Task<IEnumerable<ProductModel>> GetProductAsync(long? productId, CancellationToken cancellationToken)
    {
        var result = await _demoService.GetProductAsync(productId, cancellationToken);
        return result;
    }

    [HttpPost]
    [Route("product")]
    public async Task<IEnumerable<ProductModel>> PostProductAsync(ProductModel product, CancellationToken cancellationToken)
    {
        var result = await _demoService.UpsertProductAsync(product, cancellationToken);
        return result;
    }

    [HttpPost]
    [Route("stock")]
    public async Task<IEnumerable<StockItemModel>> PoststockAsync(StockItemModel stockItem, CancellationToken cancellationToken)
    {
        var result = await _demoService.UpsertStockItemAsync(stockItem, cancellationToken);
        return result;
    }

    [HttpPost]
    [Route("stock-change")]
    public async Task<IEnumerable<StockItemModel>> PostStockAsync(StockChangeModel change, CancellationToken cancellationToken)
    {
        var result = await _demoService.ChangeStockAsync(change, cancellationToken);
        return result;
    }

    [HttpPost]
    [Route("price")]
    public async Task<IEnumerable<PriceModel>> PostPriceAsync(PriceModel price, CancellationToken cancellationToken)
    {
        var result = await _demoService.UpsertPriceAsync(price, cancellationToken);
        return result;
    }
}
