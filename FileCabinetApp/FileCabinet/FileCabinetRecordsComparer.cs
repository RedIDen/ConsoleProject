#pragma warning disable CS8602

namespace FileCabinetApp.FileCabinet;

/// <summary>
/// The file cabinet record comparer.
/// </summary>
internal class FileCabinetRecordsComparer : IEqualityComparer<FileCabinetRecord>
{
    /// <summary>
    /// Compares two file cabinet records.
    /// </summary>
    /// <param name="x">First record.</param>
    /// <param name="y">Second record.</param>
    /// <returns>If records equal.</returns>
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

    /// <summary>
    /// Gets the hash code of record.
    /// </summary>
    /// <param name="obj">Record.</param>
    /// <returns>Hash code.</returns>
    public int GetHashCode(FileCabinetRecord obj)
    {
        return obj.Id.GetHashCode() & obj.FirstName.GetHashCode(StringComparison.InvariantCultureIgnoreCase);
    }
}