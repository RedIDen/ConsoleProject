using System.Globalization;
using System.Text;

#pragma warning disable CS8602
#pragma warning disable CS8601

namespace FileCabinetApp;

/// <summary>
/// The main class of the program.
/// </summary>
public static class Program
{
    private const string DeveloperName = "Deniska Vasilyev";
    private const string HintMessage = "Enter your command, or enter 'help' to get help.";
    private const int CommandHelpIndex = 0;
    private const int DescriptionHelpIndex = 1;
    private const int ExplanationHelpIndex = 2;

    private static FileCabinetService fileCabinetService = new FileCabinetService();

    private static bool isRunning = true;

    private static Tuple<string, Action<string>>[] commands = new Tuple<string, Action<string>>[]
    {
            new Tuple<string, Action<string>>("help", PrintHelp),
            new Tuple<string, Action<string>>("create", Create),
            new Tuple<string, Action<string>>("edit", Edit),
            new Tuple<string, Action<string>>("list", List),
            new Tuple<string, Action<string>>("stat", Stat),
            new Tuple<string, Action<string>>("find", Find),
            new Tuple<string, Action<string>>("exit", Exit),
    };

    private static string[][] helpMessages = new string[][]
    {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "edit", "edits the record", "The 'edit' command edits the record." },
            new string[] { "list", "shows the list of records", "The 'list' command shows the list of records." },
            new string[] { "stat", "shows stat", "The 'stat' command shows stat." },
            new string[] { "find", "searches for the records", "The 'find' searches for the records by parameters." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
    };

    /// <summary>
    /// The main method of the program.
    /// </summary>
    /// <param name="args">Extra arguments to run the application.</param>
    public static void Main(string[] args)
    {
        Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}");
        Console.WriteLine(Program.HintMessage);
        Console.WriteLine();

        do
        {
            Console.Write("> ");
            var inputs = Console.ReadLine().Split(' ', 2);
            const int commandIndex = 0;
            var command = inputs[commandIndex];

            if (string.IsNullOrEmpty(command))
            {
                Console.WriteLine(Program.HintMessage);
                continue;
            }

            var index = Array.FindIndex(commands, 0, commands.Length, i => i.Item1.Equals(command, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                const int parametersIndex = 1;
                var parameters = inputs.Length > 1 ? inputs[parametersIndex] : string.Empty;
                commands[index].Item2(parameters);
            }
            else
            {
                PrintMissedCommandInfo(command);
            }
        }
        while (isRunning);
    }

    /// <summary>
    /// Prints the error message, if user wtires the incorrect command.
    /// </summary>
    /// <param name="command">The name of comman user wrote.</param>
    private static void PrintMissedCommandInfo(string command)
    {
        Console.WriteLine($"There is no '{command}' command.");
        Console.WriteLine();
    }

    /// <summary>
    /// Shows the list of all commands and their descriptions.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    private static void PrintHelp(string parameters)
    {
        if (!string.IsNullOrEmpty(parameters))
        {
            var index = Array.FindIndex(helpMessages, 0, helpMessages.Length, i => string.Equals(i[Program.CommandHelpIndex], parameters, StringComparison.InvariantCultureIgnoreCase));
            if (index >= 0)
            {
                Console.WriteLine(helpMessages[index][Program.ExplanationHelpIndex]);
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
                Console.WriteLine("\t{0}\t- {1}", helpMessage[Program.CommandHelpIndex], helpMessage[Program.DescriptionHelpIndex]);
            }
        }

        Console.WriteLine();
    }

    /// <summary>
    /// Shows the count of records.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    private static void Stat(string parameters)
    {
        var recordsCount = Program.fileCabinetService.GetStat();
        Console.WriteLine($"{recordsCount} record(s).");
    }

