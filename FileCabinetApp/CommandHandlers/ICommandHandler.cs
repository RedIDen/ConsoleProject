namespace FileCabinetApp.CommandHandlers;

internal interface ICommandHandler
{
    public void SetNext(ICommandHandler commandHandler);

    public void Handle(AppCommandRequest appCommandRequest);
}