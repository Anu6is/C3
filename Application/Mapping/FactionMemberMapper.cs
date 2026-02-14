using C3.Domain.Models;
using C3.Application.DTOs;

namespace C3.Application.Mapping;

public static class FactionMemberMapper
{
    public static FactionMemberDto ToDto(
        int id,
        TornFactionMember member,
        Dictionary<int, Spy> spies)
    {
        var spyData = spies.TryGetValue(id, out var spy)
            ? new SpyDataDto(spy.Strength, spy.Defense, spy.Speed, spy.Dexterity, spy.Total)
            : null;

        return new FactionMemberDto
        {
            Id = id,
            Name = member.Name,
            Level = member.Level,
            State = member.Status.State,
            Status = member.Last_Action.Status,
            StateUntil = member.Status.Until,
            SpyData = spyData
        };
    }
}
