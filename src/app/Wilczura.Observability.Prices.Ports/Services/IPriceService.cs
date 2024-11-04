using Wilczura.Observability.Prices.Ports.Models;

namespace Wilczura.Observability.Prices.Ports.Services;

public interface IPriceService
{
    Task<IEnumerable<PriceModel>> GetAsync(long? priceId, long? productId);
    Task<IEnumerable<PriceModel>> UpsertAsync(PriceModel model);
}
