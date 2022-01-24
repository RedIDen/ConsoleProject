namespace FileCabinetApp.Validators;

/// <summary>
/// The balance validator.
/// </summary>
internal class BalanceValidator : IRecordValidator
{
    private readonly decimal minValue;

    private readonly decimal maxValue;

    /// <summary>
    /// Initializes a new instance of the <see cref="BalanceValidator"/> class.
    /// </summary>
    /// <param name="minValue">Minimal value.</param>
    /// <param name="maxValue">Maximal value.</param>
    public BalanceValidator(decimal minValue, decimal maxValue)
    {
        this.minValue = minValue;
        this.maxValue = maxValue;
    }

    /// <summary>
    /// Validates the record's firstname.
    /// </summary>
    /// <param name="record">Record.</param>
    /// <returns>The flag showing if validation is succesful and the error message.</returns>
    public (bool, string) Validate(FileCabinetRecord record)
    {
        var value = record.Balance;

        if (value < this.minValue)
        {
            return (false, $"the balance can't be less than {this.minValue}");
        }

        if (value > this.maxValue)
        {
            return (false, $"the balance can't be more than {this.maxValue}");
        }

        return (true, string.Empty);
    }
}
