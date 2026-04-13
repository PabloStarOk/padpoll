namespace PadPoll.Implementations.Windows.GameInput.Enums;

/// <summary>
/// Specifies the focus policy for GameInput, determining how input and system button events are handled based on window focus.
/// </summary>
[Flags]
internal enum GameInputFocusPolicy : uint
{
    /// <summary>
    /// The default focus policy.
    /// </summary>
    Default = 0x00000000,

    /// <summary>
    /// Only the foreground application receives input.
    /// </summary>
    ExclusiveForegroundInput = 0x00000002,

    /// <summary>
    /// Only the foreground application receives Guide button events.
    /// </summary>
    ExclusiveForegroundGuideButton = 0x00000008,

    /// <summary>
    /// Only the foreground application receives Share button events.
    /// </summary>
    ExclusiveForegroundShareButton = 0x00000020,

    /// <summary>
    /// The application receives input even when in the background.
    /// </summary>
    EnableBackgroundInput = 0x00000040,

    /// <summary>
    /// The application receives Guide button events even when in the background.
    /// </summary>
    EnableBackgroundGuideButton = 0x00000080,

    /// <summary>
    /// The application receives Share button events even when in the background.
    /// </summary>
    EnableBackgroundShareButton = 0x00000100,
}