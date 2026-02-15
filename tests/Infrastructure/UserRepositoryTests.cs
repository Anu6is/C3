using C3.Domain;
using C3.Domain.Models;
using C3.Infrastructure.Repositories;
using C3.Infrastructure.TornApi;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;

namespace C3.Tests.Infrastructure;

public class UserRepositoryTests
{
    private readonly Mock<ITornApiService> _mockApiService;
    private readonly Mock<ILogger<UserRepository>> _mockLogger;
    private readonly UserRepository _repository;

    public UserRepositoryTests()
    {
        _mockApiService = new Mock<ITornApiService>();
        _mockLogger = new Mock<ILogger<UserRepository>>();
        _repository = new UserRepository(_mockApiService.Object, _mockLogger.Object);
    }

    [Fact]
    public async Task GetCurrentUserAsync_ValidUser_ReturnsSuccess()
    {
        // Arrange
        var testUser = CreateTestUser();
        _mockApiService.Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(Result<TornUser>.Success(testUser));

        // Act
        var result = await _repository.GetCurrentUserAsync();

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(testUser.Player_Id, result.Value!.PlayerId);
    }

    [Fact]
    public async Task GetCurrentUserAsync_ApiFailure_ReturnsFailure()
    {
        // Arrange
        _mockApiService.Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(Result<TornUser>.Failure("API Error"));

        // Act
        var result = await _repository.GetCurrentUserAsync();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("API Error", result.Error);
    }

    [Fact]
    public async Task GetCurrentUserAsync_NullUser_ReturnsFailure()
    {
        // Arrange
        _mockApiService.Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(Result<TornUser>.Success(null!));

        // Act
        var result = await _repository.GetCurrentUserAsync();

        // Assert
        Assert.False(result.IsSuccess);
        Assert.Equal("Failed to retrieve user data", result.Error);
    }

    private static TornUser CreateTestUser()
    {
        return new TornUser(
            1,
            123,
            "TestUser",
            100, 100, 100, 100,
            0, 0, 0, 0,
            new TornUserState("desc", "Okay", 0),
            new TornUserActivity("Online", 0, "rel"),
            new TornUserFaction(1, "Faction", "TAG"));
    }
}
