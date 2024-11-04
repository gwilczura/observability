namespace Wilczura.Observability.LoadRunner.Host.Models;

public class StockItemModel
{
    public long StockItemId { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public long Quantity { get; set; }
}
