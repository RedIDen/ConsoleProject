using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class FindCommandHandler : ServiceCommandHandlerBase
{
    private Action<IEnumerable<FileCabinetRecord>, string> recordPrinter;

    public FindCommandHandler(FileCabinetTrasferHelper service, Action<IEnumerable<FileCabinetRecord>, string> recordPrinter)
        : base(service)
    {
        this.recordPrinter = recordPrinter;
    }

    protected override string[] CommandNames { get; } = { "find" };

    protected override void MakeWork(string parameters)
    {
        var parametersArray = parameters.Split(' ', 2);
        parametersArray[1] = parametersArray[1].Trim('\"');

        var foundRecords = parametersArray[0].ToLower() switch
        {
            "firstname" => this.service.Service.FindByFirstName(parametersArray[1]),
            "lastname" => this.service.Service.FindByLastName(parametersArray[1]),
            "dateofbirth" => this.service.Service.FindByDateOfBirth(parametersArray[1]),
            _ => new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>()),
        };

        this.recordPrinter(foundRecords, "No records found.");
    }
}
