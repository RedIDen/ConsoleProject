namespace FileCabinetApp.Validators;

/// <summary>
/// The DateOfBirth validator.
/// </summary>
internal class DateOfBirthValidator : IRecordValidator
{
    private readonly DateTime from;

    private readonly DateTime to;

    /// <summary>
    /// Initializes a new instance of the <see cref="DateOfBirthValidator"/> class.
    /// </summary>
    /// <param name="from">Minimal date.</param>
    /// <param name="to">Maximal date.</param>
    public DateOfBirthValidator(DateTime from, DateTime to)
    {
        this.from = from;
        this.to = to;
    }

    /// <summary>
    /// Validates the record's date of birth.
    /// </summary>
    /// <param name="record">Record.</param>
    /// <returns>The flag showing if validation is succesful and the error message.</returns>
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