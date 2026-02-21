namespace PadPoll.Abstractions;

/// <summary>
/// Collects timestamps for input packets, providing microsecond precision.
/// </summary>
public interface IInputTimestampCollector : IDisposable
{
    /// <summary>
    /// Gets the timestamp of a packet in microseconds.
    /// </summary>
    /// <returns>The timestamp in microseconds.</returns>
    long GetPacketMicrosecondsTimestamp();
}