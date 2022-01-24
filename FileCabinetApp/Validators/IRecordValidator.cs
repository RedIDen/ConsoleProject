namespace FileCabinetApp.Validators;

/// <summary>
/// The intarface for the record validators.
/// </summary>
internal interface IRecordValidator
{
    /// <summary>
    /// Validates the record.
    /// </summary>
    /// <param name="record">Record.</param>
    /// <returns>The flag showing if validation is succesful and the error message.</returns>
    public (bool, string) Validate(FileCabinetRecord record);
}