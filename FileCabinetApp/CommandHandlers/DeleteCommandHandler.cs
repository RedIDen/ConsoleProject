namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The delete command handler.
/// </summary>
internal class DeleteCommandHandler : ServiceCommandHandlerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="DeleteCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    public DeleteCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "delete" };

    /// <summary>
    /// Delete records with current parameters.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        var list = this.transferHelper.Service.Find(parameters.Replace("where", string.Empty, StringComparison.InvariantCultureIgnoreCase));
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
