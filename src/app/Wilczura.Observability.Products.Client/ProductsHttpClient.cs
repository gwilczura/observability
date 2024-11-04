using System.Net.Http.Json;
using Wilczura.Observability.Products.Contract;
using Wilczura.Observability.Products.Contract.Models;

namespace Wilczura.Observability.Products.Client;

public class ProductsHttpClient : IProductsHttpClient
{
    private readonly IHttpClientFactory _httpClientFactory;

    public ProductsHttpClient(IHttpClientFactory httpClientFactory)
    {
        _httpClientFactory = httpClientFactory;
    }

    public async Task<IEnumerable<ProductDto>> GetProductsAsync(long? productId, CancellationToken cancellationToken)
    {
        var url = productId.HasValue
            ? $"/product?productId={productId}"
            : $"/product";
        var client = _httpClientFactory.CreateClient(nameof(ProductsHttpClient));
        var response = await client.GetAsync(url, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>(cancellationToken)) ?? [];
    }

    public async Task<IEnumerable<ProductDto>> UpsertProductAsync(ProductDto dto, CancellationToken cancellationToken)
    {
        var url = "/product";
        var client = _httpClientFactory.CreateClient(nameof(ProductsHttpClient));
        var response = await client.PostAsJsonAsync(url, dto, cancellationToken);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<IEnumerable<ProductDto>>(cancellationToken)) ?? [];
    }
}
