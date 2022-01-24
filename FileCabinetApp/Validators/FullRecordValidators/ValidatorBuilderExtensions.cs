namespace FileCabinetApp.Validators.FullRecordValidators;

/// <summary>
/// The validator builder extensions.
/// </summary>
internal static class ValidatorBuilderExtensions
{
    /// <summary>
    /// Creates default validator.
    /// </summary>
    /// <param name="validatorBuilder">This ValidatorBuilder.</param>
    /// <returns>Default validator.</returns>
    public static CompositeValidator CreateDefault(this ValidatorBuilder validatorBuilder) => validatorBuilder
        .ValidateFirstName(3, 60)
        .ValidateLastName(3, 60)
        .ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now)
        .ValidateFavLetter()
        .ValidateBalance(0, decimal.MaxValue)
        .ValidateWorkExperience(0, short.MaxValue)
        .Create();

    /// <summary>
    /// Creates custom validator.
    /// </summary>
    /// <param name="validatorBuilder">This ValidatorBuilder.</param>
    /// <returns>Custom validator.</returns>
    public static CompositeValidator CreateCustom(this ValidatorBuilder validatorBuilder) => validatorBuilder
        .ValidateFirstName(0, 50)
        .ValidateLastName(0, 50)
        .ValidateDateOfBirth(new DateTime(1900, 1, 1), DateTime.Now)
        .ValidateFavLetter()
        .ValidateBalance(0, 100)
        .ValidateWorkExperience(0, 100)
        .Create();
}
