namespace FileCabinetApp.Validators;

public class LastNameValidator : IRecordValidator
{
    private int minLength;

    private int maxLength;

    public LastNameValidator(int minLength, int maxLength)
    {
        this.minLength = minLength;
        this.maxLength = maxLength;
    }

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
