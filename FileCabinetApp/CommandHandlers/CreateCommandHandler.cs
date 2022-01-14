using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class CreateCommandHandler : ServiceCommandHandlerBase
{
    public CreateCommandHandler(FileCabinetServiceTransferHelper fileCabinetServiceTransferHelper)
        : base(fileCabinetServiceTransferHelper)
    {
    }

    protected override string CommandName { get; set; } = "create";

    protected override void MakeWork(string parameters)
    {
        int id = this.fileCabinetServiceTransferHelper.fileCabinetService.CreateRecord(this.ReadDataForRecord());

        Console.WriteLine($"Record #{id} is created.");
    }
}
