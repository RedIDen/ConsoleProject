using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class FindCommandHandler : ServiceCommandHandlerBase
{
    private IRecordPrinter recordPrinter;

    public FindCommandHandler(FileCabinetServiceTransferHelper fileCabinetServiceTransferHelper, IRecordPrinter recordPrinter)
        : base(fileCabinetServiceTransferHelper)
    {
        this.recordPrinter = recordPrinter;
    }

    protected override string CommandName { get; set; } = "find";

    protected override void MakeWork(string parameters)
    {
        var parametersArray = parameters.Split(' ', 2);
        parametersArray[1] = parametersArray[1].Trim('\"');

        var foundRecords = parametersArray[0].ToLower() switch
        {
            "firstname" => this.fileCabinetServiceTransferHelper.fileCabinetService.FindByFirstName(parametersArray[1]),
            "lastname" => this.fileCabinetServiceTransferHelper.fileCabinetService.FindByLastName(parametersArray[1]),
            "dateofbirth" => this.fileCabinetServiceTransferHelper.fileCabinetService.FindByDateOfBirth(parametersArray[1]),
            _ => new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>()),
        };

        this.recordPrinter.Print(foundRecords, "No records found.");
    }
}
