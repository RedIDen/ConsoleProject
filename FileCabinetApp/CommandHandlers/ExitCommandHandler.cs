namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The exit command handler.
/// </summary>
internal class ExitCommandHandler : CommandHandlerBase
{
    /// <summary>
    /// Sets the program isRunning flag.
    /// </summary>
    private readonly Action<bool> setProgramIsRunning;

    /// <summary>
    /// Initializes a new instance of the <see cref="ExitCommandHandler"/> class.
    /// </summary>
    /// <param name="action">Delegate setting the program isRunning flag.</param>
    public ExitCommandHandler(Action<bool> action)
        : base()
    {
        this.setProgramIsRunning = action;
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected override string[] CommandNames { get; } = { "exit" };

    /// <summary>
    /// Exits the application.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected override void MakeWork(string parameters)
    {
        Console.WriteLine("Exitting the application...");
        this.setProgramIsRunning(false);
    }
}
