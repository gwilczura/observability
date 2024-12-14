using MassTransit;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using Wilczura.Observability.Common.Activities;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Logging;

namespace Wilczura.Observability.Common.ServiceBus
{
    public class ConsumeObserver : IConsumeObserver
    {
        private readonly ILogger<ConsumeObserver> _logger;
        private const string _activityName = "message-consume";
        private const string _activityActionName = "Message consumed";

        public ConsumeObserver(ILogger<ConsumeObserver> logger) 
        {
            _logger = logger;
        }

        // TODO: SHOW P5 - Consume Observer not working :) ︽︾↑↓▲▼
        // consume observer is registered as singleton
        public async Task PreConsume<T>(ConsumeContext<T> context) where T : class
        {
            await Task.CompletedTask;
            // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            _ = CustomActivitySource.Source.Value.StartActivity(_activityName, ActivityKind.Consumer);
            // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
        }

        public async Task PostConsume<T>(ConsumeContext<T> context) where T : class
        {
            // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            var activity = Activity.Current;
            // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            if (activity != null && activity.OperationName == _activityName) // this is false
            {
                var info = new LogInfo(activity.DisplayName, ObservabilityConsts.EventCategoryProcess);
                info.EventReason = _activityActionName;
                info.EventAction = _activityActionName;
                info.EventOutcome = ObservabilityConsts.EventOutcomeSuccess;
                info.EventDuration = (long)activity.Duration.TotalMilliseconds;
                _logger.LogInformation(info, LogEvents.ConsumeMessage);
                activity.Dispose();
            }

            await Task.CompletedTask;
        }

        public async Task ConsumeFault<T>(ConsumeContext<T> context, Exception exception) where T : class
        {
            // ↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓↓
            var activity = Activity.Current;
            // ↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑↑
            if (activity != null && activity.OperationName == _activityName) // this is false
            {
                var info = new LogInfo(activity.DisplayName, ObservabilityConsts.EventCategoryProcess);
                info.ExceptionMessage = exception.Message;
                info.EventReason = exception.Message;
                info.EventAction = _activityActionName;
                info.EventOutcome = ObservabilityConsts.EventOutcomeFailure;
                info.EventDuration = (long)activity.Duration.TotalMilliseconds;
                _logger.LogInformation(info, LogEvents.ConsumeMessage);
                activity.Dispose();
            }
            await Task.CompletedTask;
        }
    }
}
