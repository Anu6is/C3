namespace C3.Models;
public sealed record SpyResults(bool Status, Faction Faction) { }
public sealed record Faction(int Id, string Name, Dictionary<string, Member> Members) { }
public sealed record Member(int Id, string Name, Spy Spy) { }
public sealed record Spy(ulong Strength, ulong Defense, ulong Speed, ulong Dexterity, ulong Total, ulong Timestamp) { }
