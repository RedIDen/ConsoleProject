using System.Xml.Serialization;

namespace FileCabinetApp;

#pragma warning disable CS8618

/// <summary>
/// The class for file cabinet record's name serialization.
/// </summary>
public class XmlFileCabinetRecordName
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFileCabinetRecordName"/> class.
    /// </summary>
    public XmlFileCabinetRecordName()
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XmlFileCabinetRecordName"/> class.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    public XmlFileCabinetRecordName(string firstName, string lastName)
    {
        this.FirstName = firstName;
        this.LastName = lastName;
    }

    /// <summary>
    /// Gets or sets the first name of the record's owner.
    /// </summary>
    /// <value>
    /// String.
    /// </value>
    [XmlAttribute("first")]
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the record's owner.
    /// </summary>
    /// <value>
    /// String.
    /// </value>
    [XmlAttribute("last")]
    public string LastName { get; set; }
}