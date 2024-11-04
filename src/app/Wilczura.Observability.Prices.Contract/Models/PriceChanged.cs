namespace Wilczura.Observability.Prices.Contract.Models;

public class PriceChanged
{
    public long PriceId {  get; set; }
    public long ProductId { get; set; }
}