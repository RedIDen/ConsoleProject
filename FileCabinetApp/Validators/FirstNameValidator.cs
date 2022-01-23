namespace FileCabinetApp.Validators;

public class FirstNameValidator : IRecordValidator
{
    private int minLength;

    private int maxLength;

    public FirstNameValidator(int minLength, int maxLength)
    {
        this.minLength = minLength;
        this.maxLength = maxLength;
    }

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
