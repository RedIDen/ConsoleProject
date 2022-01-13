using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class StatCommandHandler : ServiceCommandHandlerBase
{
    public StatCommandHandler(FileCabinetServiceTransferHelper fileCabinetServiceTransferHelper)
        : base(fileCabinetServiceTransferHelper)
    {
    }

    protected override string CommandName { get; set; } = "stat";

    protected override void MakeWork(string parameters)
    {
        (int records, int deleted) = this.fileCabinetServiceTransferHelper.fileCabinetService.GetStat();
        Console.WriteLine($"{records} record(s).");
        Console.WriteLine($"{deleted} removed.");
    }
}