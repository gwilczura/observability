namespace Wilczura.Observability.Prices.Contract.Models;

public class PriceDetailsDto
{
    public long PriceId { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal BasePrice { get; set; }

    public decimal CurrentPrice { get; set; }

    public DateOnly DateFrom { get; set; }

    public DateOnly DateTo { get; set; }
}
