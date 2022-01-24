namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The command request class.
/// </summary>
internal class AppCommandRequest
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppCommandRequest"/> class.
    /// </summary>
    /// <param name="command">Command name.</param>
    /// <param name="parameters">Command parameters.</param>
    public AppCommandRequest(string command, string parameters)
    {
        this.Command = command;
        this.Parameters = parameters;
    }

    /// <summary>
    /// Gets the command name.
    /// </summary>
    /// <value>
    /// The command name (string).
    /// </value>
    public string Command { get; }

    /// <summary>
    /// Gets the command parameters.
    /// </summary>
    /// <value>
    /// The command parameters (string).
    /// </value>
    public string Parameters { get; }
}