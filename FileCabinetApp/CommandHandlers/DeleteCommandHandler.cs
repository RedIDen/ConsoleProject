using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class DeleteCommandHandler : ServiceCommandHandlerBase
{
    public DeleteCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "delete" };

    protected override void MakeWork(string parameters)
    {
        var list = this.service.Service.Find(parameters.Replace("where", string.Empty));
        var ids = new List<int>();

        int count = 0;
        foreach (var foundRecord in list)
        {
            ids.Add(foundRecord.Id);
            count++;
            this.service.Service.RemoveRecord(this.service.Service.FindRecordIndexById(foundRecord.Id));
        }

        if (count == 0)
        {
            Console.WriteLine("No records found.");
        }
        else if (count == 1)
        {
            Console.WriteLine($"Record #{ids[0]} is deleted.");
        }
        else
        {
            Console.Write("Records ");
            foreach (var id in ids)
            {
                Console.Write('#');
                Console.Write(id);
                Console.Write(' ');
            }

            Console.WriteLine("are deleted.");
        }
    }
}
