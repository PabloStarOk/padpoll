namespace PadPoll.Models;

/// <summary>
/// Represents the possible controller input sources.
/// </summary>
[Flags]
public enum ControllerInput : uint
{
    /// <summary>
    /// The left joystick input.
    /// </summary>
    LeftJoystick = 1 << 0,

    /// <summary>
    /// The right joystick input.
    /// </summary>
    RightJoystick = 1 << 1,

    /// <summary>
    /// All possible controller inputs.
    /// </summary>
    All = uint.MaxValue,
}