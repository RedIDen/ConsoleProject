using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

#pragma warning disable CS8602
#pragma warning disable CS8604

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

    private static IFileCabinetService fileCabinetService = new FileCabinetMemoryService(new DefaultValidator());

    private static string validationRulesMessage = "Using default validation rules.";
    private static string storageTypeMessage = "Using memory storage.";

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
            new Tuple<string, Action<string>>("export", Export),
            new Tuple<string, Action<string>>("import", Import),
            new Tuple<string, Action<string>>("remove", Remove),
            new Tuple<string, Action<string>>("purge", Purge),
            new Tuple<string, Action<string>>("--validation-rules", ChangeValidationRules),
            new Tuple<string, Action<string>>("-v", ChangeValidationRules),
            new Tuple<string, Action<string>>("--storage", ChangeStorage),
            new Tuple<string, Action<string>>("-s", ChangeStorage),
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
            new string[] { "export", "exports records to the file", "The 'export' command exports records to the file." },
            new string[] { "import", "imports records from the file", "The 'import' command imports records from the file." },
            new string[] { "remove", "removes the record", "The 'remove' command removes the record." },
            new string[] { "purge", "purges the filesystem", "The 'purge' command purges the filesystem." },
            new string[] { "--validation-rules", "changes the validation rules", "The '--validation-rules (-v)' command changes the validation rules." },
            new string[] { "--storage", "changes the storage", "The '--storage' command changes the storage." },
    };

    /// <summary>
    /// The main method of the program.
    /// </summary>
    /// <param name="args">Extra arguments to run the application.</param>
    public static void Main(string[] args)
    {
        WriteGreeting();
        do
        {
            Console.Write("> ");
            var inputs = Console.ReadLine().Split(new char[] { ' ', '=' }, 2);
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
    /// Writes the greeting message.
    /// </summary>
    private static void WriteGreeting()
    {
        Console.WriteLine($"File Cabinet Application, developed by {Program.DeveloperName}.");
        Console.WriteLine(Program.validationRulesMessage);
        Console.WriteLine(Program.storageTypeMessage);
        Console.WriteLine(Program.HintMessage);
        Console.WriteLine();
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

    private static void ChangeStorage(string parameters)
    {
        if (parameters == "memory")
        {
            if (Program.fileCabinetService is FileCabinetMemoryService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            ((FileCabinetFilesystemService)Program.fileCabinetService).Close();
            Program.storageTypeMessage = "Using memory storage.";
            Program.fileCabinetService = new FileCabinetMemoryService(Program.fileCabinetService.Validator);
        }
        else if (parameters == "file")
        {
            if (Program.fileCabinetService is FileCabinetFilesystemService)
            {
                Console.WriteLine("This storage is already in use.");
                return;
            }

            Program.storageTypeMessage = "Using filesystem storage.";
            Program.fileCabinetService = new FileCabinetFilesystemService(Program.fileCabinetService.Validator, File.Open(FileCabinetFilesystemService.FILENAME, FileMode.OpenOrCreate));
        }
        else
        {
            Console.WriteLine("Wrong parameters!");
            return;
        }

        Program.WriteGreeting();
    }

    /// <summary>
    /// Changes the validation rules.
    /// </summary>
    /// <param name="parameters">Parameters for the method.</param>
    private static void ChangeValidationRules(string parameters)
    {
        if (parameters.Equals("default", StringComparison.InvariantCultureIgnoreCase))
        {
            Program.fileCabinetService.Validator = new DefaultValidator();
            Program.validationRulesMessage = "Using default validation rules.";
        }
        else if (parameters.Equals("custom", StringComparison.InvariantCultureIgnoreCase))
        {
            Program.fileCabinetService.Validator = new CustomValidator();
            Program.validationRulesMessage = "Using custom validation rules.";
        }
        else
        {
            Console.WriteLine("Wrong parameters!");
            return;
        }

        Program.WriteGreeting();
    }

    /// <summary>
    /// Exports the list of records to the file.
    /// </summary>
    /// <param name="parameters">Parameters for the method.</param>
    private static void Export(string parameters)
    {
        var parametersArray = parameters.Trim().Split();

        if (parametersArray.Length != 2)
        {
            Console.WriteLine("Wrong command syntax!");
            return;
        }

        string fileType = parametersArray[0];
        string fileName = parametersArray[1];

        if (!fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase) &&
            !fileType.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
        {
            Console.WriteLine("Wrong file type!");
            return;
        }

        if (File.Exists(fileName))
        {
            Console.Write($"File exists - rewrite {fileName}? [Y/n] ");
            do
            {
                ConsoleKey key = Console.ReadKey().Key;
                if (key == ConsoleKey.Y)
                {
                    Console.WriteLine();
                    break;
                }

                if (key == ConsoleKey.N)
                {
                    Console.WriteLine();
                    return;
                }
            }
            while (true);
        }

        try
        {
            var streamWriter = new StreamWriter(fileName);
            var fileCabinetServiceSnapshot = Program.fileCabinetService.MakeSnapshot();

            if (fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
            {
                fileCabinetServiceSnapshot.SaveToCsv(streamWriter);
            }
            else
            {
                fileCabinetServiceSnapshot.SaveToXml(streamWriter);
            }

            Console.WriteLine($"All records are exported to {fileName}.");
            streamWriter.Close();
        }
        catch (DirectoryNotFoundException)
        {
            Console.WriteLine($"Export failed: can't open file {fileName}.");
            return;
        }
    }

    /// <summary>
    /// Imports the list of records from the file.
    /// </summary>
    /// <param name="parameters">Parameters for the method.</param>
    private static void Import(string parameters)
    {
        var parametersArray = parameters.Trim().Split();

        if (parametersArray.Length != 2)
        {
            Console.WriteLine("Wrong command syntax!");
            return;
        }

        string fileType = parametersArray[0];
        string fileName = parametersArray[1];

        if (!fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase) &&
            !fileType.Equals("xml", StringComparison.InvariantCultureIgnoreCase))
        {
            Console.WriteLine("Wrong file type!");
            return;
        }

        if (File.Exists(fileName))
        {
            try
            {
                var fileStream = new FileStream(fileName, FileMode.Open);

                var snapshot = new FileCabinetServiceSnapshot();

                if (fileType.Equals("csv", StringComparison.InvariantCultureIgnoreCase))
                {
                    snapshot.LoadFromCsv(fileStream);
                }
                else
                {
                    snapshot.LoadFromXml(fileStream);
                }

                Program.fileCabinetService.Restore(snapshot);

                Console.WriteLine($"All records are imported from {fileName}.");
                fileStream.Close();
            }
            catch (DirectoryNotFoundException)
            {
                Console.WriteLine($"Import failed: can't open file {fileName}.");
                return;
            }
        }
        else
        {
            Console.Write($"This file doesn't exist!");
        }
    }

    /// <summary>
    /// Shows the count of records.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    private static void Stat(string parameters)
    {
        (int records, int deleted) = Program.fileCabinetService.GetStat();
        Console.WriteLine($"{records} record(s).");
        Console.WriteLine($"{deleted} removed.");
    }

    /// <summary>
    /// Edits the record data.
    /// </summary>
    /// <param name="parameters">Parameters for the method.</param>
    private static void Edit(string parameters)
    {
        string firstName;
        string lastName;
        DateTime dateOfBirth;
        short workExperience;
        decimal balance;
        char favLetter;

        if (!int.TryParse(parameters, out int id))
        {
            Console.WriteLine("Wrong command syntax!");
            return;
        }

        int index = Program.fileCabinetService.FindRecordIndexById(id);

        if (index == -1)
        {
            Console.WriteLine($"#{id} record is not found.");
            return;
        }

        ReadDataForRecord(out firstName, out lastName, out dateOfBirth, out workExperience, out balance, out favLetter);

        Program.fileCabinetService.EditRecord(
            new FileCabinetRecord()
            {
                Id = id,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                WorkExperience = workExperience,
                Balance = balance,
                FavLetter = favLetter,
            },
            index);

        Console.WriteLine($"Record #{id} is edited.");
    }

    /// <summary>
    /// Removes the record.
    /// </summary>
    /// <param name="parameters">Parameters for the method.</param>
    private static void Remove(string parameters)
    {
        if (!int.TryParse(parameters, out int id))
        {
            Console.WriteLine("Wrong command syntax!");
            return;
        }

        int index = Program.fileCabinetService.FindRecordIndexById(id);

        if (index == -1)
        {
            Console.WriteLine($"#{id} record is not found.");
            return;
        }

        Program.fileCabinetService.RemoveRecord(index);

        Console.WriteLine($"Record #{id} is removed.");
    }

    /// <summary>
    /// Purges the filesystem.
    /// </summary>
    /// <param name="parameters">Parameters for the method.</param>
    private static void Purge(string parameters)
    {
        if (Program.fileCabinetService is not FileCabinetFilesystemService)
        {
            Console.WriteLine("This command is available only for filesystem storage mode.");
            return;
        }

        (int deletedNum, int beforeNum) = ((FileCabinetFilesystemService)Program.fileCabinetService).Purge();

        Console.WriteLine($"Data file processing is completed: {deletedNum} of {beforeNum} records were purged.");
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
            _ => new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>()),
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

        ReadDataForRecord(out firstName, out lastName, out dateOfBirth, out workExperience, out balance, out favLetter);

        int id = Program.fileCabinetService.CreateRecord(
            new FileCabinetRecord()
            {
                Id = 0,
                FirstName = firstName,
                LastName = lastName,
                DateOfBirth = dateOfBirth,
                WorkExperience = workExperience,
                Balance = balance,
                FavLetter = favLetter,
            });

        Console.WriteLine($"Record #{id} is created.");
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
        Console.Write("First name: ");
        firstName = ReadInput(Program.StringConverter, fileCabinetService.Validator.FirstNameValidator);

        Console.Write("Last name: ");
        lastName = ReadInput(Program.StringConverter, fileCabinetService.Validator.LastNameValidator);

        Console.Write("Date of birth: ");
        dateOfBirth = ReadInput(Program.DateConverter, fileCabinetService.Validator.DateOfBirthValidator);

        Console.Write("Work experience: ");
        workExperience = ReadInput(Program.ShortConverter, fileCabinetService.Validator.WorkExperienceValidator);

        Console.Write("Balance: ");
        balance = ReadInput(Program.DecimalConverter, fileCabinetService.Validator.BalanceValidator);

        Console.Write("Favorite letter: ");
        favLetter = ReadInput(Program.CharConverter, fileCabinetService.Validator.FavLetterValidator);
    }

    /// <summary>
    /// Shows the list of all records.
    /// </summary>
    /// <param name="parameters">Extra parameteres for the method.</param>
    private static void List(string parameters)
    {
        ReadOnlyCollection<FileCabinetRecord> list = Program.fileCabinetService.GetRecords();
        Program.ShowRecords(list, "The list is empty.");
    }

    /// <summary>
    /// Shows the recieved list of records in the console.
    /// </summary>
    /// <param name="list">The list to show.</param>
    /// <param name="errorMessage">Error message to show if the list is empty.</param>
    private static void ShowRecords(ReadOnlyCollection<FileCabinetRecord> list, string errorMessage)
    {
        if (list.Count == 0)
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
            stringBuilder.Append($"\'{record.FavLetter}\'");

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

    /// <summary>
    /// Reads the input, converts and validates it.
    /// </summary>
    /// <typeparam name="T">The type of the data to convert to.</typeparam>
    /// <param name="converter">Converter.</param>
    /// <param name="validator">Validator.</param>
    /// <returns>The result of convertation and validation.</returns>
    private static T ReadInput<T>(Func<string, ValueTuple<bool, string, T>> converter, Func<T, ValueTuple<bool, string>> validator)
    {
        do
        {
            T value;

            var input = Console.ReadLine();
            var conversionResult = converter(input);

            if (!conversionResult.Item1)
            {
                Console.WriteLine($"Conversion failed: {conversionResult.Item2}. Please, correct your input.");
                continue;
            }

            value = conversionResult.Item3;

            var validationResult = validator(value);
            if (!validationResult.Item1)
            {
                Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                continue;
            }

            return value;
        }
        while (true);
    }

    /// <summary>
    /// Converter to string.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result.</returns>
    private static (bool, string, string) StringConverter(string value) => (true, string.Empty, value);

    /// <summary>
    /// Converter to date.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result.</returns>
    private static (bool, string, DateTime) DateConverter(string value)
    {
        var culture = CultureInfo.CreateSpecificCulture("en-US");
        var styles = DateTimeStyles.None;
        if (DateTime.TryParse(value, culture, styles, out DateTime result))
        {
            return (true, string.Empty, result);
        }
        else
        {
            return (false, "incorrect date format", result);
        }
    }

    /// <summary>
    /// Converter to short.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result.</returns>
    private static (bool, string, short) ShortConverter(string value)
    {
        if (short.TryParse(value, out short result))
        {
            return (true, string.Empty, result);
        }
        else
        {
            return (false, "enter the correct number", result);
        }
    }

    /// <summary>
    /// Converter to decimal.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result.</returns>
    private static (bool, string, decimal) DecimalConverter(string value)
    {
        if (decimal.TryParse(value, out decimal result))
        {
            return (true, string.Empty, result);
        }
        else
        {
            return (false, "enter the correct number", result);
        }
    }

    /// <summary>
    /// Converter to char.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result.</returns>
    private static (bool, string, char) CharConverter(string value)
    {
        if (char.TryParse(value, out char result))
        {
            return (true, string.Empty, result);
        }
        else
        {
            return (false, "enter the correct char", result);
        }
    }
}