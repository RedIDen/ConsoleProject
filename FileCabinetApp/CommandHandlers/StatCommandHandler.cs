using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class StatCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "stat";

    public StatCommandHandler(IFileCabinetService fileCabinetService)
    {
        this.fileCabinetService = fileCabinetService;
    }

    protected override void MakeWork(string parameters)
    {
        (int records, int deleted) = this.fileCabinetService.GetStat();
        Console.WriteLine($"{records} record(s).");
        Console.WriteLine($"{deleted} removed.");
    }
}