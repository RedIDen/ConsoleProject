using System.Globalization;
using System.Xml.Serialization;

namespace FileCabinetApp;

#pragma warning disable CS8618

/// <summary>
/// The class for file cabinet record's serialization.
/// </summary>
public class XmlFileCabinetRecord
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFileCabinetRecord"/> class.
    /// </summary>
    /// <param name="id">Id.</param>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <param name="dateOfBirth">Date of birth.</param>
    /// <param name="balance">Balance.</param>
    /// <param name="favLetter">Favorite letter.</param>
    /// <param name="workExperience">Work experience.</param>
    public XmlFileCabinetRecord(int id, string firstName, string lastName, DateTime dateOfBirth, decimal balance, char favLetter, short workExperience)
    {
        this.Id = id;
        this.Name = new XmlFileCabinetRecordName(firstName, lastName);
        this.DateOfBirth = dateOfBirth.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        this.Balance = balance;
        this.FavLetter = favLetter;
        this.WorkExperience = workExperience;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFileCabinetRecord"/> class.
    /// </summary>
    /// <param name="record">Record.</param>
    public XmlFileCabinetRecord(FileCabinetRecord record)
    {
        this.Id = record.Id;
        this.Name = new XmlFileCabinetRecordName(record.FirstName, record.LastName);
        this.DateOfBirth = record.DateOfBirth.ToString("MM/dd/yyyy", CultureInfo.CreateSpecificCulture("en-US"));
        this.Balance = record.Balance;
        this.FavLetter = record.FavLetter;
        this.WorkExperience = record.WorkExperience;
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFileCabinetRecord"/> class.
    /// </summary>
    public XmlFileCabinetRecord()
    {
        this.Name = new XmlFileCabinetRecordName("a", "b");
    }

    /// <summary>
    /// Gets or sets the unique identifier of the record.
    /// </summary>
    /// <value>
    /// The unique Int32 number.
    /// </value>
    [XmlAttribute("id")]
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the Name object containing first and last names.
    /// </summary>
    /// <value>
    /// The Name object.
    /// </value>
    [XmlElement("name")]
    public XmlFileCabinetRecordName Name { get; set; }

    /// <summary>
    /// Gets or sets the the record's owner's date of birth.
    /// </summary>
    /// <value>
    /// DateTime.
    /// </value>
    [XmlElement("dateOfBirth")]
    public string DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the the record's owner's balance.
    /// </summary>
    /// <value>
    /// Decimal.
    /// </value>
    [XmlElement("balance")]
    public decimal Balance { get; set; }

    /// <summary>
    /// Gets or sets the the record's owner's favorite letter.
    /// </summary>
    /// <value>
    /// Char.
    /// </value>
    [XmlElement("favoriteLetter")]
    public char FavLetter { get; set; }

    /// <summary>
    /// Gets or sets the the record's owner's work experience.
    /// </summary>
    /// <value>
    /// Short (Int16).
    /// </value>
    [XmlElement("workExperience")]
    public short WorkExperience { get; set; }
}