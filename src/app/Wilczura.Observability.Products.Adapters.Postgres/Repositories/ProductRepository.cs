using Microsoft.EntityFrameworkCore;
using Wilczura.Observability.Products.Adapters.Postgres.Models;
using Wilczura.Observability.Products.Ports.Models;
using Wilczura.Observability.Products.Ports.Repositories;

namespace Wilczura.Observability.Products.Adapters.Postgres.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ProductsContext _context;

    public ProductRepository(ProductsContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<ProductModel>> GetAsync(long? productId)
    {
        var query = _context.Products.AsNoTracking();
        if (productId != null)
        {
            query = query.Where(p => p.ProductId == productId);
        }

        var result = await query.ToArrayAsync();
        return result.Select(a => new ProductModel
        {
            ProductId = a.ProductId,
            Name = a.Name,
        }).ToArray();
    }

    public async Task<IEnumerable<ProductModel>> UpsertAsync(ProductModel model)
    {
        Product entity;
        if (model.ProductId > 0)
        {
            entity = await _context.Products.SingleAsync(a => a.ProductId == model.ProductId);
        }
        else
        {
            entity = new Product();
            _context.Products.Add(entity);
        }

        entity.Name = model.Name;

        await _context.SaveChangesAsync();
        model.ProductId = entity.ProductId;
        model.Name = entity.Name;

        return new[] { model };
    }
}
