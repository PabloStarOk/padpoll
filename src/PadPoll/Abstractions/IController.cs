using PadPoll.Models;

namespace PadPoll.Abstractions;

/// <summary>
/// Represents a controller abstraction with a display name and the ability to provide a timestamp collector
/// for monitored controller inputs.
/// </summary>
public interface IController
{
    /// <summary>
    /// Gets the display name of the controller.
    /// </summary>
    string DisplayName { get; }

    /// <summary>
    /// Returns an <see cref="IInputTimestampCollector"/> for the specified monitored controller inputs.
    /// </summary>
    /// <param name="monitoredInputs">The <see cref="ControllerInput"/> to monitor.</param>
    /// <returns>An instance of <see cref="IInputTimestampCollector"/>.</returns>
    IInputTimestampCollector GetTimestampCollector(ControllerInput monitoredInputs);
}