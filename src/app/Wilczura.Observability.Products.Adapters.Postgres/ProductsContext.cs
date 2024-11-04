using Microsoft.EntityFrameworkCore;
using Wilczura.Observability.Products.Adapters.Postgres.Models;

namespace Wilczura.Observability.Products.Adapters.Postgres;

public class ProductsContext(DbContextOptions<ProductsContext> options) : DbContext(options)
{
    public DbSet<Product> Products { get; set; }
}
