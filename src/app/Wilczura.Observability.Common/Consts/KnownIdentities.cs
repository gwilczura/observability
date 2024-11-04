namespace Wilczura.Observability.Common.Consts;

public static class KnownIdentities
{
    public static readonly Dictionary<string, string> IdentityNames = new()
    {
        { "85fba9bf-3c86-45ea-8f92-419f4a18a2c3","prices" },
        { "e3f827c7-087c-4a3f-9870-fc284a81a2b9","stock" },
        { "bdbd8acd-8e20-43c7-ba1c-63415c4417f7","bff" },
        { "df57983d-ba4d-461d-a253-faffb49348f4","products" }
    };

    public static string GetIdentityName(string? identity)
    {
        if (string.IsNullOrEmpty(identity))
        {
            return identity ?? string.Empty;
        }

        if (IdentityNames.TryGetValue(identity, out string? value))
        {
            return value;
        }

        return identity;
    }
}
