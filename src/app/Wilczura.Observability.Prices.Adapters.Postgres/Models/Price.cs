namespace Wilczura.Observability.Prices.Adapters.Postgres.Models;

public class Price
{
    public long PriceId { get; set; }

    public long ProductId { get; set; }

    public Product? Product { get; set; }

    public decimal BasePrice { get; set; }

    public decimal CurrentPrice { get; set; }

    public DateOnly DateFrom { get; set; }

    public DateOnly DateTo { get; set; }
}
