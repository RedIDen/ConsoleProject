using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public class PurgeCommandHandler : ServiceCommandHandlerBase
{
    public PurgeCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "purge" };

    protected override void MakeWork(string parameters)
    {
        IFileCabinetService temp = this.service.Service is IServiceDecorator ? ((IServiceDecorator)this.service.Service).GetLast() : this.service.Service;

        if (temp is FileCabinetMemoryService)
        {
            Console.WriteLine("This command isn't allowed for the memory storage.");
            return;
        }

        (int deletedNum, int beforeNum) = this.service.Service.Purge();

        Console.WriteLine($"Data file processing is completed: {deletedNum} of {beforeNum} records were purged.");
    }
}