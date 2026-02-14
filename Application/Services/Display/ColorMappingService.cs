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

        return stat switch
        {
            "str" => spy.Strength == 0 ? Color.Info : spy.Strength > stats.Strength ? Color.Error : Color.Success,
            "def" => spy.Defense == 0 ? Color.Info : spy.Defense > stats.Defense ? Color.Error : Color.Success,
            "spd" => spy.Speed == 0 ? Color.Info : spy.Speed > stats.Speed ? Color.Error : Color.Success,
            "dex" => spy.Dexterity == 0 ? Color.Info : spy.Dexterity > stats.Dexterity ? Color.Error : Color.Success,
            _ => spy.Total == 0 ? Color.Info : spy.Total > stats.Total ? Color.Error : Color.Success,
        };
    }
}
