using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp;

/// <summary>
/// The class overriding FileCabinetService.ValidateParameters method.
/// </summary>
public class DefaultValidator : IRecordValidator
{
    /// <summary>
    /// Validates the first name.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result of validation.</returns>
    public (bool, string) FirstNameValidator(string value)
    {
        if (value is null)
        {
            return (false, "the name is null");
        }
        else if (value.Length > 60)
        {
            return (false, "the name is too long");
        }
        else if (value.Length < 2)
        {
            return (false, "the name is too short");
        }
        else if (value.Trim().Length == 0)
        {
            return (false, "the name can't consist of only whitespaces");
        }
        else
        {
            return (true, string.Empty);
        }
    }

    /// <summary>
    /// Validates the second name.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result of validation.</returns>
    public (bool, string) LastNameValidator(string value) => this.FirstNameValidator(value);

    /// <summary>
    /// Validates the date of birth.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result of validation.</returns>
    public (bool, string) DateOfBirthValidator(DateTime value)
    {
        if (value < new DateTime(1950, 1, 1) || value > DateTime.Now)
        {
            return (false, "the date should be between 1950-Jan-1 and today");
        }
        else
        {
            return (true, string.Empty);
        }
    }

    /// <summary>
    /// Validates the work experience.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result of validation.</returns>
    public (bool, string) WorkExperienceValidator(short value)
    {
        if (value < 0)
        {
            return (false, "the work experience can't be less than 0");
        }
        else
        {
            return (true, string.Empty);
        }
    }

    /// <summary>
    /// Validates the balance.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result of validation.</returns>
    public (bool, string) BalanceValidator(decimal value)
    {
        if (value < 0)
        {
            return (false, "the balance can't be less than 0");
        }
        else
        {
            return (true, string.Empty);
        }
    }

    /// <summary>
    /// Validates the favorite letter.
    /// </summary>
    /// <param name="value">Value.</param>
    /// <returns>Result of validation.</returns>
    public (bool, string) FavLetterValidator(char value)
    {
        if (!char.IsLetter(value))
        {
            return (false, "enter the correct letter");
        }
        else
        {
            return (true, string.Empty);
        }
    }

    /// <summary>
    /// Validates the whole record.
    /// </summary>
    /// <param name="record">Record.</param>
    /// <returns>Result of validation.</returns>
    public (bool, string) RecordValidator(FileCabinetRecord record)
    {
        bool result = true;
        StringBuilder errorMessage = new StringBuilder();
        bool tempResult;
        string tempMessage;

        (tempResult, tempMessage) = this.FirstNameValidator(record.FirstName);
        result &= tempResult;
        errorMessage.Append(tempMessage);

        (tempResult, tempMessage) = this.LastNameValidator(record.LastName);
        result &= tempResult;
        errorMessage.Append(tempMessage);

        (tempResult, tempMessage) = this.DateOfBirthValidator(record.DateOfBirth);
        result &= tempResult;
        errorMessage.Append(tempMessage);

        (tempResult, tempMessage) = this.BalanceValidator(record.Balance);
        result &= tempResult;
        errorMessage.Append(tempMessage);

        (tempResult, tempMessage) = this.FavLetterValidator(record.FavLetter);
        result &= tempResult;
        errorMessage.Append(tempMessage);

        (tempResult, tempMessage) = this.WorkExperienceValidator(record.WorkExperience);
        result &= tempResult;
        errorMessage.Append(tempMessage);

        return (result, errorMessage.ToString());
    }
}