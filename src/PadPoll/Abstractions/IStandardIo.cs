namespace PadPoll.Abstractions;

/// <summary>
/// Represents an abstraction for standard input, output, and error streams.
/// </summary>
public interface IStandardIo
{
    /// <summary>
    /// Gets the standard input stream.
    /// </summary>
    TextReader In { get; }

    /// <summary>
    /// Gets the standard output stream.
    /// </summary>
    TextWriter Out { get; }

    /// <summary>
    /// Gets the standard error stream.
    /// </summary>
    TextWriter Error { get; }

    /// <summary>
    /// Prints a section title to the output.
    /// </summary>
    /// <param name="title">The title to print.</param>
    void PrintSectionTitle(string title);
}