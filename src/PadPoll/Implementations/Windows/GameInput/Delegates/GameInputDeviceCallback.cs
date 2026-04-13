using System.Runtime.InteropServices;
using PadPoll.Implementations.Windows.GameInput.Enums;

namespace PadPoll.Implementations.Windows.GameInput.Delegates;

/// <summary>
/// Represents a callback delegate for game input device events.
/// </summary>
/// <param name="callbackToken">A unique token identifying the callback registration.</param>
/// <param name="context">A pointer to user-defined context data.</param>
/// <param name="device">A pointer to the device that triggered the event.</param>
/// <param name="timestamp">The timestamp of the event.</param>
/// <param name="currentStatus">The current status of the device.</param>
/// <param name="previousStatus">The previous status of the device.</param>
[UnmanagedFunctionPointer(CallingConvention.StdCall)]
internal delegate void GameInputDeviceCallback(
    ulong callbackToken,
    nint context,
    nint device,
    ulong timestamp,
    GameInputDeviceStatus currentStatus,
    GameInputDeviceStatus previousStatus);