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
        var members = new Dictionary<int, FactionMemberData>
        {
            [1] = new FactionMemberData("User1", 10, "Okay", "Online", 0, 0),
            [2] = new FactionMemberData("User2", 20, "Hospital", "Offline", 0, 0),
            [3] = new FactionMemberData("User3", 30, "Jail", "Offline", 0, 0)
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
        var members = new Dictionary<int, FactionMemberData>
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

    private static FactionMemberData CreateMember(string name) =>
        new FactionMemberData(name, 1, "Okay", "Online", 0, 0);
}
