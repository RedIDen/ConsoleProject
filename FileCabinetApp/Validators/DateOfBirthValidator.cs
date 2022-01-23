namespace FileCabinetApp.Validators;

public class DateOfBirthValidator : IRecordValidator
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

        if (value < this.from || value > this.to)
        {
            return (false, $"the date should be between {this.from} and {this.to}");
        }
        else
        {
            return (true, string.Empty);
        }
    }
}
