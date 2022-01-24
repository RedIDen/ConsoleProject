namespace FileCabinetApp;

/// <summary>
/// The records enumerator for filesystem service.
/// </summary>
/// <typeparam name="T">FileCabinetRecord class.</typeparam>
internal class FilesystemEnumerator<T> : IEnumerable<T>
{
    private readonly IEnumerable<long> list;
    private readonly Func<T> getRecord;
    private readonly Action<long> setReaderPosition;

    /// <summary>
    /// Initializes a new instance of the <see cref="FilesystemEnumerator{T}"/> class.
    /// </summary>
    /// <param name="list">List of records' offsets.</param>
    /// <param name="getRecord">Delegate getting record.</param>
    /// <param name="setReaderPosition">Delegate setting reader position.</param>
    public FilesystemEnumerator(IEnumerable<long> list, Func<T> getRecord, Action<long> setReaderPosition)
    {
        this.list = list;
        this.getRecord = getRecord;
        this.setReaderPosition = setReaderPosition;
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    public IEnumerator<T> GetEnumerator()
    {
        foreach (var position in this.list)
        {
            this.setReaderPosition(position);
            yield return this.getRecord();
        }
    }

    /// <summary>
    /// Gets the enumerator.
    /// </summary>
    /// <returns>The enumerator.</returns>
    IEnumerator IEnumerable.GetEnumerator()
    {
        return this.GetEnumerator();
    }
}
