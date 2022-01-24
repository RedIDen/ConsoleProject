namespace FileCabinetApp.Validators;

/// <summary>
/// The LastName validator.
/// </summary>
internal class LastNameValidator : IRecordValidator
{
    private readonly int minLength;

    private readonly int maxLength;

    /// <summary>
    /// Initializes a new instance of the <see cref="LastNameValidator"/> class.
    /// </summary>
    /// <param name="minLength">Minimal length.</param>
    /// <param name="maxLength">Maximal length.</param>
    public LastNameValidator(int minLength, int maxLength)
    {
        this.minLength = minLength;
        this.maxLength = maxLength;
    }

    /// <summary>
    /// Validates the record's last name.
    /// </summary>
    /// <param name="record">Record.</param>
    /// <returns>The flag showing if validation is succesful and the error message.</returns>
    public (bool, string) Validate(FileCabinetRecord record)
    {
        var value = record.LastName;

        if (string.IsNullOrWhiteSpace(value))
        {
            return (false, "the last name is null or consists of only whitespaces");
        }

        if (value.Length > this.maxLength)
        {
            return (false, "the last name is too long");
        }

        if (value.Length < this.minLength)
        {
            return (false, "the last name is too short");
        }

        return (true, string.Empty);
    }
}
