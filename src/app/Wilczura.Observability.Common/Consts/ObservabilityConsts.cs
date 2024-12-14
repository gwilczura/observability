namespace Wilczura.Observability.Common.Consts;

public static class ObservabilityConsts
{
    public const string BearerAuthKey = "Bearer";

    public const string HttpCLientSectionName = "HttpClient";

    public const string ServicePrincipalKey = "ServicePrincipal";
    public const string KeyVaultNameKey = "KeyVaultName";

    public const string ApplicationNameClaim = "appid";

    public const string DefaultListenerName = "WilczuraObservability";


    public const string EventCategoryProcess = "process";
    public const string EventCategoryWeb = "web";
    public const string EventOutcomeSuccess = "success";
    public const string EventOutcomeFailure = "failure";
    public const string EventOutcomeUnknown = "unknown";
    public const string EvenReasonException = "Exception";

    

}
