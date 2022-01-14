using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class ListCommandHandler : ServiceCommandHandlerBase
{
    private Action<IEnumerable<FileCabinetRecord>, string> recordPrinter;

    public ListCommandHandler(FileCabinetServiceTransferHelper fileCabinetServiceTransferHelper, Action<IEnumerable<FileCabinetRecord>, string> recordPrinter)
        : base(fileCabinetServiceTransferHelper)
    {
        this.recordPrinter = recordPrinter;
    }

    protected override string CommandName { get; set; } = "list";

    protected override void MakeWork(string parameters)
    {
        ReadOnlyCollection<FileCabinetRecord> list = this.fileCabinetServiceTransferHelper.fileCabinetService.GetRecords();
        this.recordPrinter(list, "The list is empty.");
    }
}
