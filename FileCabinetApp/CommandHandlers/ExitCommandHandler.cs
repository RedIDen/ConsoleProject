using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class ExitCommandHandler : CommandHandlerBase
{
    protected override string[] CommandNames { get; } = { "exit" };

    private Action<bool> setProgramIsRunning;

    public ExitCommandHandler(Action<bool> action)
        : base()
    {
        this.setProgramIsRunning = action;
    }

    protected override void MakeWork(string parameters)
    {
        Console.WriteLine("Exitting the application...");
        this.setProgramIsRunning(false);
    }
}
