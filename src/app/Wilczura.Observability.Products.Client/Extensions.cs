using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Security;
using Wilczura.Observability.Products.Contract;

namespace Wilczura.Observability.Products.Client;

public static class Extensions
{
    public static IHostApplicationBuilder AddProductsClient(this IHostApplicationBuilder app)
    {
        var productsHttpClientSection = app.Configuration.GetSection(ObservabilityConsts.HttpCLientSectionName).GetSection("Products")!;
        app.Services.AddScoped<IProductsHttpClient, ProductsHttpClient>();
        app.Services.Configure<CustomHttpClientOptions>(nameof(ProductsHttpClient), productsHttpClientSection);
        app.Services.AddHttpClient<ProductsHttpClient>((services, httpClient) =>
        {
            var principalProvider = services.GetRequiredService<ICustomPrincipalProvider>();
            var options = services.GetRequiredService<IOptionsMonitor<CustomHttpClientOptions>>().Get(nameof(ProductsHttpClient));
            var token = principalProvider.GetTokenAsync(options!.Scopes!).Result;
            httpClient.BaseAddress = new Uri(options!.BaseUrl!);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ObservabilityConsts.BearerAuthKey, token);
        });

        return app;
    }
}
