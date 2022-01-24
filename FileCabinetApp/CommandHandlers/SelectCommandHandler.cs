namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// Select command handler.
/// </summary>
internal class SelectCommandHandler : ServiceCommandHandlerBase
{
    private readonly Action<IEnumerable<FileCabinetRecord>, IEnumerable<string>> recordPrinter;

    /// <summary>
    /// Initializes a new instance of the <see cref="SelectCommandHandler"/> class.
    /// </summary>
    /// <param name="service">Transfer helper.</param>
    /// <param name="recordPrinter">Record printer delegate.</param>
    public SelectCommandHandler(FileCabinetTrasferHelper service, Action<IEnumerable<FileCabinetRecord>, IEnumerable<string>> recordPrinter)
        : base(service)
    {
        this.recordPrinter = recordPrinter;
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "select" };

    /// <summary>
    /// Selects and prints the records with specified data.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        var parametersAndPredicates = parameters.Split("where");

        char[] symbols = { ',', ' ', '(', ')', '\'', '\"', '=' };

        IEnumerable<FileCabinetRecord> list;

        if (parametersAndPredicates.Length == 2)
        {
            list = this.transferHelper.Service.Find(parametersAndPredicates[1]);
        }
        else
        {
            list = this.transferHelper.Service.GetRecords();
        }

        var parametersList = parametersAndPredicates[0].Split(symbols, StringSplitOptions.RemoveEmptyEntries).ToList();

        this.recordPrinter(list, parametersList);
    }
}
