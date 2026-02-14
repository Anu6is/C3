using MudBlazor;

namespace C3.Models;

public record MemberViewModel(
    int Id,
    string Name,
    int Level,
    Color StatusColor,
    string StatusText,
    ulong TotalStats,
    string TotalStatsText,
    Color TotalStatsColor,
    string StrengthText,
    Color StrengthColor,
    string DefenseText,
    Color DefenseColor,
    string SpeedText,
    Color SpeedColor,
    string DexterityText,
    Color DexterityColor,
    bool HasStatValues,
    string State,
    Color StateColor,
    string StateIcon,
    ulong Until,
    bool IsMonitored
)
{
    public static Color GetStatusColor(string status) => status switch
    {
        "Online" => Color.Success,
        "Idle" => Color.Warning,
        _ => Color.Default
    };

    public static Color GetStateColor(string state) => state switch
    {
        "Okay" => Color.Success,
        "Jail" => Color.Warning,
        "Hospital" => Color.Error,
        _ => Color.Info
    };

    public static string GetStateIcon(string state) => state switch
    {
        "Okay" => Icons.Material.Filled.CheckCircle,
        "Jail" => Icons.Material.Filled.Gavel,
        "Hospital" => Icons.Material.Filled.LocalHospital,
        "Traveling" => Icons.Material.Filled.Flight,
        "Abroad" => Icons.Material.Filled.Public,
        _ => Icons.Material.Filled.Info
    };

    public static Color GetBattleStatusColor(Spy? spy, BattleStats stats, string stat)
    {
        if (spy is null) return Color.Default;

        return stat switch
        {
            "str" => spy.Strength == 0 ? Color.Info : spy.Strength > stats.Strength ? Color.Error : Color.Success,
            "def" => spy.Defense == 0 ? Color.Info : spy.Defense > stats.Defense ? Color.Error : Color.Success,
            "spd" => spy.Speed == 0 ? Color.Info : spy.Speed > stats.Speed ? Color.Error : Color.Success,
            "dex" => spy.Dexterity == 0 ? Color.Info : spy.Dexterity > stats.Dexterity ? Color.Error : Color.Success,
            _ => spy.Total == 0 ? Color.Info : spy.Total > stats.Total ? Color.Error : Color.Success,
        };
    }

    public static ulong GetSumStats(Spy? spy)
    {
        if (spy is null) return 0;
        return spy.Strength + spy.Defense + spy.Speed + spy.Dexterity;
    }

    public static string GetBattleStatsText(Spy? spy, string stat = "Total")
    {
        if (spy is not null)
        {
            var value = stat switch
            {
                "str" => spy.Strength,
                "def" => spy.Defense,
                "spd" => spy.Speed,
                "dex" => spy.Dexterity,
                _ => spy.Total
            };

            if (value == 0) return "Unknown";

            return FormatNumber(value);
        }

        return "Unavailable";
    }

    public static string FormatNumber(ulong number) => number switch
    {
        >= 1_000_000_000 => $"{number / 1_000_000_000}B",
        >= 1_000_000 => $"{number / 1_000_000}M",
        >= 1_000 => $"{number / 1_000}K",
        _ => number.ToString()
    };
}
