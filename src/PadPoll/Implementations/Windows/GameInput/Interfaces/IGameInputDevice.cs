using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using PadPoll.Implementations.Windows.GameInput.Enums;
using PadPoll.Implementations.Windows.GameInput.Structs;

namespace PadPoll.Implementations.Windows.GameInput.Interfaces;

/// <summary>
/// Represents a physical input device and provides methods to query its capabilities and status.
/// </summary>
[GeneratedComInterface]
[Guid(Constants.GameInputDeviceIid)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal partial interface IGameInputDevice
{
    /// <summary>
    /// Retrieves static information about the device.
    /// </summary>
    /// <param name="info">A pointer that receives the GameInputDeviceInfo structure.</param>
    /// <returns>Returns S_OK on success; otherwise, returns an error code.</returns>
    [PreserveSig]
    unsafe int GetDeviceInfo(out GameInputDeviceInfo* info);

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetHapticInfo"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int GetHapticInfo();

    /// <summary>
    /// Retrieves the current status of the device.
    /// </summary>
    /// <returns>A bitmask of GameInputDeviceStatus flags.</returns>
    [PreserveSig]
    uint GetDeviceStatus();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="CreateForceFeedbackEffect"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int CreateForceFeedbackEffect();

    /// <summary>
    /// Checks whether a specific force feedback motor is powered on.
    /// </summary>
    /// <param name="motorIndex">The index of the motor to check.</param>
    /// <returns>True if the motor is powered on; otherwise, false.</returns>
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Bool)]
    bool IsForceFeedbackMotorPoweredOn(uint motorIndex);

    /// <summary>
    /// Sets the master gain for a force feedback motor.
    /// </summary>
    /// <param name="motorIndex">The index of the motor.</param>
    /// <param name="masterGain">The gain value to set, ranging from 0.0 to 1.0.</param>
    [PreserveSig]
    void SetForceFeedbackMotorGain(uint motorIndex, float masterGain);

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="SetRumbleState"/>.
    /// </summary>
    [PreserveSig]
    void SetRumbleState();

    /// <summary>
    /// Sends a command to the device through the DirectInput escape mechanism.
    /// </summary>
    /// <param name="command">The command to send.</param>
    /// <param name="bufferIn">A pointer to the input buffer.</param>
    /// <param name="bufferInSize">The size of the input buffer.</param>
    /// <param name="bufferOut">A pointer to the output buffer.</param>
    /// <param name="bufferOutSize">The size of the output buffer.</param>
    /// <param name="bufferOutSizeWritten">A pointer that receives the number of bytes written to the output buffer.</param>
    /// <returns>Returns S_OK on success; otherwise, returns an error code.</returns>
    [PreserveSig]
    unsafe int DirectInputEscape(
        uint command,
        void* bufferIn,
        uint bufferInSize,
        void* bufferOut,
        uint bufferOutSize,
        uint* bufferOutSizeWritten);

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="CreateInputMapper"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int CreateInputMapper();

    /// <summary>
    /// Retrieves the number of extra axes for the specified input kind.
    /// </summary>
    /// <param name="inputKind">The input kind to query.</param>
    /// <param name="extraAxisCount">A pointer that receives the count of extra axes.</param>
    /// <returns>Returns S_OK on success; otherwise, returns an error code.</returns>
    [PreserveSig]
    unsafe int GetExtraAxisCount(GameInputKind inputKind, uint* extraAxisCount);

    /// <summary>
    /// Retrieves the number of extra buttons for the specified input kind.
    /// </summary>
    /// <param name="inputKind">The input kind to query.</param>
    /// <param name="extraButtonCount">A pointer that receives the count of extra buttons.</param>
    /// <returns>Returns S_OK on success; otherwise, returns an error code.</returns>
    [PreserveSig]
    unsafe int GetExtraButtonCount(GameInputKind inputKind, uint* extraButtonCount);

    /// <summary>
    /// Retrieves the indexes of extra axes for the specified input kind.
    /// </summary>
    /// <param name="inputKind">The input kind to query.</param>
    /// <param name="extraAxisCount">The number of extra axes to retrieve.</param>
    /// <param name="extraAxisIndexes">A pointer to an array that receives the extra axis indexes.</param>
    /// <returns>Returns S_OK on success; otherwise, returns an error code.</returns>
    [PreserveSig]
    unsafe int GetExtraAxisIndexes(GameInputKind inputKind, uint extraAxisCount, byte* extraAxisIndexes);

    /// <summary>
    /// Retrieves the indexes of extra buttons for the specified input kind.
    /// </summary>
    /// <param name="inputKind">The input kind to query.</param>
    /// <param name="extraButtonCount">The number of extra buttons to retrieve.</param>
    /// <param name="extraButtonIndexes">A pointer to an array that receives the extra button indexes.</param>
    /// <returns>Returns S_OK on success; otherwise, returns an error code.</returns>
    [PreserveSig]
    unsafe int GetExtraButtonIndexes(GameInputKind inputKind, uint extraButtonCount, byte* extraButtonIndexes);

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="CreateRawDeviceReport"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int CreateRawDeviceReport();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="SendRawDeviceOutput"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int SendRawDeviceOutput();
}