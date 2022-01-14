using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.CommandHandlers;
public abstract class ServiceCommandHandlerBase : CommandHandlerBase
{
    protected FileCabinetServiceTransferHelper fileCabinetServiceTransferHelper;

    public ServiceCommandHandlerBase(FileCabinetServiceTransferHelper fileCabinetServiceTransferHelper)
    {
        this.fileCabinetServiceTransferHelper = fileCabinetServiceTransferHelper;
    }

    protected FileCabinetRecord ReadDataForRecord()
    {
        var record = new FileCabinetRecord();

        do
        {
            Console.Write("First name: ");
            record.FirstName = this.ReadInput(this.StringConverter);

            Console.Write("Last name: ");
            record.LastName = this.ReadInput(this.StringConverter);

            Console.Write("Date of birth: ");
            record.DateOfBirth = this.ReadInput(this.DateConverter);

            Console.Write("Work experience: ");
            record.WorkExperience = this.ReadInput(this.ShortConverter);

            Console.Write("Balance: ");
            record.Balance = this.ReadInput(this.DecimalConverter);

            Console.Write("Favorite letter: ");
            record.FavLetter = this.ReadInput(this.CharConverter);

            var validationResult = this.fileCabinetServiceTransferHelper.fileCabinetService.Validator.Validate(record);
            if (!validationResult.Item1)
            {
                Console.WriteLine($"Validation failed: {validationResult.Item2}. Please, correct your input.");
                continue;
            }

            return record;
        }
        while (true);
    }

    protected T ReadInput<T>(Func<string, ValueTuple<bool, string, T>> converter)
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
}