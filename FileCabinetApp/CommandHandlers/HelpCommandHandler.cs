﻿namespace FileCabinetApp.CommandHandlers;

internal class HelpCommandHandler : CommandHandlerBase
{
    protected override string[] CommandNames { get; } = { "help" };

    private const int CommandHelpIndex = 0;
    private const int DescriptionHelpIndex = 1;
    private const int ExplanationHelpIndex = 2;

    private static string[][] helpMessages = new string[][]
    {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "insert", "creates new record", "The 'insert' command creates new record." },
            new string[] { "update", "edits the record", "The 'update' command edits the record." },
            new string[] { "select", "shows the list of records with parameters", "The 'select' command shows the list of records with parameters." },
            new string[] { "delete", "deletes the record", "The 'delete' command deletes the record." },
            new string[] { "stat", "shows stat", "The 'stat' command shows stat." },
            new string[] { "export", "exports records to the file", "The 'export' command exports records to the file." },
            new string[] { "import", "imports records from the file", "The 'import' command imports records from the file." },
            new string[] { "purge", "purges the filesystem", "The 'purge' command purges the filesystem." },
            new string[] { "--validation-rules", "changes the validation rules", "The '--validation-rules (-v)' command changes the validation rules." },
            new string[] { "--storage", "changes the storage", "The '--storage' command changes the storage." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
    };

    /// <summary>
    /// Shows the list of all commands and their descriptions.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    protected override void MakeWork(string parameters)
    {
        if (!string.IsNullOrEmpty(parameters))
        {
            var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[HelpCommandHandler.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                Console.WriteLine(helpMessages[index][HelpCommandHandler.ExplanationHelpIndex]);
            }
            else
            {
                Console.WriteLine($"There is no explanation for '{parameters}' command.");
            }
        }
        else
        {
            Console.WriteLine("Available commands:");

            foreach (var helpMessage in helpMessages)
            {
                Console.WriteLine("\t{0}\t- {1}", helpMessage[HelpCommandHandler.CommandHelpIndex], helpMessage[HelpCommandHandler.DescriptionHelpIndex]);
            }
        }

        Console.WriteLine();
    }
}
