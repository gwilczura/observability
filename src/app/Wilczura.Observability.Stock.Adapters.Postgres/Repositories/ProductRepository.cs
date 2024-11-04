using Microsoft.EntityFrameworkCore;
using Wilczura.Observability.Stock.Adapters.Postgres.Models;
using Wilczura.Observability.Stock.Ports.Models;
using Wilczura.Observability.Stock.Ports.Repositories;

namespace Wilczura.Observability.Stock.Adapters.Postgres.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly StockContext _context;

    public ProductRepository(StockContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductModel>> UpsertAsync(ProductModel model)
    {
        Product? entity = await _context.Products.SingleOrDefaultAsync(a => a.ProductId == model.ProductId);
        if(entity == null)
        {
            entity = new Product();
            _context.Products.Add(entity);
        }

        entity.ProductId = model.ProductId;
        entity.Name = model.Name;

        await _context.SaveChangesAsync();
        model.ProductId = entity.ProductId;
        model.Name = entity.Name;

        return new[] { model };
    }
}
