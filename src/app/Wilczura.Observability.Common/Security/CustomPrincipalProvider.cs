using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;

namespace Wilczura.Observability.Common.Security;

public class CustomPrincipalProvider : ICustomPrincipalProvider
{
    private readonly ConfidentialClientApplicationOptions _options;
    private readonly IConfidentialClientApplication _application;

    public CustomPrincipalProvider(IOptions<ConfidentialClientApplicationOptions> options)
    {
        _options = options.Value;
        _application = ConfidentialClientApplicationBuilder.CreateWithApplicationOptions(_options).Build();
    }

    public async Task<string> GetTokenAsync(IEnumerable<string> scopes)
    {
        var result = await _application.AcquireTokenForClient(scopes).ExecuteAsync();
        return result.AccessToken;
    }
}
