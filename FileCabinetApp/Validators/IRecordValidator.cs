namespace FileCabinetApp.Validators;

/// <summary>
/// The intarface for the record validators.
/// </summary>
public interface IRecordValidator
{
    public (bool, string) Validate(FileCabinetRecord record);
}