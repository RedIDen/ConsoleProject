namespace FileCabinetApp.Validators.FullRecordValidators;

[JsonObject(MemberSerialization.Fields)]
internal class CompositeValidator : IRecordValidator
{
    private readonly List<IRecordValidator> validators;

    public CompositeValidator(IEnumerable<IRecordValidator> validators)
    {
        this.validators = validators.ToList();
    }

    public (bool, string) Validate(FileCabinetRecord record)
    {
        bool result = true;
        StringBuilder errorMessage = new StringBuilder();
        bool tempResult;
        string tempMessage;

        foreach (var validator in this.validators)
        {
            (tempResult, tempMessage) = validator.Validate(record);
            result &= tempResult;
            errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + ", ");
        }

        return (result, errorMessage.ToString().Trim(' ', ','));
    }

    public (bool, string) ValidateInitializedFields(FileCabinetRecord record)
    {
        bool isValid = true;
        var errorMessage = new StringBuilder();
        bool tempResult;
        string tempMessage;

        if (record.FirstName is not null)
        {
            (tempResult, tempMessage) = this.ValidateFirstName(record);
            isValid &= tempResult;
            errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + ", ");
        }

        if (record.LastName is not null)
        {
            (tempResult, tempMessage) = this.ValidateLastName(record);
            isValid &= tempResult;
            errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + ", ");
        }

        if (record.DateOfBirth != new DateTime(0))
        {
            (tempResult, tempMessage) = this.ValidateDateOfBirth(record);
            isValid &= tempResult;
            errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + ", ");
        }

        if (record.Balance != -1)
        {
            (tempResult, tempMessage) = this.ValidateBalance(record);
            isValid &= tempResult;
            errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + ", ");
        }

        if (record.WorkExperience != -1)
        {
            (tempResult, tempMessage) = this.ValidateWorkExperience(record);
            isValid &= tempResult;
            errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + ", ");
        }

        if (record.FavLetter != '\0')
        {
            (tempResult, tempMessage) = this.ValidateFavLeter(record);
            isValid &= tempResult;
            errorMessage.Append(tempMessage.Length == 0 ? tempMessage : tempMessage + ", ");
        }

        return (isValid, errorMessage.ToString().Trim(' ', ','));
    }

    private (bool, string) ValidateFirstName(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is FirstNameValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    private (bool, string) ValidateLastName(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is LastNameValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    private (bool, string) ValidateDateOfBirth(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is DateOfBirthValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    private (bool, string) ValidateBalance(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is BalanceValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    private (bool, string) ValidateWorkExperience(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is WorkExperienceValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    private (bool, string) ValidateFavLeter(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is FavLetterValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }
}
