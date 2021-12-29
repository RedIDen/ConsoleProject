using System.Xml.Serialization;

namespace FileCabinetApp;

[XmlRoot(ElementName = "records", IsNullable = false)]
public class XmlFileCabinetList
{
    [XmlElement("record")]
    public List<XmlFileCabinetRecord> List { get; set; }

    public XmlFileCabinetList(List<XmlFileCabinetRecord> list)
    {
        this.List = list;
    }

    public XmlFileCabinetList()
    {
        this.List = new List<XmlFileCabinetRecord>();
    }
}