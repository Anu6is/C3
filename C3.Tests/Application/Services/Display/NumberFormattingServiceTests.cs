using C3.Application.Services.Display;
using Xunit;

namespace C3.Tests.Application.Services.Display;

public class NumberFormattingServiceTests
{
    private readonly NumberFormattingService _service = new();

    [Theory]
    [InlineData(0, "Unknown")]
    [InlineData(500, "500")]
    [InlineData(1500, "1.5K")]
    [InlineData(1000000, "1M")]
    [InlineData(2500000, "2.5M")]
    [InlineData(1000000000, "1B")]
    [InlineData(1200000000, "1.2B")]
    public void FormatBattleStat_ReturnsExpectedFormat(ulong value, string expected)
    {
        var result = _service.FormatBattleStat(value);
        Assert.Equal(expected, result);
    }
}
