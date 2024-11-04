using Wilczura.Observability.Bff.Ports.Models;
using Wilczura.Observability.Bff.Ports.Repositories;
using Wilczura.Observability.Prices.Contract;
using Wilczura.Observability.Prices.Contract.Models;

namespace Wilczura.Observability.Bff.Adapters.Prices.Repositories;

public class PriceRepository : IPriceRepository
{
    private readonly IPricesHttpClient _pricesHttpClient;

    public PriceRepository(IPricesHttpClient pricesHttpClient)
    {
        _pricesHttpClient = pricesHttpClient;
    }

    public async Task<IEnumerable<PriceModel>> GetPricesAsync(CancellationToken cancellationToken)
    {
        var products = await _pricesHttpClient.GetPricesAsync(cancellationToken);
        return products.Select(p => new PriceModel
        {
            PriceId = p.PriceId,
            ProductId = p.ProductId,
            ProductName = p.ProductName,
            BasePrice = p.BasePrice,
            CurrentPrice = p.CurrentPrice,
            DateFrom = p.DateFrom,
            DateTo = p.DateTo,
        }).ToArray();
    }

    public async Task<IEnumerable<PriceModel>> UpsertPriceAsync(PriceModel price, CancellationToken cancellationToken)
    {
        var dto = new PriceDetailsDto
        {
            PriceId = price.PriceId,
            ProductId = price.ProductId,
            ProductName = price.ProductName,
            BasePrice = price.BasePrice,
            CurrentPrice = price.CurrentPrice,
            DateFrom = price.DateFrom,
            DateTo = price.DateTo,
        };

        var prices = await _pricesHttpClient.UpsertPriceAsync(dto, cancellationToken);

        return prices.Select(price => new PriceModel
        {
            PriceId = price.PriceId,
            ProductId = price.ProductId,
            ProductName = price.ProductName,
            BasePrice = price.BasePrice,
            CurrentPrice = price.CurrentPrice,
            DateFrom = price.DateFrom,
            DateTo = price.DateTo,
        }).ToArray();
    }
}
