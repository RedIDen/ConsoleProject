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
    protected FileCabinetTrasferHelper service;

    public ServiceCommandHandlerBase(FileCabinetTrasferHelper service)
        : base()
    {
        this.service = service;
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
    /// Converter to int.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result.</returns>
    protected (bool, string, int) IntConverter(string value)
    {
        if (int.TryParse(value, out int result))
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
        if (decimal.TryParse(value, NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture.NumberFormat, out decimal result))
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

    protected (bool, string) TryInsertId(FileCabinetRecord record, string value)
    {
        var result = this.IntConverter(value);
        if (result.Item1)
        {
            record.Id = result.Item3;
        }

        if (this.service.Service.FindRecordIndexById(result.Item3) != -1)
        {
            string thisIdAlreadyExistsError = "this id is already taken";
            return (false, thisIdAlreadyExistsError);
        }

        return (result.Item1, result.Item2);
    }

    protected (bool, string) TryInsertFirstName(FileCabinetRecord record, string value)
    {
        var result = this.StringConverter(value);
        if (result.Item1)
        {
            record.FirstName = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    protected (bool, string) TryInsertLastName(FileCabinetRecord record, string value)
    {
        var result = this.StringConverter(value);
        if (result.Item1)
        {
            record.LastName = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    protected (bool, string) TryInsertDateOfBirth(FileCabinetRecord record, string value)
    {
        var result = this.DateConverter(value);
        if (result.Item1)
        {
            record.DateOfBirth = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    protected (bool, string) TryInsertBalance(FileCabinetRecord record, string value)
    {
        var result = this.DecimalConverter(value);
        if (result.Item1)
        {
            record.Balance = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    protected (bool, string) TryInsertWorkExperience(FileCabinetRecord record, string value)
    {
        var result = this.ShortConverter(value);
        if (result.Item1)
        {
            record.WorkExperience = result.Item3;
        }

        return (result.Item1, result.Item2);
    }

    protected (bool, string) TryInsertFavLetter(FileCabinetRecord record, string value)
    {
        var result = this.CharConverter(value);
        if (result.Item1)
        {
            record.FavLetter = result.Item3;
        }

        return (result.Item1, result.Item2);
    }
}