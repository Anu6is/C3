using System.Text.Json.Serialization;

namespace C3.Services;

public sealed class WarSession
{
    [JsonIgnore]
    public const string Key = "war-session";

    public ulong StartTime { get; set; }
    public string? EncryptedToken { get; set; }
    public List<int> FactionTargets { get; set; } = [];
}
