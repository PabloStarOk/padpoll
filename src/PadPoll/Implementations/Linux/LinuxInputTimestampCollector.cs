using EvDevSharp;
using Microsoft.Win32.SafeHandles;
using PadPoll.Abstractions;
using PadPoll.Models;

namespace PadPoll.Implementations.Linux;

/// <summary>
/// Collects timestamp data from Linux input devices using evdev.
/// </summary>
internal sealed class LinuxInputTimestampCollector : IInputTimestampCollector
{
    private readonly SafeFileHandle _evDevHandle;
    private readonly int _evDevFd;
    private readonly bool _watchAll;
    private readonly bool _watchLeftJoystick;
    private readonly bool _watchRightJoystick;
    private bool _currentIsValidPacket;

    /// <summary>
    /// Initializes a new instance of the <see cref="LinuxInputTimestampCollector"/> class.
    /// </summary>
    /// <param name="handle">The safe file handle to the evdev device.</param>
    /// <param name="monitoredInputs">The controller inputs to monitor for timestamp collection.</param>
    public LinuxInputTimestampCollector(SafeFileHandle handle, ControllerInput monitoredInputs)
    {
        _evDevHandle = handle;
        _evDevFd = handle.DangerousGetHandle().ToInt32();
        _watchAll = monitoredInputs is ControllerInput.All;
        _watchLeftJoystick = (monitoredInputs & ControllerInput.LeftJoystick) != 0;
        _watchRightJoystick = (monitoredInputs & ControllerInput.RightJoystick) != 0;
    }

    /// <inheritdoc/>
    public unsafe long GetPacketMicrosecondsTimestamp()
    {
        while (true)
        {
            InputEvent inputEvent;
            long bytesRead = LibC.read(_evDevFd, &inputEvent, sizeof(InputEvent));
            if (bytesRead != sizeof(InputEvent))
            {
                continue;
            }

            if (IsWatchableInput(inputEvent))
            {
                _currentIsValidPacket = true;
            }

            if (inputEvent.Type is not EvDev.EventType.EvSyn || inputEvent.Code is not EvDev.SynCode.SynReport
                || !_currentIsValidPacket)
            {
                continue;
            }

            _currentIsValidPacket = false;
            return inputEvent.GetMicrosecondsTimestamp();
        }
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _evDevHandle.Dispose();
    }

    private bool IsWatchableInput(InputEvent inputEvent)
    {
        if (inputEvent.Type is not (ushort)EvDevEventType.EV_ABS)
        {
            return false;
        }

        if (_watchAll)
        {
            return true;
        }

        return inputEvent.Code switch
        {
            EvDev.AbsCodes.AbsX or EvDev.AbsCodes.AbsY => _watchLeftJoystick,
            EvDev.AbsCodes.AbsRx or EvDev.AbsCodes.AbsRy => _watchRightJoystick,
            _ => false,
        };
    }
}