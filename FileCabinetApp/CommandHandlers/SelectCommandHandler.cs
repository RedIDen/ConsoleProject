using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class SelectCommandHandler : ServiceCommandHandlerBase
{
    private Action<IEnumerable<FileCabinetRecord>, IEnumerable<string>> recordPrinter;

    public SelectCommandHandler(FileCabinetTrasferHelper service, Action<IEnumerable<FileCabinetRecord>, IEnumerable<string>> recordPrinter)
        : base(service)
    {
        this.recordPrinter = recordPrinter;
    }

    protected override string[] CommandNames { get; } = { "select" };

    protected override void MakeWork(string parameters)
    {
        var parametersAndPredicates = parameters.Split("where");

        char[] symbols = { ',', ' ', '(', ')', '\'', '\"', '=' };

        IEnumerable<FileCabinetRecord> list;

        if (parametersAndPredicates.Length == 2)
        {
            list = this.service.Service.Find(parametersAndPredicates[1]);
        }
        else
        {
            list = this.service.Service.GetRecords();
        }

        var parametersList = parametersAndPredicates[0].Split(symbols, StringSplitOptions.RemoveEmptyEntries).ToList();

        this.recordPrinter(list, parametersList);
    }
}
