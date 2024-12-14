using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Security;
using Wilczura.Observability.Prices.Contract;

namespace Wilczura.Observability.Prices.Client;

public static class Extensions
{
    public static IHostApplicationBuilder AddPricesClient(this IHostApplicationBuilder app)
    {
        var pricesHttpClientSection = app.Configuration.GetSection(ObservabilityConsts.HttpCLientSectionName).GetSection("Prices")!;
        app.Services.AddScoped<IPricesHttpClient, PricesHttpClient>();
        app.Services.Configure<CustomHttpClientOptions>(nameof(PricesHttpClient), pricesHttpClientSection);
        app.Services.AddHttpClient<PricesHttpClient>((services, httpClient) =>
        {
            var principalProvider = services.GetRequiredService<ICustomPrincipalProvider>();
            var options = services.GetRequiredService<IOptionsMonitor<CustomHttpClientOptions>>().Get(nameof(PricesHttpClient));
            var token = principalProvider.GetTokenAsync(options!.Scopes!).Result;
            httpClient.BaseAddress = new Uri(options!.BaseUrl!);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ObservabilityConsts.BearerAuthKey, token);
        });

        return app;
    }
}
