namespace Wilczura.Observability.Bff.Ports.Models;

public class StockChangeModel
{
    public long StockItemId { get; set; }
    public long QuantityChange { get; set; }
}
