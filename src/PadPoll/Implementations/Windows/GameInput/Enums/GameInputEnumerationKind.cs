namespace PadPoll.Implementations.Windows.GameInput.Enums;

/// <summary>
/// Specifies the kind of enumeration to use for game input operations.
/// </summary>
internal enum GameInputEnumerationKind : uint
{
    /// <summary>
    /// No enumeration kind specified.
    /// </summary>
    None = 0,

    /// <summary>
    /// Asynchronous enumeration.
    /// </summary>
    Async = 1,

    /// <summary>
    /// Blocking enumeration.
    /// </summary>
    Blocking = 2,
}