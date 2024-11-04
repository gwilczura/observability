using Microsoft.AspNetCore.Mvc;

namespace Wilczura.Observability.Common.Web.Authorization;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}
