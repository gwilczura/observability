using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Publishers;
using Wilczura.Observability.Prices.Ports.Repositories;
using Wilczura.Observability.Prices.Ports.Services;

namespace Wilczura.Observability.Prices.Application.Services;

public class PriceService : IPriceService
{

    private readonly IPriceRepository _priceRepository;
    private readonly IPricePublisher _pricePublisher;

    public PriceService(
        IPriceRepository priceRepository,
        IPricePublisher pricePublisher)
    {
        _priceRepository = priceRepository;
        _pricePublisher = pricePublisher;
    }
    public async Task<IEnumerable<PriceModel>> GetAsync(long? priceId, long? productId)
    {
        return await _priceRepository.GetAsync(priceId, productId);
    }

    public async Task<IEnumerable<PriceModel>> UpsertAsync(PriceModel model)
    {
        var result = await _priceRepository.UpsertAsync(model);
        await _pricePublisher.PublishPriceChangedAsync(result.First());
        return result;
    }
}