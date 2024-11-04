using Microsoft.EntityFrameworkCore;
using Wilczura.Observability.Common.Exceptions;
using Wilczura.Observability.Stock.Adapters.Postgres.Models;
using Wilczura.Observability.Stock.Ports.Models;
using Wilczura.Observability.Stock.Ports.Repositories;

namespace Wilczura.Observability.Stock.Adapters.Postgres.Repositories;

public class StockRepository : IStockRepository
{
    private readonly StockContext _context;

    public StockRepository(StockContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<StockItemModel>> GetAsync(long? stockItemId, long? productId)
    {
        var query = _context.StockItems.AsNoTracking();
        if (stockItemId != null)
        {
            query = query.Where(p => p.StockItemId == stockItemId);
        }

        if (productId != null)
        {
            query = query.Where(p => p.Product!.ProductId == productId);
        }

        var result = await query.Select(a => new StockItemModel
        {
            StockItemId = a.StockItemId,
            ProductId = a.Product!.ProductId,
            ProductName = a.Product.Name,
            Quantity = a.Quantity,

        }).ToArrayAsync();
        return result;
    }

    public async Task<IEnumerable<StockItemModel>> UpsertAsync(StockItemModel model)
    {
        if (model.ProductId == 0)
        {
            throw new ObservabilityException("ProductId is required to create a Price");
        }

        StockItem entity;
        if (model.StockItemId > 0)
        {
            entity = await _context.StockItems.SingleAsync(a => a.StockItemId == model.StockItemId);
        }
        else
        {
            entity = new StockItem();
            _context.StockItems.Add(entity);
        }

        entity.ProductId = model.ProductId;
        entity.Quantity = model.Quantity;

        await _context.SaveChangesAsync();
        var result = await GetAsync(stockItemId: entity.StockItemId, null);

        return result;
    }

    public async Task<IEnumerable<StockItemModel>> ChangeStockAsync(StockChangeModel model, CancellationToken cancellationToken)
    {
        FormattableString updateQuery = $"update stock_items set quantity = quantity + {model.QuantityChange} where stock_item_id = {model.StockItemId}";
        var updated = await _context.Database.ExecuteSqlAsync(updateQuery, cancellationToken);
        if(updated != 1) 
        {
            return [];
        }

        return await _context.StockItems
            .Where(a => a.StockItemId == model.StockItemId)
            .Select(a=> new StockItemModel
            {
                StockItemId = a.StockItemId,
                ProductId = a.ProductId,
                Quantity = a.Quantity,
                ProductName = a.Product!.Name,
            }).ToArrayAsync(cancellationToken: cancellationToken);
    }
}
