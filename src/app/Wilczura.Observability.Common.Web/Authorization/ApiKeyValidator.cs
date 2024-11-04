using Microsoft.Extensions.Options;
using Wilczura.Observability.Common.Exceptions;
using Wilczura.Observability.Common.Models;

namespace Wilczura.Observability.Common.Web.Authorization;

public class ApiKeyValidator : IApiKeyValidator
{
    string ApiKey;
    public ApiKeyValidator(IOptions<ObsOptions> options)
    {
        ApiKey = options.Value.ApiKey ?? string.Empty;
        if(string.IsNullOrWhiteSpace(ApiKey))
        {
            throw new ObservabilityException("ApiKey can not be empty");
        }
    }

    public bool IsValid(string apiKey)
    {
        return apiKey?.ToLower() == ApiKey.ToLower();
    }
}
