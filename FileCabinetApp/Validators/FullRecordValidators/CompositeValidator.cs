namespace FileCabinetApp.Validators.FullRecordValidators;

[JsonObject(MemberSerialization.Fields)]
public class CompositeValidator : IRecordValidator
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

    public (bool, string) ValidateFirstName(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is FirstNameValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    public (bool, string) ValidateLastName(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is LastNameValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    public (bool, string) ValidateDateOfBirth(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is DateOfBirthValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    public (bool, string) ValidateBalance(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is BalanceValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    public (bool, string) ValidateWorkExperience(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is WorkExperienceValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }

    public (bool, string) ValidateFavLeter(FileCabinetRecord record)
    {
        var validator = this.validators.FirstOrDefault(x => x is FavLetterValidator);
        return validator is null ? (true, string.Empty) : validator.Validate(record);
    }
}
