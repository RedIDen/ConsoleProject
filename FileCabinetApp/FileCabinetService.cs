using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp;

public class FileCabinetService
{
    private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

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

        this.list.Add(record);

        return record.Id;
    }

    public void EditRecord(int id, string firstName, string lastName, DateTime dateOfBirth, short workExperience, decimal balance, char favLetter, int index)
    {
        CheckNames(firstName, lastName);
        CheckDateOfBirth(dateOfBirth);
        CheckWorkExperience(workExperience, dateOfBirth);
        CheckBalance(balance);
        CheckFavLetter(favLetter);

        this.list[index] = new FileCabinetRecord
        {
            Id = id,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            WorkExperience = workExperience,
            Balance = balance,
            FavChar = favLetter,
        };
    }

    public int FindRecordIndexById(int id) => this.list.FindIndex(e => e.Id == id);

    public FileCabinetRecord[] GetRecords() => this.list.ToArray();

    public int GetStat() => this.list.Count;

    public FileCabinetRecord[] FindByFirstName(string firstName)
    {
        List<FileCabinetRecord> searchResult = new List<FileCabinetRecord>();
        foreach (var record in this.list)
        {
            if (record.FirstName.Contains(firstName, StringComparison.InvariantCultureIgnoreCase))
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
}