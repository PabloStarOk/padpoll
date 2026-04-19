namespace PadPoll.Models.Exceptions;

/// <summary>
/// Exception thrown when a required device cannot be found or has been disconnected.
/// </summary>
public sealed class DeviceException(string message)
    : Exception(message)
{
}