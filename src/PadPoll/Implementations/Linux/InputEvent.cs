using System.Runtime.InteropServices;
using PadPoll.Shared;

namespace PadPoll.Implementations.Linux;

/// <summary>
/// Represents a Linux input event structure, matching the native layout for interop.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal readonly struct InputEvent
{
    /// <summary>
    /// Gets seconds part of the event timestamp (seconds since the epoch).
    /// </summary>
    public long TimeSeconds { get; init; }

    /// <summary>
    /// Gets microseconds part of the event timestamp.
    /// </summary>
    public long TimeMicroseconds { get; init; }

    /// <summary>
    /// Gets type of the input event (e.g., key, mouse, etc.).
    /// </summary>
    public ushort Type { get; init; }

    /// <summary>
    /// Gets code identifying the specific event (e.g., key code, button code).
    /// </summary>
    public ushort Code { get; init;  }

    /// <summary>
    /// Gets value associated with the event (e.g., pressed/released state, axis value).
    /// </summary>
    public int Value { get; init; }

    /// <summary>
    /// Returns the timestamp of the input event in microseconds since the epoch,
    /// calculated from <see cref="TimeSeconds"/> and <see cref="TimeMicroseconds"/>.
    /// </summary>
    /// <returns>A microseconds since the epoch.</returns>
    public long GetMicrosecondsTimestamp()
    {
        return (TimeSeconds * Time.MicrosecondsPerSecond) + TimeMicroseconds;
    }
}