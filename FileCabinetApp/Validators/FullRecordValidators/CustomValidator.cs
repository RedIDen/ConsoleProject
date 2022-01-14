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
public class CustomValidator : IRecordValidator
{
    public (bool, string) Validate(FileCabinetRecord record)
    {
        bool result = true;
        StringBuilder errorMessage = new StringBuilder();
        bool tempResult;
        string tempMessage;

        (tempResult, tempMessage) = new FirstNameValidator(3, 50).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new LastNameValidator(3, 50).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new DateOfBirthValidator(new DateTime(1900, 1, 1), DateTime.Now).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new BalanceValidator(0, 100).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new FavLetterValidator().Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        (tempResult, tempMessage) = new WorkExperienceValidator(0, 100).Validate(record);
        result &= tempResult;
        errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + '\n');

        return (result, errorMessage.ToString());
    }
}