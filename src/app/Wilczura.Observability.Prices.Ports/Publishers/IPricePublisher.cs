using Wilczura.Observability.Prices.Ports.Models;

namespace Wilczura.Observability.Prices.Ports.Publishers;

public interface IPricePublisher
{
    Task PublishPriceChangedAsync(PriceModel price);
}
