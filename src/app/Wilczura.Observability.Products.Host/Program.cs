using MassTransit.Logging;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Wilczura.Observability.Common.Host.Extensions;
using Wilczura.Observability.Common.Host.Models;
using Wilczura.Observability.Products.Adapters.Controllers;
using Wilczura.Observability.Products.Adapters.Postgres.Extensions;
using Wilczura.Observability.Products.Adapters.ServiceBus.Extensions;
using Wilczura.Observability.Products.Host.Extensions;

var builder = WebApplication.CreateBuilder(args);
var configName = "Products";
// Add services to the container.
builder.AddCustomHostServices(
    configName, 
    DiagnosticHeaders.DefaultListenerName,
    AuthenticationType.Default,
    new AssemblyPart(typeof(ProductController).Assembly));
builder.AddProductsApplication();
builder.AddProductsPostgres(string.Empty);
builder.AddProductsServiceBus(string.Empty);
builder.AddSharedServiceBus();

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseObservabilityDefaults();

app.Run();
