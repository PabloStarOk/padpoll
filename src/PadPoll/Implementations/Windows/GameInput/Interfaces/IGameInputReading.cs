using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using PadPoll.Implementations.Windows.GameInput.Enums;
using PadPoll.Implementations.Windows.GameInput.Structs;

namespace PadPoll.Implementations.Windows.GameInput.Interfaces;

/// <summary>
/// Represents a single snapshot of input data from a device.
/// </summary>
[GeneratedComInterface]
[Guid(Constants.GameInputReadingIid)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal partial interface IGameInputReading
{
    /// <summary>
    /// Gets the type of input contained in the reading.
    /// </summary>
    /// <returns>The <see cref="GameInputKind"/> of the reading.</returns>
    [PreserveSig]
    GameInputKind GetInputKind();

    /// <summary>
    /// Gets the microsecond timestamp describing when the input was made.
    /// </summary>
    /// <returns>The microsecond timestamp of the reading.</returns>
    [PreserveSig]
    ulong GetTimestamp();

    /// <summary>
    /// Retrieves the device associated with the reading.
    /// </summary>
    /// <param name="device">An <see cref="IGameInputDevice"/> object that represents the device.</param>
    [PreserveSig]
    void GetDevice([MarshalAs(UnmanagedType.Interface)] out IGameInputDevice? device);

    /// <summary>
    /// Gets the number of axes available on the controller.
    /// </summary>
    /// <returns>The axis count.</returns>
    [PreserveSig]
    uint GetControllerAxisCount();

    /// <summary>
    /// Retrieves the state of the controller axes.
    /// </summary>
    /// <param name="stateArrayCount">The expected number of elements in the state array.</param>
    /// <param name="stateArray">A pointer to an array that receives the axis states.</param>
    /// <returns>The number of items written to the array.</returns>
    [PreserveSig]
    uint GetControllerAxisState(uint stateArrayCount, ref float stateArray);

    /// <summary>
    /// Gets the number of buttons available on the controller.
    /// </summary>
    /// <returns>The button count.</returns>
    [PreserveSig]
    uint GetControllerButtonCount();

    /// <summary>
    /// Retrieves the state of the controller buttons.
    /// </summary>
    /// <param name="stateArrayCount">The expected number of elements in the state array.</param>
    /// <param name="stateArray">A pointer to an array that receives the button states.</param>
    /// <returns>The number of items written to the array.</returns>
    [PreserveSig]
    uint GetControllerButtonState(uint stateArrayCount, [MarshalAs(UnmanagedType.Bool)] ref bool stateArray);

    /// <summary>
    /// Gets the number of multi-directional switches available on the controller.
    /// </summary>
    /// <returns>The switch count.</returns>
    [PreserveSig]
    uint GetControllerSwitchCount();

    /// <summary>
    /// Retrieves the state of the controller switches.
    /// </summary>
    /// <param name="stateArrayCount">The expected number of elements in the state array.</param>
    /// <param name="stateArray">A pointer to an array that receives the switch states.</param>
    /// <returns>The number of items written to the array.</returns>
    [PreserveSig]
    uint GetControllerSwitchState(uint stateArrayCount, ref GameInputSwitchPosition stateArray);

    /// <summary>
    /// Gets the number of active key states in the reading.
    /// </summary>
    /// <returns>The key count.</returns>
    [PreserveSig]
    uint GetKeyCount();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetKeyState"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    uint GetKeyState();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetMouseState"/>.
    /// </summary>
    /// <returns>True if the state was read successfully; otherwise, false.</returns>
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Bool)]
    bool GetMouseState();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetSensorsState"/>.
    /// </summary>
    /// <returns>True if the state was read successfully; otherwise, false.</returns>
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Bool)]
    bool GetSensorsState();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetArcadeStickState"/>.
    /// </summary>
    /// <returns>True if the state was read successfully; otherwise, false.</returns>
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Bool)]
    bool GetArcadeStickState();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetFlightStickState"/>.
    /// </summary>
    /// <returns>True if the state was read successfully; otherwise, false.</returns>
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Bool)]
    bool GetFlightStickState();

    /// <summary>
    /// Retrieves the state of a standard gamepad.
    /// </summary>
    /// <param name="state">A pointer to receive the gamepad state.</param>
    /// <returns>True if the state was read successfully; otherwise, false.</returns>
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Bool)]
    bool GetGamepadState(ref GameInputGamepadState state);

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetRacingWheelState"/>.
    /// </summary>
    /// <returns>True if the state was read successfully; otherwise, false.</returns>
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Bool)]
    bool GetRacingWheelState();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetRawReport"/>.
    /// </summary>
    /// <returns>True if the report was read successfully; otherwise, false.</returns>
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Bool)]
    bool GetRawReport();
}