using Microsoft.EntityFrameworkCore;
using Wilczura.Observability.Stock.Adapters.Postgres.Models;

namespace Wilczura.Observability.Stock.Adapters.Postgres;

public class StockContext(DbContextOptions<StockContext> options) : DbContext(options)
{
    public DbSet<StockItem> StockItems { get; set; }
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<StockItem>(st =>
        {
            st.ToTable(t => t.HasCheckConstraint("ck_stock_items_quantity", "quantity >= 0"));
        });
    }
}
