namespace PadPoll.Models;

/// <summary>
/// Represents latency metrics for a device.
/// </summary>
/// <param name="AverageMs">The average latency in milliseconds.</param>
/// <param name="MinMs">The minimum latency in milliseconds.</param>
/// <param name="MaxMs">The maximum latency in milliseconds.</param>
/// <param name="OnePercentLowMs">The 1% of the highest (worst) latency in milliseconds.</param>
/// <param name="JitterMs">The jitter (Standard Deviation) in milliseconds.</param>
public readonly record struct LatencyMetrics(
    decimal AverageMs,
    decimal MinMs,
    decimal MaxMs,
    decimal OnePercentLowMs,
    decimal JitterMs);