using C3.Domain.Models;
using C3.Domain.Extensions;

namespace C3.Domain.Specifications;

public class MemberFilterSpecification
{
    private readonly List<Func<int, FactionMemberData, bool>> _filters = [];

    public MemberFilterSpecification WithStates(IEnumerable<string> states)
    {
        if (states?.Any() == true)
        {
            var stateSet = states.ToHashSet();
            _filters.Add((id, member) => stateSet.Contains(member.State));
        }
        return this;
    }

    public MemberFilterSpecification WithMonitoredOnly(IEnumerable<int> monitoredIds)
    {
        if (monitoredIds?.Any() == true)
        {
            var monitoredSet = monitoredIds.ToHashSet();
            _filters.Add((id, member) => monitoredSet.Contains(id));
        }
        return this;
    }

    public MemberFilterSpecification WithStatComparison(
        bool? hasHigherStats,
        ulong comparisonStats,
        Func<int, ulong> getMemberStats)
    {
        if (hasHigherStats.HasValue)
        {
            _filters.Add((id, member) =>
            {
                var memberStats = getMemberStats(id);
                return (memberStats >= comparisonStats) == hasHigherStats.Value;
            });
        }
        return this;
    }

    public MemberFilterSpecification WithSearchTerm(string? searchTerm, Func<int, ulong>? getMemberStats = null)
    {
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            _filters.Add((id, member) =>
                member.FilterByName(searchTerm) ||
                member.FilterByStatus(searchTerm) ||
                member.FilterByLevel(searchTerm) ||
                (getMemberStats != null && member.FilterByBattleStats(searchTerm, getMemberStats(id))));
        }
        return this;
    }

    public MemberFilterSpecification WithCustomFilter(Func<int, FactionMemberData, bool> filter)
    {
        _filters.Add(filter);
        return this;
    }

    public bool IsSatisfiedBy(int id, FactionMemberData member)
    {
        return _filters.All(filter => filter(id, member));
    }
}
