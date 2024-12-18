using System.Reflection;
using Wilczura.Observability.Bff.Ports.Models;
using Wilczura.Observability.Bff.Ports.Repositories;
using Wilczura.Observability.Bff.Ports.Services;

namespace Wilczura.Observability.Bff.Application.Services;

public class DemoService : IDemoService
{
    private readonly IProductRepository _productRepository;
    private readonly IStockRepository _stockRepository;
    private readonly IPriceRepository _priceRepository;

    public DemoService(
        IProductRepository productRepository,
        IStockRepository stockRepository,
        IPriceRepository priceRepository)
    {
        _productRepository = productRepository;
        _stockRepository = stockRepository;
        _priceRepository = priceRepository;
    }

    public async Task<IEnumerable<StockItemModel>> ChangeStockAsync(StockChangeModel change, CancellationToken cancellationToken)
    {
        return await _stockRepository.ChangeStockAsync(change, cancellationToken);
    }

    public async Task<IEnumerable<StockPriceModel>> GetStockAndPriceAsync(long? productId, CancellationToken cancellationToken)
    {
        var products = await _productRepository.GetProductsAsync(productId, cancellationToken);
        var prices = await _priceRepository.GetPricesAsync(cancellationToken);
        var stockItems = await _stockRepository.GetStockAsync(null, cancellationToken);

        var stockPrice = products.SelectMany(p =>
        {
            var pricesForProduct = prices.Where(pr => pr.ProductId == p.ProductId).ToArray();
            var stockItem = stockItems.Where(si => si.ProductId == p.ProductId).FirstOrDefault();
            var items = pricesForProduct.Select(pfp =>
                new StockPriceModel
                {
                    ProductId = p.ProductId,
                    ProductName = p.Name,
                    Quantity = stockItem?.Quantity ?? 0,
                    BasePrice = pfp.BasePrice,
                    CurrentPrice = pfp.CurrentPrice,
                    DateFrom = pfp.DateFrom,
                    DateTo = pfp.DateTo,
                    PriceId = pfp.PriceId,
                    StockItemId = stockItem?.StockItemId
                }
            ).ToList();

            if (items.Count == 0)
            {
                var item = new StockPriceModel 
                { 
                    ProductId = p.ProductId, 
                    ProductName = p.Name,
                    Quantity = stockItem?.Quantity,
                    StockItemId = stockItem?.StockItemId
                };

                items.Add(item);
            }

            return items;

        }).OrderBy(a => a.ProductName).ThenBy(a => a.DateFrom).ThenBy(a => a.DateTo);
        return stockPrice;
    }

    public async Task<IEnumerable<PriceModel>> UpsertPriceAsync(PriceModel price, CancellationToken cancellationToken)
    {
        return await _priceRepository.UpsertPriceAsync(price, cancellationToken);
    }

    public async Task<IEnumerable<ProductModel>> GetProductAsync(long? productId, CancellationToken cancellationToken)
    {
        return await _productRepository.GetProductsAsync(productId, cancellationToken);
    }

    public async Task<IEnumerable<ProductModel>> UpsertProductAsync(ProductModel model, CancellationToken cancellationToken)
    {
        return await _productRepository.UpsertProductAsync(model, cancellationToken);
    }

    public async Task<IEnumerable<StockItemModel>> UpsertStockItemAsync(StockItemModel model, CancellationToken cancellationToken)
    {
        return await _stockRepository.UpsertStockItemAsync(model, cancellationToken);
    }
}
