using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class ListCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "list";

    public ListCommandHandler(IFileCabinetService fileCabinetService)
    {
        this.fileCabinetService = fileCabinetService;
    }

    protected override void MakeWork(string parameters)
    {
        ReadOnlyCollection<FileCabinetRecord> list = this.fileCabinetService.GetRecords();
        this.ShowRecords(list, "The list is empty.");
    }
}
