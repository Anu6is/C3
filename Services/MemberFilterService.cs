using C3.Models;
using System.Collections.Generic;
using System.Linq;

namespace C3.Services;

public record FilterCriteria(
    List<string> SelectedStates,
    bool MonitoredOnly,
    bool? HasHigherStats,
    List<int> MonitoredTargets)
{
    public virtual bool Equals(FilterCriteria? other)
    {
        if (other is null) return false;
        if (ReferenceEquals(this, other)) return true;

        return MonitoredOnly == other.MonitoredOnly &&
               HasHigherStats == other.HasHigherStats &&
               SelectedStates.SequenceEqual(other.SelectedStates) &&
               MonitoredTargets.SequenceEqual(other.MonitoredTargets);
    }

    public override int GetHashCode()
    {
        var hash = new HashCode();
        hash.Add(MonitoredOnly);
        hash.Add(HasHigherStats);
        foreach (var state in SelectedStates) hash.Add(state);
        foreach (var target in MonitoredTargets) hash.Add(target);
        return hash.ToHashCode();
    }
}

public class MemberFilterService
{
    private Dictionary<int, TornFactionMember> _cachedMembers = [];
    private FilterCriteria? _lastCriteria;
    private Dictionary<int, TornFactionMember>? _lastSourceMembers;
    private Dictionary<int, Spy>? _lastSpies;

    public Dictionary<int, TornFactionMember> FilterMembers(
        Dictionary<int, TornFactionMember> sourceMembers,
        FilterCriteria criteria,
        Dictionary<int, Spy> spies,
        BattleStats currentUserStats)
    {
        // Return cached if criteria and source data unchanged
        if (_lastCriteria?.Equals(criteria) == true &&
            ReferenceEquals(_lastSourceMembers, sourceMembers) &&
            ReferenceEquals(_lastSpies, spies))
        {
            return _cachedMembers;
        }

        IEnumerable<KeyValuePair<int, TornFactionMember>> filtered = sourceMembers;

        // Apply filters sequentially
        if (criteria.SelectedStates.Count > 0)
        {
            var statesHashSet = criteria.SelectedStates.ToHashSet();
            filtered = filtered.Where(x => statesHashSet.Contains(x.Value.Status.State));
        }

        if (criteria.MonitoredOnly)
        {
            var targetsHashSet = criteria.MonitoredTargets.ToHashSet();
            filtered = filtered.Where(x => targetsHashSet.Contains(x.Key));
        }

        if (criteria.HasHigherStats.HasValue)
        {
            filtered = filtered.Where(x =>
            {
                var total = spies.TryGetValue(x.Key, out var spy)
                    ? spy.Total
                    : 0UL;
                return (total >= currentUserStats.Total) == criteria.HasHigherStats.Value;
            });
        }

        _cachedMembers = filtered.ToDictionary(x => x.Key, x => x.Value);
        _lastCriteria = criteria;
        _lastSourceMembers = sourceMembers;
        _lastSpies = spies;

        return _cachedMembers;
    }

    public void InvalidateCache() => _lastCriteria = null;
}
