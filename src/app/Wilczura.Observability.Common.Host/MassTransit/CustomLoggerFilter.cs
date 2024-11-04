using MassTransit;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Logging;

namespace Wilczura.Observability.Common.Host.MassTransit
{
    public class CustomLoggerFilter<T> : IFilter<T> where T : class, PipeContext
    {
        private readonly ILogger<CustomLoggerFilter<T>> _logger;
        long _exceptionCount;
        long _successCount;
        long _attemptCount;

        public CustomLoggerFilter(ILogger<CustomLoggerFilter<T>> logger)
        {
            _logger = logger;
        }

        public void Probe(ProbeContext context)
        {
            var scope = context.CreateFilterScope("customLogger");
            scope.Add("attempted", _attemptCount);
            scope.Add("succeeded", _successCount);
            scope.Add("faulted", _exceptionCount);
        }

        // TODO: SHOW P3 - MassTransit filter based logging (Consumer)
        public async Task Send(T context, IPipe<T> next)
        {
            var consumeContext = context as ConsumeContext;
            var sendContext = context as SendContext;
            var publishContext = context as PublishContext;

            Exception? exception = null;
            var message = GetMessage(consumeContext, sendContext, publishContext);
            var activityName = GetActivityName(consumeContext, sendContext, publishContext);
            var logInfo = new LogInfo(message, ObservabilityConsts.EventCategoryProcess);
            logInfo.Endpoint = GetEndpoint(consumeContext, sendContext, publishContext);
            logInfo.EventAction = activityName;
            var logScope = new LogScope(_logger, logInfo, LogLevel.Information, LogEvents.WebRequest, activityName: activityName);

            try
            {
                Interlocked.Increment(ref _attemptCount);

                // here the next filter in the pipe is called
                await next.Send(context);

                Interlocked.Increment(ref _successCount);
            }
            catch (Exception ex)
            {
                Interlocked.Increment(ref _exceptionCount);

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

        private string GetMessage(ConsumeContext? consumeContext, SendContext? sendContext, PublishContext? publishContext)
        {
            return consumeContext != null
                ? "Message consume"
                : sendContext != null
                ? "Message send"
                : publishContext != null
                ? "Message publish"
                : "Unknown message processing pipe";
        }

        private string GetActivityName(ConsumeContext? consumeContext, SendContext? sendContext, PublishContext? publishContext)
        {
            return consumeContext != null
                ? "message-consume"
                : sendContext != null
                ? "message-send"
                : publishContext != null
                ? "message-publish"
                : "message-unknown";
        }

        private string GetEndpoint(ConsumeContext? consumeContext, SendContext? sendContext, PublishContext? publishContext)
        {
            return consumeContext != null
                ? $"{consumeContext.DestinationAddress?.LocalPath}"
                : sendContext != null
                ? $"{sendContext.DestinationAddress?.LocalPath}"
                : publishContext != null
                ? $"{publishContext.DestinationAddress?.LocalPath}"
                : "unknown";
        }
    }
}
