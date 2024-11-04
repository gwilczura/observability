using MassTransit;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Logging;

namespace Wilczura.Observability.Common.Producers;

public class PublishFilter<T> : IFilter<PublishContext<T>> where T : class
{
    private readonly ILogger<PublishFilter<T>> _logger;

    public PublishFilter(ILogger<PublishFilter<T>> logger)
    {
        _logger = logger;
    }

    public void Probe(ProbeContext context)
    {
    }

    // TODO: SHOW P3 - MassTransit filter based logging (Publish)
    public async Task Send(PublishContext<T> context, IPipe<PublishContext<T>> next)
    {
        Exception? exception = null;
        var message = "Message publish";
        var activityName = "message-publish";
        var logInfo = new LogInfo(message, ObservabilityConsts.EventCategoryProcess);
        logInfo.Endpoint = $"{context.DestinationAddress?.LocalPath}";
        logInfo.EventAction = activityName;
        var logScope = new LogScope(_logger, logInfo, LogLevel.Information, LogEvents.WebRequest, activityName: activityName);
        try
        {
            await next.Send(context);
        }
        catch (Exception ex)
        {
            exception = ex;
            throw;
        }
        finally
        {
            if (exception != null)
            {
                logInfo.ApplyException(exception);
            }
            else
            {
                logInfo.EventOutcome = ObservabilityConsts.EventOutcomeSuccess;
            }

            logScope.Dispose();
        }
    }
}
