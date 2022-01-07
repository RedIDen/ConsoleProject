using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class RemoveCommandHandler : CommandHandlerBase
{
    protected override string CommandName { get; set; } = "remove";

    public RemoveCommandHandler(IFileCabinetService fileCabinetService)
    {
        this.fileCabinetService = fileCabinetService;
    }

    protected override void MakeWork(string parameters)
    {
        if (!int.TryParse(parameters, out int id))
        {
            Console.WriteLine("Wrong command syntax!");
            return;
        }

        int index = this.fileCabinetService.FindRecordIndexById(id);

        if (index == -1)
        {
            Console.WriteLine($"#{id} record is not found.");
            return;
        }

        this.fileCabinetService.RemoveRecord(index);

        Console.WriteLine($"Record #{id} is removed.");
    }
}
