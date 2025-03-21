using MassTransit;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Logging;

namespace Wilczura.Observability.Common.ServiceBus;

public class ConsumeFilter<T> : IFilter<ConsumeContext<T>> where T : class
{
    private readonly ILogger<ConsumeFilter<T>> _logger;

    public ConsumeFilter(ILogger<ConsumeFilter<T>> logger)
    {
        _logger = logger;
    }

    public void Probe(ProbeContext context)
    {
    }

    // TODO: SHOW P3.3 - MassTransit filter based logging (Consume)
    public async Task Send(ConsumeContext<T> context, IPipe<ConsumeContext<T>> next)
    {
        Exception? exception = null;
        var message = "Message consume";
        var activityName = "message-consume";
        var logInfo = new LogInfo(message, ObservabilityConsts.EventCategoryProcess);
        logInfo.Endpoint = $"{context.DestinationAddress?.LocalPath}";
        logInfo.EventAction = activityName;
        var logScope = new LogScope(_logger, logInfo, LogLevel.Information, LogEvents.ConsumeMessage, activityName: activityName);
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
