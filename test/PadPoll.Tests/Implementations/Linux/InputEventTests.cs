using PadPoll.Implementations.Linux;

namespace PadPoll.Tests.Implementations.Linux;

/// <summary>
/// Unit tests for the <see cref="InputEvent"/> class.
/// </summary>
[Trait("Category", "Unit")]
public sealed class InputEventTests
{
    /// <summary>
    /// Tests that <see cref="InputEvent.GetMicrosecondsTimestamp"/> returns the expected microseconds value.
    /// </summary>
    [Fact]
    public void GetMicrosecondsTimestamp_should_ReturnExpectedMicroseconds()
    {
        // Arrange
        const long expected = 201_000_000L;
        var inputEvent = new InputEvent { TimeSeconds = 200L, TimeMicroseconds = 1_000_000L };

        // Act
        long actual = inputEvent.GetMicrosecondsTimestamp();

        // Assert
        Assert.Equal(expected, actual);
    }
}