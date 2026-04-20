using PadPoll.Abstractions;

namespace PadPoll.Implementations;

/// <summary>
/// Default implementation of <see cref="IStandardIo"/> using <see cref="Console"/> for standard input, output, and error streams.
/// </summary>
internal sealed class DefaultStandardIo : IStandardIo
{
    /// <inheritdoc/>
    public TextReader In => Console.In;

    /// <inheritdoc/>
    public TextWriter Out => Console.Out;

    /// <inheritdoc/>
    public TextWriter Error => Console.Error;

    private const char SectionSeparatorChar = '=';

    /// <inheritdoc/>
    public void PrintSectionTitle(string title)
    {
        if (Console.WindowWidth <= 0)
        {
            return;
        }

        Span<char> line = stackalloc char[Console.WindowWidth];
        line.Fill(SectionSeparatorChar);

        int startPos = (Console.WindowWidth - title.Length) / 2;
        if (startPos > 1)
        {
            line[startPos - 1] = ' ';
            title.AsSpan().CopyTo(line.Slice(startPos, title.Length));
            line[startPos + title.Length] = ' ';
        }
        else
        {
            title.AsSpan()[..line.Length].CopyTo(line);
        }

        Console.ForegroundColor = ConsoleColor.Blue;
        Out.WriteLine();
        Out.WriteLine(line);
        Console.ResetColor();
    }
}