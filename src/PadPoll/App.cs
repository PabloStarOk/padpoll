using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using PadPoll.Abstractions;
using PadPoll.Models;

namespace PadPoll;

/// <summary>
/// Main application logic for PadPoll.
/// </summary>
public sealed class App
{
    private const string LeftJoystick = "l";
    private const string RightJoystick = "r";
    private const int StabilizationSamples = 20;
    private const int MinimumSamples = 250;

    private readonly IStandardIo _stdIo;
    private readonly IControllerDetector _controllerDetector;
    private readonly IMetricsCalculator _metricsCalculator;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    /// <param name="stdIo">Standard input/output abstraction.</param>
    /// <param name="controllerDetector">Controller detection service.</param>
    /// <param name="metricsCalculator">Metrics calculation service.</param>
    public App(IStandardIo stdIo, IControllerDetector controllerDetector, IMetricsCalculator metricsCalculator)
    {
        _stdIo = stdIo;
        _controllerDetector = controllerDetector;
        _metricsCalculator = metricsCalculator;
    }

    /// <summary>
    /// Runs the main application workflow asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A token to monitor for cancellation requests.</param>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RunAsync(CancellationToken cancellationToken)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var versionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        _stdIo.PrintSectionTitle($"{assembly.GetName().Name} {versionAttr?.InformationalVersion}");

        IReadOnlyList<IController> controllers = await _controllerDetector.DetectAsync(cancellationToken);
        if (controllers.Count is 0)
        {
            _stdIo.Out.WriteLine("Controllers not found");
            return;
        }

        if (!TrySelectController(controllers, out IController? selectedController)
            || !TrySelectJoystick(out ControllerInput? selectedJoystick))
        {
            return;
        }

        if (!TrySelectSamplesNumber(out uint requiredSamples) || !TrySelectExpectedHz(out decimal expectedHz))
        {
            return;
        }

