#pragma warning disable CS8618

using System.Xml.Serialization;

namespace FileCabinetApp;

/// <summary>
/// The class describing the record unit.
/// </summary>
public class FileCabinetRecord
{
    /// <summary>
    /// Gets or sets the unique identifier of the record.
    /// </summary>
    /// <value>
    /// The unique Int32 number.
    /// </value>
    public int Id { get; set; }

    /// <summary>
    /// Gets or sets the first name of the record's owner.
    /// </summary>
    /// <value>
    /// String.
    /// </value>
    public string FirstName { get; set; }

    /// <summary>
    /// Gets or sets the last name of the record's owner.
    /// </summary>
    /// <value>
    /// String.
    /// </value>
    public string LastName { get; set; }

    /// <summary>
    /// Gets or sets the the record's owner's date of birth.
    /// </summary>
    /// <value>
    /// DateTime.
    /// </value>
    public DateTime DateOfBirth { get; set; }

    /// <summary>
    /// Gets or sets the the record's owner's balance.
    /// </summary>
    /// <value>
    /// Decimal.
    /// </value>
    public decimal Balance { get; set; }

    /// <summary>
    /// Gets or sets the the record's owner's favorite letter.
    /// </summary>
    /// <value>
    /// Char.
    /// </value>
    public char FavLetter { get; set; }

    /// <summary>
    /// Gets or sets the the record's owner's work experience.
    /// </summary>
    /// <value>
    /// Short (Int16).
    /// </value>
    public short WorkExperience { get; set; }
}