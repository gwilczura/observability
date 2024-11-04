using Wilczura.Observability.Prices.Ports.Models;

namespace Wilczura.Observability.Prices.Ports.Repositories;

public interface IPriceRepository
{
    Task<IEnumerable<PriceModel>> GetAsync(long? priceId, long? productId);
    Task<IEnumerable<PriceModel>> UpsertAsync(PriceModel model);
}
