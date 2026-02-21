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

    private readonly IStandardIo _stdIo;
    private readonly IControllerDetector _controllerDetector;

    /// <summary>
    /// Initializes a new instance of the <see cref="App"/> class.
    /// </summary>
    /// <param name="stdIo">Standard input/output abstraction.</param>
    /// <param name="controllerDetector">Controller detection service.</param>
    public App(IStandardIo stdIo, IControllerDetector controllerDetector)
    {
        _stdIo = stdIo;
        _controllerDetector = controllerDetector;
    }

    /// <summary>
    /// Runs the main application workflow asynchronously.
    /// </summary>
    /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
    public async Task RunAsync()
    {
        var assembly = Assembly.GetExecutingAssembly();
        var versionAttr = assembly.GetCustomAttribute<AssemblyInformationalVersionAttribute>();
        _stdIo.PrintSectionTitle($"{assembly.GetName().Name} {versionAttr?.InformationalVersion}");

        IReadOnlyList<IController> controllers = await _controllerDetector.DetectAsync();
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

        _stdIo.PrintSectionTitle("Selected Controller");
        _stdIo.Out.WriteLine($"Selected controller: {selectedController.DisplayName}");
        _stdIo.Out.WriteLine($"Selected joystick: {selectedJoystick}");
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
        string? number = _stdIo.In.ReadLine();

        if (number is not null && int.TryParse(number, out int selectedIndex) && selectedIndex >= 0
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
}