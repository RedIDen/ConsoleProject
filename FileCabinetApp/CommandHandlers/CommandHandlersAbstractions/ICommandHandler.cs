namespace FileCabinetApp.CommandHandlers;

/// <summary>
/// The inteface of command handlers.
/// </summary>
internal interface ICommandHandler
{
    /// <summary>
    /// Sets the next command handler.
    /// </summary>
    /// <param name="commandHandler">Command handler.</param>
    public void SetNext(ICommandHandler commandHandler);

    /// <summary>
    /// Handles the command request.
    /// </summary>
    /// <param name="appCommandRequest">The command request.</param>
    public void Handle(AppCommandRequest appCommandRequest);
}