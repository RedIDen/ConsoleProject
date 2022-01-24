#pragma warning disable CS8602

namespace FileCabinetApp.FileCabinet.WorkingWithFiles;

/// <summary>
/// The file cabinet record CSV reader.
/// </summary>
internal class FileCabinetRecordCsvReader
{
    private readonly StreamReader stream;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCabinetRecordCsvReader"/> class.
    /// </summary>
    /// <param name="stream">The stream to work with.</param>
    public FileCabinetRecordCsvReader(StreamReader stream)
    {
        this.stream = stream;
    }

    /// <summary>
    /// Reads all the records from stream.
    /// </summary>
    /// <returns>The list of records.</returns>
    public ReadOnlyCollection<FileCabinetRecord> ReadAll()
    {
        var list = new List<FileCabinetRecord>();

        while (!this.stream.EndOfStream)
        {
            string[] data = this.stream.ReadLine().Split(',');

            list.Add(new FileCabinetRecord
            {
                Id = int.Parse(data[0], CultureInfo.InvariantCulture),
                FirstName = data[1],
                LastName = data[2],
                DateOfBirth = DateTime.Parse(data[3], CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None),
                Balance = decimal.Parse(data[4], CultureInfo.InvariantCulture),
                FavLetter = char.Parse(data[5]),
                WorkExperience = short.Parse(data[6], CultureInfo.InvariantCulture),
            });
        }

        return new ReadOnlyCollection<FileCabinetRecord>(list);
    }
}