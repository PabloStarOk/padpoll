namespace PadPoll.Abstractions;

/// <summary>
/// Defines a contract for detecting controllers connected to the system.
/// </summary>
public interface IControllerDetector
{
    /// <summary>
    /// Asynchronously detects and returns a list of available controllers.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the operation.</param>
    /// <returns>A task that represents the asynchronous operation. The task result contains a read-only list of detected controllers.</returns>
    ValueTask<IReadOnlyList<IController>> DetectAsync(CancellationToken cancellationToken = default);
}