using System.Xml;
using System.Xml.Serialization;

namespace FileCabinetApp;

/// <summary>
/// The file cabinet record XML writer.
/// </summary>
public class FileCabinetRecordXmlWriter
{
    private readonly XmlSerializer xmlSerializer;
    private readonly XmlWriter xmlWriter;

    /// <summary>
    /// Initializes a new instance of the <see cref="FileCabinetRecordXmlWriter"/> class.
    /// </summary>
    /// <param name="textWriter">The TextWriter object to work with.</param>
    public FileCabinetRecordXmlWriter(TextWriter textWriter)
    {
        XmlWriterSettings settings = new XmlWriterSettings();
        settings.Indent = true;

        XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings);

        this.xmlSerializer = new XmlSerializer(typeof(XmlFileCabinetList));
        this.xmlWriter = xmlWriter;
    }

    /// <summary>
    /// Wtites the record into the file.
    /// </summary>
    /// <param name="records">The record to wtire.</param>
    public void Write(IReadOnlyCollection<FileCabinetRecord> records)
    {
        var xmlRecords = new List<XmlFileCabinetRecord>(records.Count);

        foreach (var record in records)
        {
            xmlRecords.Add(new XmlFileCabinetRecord(record));
        }

        var xmlRecordsObject = new XmlFileCabinetList(xmlRecords);

        this.xmlSerializer.Serialize(this.xmlWriter, xmlRecordsObject);
    }

    /// <summary>
    /// Closes the current writer.
    /// </summary>
    public void Close()
    {
        this.xmlWriter.Close();
    }
}