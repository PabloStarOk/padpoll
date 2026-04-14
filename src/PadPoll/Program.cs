using PadPoll;
using PadPoll.Abstractions;
using PadPoll.Implementations;
using PadPoll.Models.Exceptions;

var console = new DefaultStandardIo();

try
{
    IControllerDetector controllerDetector = ControllerDetectorFactory.Create();
    var app = new App(console, controllerDetector, new MetricsCalculator());
    await app.RunAsync();
}
catch (InitializationException ex)
{
    console.Error.WriteLine(ex.Message);
}
catch (PlatformNotSupportedException ex)
{
    console.Error.WriteLine(ex.Message);
}
