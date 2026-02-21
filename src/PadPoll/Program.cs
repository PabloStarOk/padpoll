using PadPoll;
using PadPoll.Abstractions;
using PadPoll.Implementations;

var console = new DefaultStandardIo();

try
{
    IControllerDetector controllerDetector = ControllerDetectorFactory.Create();
    var app = new App(console, controllerDetector);
    await app.RunAsync();
}
catch (PlatformNotSupportedException ex)
{
    console.Out.WriteLine(ex.Message);
}
