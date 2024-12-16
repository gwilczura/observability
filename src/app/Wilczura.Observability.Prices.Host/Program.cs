using MassTransit.Logging;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Wilczura.Observability.Common.Host.Extensions;
using Wilczura.Observability.Common.Host.Models;
using Wilczura.Observability.Prices.Adapters.Controllers;
using Wilczura.Observability.Prices.Adapters.Postgres.Extensions;
using Wilczura.Observability.Prices.Host.Extensions;
using Wilczura.Observability.Products.Client;
using Wilczura.Observability.Stock.Client;

// TODO: SHOW P0 - Show Project Structure

var builder = WebApplication.CreateBuilder(args);
var configName = "Prices";
// Add services to the container.

var logger = builder.GetStartupLogger();
builder.AddCustomHostServices(
    configName, 
    DiagnosticHeaders.DefaultListenerName,
    AuthenticationType.Default,
    new AssemblyPart(typeof(PriceController).Assembly),
    logger);
builder.AddPricesApplication();
builder.AddPricesPostgres(string.Empty);
builder.AddPricesServiceBus(string.Empty);
builder.AddProductsClient();
builder.AddStockClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseObservabilityDefaults();

app.Run();
