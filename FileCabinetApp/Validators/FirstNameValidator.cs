namespace FileCabinetApp.Validators;

/// <summary>
/// The FirstName validator.
/// </summary>
internal class FirstNameValidator : IRecordValidator
{
    private readonly int minLength;

    private readonly int maxLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="FirstNameValidator"/> class.
    /// </summary>
    /// <param name="minLength">Minimal length.</param>
    /// <param name="maxLength">Maximal length.</param>
    public FirstNameValidator(int minLength, int maxLength)
    {
        this.minLength = minLength;
        this.maxLength = maxLength;
    }

    /// <summary>
    /// Validates the record's first name.
    /// </summary>
    /// <param name="record">Record.</param>
    /// <returns>The flag showing if validation is succesful and the error message.</returns>
    public (bool, string) Validate(FileCabinetRecord record)
    {
        var value = record.FirstName;

        if (string.IsNullOrWhiteSpace(value))
        {
            return (false, "the first name is null or consists of only whitespaces");
        }

        if (value.Length > this.maxLength)
        {
            return (false, "the first name is too long");
        }

        if (value.Length < this.minLength)
        {
            return (false, "the first name is too short");
        }

        return (true, string.Empty);
    }
}
