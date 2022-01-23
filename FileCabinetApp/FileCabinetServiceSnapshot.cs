namespace FileCabinetApp;

/// <summary>
/// The file cabinet service snapshot.
/// </summary>
public class FileCabinetServiceSnapshot
{
    private ReadOnlyCollection<FileCabinetRecord> list;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
    /// </summary>
    /// <param name="list">The list of records.</param>
    public FileCabinetServiceSnapshot(List<FileCabinetRecord> list)
    {
        this.list = new ReadOnlyCollection<FileCabinetRecord>(list);
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCabinetServiceSnapshot"/> class.
    /// </summary>
    public FileCabinetServiceSnapshot()
    {
        this.list = new ReadOnlyCollection<FileCabinetRecord>(new List<FileCabinetRecord>());
    }

    /// <summary>
    /// Saves the snapshot to the CSV file.
    /// </summary>
    /// <param name="stream">The stream to work with.</param>
    public void SaveToCsv(StreamWriter stream)
    {
        var fileCabinetRecordCsvWriter = new FileCabinetRecordCsvWriter(stream);

        foreach (var record in this.list)
        {
            fileCabinetRecordCsvWriter.Write(record);
        }

        fileCabinetRecordCsvWriter.Close();
    }

    /// <summary>
    /// Saves the snapshot to the XML file.
    /// </summary>
    /// <param name="stream">The stream to work with.</param>
    public void SaveToXml(StreamWriter stream)
    {
        var fileCabinetRecordXmlWriter = new FileCabinetRecordXmlWriter(stream);

        fileCabinetRecordXmlWriter.Write(this.list);

        fileCabinetRecordXmlWriter.Close();
    }

    /// <summary>
    /// Loads snapshot from CSV.
    /// </summary>
    /// <param name="stream">The stream to work with.</param>
    public void LoadFromCsv(FileStream stream)
    {
        var fileCabinetRecordCsvReader = new FileCabinetRecordCsvReader(new StreamReader(stream));

        this.list = fileCabinetRecordCsvReader.ReadAll();
    }

    /// <summary>
    /// Loads snapshot from XML.
    /// </summary>
    /// <param name="stream">The stream to work with.</param>
    public void LoadFromXml(FileStream stream)
    {
        var fileCabinetRecordXmlReader = new FileCabinetRecordXmlReader(new StreamReader(stream));

        this.list = fileCabinetRecordXmlReader.ReadAll();
    }

    /// <summary>
    /// Returns the list of records.
    /// </summary>
    /// <returns>The list of records.</returns>
    public ReadOnlyCollection<FileCabinetRecord> GetRecords() => this.list;
}