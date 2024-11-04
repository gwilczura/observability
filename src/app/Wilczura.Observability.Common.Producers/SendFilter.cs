using MassTransit;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Logging;

namespace Wilczura.Observability.Common.Producers;

public class SendFilter<T> : IFilter<SendContext<T>> where T : class
{
    private readonly ILogger<SendFilter<T>> _logger;

    public SendFilter(ILogger<SendFilter<T>> logger)
    {
        _logger = logger;
    }

    public void Probe(ProbeContext context)
    {
    }

    // TODO: SHOW P3 - MassTransit filter based logging (Send)
    public async Task Send(SendContext<T> context, IPipe<SendContext<T>> next)
    {
        Exception? exception = null;
        var message = "Message send";
        var activityName = "message-send";
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
