using System.Runtime.InteropServices;
using PadPoll.Implementations.Windows.GameInput.Enums;

namespace PadPoll.Implementations.Windows.GameInput.Structs;

/// <summary>
/// Provides information about a GameInput device, including its hardware IDs, capabilities, and detailed sub-info.
/// </summary>
[StructLayout(LayoutKind.Sequential)]
internal struct GameInputDeviceInfo
{
    private const int HidPMaxLength = 256;

    private readonly ushort _vendorId;
    private readonly ushort _productId;
    private readonly ushort _revisionNumber;
    private readonly nint _usage;
    private readonly nint _hardwareVersion;
    private readonly nint _firmwareVersion;
    private unsafe fixed byte _deviceId[Constants.AppLocalDeviceIdSize];
    private unsafe fixed byte _deviceRootId[Constants.AppLocalDeviceIdSize];
    private readonly int _deviceFamily;
    private readonly GameInputKind _supportedInput;
    private readonly int _supportedRumbleMotors;
    private readonly int _supportedSystemButtons;
    private readonly Guid _containerId;

    private readonly nint _displayNamePtr;
    private readonly nint _pnpPath;

    private readonly nint _keyboardInfo;
    private readonly nint _mouseInfo;
    private readonly nint _sensorsInfo;
    private readonly nint _controllerInfo;
    private readonly nint _arcadeStickInfo;
    private readonly nint _flightStickInfo;
    private readonly nint _gamepadInfo;
    private readonly nint _racingWheelInfo;

    private readonly uint _forceFeedbackMotorCount;
    private readonly nint _forceFeedbackMotorInfo;

    private readonly uint _inputReportCount;
    private readonly nint _inputReportInfo;

    private readonly uint _outputReportCount;
    private readonly nint _outputReportInfo;

    /// <summary>
    /// Gets the display name of the device.
    /// </summary>
    public string DisplayName => Marshal.PtrToStringUTF8(_displayNamePtr, HidPMaxLength);
}