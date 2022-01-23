namespace FileCabinetApp.Validators;

internal class WorkExperienceValidator : IRecordValidator
{
    private short minValue;

    private short maxValue;

    public WorkExperienceValidator(short minValue, short maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

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
