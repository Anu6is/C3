using C3.Domain.Models;

namespace C3.Application.DTOs;

public record FactionMemberDto
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Level { get; init; }
    public string State { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public ulong StateUntil { get; init; }
    public SpyData? SpyData { get; init; }
}
