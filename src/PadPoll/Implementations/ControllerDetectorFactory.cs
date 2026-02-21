using System.Runtime.InteropServices;
using PadPoll.Abstractions;
using PadPoll.Implementations.Linux;

namespace PadPoll.Implementations;

/// <summary>
/// Factory class for creating platform-specific <see cref="IControllerDetector"/> instances.
/// </summary>
public static class ControllerDetectorFactory
{
    /// <summary>
    /// Creates a platform-specific <see cref="IControllerDetector"/> instance.
    /// </summary>
    /// <returns>An instance of <see cref="IControllerDetector"/> for the current platform.</returns>
    public static IControllerDetector Create()
    {
        if (OperatingSystem.IsLinux())
        {
            return new LinuxControllerDetector();
        }

        throw new PlatformNotSupportedException($"{RuntimeInformation.OSDescription} is not currently supported.");
    }
}