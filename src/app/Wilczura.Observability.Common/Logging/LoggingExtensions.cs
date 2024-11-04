using Microsoft.Extensions.Logging;
using System.Text.Json;
using Wilczura.Observability.Common.Consts;

namespace Wilczura.Observability.Common.Logging;

public static class LoggingExtensions
{
    // TODO: SHOW P2 - custom log message formatter
    private static string MessageFormatter<T>(T state, Exception? exception)
    {
        return JsonSerializer.Serialize(state);
    }

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
        logger.Log(logLevel, eventId ?? LogEvents.Custom, logInfo, null, MessageFormatter<LogInfo>);
    }
}
