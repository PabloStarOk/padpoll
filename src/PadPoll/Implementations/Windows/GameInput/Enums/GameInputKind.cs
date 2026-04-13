namespace PadPoll.Implementations.Windows.GameInput.Enums;

/// <summary>
/// Specifies the types of input devices supported by GameInput.
/// </summary>
[Flags]
internal enum GameInputKind : uint
{
    /// <summary>
    /// Unknown device type.
    /// </summary>
    Unknown = 0x00000000,

    /// <summary>
    /// A raw device report.
    /// </summary>
    RawDeviceReport = 0x00000001,

    /// <summary>
    /// A controller axis.
    /// </summary>
    ControllerAxis = 0x00000002,

    /// <summary>
    /// A controller button.
    /// </summary>
    ControllerButton = 0x00000004,

    /// <summary>
    /// A controller switch.
    /// </summary>
    ControllerSwitch = 0x00000008,

    /// <summary>
    /// A combination of axis, button, and switch inputs.
    /// </summary>
    Controller = 0x0000000E,

    /// <summary>
    /// A keyboard device.
    /// </summary>
    Keyboard = 0x00000010,

    /// <summary>
    /// A mouse device.
    /// </summary>
    Mouse = 0x00000020,

    /// <summary>
    /// Sensor input (e.g., accelerometer, gyroscope).
    /// </summary>
    Sensors = 0x00000040,

    /// <summary>
    /// An arcade stick device.
    /// </summary>
    ArcadeStick = 0x00010000,

    /// <summary>
    /// A flight stick device.
    /// </summary>
    FlightStick = 0x00020000,

    /// <summary>
    /// A gamepad device.
    /// </summary>
    Gamepad = 0x00040000,

    /// <summary>
    /// A racing wheel device.
    /// </summary>
    RacingWheel = 0x00080000,
}