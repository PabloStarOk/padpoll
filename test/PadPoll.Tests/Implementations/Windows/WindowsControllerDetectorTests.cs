using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using System.Runtime.Versioning;
using Moq;
using PadPoll.Abstractions;
using PadPoll.Implementations.Windows;
using PadPoll.Implementations.Windows.GameInput.Delegates;
using PadPoll.Implementations.Windows.GameInput.Enums;
using PadPoll.Implementations.Windows.GameInput.Interfaces;
using PadPoll.Tests.Implementations.Windows.Stubs;

namespace PadPoll.Tests.Implementations.Windows;

/// <summary>
/// Unit tests for the <see cref="WindowsControllerDetector"/> class.
/// </summary>
[Trait("Category", "Unit")]
[SupportedOSPlatform("windows")]
public sealed class WindowsControllerDetectorTests : IDisposable
{
    private readonly MockRepository _mockRepository;
    private readonly Mock<IGameInput> _gameInputMock;
    private readonly StrategyBasedComWrappers _comWrappers;
    private readonly WindowsControllerDetector _detector;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsControllerDetectorTests"/> class.
    /// </summary>
    public WindowsControllerDetectorTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);
        _gameInputMock = _mockRepository.Create<IGameInput>();
        _comWrappers = new StrategyBasedComWrappers();
        _detector = new WindowsControllerDetector(_gameInputMock.Object, _comWrappers);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _mockRepository.VerifyAll();
    }

    /// <summary>
    /// Verifies that <see cref="WindowsControllerDetector.DetectAsync"/> enables background input delivering
    /// by setting the appropriate focus policy on the GameInput instance.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task DetectAsync_should_EnableBackgroundInputDelivering()
    {
        // Act
        await _detector.DetectAsync();

        // Assert
        _gameInputMock.Verify(
            m => m.SetFocusPolicy(
            GameInputFocusPolicy.EnableBackgroundInput
            | GameInputFocusPolicy.EnableBackgroundGuideButton
            | GameInputFocusPolicy.EnableBackgroundShareButton),
            Times.Once);
    }

    /// <summary>
    /// Verifies that <see cref="WindowsControllerDetector.DetectAsync"/> registers a device callback
    /// during execution and correctly unregisters it before returning.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task DetectAsync_should_RegisterAndUnregisterDeviceCallback()
    {
        // Arrange
        ulong callbackToken = 1234;
        _gameInputMock.Setup(m => m.RegisterDeviceCallback(
            null,
            GameInputKind.ControllerAxis,
            GameInputDeviceStatus.Connected,
            GameInputEnumerationKind.Blocking,
            nint.Zero,
            It.IsAny<GameInputDeviceCallback>(),
            out callbackToken));

        // Act
        await _detector.DetectAsync();

        // Assert
        _gameInputMock.Verify(
            m => m.RegisterDeviceCallback(
                null,
                GameInputKind.ControllerAxis,
                GameInputDeviceStatus.Connected,
                GameInputEnumerationKind.Blocking,
                nint.Zero,
                It.IsAny<GameInputDeviceCallback>(),
                out callbackToken),
            Times.Once);
        _gameInputMock.Verify(m => m.UnregisterCallback(callbackToken), Times.Once);
    }

    /// <summary>
    /// Verifies that <see cref="WindowsControllerDetector.DetectAsync"/> returns the expected list of controllers
    /// when the device callback is triggered.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task DetectAsync_should_ReturnExpectedControllers()
    {
        // Arrange
        var device = new GameInputDeviceStub();
        ulong callbackToken = 1234;
        nint devicePtr = _comWrappers.GetOrCreateComInterfaceForObject(device, CreateComInterfaceFlags.None);
        _gameInputMock.Setup(m => m.RegisterDeviceCallback(
                null,
                GameInputKind.ControllerAxis,
                GameInputDeviceStatus.Connected,
                GameInputEnumerationKind.Blocking,
                nint.Zero,
                It.IsAny<GameInputDeviceCallback>(),
                out callbackToken))
            .Callback((
                IGameInputDevice _,
                GameInputKind _,
                GameInputDeviceStatus _,
                GameInputEnumerationKind _,
                nint _,
                GameInputDeviceCallback callback,
                out ulong token) =>
            {
                token = callbackToken;
                callback.Invoke(token, nint.Zero, devicePtr, 0, GameInputDeviceStatus.Any, GameInputDeviceStatus.Any);
            })
            .Returns(0);

        // Act
        IReadOnlyList<IController> controllers = await _detector.DetectAsync();

        // Assert
        Assert.NotEmpty(controllers);
        Assert.Single(controllers);
        Marshal.Release(devicePtr);
    }
}