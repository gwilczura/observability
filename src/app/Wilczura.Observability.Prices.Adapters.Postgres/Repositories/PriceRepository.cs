using Microsoft.EntityFrameworkCore;
using Wilczura.Observability.Common.Exceptions;
using Wilczura.Observability.Prices.Adapters.Postgres.Models;
using Wilczura.Observability.Prices.Ports.Models;
using Wilczura.Observability.Prices.Ports.Repositories;

namespace Wilczura.Observability.Prices.Adapters.Postgres.Repositories;

public class PriceRepository : IPriceRepository
{
    private readonly PricesContext _context;

    public PriceRepository(PricesContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<PriceModel>> GetAsync(long? priceId, long? productId)
    {
        var query = _context.Prices.AsNoTracking();
        if (priceId != null)
        {
            query = query.Where(p => p.PriceId == priceId);
        }

        if (productId != null)
        {
            query = query.Where(p => p.Product!.ProductId == productId);
        }

        var result = await query.Select(a=> new PriceModel
        {
            PriceId = a.PriceId,
            ProductId = a.Product!.ProductId,
            ProductName = a.Product.Name,
            BasePrice = a.BasePrice,
            CurrentPrice = a.CurrentPrice,
            DateFrom = a.DateFrom,
            DateTo = a.DateTo,
            
        }).ToArrayAsync();
        return result;
    }

    public async Task<IEnumerable<PriceModel>> UpsertAsync(PriceModel model)
    {
        if(model.ProductId == 0)
        {
            throw new ObservabilityException("ProductId is required to create a Price");
        }

        Price entity;
        if (model.PriceId > 0)
        {
            entity = await _context.Prices.SingleAsync(a => a.PriceId == model.PriceId);
        }
        else
        {
            entity = new Price();
            _context.Prices.Add(entity);
        }

        entity.ProductId = model.ProductId;
        entity.BasePrice = model.BasePrice;
        entity.CurrentPrice = model.CurrentPrice;
        entity.DateFrom = model.DateFrom;
        entity.DateTo = model.DateTo;

        await _context.SaveChangesAsync();
        var result = await GetAsync(priceId: entity.PriceId, null);

        return result;
    }
}
