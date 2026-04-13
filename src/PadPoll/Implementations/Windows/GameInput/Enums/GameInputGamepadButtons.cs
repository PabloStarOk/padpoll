namespace PadPoll.Implementations.Windows.GameInput.Enums;

/// <summary>
/// Specifies the buttons available on a GameInput gamepad.
/// </summary>
[Flags]
internal enum GameInputGamepadButtons : uint
{
    /// <summary>No buttons pressed.</summary>
    None = 0x00000000,

    /// <summary>The Menu button.</summary>
    Menu = 0x00000001,

    /// <summary>The View button.</summary>
    View = 0x00000002,

    /// <summary>The A button.</summary>
    A = 0x00000004,

    /// <summary>The B button.</summary>
    B = 0x00000008,

    /// <summary>The C button.</summary>
    C = 0x00004000,

    /// <summary>The X button.</summary>
    X = 0x00000010,

    /// <summary>The Y button.</summary>
    Y = 0x00000020,

    /// <summary>The Z button.</summary>
    Z = 0x00008000,

    /// <summary>The Directional Pad Up button.</summary>
    DPadUp = 0x00000040,

    /// <summary>The Directional Pad Down button.</summary>
    DPadDown = 0x00000080,

    /// <summary>The Directional Pad Left button.</summary>
    DPadLeft = 0x00000100,

    /// <summary>The Directional Pad Right button.</summary>
    DPadRight = 0x00000200,

    /// <summary>The left shoulder button (LB).</summary>
    LeftShoulder = 0x00000400,

    /// <summary>The right shoulder button (RB).</summary>
    RightShoulder = 0x00000800,

    /// <summary>The left trigger button.</summary>
    LeftTriggerButton = 0x00010000,

    /// <summary>The right trigger button.</summary>
    RightTriggerButton = 0x00020000,

    /// <summary>The left thumbstick click button (LS).</summary>
    LeftThumbstick = 0x00001000,

    /// <summary>Left thumbstick moved up.</summary>
    LeftThumbstickUp = 0x00040000,

    /// <summary>Left thumbstick moved down.</summary>
    LeftThumbstickDown = 0x00080000,

    /// <summary>Left thumbstick moved left.</summary>
    LeftThumbstickLeft = 0x00100000,

    /// <summary>Left thumbstick moved right.</summary>
    LeftThumbstickRight = 0x00200000,

    /// <summary>The right thumbstick click button (RS).</summary>
    RightThumbstick = 0x00002000,

    /// <summary>Right thumbstick moved up.</summary>
    RightThumbstickUp = 0x00400000,

    /// <summary>Right thumbstick moved down.</summary>
    RightThumbstickDown = 0x00800000,

    /// <summary>Right thumbstick moved left.</summary>
    RightThumbstickLeft = 0x01000000,

    /// <summary>Right thumbstick moved right.</summary>
    RightThumbstickRight = 0x02000000,

    /// <summary>The first left paddle button.</summary>
    PaddleLeft1 = 0x04000000,

    /// <summary>The second left paddle button.</summary>
    PaddleLeft2 = 0x08000000,

    /// <summary>The first right paddle button.</summary>
    PaddleRight1 = 10000000,

    /// <summary>The second right paddle button.</summary>
    PaddleRight2 = 0x20000000,
}