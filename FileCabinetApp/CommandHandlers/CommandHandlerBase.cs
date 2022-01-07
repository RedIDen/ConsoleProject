namespace FileCabinetApp.CommandHandlers
{
    public abstract class CommandHandlerBase : ICommandHandler
    {
        private ICommandHandler nextHandler;

        public abstract void Handle(AppCommandRequest appCommandRequest);

        public void SetNext(ICommandHandler commandHandler)
        {
            this.nextHandler = commandHandler;
        }
    }
}
