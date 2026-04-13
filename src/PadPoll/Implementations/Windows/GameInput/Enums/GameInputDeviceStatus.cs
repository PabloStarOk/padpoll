namespace PadPoll.Implementations.Windows.GameInput.Enums;

/// <summary>
/// Represents the status flags for a GameInput device.
/// </summary>
[Flags]
internal enum GameInputDeviceStatus : uint
{
    /// <summary>
    /// No status flags are set.
    /// </summary>
    None = 0x00000000,

    /// <summary>
    /// The device is connected.
    /// </summary>
    Connected = 0x00000001,

    /// <summary>
    /// Haptic information for the device is ready.
    /// </summary>
    HapticInfoReady = 0x00200000,

    /// <summary>
    /// Any status flag (all bits set).
    /// </summary>
    Any = 0xFFFFFFFF,
}