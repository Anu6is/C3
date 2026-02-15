namespace C3.Domain.Models;

public sealed record TornFaction(int Id, string Name, string Tag, string Tag_Image, 
                                 Dictionary<int, TornFactionMember> Members, 
                                 Dictionary<int, TornFactionRankedWar> Ranked_Wars) { }

public sealed record TornFactionRankedWar(TornFactionWar War, Dictionary<int, TornFactionWarScore> Factions) { }

public sealed record TornFactionWar(ulong Start, ulong End, int Target, int Winner) { }

public sealed record TornFactionWarScore(string Name, int Score, int Chain) { }

public sealed record TornFactionMember(string Name, int Level, TornUserState Status, TornUserActivity Last_Action) { }