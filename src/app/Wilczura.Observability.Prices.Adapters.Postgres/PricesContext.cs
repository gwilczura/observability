using Microsoft.EntityFrameworkCore;
using Wilczura.Observability.Prices.Adapters.Postgres.Models;

namespace Wilczura.Observability.Prices.Adapters.Postgres;

public class PricesContext(DbContextOptions<PricesContext> options) : DbContext(options)
{
    public DbSet<Price> Prices { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<StockItem> StockItems { get; set; }
}