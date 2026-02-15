using System.Text.Json.Serialization;

namespace C3.Domain.Models;

public sealed class WarSession
{
    [JsonIgnore]
    public const string Key = "war-session";

    public int WarId { get; set; }
    public ulong StartTime { get; set; }
    public string? EncryptedToken { get; set; }
    public List<int> FactionTargets { get; set; } = [];
}
