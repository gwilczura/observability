using System;
using System.Security.Principal;
using Wilczura.Observability.Common.Consts;

namespace Wilczura.Observability.Common.Logging;

public class LogInfo(string message, string eventCategory)
{
    public static LogInfo From(string message) => new LogInfo(message, ObservabilityConsts.EventCategoryProcess);
    public string Message { get; set; } = message;

    // TODO: SHOW P5 - not nanoseconds
    /// <summary>
    /// Should be nanoseconds according to ECS spec :)
    /// </summary>
    public long? EventDuration { get; set; }

    public string? EventAction { get; set; }

    public string? EventReason { get; set; }

    /// <summary>
    /// https://www.elastic.co/guide/en/ecs/current/ecs-allowed-values-event-outcome.html
    /// </summary>
    public string? EventOutcome { get; set; }

    // TODO: SHOW P6 - event category is string
    /// <summary>
    /// https://www.elastic.co/guide/en/ecs/current/ecs-allowed-values-event-category.html
    /// </summary>
    public string? EventCategory { get; set; } = eventCategory;

    public string? UserName { get; set; }

    public string? UserId { get; set;}

    public string? HttpMethod {  get; set; }

    public string? Endpoint { get; set; }

    public string? ExceptionMessage { get; set; }

    public void ApplyPrincipal(IPrincipal? principal)
    {
        if (principal?.Identity != null && principal.Identity.IsAuthenticated)
        {
            UserName = principal.Identity.Name;
            UserId = principal.Identity.Name;
        }
    }

    public void ApplyException(Exception exception)
    {
        ExceptionMessage = exception.Message;
        EventReason = exception.Message;
        EventOutcome = ObservabilityConsts.EventOutcomeFailure;
    }
}
