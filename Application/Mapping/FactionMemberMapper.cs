using C3.Domain.Models;
using C3.Application.DTOs;

namespace C3.Application.Mapping;

public static class FactionMemberMapper
{
    public static FactionMemberDto ToDto(
        int id,
        FactionMemberData member,
        Dictionary<int, SpyData> spies)
    {
        var spyData = spies.TryGetValue(id, out var spy) ? spy : null;

        return new FactionMemberDto
        {
            Id = id,
            Name = member.Name,
            Level = member.Level,
            State = member.State,
            Status = member.Status,
            StateUntil = member.StateUntil,
            SpyData = spyData
        };
    }
}
