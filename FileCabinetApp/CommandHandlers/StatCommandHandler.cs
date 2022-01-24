namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The stat command handler.
/// </summary>
internal class StatCommandHandler : ServiceCommandHandlerBase
{
    /// <summary>
    /// Initializes a new instance of the <see cref="StatCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    public StatCommandHandler(FileCabinetTrasferHelper service)
        : base(service)
    {
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "stat" };

    /// <summary>
    /// Shows the stats.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        (int records, int deleted) = this.transferHelper.Service.GetStat();
        Console.WriteLine($"{records} record(s).");
        Console.WriteLine($"{deleted} removed.");
    }
}