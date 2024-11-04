using Microsoft.EntityFrameworkCore;
using Wilczura.Observability.Prices.Adapters.Postgres.Models;
using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Repositories;

namespace Wilczura.Observability.Prices.Adapters.Postgres.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly PricesContext _context;

    public ProductRepository(PricesContext context)
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
