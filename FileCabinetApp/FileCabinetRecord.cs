#pragma warning disable CS8618

namespace FileCabinetApp;

public class FileCabinetRecord
{
    public int Id { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }

    public DateTime DateOfBirth { get; set; }

    public decimal Balance { get; set; }

    public char FavChar { get; set; }

    public short WorkExperience { get; set; }
}