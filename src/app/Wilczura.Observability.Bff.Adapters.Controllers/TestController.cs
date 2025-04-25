using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Wilczura.Observability.Bff.Ports.Services;
using Wilczura.Observability.Common.Consts;
using Wilczura.Observability.Common.Logging;

namespace Wilczura.Observability.Bff.Adapters.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{

    private readonly ILogger<TestController> _logger;

    public TestController(ILogger<TestController> logger,
        IDemoService demoService)
    {
        _logger = logger;
    }

    [HttpGet]
    [AllowAnonymous]
    public ActionResult Get()
    {
        // TODO: SHOW P1.5 - Test Controller - example logs

        // base logger method uses "formatter"
        string formatter(LogInfo info, Exception? exception)
        {
            return info.Message + " formatted";
        };

        // this is base ILogger method
        _logger.Log(LogLevel.Information, new EventId(12345, "Test"), new LogInfo("test 1", ObservabilityConsts.EventCategoryWeb), null, formatter);

        // elastic search logger
        // https://github.com/elastic/ecs-dotnet/blob/991cd3606b000afc22e973c4f5cb9c249af361bf/src/Elastic.Extensions.Logging.Common/LogEventBuilderExtensions.cs#L60
        _logger.LogInformation("test 2 {QueryString}", "someRandomQueryString");

        string dictionaryFormatter(IDictionary<string, object> dictionary, Exception? exception)
        {
            return "test 3 dictionary";
        }

        // https://github.com/elastic/ecs-dotnet/blob/991cd3606b000afc22e973c4f5cb9c249af361bf/tools/Elastic.CommonSchema.Generator/Views/PropDispatch.Generated.cshtml#L47
        var state = new Dictionary<string, object>()
        {
            // this works
            {"QueryString","queryStringFromDictionary"},
            {"event.reason","test-reason"},
            // this does not work
            {"Event.Outcome","test-outcome"}
        };

        _logger.Log(LogLevel.Information, new EventId(12346, "Test"), state, null, dictionaryFormatter);

        _logger.LogInformation(new LogInfo("test 4", ObservabilityConsts.EventCategoryWeb)
        {
            UserName = "some-username",
            UserId = "ID12345",
            EventAction = "some-action",
            EventReason = "test-reason",
            EventDuration = 12345,
            HttpMethod = "GET",
            Endpoint = "test-endpoint",
            EventOutcome = ObservabilityConsts.EventOutcomeSuccess,
        });

        return Ok();
    }
}
