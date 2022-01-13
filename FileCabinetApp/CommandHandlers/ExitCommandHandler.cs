using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class ExitCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "exit";

    protected override void MakeWork(string parameters)
    {
        Console.WriteLine("Exitting the application...");
        Program.isRunning = false;
    }
}
