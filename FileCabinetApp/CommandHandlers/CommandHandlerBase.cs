using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;

namespace FileCabinetApp.CommandHandlers;
public abstract class CommandHandlerBase : ICommandHandler
{
    protected ICommandHandler nextHandler;

    protected IFileCabinetService fileCabinetService;

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

    protected void ReadDataForRecord(
    out string firstName,
    out string lastName,
    out DateTime dateOfBirth,
    out short workExperience,
    out decimal balance,
    out char favLetter)
    {
        Console.Write("First name: ");
        firstName = this.ReadInput(this.StringConverter, this.fileCabinetService.Validator.FirstNameValidator);

        Console.Write("Last name: ");
        lastName = this.ReadInput(this.StringConverter, this.fileCabinetService.Validator.LastNameValidator);

        Console.Write("Date of birth: ");
        dateOfBirth = this.ReadInput(this.DateConverter, this.fileCabinetService.Validator.DateOfBirthValidator);

        Console.Write("Work experience: ");
        workExperience = this.ReadInput(this.ShortConverter, this.fileCabinetService.Validator.WorkExperienceValidator);

        Console.Write("Balance: ");
        balance = this.ReadInput(this.DecimalConverter, this.fileCabinetService.Validator.BalanceValidator);

        Console.Write("Favorite letter: ");
        favLetter = this.ReadInput(this.CharConverter, this.fileCabinetService.Validator.FavLetterValidator);
    }

    protected T ReadInput<T>(Func<string, ValueTuple<bool, string, T>> converter, Func<T, ValueTuple<bool, string>> validator)
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
    protected (bool, string, string) StringConverter(string value) => (true, string.Empty, value);

    /// <summary>
    /// Converter to date.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result.</returns>
    protected (bool, string, DateTime) DateConverter(string value)
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
    protected (bool, string, short) ShortConverter(string value)
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
    protected (bool, string, decimal) DecimalConverter(string value)
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
    protected (bool, string, char) CharConverter(string value)
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

    /// <summary>
    /// Shows the recieved list of records in the console.
    /// </summary>
    /// <param name="list">The list to show.</param>
    /// <param name="errorMessage">Error message to show if the list is empty.</param>
    protected void ShowRecords(ReadOnlyCollection<FileCabinetRecord> list, string errorMessage)
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
}