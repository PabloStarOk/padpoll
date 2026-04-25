using System.Runtime.InteropServices;
using PadPoll.Implementations.Windows.GameInput.Enums;

namespace PadPoll.Implementations.Windows.GameInput.Structs;

/// <summary>
/// Represents the state of a gamepad, including buttons, triggers, and thumbsticks.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal readonly struct GameInputGamepadState
{
    /// <summary>
    /// The state of the gamepad buttons.
    /// </summary>
    public readonly GameInputGamepadButtons Buttons;

    /// <summary>
    /// The position of the left trigger, from 0.0 (released) to 1.0 (fully pressed).
    /// </summary>
    public readonly float LeftTrigger;

    /// <summary>
    /// The position of the right trigger, from 0.0 (released) to 1.0 (fully pressed).
    /// </summary>
    public readonly float RightTrigger;

    /// <summary>
    /// The horizontal position of the left thumbstick, from -1.0 (left) to 1.0 (right).
    /// </summary>
    public readonly float LeftThumbstickX;

    /// <summary>
    /// The vertical position of the left thumbstick, from -1.0 (bottom) to 1.0 (top).
    /// </summary>
    public readonly float LeftThumbstickY;

    /// <summary>
    /// The horizontal position of the right thumbstick, from -1.0 (left) to 1.0 (right).
    /// </summary>
    public readonly float RightThumbstickX;

    /// <summary>
    /// The vertical position of the right thumbstick, from -1.0 (bottom) to 1.0 (top).
    /// </summary>
    public readonly float RightThumbstickY;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameInputGamepadState"/> struct.
    /// </summary>
    /// <param name="buttons">The state of the gamepad buttons.</param>
    /// <param name="leftTrigger">The position of the left trigger.</param>
    /// <param name="rightTrigger">The position of the right trigger.</param>
    /// <param name="leftThumbstickX">The horizontal position of the left thumbstick.</param>
    /// <param name="leftThumbstickY">The vertical position of the left thumbstick.</param>
    /// <param name="rightThumbstickX">The horizontal position of the right thumbstick.</param>
    /// <param name="rightThumbstickY">The vertical position of the right thumbstick.</param>
    public GameInputGamepadState(
        GameInputGamepadButtons buttons,
        float leftTrigger,
        float rightTrigger,
        float leftThumbstickX,
        float leftThumbstickY,
        float rightThumbstickX,
        float rightThumbstickY)
    {
        Buttons = buttons;
        LeftTrigger = leftTrigger;
        RightTrigger = rightTrigger;
        LeftThumbstickX = leftThumbstickX;
        LeftThumbstickY = leftThumbstickY;
        RightThumbstickX = rightThumbstickX;
        RightThumbstickY = rightThumbstickY;
    }
}