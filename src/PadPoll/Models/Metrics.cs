namespace PadPoll.Models;

/// <summary>
/// Represents metrics collected during a polling test.
/// </summary>
/// <param name="TestDuration">The total duration of the test.</param>
/// <param name="CollectedSamples">The number of samples collected.</param>
/// <param name="PollingRate">Metrics related to polling rate.</param>
/// <param name="Latency">Metrics related to latency.</param>
public readonly record struct Metrics(
    TimeSpan TestDuration,
    int CollectedSamples,
    PollingRateMetrics PollingRate,
    LatencyMetrics Latency);