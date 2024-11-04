using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Security.Principal;

namespace Wilczura.Observability.Common.Web.Extensions;

public static class HttpContextExtensions
{
    public static IPrincipal? GetPrincipal(this HttpContext httpContext)
    {
        var threadUser = Thread.CurrentPrincipal ?? httpContext.User;
        var feature = httpContext.Features.Get<IAuthenticateResultFeature>();
        var user = threadUser ?? feature?.AuthenticateResult?.Principal;
        return user;
    }
}
