namespace FileCabinetApp.Validators.FullRecordValidators;

/// <summary>
/// The validator builder.
/// </summary>
internal class ValidatorBuilder
{
    private readonly List<IRecordValidator> validators = new List<IRecordValidator>();

    /// <summary>
    /// Adds FirstNameValidator.
    /// </summary>
    /// <param name="minLength">Minimal length.</param>
    /// <param name="maxLength">Maximal length.</param>
    /// <returns>This validator builder.</returns>
    public ValidatorBuilder ValidateFirstName(int minLength, int maxLength)
    {
        this.validators.Add(new FirstNameValidator(minLength, maxLength));
        return this;
    }

    /// <summary>
    /// Adds LastNameValidator.
    /// </summary>
    /// <param name="minLength">Minimal length.</param>
    /// <param name="maxLength">Maximal length.</param>
    /// <returns>This validator builder.</returns>
    public ValidatorBuilder ValidateLastName(int minLength, int maxLength)
    {
        this.validators.Add(new LastNameValidator(minLength, maxLength));
        return this;
    }

    /// <summary>
    /// Adds DateOfBirthValidator.
    /// </summary>
    /// <param name="from">Minimal date.</param>
    /// <param name="to">Maximal date.</param>
    /// <returns>This validator builder.</returns>
    public ValidatorBuilder ValidateDateOfBirth(DateTime from, DateTime to)
    {
        this.validators.Add(new DateOfBirthValidator(from, to));
        return this;
    }

    /// <summary>
    /// Adds BalanceValidator.
    /// </summary>
    /// <param name="minValue">Minimal value.</param>
    /// <param name="maxValue">Maximal value.</param>
    /// <returns>This validator builder.</returns>
    public ValidatorBuilder ValidateBalance(decimal minValue, decimal maxValue)
    {
        this.validators.Add(new BalanceValidator(minValue, maxValue));
        return this;
    }

    /// <summary>
    /// Adds WorkExperienceValidator.
    /// </summary>
    /// <param name="minValue">Minimal value.</param>
    /// <param name="maxValue">Maximal value.</param>
    /// <returns>This validator builder.</returns>
    public ValidatorBuilder ValidateWorkExperience(short minValue, short maxValue)
    {
        this.validators.Add(new WorkExperienceValidator(minValue, maxValue));
        return this;
    }

    /// <summary>
    /// Adds FavLetterValidator.
    /// </summary>
    /// <returns>This validator builder.</returns>
    public ValidatorBuilder ValidateFavLetter()
    {
        this.validators.Add(new FavLetterValidator());
        return this;
    }

    /// <summary>
    /// Creates new CompositeValidator with added validators.
    /// </summary>
    /// <returns>New CompositeValidator.</returns>
    public CompositeValidator Create() => new CompositeValidator(this.validators);
}
