using C3.Domain.Models;
using C3.Domain.Specifications;
using C3.Application.DTOs;
using C3.Application.Mapping;

namespace C3.Application.Services;

public class FactionMemberService
{
    public Dictionary<int, TornFactionMember> FilterMembers(
        Dictionary<int, TornFactionMember> allMembers,
        MemberFilterSpecification specification)
    {
        return allMembers
            .Where(kvp => specification.IsSatisfiedBy(kvp.Key, kvp.Value))
            .ToDictionary(kvp => kvp.Key, kvp => kvp.Value);
    }

    public IEnumerable<FactionMemberDto> GetMemberDtos(
        Dictionary<int, TornFactionMember> filteredMembers,
        Dictionary<int, Spy> spies)
    {
        return filteredMembers.Select(m =>
            FactionMemberMapper.ToDto(m.Key, m.Value, spies));
    }
}
