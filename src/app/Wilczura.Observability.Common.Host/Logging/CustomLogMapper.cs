using Elastic.CommonSchema;
using Elastic.Extensions.Logging;
using Wilczura.Observability.Common.Consts;

namespace Wilczura.Observability.Common.Host.Logging;

public static class CustomLogMapper
{
    // TODO: SHOW P2.5 - map LogInfo to LogEvent
    // Log Event is populated with only some data
    // Here we use the LogInfo to add more data to Log Event 
    // https://github.com/elastic/ecs-dotnet/blob/main/src/Elastic.Extensions.Logging/README.md#tracing-fields
    // https://github.com/elastic/ecs-dotnet/blob/main/src/Elastic.Extensions.Logging/ElasticsearchLogger.cs
    // https://github.com/elastic/ecs-dotnet/blob/main/src/Elastic.CommonSchema/EcsDocument.cs
    // also check:
    // LogTemplateProperties - this is list of all "codes" supported by elasticsearch logger
    public static void Map(LogEvent logEvent)
    {

        // LogTemplateProperties
        logEvent.Agent ??= new Agent();
        logEvent.Agent.Name = "Wilczura";

        var serviceName = NormalizeServiceName(logEvent.Service?.Name);
        if (serviceName != null)
        {
            logEvent!.Service!.Name = serviceName;
        }

        logEvent.User ??= new Elastic.CommonSchema.User();
        logEvent.Client ??= new Elastic.CommonSchema.Client();
        logEvent.Client.User ??= new Elastic.CommonSchema.User();

        var clientUser = logEvent.Client.User;

        if (clientUser.Id != null)
        {
            logEvent.Client.User.Name = KnownIdentities.GetIdentityName(clientUser.Name ?? clientUser.Id);
        }
        else
        {
            var userName = Thread.CurrentPrincipal?.Identity?.Name;
            logEvent.Client.User.Name = KnownIdentities.GetIdentityName(userName);
        }

        var eventCategory = logEvent.Labels?.Where(a => a.Key == "event.category").Select(a=>a.Value).FirstOrDefault();
        if (eventCategory != null)
        {
            logEvent.Event ??= new Elastic.CommonSchema.Event();
            logEvent.Event.Category = [eventCategory];
        }

        // logEvent.Event.Code = LogEvent.Id - reusable event "type" id
        // logEvent.TraceId; - interconnected service calls
        // logEvent.TransactionId; - server request
        // logEvent.SpanId - lowest scope - DB query, external service call
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
