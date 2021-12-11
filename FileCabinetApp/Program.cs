using System.Globalization;

#pragma warning disable CS8604
#pragma warning disable CS8602
#pragma warning disable CS8601

namespace FileCabinetApp;
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
            new Tuple<string, Action<string>>("exit", Exit),
    };

    private static string[][] helpMessages = new string[][]
    {
            new string[] { "help", "prints the help screen", "The 'help' command prints the help screen." },
            new string[] { "create", "creates new record", "The 'create' command creates new record." },
            new string[] { "edit", "edits the record", "The 'edit' command edits the record." },
            new string[] { "list", "shows the list of records", "The 'list' command shows the list of records." },
            new string[] { "stat", "shows stat", "The 'stat' command shows stat." },
            new string[] { "exit", "exits the application", "The 'exit' command exits the application." },
    };

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

    private static void PrintMissedCommandInfo(string command)
    {
        Console.WriteLine($"There is no '{command}' command.");
        Console.WriteLine();
    }

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
                Program.fileCabinetService.EditRecord(id, firstName, lastName, dateOfBirth, workExperience, balance, favLetter, index);
                Console.WriteLine($"Record #{id} is created.");
                return;
            }
            catch (Exception e)
            {
                Console.WriteLine($"{e.Message} Please, enter the correct data:");
            }
        }
    }

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

        Console.Write("Date of birth: ");
        dateOfBirth = DateTime.Parse(
            Console.ReadLine(),
            CultureInfo.CreateSpecificCulture("en-US"),
            DateTimeStyles.None);

        Console.Write("Work experience: ");
        workExperience = short.Parse(Console.ReadLine());

        Console.Write("Balance: ");
        balance = decimal.Parse(Console.ReadLine(), NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture);

        Console.Write("Favorive letter: ");
        favLetter = char.Parse(Console.ReadLine());
    }

    private static void List(string parameters)
    {
        FileCabinetRecord[] list = fileCabinetService.GetRecords();
        if (list.Length == 0)
        {
            Console.WriteLine("The list is empty.");
        }
        else
        {
            foreach (var record in list)
            {
                Console.WriteLine(
                    $"#{record.Id}, " +
                    $"{record.FirstName}, " +
                    $"{record.LastName}, " +
                    $"{record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.CreateSpecificCulture("en-US"))}, " +
                    $"{record.WorkExperience}, " +
                    $"{record.Balance.ToString(CultureInfo.InvariantCulture)}, " +
                    $"\'{record.FavChar}\'");
            }
        }
    }

    private static void Exit(string parameters)
    {
        Console.WriteLine("Exiting an application...");
        isRunning = false;
    }
}