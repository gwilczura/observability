using System.ComponentModel.DataAnnotations.Schema;

namespace Wilczura.Observability.Stock.Adapters.Postgres.Models;

public class Product
{
    [DatabaseGenerated(DatabaseGeneratedOption.None)]
    public long ProductId { get; set; }
    public string Name { get; set; } = string.Empty;
}