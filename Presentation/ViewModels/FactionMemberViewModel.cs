using MudBlazor;

namespace C3.Presentation.ViewModels;

public record FactionMemberViewModel
{
    public int Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public int Level { get; init; }

    // Pre-computed display values
    public string StatusDisplay { get; init; } = string.Empty;
    public Color StatusColor { get; init; }
    public string StateDisplay { get; init; } = string.Empty;
    public Color StateColor { get; init; }
    public string StateIcon { get; init; } = string.Empty;
    public ulong StateUntil { get; init; }

    public ulong BattleStats { get; init; }
    public string BattleStatsDisplay { get; init; } = "Unavailable";
    public Color BattleStatsColor { get; init; }
    public bool HasSpyData { get; init; }

    public ulong Strength { get; init; }
    public string StrengthText { get; init; } = "Unknown";
    public Color StrengthColor { get; init; }

    public ulong Defense { get; init; }
    public string DefenseText { get; init; } = "Unknown";
    public Color DefenseColor { get; init; }

    public ulong Speed { get; init; }
    public string SpeedText { get; init; } = "Unknown";
    public Color SpeedColor { get; init; }

    public ulong Dexterity { get; init; }
    public string DexterityText { get; init; } = "Unknown";
    public Color DexterityColor { get; init; }

    public bool IsMonitored { get; init; }
    public string AttackUrl { get; init; } = string.Empty;
    public string ProfileUrl { get; init; } = string.Empty;
}
