using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Wilczura.Observability.Bff.Adapters.Controllers;
using Wilczura.Observability.Bff.Host.Extensions;
using Wilczura.Observability.Common.Host.Extensions;
using Wilczura.Observability.Common.Host.Models;
using Wilczura.Observability.Prices.Client;
using Wilczura.Observability.Products.Client;
using Wilczura.Observability.Stock.Client;

var builder = WebApplication.CreateBuilder(args);
var configName = "Bff";
// Add services to the container.
builder.AddCustomHostServices(
    configName, 
    mtActivitySourceName: string.Empty,
    AuthenticationType.ApiKey,
    new AssemblyPart(typeof(DemoController).Assembly));
builder.AddBffApplication();
builder.AddPricesClient();
builder.AddProductsClient();
builder.AddStockClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseObservabilityDefaults();

app.Run();
