namespace PadPoll.Models.Exceptions;

/// <summary>
/// Exception thrown when the initialization process of a component fails.
/// </summary>
/// <param name="message">The message that describes the error.</param>
internal sealed class InitializationException(string message)
    : Exception(message)
{
}