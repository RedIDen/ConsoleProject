using System.Xml.Serialization;

namespace FileCabinetApp;

public class XmlFileCabinetRecordName
{
    [XmlAttribute("first")]
    public string FirstName { get; set; }

    [XmlAttribute("last")]
    public string LastName { get; set; }

    public XmlFileCabinetRecordName(string firstName, string lastName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    public XmlFileCabinetRecordName()
    {

    }
}