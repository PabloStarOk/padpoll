using Moq;
using PadPoll.Abstractions;
using PadPoll.Models;

namespace PadPoll.Tests;

/// <summary>
/// Unit tests for the <see cref="App"/> class.
/// </summary>
[Trait("Category", "Unit")]
public sealed class AppTests : IDisposable
{
    private readonly MockRepository _mockRepository;
    private readonly Mock<IStandardIo> _stdIoMock;
    private readonly Mock<IControllerDetector> _controllerDetectorMock;
    private readonly Mock<IMetricsCalculator> _metricsCalculatorMock;
    private readonly Mock<IInputTimestampCollector> _timestampCollectorMock;
    private readonly Mock<IController> _controllerMock;
    private readonly App _app;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppTests"/> class.
    /// </summary>
    public AppTests()
    {
        _mockRepository = new MockRepository(MockBehavior.Loose);
        _stdIoMock = _mockRepository.Create<IStandardIo>();
        _controllerDetectorMock = _mockRepository.Create<IControllerDetector>();
        _metricsCalculatorMock = _mockRepository.Create<IMetricsCalculator>();
        _timestampCollectorMock = _mockRepository.Create<IInputTimestampCollector>();
        _controllerMock = _mockRepository.Create<IController>();
        _app = new App(_stdIoMock.Object, _controllerDetectorMock.Object, _metricsCalculatorMock.Object);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        _mockRepository.VerifyAll();
    }

    /// <summary>
    /// Tests that <see cref="App.RunAsync"/> executes the expected process flow with valid inputs.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task RunAsync_should_ExecuteExpectedProcess()
    {
        // Arrange
        const string selectedController = "0";
        const string selectedJoystick = "R";
        const int stabilizationSamples = 20;
        const int expectedSamples = 2500;
        const int expectedHz = 120;
        long timestamp = 1000;
        List<IController> controllers = [_controllerMock.Object];
        _controllerDetectorMock.Setup(m => m.DetectAsync(It.IsAny<CancellationToken>())).ReturnsAsync(controllers);
        _stdIoMock.SetupSequence(m => m.In.ReadLine()).Returns(selectedController).Returns(selectedJoystick)
            .Returns(expectedSamples.ToString).Returns(expectedHz.ToString);
        _controllerMock.Setup(m => m.GetTimestampCollector(ControllerInput.RightJoystick))
            .Returns(_timestampCollectorMock.Object);
        _timestampCollectorMock.Setup(m => m.GetPacketMicrosecondsTimestamp()).Returns(() =>
        {
            timestamp += 8;
            return timestamp;
        });
        _stdIoMock.Setup(m => m.Out).Returns(Console.Out);
        _metricsCalculatorMock.Setup(m => m.Calculate(It.IsAny<long[]>(), expectedHz));

        // Act
        await RunAppAsync();

        // Assert
        _controllerDetectorMock.Verify(m => m.DetectAsync(It.IsAny<CancellationToken>()), Times.Once);
        _stdIoMock.Verify(m => m.In.ReadLine(), Times.Exactly(4));
        _controllerMock.Verify(m => m.GetTimestampCollector(ControllerInput.RightJoystick), Times.Once);
        _timestampCollectorMock
            .Verify(m => m.GetPacketMicrosecondsTimestamp(), Times.Exactly(expectedSamples + stabilizationSamples));
        _metricsCalculatorMock
            .Verify(m => m.Calculate(It.Is<long[]>(a => a.Length == expectedSamples), expectedHz), Times.Once);
    }

    /// <summary>
    /// Tests that <see cref="App.RunAsync"/> returns early when no controllers are detected.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Fact]
    public async Task RunAsync_should_ReturnEarlyWhenThereAreNoControllers()
    {
        // Arrange
        _controllerDetectorMock.Setup(m => m.DetectAsync(It.IsAny<CancellationToken>())).ReturnsAsync([]);
        _stdIoMock.Setup(m => m.Out).Returns(Console.Out);

        // Act
        await RunAppAsync();

        // Assert
        VerifyUnhappyPathMocks(expectedReadLineCalls: 0);
    }

    /// <summary>
    /// Tests that <see cref="App.RunAsync"/> returns early when the selected controller input is invalid.
    /// </summary>
    /// <param name="selectedController">The controller input value to test.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Theory]
    [InlineData("1")]
    [InlineData("-1")]
    [InlineData("Invalid Controller")]
    public async Task RunAsync_should_ReturnEarlyWhenSelectedControllerInputIsInvalid(string selectedController)
    {
        // Arrange
        List<IController> controllers = [_controllerMock.Object];
        _controllerDetectorMock.Setup(m => m.DetectAsync(It.IsAny<CancellationToken>())).ReturnsAsync(controllers);
        _stdIoMock.Setup(m => m.In.ReadLine()).Returns(selectedController);
        SetupStdOutAndErr();

        // Act
        await RunAppAsync();

        // Assert
        VerifyUnhappyPathMocks(expectedReadLineCalls: 1);
    }

