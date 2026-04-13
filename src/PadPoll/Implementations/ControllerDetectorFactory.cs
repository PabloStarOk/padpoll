using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using PadPoll.Abstractions;
#if LINUX
using PadPoll.Implementations.Linux;
#endif
#if WINDOWS
using PadPoll.Implementations.Windows;
using PadPoll.Implementations.Windows.GameInput;
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
    /// <param name="stdIo">The standard input/output interface.</param>
    /// <returns>An instance of <see cref="IControllerDetector"/> for the current platform.</returns>
    public static IControllerDetector Create(IStandardIo stdIo)
    {
#if LINUX
        if (OperatingSystem.IsLinux())
        {
            return new LinuxControllerDetector();
        }
#endif
#if WINDOWS
        if (OperatingSystem.IsWindows())
        {
            try
            {
                var comWrappers = new StrategyBasedComWrappers();
                return new WindowsControllerDetector(GameInputLib.GetInstance(), comWrappers);
            }
            catch (InvalidCastException)
            {
                stdIo.Error.WriteLine("You must install GameInput before using this app, you can do it by executing 'winget install Microsoft.GameInput' in your terminal.");
            }
        }
#endif
        throw new PlatformNotSupportedException($"{RuntimeInformation.OSDescription} is not currently supported.");
    }
}