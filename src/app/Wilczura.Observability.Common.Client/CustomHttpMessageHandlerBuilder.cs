using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;

namespace Wilczura.Observability.Common.Client;

public class CustomHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
{
    private readonly ILogger<CustomLoggingHttpMessageHandler> _logger;

    public override string? Name { get; set; }
    public override HttpMessageHandler PrimaryHandler { get; set; }
    public override IList<DelegatingHandler> AdditionalHandlers => new List<DelegatingHandler>();

    public CustomHttpMessageHandlerBuilder(ILogger<CustomLoggingHttpMessageHandler> logger)
    {
        _logger = logger;
        PrimaryHandler = new CustomLoggingHttpMessageHandler(logger);
    }

    public override HttpMessageHandler Build()
    {
        return new CustomLoggingHttpMessageHandler(_logger);
    }
}
