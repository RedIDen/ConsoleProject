#pragma warning disable CS8602
#pragma warning disable CS8600

using System.Collections.ObjectModel;
using System.Globalization;
using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp;

/// <summary>
/// The file cabinet record XML reader.
/// </summary>
public class FileCabinetRecordXmlReader
{
    private readonly XmlSerializer xmlSerializer;
    private readonly StreamReader stream;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCabinetRecordXmlReader"/> class.
    /// </summary>
    /// <param name="stream">The stream to work with.</param>
    public FileCabinetRecordXmlReader(StreamReader stream)
    {
        this.xmlSerializer = new XmlSerializer(typeof(XmlFileCabinetList));
        this.stream = stream;
    }

    /// <summary>
    /// Reads all the records from stream.
    /// </summary>
    /// <returns>The list of records.</returns>
    public ReadOnlyCollection<FileCabinetRecord> ReadAll()
    {
        XmlFileCabinetList xmlList = (XmlFileCabinetList)this.xmlSerializer.Deserialize(this.stream);

        var list = new List<FileCabinetRecord>();

        var culture = CultureInfo.CreateSpecificCulture("en-US");
        var styles = DateTimeStyles.None;

        foreach (var element in xmlList.List)
        {
            list.Add(new FileCabinetRecord
            {
                Id = element.Id,
                FirstName = element.Name.FirstName,
                LastName = element.Name.LastName,
                DateOfBirth = DateTime.Parse(element.DateOfBirth, culture, styles),
                Balance = element.Balance,
                WorkExperience = element.WorkExperience,
                FavLetter = element.FavLetter,
            });
        }

        return new ReadOnlyCollection<FileCabinetRecord>(list);
    }
}