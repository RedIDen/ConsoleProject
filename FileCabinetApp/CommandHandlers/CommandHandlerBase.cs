using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public abstract class CommandHandlerBase : ICommandHandler
{
    public const string WrongSyntaxError = "Wrong command syntax!";

    protected ICommandHandler nextHandler;

    protected virtual string CommandName { get; set; }

    public virtual void Handle(AppCommandRequest appCommandRequest)
    {
        if (string.Equals(appCommandRequest.Command, this.CommandName, StringComparison.InvariantCultureIgnoreCase))
        {
            this.MakeWork(appCommandRequest.Parameters);
        }
        else if (this.nextHandler != null)
        {
            this.nextHandler.Handle(appCommandRequest);
        }
        else
        {
            Console.WriteLine($"There is no '{appCommandRequest.Command}' command.");
            Console.WriteLine();
        }
    }

    protected abstract void MakeWork(string parameters);

    public void SetNext(ICommandHandler commandHandler)
    {
        this.nextHandler = commandHandler;
    }
}