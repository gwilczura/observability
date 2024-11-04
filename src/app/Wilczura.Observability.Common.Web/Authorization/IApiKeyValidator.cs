namespace Wilczura.Observability.Common.Web.Authorization;

public interface IApiKeyValidator
{
    bool IsValid(string apiKey);
}
