using MudBlazor;
using C3.Domain.Models;

namespace C3.Application.Services.Display;

public class ColorMappingService
{
    public Color GetStatusColor(string status) => status switch
    {
        "Online" => Color.Success,
        "Idle" => Color.Warning,
        _ => Color.Default
    };

    public Color GetStateColor(string state) => state switch
    {
        "Okay" => Color.Success,
        "Jail" => Color.Warning,
        "Hospital" => Color.Error,
        _ => Color.Info
    };

    public string GetStateIcon(string state) => state switch
    {
        "Okay" => Icons.Material.Filled.CheckCircle,
        "Jail" => Icons.Material.Filled.Gavel,
        "Hospital" => Icons.Material.Filled.LocalHospital,
        "Traveling" => Icons.Material.Filled.Flight,
        "Abroad" => Icons.Material.Filled.Public,
        _ => Icons.Material.Filled.Info
    };

    public Color GetBattleStatColor(ulong value, ulong threshold)
    {
        if (value == 0) return Color.Info;
        return value > threshold ? Color.Error : Color.Success;
    }

    public Color GetBattleStatusColor(Spy? spy, BattleStats stats, string stat)
    {
        if (spy is null) return Color.Default;

        Color GetColor(ulong spyValue, ulong userValue) =>
            spyValue == 0 ? Color.Info : spyValue > userValue ? Color.Error : Color.Success;

        return stat switch
        {
            "str" => GetColor(spy.Strength, stats.Strength),
            "def" => GetColor(spy.Defense, stats.Defense),
            "spd" => GetColor(spy.Speed, stats.Speed),
            "dex" => GetColor(spy.Dexterity, stats.Dexterity),
            _ => GetColor(spy.Total, stats.Total),
        };
    }
}
