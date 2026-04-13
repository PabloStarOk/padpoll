using System.Runtime.InteropServices.Marshalling;
using Moq;
using PadPoll.Implementations.Windows.GameInput.Enums;
using PadPoll.Implementations.Windows.GameInput.Interfaces;
using PadPoll.Implementations.Windows.GameInput.Structs;

namespace PadPoll.Tests.Implementations.Windows.Stubs;

/// <summary>
/// A wrapper for <see cref="Mock{IGameInputReading}"/> that can be used with <see cref="GeneratedComClassAttribute"/>.
/// </summary>
[GeneratedComClass]
internal sealed partial class GameInputReadingMockWrapper : IGameInputReading
{
    /// <summary>
    /// Represents a callback for getting the controller axis state.
    /// </summary>
    /// <param name="axisCount">The number of axes.</param>
    /// <param name="axisArray">A reference to the array containing axis states.</param>
    /// <returns>The number of axis values retrieved.</returns>
    public delegate uint ControllerAxisStateCallback(uint axisCount, ref float axisArray);

    /// <summary>
    /// Gets or sets the callback for <see cref="GetControllerAxisState"/>.
    /// </summary>
    public ControllerAxisStateCallback? GetControllerAxisStateCallback { get; set; }

    private readonly Mock<IGameInputReading> _mock;

    /// <summary>
    /// Initializes a new instance of the <see cref="GameInputReadingMockWrapper"/> class.
    /// </summary>
    /// <param name="mock">The mock instance to wrap.</param>
    public GameInputReadingMockWrapper(Mock<IGameInputReading> mock)
    {
        _mock = mock;
    }

    /// <inheritdoc/>
    public GameInputKind GetInputKind() => _mock.Object.GetInputKind();

    /// <inheritdoc/>
    public ulong GetTimestamp() => _mock.Object.GetTimestamp();

    /// <inheritdoc/>
    public void GetDevice(out IGameInputDevice? device) => _mock.Object.GetDevice(out device);

    /// <inheritdoc/>
    public uint GetControllerAxisCount() => _mock.Object.GetControllerAxisCount();

    /// <inheritdoc/>
    public uint GetControllerAxisState(uint stateArrayCount, ref float stateArray)
    {
        return GetControllerAxisStateCallback?.Invoke(stateArrayCount, ref stateArray)
            ?? _mock.Object.GetControllerAxisState(stateArrayCount, ref stateArray);
    }

    /// <inheritdoc/>
    public uint GetControllerButtonCount() => _mock.Object.GetControllerButtonCount();

    /// <inheritdoc/>
    public uint GetControllerButtonState(uint stateArrayCount, ref bool stateArray)
        => _mock.Object.GetControllerButtonState(stateArrayCount, ref stateArray);

    /// <inheritdoc/>
    public uint GetControllerSwitchCount() => _mock.Object.GetControllerSwitchCount();

    /// <inheritdoc/>
    public uint GetControllerSwitchState(uint stateArrayCount, ref GameInputSwitchPosition stateArray)
        => _mock.Object.GetControllerSwitchState(stateArrayCount, ref stateArray);

    /// <inheritdoc/>
    public uint GetKeyCount() => _mock.Object.GetKeyCount();

    /// <inheritdoc/>
    public uint GetKeyState() => _mock.Object.GetKeyState();

    /// <inheritdoc/>
    public bool GetMouseState() => _mock.Object.GetMouseState();

    /// <inheritdoc/>
    public bool GetSensorsState() => _mock.Object.GetSensorsState();

    /// <inheritdoc/>
    public bool GetArcadeStickState() => _mock.Object.GetArcadeStickState();

    /// <inheritdoc/>
    public bool GetFlightStickState() => _mock.Object.GetFlightStickState();

    /// <inheritdoc/>
    public bool GetGamepadState(ref GameInputGamepadState state)
        => _mock.Object.GetGamepadState(ref state);

    /// <inheritdoc/>
    public bool GetRacingWheelState() => _mock.Object.GetRacingWheelState();

    /// <inheritdoc/>
    public bool GetRawReport() => _mock.Object.GetRawReport();
}
