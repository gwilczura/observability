using System.Net.Http.Json;
using Wilczura.Observability.LoadRunner.Host.Models;

namespace Wilczura.Observability.LoadRunner.Host.Client;

public class BffHttpClient : IBffHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public BffHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<StockItemModel>> ChangeStockAsync(StockChangeModel change, CancellationToken cancellationToken)
    {
        var url = "/demo/stock-change";
        var client = _httpClientFactory.CreateClient(nameof(BffHttpClient));
        var response = await client.PostAsJsonAsync(url, change, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<StockItemModel>>(cancellationToken)) ?? [];
    }

    public async Task<IEnumerable<StockPriceModel>> GetStockAndPriceAsync(long? productId, CancellationToken cancellationToken)
    {
        var url = "/demo";
        var client = _httpClientFactory.CreateClient(nameof(BffHttpClient));
        var response = await client.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<StockPriceModel>>(cancellationToken)) ?? [];
    }

    public async Task<IEnumerable<PriceModel>> UpsertPriceAsync(PriceModel price, CancellationToken cancellationToken)
    {
        var url = "/demo/price";
        var client = _httpClientFactory.CreateClient(nameof(BffHttpClient));
        var response = await client.PostAsJsonAsync(url, price, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<PriceModel>>(cancellationToken)) ?? [];
    }

    public async Task<IEnumerable<ProductModel>> UpsertProductAsync(ProductModel model, CancellationToken cancellationToken)
    {
        var url = "/demo/product";
        var client = _httpClientFactory.CreateClient(nameof(BffHttpClient));
        var response = await client.PostAsJsonAsync(url, model, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<ProductModel>>(cancellationToken)) ?? [];
    }

    public async Task<IEnumerable<StockItemModel>> UpsertStockItemAsync(StockItemModel stockItem, CancellationToken cancellationToken)
    {
        var url = "/demo/stock";
        var client = _httpClientFactory.CreateClient(nameof(BffHttpClient));
        var response = await client.PostAsJsonAsync(url, stockItem, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<StockItemModel>>(cancellationToken)) ?? [];
    }
}
