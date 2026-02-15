namespace C3.Domain.Models;

public record UserData(
    int PlayerId,
    string Name,
    int Level,
    UserFactionData Faction,
    BattleStatsData Stats,
    UserStateData State,
    UserActivityData Activity);

public record UserFactionData(int FactionId, string FactionName, string FactionTag);
public record BattleStatsData(ulong Strength, ulong Defense, ulong Speed, ulong Dexterity)
{
    public ulong Total => Strength + Defense + Speed + Dexterity;
}
public record UserStateData(string Description, string State, ulong Until);
public record UserActivityData(string Status, ulong Timestamp);
