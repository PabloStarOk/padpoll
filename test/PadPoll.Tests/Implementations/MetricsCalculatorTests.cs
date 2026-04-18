using PadPoll.Implementations;
using PadPoll.Models;

namespace PadPoll.Tests.Implementations;

/// <summary>
/// Unit tests for the <see cref="MetricsCalculator"/> class.
/// </summary>
[Trait("Category", "Unit")]
public sealed class MetricsCalculatorTests
{
    private readonly MetricsCalculator _calculator;

    /// <summary>
    /// Initializes a new instance of the <see cref="MetricsCalculatorTests"/> class.
    /// </summary>
    public MetricsCalculatorTests()
    {
        _calculator = new MetricsCalculator();
    }

    /// <summary>
    /// Provides test data for invalid timestamp arrays (empty or single element).
    /// </summary>
    /// <returns>A <see cref="TheoryData{T}"/> containing invalid timestamp arrays.</returns>
    public static TheoryData<long[]> GetInvalidTimestamps()
    {
        return
        [
            [],
            [1000],
        ];
    }

    /// <summary>
    /// Tests that <see cref="MetricsCalculator.Calculate"/> returns the expected metrics for perfectly spaced intervals.
    /// </summary>
    [Fact]
    public void Calculate_should_ReturnExpectedMetricsForPerfectIntervals()
    {
        // Arrange
        var expectedRate = new PollingRateMetrics(1000m, 1000m, 1m, 1m);
        var expectedLatency = new LatencyMetrics(1m, 1m, 1m, 1m, 0m);
        long[] timestamps = [1000, 2000, 3000, 4000];
        const decimal expectedHz = 1000m;
        var expected =
            new Metrics(TimeSpan.FromMicroseconds(3000), timestamps.Length, expectedRate, expectedLatency);

        // Act
        Metrics actual = _calculator.Calculate(timestamps, expectedHz);

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Tests that <see cref="MetricsCalculator.Calculate"/> correctly handles jitter and variance in timestamp intervals,
    /// and produces the expected metrics.
    /// </summary>
    [Fact]
    public void Calculate_should_HandleJitterAndVariance()
    {
        // Arrange
        long[] timestamps = [1000, 2000, 4000, 4500];
        var expectedLatency = new LatencyMetrics(1.1666666666666666666666666667m, 0.5m, 2m, 2m, 0.763762615825973m);
        const decimal expectedHz = 1000m;

        // Act
        Metrics metrics = _calculator.Calculate(timestamps, expectedHz);

        // Assert
        Assert.Equal(TimeSpan.FromMicroseconds(3500), metrics.TestDuration);
        Assert.Equal(4, metrics.CollectedSamples);
        Assert.Equal(3m / 0.0035m, metrics.PollingRate.AverageHz);
        Assert.Equal(2000m, metrics.PollingRate.PeakHz);
        Assert.Equal(expectedLatency.AverageMs, metrics.Latency.AverageMs, 10);
        Assert.Equal(expectedLatency.MinMs, metrics.Latency.MinMs);
        Assert.Equal(expectedLatency.MaxMs, metrics.Latency.MaxMs);
        Assert.Equal(expectedLatency.OnePercentLowMs, metrics.Latency.OnePercentLowMs);
        Assert.Equal(expectedLatency.JitterMs, metrics.Latency.JitterMs, 5);
    }

    /// <summary>
    /// Tests that <see cref="MetricsCalculator.Calculate"/> returns the default <see cref="Metrics"/>
    /// value when the provided timestamp sequence contains fewer than two elements.
    /// </summary>
    /// <param name="timestamps">The array of timestamps to test.</param>
    [Theory]
    [MemberData(nameof(GetInvalidTimestamps))]
    public void Calculate_should_ReturnDefault_when_NotEnoughTimestamps(long[] timestamps)
    {
        // Assert
        Assert.Equal(default, _calculator.Calculate(timestamps, null));
    }
}