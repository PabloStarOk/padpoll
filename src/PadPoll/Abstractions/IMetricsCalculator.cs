using PadPoll.Models;

namespace PadPoll.Abstractions;

/// <summary>
/// Defines a contract for calculating metrics based on timestamps and an optional expected frequency.
/// </summary>
public interface IMetricsCalculator
{
    /// <summary>
    /// Calculates metrics from the provided microsecond timestamps and expected frequency (Hz).
    /// </summary>
    /// <param name="timestamps">Array of timestamp values in microseconds.</param>
    /// <param name="expectedHz">Optional expected frequency in Hz.</param>
    /// <returns>Calculated <see cref="Metrics"/> object.</returns>
    Metrics Calculate(long[] timestamps, decimal? expectedHz);
}