using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Wilczura.Observability.Common.Models;
using Wilczura.Observability.Common.Security;
using Wilczura.Observability.LoadRunner.Host.Client;

namespace Wilczura.Observability.LoadRunner.Host.Extensions;

public static class HostExtensions
{

    public static IHostApplicationBuilder AddBffClient(this IHostApplicationBuilder app)
    {
        var bffHttpClientSection = app.Configuration.GetSection("HttpClient").GetSection("Bff")!;
        app.Services.AddScoped<IBffHttpClient, BffHttpClient>();
        app.Services.Configure<CustomHttpClientOptions>(nameof(BffHttpClient), bffHttpClientSection);
        app.Services.AddHttpClient<BffHttpClient>((services, httpClient) =>
        {
            var options = services.GetRequiredService<IOptionsMonitor<CustomHttpClientOptions>>().Get(nameof(BffHttpClient));
            var keyOptions = services.GetRequiredService<IOptionsMonitor<ObsOptions>>();
            httpClient.BaseAddress = new Uri(options!.BaseUrl!);
            httpClient.DefaultRequestHeaders.Add("X-API-Key", keyOptions.CurrentValue.ApiKey);
        });

        return app;
    }
}
