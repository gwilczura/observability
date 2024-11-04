namespace Wilczura.Observability.Common.Security;

public interface ICustomPrincipalProvider
{
    Task<string> GetTokenAsync(IEnumerable<string> scopes);
}
