using C3.Domain.Models;
using C3.Domain.Specifications;
using Xunit;

namespace C3.Tests.Domain;

public class MemberFilterSpecificationTests
{
    [Fact]
    public void WithStates_FiltersCorrectly()
    {
        // Arrange
        var members = new Dictionary<int, TornFactionMember>
        {
            [1] = new TornFactionMember("User1", 10, new TornUserState("desc", "Okay", 0), new TornUserActivity("Online", 0, "rel")),
            [2] = new TornFactionMember("User2", 20, new TornUserState("desc", "Hospital", 0), new TornUserActivity("Offline", 0, "rel")),
            [3] = new TornFactionMember("User3", 30, new TornUserState("desc", "Jail", 0), new TornUserActivity("Offline", 0, "rel"))
        };
        var spec = new MemberFilterSpecification().WithStates(new[] { "Okay", "Hospital" });

        // Act
        var filtered = members.Where(m => spec.IsSatisfiedBy(m.Key, m.Value)).ToList();

        // Assert
        Assert.Equal(2, filtered.Count);
        Assert.Contains(filtered, m => m.Key == 1);
        Assert.Contains(filtered, m => m.Key == 2);
    }

    [Fact]
    public void WithStatComparison_HigherStats_FiltersCorrectly()
    {
        // Arrange
        var members = new Dictionary<int, TornFactionMember>
        {
            [1] = CreateMember("User1"),
            [2] = CreateMember("User2")
        };
        var getMemberStats = new Func<int, ulong>(id => id == 1 ? 8000UL : 2000UL);
        var spec = new MemberFilterSpecification().WithStatComparison(hasHigherStats: true, comparisonStats: 5000UL, getMemberStats);

        // Act
        var filtered = members.Where(m => spec.IsSatisfiedBy(m.Key, m.Value)).ToList();

        // Assert
        Assert.Single(filtered);
        Assert.Equal(1, filtered[0].Key);
    }

    private static TornFactionMember CreateMember(string name) =>
        new TornFactionMember(name, 1, new TornUserState("desc", "Okay", 0), new TornUserActivity("Online", 0, "rel"));
}
