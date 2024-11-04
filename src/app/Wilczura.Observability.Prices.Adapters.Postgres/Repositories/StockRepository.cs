using Microsoft.EntityFrameworkCore;
using Wilczura.Observability.Prices.Adapters.Postgres.Models;
using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Repositories;

namespace Wilczura.Observability.Prices.Adapters.Postgres.Repositories;

public class StockRepository : IStockRepository
{
    private readonly PricesContext _context;

    public StockRepository(PricesContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockItemModel>> UpsertAsync(StockItemModel model)
    {
        StockItem? entity = await _context.StockItems.SingleOrDefaultAsync(a => a.StockItemId == model.StockItemId);
        if (entity == null)
        {
            entity = new StockItem();
            _context.StockItems.Add(entity);
        }

        entity.StockItemId = model.StockItemId;
        entity.ProductId = model.ProductId;
        entity.Quantity = model.Quantity;

        await _context.SaveChangesAsync();
        model.StockItemId = entity.StockItemId;

        return new[] { model };
    }
}
