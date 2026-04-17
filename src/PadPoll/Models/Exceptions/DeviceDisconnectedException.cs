namespace PadPoll.Models.Exceptions;

/// <summary>
/// Exception thrown when a required device cannot be found or has been disconnected.
/// </summary>
public sealed class DeviceDisconnectedException()
    : Exception("The device could not be found, it could be disconnected.")
{
}