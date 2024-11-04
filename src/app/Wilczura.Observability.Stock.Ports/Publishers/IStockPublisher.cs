using Wilczura.Observability.Stock.Ports.Models;

namespace Wilczura.Observability.Stock.Ports.Publishers;

public interface IStockPublisher
{
    Task PublishStockChangedAsync(StockItemModel stockItem);
}
