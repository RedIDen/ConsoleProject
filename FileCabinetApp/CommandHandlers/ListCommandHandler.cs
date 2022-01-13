using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class ListCommandHandler : ServiceCommandHandlerBase
{
    public ListCommandHandler(FileCabinetServiceTransferHelper fileCabinetServiceTransferHelper)
        : base(fileCabinetServiceTransferHelper)
    {
    }

    protected override string CommandName { get; set; } = "list";

    protected override void MakeWork(string parameters)
    {
        ReadOnlyCollection<FileCabinetRecord> list = this.fileCabinetServiceTransferHelper.fileCabinetService.GetRecords();
        this.ShowRecords(list, "The list is empty.");
    }
}
