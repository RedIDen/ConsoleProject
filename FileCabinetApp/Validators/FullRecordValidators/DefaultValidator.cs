using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp.Validators;

/// <summary>
/// The class overriding FileCabinetService.ValidateParameters method.
/// </summary>
public class DefaultValidator : IRecordValidator
{
    public (bool, string) Validate(FileCabinetRecord record)
    {
        bool result = true;
        StringBuilder errorMessage = new StringBuilder();
        bool tempResult;
        string tempMessage;

        (tempResult, tempMessage) = new FirstNameValidator(3, 60).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new LastNameValidator(3, 60).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new DateOfBirthValidator(new DateTime(1950, 1, 1), DateTime.Now).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new BalanceValidator(0, decimal.MaxValue).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new FavLetterValidator().Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new WorkExperienceValidator(0, short.MaxValue).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        return (result, errorMessage.ToString());
    }
}