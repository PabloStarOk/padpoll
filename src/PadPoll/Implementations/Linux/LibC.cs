using System.Runtime.InteropServices;

namespace PadPoll.Implementations.Linux;

/// <summary>
/// Provides P/Invoke signatures for selected Linux libc functions.
/// </summary>
internal static partial class LibC
{
    private const string LibCName = "libc";

    /// <summary>
    /// Reads up to <paramref name="size"/> bytes from the file descriptor <paramref name="fd"/> into the buffer <paramref name="buf"/>.
    /// </summary>
    /// <param name="fd">File descriptor to read from.</param>
    /// <param name="buf">Pointer to the buffer to store the read data.</param>
    /// <param name="size">Maximum number of bytes to read.</param>
    /// <returns>The number of bytes read, or -1 on error.</returns>
    [LibraryImport(LibCName, SetLastError = true)]
    public static unsafe partial int read(int fd, void* buf, int size);

    /// <summary>
    /// Performs device-specific input/output operations on the file descriptor <paramref name="fd"/>.
    /// </summary>
    /// <param name="fd">File descriptor on which to perform the operation.</param>
    /// <param name="cmd">Device-dependent request code.</param>
    /// <param name="arg">Reference to an argument for the operation (varies by request code).</param>
    /// <returns>Result of the operation, or -1 on error.</returns>
    [LibraryImport(LibCName, SetLastError = true)]
    public static partial int ioctl(int fd, int cmd, ref int arg);
}