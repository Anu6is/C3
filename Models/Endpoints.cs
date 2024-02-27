namespace C3.Models;

public class Endpoints
{
    public static UserEndpointBuilder User(int userId = 0) => new(userId);

    public static FactionEndpointBuilder Faction(int factionId = 0) => new(factionId);

    public static FactionSpyEndpointBuilder FactionSpies(int factionId) => new(factionId);

    public class UserEndpointBuilder(int userId = 0) : EndpointBuilder
    {
        public override int ResourceId { get; set; } = userId;
        public override string Endpoint => "user/";

        public UserEndpointBuilder Profile()
        {
            selections.Add("profile");
            return this;
        }

        public UserEndpointBuilder BattleStats()
        {
            selections.Add("battlestats");
            return this;
        }
    }

    public class FactionEndpointBuilder(int factionId = 0) : EndpointBuilder
    {
        public override int ResourceId { get; set; } = factionId;
        public override string Endpoint => "faction/";
    }

    public class FactionSpyEndpointBuilder(int factionId) : EndpointBuilder
    {
        public override int ResourceId { get; set; } = factionId;
        public override string Endpoint => "/spy/faction/";

        public override string WithAuthorization(string key) => $"{key}{Endpoint}{ResourceId}";
    }

    public abstract class EndpointBuilder
    {
        public abstract string Endpoint { get; }
        public abstract int ResourceId { get; set; }

        protected readonly List<string> selections = [];

        public virtual string WithAuthorization(string key)
            => $"{Endpoint}{(ResourceId == 0 ? "" : ResourceId)}/?selections={string.Join(",", selections)}&key={key}&comment=ConflictCommandCenter";
    }

}

