using C3.Domain.Models;
using System.Text.RegularExpressions;

namespace C3.Domain.Extensions;

public static partial class FactionMemberExtensions
{
    public static bool FilterByStatus(this FactionMemberData member, string status)
    {
        return member.Status.Contains(status, StringComparison.OrdinalIgnoreCase);
    }

    public static bool FilterByName(this FactionMemberData member, string name)
    {
        return member.Name.Contains(name, StringComparison.OrdinalIgnoreCase);
    }

    public static bool FilterByLevel(this FactionMemberData member, string filter)
    {
        return FilterByLevelInternal(member.Level, filter);
    }

    public static bool FilterByBattleStats(this FactionMemberData _, string filter, ulong spyTotal)
    {
        return FilterByBattleStatsInternal(spyTotal, filter);
    }

    private static bool FilterByLevelInternal(int level, string filter)
    {
        var match = IsLessThanOrEqualToFilter().Match(filter);
        if (match.Success)
        {
            var value = int.Parse(match.Groups[1].Value);
            return value <= 100 && level <= value;
        }

        match = IsLessThanFilter().Match(filter);
        if (match.Success)
        {
            var value = int.Parse(match.Groups[1].Value);
            return value <= 100 && level < value;
        }

        match = IsGreaterThanOrEqualToFilter().Match(filter);
        if (match.Success)
            return level >= int.Parse(match.Groups[1].Value);

        match = IsGreaterThanFilter().Match(filter);
        if (match.Success)
            return level > int.Parse(match.Groups[1].Value);

        match = IsEqualToFilter().Match(filter);
        if (match.Success)
            return level == int.Parse(match.Groups[1].Value);

        return false;
    }

    private static bool FilterByBattleStatsInternal(ulong spyTotal, string filter)
    {
        if (spyTotal == 0) return false;

        var match = IsLessThanOrEqualToFilter().Match(filter);
        if (match.Success)
            return spyTotal <= ulong.Parse(match.Groups[1].Value);

        match = IsLessThanFilter().Match(filter);
        if (match.Success)
            return spyTotal < ulong.Parse(match.Groups[1].Value);

        match = IsGreaterThanOrEqualToFilter().Match(filter);
        if (match.Success)
            return spyTotal >= ulong.Parse(match.Groups[1].Value);

        match = IsGreaterThanFilter().Match(filter);
        if (match.Success)
            return spyTotal > ulong.Parse(match.Groups[1].Value);

        match = IsEqualToFilter().Match(filter);
        if (match.Success)
            return spyTotal == ulong.Parse(match.Groups[1].Value);

        return false;
    }

    [GeneratedRegex(@"^(?:<|lt)\s*(\d+)$", RegexOptions.IgnoreCase)]
    private static partial Regex IsLessThanFilter();

    [GeneratedRegex(@"^(?:>|gt)\s*(\d+)$", RegexOptions.IgnoreCase)]
    private static partial Regex IsGreaterThanFilter();

    [GeneratedRegex(@"^(?:=|eq)\s*(\d+)$", RegexOptions.IgnoreCase)]
    private static partial Regex IsEqualToFilter();

    [GeneratedRegex(@"^(?:<=|lte)\s*(\d+)$", RegexOptions.IgnoreCase)]
    private static partial Regex IsLessThanOrEqualToFilter();

    [GeneratedRegex(@"^(?:>=|gte)\s*(\d+)$", RegexOptions.IgnoreCase)]
    private static partial Regex IsGreaterThanOrEqualToFilter();
}
