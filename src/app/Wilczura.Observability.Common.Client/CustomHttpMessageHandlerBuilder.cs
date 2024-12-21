using Microsoft.Extensions.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Wilczura.Observability.Common.Models;

namespace Wilczura.Observability.Common.Client;

public class CustomHttpMessageHandlerBuilder : HttpMessageHandlerBuilder
{
    private readonly ILogger<CustomLoggingHttpMessageHandler> _logger;
    private readonly IOptionsSnapshot<ObsOptions> _optionsSnapshot;

    public override string? Name { get; set; }
    public override HttpMessageHandler PrimaryHandler { get; set; }
    public override IList<DelegatingHandler> AdditionalHandlers => new List<DelegatingHandler>();

    public CustomHttpMessageHandlerBuilder(
        ILogger<CustomLoggingHttpMessageHandler> logger,
        IOptionsSnapshot<ObsOptions> optionsSnapshot)
    {
        _logger = logger;
        _optionsSnapshot = optionsSnapshot;
        PrimaryHandler = new CustomLoggingHttpMessageHandler(logger, optionsSnapshot.Value);
    }

    public override HttpMessageHandler Build()
    {
        return new CustomLoggingHttpMessageHandler(_logger, _optionsSnapshot.Value);
    }
}
