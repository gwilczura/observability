namespace Wilczura.Observability.LoadRunner.Host.Models;

public class PriceModel
{
    public long PriceId { get; set; }

    public long ProductId { get; set; }

    public string ProductName { get; set; } = string.Empty;

    public decimal BasePrice { get; set; }

    public decimal CurrentPrice { get; set; }

    public DateOnly DateFrom { get; set; }

    public DateOnly DateTo { get; set; }
}
