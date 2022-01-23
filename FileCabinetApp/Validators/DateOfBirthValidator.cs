namespace FileCabinetApp.Validators;

internal class DateOfBirthValidator : IRecordValidator
{
    private DateTime from;

    private DateTime to;

    public DateOfBirthValidator(DateTime from, DateTime to)
    {
        this.from = from;
        this.to = to;
    }

    public (bool, string) Validate(FileCabinetRecord record)
    {
        var value = record.DateOfBirth;

        if (value == new DateTime(0))
        {
            return (true, string.Empty);
        }

        if (value < this.from || value > this.to)
        {
            var provider = CultureInfo.InvariantCulture;
            var format = "yyyy-MMM-dd";
            return (false, $"the date should be between {this.from.ToString(format, provider)} and {this.to.ToString(format, provider)}");
        }

        return (true, string.Empty);
    }
}