    /// <summary>
    /// Tests that <see cref="App.RunAsync"/> returns early when the selected joystick input is invalid.
    /// </summary>
    /// <param name="selectedJoystick">The joystick input value to test.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Theory]
    [InlineData("0")]
    [InlineData("Left")]
    [InlineData("Right")]
    [InlineData("Invalid Joystick")]
    [InlineData("d")]
    public async Task RunAsync_should_ReturnEarlyWhenSelectedJoystickInputIsInvalid(string selectedJoystick)
    {
        // Arrange
        const string selectedController = "0";
        List<IController> controllers = [_controllerMock.Object];
        _controllerDetectorMock.Setup(m => m.DetectAsync(It.IsAny<CancellationToken>())).ReturnsAsync(controllers);
        _stdIoMock.SetupSequence(m => m.In.ReadLine()).Returns(selectedController).Returns(selectedJoystick);
        SetupStdOutAndErr();

        // Act
        await RunAppAsync();

        // Assert
        VerifyUnhappyPathMocks(expectedReadLineCalls: 2);
    }

    /// <summary>
    /// Tests that <see cref="App.RunAsync"/> returns early when the samples input is invalid.
    /// </summary>
    /// <param name="selectedSamples">The samples input value to test.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Theory]
    [InlineData("0")]
    [InlineData("25")]
    [InlineData("100")]
    [InlineData("249")]
    [InlineData("-250")]
    [InlineData("Invalid samples")]
    public async Task RunAsync_should_ReturnEarlyWhenSamplesInputIsInvalid(string selectedSamples)
    {
        // Arrange
        const string selectedController = "0";
        const string selectedJoystick = "L";
        List<IController> controllers = [_controllerMock.Object];
        _controllerDetectorMock.Setup(m => m.DetectAsync(It.IsAny<CancellationToken>())).ReturnsAsync(controllers);
        _stdIoMock.SetupSequence(m => m.In.ReadLine()).Returns(selectedController).Returns(selectedJoystick)
            .Returns(selectedSamples);
        SetupStdOutAndErr();

        // Act
        await RunAppAsync();

        // Assert
        VerifyUnhappyPathMocks(expectedReadLineCalls: 3);
    }

    /// <summary>
    /// Tests that <see cref="App.RunAsync"/> returns early when the expected Hz input is invalid.
    /// </summary>
    /// <param name="expectedHz">The expected Hz input value to test.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous unit test.</returns>
    [Theory]
    [InlineData("0")]
    [InlineData("-250")]
    [InlineData("Invalid samples")]
    public async Task RunAsync_should_ReturnEarlyWhenExpectedHzInputIsInvalid(string expectedHz)
    {
        // Arrange
        const string selectedController = "0";
        const string selectedJoystick = "L";
        const string selectedSamples = "2500";
        List<IController> controllers = [_controllerMock.Object];
        _controllerDetectorMock.Setup(m => m.DetectAsync(It.IsAny<CancellationToken>())).ReturnsAsync(controllers);
        _stdIoMock.SetupSequence(m => m.In.ReadLine()).Returns(selectedController).Returns(selectedJoystick)
            .Returns(selectedSamples).Returns(expectedHz);
        SetupStdOutAndErr();

        // Act
        await RunAppAsync();

        // Assert
        VerifyUnhappyPathMocks(expectedReadLineCalls: 4);
    }

    private async Task RunAppAsync()
    {
        using var cts = new CancellationTokenSource();
        await cts.CancelAsync();
        try
        {
            await _app.RunAsync(cts.Token);
        }
        catch (OperationCanceledException)
        {
        }
    }

    private void VerifyUnhappyPathMocks(int expectedReadLineCalls)
    {
        _controllerDetectorMock.Verify(m => m.DetectAsync(It.IsAny<CancellationToken>()), Times.Once);
        _stdIoMock.Verify(m => m.In.ReadLine(), Times.Exactly(expectedReadLineCalls));
        _controllerMock.Verify(m => m.GetTimestampCollector(It.IsAny<ControllerInput>()), Times.Never);
        _timestampCollectorMock.Verify(m => m.GetPacketMicrosecondsTimestamp(), Times.Never);
        _metricsCalculatorMock.Verify(m => m.Calculate(It.IsAny<long[]>(), It.IsAny<long>()), Times.Never);
    }

    private void SetupStdOutAndErr()
    {
        _stdIoMock.Setup(m => m.Out).Returns(Console.Out);
        _stdIoMock.Setup(m => m.Error).Returns(Console.Error);
    }
}