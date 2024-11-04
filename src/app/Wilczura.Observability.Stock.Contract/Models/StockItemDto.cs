namespace Wilczura.Observability.Stock.Contract.Models;

public class StockItemDto
{
    public long StockItemId { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public long Quantity { get; set; }
}