        _stdIo.PrintSectionTitle("Test Conditions");
        _stdIo.Out.WriteLine($"Controller: {selectedController.DisplayName}");
        _stdIo.Out.WriteLine($"Joystick: {selectedJoystick}");
        _stdIo.Out.WriteLine($"Samples to Collect: {requiredSamples}");
        long[] timestamps = CollectTimestamps(selectedController, (ControllerInput)selectedJoystick, requiredSamples);
        Metrics metrics = _metricsCalculator.Calculate(timestamps, expectedHz);
        DisplayMetrics(metrics);
        await Task.Delay(Timeout.InfiniteTimeSpan, cancellationToken);
    }

    private bool TrySelectController(
        IReadOnlyList<IController> controllers,
        [NotNullWhen(true)] out IController? controller)
    {
        controller = null;

        _stdIo.Out.WriteLine($"Found {controllers.Count} controllers");
        for (int i = 0; i < controllers.Count; i++)
        {
            _stdIo.Out.WriteLine($"#[{i}]: {controllers[i].DisplayName}");
        }

        _stdIo.Out.Write("Type the number of the controller to test: ");
        ReadOnlySpan<char> number = _stdIo.In.ReadLine().AsSpan();
        if (!number.IsEmpty && !number.IsWhiteSpace()
            && int.TryParse(number, out int selectedIndex) && selectedIndex >= 0
            && selectedIndex < controllers.Count)
        {
            controller = controllers[selectedIndex];
            return true;
        }

        _stdIo.Error.WriteLine($"Invalid controller number: '{number}'");
        return false;
    }

    private bool TrySelectJoystick([NotNullWhen(true)] out ControllerInput? joystick)
    {
        joystick = null;
        _stdIo.Out.Write("Type 'L' (Left) or 'R' (Right) to select the joystick to test: ");
        string? selectedJoystick = _stdIo.In.ReadLine();
        switch (selectedJoystick?.ToLower())
        {
            case LeftJoystick:
                joystick = ControllerInput.LeftJoystick;
                break;

            case RightJoystick:
                joystick = ControllerInput.RightJoystick;
                break;

            default:
                _stdIo.Error.WriteLine($"Invalid joystick: '{selectedJoystick}'");
                return false;
        }

        return true;
    }

    private bool TrySelectSamplesNumber(out uint samples)
    {
        _stdIo.Out.Write($"Type the number of samples to collect (Min. {MinimumSamples}  - Max. {uint.MaxValue}): ");
        ReadOnlySpan<char> input = _stdIo.In.ReadLine().AsSpan();
        if (!uint.TryParse(input, out samples))
        {
            _stdIo.Error.WriteLine($"Invalid samples number: '{input}'");
            return false;
        }

        if (samples < MinimumSamples)
        {
            _stdIo.Error.WriteLine($"Samples number must be at least {MinimumSamples}");
            return false;
        }

        return true;
    }

    private bool TrySelectExpectedHz(out decimal expectedHz)
    {
        expectedHz = -1;
        _stdIo.Out.Write("(Optional) Type the expected Hz for the selected controller or leave empty if unknown: ");
        ReadOnlySpan<char> input = _stdIo.In.ReadLine()?.Trim();
        if (input.EndsWith("Hz", StringComparison.OrdinalIgnoreCase))
        {
            input = input[..^2].Trim();
        }

        if (input.IsEmpty || input.IsWhiteSpace() || decimal.TryParse(input, out expectedHz))
        {
            return true;
        }

        _stdIo.Error.WriteLine($"Invalid number: '{input}'");
        return false;
    }

    private long[] CollectTimestamps(IController controller, ControllerInput monitoredInputs, uint requiredSamples)
    {
        _stdIo.PrintSectionTitle("Benchmarking");
        _stdIo.Out.WriteLine("Keep moving the selected joystick until the test completes");
        _stdIo.Out.WriteLine();
        _stdIo.Out.WriteLine("Phase 1/2 - Buffer Flush");
        using IInputTimestampCollector timestampCollector = controller.GetTimestampCollector(monitoredInputs);
        _stdIo.Out.Write("\rProgress: [0.0 %]");
        for (int i = 0; i < StabilizationSamples; i++)
        {
            _ = timestampCollector.GetPacketMicrosecondsTimestamp();
            _stdIo.Out.Write($"\rProgress: [{(decimal)(i + 1) / StabilizationSamples:P}]");
        }

        _stdIo.Out.WriteLine("\nCompleted\n");
        _stdIo.Out.WriteLine("Phase 2/2 - Data Capture");
        _stdIo.Out.Write("\rProgress: [0.0 %]");

        long[] timestamps = new long[requiredSamples];
        int reportInterval = (int)(requiredSamples / 100) + 1;
        for (int i = 0; i < requiredSamples; i++)
        {
            timestamps[i] = timestampCollector.GetPacketMicrosecondsTimestamp();
            if (i % reportInterval == 0)
            {
                _stdIo.Out.Write($"\rProgress: [{(decimal)(i + 1) / requiredSamples:P}]");
            }
        }

        _stdIo.Out.WriteLine("\nCompleted");
        return timestamps;
    }

    private void DisplayMetrics(Metrics metrics)
    {
        _stdIo.PrintSectionTitle("Metrics");
        _stdIo.Out.WriteLine();
        _stdIo.Out.WriteLine($"Duration: {metrics.TestDuration:g}");
        _stdIo.Out.WriteLine($"Collected Samples: {metrics.CollectedSamples}");

        _stdIo.Out.WriteLine();
        _stdIo.Out.WriteLine("Polling Rate");
        _stdIo.Out.WriteLine($"\tAverage: {metrics.PollingRate.AverageHz:F2} hz (sustained)");
        _stdIo.Out.WriteLine($"\tPeak: {metrics.PollingRate.PeakHz:F2} hz (instantaneous)");
        if (metrics.PollingRate.Accuracy is not null)
        {
            _stdIo.Out.WriteLine($"\tAccuracy: {metrics.PollingRate.Accuracy:P}");
        }

        if (metrics.PollingRate.Consistency is not null)
        {
            _stdIo.Out.WriteLine($"\tConsistency: {metrics.PollingRate.Consistency:P}");
        }

        _stdIo.Out.WriteLine();
        _stdIo.Out.WriteLine("Latency");
        _stdIo.Out.WriteLine($"\tAverage: {metrics.Latency.AverageMs:F2} ms");
        _stdIo.Out.WriteLine($"\tMinimum: {metrics.Latency.MinMs:F2} ms");
        _stdIo.Out.WriteLine($"\tMaximum: {metrics.Latency.MaxMs:F2} ms");
        _stdIo.Out.WriteLine($"\t1%: {metrics.Latency.OnePercentLowMs:F2} ms");
        _stdIo.Out.WriteLine($"\tJitter: {metrics.Latency.JitterMs:F2} ms");
    }
}