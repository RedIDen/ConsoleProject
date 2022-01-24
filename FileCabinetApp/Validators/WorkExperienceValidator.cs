namespace FileCabinetApp.Validators;

/// <summary>
/// The work experience validator.
/// </summary>
internal class WorkExperienceValidator : IRecordValidator
{
    private readonly short minValue;

    private readonly short maxValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="WorkExperienceValidator"/> class.
    /// </summary>
    /// <param name="minValue">Minimal value.</param>
    /// <param name="maxValue">Maximal value.</param>
    public WorkExperienceValidator(short minValue, short maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    /// <summary>
    /// Validates the record's work experience.
    /// </summary>
    /// <param name="record">Record.</param>
    /// <returns>The flag showing if validation is succesful and the error message.</returns>
    public (bool, string) Validate(FileCabinetRecord record)
    {
        var value = record.WorkExperience;

        if (value < this.minValue)
        {
            return (false, $"the work experience can't be less than {this.minValue}");
        }

        if (value > this.maxValue)
        {
            return (false, $"the work experience can't be more than {this.maxValue}");
        }

        return (true, string.Empty);
    }
}
