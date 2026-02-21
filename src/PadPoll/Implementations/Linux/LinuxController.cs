using Microsoft.Win32.SafeHandles;
using PadPoll.Abstractions;
using PadPoll.Models;

namespace PadPoll.Implementations.Linux;

/// <summary>
/// Represents a Linux-specific implementation of the <see cref="IController"/> interface.
/// </summary>
internal sealed class LinuxController : IController
{
    /// <inheritdoc/>
    public string DisplayName { get; }

    private readonly string _evDevFilePath;

    private LinuxController(string displayName, string evDevFilePath)
    {
        _evDevFilePath = evDevFilePath;
        DisplayName = displayName;
    }

    /// <summary>
    /// Creates a new instance of <see cref="LinuxController"/> if the specified event file exists.
    /// </summary>
    /// <param name="displayName">The display name of the controller.</param>
    /// <param name="eventFilePath">The path to the event device file.</param>
    /// <returns>A new <see cref="LinuxController"/> instance.</returns>
    /// <exception cref="FileNotFoundException">Thrown if the event file does not exist.</exception>
    public static LinuxController Create(string displayName, string eventFilePath)
    {
        return !File.Exists(eventFilePath)
            ? throw new FileNotFoundException(eventFilePath)
            : new LinuxController(displayName, eventFilePath);
    }

    /// <inheritdoc/>
    public IInputTimestampCollector GetTimestampCollector(ControllerInput monitoredInputs)
    {
        const int evIoCsClockId = 0x400445A0;
        int monotonicClockId = 1;
        SafeFileHandle handle = File.OpenHandle(_evDevFilePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        int evDevFd = handle.DangerousGetHandle().ToInt32();
        _ = LibC.ioctl(evDevFd, evIoCsClockId, ref monotonicClockId); // Set monotonic clock
        return new LinuxInputTimestampCollector(handle, monitoredInputs);
    }
}
