using System.Globalization;
using EvDevSharp;
using PadPoll.Abstractions;

namespace PadPoll.Implementations.Linux;

/// <summary>
/// Detects connected controller-like devices on Linux systems by parsing the `/proc/bus/input/devices` file.
/// TODO: Use libudev and sd-device libraries instead of parsing the string file.
/// </summary>
internal sealed class LinuxControllerDetector : IControllerDetector
{
    private const string DeviceListFilePath = "/proc/bus/input/devices";
    private const string DeviceInputDirPath = "/dev/input/";
    private const string BlocksSeparator = "\n\n";
    private const string NameLinePrefix = "N: Name=";
    private const string HandlersLinePrefix = "H: Handlers=";
    private const string BitmapLinePrefix = "B: ";
    private const string EventLinePrefix = "EV";
    private const string EventFilePrefix = "event";
    private const string UnknownDeviceName = "Unknown Device Name";
    private static readonly EvDevEventType[] EvDevEventTypes = Enum.GetValues<EvDevEventType>();

    /// <inheritdoc/>
    public async ValueTask<IReadOnlyList<IController>> DetectAsync(CancellationToken cancellationToken = default)
    {
        if (!File.Exists(DeviceListFilePath))
        {
            throw new FileNotFoundException("Device list file not found", DeviceListFilePath);
        }

        string deviceList = await File.ReadAllTextAsync(DeviceListFilePath, cancellationToken);
        ReadOnlySpan<char> deviceListSpan = deviceList.AsSpan();
        List<LinuxController> controllers = [];
        using MemoryExtensions.SpanSplitEnumerator<char> blocksEnumerator = deviceListSpan.Split(BlocksSeparator);
        foreach (Range blockRange in blocksEnumerator)
        {
            ReadOnlySpan<char> displayName = UnknownDeviceName;
            ReadOnlySpan<char> evDevHandler = [];
            bool supportAbsAndKeyEvents = false;
            ReadOnlySpan<char> devBlock = deviceListSpan[blockRange];
            foreach (Range lineRange in devBlock.Split('\n'))
            {
                ReadOnlySpan<char> line = devBlock[lineRange];
                if (line.StartsWith(NameLinePrefix))
                {
                    displayName = line[NameLinePrefix.Length..].Trim('"');
                    continue;
                }

                if (line.StartsWith(HandlersLinePrefix))
                {
                    evDevHandler = ParseEvHandlerFileName(line);
                    continue;
                }

                if (!line.StartsWith(BitmapLinePrefix) || !line[BitmapLinePrefix.Length..].StartsWith(EventLinePrefix))
                {
                    continue;
                }

                supportAbsAndKeyEvents = SupportAbsAndKeyEvents(line);
            }

            if (!supportAbsAndKeyEvents)
            {
                continue;
            }

            string evDevFilePath = Path.Combine(DeviceInputDirPath, evDevHandler.ToString());
            controllers.Add(LinuxController.Create(displayName.ToString(), evDevFilePath));
        }

        return controllers;
    }

    private static ReadOnlySpan<char> ParseEvHandlerFileName(ReadOnlySpan<char> handlerLine)
    {
        ReadOnlySpan<char> handlers = handlerLine[HandlersLinePrefix.Length..];
        using MemoryExtensions.SpanSplitEnumerator<char> enumerator = handlers.Split(' ');
        foreach (Range range in enumerator)
        {
            ReadOnlySpan<char> slice = handlers[range];
            if (slice.StartsWith(EventFilePrefix))
            {
                return slice;
            }
        }

        return [];
    }

    private static bool SupportAbsAndKeyEvents(ReadOnlySpan<char> evLine)
    {
        using MemoryExtensions.SpanSplitEnumerator<char> enumerator = evLine.Split('=');
        enumerator.MoveNext();
        enumerator.MoveNext();
        ReadOnlySpan<char> hexValue = evLine[enumerator.Current];
        if (!ulong.TryParse(hexValue, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out ulong value))
        {
            return false;
        }

        bool supportAbsEvents = false;
        bool supportKeyEvents = false;
        foreach (EvDevEventType ev in EvDevEventTypes)
        {
            if ((value & (1UL << (int)ev)) == 0)
            {
                continue;
            }

            if (ev is EvDevEventType.EV_ABS)
            {
                supportAbsEvents = true;
            }

            if (ev is EvDevEventType.EV_KEY)
            {
                supportKeyEvents = true;
            }

            if (supportAbsEvents && supportKeyEvents)
            {
                return true;
            }
        }

        return false;
    }
}
