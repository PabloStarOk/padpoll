using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using PadPoll.Implementations.Windows.GameInput.Delegates;
using PadPoll.Implementations.Windows.GameInput.Enums;

namespace PadPoll.Implementations.Windows.GameInput.Interfaces;

/// <summary>
/// The main interface for GameInput, used to find devices, register callbacks,
/// and retrieve input readings.
/// </summary>
[GeneratedComInterface]
[Guid(Constants.GameInputIid)]
[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
internal partial interface IGameInput
{
    /// <summary>
    /// Retrieves the current high-resolution timestamp from the GameInput system.
    /// </summary>
    /// <returns>The current timestamp in microseconds.</returns>
    [PreserveSig]
    ulong GetCurrentTimestamp();

    /// <summary>
    /// Retrieves the most recent reading from the GameInput system for a specific device or input kind.
    /// </summary>
    /// <param name="inputKind">The kind of input to retrieve.</param>
    /// <param name="device">An optional pointer to a specific device to retrieve input from.</param>
    /// <param name="reading">When this method returns, contains the most recent reading.</param>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int GetCurrentReading(
        GameInputKind inputKind,
        [MarshalAs(UnmanagedType.Interface)] IGameInputDevice? device,
        out IGameInputReading? reading);

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetNextReading"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int GetNextReading();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="GetPreviousReading"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int GetPreviousReading();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="RegisterReadingCallback"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int RegisterReadingCallback();

    /// <summary>
    /// Registers a callback to be notified when a device's status changes or when new devices are connected.
    /// </summary>
    /// <param name="device">An optional pointer to a specific device to monitor.</param>
    /// <param name="inputKind">Filters callbacks to devices supporting the specified input kinds.</param>
    /// <param name="statusFilter">Filters callbacks to specific device status changes.</param>
    /// <param name="enumerationKind">Specifies how to initial enumerate existing devices.</param>
    /// <param name="context">A user-defined pointer passed to the callback function.</param>
    /// <param name="callback">The callback function to be invoked.</param>
    /// <param name="callbackToken">When this method returns, contains a token representing the registered callback.</param>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int RegisterDeviceCallback(
        IGameInputDevice? device,
        GameInputKind inputKind,
        GameInputDeviceStatus statusFilter,
        GameInputEnumerationKind enumerationKind,
        nint context,
        GameInputDeviceCallback callback,
        out ulong callbackToken);

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="RegisterSystemButtonCallback"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int RegisterSystemButtonCallback();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="RegisterKeyboardLayoutCallback"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int RegisterKeyboardLayoutCallback();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="StopCallback"/>.
    /// </summary>
    [PreserveSig]
    void StopCallback();

    /// <summary>
    /// Unregisters a previously registered callback.
    /// </summary>
    /// <param name="callbackToken">The token returned when the callback was registered.</param>
    /// <returns>True if the callback was successfully unregistered; otherwise, false.</returns>
    [PreserveSig]
    [return: MarshalAs(UnmanagedType.Bool)]
    bool UnregisterCallback(ulong callbackToken);

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="CreateDispatcher"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int CreateDispatcher();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="FindDeviceFromId"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int FindDeviceFromId();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="FindDeviceFromPlatformString"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int FindDeviceFromPlatformString();

    /// <summary>
    /// Sets the focus policy for the GameInput system, determining when input is delivered to the application.
    /// </summary>
    /// <param name="policy">The focus policy to apply.</param>
    [PreserveSig]
    void SetFocusPolicy(GameInputFocusPolicy policy);

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="CreateAggregateDevice"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int CreateAggregateDevice();

    /// <summary>
    /// Placeholder method to maintain the COM VTable order for <see cref="DisableAggregateDevice"/>.
    /// </summary>
    /// <returns>An HRESULT indicating success or failure.</returns>
    [PreserveSig]
    int DisableAggregateDevice();
}
