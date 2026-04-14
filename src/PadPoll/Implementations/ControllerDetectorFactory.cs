using System.Runtime.InteropServices;
using System.Runtime.InteropServices.Marshalling;
using PadPoll.Abstractions;
using PadPoll.Models.Exceptions;
#if LINUX
using PadPoll.Implementations.Linux;
#endif
#if WINDOWS
using PadPoll.Implementations.Windows;
using PadPoll.Implementations.Windows.GameInput;
using PadPoll.Implementations.Windows.GameInput.Interfaces;
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
#if WINDOWS
        if (OperatingSystem.IsWindows())
        {
            IGameInput gameInput;
            try
            {
                gameInput = GameInputLib.GetInstance();
            }
            catch (InvalidCastException)
            {
                throw new InitializationException("You must install GameInput library before using this app, you can do it by executing 'winget install Microsoft.GameInput' in your terminal.");
            }

            var comWrappers = new StrategyBasedComWrappers();
            return new WindowsControllerDetector(gameInput, comWrappers);
        }
#endif
        throw new PlatformNotSupportedException($"{RuntimeInformation.OSDescription} is not currently supported.");
    }
}