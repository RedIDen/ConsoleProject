namespace FileCabinetApp.Validators;

/// <summary>
/// The intarface for the record validators.
/// </summary>
internal interface IRecordValidator
{
    public (bool, string) Validate(FileCabinetRecord record);
}