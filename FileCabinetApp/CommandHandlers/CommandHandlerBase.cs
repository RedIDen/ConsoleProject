using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public abstract class CommandHandlerBase : ICommandHandler
{
    public const string WrongSyntaxError = "Wrong command syntax!";

    protected static List<string> allCommands = new List<string>();

    protected ICommandHandler nextHandler;

    protected abstract string[] CommandNames { get; }

    public CommandHandlerBase()
    {
        foreach (var command in this.CommandNames)
        {
            CommandHandlerBase.allCommands.Add(command);
        }
    }

    public virtual void Handle(AppCommandRequest appCommandRequest)
    {
        foreach (var commandName in this.CommandNames)
        {
            if (string.Equals(appCommandRequest.Command, commandName, StringComparison.InvariantCultureIgnoreCase))
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

            foreach (var command in CommandHandlerBase.allCommands)
            {
                if (command.Contains(appCommandRequest.Command) ||
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

    protected abstract void MakeWork(string parameters);

    public void SetNext(ICommandHandler commandHandler)
    {
        this.nextHandler = commandHandler;
    }
}