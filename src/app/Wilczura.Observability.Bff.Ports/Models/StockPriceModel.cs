namespace Wilczura.Observability.Bff.Ports.Models;

public class StockPriceModel
{
    public long ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public long? PriceId { get; set; }
    public long? StockItemId { get; set; }

    public long? Quantity { get; set; }

    public decimal? BasePrice { get; set; }

    public decimal? CurrentPrice { get; set; }

    public DateOnly? DateFrom { get; set; }

    public DateOnly? DateTo { get; set; }
}
