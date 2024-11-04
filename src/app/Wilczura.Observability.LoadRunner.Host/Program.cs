using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Wilczura.Observability.Common.Host.Extensions;
using Wilczura.Observability.Common.Models;
using Wilczura.Observability.LoadRunner.Host.Extensions;
using Wilczura.Observability.LoadRunner.Host.ScenarioWorkers;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);
builder.Configuration.AddJsonFile("appsettings.json");
var configName = "LoadRunner";
var bffConfigName = "Bff";
builder.AddConfigurationFromKeyVault(configName);
var config = builder.Configuration.GetSection(bffConfigName);
builder.Services.Configure<ObsOptions>(config);
builder.AddBffClient();

builder.Services.AddHostedService<ProductCreationWorker>();
builder.Services.AddHostedService<StockInitializationWorker>();
builder.Services.AddHostedService<StockReplenishingWorker>();
builder.Services.AddHostedService<PriceSettingWorker>();
builder.Services.AddHostedService<StockSubtractWorker>();
IHost host = builder.Build();
host.Run();