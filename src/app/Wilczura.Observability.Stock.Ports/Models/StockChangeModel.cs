namespace Wilczura.Observability.Stock.Ports.Models;

public class StockChangeModel
{
    public long StockItemId { get; set; }
    public long QuantityChange { get; set; }
}
