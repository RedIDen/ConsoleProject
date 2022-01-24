namespace FileCabinetApp.Validators;

/// <summary>
/// The favorite letter validator.
/// </summary>
internal class FavLetterValidator : IRecordValidator
{
    /// <summary>
    /// Validates the record's favorite letter.
    /// </summary>
    /// <param name="record">Record.</param>
    /// <returns>The flag showing if validation is succesful and the error message.</returns>
    public (bool, string) Validate(FileCabinetRecord record)
    {
        var value = record.FavLetter;

        if (value == '\0')
        {
            return (true, string.Empty);
        }

        if (!char.IsLetter(value))
        {
            return (false, "enter the correct letter");
        }

        return (true, string.Empty);
    }
}