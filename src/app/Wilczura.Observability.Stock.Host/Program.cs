using MassTransit.Logging;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Wilczura.Observability.Common.Host.Extensions;
using Wilczura.Observability.Common.Host.Models;
using Wilczura.Observability.Products.Client;
using Wilczura.Observability.Stock.Adapters.Controllers;
using Wilczura.Observability.Stock.Adapters.Postgres.Extensions;
using Wilczura.Observability.Stock.Host.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configName = "Stock";
// Add services to the container.
var logger = builder.GetStartupLogger();
builder.AddCustomHostServices(
    configName, 
    DiagnosticHeaders.DefaultListenerName,
    AuthenticationType.Default, 
    new AssemblyPart(typeof(StockController).Assembly),
    logger);
builder.AddStockApplication();
builder.AddStockPostgres(string.Empty);
builder.AddStockServiceBus(string.Empty);
builder.AddProductsClient();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseObservabilityDefaults();

app.Run();
