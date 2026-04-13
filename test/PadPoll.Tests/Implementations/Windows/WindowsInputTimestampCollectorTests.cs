using System.Runtime.InteropServices;
using System.Runtime.Versioning;
using Moq;
using PadPoll.Implementations.Windows;
using PadPoll.Implementations.Windows.GameInput.Enums;
using PadPoll.Implementations.Windows.GameInput.Interfaces;
using PadPoll.Implementations.Windows.GameInput.Structs;
using PadPoll.Models;
using PadPoll.Tests.Implementations.Windows.Stubs;

namespace PadPoll.Tests.Implementations.Windows;

/// <summary>
/// Unit tests for the <see cref="WindowsInputTimestampCollector"/> class.
/// </summary>
[Trait("Category", "Unit")]
[SupportedOSPlatform("Windows")]
public sealed class WindowsInputTimestampCollectorTests : IDisposable
{
    private readonly MockRepository _mockRepository;
    private readonly Mock<IGameInput> _gameInputMock;
    private readonly Mock<IGameInputDevice> _deviceMock;
    private readonly Mock<IGameInputReading> _readingMock;
    private readonly WindowsInputTimestampCollector _collector;
    private IGameInputReading? _reading;

    /// <summary>
    /// Initializes a new instance of the <see cref="WindowsInputTimestampCollectorTests"/> class.
    /// </summary>
    public WindowsInputTimestampCollectorTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);
        _gameInputMock = _mockRepository.Create<IGameInput>();
        _deviceMock = _mockRepository.Create<IGameInputDevice>();
        _readingMock = _mockRepository.Create<IGameInputReading>();
        _reading = new GameInputReadingMockWrapper(_readingMock);
        _collector = new WindowsInputTimestampCollector(_gameInputMock.Object, _deviceMock.Object, ControllerInput.All);
    }

    /// <summary>
    /// Gets the test cases for generic unmonitored input scenarios on generic controllers.
    /// </summary>
    /// <returns>A <see cref="TheoryData{T1, T2, T3}"/> containing the monitored input and expected axis states.</returns>
    public static TheoryData<ControllerInput, float[], float[]> GetGenericUnmonitoredInputTestCases()
    {
        const float defaultAxisValue = 0.5f;
        return new TheoryData<ControllerInput, float[], float[]>
        {
            {
                ControllerInput.LeftJoystick,
                [defaultAxisValue, defaultAxisValue, 10f, 14f],
                [10f, 14f, defaultAxisValue, defaultAxisValue]
            },
            {
                ControllerInput.RightJoystick,
                [10f, 14f, defaultAxisValue, defaultAxisValue],
                [defaultAxisValue, defaultAxisValue, 10f, 14f]
            },
        };
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _mockRepository.VerifyAll();
    }

    /// <summary>
    /// Verifies that <see cref="WindowsInputTimestampCollector.GetPacketMicrosecondsTimestamp"/> returns the expected
    /// timestamp from the input reading.
    /// </summary>
    [Fact]
    public void GetPacketMicrosecondsTimestamp_should_ReturnExpectedTimestamp()
    {
        // Arrange
        const long expected = 1234;
        _readingMock.Setup(m => m.GetTimestamp()).Returns(expected);
        SetupGameInputMock();

        // Act
        long actual = _collector.GetPacketMicrosecondsTimestamp();

        // Assert
        Assert.Equal(expected, actual);
    }

    /// <summary>
    /// Verifies that <see cref="WindowsInputTimestampCollector.GetPacketMicrosecondsTimestamp"/> retries acquiring
    /// a reading when the initial call fails.
    /// </summary>
    [Fact]
    public void GetPacketMicrosecondsTimestamp_should_RetryWhenReadingFails()
    {
        // Arrange
        const long expected = 1234;
        _readingMock.Setup(m => m.GetTimestamp()).Returns(expected);
        _gameInputMock
            .SetupSequence(m => m.GetCurrentReading(GameInputKind.Controller, _deviceMock.Object, out _reading))
            .Returns(1)
            .Returns(0);

        // Act
        long actual = _collector.GetPacketMicrosecondsTimestamp();

        // Assert
        Assert.Equal(expected, actual);
        _gameInputMock.Verify(
            m => m.GetCurrentReading(GameInputKind.Controller, _deviceMock.Object, out _reading),
            Times.Exactly(2));
    }

    /// <summary>
    /// Verifies that <see cref="WindowsInputTimestampCollector.GetPacketMicrosecondsTimestamp"/> ignores
    /// duplicate timestamps and waits for a new one.
    /// </summary>
    [Fact]
    public void GetPacketMicrosecondsTimestamp_should_IgnoreDuplicateTimestamp()
    {
        // Arrange
        const long firstExpected = 1111;
        const long secondExpected = 2222;
        _readingMock.SetupSequence(m => m.GetTimestamp()).Returns(firstExpected).Returns(firstExpected)
            .Returns(secondExpected);
        SetupGameInputMock();

        // Act
        long firstActual = _collector.GetPacketMicrosecondsTimestamp();
        long secondActual = _collector.GetPacketMicrosecondsTimestamp();

        // Assert
        Assert.Equal(firstExpected, firstActual);
        Assert.Equal(secondExpected, secondActual);
        _gameInputMock.Verify(
            m => m.GetCurrentReading(GameInputKind.Controller, _deviceMock.Object, out _reading),
            Times.Exactly(3));
    }

    /// <summary>
    /// Verifies that <see cref="WindowsInputTimestampCollector.GetPacketMicrosecondsTimestamp"/> ignores
    /// a timestamp of zero and waits for a non-zero value.
    /// </summary>
    [Fact]
    public void GetPacketMicrosecondsTimestamp_should_IgnoreZeroTimestamp()
    {
        // Arrange
        const long zeroTimestamp = 0;
        const long expected = 1234;
        _readingMock.SetupSequence(m => m.GetTimestamp()).Returns(zeroTimestamp).Returns(expected);
        SetupGameInputMock();

        // Act
        long actual = _collector.GetPacketMicrosecondsTimestamp();

        // Assert
        Assert.Equal(expected, actual);
        _gameInputMock.Verify(
            m => m.GetCurrentReading(GameInputKind.Controller, _deviceMock.Object, out _reading),
            Times.Exactly(2));
    }

    /// <summary>
    /// Verifies that <see cref="WindowsInputTimestampCollector.GetPacketMicrosecondsTimestamp"/> ignores
    /// timestamps from unmonitored inputs when the controller is detected as a gamepad.
    /// </summary>
    [Fact]
    public void GetPacketMicrosecondsTimestamp_should_IgnoreUnmonitoredInput_when_ControllerIsDetectedAsGamepad()
    {
        // Arrange
        const long unexpectedTimestamp = 1111;
        const long expectedTimestamp = 2222;
        bool returnValidState = false;
        var collector = new WindowsInputTimestampCollector(
            _gameInputMock.Object, _deviceMock.Object, ControllerInput.LeftJoystick);
        var invalidState = new GameInputGamepadState(
            GameInputGamepadButtons.LeftThumbstickLeft | GameInputGamepadButtons.LeftThumbstickRight,
            leftTrigger: 0,
            rightTrigger: 0,
            leftThumbstickX: 0,
            leftThumbstickY: 0,
            rightThumbstickX: 1,
            rightThumbstickY: 1);
        var validState = new GameInputGamepadState(
            GameInputGamepadButtons.LeftThumbstickLeft | GameInputGamepadButtons.LeftThumbstickRight,
            leftTrigger: 0,
            rightTrigger: 0,
            leftThumbstickX: 1,
            leftThumbstickY: 1,
            rightThumbstickX: 0,
            rightThumbstickY: 0);
        _readingMock.SetupSequence(m => m.GetTimestamp()).Returns(unexpectedTimestamp).Returns(expectedTimestamp);
        _readingMock.Setup(m => m.GetGamepadState(ref It.Ref<GameInputGamepadState>.IsAny))
            .Callback((ref GameInputGamepadState state) =>
            {
                if (returnValidState)
                {
                    state = validState;
                }
                else
                {
                    state = invalidState;
                    returnValidState = true;
                }
            })
            .Returns(true);
        SetupGameInputMock();

        // Act
        long actual = collector.GetPacketMicrosecondsTimestamp();

        // Assert
        Assert.Equal(expectedTimestamp, actual);
        _gameInputMock.Verify(
            m => m.GetCurrentReading(GameInputKind.Controller, _deviceMock.Object, out _reading),
            Times.Exactly(2));
    }

    /// <summary>
    /// Verifies that <see cref="WindowsInputTimestampCollector.GetPacketMicrosecondsTimestamp"/> ignores
    /// timestamps from unmonitored inputs when the controller is detected as a generic device.
    /// </summary>
    /// <param name="monitoredInput">The input that is being monitored.</param>
    /// <param name="unmonitoredAxisStates">The axis states representing unmonitored input.</param>
    /// <param name="monitoredAxisStates">The axis states representing monitored input.</param>
    [Theory]
    [MemberData(nameof(GetGenericUnmonitoredInputTestCases))]
    public void GetPacketMicrosecondsTimestamp_should_IgnoreUnmonitoredInput_when_ControllerIsDetectedAsGeneric(
        ControllerInput monitoredInput,
        float[] unmonitoredAxisStates,
        float[] monitoredAxisStates)
    {
        // Arrange
        const uint axisCount = 4;
        const long unexpectedTimestamp = 1111;
        const long expectedTimestamp = 2222;
        bool returnValidState = false;
        var readingWrapper = (GameInputReadingMockWrapper)_reading!;
        var collector = new WindowsInputTimestampCollector(_gameInputMock.Object, _deviceMock.Object, monitoredInput);
        _readingMock.SetupSequence(m => m.GetTimestamp()).Returns(unexpectedTimestamp).Returns(expectedTimestamp);
        _readingMock.Setup(m => m.GetGamepadState(ref It.Ref<GameInputGamepadState>.IsAny)).Returns(false);
        _readingMock.Setup(m => m.GetControllerAxisCount()).Returns(axisCount);
        readingWrapper.GetControllerAxisStateCallback = (axes, ref firstArrayValue) =>
            {
                Span<float> axisStates = MemoryMarshal.CreateSpan(ref firstArrayValue, (int)axes);
                if (returnValidState)
                {
                    axisStates[0] = monitoredAxisStates[0];
                    axisStates[1] = monitoredAxisStates[1];
                    axisStates[2] = monitoredAxisStates[2];
                    axisStates[3] = monitoredAxisStates[3];
                }
                else
                {
                    axisStates[0] = unmonitoredAxisStates[0];
                    axisStates[1] = unmonitoredAxisStates[1];
                    axisStates[2] = unmonitoredAxisStates[2];
                    axisStates[3] = unmonitoredAxisStates[3];
                    returnValidState = true;
                }

                return 2;
            };
        SetupGameInputMock();

        // Act
        long actual = collector.GetPacketMicrosecondsTimestamp();

        // Assert
        Assert.Equal(expectedTimestamp, actual);
        _gameInputMock.Verify(
            m => m.GetCurrentReading(GameInputKind.Controller, _deviceMock.Object, out _reading),
            Times.Exactly(2));
    }

    private void SetupGameInputMock()
    {
        _gameInputMock
            .Setup(m => m.GetCurrentReading(GameInputKind.Controller, _deviceMock.Object, out _reading))
            .Returns(0);
    }
}