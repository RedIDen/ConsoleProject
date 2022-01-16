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
        Program.fileCabinetService = new FileCabinetTrasferHelper(new ServiceMeter(new FileCabinetMemoryService(Program.validators.GetValueOrDefault("default"))));
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

        var edit = new EditCommandHandler(Program.fileCabinetService);
        edit.SetNext(exit);

        var export = new ExportCommandHandler(Program.fileCabinetService);
        export.SetNext(edit);

        var find = new FindCommandHandler(Program.fileCabinetService, Program.DefaultRecordPrinter);
        find.SetNext(export);

        var help = new HelpCommandHandler();
        help.SetNext(find);

        var import = new ImportCommandHandler(Program.fileCabinetService);
        import.SetNext(help);

        var list = new ListCommandHandler(Program.fileCabinetService, Program.DefaultRecordPrinter);
        list.SetNext(import);

        var purge = new PurgeCommandHandler(Program.fileCabinetService);
        purge.SetNext(list);

        var remove = new RemoveCommandHandler(Program.fileCabinetService);
        remove.SetNext(purge);

        var stat = new StatCommandHandler(Program.fileCabinetService);
        stat.SetNext(remove);

        var storage = new StorageCommandHandler(Program.fileCabinetService);
        storage.SetNext(stat);

        var validation = new ValidationRulesCommandHandler(Program.fileCabinetService, Program.validators);
        validation.SetNext(storage);

        var create = new CreateCommandHandler(Program.fileCabinetService);
        create.SetNext(validation);

        return create;
    }

    private static void DefaultRecordPrinter(IEnumerable<FileCabinetRecord> records, string errorMessage)
    {
        if (records.Count() == 0)
        {
            Console.WriteLine(errorMessage);
            return;
        }

        foreach (var record in records)
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