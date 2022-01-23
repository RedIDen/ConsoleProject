namespace FileCabinetApp.Validators;

    public class FavLetterValidator : IRecordValidator
{
    public (bool, string) Validate(FileCabinetRecord record)
    {
        var value = record.FavLetter;

        if (!char.IsLetter(value))
        {
            return (false, "enter the correct letter");
        }
        else
        {
            return (true, string.Empty);
        }
    }
}
