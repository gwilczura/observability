using System.Net.Http.Json;
using Wilczura.Observability.Prices.Contract;
using Wilczura.Observability.Prices.Contract.Models;

namespace Wilczura.Observability.Prices.Client;

public class PricesHttpClient : IPricesHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public PricesHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<PriceDetailsDto>> GetPricesAsync(CancellationToken cancellationToken)
    {
        var client = _httpClientFactory.CreateClient(nameof(PricesHttpClient));
        var response = await client.GetAsync("/price", cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<PriceDetailsDto>>(cancellationToken)) ?? [];
    }

    public async Task<IEnumerable<PriceDetailsDto>> UpsertPriceAsync(PriceDetailsDto dto, CancellationToken cancellationToken)
    {
        var url = "/price";
        var client = _httpClientFactory.CreateClient(nameof(PricesHttpClient));
        var response = await client.PostAsJsonAsync(url, dto, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<PriceDetailsDto>>(cancellationToken)) ?? [];
    }
}
