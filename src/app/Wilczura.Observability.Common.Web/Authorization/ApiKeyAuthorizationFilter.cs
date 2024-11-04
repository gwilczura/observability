using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace Wilczura.Observability.Common.Web.Authorization;

public class ApiKeyAuthorizationFilter : IAuthorizationFilter
{
    private const string _apiKeyHeaderName = "X-API-Key";

    private readonly IApiKeyValidator _apiKeyValidator;

    public ApiKeyAuthorizationFilter(IApiKeyValidator apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        bool hasAllowAnonymous = context.ActionDescriptor.EndpointMetadata
                                 .Any(em => em.GetType() == typeof(AllowAnonymousAttribute));
        if(hasAllowAnonymous) { return; }

        string? apiKey = context.HttpContext.Request.Headers[_apiKeyHeaderName];
        if (!_apiKeyValidator.IsValid(apiKey ?? string.Empty))
        {
            context.Result = new UnauthorizedResult();
        }
    }
}
