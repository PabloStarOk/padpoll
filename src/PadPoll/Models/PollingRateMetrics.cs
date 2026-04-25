namespace PadPoll.Models;

/// <summary>
/// Represents polling rate metrics for a device.
/// </summary>
/// <param name="AverageHz">The average polling rate in Hz.</param>
/// <param name="PeakHz">The peak polling rate in Hz.</param>
/// <param name="Accuracy">The accuracy of the polling rate, if available.</param>
/// <param name="Consistency">The consistency of the polling rate, if available.</param>
public readonly record struct PollingRateMetrics(
    decimal AverageHz,
    decimal PeakHz,
    decimal? Accuracy,
    decimal? Consistency);