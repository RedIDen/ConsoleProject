namespace FileCabinetApp.CommandHandlers;
internal class DeleteCommandHandler : ServiceCommandHandlerBase
{
    public DeleteCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    protected override string[] CommandNames { get; } = { "delete" };

    protected override void MakeWork(string parameters)
    {
        var list = this.transferHelper.Service.Find(parameters.Replace("where", string.Empty));
        var ids = new List<int>();

        int count = 0;
        foreach (var foundRecord in list)
        {
            ids.Add(foundRecord.Id);
            count++;
            this.transferHelper.Service.RemoveRecord(this.transferHelper.Service.FindRecordIndexById(foundRecord.Id));
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
                Console.Write($"#{id} ");
            }

            Console.WriteLine("are deleted.");
        }
    }
}
