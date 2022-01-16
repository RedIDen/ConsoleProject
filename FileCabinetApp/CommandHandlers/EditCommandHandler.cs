using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class EditCommandHandler : ServiceCommandHandlerBase
{
    public EditCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string CommandName { get; set; } = "edit";

    protected override void MakeWork(string parameters)
    {
        if (!int.TryParse(parameters, out int id))
        {
            Console.WriteLine("Wrong command syntax!");
            return;
        }

        int index = this.service.Service.FindRecordIndexById(id);

        if (index == -1)
        {
            Console.WriteLine($"#{id} record is not found.");
            return;
        }

        var record = this.ReadDataForRecord();
        record.Id = id;

        this.service.Service.EditRecord(record, index);

        Console.WriteLine($"Record #{id} is edited.");
    }
}
