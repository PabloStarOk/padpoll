using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;
using PadPoll.Abstractions;
using PadPoll.Implementations.Windows.GameInput.Enums;
using PadPoll.Implementations.Windows.GameInput.Interfaces;
using PadPoll.Implementations.Windows.GameInput.Structs;
using PadPoll.Models;

namespace PadPoll.Implementations.Windows;

/// <summary>
/// Collects input timestamps from a Windows GameInput device.
/// </summary>
/// <param name="gameInput">The GameInput instance.</param>
/// <param name="device">The specific device to monitor.</param>
/// <param name="monitoredInputs">The set of inputs to monitor for timestamp updates.</param>
[SupportedOSPlatform("Windows")]
internal sealed class WindowsInputTimestampCollector(
    IGameInput gameInput,
    IGameInputDevice device,
    ControllerInput monitoredInputs)
    : IInputTimestampCollector
{
    private const int SpinWaitIterations = 50;
    private const double AxisDeltaTolerance = 0.1;
    private const double DefaultAxisValue = 0.5;
    private const int LeftYAxisIndex = 0;
    private const int LeftXAxisIndex = 1;
    private const int RightYAxisIndex = 2;
    private const int RightXAxisIndex = 3;

    private readonly bool _watchAll = monitoredInputs is ControllerInput.All;
    private readonly bool _watchLeftJoystick = (monitoredInputs & ControllerInput.LeftJoystick) != 0;
    private readonly bool _watchRightJoystick = (monitoredInputs & ControllerInput.RightJoystick) != 0;
    private ulong _lastTimestamp;

    /// <inheritdoc/>
    public void Dispose()
    {
    }

    /// <inheritdoc/>
    public long GetPacketMicrosecondsTimestamp()
    {
        bool isInvalidInput = true;
        ulong currentTimestamp = 0;
        do
        {
            int hr = gameInput.GetCurrentReading(GameInputKind.Controller, device, out IGameInputReading? reading);
            if (hr is not 0 || reading is null)
            {
                continue;
            }

            currentTimestamp = reading.GetTimestamp();
            isInvalidInput = currentTimestamp == 0 || currentTimestamp == _lastTimestamp || !IsWatchableInput(reading);
            DisposeReading(reading);
            if (isInvalidInput && SpinWaitIterations > 0)
            {
                Thread.SpinWait(SpinWaitIterations);
            }
        }
        while (isInvalidInput);
        _lastTimestamp = currentTimestamp;
        return (long)currentTimestamp;
    }

    private bool IsWatchableInput(IGameInputReading? reading)
    {
        if (reading is null)
        {
            return false;
        }

        if (_watchAll)
        {
            return true;
        }

        GameInputGamepadState gamepadState = default;
        bool gamepadRecognized = reading.GetGamepadState(ref gamepadState);
        return gamepadRecognized ? ValidateGamepadInput(gamepadState) : ValidateControllerInput(reading);
    }

    private bool ValidateGamepadInput(GameInputGamepadState state)
    {
        if (_watchLeftJoystick && (state.LeftThumbstickX is not 0 || state.LeftThumbstickY is not 0))
        {
            return true;
        }

        if (_watchRightJoystick && (state.RightThumbstickX is not 0 || state.RightThumbstickY is not 0))
        {
            return true;
        }

        return false;
    }

    private bool ValidateControllerInput(IGameInputReading reading)
    {
        if (_watchLeftJoystick && IsJoystickMoving(reading, LeftYAxisIndex, LeftXAxisIndex))
        {
            return true;
        }

        if (_watchRightJoystick && IsJoystickMoving(reading, RightYAxisIndex, RightXAxisIndex))
        {
            return true;
        }

        return false;
    }

    private bool IsJoystickMoving(IGameInputReading reading, int yAxisIndex, int xAxisIndex)
    {
        uint axisCount = reading.GetControllerAxisCount();
        Span<float> axisStates = stackalloc float[(int)axisCount];
        _ = reading.GetControllerAxisState(axisCount, ref axisStates[0]);
        double yAxisDelta = Math.Abs(axisStates[yAxisIndex] - DefaultAxisValue);
        double xAxisDelta = Math.Abs(axisStates[xAxisIndex] - DefaultAxisValue);
        return yAxisDelta > AxisDeltaTolerance || xAxisDelta > AxisDeltaTolerance;
    }

    private unsafe void DisposeReading(IGameInputReading reading)
    {
        nint ptr = (nint)ComInterfaceMarshaller<IGameInputReading>.ConvertToUnmanaged(reading);
        if (ptr != nint.Zero)
        {
            Marshal.Release(ptr);
        }
    }
}
