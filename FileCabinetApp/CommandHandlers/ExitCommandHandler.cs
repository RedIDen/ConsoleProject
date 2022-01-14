using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class ExitCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "exit";

    private Action<bool> setProgramIsRunning;

    public ExitCommandHandler(Action<bool> action)
    {
        this.setProgramIsRunning = action;
    }

    protected override void MakeWork(string parameters)
    {
        Console.WriteLine("Exitting the application...");
        this.setProgramIsRunning(false);
    }
}
