using Blazor.SubtleCrypto;

namespace C3.Infrastructure.Storage;

public class ProtectedTokenStore(ICryptoService Crypto)
{
    public const string Key = "c3KEY.torn.ranked.war.tracker_ccc";

    public string? EncryptedToken { get; set; }

    public async Task SetTokenAsync(string apiKey)
    {
        var result = await Crypto.EncryptAsync(apiKey);

        EncryptedToken = result.Value;
    }

    public async Task<string?> GetTokenAsync() => EncryptedToken is null ? null : await Crypto.DecryptAsync(EncryptedToken);

    public async Task<string?> RefreshTokenAsync(string? token)
    {
        EncryptedToken = token;

        return await GetTokenAsync();
    }
}
