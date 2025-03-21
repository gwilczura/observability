using Elastic.Extensions.Logging;
using System.Text.Json;
using System.Text.Json.Serialization;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Logging;

namespace Wilczura.Observability.Common.Host.Logging;

public static class CustomLogMapper
{
    private const string ClosingBracket = "}";
    private const string OpeningBracket = "{";
    private static readonly JsonSerializerOptions _serializerOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true,
        UnmappedMemberHandling = JsonUnmappedMemberHandling.Disallow
    };

    // TODO: SHOW P2.5 - map LogInfo to LogEvent
    // Log Event is populated with only some data
    // Here we use the LogInfo to add more data to Log Event 
    // https://github.com/elastic/ecs-dotnet/blob/main/src/Elastic.Extensions.Logging/README.md#tracing-fields
    // https://github.com/elastic/ecs-dotnet/blob/main/src/Elastic.Extensions.Logging/ElasticsearchLogger.cs
    // https://github.com/elastic/ecs-dotnet/blob/main/src/Elastic.CommonSchema/EcsDocument.cs
    public static void Map(LogEvent logEvent)
    {
        if (logEvent.Message != null && logEvent.Message.StartsWith(OpeningBracket) && logEvent.Message.EndsWith(ClosingBracket))
        {
            try
            {
                var logInfo = JsonSerializer.Deserialize<LogInfo>(logEvent.Message, _serializerOptions);
                if(logInfo != null)
                {
                    logEvent.Message = logInfo.Message ?? logEvent.Message;

                    logEvent.Agent ??= new Elastic.CommonSchema.Agent();
                    logEvent.Agent.Name = "Wilczura";

                    logEvent.Event ??= new Elastic.CommonSchema.Event();
                    logEvent.Event.Duration = logInfo.EventDuration;
                    logEvent.Event.Action = logInfo.EventAction;
                    if (logInfo.EventCategory != null)
                    {
                        logEvent.Event.Category = [logInfo.EventCategory];
                    }
                    logEvent.Event.Reason = logInfo.EventReason;
                    logEvent.Event.Outcome = logInfo.EventOutcome;

                    logEvent.User ??= new Elastic.CommonSchema.User();
                    logEvent.Client ??= new Elastic.CommonSchema.Client();
                    logEvent.Client.User ??= new Elastic.CommonSchema.User();
                    if(logInfo.UserId != null)
                    {
                        logEvent.Client.User.Id = logInfo.UserId;
                        logEvent.Client.User.Name = KnownIdentities.GetIdentityName(logInfo.UserName ?? logInfo.UserId);
                    }
                    else
                    {
                        var userName = Thread.CurrentPrincipal?.Identity?.Name;
                        logEvent.Client.User.Name = KnownIdentities.GetIdentityName(userName);
                    }

                    if(logInfo.HttpMethod != null)
                    {
                        logEvent.Http ??= new Elastic.CommonSchema.Http();
                        logEvent.Http.RequestMethod = logInfo.HttpMethod;
                    }

                    if(logInfo.Endpoint != null)
                    {
                        logEvent.Labels ??= new Elastic.CommonSchema.Labels();
                        logEvent.Labels.Add("endpoint", logInfo.Endpoint);

                        logEvent.Url ??= new Elastic.CommonSchema.Url();
                        logEvent.Url.Path = logInfo.Endpoint;
                    };

                    var serviceName = NormalizeServiceName(logEvent.Service?.Name);
                    if(serviceName != null)
                    {
                        logEvent!.Service!.Name = serviceName;
                    }

                    // logEvent.Event.Code = LogEvent.Id - reusable event "type" id
                    // logEvent.TraceId; - interconnected service calls
                    // logEvent.TransactionId; - server request
                    // logEvent.SpanId - lowest scope - DB query, external service call
                }
            }
            catch { }
        }
    }

    private static string? NormalizeServiceName(string? serviceName)
    {
        if(serviceName == null)
        {
            return serviceName;
        }

        return serviceName.ToLowerInvariant()
            .Replace("wilczura.observability.", string.Empty)
            .Replace(".host", string.Empty);
    }
}
