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
        IFileCabinetService temp = this.transferHelper.Service is IServiceDecorator ? ((IServiceDecorator)this.transferHelper.Service).GetLast() : this.transferHelper.Service;

        if (temp is FileCabinetMemoryService)
        {
            Console.WriteLine("This command isn't allowed for the memory storage.");
            return;
        }

        (int deletedNum, int beforeNum) = this.transferHelper.Service.Purge();

        Console.WriteLine($"Data file processing is completed: {deletedNum} of {beforeNum} records were purged.");
    }
}