using System.Runtime.InteropServices;
using PadPoll.Abstractions;
#if LINUX
using PadPoll.Implementations.Linux;
#endif

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
#if LINUX
        if (OperatingSystem.IsLinux())
        {
            return new LinuxControllerDetector();
        }
#endif

        throw new PlatformNotSupportedException($"{RuntimeInformation.OSDescription} is not currently supported.");
    }
}