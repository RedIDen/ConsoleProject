namespace FileCabinetApp;

public class FileCabinetRecordsComparer : IEqualityComparer<FileCabinetRecord>
{
    public bool Equals(FileCabinetRecord? x, FileCabinetRecord? y)
    {
        return x.Id == y.Id &&
            x.FirstName == y.FirstName &&
            x.LastName == y.LastName &&
            x.DateOfBirth == y.DateOfBirth &&
            x.Balance == y.Balance &&
            x.WorkExperience == y.WorkExperience &&
            x.FavLetter == y.FavLetter;
    }

    public int GetHashCode(FileCabinetRecord obj)
    {
        return obj.Id.GetHashCode() & obj.FirstName.GetHashCode();
    }
}