using System.Runtime.InteropServices;
using IGameInput = PadPoll.Implementations.Windows.GameInput.Interfaces.IGameInput;

namespace PadPoll.Implementations.Windows.GameInput;

/// <summary>
/// Provides access to the GameInput API and manages the global IGameInput instance.
/// </summary>
internal static partial class GameInputLib
{
    private static IGameInput? _gameInput;

    /// <summary>
    /// Gets the singleton instance of the <see cref="IGameInput"/> interface.
    /// </summary>
    /// <returns>The <see cref="IGameInput"/> instance.</returns>
    public static IGameInput GetInstance()
    {
        if (_gameInput is null)
        {
            _ = GameInputCreate(out _gameInput);
        }

        return _gameInput;
    }

    [LibraryImport("GameInput.dll")]
    private static partial int GameInputCreate(out IGameInput gameInput);
}