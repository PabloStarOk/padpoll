using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using PadPoll.Abstractions;
using PadPoll.Implementations.Windows.GameInput.Enums;
using GameInputDeviceStatus = PadPoll.Implementations.Windows.GameInput.Enums.GameInputDeviceStatus;
using GameInputFocusPolicy = PadPoll.Implementations.Windows.GameInput.Enums.GameInputFocusPolicy;
using IGameInput = PadPoll.Implementations.Windows.GameInput.Interfaces.IGameInput;
using IGameInputDevice = PadPoll.Implementations.Windows.GameInput.Interfaces.IGameInputDevice;

namespace PadPoll.Implementations.Windows;

/// <summary>
/// Detects game controllers on Windows using the GameInput API.
/// </summary>
/// <param name="gameInput">The GameInput interface instance.</param>
/// <param name="comWrappers">The COM wrappers for interacting with unmanaged objects.</param>
[SupportedOSPlatform("Windows")]
internal sealed class WindowsControllerDetector(IGameInput gameInput, ComWrappers comWrappers)
    : IControllerDetector
{
    private readonly Lock _lock = new ();
    private readonly List<WindowsController> _controllers = [];

    /// <inheritdoc/>
    public ValueTask<IReadOnlyList<IController>> DetectAsync(CancellationToken cancellationToken = default)
    {
        gameInput.SetFocusPolicy(
            GameInputFocusPolicy.EnableBackgroundInput
            | GameInputFocusPolicy.EnableBackgroundGuideButton
            | GameInputFocusPolicy.EnableBackgroundShareButton);
        gameInput.RegisterDeviceCallback(
            null,
            GameInputKind.ControllerAxis,
            GameInputDeviceStatus.Connected,
            GameInputEnumerationKind.Blocking,
            nint.Zero,
            OnDeviceRegistered,
            out ulong callbackToken);
        gameInput.UnregisterCallback(callbackToken);
        return ValueTask.FromResult<IReadOnlyList<IController>>(_controllers);
    }

    private void OnDeviceRegistered(
        ulong callbackToken,
        nint context,
        nint devicePtr,
        ulong timestamp,
        GameInputDeviceStatus currentStatus,
        GameInputDeviceStatus previousStatus)
    {
        object obj = comWrappers.GetOrCreateObjectForComInstance(devicePtr, CreateObjectFlags.None);
        var controller = (IGameInputDevice)obj;
        using Lock.Scope lockScope = _lock.EnterScope();
        _controllers.Add(new WindowsController(gameInput, controller));
    }
}