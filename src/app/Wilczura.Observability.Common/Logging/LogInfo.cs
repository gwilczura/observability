using System.Security.Principal;
using Wilczura.Observability.Common.Consts;

namespace Wilczura.Observability.Common.Logging;

public class LogInfo : Dictionary<string, object>
{
    public static LogInfo From(string message) => new LogInfo(message, ObservabilityConsts.EventCategoryProcess);


    // could also use
    // LogTemplateProperties - this is list of all "codes" supported by elasticsearch logger
    // but would have to reference ECS nuget
    private const string messageKey = "message";
    public string Message 
    {   
        get
        {
            TryGetValue(eventDurationKey, out object? message);
            return message != null ? (string)message : string.Empty;
        }
        set
        {
            this[messageKey] = value;
        }
    }


    private const string eventDurationKey = "event.duration";
    // TODO: SHOW P5 - not nanoseconds
    /// <summary>
    /// Should be nanoseconds according to ECS spec :)
    /// </summary>
    public long? EventDuration
    {
        get
        {
            TryGetValue(eventDurationKey, out object? duration);
            return duration != null ? (long)duration : null;
        }
        set
        {
            if (value != null)
            {
                this[eventDurationKey] = value;
            }
            else
            {
                Remove(eventDurationKey);
            }
        }
    }

    private const string eventActionKey = "event.action";
    public string? EventAction
    {
        get
        {
            TryGetValue(eventActionKey, out object? eventAction);
            return eventAction != null ? (string)eventAction : string.Empty;
        }
        set
        {
            if (value != null)
            {
                this[eventActionKey] = value;
            }
            else
            {
                Remove(eventActionKey);
            }
        }
    }

    private const string eventReasonKey = "event.reason";
    public string? EventReason
    {
        get
        {
            TryGetValue(eventReasonKey, out object? eventReason);
            return eventReason != null ? (string)eventReason : string.Empty;
        }
        set
        {
            if (value != null)
            {
                this[eventReasonKey] = value;
            }
            else
            {
                Remove(eventReasonKey);
            }
        }
    }

    private const string eventOutcomeKey = "event.outcome";
    /// <summary>
    /// https://www.elastic.co/guide/en/ecs/current/ecs-allowed-values-event-outcome.html
    /// </summary>
    public string? EventOutcome
    {
        get
        {
            TryGetValue(eventOutcomeKey, out object? eventOutcom);
            return eventOutcom != null ? (string)eventOutcom : string.Empty;
        }
        set
        {
            if (value != null)
            {
                this[eventOutcomeKey] = value;
            }
            else
            {
                Remove(eventOutcomeKey);
            }
        }
    }

    private const string eventCategoryKey = "event.category";
    // TODO: SHOW P6 - event category is string
    /// <summary>
    /// https://www.elastic.co/guide/en/ecs/current/ecs-allowed-values-event-category.html
    /// </summary>
    public string? EventCategory
    {
        get
        {
            TryGetValue(eventCategoryKey, out object? eventCategory);
            return eventCategory != null ? (string)eventCategory : string.Empty;
        }
        set
        {
            if (value != null)
            {
                this[eventCategoryKey] = value;
            }
            else
            {
                Remove(eventCategoryKey);
            }
        }
    }

    private const string clientUserNameKey = "client.user.name";
    public string? UserName
    {
        get
        {
            TryGetValue(clientUserNameKey, out object? clientUserName);
            return clientUserName != null ? (string)clientUserName : string.Empty;
        }
        set
        {
            if (value != null)
            {
                this[clientUserNameKey] = value;
            }
            else
            {
                Remove(clientUserNameKey);
            }
        }
    }

    private const string clientUserIdKey = "client.user.id";
    public string? UserId
    {
        get
        {
            TryGetValue(clientUserIdKey, out object? clientUserId);
            return clientUserId != null ? (string)clientUserId : string.Empty;
        }
        set
        {
            if (value != null)
            {
                this[clientUserIdKey] = value;
            }
            else
            {
                Remove(clientUserIdKey);
            }
        }
    }

    private const string httpRequestMethodKey = "http.request.method";
    public string? HttpMethod
    {
        get
        {
            TryGetValue(httpRequestMethodKey, out object? httpRequestMethod);
            return httpRequestMethod != null ? (string)httpRequestMethod : string.Empty;
        }
        set
        {
            if (value != null)
            {
                this[httpRequestMethodKey] = value;
            }
            else
            {
                Remove(httpRequestMethodKey);
            }
        }
    }


    private const string urlPathKey = "url.path";
    public string? Endpoint
    {
        get
        {
            TryGetValue(urlPathKey, out object? urlPath);
            return urlPath != null ? (string)urlPath : string.Empty;
        }
        set
        {
            if (value != null)
            {
                this[urlPathKey] = value;
            }
            else
            {
                Remove(urlPathKey);
            }
        }
    }

    public string? ExceptionMessage { get; set; }

    public LogInfo(string message, string eventCategory)
    {
        Message = message;
        EventCategory = eventCategory;
    }

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
