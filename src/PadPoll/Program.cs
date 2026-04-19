using PadPoll;
using PadPoll.Abstractions;
using PadPoll.Implementations;
using PadPoll.Models.Exceptions;

using var cts = new CancellationTokenSource();
Console.CancelKeyPress += Cancel;
var console = new DefaultStandardIo();

try
{
    IControllerDetector controllerDetector = ControllerDetectorFactory.Create();
    var app = new App(console, controllerDetector, new MetricsCalculator());
    await app.RunAsync(cts.Token);
}
catch (OperationCanceledException)
{
    // Ignore
}
catch (Exception ex) when (ex is InitializationException or DeviceException or PlatformNotSupportedException)
{
    console.PrintSectionTitle("Error");
    console.Error.WriteLine(ex.Message);
}

return;

void Cancel(object? sender, ConsoleCancelEventArgs args)
{
    args.Cancel = true;
    cts.Cancel();
    Console.CancelKeyPress -= Cancel;
}
