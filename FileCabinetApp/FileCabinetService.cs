using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileCabinetApp;

public class FileCabinetService
{
    private readonly List<FileCabinetRecord> list = new List<FileCabinetRecord>();

    public int CreateRecord(string firstName, string lastName, DateTime dateOfBirth, short workExperience, decimal balance, char favChar)
    {
        var record = new FileCabinetRecord
        {
            Id = this.list.Count + 1,
            FirstName = firstName,
            LastName = lastName,
            DateOfBirth = dateOfBirth,
            WorkExperience = workExperience,
            Balance = balance,
            FavChar = favChar,
        };

        this.list.Add(record);

        return record.Id;
    }

    public FileCabinetRecord[] GetRecords() => this.list.ToArray();

    public int GetStat() => this.list.Count;
}