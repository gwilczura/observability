using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Prices.Contract.Models;
using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Services;

namespace Wilczura.Observability.Prices.Adapters.Controllers;

[ApiController]
[Route("[controller]")]
public class PriceController : ControllerBase
{

    private readonly ILogger<PriceController> _logger;
    private readonly IPriceService _priceService;

    public PriceController(
        ILogger<PriceController> logger,
        IPriceService priceService)
    {
        _logger = logger;
        _priceService = priceService;
    }

    [HttpGet]
    public async Task<IEnumerable<PriceDetailsDto>> GetAsync(long? priceId, long? productId, CancellationToken cancellationToken)
    {
        var result = await _priceService.GetAsync(priceId, productId);
        return result.Select(x => new PriceDetailsDto
        {
            PriceId = x.PriceId,
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            BasePrice = x.BasePrice,
            CurrentPrice = x.CurrentPrice,
            DateFrom = x.DateFrom,
            DateTo = x.DateTo,
        }).ToArray();
    }

    [HttpPost]
    public async Task<IEnumerable<PriceDetailsDto>> PostAsync(PriceDetailsDto product, CancellationToken cancellationToken)
    {
        var model = new PriceModel()
        {
            PriceId = product.PriceId,
            ProductId = product.ProductId,
            BasePrice = product.BasePrice,
            CurrentPrice = product.CurrentPrice,
            DateFrom = product.DateFrom,
            DateTo = product.DateTo,
        };
        var result = await _priceService.UpsertAsync(model);
        return result.Select(x => new PriceDetailsDto
        {
            PriceId = x.PriceId,
            ProductId = x.ProductId,
            ProductName = x.ProductName,
            BasePrice = x.BasePrice,
            CurrentPrice = x.CurrentPrice,
            DateFrom = x.DateFrom,
            DateTo = x.DateTo,
        }).ToArray();
    }
}
