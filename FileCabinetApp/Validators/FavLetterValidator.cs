namespace FileCabinetApp.Validators;

public class FavLetterValidator : IRecordValidator
{
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