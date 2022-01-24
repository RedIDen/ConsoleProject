#pragma warning disable CS8618

namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// Tha command hadler base.
/// </summary>
internal abstract class CommandHandlerBase : ICommandHandler
{
    /// <summary>
    /// Wrong syntax error.
    /// </summary>
    public const string WrongSyntaxError = "Wrong command syntax!";

    /// <summary>
    /// The list of all commands added to the application.
    /// </summary>
    protected static readonly List<string> AllCommands = new List<string>();

    /// <summary>
    /// The next command hadler link.
    /// </summary>
    private ICommandHandler nextHandler;

    /// <summary>
    /// Initializes a new instance of the <see cref="CommandHandlerBase"/> class.
    /// </summary>
    public CommandHandlerBase()
    {
        foreach (var command in this.CommandNames)
        {
            CommandHandlerBase.AllCommands.Add(command);
        }
    }

    /// <summary>
    /// Gets the list of command names (only full or full and short).
    /// </summary>
    /// <value>
    /// The list of command names (strings).
    /// </value>
    protected abstract string[] CommandNames { get; }

    /// <summary>
    /// Handles the command request.
    /// </summary>
    /// <param name="appCommandRequest">The command request.</param>
    public void Handle(AppCommandRequest appCommandRequest)
    {
        foreach (var commandName in this.CommandNames)
        {
            if (string.Equals(appCommandRequest.Command, commandName, StringComparison.OrdinalIgnoreCase))
            {
                this.MakeWork(appCommandRequest.Parameters);
                return;
            }
        }

        if (this.nextHandler != null)
        {
            this.nextHandler.Handle(appCommandRequest);
        }
        else
        {
            Console.WriteLine($"There is no '{appCommandRequest.Command}' command.");
            Console.WriteLine();

            var similarCommands = new List<string>();

            foreach (var command in CommandHandlerBase.AllCommands)
            {
                if (command.Contains(appCommandRequest.Command, StringComparison.InvariantCultureIgnoreCase) ||
                    !command.ToCharArray().OrderBy(x => x).Except(appCommandRequest.Command.ToCharArray().OrderBy(x => x)).Any())
                {
                    similarCommands.Add(command);
                }
                else
                {
                    int intersects = command.ToCharArray().Intersect(appCommandRequest.Command.ToCharArray()).Count();
                    int delta = command.Length - intersects;

                    const int maxDelta = 2;

                    if (delta <= maxDelta && Math.Abs(command.Length - appCommandRequest.Command.Length) < maxDelta)
                    {
                        similarCommands.Add(command);
                    }
                }
            }

            if (similarCommands.Count == 0)
            {
                return;
            }

            const string oneSimilarCommandText = "The most similar command is";
            const string manySimilarCommandsText = "The most similar commands are";

            Console.WriteLine(similarCommands.Count == 1 ? oneSimilarCommandText : manySimilarCommandsText);

            foreach (var command in similarCommands)
            {
                Console.Write('\t');
                Console.WriteLine(command);
            }

            Console.WriteLine();
        }
    }

    /// <summary>
    /// Sets the next command handler.
    /// </summary>
    /// <param name="commandHandler">The next command handler.</param>
    public void SetNext(ICommandHandler commandHandler)
    {
        this.nextHandler = commandHandler;
    }

    /// <summary>
    /// The main method of the command handler. Called when the command request name is similar to the current command name.
    /// </summary>
    /// <param name="parameters">Command parameters.</param>
    protected abstract void MakeWork(string parameters);
}