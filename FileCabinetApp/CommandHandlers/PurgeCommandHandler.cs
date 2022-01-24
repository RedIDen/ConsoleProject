namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The purge command handler.
/// </summary>
internal class PurgeCommandHandler : ServiceCommandHandlerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="PurgeCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    public PurgeCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "purge" };

    /// <summary>
    /// Purges the records with isDeleted flag (for filesystem service).
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        IFileCabinetService temp = this.transferHelper.Service is IServiceDecorator decorator ? decorator.GetLast() : this.transferHelper.Service;

        if (temp is FileCabinetMemoryService)
        {
            Console.WriteLine("This command isn't allowed for the memory storage.");
            return;
        }

        (int deletedNum, int beforeNum) = this.transferHelper.Service.Purge();

        Console.WriteLine($"Data file processing is completed: {deletedNum} of {beforeNum} records were purged.");
    }
}