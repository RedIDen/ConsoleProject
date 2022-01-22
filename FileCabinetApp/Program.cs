global using System.Collections.ObjectModel;
global using System.Globalization;
global using System.Text;
global using FileCabinetApp.CommandHandlers;
global using FileCabinetApp.Validators;
global using FileCabinetApp.Validators.FullRecordValidators;
global using Newtonsoft.Json;

#pragma warning disable CS8602
#pragma warning disable CS8604
#pragma warning disable CS8618

namespace FileCabinetApp;

/// <summary>
/// The main class of the program.
/// </summary>
public static class Program
{
    private const string HintMessage = "Enter your command, or enter 'help' to get help.";
    private const string DeveloperName = "Deniska Vasilyev";
    private const string ValidatorsDataPath = "validation-rules.json";

    private static Dictionary<string, CompositeValidator> validators;
    private static FileCabinetTrasferHelper fileCabinetService;

    public static string validationRulesMessage = "Using default validation rules.";
    public static string storageTypeMessage = "Using memory storage.";

    private static ICommandHandler commandHandler;

    private static bool isRunning = true;

    /// <summary>
    /// The main method of the program.
    /// </summary>
    /// <param name="args">Extra arguments to run the application.</param>
    public static void Main(string[] args)
    {
        Program.validators = new ValidatorDeserializer().Deserialize(Program.ValidatorsDataPath);
        Program.fileCabinetService = new FileCabinetTrasferHelper(new FileCabinetMemoryService(Program.validators.GetValueOrDefault("default")));
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
        var exit = new ExitCommandHandler((bool value) => Program.isRunning = value);

        var edit = new UpdateCommandHandler(Program.fileCabinetService);
        edit.SetNext(exit);

        var export = new ExportCommandHandler(Program.fileCabinetService);
        export.SetNext(edit);

        var help = new HelpCommandHandler();
        help.SetNext(export);

        var import = new ImportCommandHandler(Program.fileCabinetService);
        import.SetNext(help);

        var list = new SelectCommandHandler(Program.fileCabinetService, Program.DefaultRecordPrinter);
        list.SetNext(import);

        var purge = new PurgeCommandHandler(Program.fileCabinetService);
        purge.SetNext(list);

        var remove = new DeleteCommandHandler(Program.fileCabinetService);
        remove.SetNext(purge);

        var stat = new StatCommandHandler(Program.fileCabinetService);
        stat.SetNext(remove);

        var storage = new StorageCommandHandler(Program.fileCabinetService);
        storage.SetNext(stat);

        var validation = new ValidationRulesCommandHandler(Program.fileCabinetService, Program.validators);
        validation.SetNext(storage);

        var create = new InsertCommandHandler(Program.fileCabinetService);
        create.SetNext(validation);

        var useStopwatch = new UseStopwatchCommandHandler(Program.fileCabinetService);
        useStopwatch.SetNext(create);

        var useLogger = new UseLoggerCommandHandler(Program.fileCabinetService);
        useLogger.SetNext(useStopwatch);

        return useLogger;
    }

