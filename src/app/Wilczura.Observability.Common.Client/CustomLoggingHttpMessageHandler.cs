using Elastic.Apm;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Logging;

namespace Wilczura.Observability.Common.Client;

public class CustomLoggingHttpMessageHandler : HttpClientHandler
{
    private readonly ILogger<CustomLoggingHttpMessageHandler> _logger;

    public CustomLoggingHttpMessageHandler(ILogger<CustomLoggingHttpMessageHandler> logger)
    {
        _logger = logger;
    }

    //TODO: SHOW P3 - HttpMessageHandler Logging (HttpClient)
    protected async override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        Exception? exception = null;
        HttpResponseMessage? responseMessage = null;
        var message = "Http out";
        var activityName = "http-out";
        var logInfo = new LogInfo(message, ObservabilityConsts.EventCategoryWeb);
        Func<Task<HttpResponseMessage>> action = async () =>
        {
            logInfo.HttpMethod = request.Method.Method;
            logInfo.Endpoint = request.RequestUri?.LocalPath;
            logInfo.EventAction = activityName;
            var logScope = new LogScope(_logger, logInfo, LogLevel.Information, LogEvents.WebRequest, activityName: activityName);
            try
            {
                responseMessage = await base.SendAsync(request, cancellationToken);
                return responseMessage;
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
                else if (responseMessage?.IsSuccessStatusCode == false)
                {
                    logInfo.EventReason = responseMessage.ReasonPhrase;
                    logInfo.EventOutcome = ObservabilityConsts.EventOutcomeFailure;
                }
                else
                {
                    logInfo.EventOutcome = ObservabilityConsts.EventOutcomeSuccess;
                }

                logScope.Dispose();
            }
        };

        return await Agent.Tracer.CaptureTransaction(activityName, nameof(LogEvents.WebRequest), action);
    }
}
