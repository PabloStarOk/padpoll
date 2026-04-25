namespace PadPoll.Implementations.Linux;

/// <summary>
/// Provides constants for Linux evdev (event device) input subsystem.
/// </summary>
internal static class EvDev
{
    /// <summary>
    /// Event types for evdev input events.
    /// </summary>
    public static class EventType
    {
        /// <summary>
        /// Synchronization event type.
        /// </summary>
        public const ushort EvSyn = 0;
    }

    /// <summary>
    /// Synchronization event codes.
    /// </summary>
    public static class SynCode
    {
        /// <summary>
        /// Report synchronization event code.
        /// </summary>
        public const ushort SynReport = 0;
    }

    /// <summary>
    /// Absolute axis codes for evdev input events.
    /// </summary>
    public static class AbsCodes
    {
        /// <summary>
        /// Absolute X axis.
        /// </summary>
        public const ushort AbsX = 0x0;

        /// <summary>
        /// Absolute Y axis.
        /// </summary>
        public const ushort AbsY = 0x1;

        /// <summary>
        /// Absolute RX axis (right stick X).
        /// </summary>
        public const ushort AbsRx = 0x3;

        /// <summary>
        /// Absolute RY axis (right stick Y).
        /// </summary>
        public const ushort AbsRy = 0x4;
    }
}