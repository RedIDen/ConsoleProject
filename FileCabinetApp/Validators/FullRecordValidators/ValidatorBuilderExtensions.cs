namespace FileCabinetApp.Validators.FullRecordValidators;

internal static class ValidatorBuilderExtensions
{
    public static CompositeValidator CreateDefault(this ValidatorBuilder validatorBuilder) => validatorBuilder
        .ValidateFirstName(3, 60)
        .ValidateLastName(3, 60)
        .ValidateDateOfBirth(new DateTime(1950, 1, 1), DateTime.Now)
        .ValidateFavLetter()
        .ValidateBalance(0, decimal.MaxValue)
        .ValidateWorkExperience(0, short.MaxValue)
        .Create();

    public static CompositeValidator CreateCustom(this ValidatorBuilder validatorBuilder) => validatorBuilder
        .ValidateFirstName(0, 50)
        .ValidateLastName(0, 50)
        .ValidateDateOfBirth(new DateTime(1900, 1, 1), DateTime.Now)
        .ValidateFavLetter()
        .ValidateBalance(0, 100)
        .ValidateWorkExperience(0, 100)
        .Create();
}
