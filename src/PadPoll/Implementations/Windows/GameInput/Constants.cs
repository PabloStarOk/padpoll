namespace PadPoll.Implementations.Windows.GameInput;

/// <summary>
/// Provides constant values used by the GameInput implementation.
/// </summary>
internal static class Constants
{
    /// <summary>
    /// The Interface ID (IID) for IGameInput.
    /// </summary>
    public const string GameInputIid = "20EFC1C7-5D9A-43BA-B26F-B807FA48609C";

    /// <summary>
    /// The Interface ID (IID) for IGameInputDevice.
    /// </summary>
    public const string GameInputDeviceIid = "63E2F38B-A399-4275-8AE7-D4C6E524D12A";

    /// <summary>
    /// The Interface ID (IID) for IGameInputReading.
    /// </summary>
    public const string GameInputReadingIid = "C81C4CDE-ED1A-4631-A30F-C556A6241A1F";

    /// <summary>
    /// The size in bytes of the application-local device identifier.
    /// </summary>
    public const int AppLocalDeviceIdSize = 32;
}