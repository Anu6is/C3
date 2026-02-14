namespace C3.Domain.Models;

public record FactionData(
    int Id,
    string Name,
    string Tag,
    string TagImage,
    Dictionary<int, FactionMemberData> Members,
    List<RankedWarData> RankedWars);

public record FactionMemberData(
    string Name,
    int Level,
    string State,
    string Status,
    ulong StateUntil,
    ulong LastActionTimestamp);

public record RankedWarData(
    int WarId,
    ulong StartTime,
    ulong EndTime,
    int TargetScore,
    int? WinnerFactionId,
    Dictionary<int, WarScoreData> FactionScores);

public record WarScoreData(string FactionName, int Score, int Chain);

public record SpyData(
    int MemberId,
    ulong Strength,
    ulong Defense,
    ulong Speed,
    ulong Dexterity,
    ulong Total,
    ulong Timestamp);
