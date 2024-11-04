using Wilczura.Observability.Bff.Ports.Models;

namespace Wilczura.Observability.Bff.Ports.Repositories;

public interface IPriceRepository
{
    Task<IEnumerable<PriceModel>> GetPricesAsync(CancellationToken cancellationToken);
    Task<IEnumerable<PriceModel>> UpsertPriceAsync(PriceModel price, CancellationToken cancellationToken);
}
