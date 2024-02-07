using C3.Models;
using System.Text.RegularExpressions;

namespace C3.Helpers;

public static partial class FactionMemberFilterHelper
{
    public static bool FilterByStatus(this TornFactionMember member, string status)
    {
        return member.Last_Action.Status.Contains(status, StringComparison.OrdinalIgnoreCase);
    }

    public static bool FilterByName(this TornFactionMember member, string name)
    {
        return member.Name.Contains(name, StringComparison.OrdinalIgnoreCase);
    }

    public static bool FilterByLevel(this TornFactionMember member, string filter)
    {
        var match = IsLessThanOrEqualToFilter().Match(filter);

        if (match is not null && match.Success)
            return member.Level <= int.Parse(match.Groups[1].Value);

        match = IsLessThanFilter().Match(filter);

        if (match is not null && match.Success)
            return member.Level < int.Parse(match.Groups[1].Value);

        match = IsGreaterThanOrEqualToFilter().Match(filter);

        if (match is not null && match.Success)
            return member.Level >= int.Parse(match.Groups[1].Value);

        match = IsGreaterThanFilter().Match(filter);

        if (match.Success)
            return member.Level > int.Parse(match.Groups[1].Value);

        match = IsEqualToFilter().Match(filter);

        if (match.Success)
            return member.Level == int.Parse(match.Groups[1].Value);

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
