namespace FileCabinetApp.Validators.FullRecordValidators;

public class ValidatorBuilder
{
    private List<IRecordValidator> validators = new List<IRecordValidator>();

    public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
    {
        this.validators.Add(new FirstNameValidator(minLength, maxLength));
        return this;
    }

    public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
    {
        this.validators.Add(new LastNameValidator(minLength, maxLength));
        return this;
    }

    public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
    {
        this.validators.Add(new DateOfBirthValidator(from, to));
        return this;
    }

    public ValidatorBuilder ValidateBalance(decimal minValue, decimal maxValue)
    {
        this.validators.Add(new BalanceValidator(minValue, maxValue));
        return this;
    }

    public ValidatorBuilder ValidateWorkExperience(short minValue, short maxValue)
    {
        this.validators.Add(new WorkExperienceValidator(minValue, maxValue));
        return this;
    }

    public ValidatorBuilder ValidateFavLetter()
    {
        this.validators.Add(new FavLetterValidator());
        return this;
    }

    public CompositeValidator Create() => new CompositeValidator(this.validators);
}
