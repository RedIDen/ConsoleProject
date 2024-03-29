﻿global using System.Collections;
global using System.Collections.Generic;
global using System.Collections.ObjectModel;
global using System.Globalization;
global using System.Text;
global using FileCabinetApp.CommandHandlers;
global using FileCabinetApp.FileCabinet;
global using FileCabinetApp.FileCabinet.Decorators;
global using FileCabinetApp.FileCabinet.WorkingWithFiles;
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
internal static class Program
{
    private const string HintMessage = "Enter your command, or enter 'help' to get help.";
    private const string DeveloperName = "Deniska Vasilyev";
    private const string ValidatorsDataPath = "validation-rules.json";

    private static string validationRulesMessage = "Using default validation rules.";
    private static string storageTypeMessage = "Using memory storage.";

    private static Dictionary<string, CompositeValidator> validators;
    private static FileCabinetTrasferHelper fileCabinetService;

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

    private static ICommandHandler CreateCommandHandler()
    {
        var exit = new ExitCommandHandler((value) => Program.isRunning = value);

        var update = new UpdateCommandHandler(Program.fileCabinetService);
        update.SetNext(exit);

        var export = new ExportCommandHandler(Program.fileCabinetService);
        export.SetNext(update);

        var help = new HelpCommandHandler();
        help.SetNext(export);

        var import = new ImportCommandHandler(Program.fileCabinetService);
        import.SetNext(help);

        var select = new SelectCommandHandler(Program.fileCabinetService, Program.DefaultRecordPrinter);
        select.SetNext(import);

        var purge = new PurgeCommandHandler(Program.fileCabinetService);
        purge.SetNext(select);

        var delete = new DeleteCommandHandler(Program.fileCabinetService);
        delete.SetNext(purge);

        var stat = new StatCommandHandler(Program.fileCabinetService);
        stat.SetNext(delete);

        var storage = new StorageCommandHandler(Program.fileCabinetService, (value) => Program.storageTypeMessage = value);
        storage.SetNext(stat);

        var validation = new ValidationRulesCommandHandler(Program.fileCabinetService, Program.validators, (value) => Program.validationRulesMessage = value);
        validation.SetNext(storage);

        var insert = new InsertCommandHandler(Program.fileCabinetService);
        insert.SetNext(validation);

        var useStopwatch = new UseStopwatchCommandHandler(Program.fileCabinetService);
        useStopwatch.SetNext(insert);

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

        byte favLetterLength = (byte)favLetterWord.Length;
        byte idLength, firstNameLength, lastNameLength, dateOfBirthLength, balanceLength, workExperienceLength;

        StringBuilder borderLine = new StringBuilder().Append('+');
        StringBuilder fieldNames = new StringBuilder().Append('|');
        foreach (var parameter in parameters)
        {
            fieldNames.Append(' ');

            switch (parameter.ToLower(CultureInfo.InvariantCulture))
            {
                case idWord:
                    var idName = "Id";
                    idLength = (byte)Math.Max(records.Select(x => x.Id).Max().ToString(CultureInfo.InvariantCulture).Length, idName.Length);
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
                    dateOfBirthLength = (byte)Math.Max("yyyy-MMM-dd".Length, dateOfBirthName.Length);
                    fieldWriters.Add(WriteDateOfBirth);
                    borderLine.Append('-', dateOfBirthLength + freeSpace);
                    fieldNames.Append(dateOfBirthName);
                    fieldNames.Append(' ', dateOfBirthLength - dateOfBirthName.Length);
                    break;
                case balanceWord:
                    var balanceName = "Balance";
                    balanceLength = (byte)Math.Max(records.Select(x => x.Balance).Max().ToString(CultureInfo.InvariantCulture).Length, balanceName.Length);
                    fieldWriters.Add(WriteBalance);
                    borderLine.Append('-', balanceLength + freeSpace);
                    fieldNames.Append(balanceName);
                    fieldNames.Append(' ', balanceLength - balanceName.Length);
                    break;
                case workExperienceWord:
                    var workExperienceName = "WorkExperience";
                    workExperienceLength = (byte)Math.Max(records.Select(x => x.WorkExperience).Max().ToString(CultureInfo.InvariantCulture).Length, workExperienceName.Length);
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
            string id = record.Id.ToString(CultureInfo.InvariantCulture);
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
            string balance = record.Balance.ToString(CultureInfo.InvariantCulture);
            stringBuilder.Append(' ', balanceLength - balance.Length);
            stringBuilder.Append(balance);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }

        void WriteWorkExperience(FileCabinetRecord record)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            string workExperience = record.WorkExperience.ToString(CultureInfo.InvariantCulture);
            stringBuilder.Append(' ', workExperienceLength - workExperience.Length);
            stringBuilder.Append(workExperience);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }

        void WriteFavLetter(FileCabinetRecord record)
        {
            var stringBuilder = new StringBuilder();
            stringBuilder.Append(' ');
            string favLetter = (record.FavLetter == '\0' ? '-' : record.FavLetter).ToString();
            stringBuilder.Append(favLetter);
            stringBuilder.Append(' ', favLetterLength - favLetter.Length);
            stringBuilder.Append(" |");
            Console.Write(stringBuilder.ToString());
        }
    }
}