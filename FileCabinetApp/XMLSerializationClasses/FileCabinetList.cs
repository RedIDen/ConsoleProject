using System.Xml.Serialization;

namespace FileCabinetApp;

/// <summary>
/// The class for file cabinet records' list serialization.
/// </summary>
[XmlRoot(ElementName = "records", IsNullable = false)]
public class XmlFileCabinetList
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFileCabinetList"/> class.
    /// </summary>
    /// <param name="list">The list of records.</param>
    public XmlFileCabinetList(List<XmlFileCabinetRecord> list)
    {
        this.List = list;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFileCabinetList"/> class.
    /// </summary>
    public XmlFileCabinetList()
    {
        this.List = new List<XmlFileCabinetRecord>();
    }

    /// <summary>
    /// Gets or sets list of records to serialize.
    /// </summary>
    /// <value>
    /// The list object.
    /// </value>
    [XmlElement("record")]
    public List<XmlFileCabinetRecord> List { get; set; }
}