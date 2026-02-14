using C3.Application.Interfaces;
using C3.Domain;
using C3.Domain.Models;
using C3.Infrastructure.Repositories;
using C3.Infrastructure.TornApi;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace C3.Tests.Infrastructure;

public class FactionRepositoryTests
{
    private readonly Mock<ITornApiService> _mockApiService;
    private readonly Mock<ITornStatsApiService> _mockStatsService;
    private readonly Mock<ILogger<FactionRepository>> _mockLogger;
    private readonly IMemoryCache _cache;
    private readonly FactionRepository _repository;

    public FactionRepositoryTests()
    {
        _mockApiService = new Mock<ITornApiService>();
        _mockStatsService = new Mock<ITornStatsApiService>();
        _mockLogger = new Mock<ILogger<FactionRepository>>();
        _cache = new MemoryCache(new MemoryCacheOptions());
        _repository = new FactionRepository(
            _mockApiService.Object,
            _mockStatsService.Object,
            _mockLogger.Object,
            _cache);
    }

    [Fact]
    public async Task GetFactionDataAsync_ValidId_ReturnsSuccess()
    {
        // Arrange
        var testFaction = CreateTestFaction();
        _mockApiService.Setup(x => x.GetFactionAsync(It.IsAny<int>()))
            .ReturnsAsync(Result<TornFaction>.Success(testFaction));

        // Act
        var result = await _repository.GetFactionDataAsync(1);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(testFaction.Id, result.Value!.Id);
    }

    [Fact]
    public async Task GetFactionDataAsync_CachesResults()
    {
        // Arrange
        _mockApiService.Setup(x => x.GetFactionAsync(It.IsAny<int>()))
            .ReturnsAsync(Result<TornFaction>.Success(CreateTestFaction()));

        // Act
        await _repository.GetFactionDataAsync(1);
        await _repository.GetFactionDataAsync(1);

        // Assert
        _mockApiService.Verify(x => x.GetFactionAsync(1), Times.Once);
    }

    private static TornFaction CreateTestFaction()
    {
        return new TornFaction(
            1,
            "Test Faction",
            "TF",
            "img.png",
            new Dictionary<int, TornFactionMember>(),
            new Dictionary<int, TornFactionRankedWar>());
    }
}
