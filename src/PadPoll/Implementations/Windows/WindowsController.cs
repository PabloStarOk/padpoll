using System.Runtime.Versioning;
using PadPoll.Abstractions;
using PadPoll.Implementations.Windows.GameInput.Interfaces;
using PadPoll.Implementations.Windows.GameInput.Structs;
using PadPoll.Models;

namespace PadPoll.Implementations.Windows;

/// <summary>
/// Represents a Windows-specific controller implementation using the GameInput API.
/// </summary>
/// <param name="gameInput">The GameInput instance used for input processing.</param>
/// <param name="device">The specific GameInput device associated with this controller.</param>
[SupportedOSPlatform("Windows")]
internal sealed class WindowsController(IGameInput gameInput, IGameInputDevice device)
    : IController
{
    private const string UnknownDeviceName = "Unknown";

    /// <inheritdoc/>
    public string DisplayName { get; } = GetDisplayName(device);

    /// <inheritdoc/>
    public IInputTimestampCollector GetTimestampCollector(ControllerInput monitoredInputs)
    {
        return new WindowsInputTimestampCollector(gameInput, device, monitoredInputs);
    }

    private static unsafe string GetDisplayName(IGameInputDevice device)
    {
        int hr = device.GetDeviceInfo(out GameInputDeviceInfo* devInfoPtr);
        if (hr is not 0)
        {
            return UnknownDeviceName;
        }

        GameInputDeviceInfo devInfo = *devInfoPtr;
        return devInfo.DisplayName;
    }
}
