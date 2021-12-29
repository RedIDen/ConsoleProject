using System.Globalization;
using System.Xml.Serialization;

namespace FileCabinetApp;

public class XmlFileCabinetRecord
{
    [XmlAttribute("id")]
    public int Id { get; set; }

    [XmlElement("name")]
    public XmlFileCabinetRecordName Name { get; set; }

    [XmlElement("dateOfBirth")]
    public string DateOfBirth { get; set; }

    [XmlElement("balance")]
    public decimal Balance { get; set; }

    [XmlElement("favoriteLetter")]
    public char FavLetter { get; set; }

    [XmlElement("workExperience")]
    public short WorkExperience { get; set; }

    public XmlFileCabinetRecord(int id, string firstName, string lastName, DateTime dateOfBirth, decimal balance, char favLetter, short workExperience)
    {
        this.Id = id;
        this.Name = new XmlFileCabinetRecordName(firstName, lastName);
        this.DateOfBirth = dateOfBirth.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        this.Balance = balance;
        this.FavLetter = favLetter;
        this.WorkExperience = workExperience;
    }

    public XmlFileCabinetRecord(FileCabinetRecord record)
    {
        this.Id = record.Id;
        this.Name = new XmlFileCabinetRecordName(record.FirstName, record.LastName);
        this.DateOfBirth = record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        this.Balance = record.Balance;
        this.FavLetter = record.FavLetter;
        this.WorkExperience = record.WorkExperience;
    }

    public XmlFileCabinetRecord()
    {
        this.Name = new XmlFileCabinetRecordName("a", "b");
    }
}