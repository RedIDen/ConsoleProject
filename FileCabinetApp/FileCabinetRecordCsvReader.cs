#pragma warning disable CS8602

namespace FileCabinetApp;

/// <summary>
/// The file cabinet record CSV reader.
/// </summary>
public class FileCabinetRecordCsvReader
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
                Id = int.Parse(data[0]),
                FirstName = data[1],
                LastName = data[2],
                DateOfBirth = DateTime.Parse(data[3], CultureInfo.CreateSpecificCulture("en-US"), DateTimeStyles.None),
                Balance = decimal.Parse(data[4]),
                FavLetter = char.Parse(data[5]),
                WorkExperience = short.Parse(data[6]),
            });
        }

        return new ReadOnlyCollection<FileCabinetRecord>(list);
    }
}