    private static void Edit(string parameters)
    {
        string firstName;
        string lastName;
        DateTime dateOfBirth;
        short workExperience;
        decimal balance;
        char favLetter;

        while (true)
        {
            int id = int.Parse(parameters);

            int index = Program.fileCabinetService.FindRecordIndexById(id);

            if (index == -1)
            {
                Console.WriteLine($"#{id} record is not found.");
                break;
            }

            ReadDataForRecord(out firstName, out lastName, out dateOfBirth, out workExperience, out balance, out favLetter);

            try
            {
                Program.fileCabinetService.EditRecord(firstName, lastName, dateOfBirth, workExperience, balance, favLetter, index);
                Console.WriteLine($"Record #{id} is created.");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} Please, enter the correct data:");
            }
        }
    }

    /// <summary>
    /// Searches the records by certain parameters.
    /// </summary>
    /// <param name="parameters">Parameters for the method.</param>
    private static void Find(string parameters)
    {
        var parametersArray = parameters.Split(' ', 2);
        parametersArray[1] = parametersArray[1].Trim('\"');

        var foundRecords = parametersArray[0].ToLower() switch
        {
            "firstname" => Program.fileCabinetService.FindByFirstName(parametersArray[1]),
            "lastname" => Program.fileCabinetService.FindByLastName(parametersArray[1]),
            "dateofbirth" => Program.fileCabinetService.FindByDateOfBirth(parametersArray[1]),
            _ => Array.Empty<FileCabinetRecord>(),
        };

        Program.ShowRecords(foundRecords, "No records found.");
    }

    /// <summary>
    /// Creates new record.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    private static void Create(string parameters)
    {
        string firstName;
        string lastName;
        DateTime dateOfBirth;
        short workExperience;
        decimal balance;
        char favLetter;

        while (true)
        {
            ReadDataForRecord(out firstName, out lastName, out dateOfBirth, out workExperience, out balance, out favLetter);

            try
            {
                int id = Program.fileCabinetService.CreateRecord(firstName, lastName, dateOfBirth, workExperience, balance, favLetter);
                Console.WriteLine($"Record #{id} is created.");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} Please, enter the correct data:");
            }
        }
    }

    /// <summary>
    /// Reads the data for the record from the console.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <param name="dateOfBirth">Date of birth.</param>
    /// <param name="workExperience">Work experience.</param>
    /// <param name="balance">Balance.</param>
    /// <param name="favLetter">Favorite letter.</param>
    private static void ReadDataForRecord(
        out string firstName,
        out string lastName,
        out DateTime dateOfBirth,
        out short workExperience,
        out decimal balance,
        out char favLetter)
    {
        Console.Write("First Name: ");
        firstName = Console.ReadLine();

        Console.Write("Last Name: ");
        lastName = Console.ReadLine();

        while (true)
        {
            Console.Write("Date of birth: ");
            if (DateTime.TryParse(
                Console.ReadLine(),
                CultureInfo.CreateSpecificCulture("en-US"),
                DateTimeStyles.None,
                out dateOfBirth))
            {
                break;
            }

            Console.Write("Wrong input. Try again. ");
        }

        while (true)
        {
            Console.Write("Work experience: ");
            if (short.TryParse(
                Console.ReadLine(),
                out workExperience))
            {
                break;
            }

            Console.Write("Wrong input. Try again. ");
        }

        while (true)
        {
            Console.Write("Balance: ");
            if (decimal.TryParse(
                Console.ReadLine(),
                NumberStyles.AllowDecimalPoint,
                CultureInfo.InvariantCulture,
                out balance))
            {
                break;
            }

            Console.Write("Wrong input. Try again. ");
        }

        while (true)
        {
            Console.Write("Favorite letter: ");
            if (char.TryParse(
                Console.ReadLine(),
                out favLetter))
            {
                break;
            }

            Console.Write("Wrong input. Try again. ");
        }
    }

    /// <summary>
    /// Shows the list of all records.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    private static void List(string parameters)
    {
        FileCabinetRecord[] list = Program.fileCabinetService.GetRecords();
        Program.ShowRecords(list, "The list is empty.");
    }

    /// <summary>
    /// Shows the recieved list of records in the console.
    /// </summary>
    /// <param name="list">The list to show.</param>
    /// <param name="errorMessage">Error message to show if the list is empty.</param>
    private static void ShowRecords(FileCabinetRecord[] list, string errorMessage)
    {
        if (list.Length == 0)
        {
            Console.WriteLine(errorMessage);
            return;
        }

        foreach (var record in list)
        {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append($"#{record.Id}, ");
            stringBuilder.Append($"{record.FirstName}, ");
            stringBuilder.Append($"{record.LastName}, ");
            stringBuilder.Append($"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}, ");
            stringBuilder.Append($"{record.WorkExperience}, ");
            stringBuilder.Append($"{record.Balance.ToString(CultureInfo.InvariantCulture)}, ");
            stringBuilder.Append($"\'{record.FavChar}\'");

            Console.WriteLine(stringBuilder);
        }
    }

    /// <summary>
    /// Exits the application.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    private static void Exit(string parameters)
    {
        Console.WriteLine("Exiting an application...");
        Program.isRunning = false;
    }
}