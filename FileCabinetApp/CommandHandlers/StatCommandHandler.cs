namespace FileCabinetApp.CommandHandlers;

public class StatCommandHandler : ServiceCommandHandlerBase
{
    public StatCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "stat" };

    protected override void MakeWork(string parameters)
    {
        (int records, int deleted) = this.transferHelper.Service.GetStat();
        Console.WriteLine($"{records} record(s).");
        Console.WriteLine($"{deleted} removed.");
    }
}