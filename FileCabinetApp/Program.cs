﻿using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using FileCabinetApp.CommandHandlers;

#pragma warning disable CS8602
#pragma warning disable CS8604

namespace FileCabinetApp;

/// <summary>
/// The main class of the program.
/// </summary>
public static class Program
{
    private const string HintMessage = "Enter your command, or enter 'help' to get help.";
    private const string DeveloperName = "Deniska Vasilyev";

    private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

    public static string validationRulesMessage = "Using default validation rules.";
    public static string storageTypeMessage = "Using memory storage.";

    private static ICommandHandler commandHandler;

    public static bool isRunning = true;

    /// <summary>
    /// The main method of the program.
    /// </summary>
    /// <param name="args">Extra arguments to run the application.</param>
    public static void Main(string[] args)
    {
        Program.commandHandler = Program.CreateCommandHandler();

        WriteGreeting();

        do
        {
            Console.Write("> ");
            var inputs = Console.ReadLine().Split(new char[] { ' ', '=' }, 2);

            const int commandIndex = 0;
            if (string.IsNullOrEmpty(inputs[commandIndex]))
            {
                Console.WriteLine(Program.HintMessage);
                return;
            }

            const int parametersIndex = 1;
            var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
            Program.commandHandler.Handle(new AppCommandRequest(inputs[commandIndex], parameters));
        }
        while (isRunning);
    }

    private static ICommandHandler CreateCommandHandler()
    {
        ICommandHandler exit = new ExitCommandHandler(Program.fileCabinetService);

        ICommandHandler edit = new EditCommandHandler(Program.fileCabinetService);
        edit.SetNext(exit);

        ICommandHandler export = new ExportCommandHandler(Program.fileCabinetService);
        export.SetNext(edit);

        ICommandHandler find = new FindCommandHandler(Program.fileCabinetService);
        find.SetNext(export);

        ICommandHandler help = new HelpCommandHandler(Program.fileCabinetService);
        help.SetNext(find);

        ICommandHandler import = new ImportCommandHandler(Program.fileCabinetService);
        import.SetNext(help);

        ICommandHandler list = new ListCommandHandler(Program.fileCabinetService);
        list.SetNext(import);

        ICommandHandler purge = new PurgeCommandHandler(Program.fileCabinetService);
        purge.SetNext(list);

        ICommandHandler remove = new RemoveCommandHandler(Program.fileCabinetService);
        remove.SetNext(purge);

        ICommandHandler stat = new StatCommandHandler(Program.fileCabinetService);
        stat.SetNext(remove);

        ICommandHandler storage = new StorageCommandHandler(Program.fileCabinetService);
        storage.SetNext(stat);

        ICommandHandler validation = new ValidationRulesCommandHandler(Program.fileCabinetService);
        validation.SetNext(storage);

        ICommandHandler create = new CreateCommandHandler(Program.fileCabinetService);
        create.SetNext(validation);

        return create;
    }

    /// <summary>
    /// Writes the greeting message.
    /// </summary>
    public static void WriteGreeting()
    {
        Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}.");
        Console.WriteLine(Program.validationRulesMessage);
        Console.WriteLine(Program.storageTypeMessage);
        Console.WriteLine(Program.HintMessage);
        Console.WriteLine();
    }
}