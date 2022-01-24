namespace FileCabinetApp.FileCabinet.WorkingWithFiles;

/// <summary>
/// The file cabinet record CSV writer.
/// </summary>
public class FileCabinetRecordCsvWriter
{
    private readonly TextWriter textWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCabinetRecordCsvWriter"/> class.
    /// </summary>
    /// <param name="textWriter">The TextWriter object to work with.</param>
    public FileCabinetRecordCsvWriter(TextWriter textWriter)
    {
        this.textWriter = textWriter;
    }

    /// <summary>
    /// Wtites the record into the file.
    /// </summary>
    /// <param name="record">The record to wtire.</param>
    public void Write(FileCabinetRecord record)
    {
        var stringBuilder = new StringBuilder();

        stringBuilder.Append(CultureInfo.InvariantCulture, $"{record.Id},");
        stringBuilder.Append(CultureInfo.InvariantCulture, $"{record.FirstName},");
        stringBuilder.Append(CultureInfo.InvariantCulture, $"{record.LastName},");
        stringBuilder.Append(CultureInfo.InvariantCulture, $"{record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"))},");
        stringBuilder.Append(CultureInfo.InvariantCulture, $"{record.Balance},");
        stringBuilder.Append(CultureInfo.InvariantCulture, $"{record.FavLetter},");
        stringBuilder.Append(CultureInfo.InvariantCulture, $"{record.WorkExperience}");
        stringBuilder.Append(Environment.NewLine);

        this.textWriter.Write(stringBuilder.ToString());
    }

    /// <summary>
    /// Closes the current writer.
    /// </summary>
    public void Close()
    {
        this.textWriter.Close();
    }
}