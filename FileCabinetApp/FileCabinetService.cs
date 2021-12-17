using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#pragma warning disable CS8602

namespace FileCabinetApp;

public class FileCabinetService
{
    private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();
    private readonly Dictionary<string, List<FileCabinetRecord>> firstNameDictionary = new Dictionary<string, List<FileCabinetRecord>>();

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

    public void EditRecord(string firstName, string lastName, DateTime dateOfBirth, short workExperience, decimal balance, char favLetter, int index)
    {
        CheckNames(firstName, lastName);
        CheckDateOfBirth(dateOfBirth);
        CheckWorkExperience(workExperience, dateOfBirth);
        CheckBalance(balance);
        CheckFavLetter(favLetter);

        var record = this.list[index];

        this.DeleteFromDictionary(record);

        record.FirstName = firstName;
        record.LastName = lastName;
        record.DateOfBirth = dateOfBirth;
        record.WorkExperience = workExperience;
        record.Balance = balance;
        record.FavChar = favLetter;

        this.AddToDictionaries(record);
    }

    public int FindRecordIndexById(int id) => this.list.FindIndex(e => e.Id == id);

    public FileCabinetRecord[] GetRecords() => this.list.ToArray();

    public int GetStat() => this.list.Count;

    public FileCabinetRecord[] FindByFirstName(string firstName) =>
        (this.firstNameDictionary.GetValueOrDefault(firstName.ToLower()) ?? new List<FileCabinetRecord>()).ToArray();

    public FileCabinetRecord[] FindByLastName(string lastName)
    {
        List<FileCabinetRecord> searchResult = new List<FileCabinetRecord>();
        foreach (var record in this.list)
        {
            if (record.LastName.Contains(lastName, StringComparison.InvariantCultureIgnoreCase))
            {
                searchResult.Add(record);
            }
        }

        return searchResult.ToArray();
    }

    public FileCabinetRecord[] FindByDateOfBirth(string date)
    {
        DateTime dateOfBirth = DateTime.Parse(
            date,
            CultureInfo.CreateSpecificCulture("en-US"),
            DateTimeStyles.None);

        List<FileCabinetRecord> searchResult = new List<FileCabinetRecord>();
        foreach (var record in this.list)
        {
            if (record.DateOfBirth.Equals(dateOfBirth))
            {
                searchResult.Add(record);
            }
        }

        return searchResult.ToArray();
    }

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

    private static void CheckDateOfBirth(DateTime date)
    {
        if (date < new DateTime(1950, 1, 1) || date > DateTime.Now)
        {
            throw new ArgumentException("Incorrect date.");
        }
    }

    private static void CheckWorkExperience(short workExperience, DateTime dateOfBirth)
    {
        int age = DateTime.Now.Subtract(dateOfBirth).Days / 365;
        if (workExperience < 0 || workExperience > age - 16)
        {
            throw new ArgumentException("Incorrect work experisnce.");
        }
    }

    private static void CheckBalance(decimal balance)
    {
        if (balance < 0)
        {
            throw new ArgumentException("Incorrect balance.");
        }
    }

    private static void CheckFavLetter(char favLetter)
    {
        if (!char.IsLetter(favLetter))
        {
            throw new ArgumentException("The char is not a letter.");
        }
    }

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
    }

    private void DeleteFromDictionary(FileCabinetRecord record) =>
        this.firstNameDictionary.GetValueOrDefault(record.FirstName.ToLower()).Remove(record);
}