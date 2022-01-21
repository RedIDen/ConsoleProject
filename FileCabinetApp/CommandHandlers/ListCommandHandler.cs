using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class ListCommandHandler : ServiceCommandHandlerBase
{
    private Action<IEnumerable<FileCabinetRecord>, string> recordPrinter;

    public ListCommandHandler(FileCabinetTrasferHelper service, Action<IEnumerable<FileCabinetRecord>, string> recordPrinter)
        : base(service)
    {
        this.recordPrinter = recordPrinter;
    }

    protected override string[] CommandNames { get; } = { "list" };

    protected override void MakeWork(string parameters)
    {
        IEnumerable<FileCabinetRecord> list = this.service.Service.GetRecords();
        this.recordPrinter(list, "The list is empty.");
    }
}
