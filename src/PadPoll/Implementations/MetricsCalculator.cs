using PadPoll.Abstractions;
using PadPoll.Models;

namespace PadPoll.Implementations;

/// <summary>
/// Calculates polling rate and latency metrics based on timestamp data.
/// </summary>
public sealed class MetricsCalculator : IMetricsCalculator
{
    private const decimal StabilityTolerancePercent = 0.10m;
    private const decimal OnePercent = 0.01m;

    /// <inheritdoc/>
    public Metrics Calculate(long[] timestamps, decimal? expectedHz)
    {
        if (timestamps.Length < 2)
        {
            return default;
        }

        TimeSpan duration = TimeSpan.FromMicroseconds(timestamps[^1] - timestamps[0]);
        decimal[] msIntervals = CalculateIntervals(timestamps);
        PollingRateMetrics pollingRateMetrics = CalculatePollingRateMetrics(timestamps, msIntervals, expectedHz);
        LatencyMetrics latencyMetrics = CalculateLatencyMetrics(msIntervals);
        return new Metrics(duration, timestamps.Length, pollingRateMetrics, latencyMetrics);
    }

    private static decimal[] CalculateIntervals(long[] timestamps)
    {
        decimal[] msIntervals = new decimal[timestamps.Length - 1];
        for (int i = 0; i < timestamps.Length - 1; i++)
        {
            long interval = timestamps[i + 1] - timestamps[i];
            msIntervals[i] = Math.Max(0.001m, (decimal)interval / TimeSpan.MicrosecondsPerMillisecond);
        }

        return msIntervals.ToArray();
    }

    private static PollingRateMetrics CalculatePollingRateMetrics(
        long[] timestamps,
        decimal[] msIntervals,
        decimal? expectedHz)
    {
        decimal averageHz = CalculateAverageHz(timestamps);
        decimal peakHz = TimeSpan.MillisecondsPerSecond / msIntervals.Min();
        bool validExpectedHz = expectedHz is > 0;
        decimal? accuracy = validExpectedHz ? averageHz / expectedHz : null;
        decimal? consistency = validExpectedHz ? CalculateConsistency(msIntervals, expectedHz!.Value) : null;
        return new PollingRateMetrics(averageHz, peakHz, accuracy, consistency);
    }

    private static decimal CalculateAverageHz(long[] timestamps)
    {
        var hzPerSeconds = new Dictionary<long, int>();
        foreach (long timestamp in timestamps)
        {
            long second = timestamp / TimeSpan.MicrosecondsPerSecond;
            hzPerSeconds[second] = hzPerSeconds.GetValueOrDefault(second) + 1;
        }

        long[] seconds = hzPerSeconds.Keys.OrderBy(k => k).ToArray();
        if (seconds.Length > 2)
        {
            return seconds[1..^1].Select(s => (decimal)hzPerSeconds[s]).Average();
        }

        int intervalsCount = timestamps.Length - 1;
        decimal totalDurationSeconds = (timestamps[^1] - timestamps[0]) / (decimal)TimeSpan.MicrosecondsPerSecond;
        return intervalsCount / totalDurationSeconds;
    }

    private static decimal CalculateConsistency(decimal[] msIntervals, decimal expectedHz)
    {
        decimal expectedIntervalMs = TimeSpan.MillisecondsPerSecond / expectedHz;
        decimal tolerance = expectedIntervalMs * StabilityTolerancePercent;
        decimal lowerBound = expectedIntervalMs - tolerance;
        decimal upperBound = expectedIntervalMs + tolerance;
        int stableSamples = msIntervals.Count(i => i >= lowerBound && i <= upperBound);
        return (decimal)stableSamples / msIntervals.Length;
    }

    private static LatencyMetrics CalculateLatencyMetrics(decimal[] msIntervals)
    {
        decimal averageMs = msIntervals.Average();
        decimal minMs = msIntervals.Min();
        decimal maxMs = msIntervals.Max();
        decimal onePercentLowMs = CalculateOnePercentLowMs(msIntervals);
        decimal jitterMs = CalculateJitterMs(msIntervals, averageMs);
        return new LatencyMetrics(averageMs, minMs, maxMs, onePercentLowMs, jitterMs);
    }

    private static decimal CalculateOnePercentLowMs(decimal[] msIntervals)
    {
        int samplesNumber = Math.Max(1, (int)Math.Ceiling(msIntervals.Length * OnePercent));
        return msIntervals.OrderDescending().Take(samplesNumber).Average();
    }

    private static decimal CalculateJitterMs(decimal[] msIntervals, decimal averageMs)
    {
        decimal sumOfIntervalSquares = 0;
        foreach (decimal interval in msIntervals)
        {
            decimal distance = interval - averageMs;
            sumOfIntervalSquares += distance * distance;
        }

        decimal variance = sumOfIntervalSquares / (msIntervals.Length - 1);
        return (decimal)Math.Sqrt((double)variance);
    }
}