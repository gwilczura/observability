using Microsoft.Extensions.Logging;
using System.Text.Json;
using Wilczura.Observability.Common.Consts;

namespace Wilczura.Observability.Common.Logging;

public static class LoggingExtensions
{

    public static void LogInformation(this ILogger logger, string message, EventId? eventId = null)
    {
        logger.LogInformation(new LogInfo(message, ObservabilityConsts.EventCategoryProcess), eventId);
    }

    public static void LogInformation(this ILogger logger, LogInfo logInfo, EventId? eventId = null)
    {
        logger.Log(LogLevel.Information, logInfo, eventId);
    }

    public static void Log(this ILogger logger, LogLevel logLevel, LogInfo logInfo, EventId? eventId = null)
    {
        logger.Log(logLevel, eventId ?? LogEvents.Custom, logInfo, null, DictionaryMessageFormatter<LogInfo>);
    }

    // TODO: SHOW P2.4 - custom log message formatter
    private static string MessageFormatter<T>(T state, Exception? exception)
    {
        return JsonSerializer.Serialize(state);
    }

    private static string DictionaryMessageFormatter<T>(T state, Exception? exception)
    {
        if (state is IDictionary<string, object> dictionary)
        {
            if (dictionary.TryGetValue("message", out var message))
            {
                return (string)message;
            }

            return "missing [message] key";
        }

        return "unsupported type, provide dictionary<string,object>";
    }
}
