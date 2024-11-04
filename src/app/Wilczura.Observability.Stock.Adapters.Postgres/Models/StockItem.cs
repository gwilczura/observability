using Microsoft.EntityFrameworkCore;

namespace Wilczura.Observability.Stock.Adapters.Postgres.Models;

[Index(nameof(ProductId), IsUnique = true)]
public class StockItem
{
    public long StockItemId { get; set; }

    public long ProductId { get; set; }

    public Product? Product { get; set; }

    public long Quantity { get; set; }
}
