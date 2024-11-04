using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace Wilczura.Observability.Prices.Adapters.Postgres.Models;

[Index(nameof(ProductId), IsUnique = true)]
public class StockItem
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long StockItemId { get; set; }

    public long ProductId { get; set; }

    public Product? Product { get; set; }

    public long Quantity { get; set; }
}
