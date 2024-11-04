using Wilczura.Observability.Prices.Contract.Models;

namespace Wilczura.Observability.Prices.Contract;

public interface IPricesHttpClient
{
    Task<IEnumerable<PriceDetailsDto>> GetPricesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<PriceDetailsDto>> UpsertPriceAsync(PriceDetailsDto dto, CancellationToken cancellationToken);
}
