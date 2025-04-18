﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Security;
using Wilczura.Observability.Stock.Contract;

namespace Wilczura.Observability.Stock.Client;

public static class Extensions
{
    public static IHostApplicationBuilder AddStockClient(this IHostApplicationBuilder app)
    {
        var stockHttpClientSection = app.Configuration.GetSection(ObservabilityConsts.HttpCLientSectionName).GetSection("Stock")!;
        app.Services.AddScoped<IStockHttpClient, StockHttpClient>();
        app.Services.Configure<CustomHttpClientOptions>(nameof(StockHttpClient), stockHttpClientSection);
        app.Services.AddHttpClient<StockHttpClient>((services, httpClient) =>
        {
            var principalProvider = services.GetRequiredService<ICustomPrincipalProvider>();
            var options = services.GetRequiredService<IOptionsMonitor<CustomHttpClientOptions>>().Get(nameof(StockHttpClient));
            var token = principalProvider.GetTokenAsync(options!.Scopes!).Result;
            httpClient.BaseAddress = new Uri(options!.BaseUrl!);
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(ObservabilityConsts.BearerAuthKey, token);
        });

        return app;
    }
}