    private static void DefaultRecordPrinter(IEnumerable<FileCabinetRecord> records, IEnumerable<string> parameters)
    {
        const string errorMessage = "No records found!";
        const string idWord = "id";
        const string firstNameWord = "firstname";
        const string lastNameWord = "lastname";
        const string dateOfBirthWord = "dateofbirth";
        const string balanceWord = "balance";
        const string workExperienceWord = "workexperience";
        const string favLetterWord = "favletter";

        if (!records.Any())
        {
            Console.WriteLine(errorMessage);
            return;
        }

        if (!parameters.Any())
        {
            parameters = parameters
                .Append(idWord)
                .Append(firstNameWord)
                .Append(lastNameWord)
                .Append(dateOfBirthWord)
                .Append(balanceWord)
                .Append(workExperienceWord)
                .Append(favLetterWord);
        }

        var fieldWriters = new List<Action<FileCabinetRecord>>();

        const int freeSpace = 2;

        byte favLetterLength = (byte)(favLetterWord.Length);
        byte idLength, firstNameLength, lastNameLength, dateOfBirthLength, balanceLength, workExperienceLength;

        StringBuilder borderLine = new StringBuilder().Append('+');
        StringBuilder fieldNames = new StringBuilder().Append('|');
        foreach (var parameter in parameters)
        {
            fieldNames.Append(' ');

            switch (parameter.ToLower())
            {
                case idWord:
                    var idName = "Id";
                    idLength = (byte)Math.Max(records.Select(x => x.Id.ToString().Length).Max(), idName.Length);
                    fieldWriters.Add(WriteId);
                    borderLine.Append('-', idLength + freeSpace);
                    fieldNames.Append(idName);
                    fieldNames.Append(' ', idLength - idName.Length);
                    break;
                case firstNameWord:
                    var firstNameName = "FirstName";
                    firstNameLength = (byte)Math.Max(records.Select(x => x.FirstName.Length).Max(), firstNameName.Length);
                    fieldWriters.Add(WriteFirstName);
                    borderLine.Append('-', firstNameLength + freeSpace);
                    fieldNames.Append(firstNameName);
                    fieldNames.Append(' ', firstNameLength - firstNameName.Length);
                    break;
                case lastNameWord:
                    var lastNameName = "LastName";
                    lastNameLength = (byte)Math.Max(records.Select(x => x.LastName.Length).Max(), lastNameName.Length);
                    fieldWriters.Add(WriteLastName);
                    borderLine.Append('-', lastNameLength + freeSpace);
                    fieldNames.Append(lastNameName);
                    fieldNames.Append(' ', lastNameLength - lastNameName.Length);
                    break;
                case dateOfBirthWord:
                    var dateOfBirthName = "DateOfBirth";
                    dateOfBirthLength = (byte)Math.Max(records.Select(x => x.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture).Length).Max(), dateOfBirthName.Length);
                    fieldWriters.Add(WriteDateOfBirth);
                    borderLine.Append('-', dateOfBirthLength + freeSpace);
                    fieldNames.Append(dateOfBirthName);
                    fieldNames.Append(' ', dateOfBirthLength - dateOfBirthName.Length);
                    break;
                case balanceWord:
                    var balanceName = "Balance";
                    balanceLength = (byte)Math.Max(records.Select(x => x.Balance.ToString().Length).Max(), balanceName.Length);
                    fieldWriters.Add(WriteBalance);
                    borderLine.Append('-', balanceLength + freeSpace);
                    fieldNames.Append(balanceName);
                    fieldNames.Append(' ', balanceLength - balanceName.Length);
                    break;
                case workExperienceWord:
                    var workExperienceName = "WorkExperience";
                    workExperienceLength = (byte)Math.Max(records.Select(x => x.WorkExperience.ToString().Length).Max(), workExperienceName.Length);
                    fieldWriters.Add(WriteWorkExperience);
                    borderLine.Append('-', workExperienceLength + freeSpace);
                    fieldNames.Append(workExperienceName);
                    fieldNames.Append(' ', workExperienceLength - workExperienceName.Length);
                    break;
                case favLetterWord:
                    var favLetterName = "FavLetter";
                    fieldWriters.Add(WriteFavLetter);
                    borderLine.Append('-', favLetterLength + freeSpace);
                    fieldNames.Append(favLetterName);
                    fieldNames.Append(' ', favLetterLength - favLetterName.Length);
                    break;
                default:
                    const string wrongSytaxError = "Wrong command syntax!";
                    Console.WriteLine(wrongSytaxError);
                    return;
            }

            fieldNames.Append(" |");
            borderLine.Append('+');
        }

        Console.WriteLine(borderLine);
        Console.WriteLine(fieldNames);
        Console.WriteLine(borderLine);

        foreach (var record in records)
        {
            Console.Write('|');
            foreach (var fieldWriter in fieldWriters)
            {
                fieldWriter(record);
            }

            Console.WriteLine();
            Console.WriteLine(borderLine);
        }

        void WriteId(FileCabinetRecord record)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            string id = record.Id.ToString();
            stringBuilder.Append(' ', idLength - id.Length);
            stringBuilder.Append(id);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }

        void WriteFirstName(FileCabinetRecord record)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            stringBuilder.Append(record.FirstName);
            stringBuilder.Append(' ', firstNameLength - record.FirstName.Length);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }

        void WriteLastName(FileCabinetRecord record)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            stringBuilder.Append(record.LastName);
            stringBuilder.Append(' ', lastNameLength - record.LastName.Length);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }

        void WriteDateOfBirth(FileCabinetRecord record)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            string dateOfBirth = record.DateOfBirth.ToString("yyyy-MMM-dd", CultureInfo.InvariantCulture);
            stringBuilder.Append(' ', dateOfBirthLength - dateOfBirth.Length);
            stringBuilder.Append(dateOfBirth);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }

        void WriteBalance(FileCabinetRecord record)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            string balance = record.Balance.ToString();
            stringBuilder.Append(' ', balanceLength - balance.Length);
            stringBuilder.Append(balance);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }

        void WriteWorkExperience(FileCabinetRecord record)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            string workExperience = record.WorkExperience.ToString();
            stringBuilder.Append(' ', workExperienceLength - workExperience.Length);
            stringBuilder.Append(workExperience);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }

        void WriteFavLetter(FileCabinetRecord record)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            string favLetter = record.FavLetter.ToString();
            stringBuilder.Append(favLetter);
            stringBuilder.Append(' ', favLetterLength - favLetter.Length);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }
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