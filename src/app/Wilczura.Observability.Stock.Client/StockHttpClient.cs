using System.Net.Http.Json;
using Wilczura.Observability.Stock.Contract;
using Wilczura.Observability.Stock.Contract.Models;

namespace Wilczura.Observability.Stock.Client;

public class StockHttpClient : IStockHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public StockHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<StockItemDto>> GetStockItemsAsync(long? stockItemId, CancellationToken cancellationToken)
    {
        var url = stockItemId.HasValue
            ? $"/stock?stockItemId={stockItemId}"
            : $"/stock";
        var client = _httpClientFactory.CreateClient(nameof(StockHttpClient));
        var response = await client.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<StockItemDto>>(cancellationToken)) ?? [];
    }

    public async Task<IEnumerable<StockItemDto>> UpsertStockItemAsync(StockItemDto dto, CancellationToken cancellationToken)
    {
        var url = "/stock";
        var client = _httpClientFactory.CreateClient(nameof(StockHttpClient));
        var response = await client.PostAsJsonAsync(url, dto, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<StockItemDto>>(cancellationToken)) ?? [];
    }

    public async Task<IEnumerable<StockItemDto>> ChangeStockAsync(StockChangeDto dto, CancellationToken cancellationToken)
    {
        var url = "/stock/change";
        var client = _httpClientFactory.CreateClient(nameof(StockHttpClient));
        var response = await client.PostAsJsonAsync(url, dto, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<StockItemDto>>(cancellationToken)) ?? [];
    }

    
}
