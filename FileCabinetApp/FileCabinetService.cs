using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8602

namespace FileCabinetApp;

/// <summary>
/// The class including the list of the recors and the methods to interact with this list.
/// </summary>
public class FileCabinetService
{
    private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
    private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
    private readonly Dictionary<string, List<FileCabinetRecord>> lastNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();
    private readonly Dictionary<DateTime, List<FileCabinetRecord>> dateOfBirthDictionary = new Dictionary<DateTime, List<FileCabinetRecord>>();

    /// <summary>
    /// Creates new record, if there's a correct data, and adds it to the list.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <param name="dateOfBirth">Date of birth.</param>
    /// <param name="workExperience">Work experience.</param>
    /// <param name="balance">Balance.</param>
    /// <param name="favLetter">Favorite letter.</param>
    /// <returns>Returns the id of the new record.</returns>
    public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short workExperience, decimal balance, char favLetter)
    {
        CheckNames(firstName, lastName);
        CheckDateOfBirth(dateOfBirth);
        CheckWorkExperience(workExperience, dateOfBirth);
        CheckBalance(balance);
        CheckFavLetter(favLetter);

        var record = new FileCabinetRecord
        {
            Id = this.list.Count + 1,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            WorkExperience = workExperience,
            Balance = balance,
            FavChar = favLetter,
        };

        this.AddToDictionaries(record);

        this.list.Add(record);

        return record.Id;
    }

    /// <summary>
    /// Edits the existing record.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <param name="dateOfBirth">Date of birth.</param>
    /// <param name="workExperience">Work experience.</param>
    /// <param name="balance">Balance.</param>
    /// <param name="favLetter">Favorite letter.</param>
    /// <param name="index">Index.</param>
    public void EditRecord(string firstName, string lastName, DateTime dateOfBirth, short workExperience, decimal balance, char favLetter, int index)
    {
        CheckNames(firstName, lastName);
        CheckDateOfBirth(dateOfBirth);
        CheckWorkExperience(workExperience, dateOfBirth);
        CheckBalance(balance);
        CheckFavLetter(favLetter);

        var record = this.list[index];

        this.DeleteFromDictionaries(record);

        record.FirstName = firstName;
        record.LastName = lastName;
        record.DateOfBirth = dateOfBirth;
        record.WorkExperience = workExperience;
        record.Balance = balance;
        record.FavChar = favLetter;

        this.AddToDictionaries(record);
    }

    /// <summary>
    /// Returnd the index of the record with the recieved id.
    /// </summary>
    /// <param name="id">The record's id.</param>
    /// <returns>The index in the list of rhe record with the recieved id.</returns>
    public int FindRecordIndexById(int id) => this.list.FindIndex(e => e.Id == id);

    /// <summary>
    /// Returns the list of all records.
    /// </summary>
    /// <returns>The list of all records.</returns>
    public FileCabinetRecord[] GetRecords() => this.list.ToArray();

    /// <summary>
    /// Returns the stats (the number of records in the list).
    /// </summary>
    /// <returns>The number of records in the list.</returns>
    public int GetStat() => this.list.Count;

    /// <summary>
    /// Returns the list of the records with recieved first name.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <returns>The list of the records with recieved first name.</returns>
    public FileCabinetRecord[] FindByFirstName(string firstName) =>
        (this.firstNameDictionary.GetValueOrDefault(firstName.ToLower()) ?? new List<FileCabinetRecord>()).ToArray();

    /// <summary>
    /// Returns the list of the records with recieved last name.
    /// </summary>
    /// <param name="lastName">Last name.</param>
    /// <returns>The list of the records with recieved last name.</returns>
    public FileCabinetRecord[] FindByLastName(string lastName) =>
        (this.lastNameDictionary.GetValueOrDefault(lastName.ToLower()) ?? new List<FileCabinetRecord>()).ToArray();

    /// <summary>
    /// Returns the list of the records with recieved date of birth.
    /// </summary>
    /// <param name="date">Date of birth.</param>
    /// <returns>The list of the records with recieved date of birth.</returns>
    public FileCabinetRecord[] FindByDateOfBirth(string date)
    {
        DateTime dateOfBirth = DateTime.Parse(
            date,
            CultureInfo.CreateSpecificCulture("en-US"),
            DateTimeStyles.None);

        return (this.dateOfBirthDictionary.GetValueOrDefault(dateOfBirth) ?? new List<FileCabinetRecord>()).ToArray();
    }

    /// <summary>
    /// Checks if the first name and the last name are correct.
    /// </summary>
    /// <param name="firstName">First name.</param>
    /// <param name="lastName">Last name.</param>
    /// <exception cref="ArgumentNullException">Thrown if at least one of the names if null.</exception>
    /// <exception cref="ArgumentException">Thrown if at least one of the names consists of only whitespaces
    /// or has length less than 2 or more than 60.</exception>
    private static void CheckNames(string firstName, string lastName)
    {
        if (firstName is null)
        {
            throw new ArgumentNullException(nameof(firstName));
        }

        if (lastName is null)
        {
            throw new ArgumentNullException(nameof(lastName));
        }

        CheckNamesLength(firstName, "First name");
        CheckNamesLength(lastName, "Last name");

        CheckNameForSpaces(firstName, "First name");
        CheckNameForSpaces(lastName, "Last name");

        void CheckNamesLength(string a, string nameOfString)
        {
            if (a.Length < 2)
            {
                throw new ArgumentException($"{nameOfString} is too short.");
            }

            if (a.Length > 60)
            {
                throw new ArgumentException($"{nameOfString} is too long.");
            }
        }

        void CheckNameForSpaces(string a, string nameOfString)
        {
            if (a.Trim().Length == 0)
            {
                throw new ArgumentException($"{nameOfString} can't consist of only whitespaces.");
            }
        }
    }

    /// <summary>
    /// Checks if the date of birth is correct.
    /// </summary>
    /// <param name="date">Date of birth.</param>
    /// <exception cref="ArgumentException">Thrown when the date is less than 1950-Jan-1 or more than today's date.</exception>
    private static void CheckDateOfBirth(DateTime date)
    {
        if (date < new DateTime(1950, 1, 1) || date > DateTime.Now)
        {
            throw new ArgumentException("Incorrect date.");
        }
    }

    /// <summary>
    /// Checks if the work experience is correct.
    /// </summary>
    /// <param name="workExperience">Work experience.</param>
    /// <param name="dateOfBirth">Date of birth.</param>
    /// <exception cref="ArgumentException">Thrown when the work experience is less than 0 or more than person's age - 16 years.</exception>
    private static void CheckWorkExperience(short workExperience, DateTime dateOfBirth)
    {
        int age = DateTime.Now.Subtract(dateOfBirth).Days / 365;
        if (workExperience < 0 || workExperience > age - 16)
        {
            throw new ArgumentException("Incorrect work experisnce.");
        }
    }

    /// <summary>
    /// Checks if the balance is correct.
    /// </summary>
    /// <param name="balance">Balance.</param>
    /// <exception cref="ArgumentException">Thrown when the balance is less than 0.</exception>
    private static void CheckBalance(decimal balance)
    {
        if (balance < 0)
        {
            throw new ArgumentException("Incorrect balance.");
        }
    }

    /// <summary>
    /// Checks if the favorite letter is correct.
    /// </summary>
    /// <param name="favLetter">Favorite letter.</param>
    /// <exception cref="ArgumentException">Thrown then the char is not a letter.</exception>
    private static void CheckFavLetter(char favLetter)
    {
        if (!char.IsLetter(favLetter))
        {
            throw new ArgumentException("The char is not a letter.");
        }
    }

    /// <summary>
    /// Adds new record to dicionaries.
    /// </summary>
    /// <param name="record">The record to add.</param>
    private void AddToDictionaries(FileCabinetRecord record)
    {
        string lowerFirstName = record.FirstName.ToLower();

        if (this.firstNameDictionary.ContainsKey(lowerFirstName))
        {
            this.firstNameDictionary.GetValueOrDefault(lowerFirstName).Add(record);
        }
        else
        {
            var list = new List<FileCabinetRecord>();
            list.Add(record);
            this.firstNameDictionary.Add(lowerFirstName, list);
        }

        string lowerLastName = record.LastName.ToLower();

        if (this.lastNameDictionary.ContainsKey(lowerLastName))
        {
            this.lastNameDictionary.GetValueOrDefault(lowerLastName).Add(record);
        }
        else
        {
            var list = new List<FileCabinetRecord>();
            list.Add(record);
            this.lastNameDictionary.Add(lowerLastName, list);
        }

        if (this.dateOfBirthDictionary.ContainsKey(record.DateOfBirth))
        {
            this.dateOfBirthDictionary.GetValueOrDefault(record.DateOfBirth).Add(record);
        }
        else
        {
            var list = new List<FileCabinetRecord>();
            list.Add(record);
            this.dateOfBirthDictionary.Add(record.DateOfBirth, list);
        }
    }

    /// <summary>
    /// Deletes the record to dicionaries.
    /// </summary>
    /// <param name="record">The record to delete.</param>
    private void DeleteFromDictionaries(FileCabinetRecord record)
    {
        string lowerFirstName = record.FirstName.ToLower();
        this.firstNameDictionary.GetValueOrDefault(lowerFirstName).Remove(record);
        this.firstNameDictionary.Remove(lowerFirstName);

        string lowerLastName = record.LastName.ToLower();
        this.lastNameDictionary.GetValueOrDefault(lowerLastName).Remove(record);
        this.lastNameDictionary.Remove(lowerLastName);

        this.dateOfBirthDictionary.GetValueOrDefault(record.DateOfBirth).Remove(record);
        this.dateOfBirthDictionary.Remove(record.DateOfBirth);
    }
}